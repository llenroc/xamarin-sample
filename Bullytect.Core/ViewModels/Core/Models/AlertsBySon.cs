using System;
using System.Collections.Generic;
using Syncfusion.SfChart.XForms;

namespace Bullytect.Core.ViewModels.Core.Models
{
    public class AlertsBySon
    {
        public string FullName { get; set; }
        public IList<ChartDataPoint> Alerts { get; set; }
    }
}
