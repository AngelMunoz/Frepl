namespace Frepl.AppEnv

open Frepl.EditorEnv
open Frepl.FsiEnv

[<Struct>]
type AppEnv(fsiEnv: FsiEnv, editorEnv: EditorEnv) =

  interface FsiEnv with
    member _.BoundValues = fsiEnv.BoundValues

    member _.EvaluateExpresion(arg1: string) = fsiEnv.EvaluateExpresion arg1

    member _.EvaluateInteraction(arg1: string) = fsiEnv.EvaluateInteraction arg1

    member _.EvaluationToken = fsiEnv.EvaluationToken
    member _.ResetEvaluationToken() = fsiEnv.ResetEvaluationToken()
    member _.Session = fsiEnv.Session
    member _.SessionStderr = fsiEnv.SessionStderr
    member _.SessionStdout = fsiEnv.SessionStdout

  interface EditorEnv with
    member _.EditorOptions = editorEnv.EditorOptions

    member _.RegistryOptions = editorEnv.RegistryOptions