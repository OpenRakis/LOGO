using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Platform.Storage;
using Avalonia.Threading;

namespace logo;

public sealed class MainWindow : Window
{
    private readonly Image _image;
    private readonly TextBlock _statusText;
    private readonly Button _openButton;
    private readonly Button _restartButton;
    private readonly Button _pauseButton;
    private readonly DispatcherTimer _timer;
    private readonly Stopwatch _clock = new();
    private readonly HnmPlaybackEngine _engine = new();
    private WriteableBitmap? _bitmap;
    private bool _isPlaying;
    private long? _nextStepAtTicks;

    public MainWindow()
    {
        Title = "LOGO HNM Player";
        Width = 960;
        Height = 720;

        _image = new Image
        {
            Stretch = Stretch.Uniform,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch,
            Margin = new Thickness(16)
        };

        _statusText = new TextBlock
        {
            Text = "Open LOGO.HNM to begin.",
            Margin = new Thickness(0, 0, 0, 8)
        };

        _openButton = new Button { Content = "Open HNM", Margin = new Thickness(0, 0, 8, 0) };
        _openButton.Click += OpenClicked;

        _restartButton = new Button { Content = "Restart", Margin = new Thickness(0, 0, 8, 0), IsEnabled = false };
        _restartButton.Click += RestartClicked;

        _pauseButton = new Button { Content = "Pause", IsEnabled = false };
        _pauseButton.Click += PauseClicked;

        var controls = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            Children =
            {
                _openButton,
                _restartButton,
                _pauseButton
            }
        };

        var header = new StackPanel
        {
            Margin = new Thickness(16, 12),
            Children =
            {
                _statusText,
                controls
            }
        };

        var layout = new Grid
        {
            RowDefinitions = RowDefinitions.Parse("Auto,*")
        };

        Grid.SetRow(header, 0);
        layout.Children.Add(header);

        Grid.SetRow(_image, 1);
        layout.Children.Add(_image);

        Content = layout;

        _timer = new DispatcherTimer(TimeSpan.FromMilliseconds(1), DispatcherPriority.Render, OnTick);
        Closed += (_, _) => _timer.Stop();

