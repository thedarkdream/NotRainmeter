using log4net;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace NotRainmeter
{
    public abstract class WidgetWindow : Window
    {
        protected ILog logger;
        public WidgetWindow()
        {
            this.Loaded += Window_Loaded;
            this.MouseLeftButtonDown += Window_MouseLeftButtonDown;
            this.LocationChanged += Window_LocationChanged;
            
            logger = LogManager.GetLogger(this.GetType());
        }

        public abstract double GetRefreshIntervalInSeconds();
        public abstract void RefreshWidget(object sender, EventArgs e);

        public void Refresh(object sender, EventArgs e)
        {
            try
            {
                RefreshWidget(sender, e);
            }
            catch (Exception ex)
            {
                logger.Error("Unable to refresh widget " + Name + " because of exception: " + ex.Message);
            }
        }

        protected void Window_Loaded(object sender, RoutedEventArgs e)
        {
            WindowUtils.SetBottom(this);
            WindowUtils.SetOnDesktop(this);

            var widgetConf = GetConfiguration();
            int left = widgetConf.Left;
            int top = widgetConf.Top;

            this.Left = left;
            this.Top = top;

            InitRefresh();

        }

        private void InitRefresh()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(GetRefreshIntervalInSeconds());
            timer.Tick += Refresh;
            timer.Start();
        }

        protected void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        protected void Window_LocationChanged(object sender, EventArgs e)
        {
            var widgetConf = GetConfiguration();
            widgetConf.Left = (int)this.Left;
            widgetConf.Top = (int)this.Top;

            App.StoreConfiguration();
        }

        public WidgetConfiguration GetConfiguration()
        {
            string name = App.FindName(this.GetType());
            return App.Config.Widgets.Where(w => w.Name.Equals(name)).First();
        }

    }
}
