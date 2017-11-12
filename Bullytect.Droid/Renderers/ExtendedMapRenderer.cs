using System;
using Android.Gms.Maps;
using Bullytect.Core.Pages.Common.Controls;
using Bullytect.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.Android;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ExtendedMap), typeof(ExtendedMapRenderer))]
namespace Bullytect.Droid.Renderers
{
    public class ExtendedMapRenderer : MapRenderer, IOnMapReadyCallback
    {
        private GoogleMap _map;

        protected override void OnMapReady(GoogleMap map)
        {
            base.OnMapReady(map);

            _map = map;
            if (_map != null)
                _map.MapClick += googleMap_MapClick;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Map> e)
        {
            if (_map != null)
                _map.MapClick -= googleMap_MapClick;
            base.OnElementChanged(e);
            if (Control != null)
                ((MapView)Control).GetMapAsync(this);
        }

        private void googleMap_MapClick(object sender, GoogleMap.MapClickEventArgs e)
        {
            ((ExtendedMap)Element).OnTap(new Position(e.Point.Latitude, e.Point.Longitude));
        }
    }
}
