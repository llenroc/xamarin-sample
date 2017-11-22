using System;
using Xamarin.Forms;

namespace Bullytect.Core.ViewModels.Core.Models
{
    public class DimensionCategoryModel: CategoryModel
    {
        public ImageSource Icon { get; set; }
        public DimensionCategoryEnum Type { get; set; }
    }
}
