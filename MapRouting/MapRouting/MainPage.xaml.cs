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
using MapRouting.Resources;
using Microsoft.Phone.Maps.Controls;
using Microsoft.Phone.Maps.Services;
using System.Diagnostics;
using System.Windows.Controls.Primitives;
using System.Device.Location;
using System.Collections.ObjectModel;

namespace MapRouting
{
    public partial class MainPage : PhoneApplicationPage
    {
        MapRoute LastRutte = null; 
        MapLayer markerLayer = null;
        MapOverlay startMark = null;
        MapOverlay enddMark = null;

        RouteOptimization ROpti = RouteOptimization.MinimizeTime;
        TravelMode RTrav = TravelMode.Driving;

        RouteQuery geoQ = null;
        
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            markerLayer = new MapLayer();

            startMark = AddMarkersToMap("Start", new GeoCoordinate(60.2214, 24.7572));
            markerLayer.Add(startMark);

            enddMark = AddMarkersToMap("End", new GeoCoordinate(61.4468, 23.8646));
            markerLayer.Add(enddMark);

            map1.Layers.Add(markerLayer);

            geoQ = new RouteQuery();
            geoQ.QueryCompleted += geoQ_QueryCompleted;

            Debug.WriteLine("All construction done for rutting");
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
                if (geoQ.IsBusy == true){
                    geoQ.CancelAsync();
                }

                geoQ.InitialHeadingInDegrees = map1.Heading;

                geoQ.RouteOptimization = ROpti;
                geoQ.TravelMode = RTrav;

                List<GeoCoordinate> MyWayPoints = new List<GeoCoordinate>();
                MyWayPoints.Add(startMark.GeoCoordinate);
                MyWayPoints.Add(enddMark.GeoCoordinate);

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
            LastRutte = new MapRoute(myRutte);

            // myRutte.Legs.Count();

            map1.AddRoute(LastRutte);
            map1.SetView(e.Result.BoundingBox);
        }

        private MapOverlay AddMarkersToMap(String text, GeoCoordinate location)
        {
            MapOverlay oneMarker = new MapOverlay();
            oneMarker.GeoCoordinate = location;

            Canvas canCan = new Canvas();

            Ellipse Circhegraphic = new Ellipse();
            Circhegraphic.Fill = new SolidColorBrush(Colors.Brown);
            Circhegraphic.Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Green);
            Circhegraphic.StrokeThickness = 5;
            Circhegraphic.Opacity = 0.8;
            Circhegraphic.Height = 40;
            Circhegraphic.Width = 60;

            canCan.Children.Add(Circhegraphic);
            TextBlock textt = new TextBlock { Text = text };
            textt.HorizontalAlignment = HorizontalAlignment.Center;
            Canvas.SetLeft(textt, 10);
            Canvas.SetTop(textt, 5);
            Canvas.SetZIndex(textt, 5);

            canCan.Children.Add(textt);
            oneMarker.Content = canCan;

            oneMarker.PositionOrigin = new Point(0.5, 0.5);
            textt.MouseLeftButtonUp += textt_MouseLeftButtonUp;
            textt.MouseLeftButtonDown += textt_MouseLeftButtonDown;
            textt.MouseMove += textt_MouseMove;

            return oneMarker;
        }

        void textt_MouseMove(object sender, MouseEventArgs e)
        {
            TextBlock blockt = sender as TextBlock;
            if (blockt != null){
                Debug.WriteLine("textt_MouseMove: " + blockt.Text);
            }
        }

        void textt_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock blockt = sender as TextBlock;
            if (blockt != null)
            {
                Debug.WriteLine("textt_MouseLeftButtonDown: " + blockt.Text);
            }
        }

        void textt_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            TextBlock blockt = sender as TextBlock;
            if (blockt != null)
            {
                Debug.WriteLine("textt_MouseLeftButtonUp: " + blockt.Text);
            }
        }
    }
}