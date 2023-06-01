open System

open Avalonia
open Avalonia.Themes.Fluent

open NXUI.Extensions

open Frepl
open Frepl.FsiEnv
open Frepl.EditorEnv
open Frepl.AppEnv

type FreplApp() =
  inherit Application()

  override this.Initialize() =
    base.Initialize()

    this.Styles.Add(FluentTheme())
    let style = Markup.Xaml.Styling.StyleInclude(baseUri = null)
    style.Source <- Uri("avares://AvaloniaEdit/Themes/Fluent/AvaloniaEdit.xaml")

    this.Styles.Add style


[<EntryPoint>]
let main argv =

  let env = AppEnv(FsiEnv.DefaultEnv, EditorEnv.DefaultEnv)

  AppBuilder
    .Configure<FreplApp>()
    .UsePlatformDetect()
    .UseFluentTheme(Styling.ThemeVariant.Dark)
    .WithApplicationName("Frepl")
    .StartWithClassicDesktopLifetime(Frepl.Window(env), argv)