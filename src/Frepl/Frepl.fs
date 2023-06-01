namespace Frepl

open System
open System.Collections

open Avalonia
open Avalonia.Data
open Avalonia.Controls
open Avalonia.Controls.Templates
open Avalonia.Input
open Avalonia.Media

open FSharp.Control.Reactive
open FSharp.Compiler.Interactive.Shell

open NXUI.Extensions
open type NXUI.Builders

open Frepl
open Frepl.Views
open Frepl.AppEnv
open Frepl.FsiEnv


module Frepl =

  let evaluationResults (evaluation: IObservable<EvaluationResult>) =
    let diagnostics =
      evaluation
      |> Observable.map(fun value ->
        value.Diagnostics |> Array.map(fun value -> value.Message) :> IEnumerable
      )

    let errors =
      evaluation
      |> Observable.choose(fun value ->
        match value.EvalResult with
        | SuccessfulEvaluation -> None
        | BoundResult(value) -> None
        | ErrorResult(error) -> Some(error.Message)
      )

    let text =
      evaluation
      |> Observable.choose(fun value ->
        match value.EvalResult with
        | SuccessfulEvaluation -> None
        | ErrorResult _ -> None
        | BoundResult(value) -> Some(value.ReflectionValue.ToString())
      )


    DockPanel()
      .children(
        StackPanel()
          .DockLeft()
          .spacing(4.)
          .children(Label().content("Errors"), TextBlock().text(errors, mode = BindingMode.OneWay)),
        StackPanel()
          .DockBottom()
          .HorizontalAlignmentStretch()
          .spacing(4.)
          .children(
            Label().content("Diagnostics"),
            ItemsControl()
              .itemTemplate(
                new FuncDataTemplate<string>(
                  (fun (diagnostic) _ -> TextBlock().padding(4.).text(diagnostic))
                )
              )
              .itemsSource(diagnostics, mode = BindingMode.OneWay)
          ),
        StackPanel()
          .DockBottom()
          .HorizontalAlignmentStretch()
          .spacing(4.)
          .children(
            Label().content("Evaluation Result"),
            TextBlock().text(text, mode = BindingMode.OneWay)
          )
      )


  let boundValuesList (values: IObservable<FsiBoundValue list>) =
    let template =
      new FuncDataTemplate<FsiBoundValue>(
        (fun (boundValue) _ ->
          TextBlock()
            .padding(2.)
            .text($"{boundValue.Name} = {boundValue.Value.ReflectionValue}")
        )
      )

    let values = values |> Observable.map(fun value -> value :> Collections.IEnumerable)

    StackPanel()
      .children(
        ItemsControl()
          .itemTemplate(template)
          .itemsSource(values, mode = BindingMode.OneWay)
      )


  let View (env: AppEnv & FsiEnv fsi) =

    let textChanged = Subject<string>.broadcast

    let evaluation =
      textChanged
      |> Observable.map(fun value -> fsi.EvaluateInteraction(value))
      |> Observable.tap(fun _ -> fsi.ResetEvaluationToken())
      |> Observable.tap(fun value -> printfn "%A" value)

    let inputBox = Editor.View env

    let evaluationResults = evaluationResults evaluation

    let boundValuesList = boundValuesList fsi.BoundValues

    let panels =

      DockPanel()
        .HorizontalAlignmentStretch()
        .VerticalAlignmentStretch()
        .children(evaluationResults.DockTop(), boundValuesList.DockRight())

    inputBox.TextChanged
    |> Observable.add(fun value -> textChanged.OnNext(inputBox.Text))

    StackPanel()
      .HorizontalAlignmentStretch()
      .VerticalAlignmentBottom()
      .children(panels, inputBox)


  let Window (env: AppEnv) : unit -> Window =
    fun () ->
      let font =

        try
          FontFamily.Parse("Fira Code,Cascadia Code,Consolas,Monospace") |> ValueSome
        with :? ArgumentException ->
          ValueNone


      let window =
        Window()
          .fontFamily(font |> ValueOption.defaultValue FontFamily.Default)
          .content(View env)

#if DEBUG
      window.AttachDevTools()
#endif
      window