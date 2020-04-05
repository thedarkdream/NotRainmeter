using System.Collections.Generic;

namespace NotRainmeter
{
    public class CovidData
    {
        public List<CovidEntry> Entries { get; set; }

        public CovidData() { Entries = new List<CovidEntry>(); }
    }

    public class CovidEntry
    {
        public string Country { get; set; }
        public int Total { get; set; }
        public int Dead { get; set; }
        public int Recovered { get; set; }
    }
}