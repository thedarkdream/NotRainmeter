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
using System.Linq;
using System.Windows.Documents;

namespace NotRainmeter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [WidgetAttribute("Coronavirus")]
    public partial class CovidWindow : WidgetWindow
    {
        public const String URL = "https://corona.lmao.ninja/countries/";

        public CovidData CovidData { get; set; }

        public CovidWindow()
        {
            CovidData = new CovidData();
            InitializeComponent();
        }

        public int WidgetHeight
        {
            get { return 200; }
            set { }
        }

        protected new void Window_Loaded(object sender, RoutedEventArgs e)
        {
            base.Window_Loaded(sender, e);

            UpdateStatistics();

            myTable.Columns.Add(new System.Windows.Documents.TableColumn());
            myTable.Columns.Add(new System.Windows.Documents.TableColumn());
            myTable.Columns.Add(new System.Windows.Documents.TableColumn());
            myTable.Columns.Add(new System.Windows.Documents.TableColumn());

            myTable.FontFamily = new FontFamily("Segoe UI");
            myTable.FontSize = 16;

        }


        public override double GetRefreshIntervalInSeconds()
        {
            return 60;
        }

        public override void RefreshWidget(object sender, EventArgs e)
        {
            UpdateStatistics();
        }
   
        private void UpdateStatistics()
        {

            CovidData = new CovidData();

            myTable.RowGroups.Clear();

            string[] relevantCountries = new string[] { "romania", "germany", "italy", "austria", "spain" };
            foreach (var country in relevantCountries) {

                String json = HttpGet(URL + country);
                CovidResponse result = JsonConvert.DeserializeObject<CovidResponse>(json);

                CovidEntry e = new CovidEntry { Country = result.country, Dead = result.deaths, Recovered = result.recovered, Total = result.cases };

                CovidData.Entries.Add(e);
            }

            {
                TableRow tr = new TableRow();
                tr.FontSize = 12;

                tr.Cells.Add(new TableCell(new Paragraph(new Run("Country"))));
                tr.Cells.Add(new TableCell(new Paragraph(new Run("Total"))));
                tr.Cells.Add(new TableCell(new Paragraph(new Run("Deaths"))));
                tr.Cells.Add(new TableCell(new Paragraph(new Run("Recovered"))));

                TableRowGroup trg = new TableRowGroup();
                trg.Rows.Add(tr);
                myTable.RowGroups.Add(trg);
            }

            foreach (var entry in CovidData.Entries)
            {
                TableRow tr = new TableRow();

                tr.Cells.Add(new TableCell(new Paragraph(new Run(entry.Country))));
                tr.Cells.Add(new TableCell(new Paragraph(new Run(entry.Total.ToString()))));
                var deadCell = new TableCell(new Paragraph(new Run(entry.Dead.ToString())));
                deadCell.Foreground = new SolidColorBrush(Colors.Red);
                tr.Cells.Add(deadCell);
                tr.Cells.Add(new TableCell(new Paragraph(new Run(entry.Recovered.ToString()))));

                TableRowGroup trg = new TableRowGroup();
                trg.Rows.Add(tr);
                myTable.RowGroups.Add(trg);
            }

        }

        private object TableCell(object @new, object p)
        {
            throw new NotImplementedException();
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

    }
}
