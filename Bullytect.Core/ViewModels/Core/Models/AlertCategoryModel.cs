using System;
using Bullytect.Core.Models.Domain;

namespace Bullytect.Core.ViewModels.Core.Models
{
    public class AlertCategoryModel: CategoryModel
    {
        public AlertLevelEnum Level { get; set; }
	}
}
