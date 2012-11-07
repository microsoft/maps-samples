using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using LocationSelector.Resources;

using System.Device.Location;

namespace LocationSelector
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if ((Application.Current as App).RouteOriginLocation != null)
            {
                LatitudeBox1.Text = (Application.Current as App).RouteOriginLocation.Latitude.ToString();
                LongittudeBox1.Text = (Application.Current as App).RouteOriginLocation.Longitude.ToString();

                (Application.Current as App).RouteOriginLocation = null;
            }

            if ((Application.Current as App).SelectedLocation != null)
            {
                LatitudeBox2.Text = (Application.Current as App).SelectedLocation.Latitude.ToString();
                LongittudeBox2.Text = (Application.Current as App).SelectedLocation.Longitude.ToString();

                (Application.Current as App).SelectedLocation = null;
            }
        }

        private void Button_gridbut_Click(object sender, RoutedEventArgs e)
        {
            if (sender == getGeoButton1)
            {
                (Application.Current as App).RouteOriginLocation = null;
                try
                {
                    GeoCoordinate toGeo = new GeoCoordinate();
                    toGeo.Latitude = Double.Parse(LatitudeBox1.Text);
                    toGeo.Longitude = Double.Parse(LongittudeBox1.Text);
                    (Application.Current as App).RouteOriginLocation = toGeo;
                }
                catch { }
                NavigationService.Navigate(new Uri("/LocationSelectorPage.xaml?target=Origin", UriKind.Relative));
            }
            else if (sender == getGeoButton2)
            {
                (Application.Current as App).SelectedLocation = null;
                try
                {
                    GeoCoordinate toGeo = new GeoCoordinate();
                    toGeo.Latitude = Double.Parse(LatitudeBox2.Text);
                    toGeo.Longitude = Double.Parse(LongittudeBox2.Text);
                    (Application.Current as App).SelectedLocation = toGeo;
                }
                catch { }
                NavigationService.Navigate(new Uri("/LocationSelectorPage.xaml?target=Destination", UriKind.Relative));
            }
        }
    }
}