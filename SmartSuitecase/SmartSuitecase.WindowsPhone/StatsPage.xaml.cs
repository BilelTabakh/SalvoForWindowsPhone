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
using Windows.UI.Xaml.Navigation;
using WinRTXamlToolkit.Controls.DataVisualization.Charting;
using Windows.Data.Json;
using Windows.UI.Popups;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Diagnostics;


namespace SmartSuitecase
{

    public sealed partial class StatsPage : Page
    {
        List<int> occurenceCountry = new List<int>();
        List<string> cont = new List<string>();
        List<string> contD = new List<string>();
        List<FinancialStuff> financialStuffList = new List<FinancialStuff>();
        int count = 0;
        bool popup = false;
        public class FinancialStuff
        {
            public string Name { get; set; }
            public int Amount { get; set; }
        }

        public StatsPage()
        {
            this.InitializeComponent();
            Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;
        }



        async private void ReadDataFromWeb()
        {
           
        }
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            var client = new HttpClient();
            var response = await client.GetAsync(new Uri("http://phpmyadminesprit.azurewebsites.net/SmartSuitcase/trips.php/1/all"));
            var result = await response.Content.ReadAsStringAsync();

            try
            {

                JArray jsonarray = JArray.Parse(result);
                var dict = (JArray)JsonConvert.DeserializeObject(Convert.ToString(jsonarray));

                foreach (var obj1 in dict)
                {
                    var g = dict.GroupBy(i => (string)obj1["country"]);

                    foreach (var grp in g)
                    {
                        cont.Add(grp.Key);
                    }
                }
                contD = cont.Distinct().ToList();
                foreach (string a in contD)
                {
                    {
                        count = cont.Count(x => x == a);
                        occurenceCountry.Add(count);
                    }
                }



                /*    foreach (int d in occurenceCountry)
                    {
                        Debug.WriteLine(d);
                    }

                    foreach (string dd in contD)
                    {
                        Debug.WriteLine(dd);
                    }
                    */

                string ea = "e";
                foreach (int d in occurenceCountry)
                {
                    Debug.WriteLine(d);
                    ea = ea + "e";
                    financialStuffList.Add(new FinancialStuff { Name = ea, Amount = d });
                }
                (BarChart.Series[0] as BubbleSeries).ItemsSource = financialStuffList;

                string CC = "";
                foreach (int d in occurenceCountry)
                {
                    CC = CC + d + "\n";
                    CCC.Text = CC + "\n";
                }

                string OO = "";
                foreach (string dd in contD)
                {
                    OO = OO + dd + "\n";
                    O.Text = OO + "\n";
                }
            }
            catch {
                popup = true;
                
            }
            if (popup) {
                MessageDialog msg = new MessageDialog("Please Connect To Internet","Error");
                await msg.ShowAsync();
                popup = false;
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

        private void GoAddTrip(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AddTrip));
        }
    }
}
