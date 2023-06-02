open System

open Avalonia
open Avalonia.Themes.Fluent

open NXUI.Extensions

open Frepl
open Frepl.FsiEnv
open Frepl.EditorEnv
open Frepl.AppEnv
open Avalonia.Controls.ApplicationLifetimes

type FreplApp() =
  inherit Application()

  override this.Initialize() =
    base.Initialize()

    this.Styles.Add(FluentTheme())
    let style = Markup.Xaml.Styling.StyleInclude(baseUri = null)
    style.Source <- Uri("avares://AvaloniaEdit/Themes/Fluent/AvaloniaEdit.xaml")

    this.Styles.Add style

  override this.OnFrameworkInitializationCompleted() =
    match this.ApplicationLifetime with
    | :? IClassicDesktopStyleApplicationLifetime as desktop ->
      let mainWindow =
        AppEnv(FsiEnv.DefaultEnv, EditorEnv.DefaultEnv this.ActualThemeVariant)
        |> Frepl.Window

      desktop.MainWindow <- mainWindow
    | _ -> ()


[<EntryPoint>]
let main argv =
  AppBuilder
    .Configure<FreplApp>()
    .UsePlatformDetect()
    .UseFluentTheme(Styling.ThemeVariant.Dark)
    .WithApplicationName("Frepl")
    .StartWithClassicDesktopLifetime(argv)