using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using TrackMyPosition.Resources;

using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace TrackMyPosition
{
    public partial class MainPage : PhoneApplicationPage
    {
        Geolocator geolocator = null;
        bool tracking = false;
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        void geolocator_PositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {
            Dispatcher.BeginInvoke(() =>
            {
                latitudeBox.Text = args.Position.Coordinate.Latitude.ToString("0.00");
                longitudeBox.Text = args.Position.Coordinate.Longitude.ToString("0.00");
                accurazyBox.Text = args.Position.Coordinate.Accuracy.ToString("0.00");
                altitudeBox.Text = args.Position.Coordinate.Altitude.ToString();
                headingBox.Text = args.Position.Coordinate.Heading.ToString();
            });   
        }

        void geolocator_StatusChanged(Geolocator sender, StatusChangedEventArgs args)
        {
            string status = "";

            switch (args.Status)
            {
                case PositionStatus.Disabled:
                    // the application does not have the right capability or the location master switch is off
                    status = "Disabled";
                    break;
                case PositionStatus.Initializing:
                    // the geolocator started the tracking operation
                    status = "initializing";
                    break;
                case PositionStatus.NoData:
                    // the location service was not able to acquire the location
                    status = "no data";
                    break;
                case PositionStatus.Ready:
                    // the location service is generating geopositions as specified by the tracking parameters
                    status = "Ready";
                    break;
                case PositionStatus.NotAvailable:
                    status = "NA";
                    // not used in WindowsPhone, Windows desktop uses this value to signal that there is no hardware capable to acquire location information
                    break;
                case PositionStatus.NotInitialized:
                    // the initial state of the geolocator, once the tracking operation is stopped by the user the geolocator moves back to this state
                    status = "NI";
                    break;
            }

            Dispatcher.BeginInvoke(() =>
            {
                statusBox.Text = status;
            });   
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!tracking)
                {
                    geolocator = new Geolocator();
                    geolocator.DesiredAccuracy = PositionAccuracy.High;
                    geolocator.MovementThreshold = 100; // The units are meters.
                    geolocator.PositionChanged += geolocator_PositionChanged;
                    geolocator.StatusChanged += geolocator_StatusChanged;

                    tracking = true;
                    StarStopBut.Content = "Stop tracking";
                }
                else
                {
                    geolocator.StatusChanged -= geolocator_StatusChanged;
                    geolocator.PositionChanged -= geolocator_PositionChanged;
                    geolocator = null;

                    tracking = false;
                    statusBox.Text = "";
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