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


namespace RoutingAdvanced
{
    public partial class MainPage : PhoneApplicationPage
    {
        MapRoute LastRutte = null;

        RouteOptimization ROpti = RouteOptimization.MinimizeTime;
        TravelMode RTrav = TravelMode.Driving;

        RouteQuery geoQ = null;

        bool draggingNow = false;
        MapLayer markerLayer = null;
        MapOverlay selectedMarker = null;
        MapOverlay OriginMarker = null;
        MapOverlay DestinationMarker = null;

        ReverseGeocodeQuery geoRev = null;
        bool DestinationRevGeoNow = false;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            System.Windows.Input.Touch.FrameReported += Touch_FrameReported;

            map1.ZoomLevelChanged += map1_ZoomLevelChanged;

            AddResultToMap(new GeoCoordinate(60.17040395, 24.94121572), new GeoCoordinate(60.29143956, 24.96187636));

            geoRev = new ReverseGeocodeQuery();
            geoRev.QueryCompleted += geoRev_QueryCompleted;

            geoQ = new RouteQuery();
            geoQ.QueryCompleted += geoQ_QueryCompleted;
        }

        void Touch_FrameReported(object sender, TouchFrameEventArgs e)
        {
            if (draggingNow == true)
            {
                TouchPoint tp = e.GetPrimaryTouchPoint(map1);

                if (tp.Action == TouchAction.Move && (selectedMarker != null))
                {
                    selectedMarker.GeoCoordinate = map1.ConvertViewportPointToGeoCoordinate(tp.Position);
                    Start_ReverceGeoCoding(selectedMarker);
                }
                else if (tp.Action == TouchAction.Up)
                {
                    selectedMarker = null;
                    draggingNow = false;
                    map1.IsEnabled = true;
                }
            }
        }

        void map1_ZoomLevelChanged(object sender, MapZoomLevelChangedEventArgs e)
        {
            zoomSlider.Value = map1.ZoomLevel;
        }

        private void zoomSlider_ValueChanged_1(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (zoomSlider != null)
            {
                map1.ZoomLevel = zoomSlider.Value;
            }
        }

        private void AddResultToMap(GeoCoordinate origin, GeoCoordinate destination)
        {
            if (markerLayer != null)
            {
                map1.Layers.Remove(markerLayer);
                markerLayer = null;
            }

            OriginMarker = MakeDotMarker(origin, false);
            DestinationMarker = MakeDotMarker(destination, true);

            markerLayer = new MapLayer();
            map1.Layers.Add(markerLayer);
            markerLayer.Add(OriginMarker);
            markerLayer.Add(DestinationMarker);
        }

