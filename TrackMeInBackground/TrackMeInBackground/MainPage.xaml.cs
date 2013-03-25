
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using TrackMeInBackground.Resources;
using System.Collections.ObjectModel;
using System.Windows.Shapes;
using System.Windows.Media;

using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Microsoft.Phone.Maps.Controls;
using System.Device.Location;


namespace TrackMeInBackground
{
    public partial class MainPage : PhoneApplicationPage
    {
        int ForeLocationCount = 0;
        MapOverlay oneMarker = null;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            App.PositionUpdated += App_PositionUpdated;
        }

        void App_PositionUpdated(object sender, EventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                ForeLocationCount++;

                if(oneMarker == null){
                    oneMarker = new MapOverlay();
                    MapLayer oneMarkerLayer = new MapLayer();

                    Ellipse Circhegraphic = new Ellipse();
                    Circhegraphic.Fill = new SolidColorBrush(Colors.Yellow);
                    Circhegraphic.Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Red);
                    Circhegraphic.StrokeThickness = 10;
                    Circhegraphic.Opacity = 0.8;
                    Circhegraphic.Height = 30;
                    Circhegraphic.Width = 30;

                    oneMarker.Content = Circhegraphic;
                    oneMarker.PositionOrigin = new Point(0.5, 0.5);
       
                    oneMarkerLayer.Add(oneMarker);
                    map1.Layers.Add(oneMarkerLayer);
                }

                oneMarker.GeoCoordinate = App.lastLocation;

                if (!App.RunningInBackground)
                {
                    map1.Center = oneMarker.GeoCoordinate;
                }

                statusBox.Text = "Count :" + ForeLocationCount  + "/"+ App.GottenLocationsCunt + ", sess: " + App.RunningInBackgroundCunt;
            });   
        }

        

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if ((Application.Current as App).ToggleTracking())
            {
                StarStopBut.Content = "Stop tracking";
            }
            else
            {
                StarStopBut.Content = "Start tracking";
            }
        }
    }
}