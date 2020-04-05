using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NotRainmeter
{
    /// <summary>
    /// Interaction logic for ConfigurationWindow.xaml
    /// </summary>
    public partial class ConfigurationWindow : Window
    {

        public ConfigurationWindow()
        {
            InitializeComponent();
            App.AllWidgets.Keys.ToList().ForEach(i => WidgetList.Items.Add(i));
            App.Config.Widgets.Select(w => w.Name).ToList().ForEach(w => WidgetList.SelectedItems.Add(w));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            foreach(String item in WidgetList.SelectedItems)
            {
                if(!App.Config.Widgets.Exists(w => w.Name.Equals(item)))
                {
                    WidgetConfiguration conf = new WidgetConfiguration();
                    conf.Name = item;
                    conf.Top = 200;
                    conf.Left = 200;

                    App.Config.Widgets.Add(conf);

                    App.CreateWindow(conf);
                }
            }

            List<String> widgetsToRemove = new List<string>();
            foreach(WidgetConfiguration c in App.Config.Widgets)
            {
                if(!WidgetList.SelectedItems.Contains(c.Name))
                {
                    widgetsToRemove.Add(c.Name);
                }
            }

            App.Config.Widgets.RemoveAll(c => widgetsToRemove.Contains(c.Name));
            widgetsToRemove.ForEach(name => App.RemoveWindow(name));
            App.StoreConfiguration();
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
