namespace PikTools.Shared.Ui.Controls
{
    using System.Timers;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for RounderProgressBar.xaml
    /// </summary>
    public partial class BusyIndicator : UserControl
    {
        private Timer timer;
        private bool loaded;

        /// <summary>
        /// ctor
        /// </summary>
        public BusyIndicator()
        {
            InitializeComponent();

            Loaded += OnLoaded;
        }

        private delegate void VoidDelegete();

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            timer = new Timer(100);
            timer.Elapsed += OnTimerElapsed;
            timer.Start();
            loaded = true;
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            rotationCanvas.Dispatcher.Invoke(
                new VoidDelegete(
                    delegate
                        {
                            SpinnerRotate.Angle += 30;
                            if (SpinnerRotate.Angle == 360)
                            {
                                SpinnerRotate.Angle = 0;
                            }
                        }),
                null);
        }
    }
}