        KeyDown += OnKeyDown;
        Opened += async (_, _) => await TryAutoLoadAsync();
    }

    private async Task TryAutoLoadAsync()
    {
        string? candidate = GetDefaultInputPath(Environment.GetCommandLineArgs());
        if (candidate is not null && File.Exists(candidate))
        {
            Debug.Assert(Path.IsPathRooted(candidate), "Auto-load path should be absolute.");
            await LoadFileAsync(candidate);
        }
    }

    private static string? GetDefaultInputPath(string[] args)
    {
        Debug.Assert(args is not null && args.Length > 0, "Command line args should include executable path.");

        foreach (string arg in args.Skip(1))
        {
            if (!arg.StartsWith('-') && File.Exists(arg))
            {
                return Path.GetFullPath(arg);
            }
        }

        string local = Path.Combine(AppContext.BaseDirectory, "LOGO.HNM");
        if (File.Exists(local))
        {
            return local;
        }

        string cwd = Path.Combine(Environment.CurrentDirectory, "LOGO.HNM");
        return File.Exists(cwd) ? cwd : null;
    }

    private async void OpenClicked(object? sender, RoutedEventArgs e)
    {
        var topLevel = TopLevel.GetTopLevel(this);
        if (topLevel?.StorageProvider is null)
        {
            return;
        }

        IReadOnlyList<IStorageFile> files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            AllowMultiple = false,
            FileTypeFilter = [new FilePickerFileType("HNM") { Patterns = ["*.hnm", "*.HNM"] }],
            Title = "Open LOGO.HNM"
        });

        if (files.Count == 0)
        {
            return;
        }

        string? path = files[0].TryGetLocalPath();
        if (path is not null)
        {
            await LoadFileAsync(path);
        }
    }

    private async void RestartClicked(object? sender, RoutedEventArgs e)
    {
        if (_engine.InputPath is not null)
        {
            await LoadFileAsync(_engine.InputPath);
        }
    }

    private void PauseClicked(object? sender, RoutedEventArgs e)
    {
        SetPlaying(!_isPlaying);
    }

    private void OnKeyDown(object? sender, KeyEventArgs e)
    {
        if (_isPlaying)
        {
            SetPlaying(false);
            _statusText.Text = "Playback stopped by key press.";
            e.Handled = true;
            return;
        }

        if (e.Key == Key.Space)
        {
            SetPlaying(!_isPlaying);
            e.Handled = true;
        }
        else if (e.Key == Key.O)
        {
            OpenClicked(sender, new RoutedEventArgs());
            e.Handled = true;
        }
    }

    private async Task LoadFileAsync(string path)
    {
        Debug.Assert(!string.IsNullOrWhiteSpace(path), "Load path must not be empty.");
        Debug.Assert(File.Exists(path), "Load path should point to an existing file.");

        SetPlaying(false);
        _statusText.Text = $"Loading {Path.GetFileName(path)}...";

        try
        {
            await _engine.LoadAsync(path);
            _bitmap = new WriteableBitmap(new PixelSize(HnmPlaybackEngine.FrameWidth, HnmPlaybackEngine.FrameHeight), new Vector(96, 96), PixelFormat.Bgra8888, AlphaFormat.Opaque);
            Debug.Assert(_bitmap.PixelSize.Width == HnmPlaybackEngine.FrameWidth && _bitmap.PixelSize.Height == HnmPlaybackEngine.FrameHeight, "Bitmap dimensions must match HNM output dimensions.");
            _engine.RenderCurrentFrame(_bitmap);
            _image.Source = _bitmap;
            _image.InvalidateVisual();
            _restartButton.IsEnabled = true;
            _pauseButton.IsEnabled = true;
            _statusText.Text = $"Loaded {Path.GetFileName(path)}: {_engine.TotalChunks} chunks, {_engine.CurrentChunkIndex} ready (paused).";
            _nextStepAtTicks = _clock.ElapsedTicks + MillisecondsToStopwatchTicks(_engine.PendingWaitMilliseconds);
            SetPlaying(false);
        }
        catch (Exception ex)
        {
            _image.Source = null;
            _bitmap = null;
            _restartButton.IsEnabled = false;
            _pauseButton.IsEnabled = false;
            _statusText.Text = $"Failed to load {Path.GetFileName(path)}: {ex.Message}";
        }
    }

    private void OnTick(object? sender, EventArgs e)
    {
        if (!_isPlaying || _bitmap is null)
        {
            return;
        }

        Debug.Assert(_clock.IsRunning, "Playback clock should run while playing.");
        Debug.Assert(_timer.IsEnabled, "Dispatcher timer should be enabled while playing.");
        Debug.Assert(_nextStepAtTicks.HasValue, "Playback should always have a scheduled next step while running.");

        long nowTicks = _clock.ElapsedTicks;
        if (nowTicks < _nextStepAtTicks.Value)
        {
            return;
        }

        HnmStepResult result = _engine.Step();
        if (!result.Advanced)
        {
            Debug.Assert(result.PlaybackComplete, "Non-advanced step should only happen at playback completion.");
            SetPlaying(false);
            _statusText.Text = _engine.StatusText;
            return;
        }

        Debug.Assert(result.WaitBiosTicks >= 0, "Wait ticks should be non-negative.");
        Debug.Assert(result.WaitMilliseconds >= 0, "Wait milliseconds should be non-negative.");

        if (result.VisualChanged)
        {
            _engine.RenderCurrentFrame(_bitmap);
            _image.InvalidateVisual();
        }

        _statusText.Text = _engine.StatusText;

        _nextStepAtTicks = nowTicks + MillisecondsToStopwatchTicks(result.WaitMilliseconds);
    }

    private void SetPlaying(bool playing)
    {
        _isPlaying = playing;
        _pauseButton.Content = _isPlaying ? "Pause" : "Play";
        Debug.Assert(_pauseButton.IsEnabled || !_isPlaying, "Play state should not be enabled when pause button is disabled.");

        if (_isPlaying)
        {
            if (!_clock.IsRunning)
            {
                _clock.Start();
            }

            if (!_nextStepAtTicks.HasValue)
            {
                _nextStepAtTicks = _clock.ElapsedTicks;
            }

            _timer.Start();
            Debug.Assert(_timer.IsEnabled, "Timer should be enabled after entering play state.");
        }
        else
        {
            _timer.Stop();
            Debug.Assert(!_timer.IsEnabled, "Timer should be disabled after leaving play state.");
        }
    }

    private static long MillisecondsToStopwatchTicks(double milliseconds)
    {
        Debug.Assert(milliseconds >= 0, "Scheduled wait should never be negative.");
        return checked((long)Math.Ceiling((milliseconds * Stopwatch.Frequency) / 1000.0));
    }
}