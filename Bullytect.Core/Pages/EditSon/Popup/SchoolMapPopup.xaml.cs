
using System;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Bullytect.Core.I18N;
using Bullytect.Core.Models.Domain;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using ReactiveUI;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using TK.CustomMap.Api;
using TK.CustomMap.Api.Google;
using TK.CustomMap.Api.OSM;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Bullytect.Core.Pages.EditSon.Popup
{
    public partial class SchoolMapPopup : PopupPage
    {

        SchoolEntity _school;
        bool _selectable;

        public SchoolMapPopup(SchoolEntity School, bool Selectable = false)
        {

            _school = School;
            _selectable = Selectable;

            InitializeComponent();

            initMap();
        }


        #region methods

        void initMap()
        {

            if (_selectable)
            {
                PlacesAutocomplete.IsVisible = true;
                PlacesAutocomplete.PlaceSelectedCommand = PlaceSelectedCommand;
                Map.Margin = new Thickness(0, 60, 0, 0);

                Observable.FromAsync(() => GetCurrentLocation())
                      .ObserveOn(RxApp.MainThreadScheduler)
                      .Where((Position) => Position != null)
                      .Subscribe((Position) =>
                      {
                          var MapRegion = MapSpan.FromCenterAndRadius(
                                            new Xamarin.Forms.Maps.Position(Position.Latitude, Position.Longitude),
                                Distance.FromKilometers(2));

                          Map.MoveToRegion(MapRegion);

                          PlacesAutocomplete.Bounds = MapRegion;

                          
                      });

                initTapHandler();

                if (_school != null)
                    Title.Text = !string.IsNullOrWhiteSpace(_school.Name) ?
                        String.Format(AppResources.EditSon_School_Map_Select, _school.Name) :
                        AppResources.EditSon_School_Map_Select2;
                else
                    Title.Text = AppResources.EditSon_School_Map_Select2;
            }
            else
            {

                PlacesAutocomplete.IsVisible = false;

                if (_school != null)
                {

                    var MapRegion = MapSpan.FromCenterAndRadius(
                        new Xamarin.Forms.Maps.Position(_school.Latitude, _school.Longitude),
                        Distance.FromKilometers(2));

                    Map.MoveToRegion(MapRegion);

                    Title.Text = !string.IsNullOrWhiteSpace(_school.Name) ?
                        String.Format(AppResources.EditSon_School_Map_Visualize, _school.Name) :
                        AppResources.EditSon_School_Map_Visualize2;

                    Map.Pins.Add(new Pin
                    {
                        Type = PinType.Place,
                        Position = new Xamarin.Forms.Maps.Position(_school.Latitude, _school.Longitude),
                        Label = _school.Name
                    });

                }
                else
                {
                    Title.Text = AppResources.EditSon_School_Map_Visualize2;
                }

            }
        }

        async Task<Plugin.Geolocator.Abstractions.Position> GetCurrentLocation()
        {
            Plugin.Geolocator.Abstractions.Position position = null;
            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 100;

                if (locator.IsGeolocationAvailable && locator.IsGeolocationEnabled)
                {
                    position = await locator.GetLastKnownLocationAsync();
                    if (position == null)
                        position = await locator.GetPositionAsync(TimeSpan.FromSeconds(20), null, true);

                    if (position != null)
                    {
                        var output = string.Format("Time: {0} \nLat: {1} \nLong: {2} \nAltitude: {3} \nAltitude Accuracy: {4} \nAccuracy: {5} \nHeading: {6} \nSpeed: {7}",
                        position.Timestamp, position.Latitude, position.Longitude,
                        position.Altitude, position.AltitudeAccuracy, position.Accuracy, position.Heading, position.Speed);

                        Debug.WriteLine(output);

                    }
                }
            }
            catch (Exception ex)
            {
                //Display error as we have timed out or can't get location.
            }

            return position;
        }

        void initTapHandler()
        {

            Map.MapLongPress += async (object sender, TK.CustomMap.TKGenericEventArgs<Xamarin.Forms.Maps.Position> e) =>
            {

                if (_school != null && e?.Value != null)
                {

                    try
                    {
                        var locator = CrossGeolocator.Current;
                        var addresses = await locator.GetAddressesForPositionAsync(new Plugin.Geolocator.Abstractions.Position()
                        {
                            Latitude = e.Value.Latitude,
                            Longitude = e.Value.Longitude
                        });
                        var address = addresses.FirstOrDefault();

                        if (address != null)
                        {
                            Debug.WriteLine("Addresss: {0} {1} {2}", address.Thoroughfare, address.Locality, address.CountryName);
                            _school.Latitude = e.Value.Latitude;
                            _school.Longitude = e.Value.Longitude;
                            _school.Residence = address.Thoroughfare;
                            _school.Province = address.Locality;

                            await PopupNavigation.PopAsync(animate: true);
                        }
                        else
                        {
                            Debug.WriteLine("No address found for position.");
                        }

                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Unable to get address: " + ex);
                    }


                }

            };
        }

        #endregion

        #region commands

        public Command<IPlaceResult> PlaceSelectedCommand
        {
            get
            {
                return new Command<IPlaceResult>(async p =>
                {
                    var gmsResult = p as GmsPlacePrediction;
                    var osmResult = p as OsmNominatimResult;

                    if(gmsResult != null || osmResult != null) {

                        Xamarin.Forms.Maps.Position PositionSelected;

                        if(gmsResult != null) {

                            var details = await GmsPlace.Instance.GetDetails(gmsResult.PlaceId);
                            PositionSelected = new Xamarin.Forms.Maps.Position(details.Item.Geometry.Location.Latitude, details.Item.Geometry.Location.Longitude);

                        } else {
                            PositionSelected = new Xamarin.Forms.Maps.Position(osmResult.Latitude, osmResult.Longitude);
                        }

                        var MapRegion = MapSpan.FromCenterAndRadius(PositionSelected, Distance.FromKilometers(2));
                        Map.MoveToRegion(MapRegion);

                    } else {
                        
                        if (Device.OS == TargetPlatform.Android)
                        {
                            var prediction = (TKNativeAndroidPlaceResult)p;

                            var details = await TKNativePlacesApi.Instance.GetDetails(prediction.PlaceId);

                            Xamarin.Forms.Maps.Position? PositionSelected = details?.Coordinate;

                            if(PositionSelected.HasValue) {

                                var MapRegion = MapSpan.FromCenterAndRadius(PositionSelected.Value, Distance.FromKilometers(2));
                                Map.MoveToRegion(MapRegion);
                            }

                           
                        }
                        else if (Device.OS == TargetPlatform.iOS)
                        {
                            var prediction = (TKNativeiOSPlaceResult)p;

                            Xamarin.Forms.Maps.Position? PositionSelected = prediction?.Details?.Coordinate;

                            if(PositionSelected.HasValue) {

                                var MapRegion = MapSpan.FromCenterAndRadius(PositionSelected.Value, Distance.FromKilometers(2));
                                Map.MoveToRegion(MapRegion);
                            }


                        }

                    }



                });
            }
        }

        #endregion


    }
}
