using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace PolyLab3DStudio.Controls;

/// <summary>
/// Dark code/terminal block used by the reading modal: horizontally scrollable,
/// mouse-selectable, and copy-friendly. There are three ways to get the text out —
/// the 복사 button (whole block), dragging a selection (copies just the selection on
/// mouse-up), and Ctrl+C — and each one flashes a 복사됨 toast so it is obvious the
/// clipboard actually changed.
/// </summary>
public partial class CodeBlock : UserControl
{
    public static readonly DependencyProperty CodeProperty = DependencyProperty.Register(
        nameof(Code), typeof(string), typeof(CodeBlock), new PropertyMetadata(null));

    private DispatcherTimer? _toastTimer;

    public CodeBlock()
    {
        InitializeComponent();

        // Selection-drag copy: SelectionChanged fires continuously while dragging, so
        // wait for the button release and copy whatever ended up selected.
        CodeText.PreviewMouseLeftButtonUp += OnCodeMouseUp;
        CodeText.KeyUp += OnCodeKeyUp;
    }

    public string? Code
    {
        get => (string?)GetValue(CodeProperty);
        set => SetValue(CodeProperty, value);
    }

    private void OnCopyClick(object sender, RoutedEventArgs e) => Copy(Code, "복사됨");

    private void OnCodeMouseUp(object sender, MouseButtonEventArgs e)
    {
        if (CodeText.SelectionLength > 0)
        {
            Copy(CodeText.SelectedText, "선택 복사됨");
        }
    }

    private void OnCodeKeyUp(object sender, KeyEventArgs e)
    {
        // Ctrl+C is handled by the TextBox itself; only the toast is ours.
        if (e.Key == Key.C && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control
            && CodeText.SelectionLength > 0)
        {
            ShowToast("복사됨");
        }
    }

    private void Copy(string? text, string message)
    {
        if (string.IsNullOrEmpty(text))
        {
            return;
        }

        try
        {
            Clipboard.SetText(text);
        }
        catch (Exception e) when (e is System.Runtime.InteropServices.COMException or InvalidOperationException)
        {
            // Another process can hold the clipboard open (CLIPBRD_E_CANT_OPEN);
            // a failed copy must not take the lesson screen down with it.
            ShowToast("복사 실패");
            return;
        }

        ShowToast(message);
    }

    private void ShowToast(string message)
    {
        Toast.Text = message;
        Toast.BeginAnimation(OpacityProperty, null);
        Toast.Opacity = 1;

        _toastTimer ??= CreateToastTimer();
        _toastTimer.Stop();
        _toastTimer.Start();
    }

    private DispatcherTimer CreateToastTimer()
    {
        var timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(1400) };
        timer.Tick += (_, _) =>
        {
            timer.Stop();
            Toast.BeginAnimation(OpacityProperty, new DoubleAnimation
            {
                To = 0,
                Duration = TimeSpan.FromMilliseconds(350),
                FillBehavior = FillBehavior.HoldEnd,
            });
        };

        return timer;
    }
}
