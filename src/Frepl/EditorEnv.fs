namespace Frepl.EditorEnv

open AvaloniaEdit
open TextMateSharp.Grammars


type EditorEnv =

  abstract member EditorOptions: TextEditorOptions
  abstract member RegistryOptions: RegistryOptions


[<AutoOpen>]
module EditorPatterns =

  let inline (|EditorEnv|) (env: #EditorEnv) = (EditorEnv env :> EditorEnv)

[<RequireQualifiedAccess>]
module EditorEnv =
  open Avalonia.Styling

  let DefaultOptions =
    new TextEditorOptions(
      ConvertTabsToSpaces = true,
      ShowSpaces = true,
      ShowTabs = true,
      ShowEndOfLine = true
    )

  let DefaultRegistryOptions (currentTheme: ThemeVariant) =
    if currentTheme = ThemeVariant.Dark then
      RegistryOptions(ThemeName.DarkPlus)
    elif currentTheme = ThemeVariant.Light then
      RegistryOptions(ThemeName.LightPlus)
    else
      RegistryOptions(ThemeName.Monokai)

  let DefaultEnv (initialTheme: ThemeVariant) : EditorEnv =
    { new EditorEnv with
        member _.EditorOptions: TextEditorOptions = DefaultOptions
        member _.RegistryOptions: RegistryOptions = DefaultRegistryOptions initialTheme
    }