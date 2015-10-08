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

namespace SmartSuitecase
{

    public sealed partial class Menu : Page
    {
        public Menu()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }
        private void Hometile_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(HomePage));
        }
        private void LockUnlockTile_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(LockUnlockPage));
        }
        private void AboutTile_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AboutPage));
        }
        private void TrackLocationTile_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(TrackLocationPage));
        }
        private void StatsTile_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(StatsPage));
        }
        private void SettingTile_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SettingPage));
        }

        private void BuiltInScaleTile_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(BuiltInScalePage));
        }

        private void ImageSecTile_Trapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SecurityImagesPage));
        }

    }
}
