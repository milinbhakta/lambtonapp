
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;

namespace App3
{
    [Activity(Label = "Map" , Theme = "@android:style/Theme.Dialog")]
    public class Map : Activity, IOnMapReadyCallback
    {
        private GoogleMap GMap;
        public string address;

        public void OnMapReady(GoogleMap googleMap)
        {
            this.GMap = googleMap;
            //var location = GetLocation(address);
            try{
                var geo = new Geocoder(this);
                var addresses = geo.GetFromLocationName(address, 1);
                GMap.UiSettings.ZoomControlsEnabled = true;
                LatLng latlng = new LatLng(Convert.ToDouble(addresses[0].Latitude), Convert.ToDouble(addresses[0].Longitude));
                CameraUpdate camera = CameraUpdateFactory.NewLatLngZoom(latlng, 5);
                GMap.MoveCamera(camera);
                MarkerOptions options = new MarkerOptions().SetPosition(latlng).SetTitle(address);
                GMap.AddMarker(options);
            }catch(Exception e){
                Console.WriteLine("Map:OnMapReadyExecption"+e.Message);
            }

        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);

            // Create your application here

            address = Transfer.Address;
            SetContentView(Resource.Layout.MapView);
            if (GMap == null)
            {
                MapView mMapView = (MapView)FindViewById(Resource.Id.mapView1);
                MapsInitializer.Initialize(this);

                mMapView.OnCreate(savedInstanceState);
                mMapView.OnResume();
                mMapView.GetMapAsync(this);
                Button close = (Android.Widget.Button)FindViewById(Resource.Id.button1);
                close.Click += (sender, e) => {
                    this.Finish();
                };
            }
        }

        public static async Task<Location> GetLocation(string address)
        {
            var geocoder = new GoogleGeocodeService();
            var result = await geocoder.GeocodeLocation(address);
            return result;
        }
    }
}
