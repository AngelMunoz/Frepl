[<AutoOpen>]
module Frepl.Extensions

open System
open Avalonia.Controls
open Avalonia.Layout

open NXUI.Extensions

open System.Runtime.CompilerServices
open Avalonia.Controls.Templates
open Avalonia.Data
open Avalonia.Controls.Primitives
open Avalonia.Media
open AvaloniaEdit
open System.Text
open AvaloniaEdit.Document
open AvaloniaEdit.Highlighting

module Observable =

  let tap (f: 'Type -> unit) (observable: IObservable<'Type>) : IObservable<'Type> =
    observable
    |> Observable.map(fun args ->
      f args
      args
    )

/// Temporary Type Extensions with different names than properties
/// to avoid having the F# compiler complain about the ambiguity
/// between the property and the extension method.
/// See https://github.com/fsharp/fslang-suggestions/issues/1039
/// We'll see if there's something we can do on the NXUI side to avoid this.

[<Extension>]
type TextEditorExtensions =
  [<Extension>]
  static member inline showLineNumbers<'Type when 'Type :> TextEditor>
    (
      editor: 'Type,
      showLineNumbers: bool
    ) : 'Type =
    editor.ShowLineNumbers <- showLineNumbers
    editor

  [<Extension>]
  static member inline options<'Type when 'Type :> TextEditor>
    (
      editor: 'Type,
      options: TextEditorOptions
    ) : 'Type =
    editor.Options <- options
    editor

  [<Extension>]
  static member inline encoding<'Type when 'Type :> TextEditor>
    (
      editor: 'Type,
      encoding: Encoding
    ) : 'Type =
    editor.Encoding <- encoding
    editor

  [<Extension>]
  static member inline document<'Type when 'Type :> TextEditor>
    (
      editor: 'Type,
      document: TextDocument
    ) : 'Type =
    editor.Document <- document
    editor

  [<Extension>]
  static member inline syntaxHighlighting<'Type when 'Type :> TextEditor>
    (
      editor: 'Type,
      syntaxHighlighting: IHighlightingDefinition
    ) : 'Type =
    editor.SyntaxHighlighting <- syntaxHighlighting
    editor

