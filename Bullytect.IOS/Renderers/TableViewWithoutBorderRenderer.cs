using System;
using Bullytect.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Bullytect.Core.Pages.Common.Extended;


[assembly: ExportRenderer(typeof(TableViewWithoutBorders), typeof(TableViewWithoutBorderRenderer))]
namespace Bullytect.iOS.Renderers
{
    public class TableViewWithoutBorderRenderer: TableViewRenderer
    {
		protected override void OnElementChanged(ElementChangedEventArgs<TableView> e)
		{
			base.OnElementChanged(e);

			if (Control == null)
				return;

			var tableView = Control as UITableView;
			tableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
		}
    }
}
