using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using log4net;

namespace NotRainmeter
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        private ILog logger;

        public static Configuration Config;
        public static Dictionary<string, Type> AllWidgets;

        NotifyIcon notifyIcon;

        public static Dictionary<String, WidgetWindow> Widgets = new Dictionary<string, WidgetWindow>();

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            logger = LogManager.GetLogger(this.GetType());

            AllWidgets = LoadAvailableWidgets();

            log.Info("Available widgets: " + string.Join(", ", AllWidgets.Keys));

            Config = JsonConvert.DeserializeObject<Configuration>(NotRainmeter.Properties.Settings.Default.Configuration);
            if(Config == null)
            {
                Config = new Configuration();
            }

            notifyIcon = new NotifyIcon();
            notifyIcon.Click += new EventHandler(NotifyIcon_Click);
            notifyIcon.Icon = NotRainmeter.Properties.Resources.TrayIcon;
            notifyIcon.Visible = true;

            notifyIcon.ContextMenu = BuildContextMenu();

            Config.Widgets.ForEach(widget => CreateWindow(widget));

        }

        internal static void CreateWindow(WidgetConfiguration widget)
        {
            log.Info("Created widget window: " + widget.Name);
            WidgetWindow w = (WidgetWindow)Activator.CreateInstance(AllWidgets[widget.Name]);
            w.Show();
            Widgets.Add(widget.Name, w);
        }

        internal static void RemoveWindow(String name)
        {
            WidgetWindow w = Widgets[name];
            Widgets.Remove(name);
            w.Close();
        }

        internal static void StoreConfiguration()
        {
            string serialisedConfig = JsonConvert.SerializeObject(Config);
            NotRainmeter.Properties.Settings.Default.Configuration = serialisedConfig;
            NotRainmeter.Properties.Settings.Default.Save();
        }

        private ContextMenu BuildContextMenu()
        {
            ContextMenu result = new ContextMenu();
            result.MenuItems.Add(new MenuItem("Show window", ClickShowWindow));
            result.MenuItems.Add(new MenuItem("Quit", ClickQuit));

            return result;
        }

        private void ClickQuit(object sender, EventArgs e)
        {
            notifyIcon.Visible = false;
            System.Windows.Application.Current.Shutdown();
        }

        private void ClickShowWindow(object sender, EventArgs e)
        {
            ConfigurationWindow window = new ConfigurationWindow();
            window.Show();
        }

        private void NotifyIcon_Click(object sender, EventArgs e)
        {
        }

        public Dictionary<string, Type> LoadAvailableWidgets()
        {
            Dictionary<string, Type> allWidgets = new Dictionary<string, Type>();
            var type = typeof(WidgetWindow);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && !p.Equals(type));

            foreach (var widgetClass in types)
            {
                string name = FindName(widgetClass);
                allWidgets.Add(name, widgetClass);
            }

            return allWidgets;
        }

        public static string FindName(Type widgetClass)
        {
            foreach (Attribute a in widgetClass.GetCustomAttributes(false))
            {
                if (a is WidgetAttribute widgetAttr)
                {
                    return widgetAttr.Name;
                }
            }
            return null;
        }

        private static readonly ILog log = LogManager.GetLogger(typeof(App));
        protected override void OnStartup(StartupEventArgs e)
        {
            log4net.Config.XmlConfigurator.Configure();
            log.Info("Application started.");
            base.OnStartup(e);
        }
    }
}
