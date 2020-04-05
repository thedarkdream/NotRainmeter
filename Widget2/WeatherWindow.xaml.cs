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
    [WidgetAttribute("Weather")]
    public partial class WeatherWindow : WidgetWindow
    {
        public const String URL = "http://api.openweathermap.org/data/2.5/weather?q=Sibiu&APPID=1b08e4d45016a0f873edb710dfc49dfc";

        public WeatherWindow()
        {
            InitializeComponent();
        }


        private void UpdateWeather()
        {
            String json = HttpGet(URL);
            WeatherResult result = JsonConvert.DeserializeObject<WeatherResult>(json);

            Double degrees = result.Main.Temp - 273.16;

            weather1Label.Content = result.Weather[0].Main;
            weather2Label.Content = result.Weather[0].Description;
            image.Source = new BitmapImage(new Uri(@"http://openweathermap.org/img/w/" + result.Weather[0].Icon + ".png", UriKind.Absolute));
            townLabel.Content = result.Name;
            tempLabel.Content = "" + ((int)degrees) + "°";
        }

        public string HttpGet(string url)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);

            request.Method = "GET";
            request.UserAgent = "Me";
            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;

            var content = string.Empty;

            using (var response = (HttpWebResponse)request.GetResponse())
            {
                using (var stream = response.GetResponseStream())
                {
                    using (var sr = new StreamReader(stream))
                    {
                        content = sr.ReadToEnd();
                    }
                }
            }

            return content;
        }

        public override double GetRefreshIntervalInSeconds()
        {
            return 1800;
        }

        public override void RefreshWidget(object sender, EventArgs e)
        {
            UpdateWeather();
        }
    }
}
