using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;
using Windows.UI.Popups;
using System.Diagnostics;
using System.Net;
using System.Text;
namespace SmartSuitecase
{

    public sealed partial class LockUnlockPage : Page
    {

        Uri connectionUri = new Uri(SharedItems.serverUrl + "state/locked");
        Dictionary<string, string> pairs = new Dictionary<string, string>();

        public LockUnlockPage()
        {
            this.InitializeComponent();
            Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            this.GetaString();

        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.GetaString();
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
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


                progress.Visibility = Visibility.Visible;
                lock1.Visibility = Visibility.Collapsed;
                unlock1.Visibility = Visibility.Collapsed;

                //Call
                string ResponseString = await httpClient.GetStringAsync(
                    connectionUri);

                if (ResponseString == "{0}")
                {
                    progress.Visibility = Visibility.Collapsed;
                    lock1.Visibility = Visibility.Visible;
                    unlock1.Visibility = Visibility.Collapsed;
                    txtStatus.Text = "Your SuitCase is Unlocked";
                    txtChangeStatus.Text = "(Tap to lock it)";
                }

                if (ResponseString == "{1}")
                {
                    progress.Visibility = Visibility.Collapsed;
                    lock1.Visibility = Visibility.Collapsed;
                    unlock1.Visibility = Visibility.Visible;
                    txtStatus.Text = "Your SuitCase is Locked";
                    txtChangeStatus.Text = "(Tap to unlock it)";
                }
                httpClient.Dispose();
            }
            catch
            {
                txtStatus.Text = "Connect to your suitecase's WiFi";
                txtChangeStatus.Text = "";
            }
            
        }

        private async void lockTapped(object sender, TappedRoutedEventArgs e)
        {
            HttpClient clientOb = new HttpClient();
            pairs.Clear();
            pairs.Add("locked", "1");
            HttpFormUrlEncodedContent formContent = new HttpFormUrlEncodedContent(pairs);

            progress.Visibility = Visibility.Visible;
            lock1.Visibility = Visibility.Collapsed;
            unlock1.Visibility = Visibility.Collapsed;

            await clientOb.PostAsync(connectionUri, formContent);

            HttpResponseMessage response = await clientOb.PostAsync(connectionUri, formContent);

            if (!response.IsSuccessStatusCode)
            {
                var dialog = new MessageDialog("Error while closing your suitecase", "Error");
                await dialog.ShowAsync();
            }
            else
            {
                progress.Visibility = Visibility.Collapsed;
                lock1.Visibility = Visibility.Collapsed;
                unlock1.Visibility = Visibility.Visible;
                txtStatus.Text = "Your Suitcase is Locked";
                txtChangeStatus.Text = "(Tap to unlock it)";
            }
            clientOb.Dispose();

        }

        private async void unlockTapped(object sender, TappedRoutedEventArgs e)
        {
            HttpClient clientOb = new HttpClient();
            pairs.Clear();
            pairs.Add("locked", "0");
            HttpFormUrlEncodedContent formContent = new HttpFormUrlEncodedContent(pairs);

            progress.Visibility = Visibility.Visible;
            lock1.Visibility = Visibility.Collapsed;
            unlock1.Visibility = Visibility.Collapsed;


            await clientOb.PostAsync(connectionUri, formContent);

            HttpResponseMessage response = await clientOb.PostAsync(connectionUri, formContent);

            if (!response.IsSuccessStatusCode)
            {
                var dialog = new MessageDialog("Error while opening your suitecase", "Error");
                await dialog.ShowAsync();
            }
            else
            {
                progress.Visibility = Visibility.Collapsed;
                lock1.Visibility = Visibility.Visible;
                unlock1.Visibility = Visibility.Collapsed;
                txtStatus.Text = "Your Suitcase is Unlocked";
                txtChangeStatus.Text = "(Tap to lock it)";
            }
            clientOb.Dispose();
        }
        private void HardwareButtons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {

            if (this.Frame.CanGoBack)
            {
                e.Handled = true;
                this.Frame.GoBack();
            }

        }

    }
}
