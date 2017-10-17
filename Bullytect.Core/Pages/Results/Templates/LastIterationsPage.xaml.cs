
using Microcharts;
using SkiaSharp;
using Xamarin.Forms;

namespace Bullytect.Core.Pages.Results.Templates
{
    public partial class LastIterationsPage : ContentView
    {
        public LastIterationsPage()
        {
            InitializeComponent();

			var entries = new[]
            {
            	new Microcharts.Entry(200)
            	{
            		Label = "January",
            		ValueLabel = "200",
                    Color = SKColor.Parse("#266489")
            	},
            	new Microcharts.Entry(400)
            	{
                	Label = "February",
                	ValueLabel = "400",
                	Color = SKColor.Parse("#68B9C0")
            	},
            	new Microcharts.Entry(-100)
            	{
                	Label = "March",
                	ValueLabel = "-100",
                	Color = SKColor.Parse("#90D585")
            	}
            };

            var chart = new LineChart() { Entries = entries };

            chartView.Chart = chart;
        }


    }
}
