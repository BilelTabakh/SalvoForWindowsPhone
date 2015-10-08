using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;

namespace SmartSuitecase
{

    public sealed partial class AddTrip : Page
    {
        Windows.Web.Http.HttpClient clientOb = new Windows.Web.Http.HttpClient();
        Uri connectionUri = new Uri("http://phpmyadminesprit.azurewebsites.net/SmartSuitcase/trips.php");
        Dictionary<string, string> pairs = new Dictionary<string, string>();
        string responseBodyAsText;
        private async void SendData()
        {
            pairs.Clear();
            pairs.Add("country", country.Text);
            pairs.Add("airport", airport.Text);
            HttpFormUrlEncodedContent formContent = new HttpFormUrlEncodedContent(pairs);
            await clientOb.PostAsync(connectionUri, formContent);

            Windows.Web.Http.HttpResponseMessage response = await clientOb.PostAsync(connectionUri, formContent);

            if (!response.IsSuccessStatusCode)
            {
                var dialog = new MessageDialog("Error while Adding a trip", "Error");
                await dialog.ShowAsync();
            }
            else
            {

                responseBodyAsText = await response.Content.ReadAsStringAsync();
                responseBodyAsText = responseBodyAsText.Replace("<br>", Environment.NewLine); // Insert new lines
                if (responseBodyAsText.Substring(13, 2).Equals("ko"))
                {
                    var dialog = new MessageDialog("Error in Adding", "Error");
                    await dialog.ShowAsync();
                }
                else
                {
                    {
                        var dialog = new MessageDialog("Trip Added", "Added");
                        await dialog.ShowAsync();
                    }
                }

            }
        }

        public AddTrip()
        {
            this.InitializeComponent();
            Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SendData();
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
