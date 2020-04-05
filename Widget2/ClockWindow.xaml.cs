using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace NotRainmeter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [WidgetAttribute("Clock")]
    public partial class ClockWindow : WidgetWindow
    {
        public const String URL = "http://api.openweathermap.org/data/2.5/weather?q=Sibiu&APPID=1b08e4d45016a0f873edb710dfc49dfc";

        public ClockWindow()
        {
            InitializeComponent();
        }

        protected new void Window_Loaded(object sender, RoutedEventArgs e)
        {
            base.Window_Loaded(sender, e);

        }

        public override double GetRefreshIntervalInSeconds()
        {
            return 1;
        }

        public override void RefreshWidget(object sender, EventArgs e)
        {
            UpdateTimes();
        }

        private void UpdateTimes()
        {
            timeLabel.Content = DateTime.Now.ToString("HH:mm");
            dateLabel.Content = DateTime.Now.ToString("ddd, dd MMM yyy");
        }

    }
}
