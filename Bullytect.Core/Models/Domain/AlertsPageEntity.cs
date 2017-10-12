using System;
using System.Collections.Generic;
using MvvmHelpers;

namespace Bullytect.Core.Models.Domain
{
    public class AlertsPageEntity: ObservableObject
    {

		IList<AlertEntity> _alerts;

		public IList<AlertEntity> Alerts
		{
			get { return _alerts; }
			set { SetProperty(ref _alerts, value); }
		}

        int _total;

		public int Total
		{
			get { return _total; }
			set { SetProperty(ref _total, value); }
		}

		int _returned;

		public int Returned
		{
			get { return _returned; }
			set { SetProperty(ref _returned, value); }
		}

		int _remaining;

		public int Remaining
		{
			get { return _remaining; }
			set { SetProperty(ref _remaining, value); }
		}

		DateTime _lastQuery;

		public DateTime LastQuery
		{
			get { return _lastQuery; }
			set { SetProperty(ref _lastQuery, value); }
		}

		public void HydrateWith(AlertsPageEntity OtherAlertsPageEntity)
		{
            Alerts = OtherAlertsPageEntity.Alerts;
            Total = OtherAlertsPageEntity.Total;
            Returned = OtherAlertsPageEntity.Returned;
			Remaining = OtherAlertsPageEntity.Remaining;
			LastQuery = OtherAlertsPageEntity.LastQuery;
		}
    }
}