[<Extension>]
type NxuiFsTextBlockExtensions =


  [<Extension>]
  static member inline padding<'Type when 'Type :> TextBlock>
    (
      control: 'Type,
      padding: float
    ) : 'Type =
    TextBlockExtensions.Padding(control, padding)

  [<Extension>]
  static member inline text<'Type when 'Type :> TextBlock>(textbox: 'Type, text: string) : 'Type =

    TextBlockExtensions.Text(textbox, text)


  [<Extension>]
  static member inline text<'Type when 'Type :> TextBlock>
    (
      textbox: 'Type,
      text: Avalonia.Data.IBinding,
      ?mode: Avalonia.Data.BindingMode,
      ?priority: Avalonia.Data.BindingPriority
    ) : 'Type =

    TextBlockExtensions.Text(textbox, text, ?mode = mode, ?priority = priority)

  [<Extension>]
  static member inline text<'Type when 'Type :> TextBlock>
    (
      textbox: 'Type,
      observable: IObservable<string>,
      ?mode: Avalonia.Data.BindingMode,
      ?priority: Avalonia.Data.BindingPriority
    ) : 'Type =

    TextBlockExtensions.Text(textbox, observable, ?mode = mode, ?priority = priority)

[<Extension>]
type NxuiFsExtensions =

  [<Extension>]
  static member inline title<'Type when 'Type :> Window>(window: 'Type, title: string) : 'Type =
    WindowExtensions.Title(window, title)


  [<Extension>]
  static member inline acceptsReturn<'Type when 'Type :> TextBox>
    (
      textbox: 'Type,
      acceptsReturn: bool
    ) : 'Type =
    TextBoxExtensions.AcceptsReturn(textbox, acceptsReturn)

  [<Extension>]
  static member inline fontFamily<'Type when 'Type :> TemplatedControl>
    (
      textbox: 'Type,
      fontFamily: FontFamily
    ) : 'Type =
    TemplatedControlExtensions.FontFamily(textbox, fontFamily)

  [<Extension>]
  static member inline text<'Type when 'Type :> TextBox>(textbox: 'Type, text: string) : 'Type =

    TextBoxExtensions.Text(textbox, text)

  [<Extension>]
  static member inline padding<'Type when 'Type :> TemplatedControl>
    (
      control: 'Type,
      padding: float
    ) : 'Type =
    TemplatedControlExtensions.Padding(control, padding)

  [<Extension>]
  static member inline spacing<'Type when 'Type :> StackPanel>
    (
      control: 'Type,
      padding: float
    ) : 'Type =
    StackPanelExtensions.Spacing(control, padding)

  [<Extension>]
  static member inline text<'Type when 'Type :> TextBox>
    (
      textbox: 'Type,
      text: Avalonia.Data.IBinding,
      ?mode: Avalonia.Data.BindingMode,
      ?priority: Avalonia.Data.BindingPriority
    ) : 'Type =

    TextBoxExtensions.Text(textbox, text, ?mode = mode, ?priority = priority)

  [<Extension>]
  static member inline height<'Type when 'Type :> Layoutable>
    (
      layoutable: 'Type,
      size: float
    ) : 'Type =
    LayoutableExtensions.Height(layoutable, size)

  [<Extension>]
  static member inline width<'Type when 'Type :> Layoutable>
    (
      layoutable: 'Type,
      size: float
    ) : 'Type =
    LayoutableExtensions.Width(layoutable, size)

  [<Extension>]
  static member inline content<'Type when 'Type :> ContentControl>
    (
      control: 'Type,
      content: obj
    ) : 'Type =
    ContentControlExtensions.Content(control, content)

  [<Extension>]
  static member inline content<'Type when 'Type :> ContentControl>
    (
      control: 'Type,
      observable: IObservable<obj>,
      ?mode: Avalonia.Data.BindingMode,
      ?priority: Avalonia.Data.BindingPriority
    ) : 'Type =
    ContentControlExtensions.Content(control, observable, ?mode = mode, ?priority = priority)

  [<Extension>]
  static member inline children<'Type when 'Type :> Panel>
    (
      panel: 'Type,
      children: Control
    ) : 'Type =
    PanelExtensions.Children(panel, children)

  [<Extension>]
  static member inline children<'Type when 'Type :> Panel>
    (
      panel: 'Type,
      [<ParamArray>] children: Control array
    ) : 'Type =
    PanelExtensions.Children(panel, children)

  [<Extension>]
  static member inline itemTemplate<'Type when 'Type :> ItemsControl>
    (
      control: 'Type,
      template: IDataTemplate
    ) : 'Type =
    ItemsControlExtensions.ItemTemplate(control, template)

  [<Extension>]
  static member inline itemTemplate<'Type when 'Type :> ItemsControl>
    (
      control: 'Type,
      binding: IBinding,
      ?mode: BindingMode,
      ?priority: BindingPriority
    ) : 'Type =
    ItemsControlExtensions.ItemTemplate(control, binding, ?mode = mode, ?priority = priority)

  [<Extension>]
  static member inline itemTemplate<'Type when 'Type :> ItemsControl>
    (
      control: 'Type,
      observable: IObservable<IDataTemplate>,
      ?mode: BindingMode,
      ?priority: BindingPriority
    ) : 'Type =
    ItemsControlExtensions.ItemTemplate(control, observable, ?mode = mode, ?priority = priority)

  [<Extension>]
  static member inline itemsSource<'Type when 'Type :> ItemsControl>
    (
      control: 'Type,
      items: System.Collections.IEnumerable
    ) : 'Type =
    ItemsControlExtensions.ItemsSource(control, items)

  [<Extension>]
  static member inline itemsSource<'Type when 'Type :> ItemsControl>
    (
      control: 'Type,
      binding: IBinding,
      ?mode: BindingMode,
      ?priority: BindingPriority
    ) : 'Type =
    ItemsControlExtensions.ItemsSource(control, binding, ?mode = mode, ?priority = priority)

  [<Extension>]
  static member inline itemsSource<'Type when 'Type :> ItemsControl>
    (
      control: 'Type,
      observable: IObservable<System.Collections.IEnumerable>,
      ?mode: BindingMode,
      ?priority: BindingPriority
    ) : 'Type =
    ItemsControlExtensions.ItemsSource(control, observable, ?mode = mode, ?priority = priority)