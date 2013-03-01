using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using AreaSelector.Resources;

using System.Device.Location;

namespace AreaSelector
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if ((Application.Current as App).SelectedLocation != null)
            {
                LatitudeBox.Text = (Application.Current as App).SelectedLocation.Latitude.ToString();
                LongittudeBox.Text = (Application.Current as App).SelectedLocation.Longitude.ToString();

                (Application.Current as App).SelectedLocation = null;
            }

            if ((Application.Current as App).CircleAreaRadius > 0)
            {
                StringBox.Text = (Application.Current as App).CircleAreaRadius.ToString();
            }
        }

        private void Button_gridbut_Click(object sender, RoutedEventArgs e)
        {
             if (sender == getGeoButton)
            {
                (Application.Current as App).SelectedLocation = null;
                try
                {
                    GeoCoordinate toGeo = new GeoCoordinate();
                    toGeo.Latitude = Double.Parse(LatitudeBox.Text);
                    toGeo.Longitude = Double.Parse(LongittudeBox.Text);
                    (Application.Current as App).SelectedLocation = toGeo;

                    (Application.Current as App).CircleAreaRadius = int.Parse(StringBox.Text);
                }
                catch { }
                NavigationService.Navigate(new Uri("/AreaSelectorPage.xaml?target=Location", UriKind.Relative));
            }
        }
    }
}