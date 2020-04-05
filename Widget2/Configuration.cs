using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotRainmeter
{
    public class Configuration
    {
        public List<WidgetConfiguration> Widgets { get; set; }

        public Configuration()
        {
            Widgets = new List<WidgetConfiguration>();
        }
    }

    public class WidgetConfiguration
    {
        public string Name { get; set; }
        public int Top { get; set; }
        public int Left { get; set; }
    }
}
