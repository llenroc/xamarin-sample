using System;
using System.Collections.Generic;
using Microcharts;

namespace Bullytect.Core.ViewModels.Core.Models
{
    public class AlertsBySon
    {
        public string FullName { get; set; }
        public IList<Entry> Alerts { get; set; }
    }
}
