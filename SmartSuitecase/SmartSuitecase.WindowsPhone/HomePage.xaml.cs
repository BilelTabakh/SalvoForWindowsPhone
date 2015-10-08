using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class HomePage : Page
    {

        HttpClient clientOb = new HttpClient();
        Uri connectionUri = new Uri(SharedItems.serverUrl +"state/locked");

        public HomePage()
        {
            this.InitializeComponent();
            Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            GetaString();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            DateCourante.Text =  DateTime.Today.ToString("dd-MM-yyyy");
            
            this.GetaString();

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
                string ResponseString = await httpClient.GetStringAsync(
                    connectionUri);

                if (ResponseString == "{0}")
                {
                    lockState.Source = new BitmapImage(new Uri("ms-appx:///Assets/ic_home_unlock.png", UriKind.Absolute));
                }

                if (ResponseString == "{1}")
                {
                    lockState.Source = new BitmapImage(new Uri("ms-appx:///Assets/ic_home_lock.png", UriKind.Absolute));
                }
            }
            catch
            {
                lockState.Source = new BitmapImage(new Uri("ms-appx:///Assets/no_wifi.png", UriKind.Absolute));
            }

            try
            {
                //Create HttpClient
                HttpClient httpClient = new HttpClient();

                //Define Http Headers
                httpClient.DefaultRequestHeaders.Accept.TryParseAdd("application/json");

                //Call
                connectionUri = new Uri(SharedItems.serverUrl + "state/temperature");
                string ResponseString = await httpClient.GetStringAsync(connectionUri);
                weather.Text = ResponseString.Substring(1, ResponseString.IndexOf(".") - 1);
                weatherImg.Source = new BitmapImage(new Uri("ms-appx:///Assets/ic_home_weather.png", UriKind.Absolute));
            }
            catch
            {
                weatherImg.Source = new BitmapImage(new Uri("ms-appx:///Assets/no_wifi.png", UriKind.Absolute));
            }
        }

        private void HardwareButtons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {

            if (this.Frame.CanGoBack)
            {
                e.Handled = true;
                this.Frame.GoBack();
            }

        }

        private void lockClicked(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(LockUnlockPage));
        }
    }
}
