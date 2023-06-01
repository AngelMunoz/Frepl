namespace Frepl.Views

open System.Text
open AvaloniaEdit
open AvaloniaEdit.TextMate
open TextMateSharp.Grammars
open NXUI.Extensions


open Frepl
open Frepl.EditorEnv


[<RequireQualifiedAccess>]
module Editor =

  let View (EditorEnv env) =

    let editor =
      TextEditor()
        .height(200.)
        .HorizontalAlignmentStretch()
        .showLineNumbers(true)
        .options(env.EditorOptions)
        .encoding(Encoding.UTF8)
        .syntaxHighlighting(Highlighting.HighlightingManager.Instance.GetDefinition("fsharp"))

    editor
      .InstallTextMate(env.RegistryOptions)
      .SetGrammar(env.RegistryOptions.GetScopeByLanguageId("fsharp"))

    editor