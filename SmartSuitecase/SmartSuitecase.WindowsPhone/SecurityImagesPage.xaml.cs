using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace SmartSuitecase
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SecurityImagesPage : Page
    {


        ObservableCollection <SSImage> images;
        HttpClient clientOb = new HttpClient();
        Uri connectionUri = new Uri(SharedItems.serverUrl+"security/images/");
        bool error = false;
        public SecurityImagesPage()
        {
            this.InitializeComponent();
            images = new ObservableCollection <SSImage>();
            GetaString();
            
        }

        public async void GetaString()
        {
            try
            {
                //Create HttpClient
                HttpClient httpClient = new HttpClient();

                //Define Http Headers
                httpClient.DefaultRequestHeaders.Accept.TryParseAdd("application/json");

                //Call
                string ResponseString = await httpClient.GetStringAsync(connectionUri);
                JsonArray ja = JsonArray.Parse(ResponseString);
                Debug.WriteLine("Response String :"+ResponseString);
                int counter = 0;
                foreach (JsonValue s in ja)
                {
                    counter ++;
                    SSImage ssimg = new SSImage();
                    ssimg.Name = "Image #" + counter;
                    ssimg.uriImg = new Uri(SharedItems.mediaServerUrl+"security/images/"  + s.Stringify().Replace("\"", ""));
                    ssimg.imgSec = new BitmapImage(ssimg.uriImg);
                    ssimg.Date = s.Stringify().Substring(4, "XX-XX-XXXX".Length).Replace("-","/");
                    ssimg.Time = s.Stringify().Substring(s.Stringify().IndexOf("-----")+5, "XX-XX-XX".Length).Replace("-", ":");

                    images.Add(ssimg);
                    Debug.WriteLine("uriiii : " + ssimg.uriImg + "name : " + ssimg.Name);
                
                }

                ImagesLV.ItemsSource = images;
                ImagesLV.DataContext = images;

               
               
            }
            catch
            {
                error = true;
            }
            if (error)
            {
                var dialog = new MessageDialog("Error while connecting to your Suitcase", "Error");
                await dialog.ShowAsync();
            }
            
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }


        private void HardwareButtons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {

            if (this.Frame.CanGoBack)
            {
                e.Handled = true;
                this.Frame.GoBack();
            }

        }

        private void goToPic(object sender, SelectionChangedEventArgs e)
        {
            
            Frame.Navigate(typeof(SecurityPicturePage));
        }


    }
}