        private MapOverlay MakeDotMarker(GeoCoordinate location, bool isDestination)
        {
            MapOverlay Marker = new MapOverlay();

            Marker.GeoCoordinate = location;

            Ellipse Circhegraphic = new Ellipse();
            if (isDestination == true)
            {
                Circhegraphic.Fill = new SolidColorBrush(Colors.Green);
                Circhegraphic.Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Purple);
            }
            else
            {
                Circhegraphic.Fill = new SolidColorBrush(Colors.Yellow);
                Circhegraphic.Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Red);
            }

            Circhegraphic.StrokeThickness = 30;
            Circhegraphic.Opacity = 0.8;
            Circhegraphic.Height = 80;
            Circhegraphic.Width = 80;

            Marker.Content = Circhegraphic;

            Marker.PositionOrigin = new Point(0.5, 0.5);
            Circhegraphic.MouseLeftButtonDown += textt_MouseLeftButtonDown;

            return Marker;
        }


        void textt_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Ellipse clickedOne = sender as Ellipse;
            if (clickedOne != null && OriginMarker != null && DestinationMarker != null)
            {
                Debug.WriteLine("Circhegraphic_MouseLeftButtonUp");

                if (OriginMarker.Content == clickedOne)
                {
                    selectedMarker = OriginMarker;
                    draggingNow = true;
                    map1.IsEnabled = false;
                }
                else if (DestinationMarker.Content == clickedOne)
                {
                    selectedMarker = DestinationMarker;
                    draggingNow = true;
                    map1.IsEnabled = false;
                }
            }
        }

        void Start_ReverceGeoCoding(MapOverlay Marker)
        {
            if (geoRev.IsBusy != true && (Marker != null))
            {
                if (Marker == DestinationMarker)
                {
                    DestinationRevGeoNow = true;
                    DestinationTitle.Text = "";
                    GeoProgress2.IsEnabled = true;
                    GeoProgress2.IsIndeterminate = true;
                }
                else
                {
                    DestinationRevGeoNow = false;
                    OriginTitle.Text = "";
                    GeoProgress1.IsEnabled = true;
                    GeoProgress1.IsIndeterminate = true;
                }

                // Set the geo coordinate for the query
                geoRev.GeoCoordinate = Marker.GeoCoordinate;
                geoRev.QueryAsync();
                Debug.WriteLine("RevGeocodeAsync started for location: ");
            }
        }

        void geoRev_QueryCompleted(object sender, QueryCompletedEventArgs<IList<MapLocation>> e)
        {
            GeoProgress1.IsEnabled = false;
            GeoProgress1.IsIndeterminate = false;
            GeoProgress2.IsEnabled = false;
            GeoProgress2.IsIndeterminate = false;

            Debug.WriteLine("Geo query, error: " + e.Error);
            Debug.WriteLine("Geo query, cancelled: " + e.Cancelled);
            Debug.WriteLine("Geo query, cancelled: " + e.UserState.ToString());
            Debug.WriteLine("Geo query, Result.Count(): " + e.Result.Count());

            String GeoStuff = "";

            if (e.Result.Count() > 0)
            {
                if (e.Result[0].Information.Address.Street.Length > 0)
                {
                    GeoStuff = GeoStuff + e.Result[0].Information.Address.Street;

                    if (e.Result[0].Information.Address.HouseNumber.Length > 0)
                    {
                        GeoStuff = GeoStuff + " " + e.Result[0].Information.Address.HouseNumber;
                    }
                }

                if (e.Result[0].Information.Address.City.Length > 0)
                {
                    if (GeoStuff.Length > 0)
                    {
                        GeoStuff = GeoStuff + ",";
                    }

                    GeoStuff = GeoStuff + " " + e.Result[0].Information.Address.City;

                    if (e.Result[0].Information.Address.Country.Length > 0)
                    {
                        GeoStuff = GeoStuff + " " + e.Result[0].Information.Address.Country;
                    }
                }
                else if (e.Result[0].Information.Address.Country.Length > 0)
                {
                    if (GeoStuff.Length > 0)
                    {
                        GeoStuff = GeoStuff + ",";
                    }
                    GeoStuff = GeoStuff + " " + e.Result[0].Information.Address.Country;
                }
            }
            if (DestinationRevGeoNow == true)
            {
                DestinationTitle.Text = GeoStuff;
            }
            else
            {
                OriginTitle.Text = GeoStuff;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            if (sender == Hplus)
            {
                map1.Heading = (map1.Heading + 12);
            }
            else if (sender == Hmins)
            {
                map1.Heading = (map1.Heading - 12);
            }
            else if (sender == TrMod)
            {
                if (RTrav == TravelMode.Driving)
                {
                    RTrav = TravelMode.Walking;
                    TrMod.Content = "Walk";
                }
                else
                {
                    RTrav = TravelMode.Driving;
                    TrMod.Content = "Drive";
                }

            }
            else if (sender == Optim)
            {
                if (ROpti == RouteOptimization.MinimizeTime)
                {
                    ROpti = RouteOptimization.MinimizeDistance;
                    Optim.Content = "Shortest";
                }
                else
                {
                    ROpti = RouteOptimization.MinimizeTime;
                    Optim.Content = "Quickest";
                }
            }
            else if (sender == Rutte)
            {
                if (geoQ.IsBusy == true)
                {
                    geoQ.CancelAsync();
                }

                geoQ.InitialHeadingInDegrees = map1.Heading;

                geoQ.RouteOptimization = ROpti;
                geoQ.TravelMode = RTrav;

                List<GeoCoordinate> MyWayPoints = new List<GeoCoordinate>();
                MyWayPoints.Add(OriginMarker.GeoCoordinate);
                MyWayPoints.Add(DestinationMarker.GeoCoordinate);

                geoQ.Waypoints = MyWayPoints;
                geoQ.QueryAsync();
            }
        }

        void geoQ_QueryCompleted(object sender, QueryCompletedEventArgs<Route> e)
        {
            Debug.WriteLine("Route query, error: " + e.Error);
            Debug.WriteLine("Route query, cancelled: " + e.Cancelled);
            Debug.WriteLine("Route query, cancelled: " + e.UserState);

            if (LastRutte != null)
            {
                map1.RemoveRoute(LastRutte);
                LastRutte = null;
            }

            Route myRutte = e.Result;


            for (var i = 0; i < myRutte.Legs.Count(); i++)
            {
                // you could also access each leg separately, and add markers etc into the map for them.
            }
            LastRutte = new MapRoute(myRutte);

            map1.AddRoute(LastRutte);
            map1.SetView(e.Result.BoundingBox);

            MessageBox.Show("Distance: " + (myRutte.LengthInMeters / 1000) + " km, Estimated traveltime: " + myRutte.EstimatedDuration);
        }
    }
}