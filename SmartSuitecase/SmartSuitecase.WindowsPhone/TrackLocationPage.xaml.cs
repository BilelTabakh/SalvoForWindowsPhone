using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

namespace SmartSuitecase
{

    public sealed partial class TrackLocationPage : Page
    {
        MapPolyline poly;
        List<BasicGeoposition> positions;
        Geolocator geolocator = null;
        BasicGeoposition b;
        BasicGeoposition SuitCasePos;
        MapIcon MapIcon;
        public TrackLocationPage()
        {
            this.InitializeComponent();
            Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            poly = new MapPolyline();
            poly.StrokeColor = Colors.Blue;
            poly.StrokeThickness = 3;
            positions = new List<BasicGeoposition>();
            b = new BasicGeoposition();
           
            
            
            MapIcon = new MapIcon();

           // MapIcon1 = new MapIcon();

            MapIcon.NormalizedAnchorPoint = new Point(0.5, 1.0);
            MapIcon.Title = "Your SuitCase is here";
          //  MapIcon.Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/Baggage.png"));
        }
 

        private void HardwareButtons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {

            if (this.Frame.CanGoBack)
            {
                e.Handled = true;
                this.Frame.GoBack();
            }

        }      

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
          //  ReadDataFromWeb();
            geolocator = new Geolocator();
            geolocator.DesiredAccuracy = PositionAccuracy.High;
            geolocator.DesiredAccuracyInMeters = 1;
            geolocator.MovementThreshold = 1;

            SuitCasePos.Longitude = 10.190039;
            SuitCasePos.Latitude = 36.899756;
            MyMap.Center = new Geopoint(SuitCasePos);
            MapIcon.Location = new Geopoint(SuitCasePos);
            MyMap.MapElements.Add(MapIcon);
            

         
        }

        async private void ReadDataFromWeb()
        {
            var client = new HttpClient();
            var response = await client.GetAsync(new Uri("http://phpmyadminesprit.azurewebsites.net/SmartSuitcase/cords.php/0/all"));
            var result = await response.Content.ReadAsStringAsync();

            try
            {
                JArray jsonarray = JArray.Parse(result);
                var dict = (JArray)JsonConvert.DeserializeObject(Convert.ToString(jsonarray));
                foreach (var obj1 in dict)
                {
                    b.Longitude = (double)obj1["longitude"];
                    b.Latitude = (double)obj1["latitude"];
                    Debug.WriteLine(obj1["id"]);
                    positions.Add(b);
                }

                MessageDialog msg = new MessageDialog("success");
                await msg.ShowAsync();

                poly.Path = new Geopath(positions);
                MyMap.MapElements.Add(poly);
                BasicGeoposition suitCasePosition = new BasicGeoposition();
                suitCasePosition.Longitude = positions.Last().Longitude;
                suitCasePosition.Latitude = positions.Last().Latitude;
              //  MapIcon1.Location = new Geopoint(suitCasePosition);
            }
            catch { }
        }

        private void MySlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (MyMap != null)
            {
                MyMap.ZoomLevel = e.NewValue;
            }
        }
    }
}
