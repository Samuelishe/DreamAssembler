using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using DreamAssembler.App.ViewModels;

namespace DreamAssembler.App;

/// <summary>
/// Представляет главное окно приложения.
/// </summary>
public partial class MainWindow : Window
{
    private WindowState _windowStateBeforeReadingMode = WindowState.Normal;

    /// <summary>
    /// Инициализирует главное окно приложения.
    /// </summary>
    public MainWindow()
    {
        InitializeComponent();
        PreviewKeyDown += MainWindow_OnPreviewKeyDown;
    }

    private void TitleBar_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ClickCount == 2)
        {
            ToggleWindowState();
            return;
        }

        DragMove();
    }

    private void SettingsButton_OnClick(object sender, RoutedEventArgs e)
    {
        SettingsPopup.IsOpen = !SettingsPopup.IsOpen;
    }

    private void MinimizeButton_OnClick(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }

    private void MaximizeRestoreButton_OnClick(object sender, RoutedEventArgs e)
    {
        ToggleWindowState();
    }

    private void CloseButton_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void ReadingModeButton_OnClick(object sender, RoutedEventArgs e)
    {
        ToggleReadingMode();
    }

    private void ExitReadingModeButton_OnClick(object sender, RoutedEventArgs e)
    {
        ExitReadingMode();
    }

    private void ToggleWindowState()
    {
        WindowState = WindowState == WindowState.Maximized
            ? WindowState.Normal
            : WindowState.Maximized;
    }

    private void ToggleReadingMode()
    {
        if (ReadingModeOverlay.Visibility == Visibility.Visible)
        {
            ExitReadingMode();
            return;
        }

        EnterReadingMode();
    }

    private void EnterReadingMode()
    {
        if (DataContext is MainViewModel viewModel
            && viewModel.SelectedResult is null
            && viewModel.Results.Count > 0)
        {
            viewModel.SelectedResult = viewModel.Results[0];
        }

        _windowStateBeforeReadingMode = WindowState;
        WindowState = WindowState.Maximized;
        ReadingModeOverlay.Visibility = Visibility.Visible;
        SettingsPopup.IsOpen = false;
    }

    private void ExitReadingMode()
    {
        if (ReadingModeOverlay.Visibility != Visibility.Visible)
        {
            return;
        }

        ReadingModeOverlay.Visibility = Visibility.Collapsed;
        WindowState = _windowStateBeforeReadingMode;
    }

    private void MainWindow_OnPreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.F11)
        {
            ToggleReadingMode();
            e.Handled = true;
            return;
        }

        if (e.Key == Key.Escape && ReadingModeOverlay.Visibility == Visibility.Visible)
        {
            ExitReadingMode();
            e.Handled = true;
        }
    }

    private void ResultsList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (DataContext is not MainViewModel viewModel || !viewModel.IsLexicalMode)
        {
            return;
        }

        AnimateLexicalSpotlight();
    }

    private void AnimateLexicalSpotlight()
    {
        if (LexicalSpotlightBorder.Visibility != Visibility.Visible)
        {
            return;
        }

        LexicalSpotlightBorder.BeginAnimation(OpacityProperty, null);
        LexicalSpotlightTransform.BeginAnimation(System.Windows.Media.TranslateTransform.YProperty, null);

        var opacityAnimation = new DoubleAnimation
        {
            From = 0.45d,
            To = 1d,
            Duration = TimeSpan.FromMilliseconds(220),
            EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
        };

        var offsetAnimation = new DoubleAnimation
        {
            From = 10d,
            To = 0d,
            Duration = TimeSpan.FromMilliseconds(260),
            EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
        };

        LexicalSpotlightBorder.BeginAnimation(OpacityProperty, opacityAnimation);
        LexicalSpotlightTransform.BeginAnimation(System.Windows.Media.TranslateTransform.YProperty, offsetAnimation);
    }
}
