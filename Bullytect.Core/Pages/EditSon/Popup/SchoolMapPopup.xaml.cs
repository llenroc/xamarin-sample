
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

        void initMap() {

            if (_selectable)
            {

                Observable.FromAsync(() => GetCurrentLocation())
                      .ObserveOn(RxApp.MainThreadScheduler)
                      .Where((Position) => Position != null)
                      .Subscribe((Position) =>
                      {
                          Map.MoveToRegion(MapSpan.FromCenterAndRadius(
                                      new Xamarin.Forms.Maps.Position(Position.Latitude, Position.Longitude),
                                          Distance.FromMiles(1)));
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

                if (_school != null)
                {
                    Map.MoveToRegion(MapSpan.FromCenterAndRadius(
                        new Xamarin.Forms.Maps.Position(_school.Latitude, _school.Longitude),
                                    Distance.FromMiles(1)));
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

            Map.MapLongPress += async (object sender, TK.CustomMap.TKGenericEventArgs<Xamarin.Forms.Maps.Position> e) => {

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


    }
}
