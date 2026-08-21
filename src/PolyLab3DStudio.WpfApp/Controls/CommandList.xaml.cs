using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace PolyLab3DStudio.Controls;

/// <summary>
/// A terminal command block rendered as one row per command. Each row carries its own
/// horizontal scroller (a long command scrolls without moving its neighbours), its own
/// copy button, and its own 복사됨 toast — so a learner copies exactly the one line
/// they are about to run. Dragging a selection inside a row copies the selection.
/// </summary>
public partial class CommandList : UserControl
{
    public static readonly DependencyProperty CodeProperty = DependencyProperty.Register(
        nameof(Code), typeof(string), typeof(CommandList), new PropertyMetadata(null, OnCodeChanged));

    private readonly Dictionary<TextBlock, DispatcherTimer> _toastTimers = [];

    public CommandList() => InitializeComponent();

    public string? Code
    {
        get => (string?)GetValue(CodeProperty);
        set => SetValue(CodeProperty, value);
    }

    private static void OnCodeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is CommandList list)
        {
            list.Rows.ItemsSource = CommandRow.Parse(e.NewValue as string);
        }
    }

    private void OnCopyClick(object sender, RoutedEventArgs e)
    {
        if (sender is FrameworkElement { DataContext: CommandRow row } element)
        {
            Copy(row.Text, "복사됨", element);
        }
    }

    private void OnCommandMouseUp(object sender, MouseButtonEventArgs e)
    {
        if (sender is TextBox { SelectionLength: > 0 } box)
        {
            Copy(box.SelectedText, "선택 복사됨", box);
        }
    }

    private void Copy(string? text, string message, FrameworkElement origin)
    {
        if (string.IsNullOrEmpty(text))
        {
            return;
        }

        TextBlock? toast = FindToast(origin);
        try
        {
            Clipboard.SetText(text);
        }
        catch (Exception e) when (e is System.Runtime.InteropServices.COMException or InvalidOperationException)
        {
            // Another process can hold the clipboard open (CLIPBRD_E_CANT_OPEN); a
            // failed copy must not take the lesson screen down with it.
            ShowToast(toast, "복사 실패");
            return;
        }

        ShowToast(toast, message);
    }

    /// <summary>Finds the toast belonging to the row the click came from.</summary>
    private static TextBlock? FindToast(FrameworkElement origin)
    {
        for (DependencyObject? node = origin; node is not null; node = VisualTreeHelper.GetParent(node))
        {
            if (node is Grid grid)
            {
                foreach (object child in grid.Children)
                {
                    if (child is TextBlock { Tag: "toast" } toast)
                    {
                        return toast;
                    }
                }
            }
        }

        return null;
    }

    private void ShowToast(TextBlock? toast, string message)
    {
        if (toast is null)
        {
            return;
        }

        toast.Text = message;
        toast.BeginAnimation(OpacityProperty, null);
        toast.Opacity = 1;

        if (!_toastTimers.TryGetValue(toast, out DispatcherTimer? timer))
        {
            timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(1400) };
            timer.Tick += (_, _) =>
            {
                timer.Stop();
                toast.BeginAnimation(OpacityProperty, new DoubleAnimation
                {
                    To = 0,
                    Duration = TimeSpan.FromMilliseconds(350),
                    FillBehavior = FillBehavior.HoldEnd,
                });
            };

            _toastTimers[toast] = timer;
        }

        timer.Stop();
        timer.Start();
    }
}
