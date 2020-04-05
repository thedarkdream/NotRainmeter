using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotRainmeter
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class WidgetAttribute : Attribute
    {
        public String Name { get; set; }

        public WidgetAttribute(String name)
        {
            Name = name;
        }
    }
}
