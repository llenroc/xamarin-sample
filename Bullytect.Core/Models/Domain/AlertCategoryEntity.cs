using System;
using MvvmHelpers;

namespace Bullytect.Core.Models.Domain
{
    public class AlertCategoryEntity: ObservableObject
    {

		public string Name { get; set; }

        public string Description { get; set; }

        public AlertLevelEnum Level { get; set; } 


        bool filtered;

		public bool IsFiltered
		{
			get { return filtered; }
			set { SetProperty(ref filtered, value); }
		}

		bool enabled;

		public bool IsEnabled
		{
			get { return enabled; }
			set { SetProperty(ref enabled, value); }
		}

    }
}
