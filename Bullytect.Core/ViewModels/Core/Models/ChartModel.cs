using System;
using System.Collections.Generic;
using Microcharts;
using System.Reflection;

namespace Bullytect.Core.ViewModels.Core.Models
{
    public class ChartModel
    {
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public IList<Entry> Entries { get; set; }
        Type _type;
        public Type Type {
            get => _type;
            set {
                #pragma warning disable CS0184
                if (value.GetTypeInfo().IsSubclassOf(typeof(Chart))){
                    _type = value;
                } else {
                    _type = typeof(BarChart);
                }
            }
        }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
