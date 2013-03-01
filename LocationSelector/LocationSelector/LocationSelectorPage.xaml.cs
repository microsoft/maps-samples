using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Maps.Controls;
using System.Diagnostics;
using System.Windows.Controls.Primitives;
using System.Device.Location;
using System.Collections.ObjectModel;
using Microsoft.Phone.Maps.Services;

namespace LocationSelector
{
    public partial class LocationSelectorPage : PhoneApplicationPage
    {
        GeoCoordinateWatcher watcher = null;
        GeoCoordinate myPosition = null;
        MapPolygon PolyCircle = null;

        bool isOrigin = false;
        bool draggingNow = false;
        MapLayer markerLayer = null;
        GeocodeQuery geoQ = null;
        MapOverlay oneMarker = null;

        IList<MapLocation> GeoResuls = null;

        // Constructor
        public LocationSelectorPage()
        {
            InitializeComponent();

            markerLayer = new MapLayer();
            map1.Layers.Add(markerLayer);

            geoQ = new GeocodeQuery();
            geoQ.QueryCompleted += geoQ_QueryCompleted;
            Debug.WriteLine("All construction done for GeoCoding");

            System.Windows.Input.Touch.FrameReported += Touch_FrameReported;

            map1.Tap += map1_Tap;
            map1.ZoomLevelChanged += map1_ZoomLevelChanged;

            resultList.SelectionChanged += resultList_SelectionChanged;

            watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.Default);
            watcher.MovementThreshold = 20; // 20 meters

            watcher.StatusChanged += new EventHandler<GeoPositionStatusChangedEventArgs>(OnStatusChanged);
            watcher.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(OnPositionChanged);

            watcher.Start();
        }

