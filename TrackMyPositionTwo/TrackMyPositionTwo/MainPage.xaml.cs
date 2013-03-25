
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using TrackMyPositionTwo.Resources;

using System.Device.Location;
using System.Windows.Threading;


namespace TrackMyPositionTwo
{
    public partial class MainPage : PhoneApplicationPage
    {
        GeoCoordinateWatcher geolocator = null;
        bool tracking = false;
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            geolocator = new GeoCoordinateWatcher(GeoPositionAccuracy.Default);
            geolocator.MovementThreshold = 20; // 20 meters
            geolocator.StatusChanged +=geolocator_StatusChanged;
            geolocator.PositionChanged +=geolocator_PositionChanged;

        }

        void geolocator_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> args)
        {
            latitudeBox.Text = args.Position.Location.Latitude.ToString("0.00");
            longitudeBox.Text = args.Position.Location.Longitude.ToString("0.00");

            double accuracy = args.Position.Location.HorizontalAccuracy;

            if (accuracy < args.Position.Location.VerticalAccuracy)
            {
                accuracy = args.Position.Location.VerticalAccuracy;
            }

            accurazyBox.Text = accuracy.ToString("0.00");
            altitudeBox.Text = args.Position.Location.Altitude.ToString();
            headingBox.Text = args.Position.Location.Course.ToString();
        }

        void geolocator_StatusChanged(object sender, GeoPositionStatusChangedEventArgs args)
        {
            string status = "";

            switch (args.Status)
            {
                case GeoPositionStatus.Disabled:
                    // the application does not have the right capability or the location master switch is off
                    status = "Disabled";
                    break;
                case GeoPositionStatus.Initializing:
                    // the geolocator started the tracking operation
                    status = "initializing";
                    break;
                case GeoPositionStatus.NoData:
                    // the location service was not able to acquire the location
                    status = "no data";
                    break;
                case GeoPositionStatus.Ready:
                    // the location service is generating geopositions as specified by the tracking parameters
                    status = "Ready";
                    break;
                default:
                    status = "N/A";
                    break;
            }

            statusBox.Text = status;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!tracking)
                {
                    statusBox.Text = "Started";
                    geolocator.Start();

                    tracking = true;
                    StarStopBut.Content = "Stop tracking";
                }
                else
                {
                    statusBox.Text = "Stopped";
                    geolocator.Stop();
                    tracking = false;
                    StarStopBut.Content = "Start tracking";
                    
                }   
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