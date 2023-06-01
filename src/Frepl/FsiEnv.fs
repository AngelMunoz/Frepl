namespace Frepl.FsiEnv

open System
open System.IO
open System.Text
open System.Threading

open FSharp.Compiler.Diagnostics
open FSharp.Compiler.Interactive.Shell


type EvalResultKind =
  | SuccessfulEvaluation
  | BoundResult of FsiValue
  | ErrorResult of exn


type EvaluationResult = {
  EvalResult: EvalResultKind
  Diagnostics: FSharpDiagnostic array
}

type FsiEnv =

  abstract member SessionStdout: StringBuilder

  abstract member SessionStderr: StringBuilder

  abstract member Session: FsiEvaluationSession

  abstract member BoundValues: IObservable<FsiBoundValue list>

  abstract member EvaluateExpresion: string -> EvaluationResult

  abstract member EvaluateInteraction: string -> EvaluationResult

  abstract member EvaluationToken: CancellationToken with get

  abstract member ResetEvaluationToken: unit -> unit


[<AutoOpen>]
module Patterns =
  let inline (|FsiEnv|) (env: #FsiEnv) = (FsiEnv env :> FsiEnv)


[<RequireQualifiedAccess>]
module FsiEnv =

  let createFsiSession () =
    let config = FsiEvaluationSession.GetDefaultConfiguration()
    let stdout = StringBuilder()
    let stderr = StringBuilder()

    let stdoutWriter = new StringWriter(stdout)
    let stderrWriter = new StringWriter(stderr)

    let session =
      FsiEvaluationSession.Create(
        config,
        [| "fsi.exe"; "--nologo"; "--gui-"; "--noninteractive"; "--utf8output" |],
        new StringReader(""),
        stdoutWriter,
        stderrWriter
      )

    session, stdout, stderr

  let getBoundValues (session: FsiEvaluationSession) =
    session.ValueBound |> Observable.map(fun _ -> session.GetBoundValues())

  let evaluateExpression (session: FsiEvaluationSession) (expression: string) =
    let result, diagnostics = session.EvalExpressionNonThrowing(expression)

    match result with
    | Choice1Of2 result ->
      let result =
        result
        |> Option.map(fun value -> BoundResult value)
        |> Option.defaultValue SuccessfulEvaluation

      {
        EvalResult = result
        Diagnostics = diagnostics
      }
    | Choice2Of2 errors -> {
        EvalResult = ErrorResult errors
        Diagnostics = diagnostics
      }

  let evaluateInteraction
    (session: FsiEvaluationSession)
    (cancellationToken: CancellationToken)
    (expression: string)
    =
    let result, diagnostics =
      session.EvalInteractionNonThrowing(expression, cancellationToken = cancellationToken)

    match result with
    | Choice1Of2 result ->
      let result =
        result
        |> Option.map(fun value -> BoundResult value)
        |> Option.defaultValue SuccessfulEvaluation

      {
        EvalResult = result
        Diagnostics = diagnostics
      }
    | Choice2Of2 errors -> {
        EvalResult = ErrorResult errors
        Diagnostics = diagnostics
      }

  let DefaultEnv: FsiEnv =
    let mutable source = new CancellationTokenSource()

    let mutable session, stdout, stderr = createFsiSession()

    { new FsiEnv with
        member _.Session: FsiEvaluationSession = session
        member _.BoundValues: IObservable<FsiBoundValue list> = getBoundValues session

        member _.EvaluateExpresion(arg1: string) : EvaluationResult =
          let result = evaluateExpression session arg1

          result

        member this.EvaluateInteraction(arg1: string) : EvaluationResult =
          let result = evaluateInteraction session source.Token arg1
          result

        member _.EvaluationToken = source.Token

        member _.ResetEvaluationToken() : unit =
          try
            source.Cancel()
            source.Dispose()
          with ex ->
            eprintfn "%A" ex.Message

          source <- new CancellationTokenSource()

        member _.SessionStderr: StringBuilder = stdout

        member _.SessionStdout: StringBuilder = stderr
    }