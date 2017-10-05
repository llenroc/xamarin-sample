using System;
using System.Collections.Generic;

namespace Bullytect.Core.Models.Domain
{
    public class AlertsPageEntity
    {
		public IList<AlertEntity> Alerts { get; set; }

		public int Total { get; set; }

		public int Returned { get; set; }

		public int Remaining { get; set; }

		public DateTime LastQuery { get; set; }
    }
}
