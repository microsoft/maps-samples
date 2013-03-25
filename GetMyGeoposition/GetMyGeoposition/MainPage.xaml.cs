using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using GetMyGeoposition.Resources;

using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace GetMyGeoposition
{
    public partial class MainPage : PhoneApplicationPage
    {
        Geolocator geolocator = null;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            geolocator = new Geolocator();
            geolocator.DesiredAccuracyInMeters = 50;
        }

        void geolocator_PositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {
            throw new NotImplementedException();
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            latitudeBox.Text = "";
            longitudeBox.Text = "";
            accurazyBox.Text = "";
            altitudeBox.Text = "";
            headingBox.Text = "";

            try
            {
                Geoposition geoposition = await geolocator.GetGeopositionAsync(
                maximumAge: TimeSpan.FromMinutes(5),
                timeout: TimeSpan.FromSeconds(10)
                );

                latitudeBox.Text = geoposition.Coordinate.Latitude.ToString("0.00");
                longitudeBox.Text = geoposition.Coordinate.Longitude.ToString("0.00");
                accurazyBox.Text = geoposition.Coordinate.Accuracy.ToString("0.00");
                altitudeBox.Text = geoposition.Coordinate.Altitude.ToString();
                headingBox.Text = geoposition.Coordinate.Heading.ToString();
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("location  is disabled in phone settings.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Got Error: " + ex.Message);
            }
        }
    }
}