using System;
using Bullytect.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(TableViewWithoutBorders), typeof(TableViewWithoutBorderRenderer))]
namespace Bullytect.Droid.Renderers
{
    public class TableViewWithoutBorderRenderer: TableViewRenderer
    {
		protected override void OnElementChanged(ElementChangedEventArgs<TableView> e)
		{
			base.OnElementChanged(e);

			if (Control == null)
				return;

			var listView = Control as global::Android.Widget.ListView;
			listView.DividerHeight = 0;
		}
    }
}
