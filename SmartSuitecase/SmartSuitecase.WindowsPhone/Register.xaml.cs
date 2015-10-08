using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
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

namespace SmartSuitecase
{
    public sealed partial class Register : Page
    {
        Windows.Web.Http.HttpClient clientOb = new Windows.Web.Http.HttpClient();
        Uri connectionUri = new Uri("http://phpmyadminesprit.azurewebsites.net/SmartSuitcase/register.php");
        Dictionary<string, string> pairs = new Dictionary<string, string>();
        string responseBodyAsText;
        public Register()
        {
            this.InitializeComponent();
            Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;
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
        }
        private async void SendData()
        {
            if (!password1.Password.Equals(password2.Password))
            {
                var dialog = new MessageDialog("Passwords do not match", "Error");
                await dialog.ShowAsync();
            }
            else if (!Regex.IsMatch(email.Text, @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"))
            {
                var dialog = new MessageDialog("E-mail is not valid", "Error");
                await dialog.ShowAsync();
            }
            else
            {
                pairs.Clear();
                pairs.Add("email", email.Text);
                pairs.Add("password", password1.Password);
                Windows.Web.Http.HttpFormUrlEncodedContent formContent = new Windows.Web.Http.HttpFormUrlEncodedContent(pairs);
                await clientOb.PostAsync(connectionUri, formContent);

                Windows.Web.Http.HttpResponseMessage response = await clientOb.PostAsync(connectionUri, formContent);

                if (!response.IsSuccessStatusCode)
                {
                    var dialog = new MessageDialog("Error while logging in", "Error");
                    await dialog.ShowAsync();
                }
                else
                {

                    responseBodyAsText = await response.Content.ReadAsStringAsync();
                    responseBodyAsText = responseBodyAsText.Replace("<br>", Environment.NewLine); // Insert new lines
                    if (responseBodyAsText.Substring(13, 2).Equals("ko"))
                    {
                        var dialog = new MessageDialog("Error in registration", "Error");
                        await dialog.ShowAsync();
                    }
                    else
                    {
                        {
                            var dialog = new MessageDialog("registred", "Registred");
                            await dialog.ShowAsync();
                            this.Frame.Navigate(typeof(MainPage));
                        }
                    }

                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SendData();
        }
    }
}