        void resultList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender == resultList && (oneMarker != null))
            {
                int indexx = resultList.SelectedIndex;
                if (indexx >= 0 && indexx < GeoResuls.Count())
                {
                    oneMarker.GeoCoordinate = GeoResuls[indexx].GeoCoordinate;
                    map1.Center = oneMarker.GeoCoordinate;
                }
            }
        }

        void map1_ZoomLevelChanged(object sender, MapZoomLevelChangedEventArgs e)
        {
            zoomSlider.Value = map1.ZoomLevel;
        }

        void map1_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (oneMarker != null)
            {
                Point markPoint = map1.ConvertGeoCoordinateToViewportPoint(oneMarker.GeoCoordinate);

                if ((markPoint.X < 0 || markPoint.Y < 0)
                || (map1.ActualWidth < markPoint.X) || (map1.ActualHeight < markPoint.Y))
                {
                    // tap event when we do not have the marker visible, so lets move it here
                    oneMarker.GeoCoordinate = map1.ConvertViewportPointToGeoCoordinate(e.GetPosition(map1));
                }
            }
        }

        void Touch_FrameReported(object sender, TouchFrameEventArgs e)
        {
            if (draggingNow == true)
            {
                TouchPoint tp = e.GetPrimaryTouchPoint(map1);

                if (tp.Action == TouchAction.Move)
                {
                    if (oneMarker != null)
                    {
                        oneMarker.GeoCoordinate = map1.ConvertViewportPointToGeoCoordinate(tp.Position);
                    }
                }
                else if (tp.Action == TouchAction.Up)
                {
                    draggingNow = false;
                    map1.IsEnabled = true;
                }
            }
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            string target = "";

            if (this.NavigationContext.QueryString.ContainsKey("target"))
            {
                target = this.NavigationContext.QueryString["target"];
            }

            Debug.WriteLine("OnNavigatedTo, target: " + target);

            base.OnNavigatedTo(e);

            TitleBox.Text = "Select " + target;

            if (target.Length > 0 && (target == "Origin"))
            {
                isOrigin = true;
                if ((Application.Current as App).RouteOriginLocation == null)
                {
                    AddResultToMap(map1.Center);
                }
                else
                {
                    AddResultToMap((Application.Current as App).RouteOriginLocation);
                }
            }
            else
            {
                isOrigin = false;
                if ((Application.Current as App).SelectedLocation == null)
                {
                    AddResultToMap(map1.Center);
                }
                else
                {
                    AddResultToMap((Application.Current as App).SelectedLocation);
                }
            }

            zoomSlider.Value = map1.ZoomLevel;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (OkBut == sender)
            {
                if (isOrigin)
                {
                    (Application.Current as App).RouteOriginLocation = oneMarker.GeoCoordinate;
                }
                else
                {
                    (Application.Current as App).SelectedLocation = oneMarker.GeoCoordinate;
                }
            }

            this.NavigationService.GoBack();
        }


        private void GeoButton_Click(object sender, RoutedEventArgs e)
        {
            if (geoBox.Text.Length > 0)
            {
                if (geoQ.IsBusy == true)
                {
                    geoQ.CancelAsync();
                }

                GeoProgress.IsEnabled = true;
                GeoProgress.IsIndeterminate = true;

                resultList.Visibility = System.Windows.Visibility.Collapsed;

                geoQ.GeoCoordinate = map1.Center;
                geoQ.SearchTerm = geoBox.Text;
                geoQ.MaxResultCount = 20;

                geoQ.QueryAsync();
                Debug.WriteLine("GeocodeAsync started for: " + geoBox.Text);
            }
            else
            {
                MessageBox.Show("Please input address, and then press again");
            }
        }

        void geoQ_QueryCompleted(object sender, QueryCompletedEventArgs<IList<MapLocation>> e)
        {
            GeoProgress.IsEnabled = false;
            GeoProgress.IsIndeterminate = false;

            Debug.WriteLine("Geo query, error: " + e.Error);
            Debug.WriteLine("Geo query, cancelled: " + e.Cancelled);
            Debug.WriteLine("Geo query, cancelled: " + e.UserState.ToString());
            Debug.WriteLine("Geo query, Result.Count(): " + e.Result.Count());

            if (e.Result.Count() > 0)
            {
                GeoResuls = e.Result;

                List<String> source = new List<String>();

                for (int i = 0; i < GeoResuls.Count(); i++)
                {

                    if (GeoResuls[i].Information.Name.Length > 0)
                    {
                        source.Add(GeoResuls[i].Information.Name);
                    }
                    else if (GeoResuls[i].Information.Description.Length > 0)
                    {
                        source.Add(GeoResuls[i].Information.Description);
                    }
                    else
                    {
                        String GeoStuff = "";

                        if (GeoResuls[i].Information.Address.Street.Length > 0)
                        {
                            GeoStuff = GeoStuff + GeoResuls[i].Information.Address.Street;

                            if (GeoResuls[i].Information.Address.HouseNumber.Length > 0)
                            {
                                GeoStuff = GeoStuff + " " + GeoResuls[i].Information.Address.HouseNumber;
                            }
                        }

                        if (GeoResuls[i].Information.Address.City.Length > 0)
                        {
                            if (GeoStuff.Length > 0)
                            {
                                GeoStuff = GeoStuff + ",";
                            }

                            GeoStuff = GeoStuff + " " + GeoResuls[i].Information.Address.City;

                            if (GeoResuls[i].Information.Address.Country.Length > 0)
                            {
                                GeoStuff = GeoStuff + " " + GeoResuls[i].Information.Address.Country;
                            }
                        }
                        else if (GeoResuls[i].Information.Address.Country.Length > 0)
                        {
                            if (GeoStuff.Length > 0)
                            {
                                GeoStuff = GeoStuff + ",";
                            }
                            GeoStuff = GeoStuff + " " + GeoResuls[i].Information.Address.Country;
                        }

                        source.Add(GeoStuff);
                    }
                }
                resultList.Visibility = System.Windows.Visibility.Visible;
                resultList.ItemsSource = source;
                resultList.SelectedIndex = 0;

                oneMarker.GeoCoordinate = GeoResuls[0].GeoCoordinate;
                map1.Center = oneMarker.GeoCoordinate;

            }
            else
            {
                MessageBox.Show("No results found.");
            }
        }

        void textt_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("textt_MouseLeftButtonDown");
            if (oneMarker != null)
            {
                draggingNow = true;
                map1.IsEnabled = false;
            }
        }


        private void zoomSlider_ValueChanged_1(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (zoomSlider != null)
            {
                map1.ZoomLevel = zoomSlider.Value;
            }
        }

        private void AddResultToMap(GeoCoordinate location)
        {
            if (markerLayer != null)
            {
                map1.Layers.Remove(markerLayer);
                markerLayer = null;
            }

            markerLayer = new MapLayer();
            map1.Layers.Add(markerLayer);

            oneMarker = new MapOverlay();
            oneMarker.GeoCoordinate = location;

            Ellipse Circhegraphic = new Ellipse();
            Circhegraphic.Fill = new SolidColorBrush(Colors.Yellow);
            Circhegraphic.Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Red);
            Circhegraphic.StrokeThickness = 30;
            Circhegraphic.Opacity = 0.8;
            Circhegraphic.Height = 80;
            Circhegraphic.Width = 80;

            oneMarker.Content = Circhegraphic;

            oneMarker.PositionOrigin = new Point(0.5, 0.5);
            Circhegraphic.MouseLeftButtonDown += textt_MouseLeftButtonDown;

            markerLayer.Add(oneMarker);

            map1.Center = oneMarker.GeoCoordinate;
        }

        void OnPositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            myPosition = e.Position.Location;
            MylocDot.Fill = new SolidColorBrush(Colors.Green);
            UpDateMyPositionCircle();
        }

        void OnStatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            switch (e.Status)
            {
                case GeoPositionStatus.Ready:
                    break;
                case GeoPositionStatus.Disabled:
                case GeoPositionStatus.Initializing:
                case GeoPositionStatus.NoData:
                default:
                    {
                        myPosition = null;
                        MylocDot.Fill = new SolidColorBrush(Colors.Gray);
                        UpDateMyPositionCircle();
                    }
                    break;
            }
        }

        void UpDateMyPositionCircle()
        {
            if (myPosition != null)
            {
                double accuracy = myPosition.HorizontalAccuracy;

                if (accuracy < myPosition.VerticalAccuracy)
                {
                    accuracy = myPosition.VerticalAccuracy;
                }

                if (PolyCircle == null)
                {
                    PolyCircle = new MapPolygon();

                    PolyCircle.FillColor = Color.FromArgb(0x55, 0x00, 0xFF, 0x00);
                    PolyCircle.StrokeColor = Color.FromArgb(0xFF, 0x00, 0xFF, 0x00);
                    PolyCircle.StrokeThickness = 1;

                    map1.MapElements.Add(PolyCircle);
                }

                if (accuracy < 50)
                {
                    accuracy = 50; // to be able to show the polygon
                }

                PolyCircle.Path = CreateCircle(myPosition, accuracy);
            }
            else if (PolyCircle != null)
            {
                map1.MapElements.Remove(PolyCircle);
                PolyCircle = null;
            }
        }

        public static double ToRadian(double degrees)
        {
            return degrees * (Math.PI / 180);
        }

        public static double ToDegrees(double radians)
        {
            return radians * (180 / Math.PI);
        }

        public static GeoCoordinateCollection CreateCircle(GeoCoordinate center, double radius)
        {
            var earthRadius = 6367000; // radius in meters
            var lat = ToRadian(center.Latitude); //radians
            var lng = ToRadian(center.Longitude); //radians
            var d = radius / earthRadius; // d = angular distance covered on earth's surface
            var locations = new GeoCoordinateCollection();

            for (var x = 0; x <= 360; x++)
            {
                var brng = ToRadian(x);
                var latRadians = Math.Asin(Math.Sin(lat) * Math.Cos(d) + Math.Cos(lat) * Math.Sin(d) * Math.Cos(brng));
                var lngRadians = lng + Math.Atan2(Math.Sin(brng) * Math.Sin(d) * Math.Cos(lat), Math.Cos(d) - Math.Sin(lat) * Math.Sin(latRadians));

                locations.Add(new GeoCoordinate(ToDegrees(latRadians), ToDegrees(lngRadians)));
            }

            return locations;
        }

        void myLocation_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (myPosition != null)
            {
                map1.Center = myPosition;
            }
        }
    }
}