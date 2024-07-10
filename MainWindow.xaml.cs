using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace SimpleVideoPlayer
{
    public partial class MainWindow : Window
    {
        private bool UserIsDraggingSlider = false;

        public MainWindow()
        {
            InitializeComponent();
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
            Player.Volume = 0.5; // Set volume to 50%
        }

        // Timer event to update the slider and label with the current playback position
        private void Timer_Tick(object sender, EventArgs e)
        {
            if ((Player.Source != null) && (Player.NaturalDuration.HasTimeSpan) && (!UserIsDraggingSlider))
            {
                lbpLaying.Content = String.Format("{0} / {1}", Player.Position.ToString(@"mm\:ss"), Player.NaturalDuration.TimeSpan.ToString(@"mm\:ss"));
                slidvideo.Minimum = 0;
                slidvideo.Maximum = Player.NaturalDuration.TimeSpan.TotalSeconds;
                slidvideo.Value = Player.Position.TotalSeconds;
            }
            else
            {
                lbpLaying.Content = "No file selected...";
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Load the video file when the window is loaded
            // Commented out to allow user to select file
            // Player.Source = new Uri(@"C:\MSSA\TechDev\20483\Week10\SimpleVideoPlayer\NetflixTrailers.mp4");
        }

        // Play button click event
        private void btnplay_Click(object sender, RoutedEventArgs e)
        {
            if (Player.Source != null)
            {
                Player.Play();
            }
        }

        // Stop button click event
        private void btnstop_Click(object sender, RoutedEventArgs e)
        {
            if (Player.Source != null)
            {
                Player.Stop();
            }
        }

        // Pause button click event
        private void btnpause_Click(object sender, RoutedEventArgs e)
        {
            if (Player.Source != null)
            {
                Player.Pause();
            }
        }

        // Open File button click event
        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Video files (*.mp4;*.avi;*.mov)|*.mp4;*.avi;*.mov|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    Player.Source = new Uri(openFileDialog.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading file: {ex.Message}");
                }
            }
        }

        // Video slider value changed event
        private void slidvideo_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (Player.Source != null && Player.NaturalDuration.HasTimeSpan && !UserIsDraggingSlider)
            {
                Player.Position = TimeSpan.FromSeconds(slidvideo.Value);
            }
        }

        // Volume slider value changed event
        private void slidvol_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Player.Volume = slidvol.Value / 100.0;
        }

        // Handle slider preview mouse down event
        private void slidvideo_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            UserIsDraggingSlider = true;
        }

        // Handle slider preview mouse up event
        private void slidvideo_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            UserIsDraggingSlider = false;
            Player.Position = TimeSpan.FromSeconds(slidvideo.Value);
        }
    }
}
