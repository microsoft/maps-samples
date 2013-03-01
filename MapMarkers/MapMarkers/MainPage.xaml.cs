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
using MapMarkers.Resources;
using Microsoft.Phone.Maps.Controls;
using System.Diagnostics;
using System.Windows.Controls.Primitives;
using System.Device.Location;
using System.Collections.ObjectModel;


namespace MapMarkers
{
    public partial class MainPage : PhoneApplicationPage
    {
        bool draggingNow = false;
        String selected_shape = "";
        MapLayer oneMarkerLayer = null;
        MapOverlay oneMarker = null;
        MapLayer markerLayer = null;
        TextBlock MarkerTxt = null;
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            Debug.WriteLine("We are started"); //Check to see if the "Redirect all Output Window text to the Immediate Window" is checked under Tools -> Options -> Debugging -> General.  
            System.Windows.Input.Touch.FrameReported += Touch_FrameReported;

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

        private void Button_Menubut_Click(object sender, RoutedEventArgs e)
        {
            if (sender == CanCelMenuBut)
            {
                MenuSelectionGrip.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                if (sender == OnemarkerBut)
                {
                   selected_shape = "One marker";
                }
                else if (sender == MarkersBut)
                {
                    selected_shape = "5 Markers";
                }

                MenuSelectionLiat.SelectedIndex = 0;
                MenuSelectionGrip.Visibility = System.Windows.Visibility.Visible;
            }
        }


        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("We are loaded");
        }

        void SelectionChangedEventHandler(Object sender, SelectionChangedEventArgs e)
        {
            ListBox BOXX = (sender as ListBox);
            String sellected = (BOXX.SelectedItem as TextBlock).Text;

            Debug.WriteLine("ListBox1SelectedIndexChanged " + sellected);

            if (sellected == "Add")
            {
                AddItem();
            }
            else if (sellected == "Remove")
            {
                RevomeItem(false);
            }
            else if (sellected == "Fit to View")
            {
                FitToView(false);
            }
            else if (sellected == "Fit All to View")
            {
                FitToView(true);
            }
            else if (sellected == "Remove one marker")
            {
                RevomeItem(true);
            }
            else if (sellected == "Change text")
            {
                if (MarkerTxt != null)
                {
                    Debug.WriteLine("change text now from " + MarkerTxt.Text);
                    if (MarkerTxt.Text == "Drag")
                    {
                        MarkerTxt.Text = "1234";
                    }
                    else
                    {
                        MarkerTxt.Text = "Drag";
                    }
                }
            }
        }



        void AddItem()
        {
            if (selected_shape == "5 Markers")
            {
                if (markerLayer == null)
                {
                    markerLayer = new MapLayer();

                    MapOverlay pin1 = new MapOverlay();
                    
                    RichTextBox boxx = new RichTextBox();
                    Ellipse circl00 = new Ellipse();
                    circl00.Fill = new SolidColorBrush(Colors.Yellow);
                    circl00.Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Red);
                    circl00.StrokeThickness = 10;
                    circl00.Opacity = 0.8;
                    circl00.Height = 30;
                    circl00.Width = 30;

                    pin1.Content = circl00; 

                    pin1.GeoCoordinate = new GeoCoordinate(60.27, 24.80);
                    pin1.PositionOrigin = new Point(0.5, 0.5);

                    MapOverlay pin2 = new MapOverlay();
                    Ellipse circl01 = new Ellipse();
                    circl01.Fill = new SolidColorBrush(Colors.Yellow);
                    circl01.Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Red);
                    circl01.StrokeThickness = 10;
                    circl01.Opacity = 0.8;
                    circl01.Height = 30;
                    circl01.Width = 30;

                    pin2.Content = circl01; 
                    pin2.GeoCoordinate = new GeoCoordinate(60.22, 24.60);
                    pin2.PositionOrigin = new Point(0.5, 0.5);

                    MapOverlay pin3 = new MapOverlay();
                    Ellipse Circhegraphic = new Ellipse();
                    Circhegraphic.Fill = new SolidColorBrush(Colors.Yellow);
                    Circhegraphic.Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Red);
                    Circhegraphic.StrokeThickness = 10;
                    Circhegraphic.Opacity = 0.8;
                    Circhegraphic.Height = 30;
                    Circhegraphic.Width = 30;

                    pin3.Content = Circhegraphic; 
                    pin3.GeoCoordinate = new GeoCoordinate(60.27, 24.70);
                    pin3.PositionOrigin = new Point(0.5, 0.5);

                    MapOverlay pin4 = new MapOverlay();
                    Ellipse Cirche2 = new Ellipse();
                    Cirche2.Fill = new SolidColorBrush(Colors.Yellow);
                    Cirche2.Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Red);
                    Cirche2.StrokeThickness = 10;
                    Cirche2.Opacity = 0.8;
                    Cirche2.Height = 30;
                    Cirche2.Width = 30;

                    pin4.Content = Cirche2; 
                    pin4.GeoCoordinate = new GeoCoordinate(60.27, 24.65);
                    pin4.PositionOrigin = new Point(0.5, 0.5);

                    MapOverlay pin5 = new MapOverlay();
                    Ellipse Cirche3 = new Ellipse();
                    Cirche3.Fill = new SolidColorBrush(Colors.Yellow);
                    Cirche3.Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Red);
                    Cirche3.StrokeThickness = 10;
                    Cirche3.Opacity = 0.8;
                    Cirche3.Height = 30;
                    Cirche3.Width = 30;

                    pin5.Content = Cirche3; 
                    pin5.GeoCoordinate = new GeoCoordinate(60.27, 24.60);
                    pin5.PositionOrigin = new Point(0.5, 0.5);

                    markerLayer.Add(pin1);
                    markerLayer.Add(pin2);
                    markerLayer.Add(pin3);
                    markerLayer.Add(pin4);
                    markerLayer.Add(pin5);
                    map1.Layers.Add(markerLayer);
                }
            }
            else if (selected_shape == "One marker")
            {
                if (oneMarker == null)
                {
                    oneMarkerLayer = new MapLayer();
                    oneMarker = new MapOverlay();

                    Canvas canCan = new Canvas();

                    Ellipse Circhegraphic = new Ellipse();
                    Circhegraphic.Fill = new SolidColorBrush(Colors.Brown);
                    Circhegraphic.Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Green);
                    Circhegraphic.StrokeThickness = 5;
                    Circhegraphic.Opacity = 0.8;
                    Circhegraphic.Height = 40;
                    Circhegraphic.Width = 60;

                    canCan.Children.Add(Circhegraphic);
                    MarkerTxt = new TextBlock { Text = "Drag" };
                    MarkerTxt.HorizontalAlignment = HorizontalAlignment.Center;
                    Canvas.SetLeft(MarkerTxt, 10);
                    Canvas.SetTop(MarkerTxt, 5);
                    Canvas.SetZIndex(MarkerTxt, 5);

                    canCan.Children.Add(MarkerTxt);
                    oneMarker.Content = canCan;

                    oneMarker.PositionOrigin = new Point(0.5, 0.5);
                    oneMarker.GeoCoordinate = new GeoCoordinate(60.35, 24.60);
                    MarkerTxt.MouseLeftButtonDown += Circh_MouseLeftButtonDown;
 
              
                    oneMarkerLayer.Add(oneMarker);
                    map1.Layers.Add(oneMarkerLayer);
                }

            }
        }

        void Circh_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("oneMarker_MouseLeftButtonDown");
            if (oneMarker != null)
            {
                draggingNow = true;
                map1.IsEnabled = false;
            }
        }

        void FitToView(bool all)
        {

            bool gotRect = false;
            double north = 0;
            double west = 0;
            double south = 0;
            double east = 0;

            if (all) // fit all
            {
                if ((oneMarker != null))
                {
                    gotRect = true;
                    north = south = oneMarker.GeoCoordinate.Latitude;
                    west = east = oneMarker.GeoCoordinate.Longitude;
                }

                if ((markerLayer != null)) // fit all
                {
                    for(var p=0; p < markerLayer.Count(); p++){
                        if (!gotRect)
                        {
                            gotRect = true;
                            north = south = markerLayer[p].GeoCoordinate.Latitude;
                            west = east = markerLayer[p].GeoCoordinate.Longitude;
                        }
                        else
                        {
                            if (north < markerLayer[p].GeoCoordinate.Latitude) north = markerLayer[p].GeoCoordinate.Latitude;
                            if (west > markerLayer[p].GeoCoordinate.Longitude) west = markerLayer[p].GeoCoordinate.Longitude;
                            if (south > markerLayer[p].GeoCoordinate.Latitude) south = markerLayer[p].GeoCoordinate.Latitude;
                            if (east < markerLayer[p].GeoCoordinate.Longitude) east = markerLayer[p].GeoCoordinate.Longitude;
                        }
                    }
                }
            }
            else if (selected_shape == "5 Markers" && (markerLayer != null))
            {
                for (var p = 0; p < markerLayer.Count(); p++)
                {
                    if (!gotRect)
                    {
                        gotRect = true;
                        north = south = markerLayer[p].GeoCoordinate.Latitude;
                        west = east = markerLayer[p].GeoCoordinate.Longitude;
                    }
                    else
                    {
                        if (north < markerLayer[p].GeoCoordinate.Latitude) north = markerLayer[p].GeoCoordinate.Latitude;
                        if (west > markerLayer[p].GeoCoordinate.Longitude) west = markerLayer[p].GeoCoordinate.Longitude;
                        if (south > markerLayer[p].GeoCoordinate.Latitude) south = markerLayer[p].GeoCoordinate.Latitude;
                        if (east < markerLayer[p].GeoCoordinate.Longitude) east = markerLayer[p].GeoCoordinate.Longitude;
                    }
                }
            }
            else if (selected_shape == "One marker" && (oneMarker != null))
            {
                gotRect = true;
                north = south = oneMarker.GeoCoordinate.Latitude;
                west = east = oneMarker.GeoCoordinate.Longitude;
            }


            if (gotRect)
            {
                map1.SetView(new LocationRectangle(north, west, south, east));
            }
        }

        void RevomeItem(bool justOne)
        {
            if (justOne)
            {
                if (markerLayer != null)
                {
                    if (markerLayer.Count > 1)
                    {
                        markerLayer.RemoveAt(0);
                    }
                    else
                    {
                        map1.Layers.Remove(markerLayer);
                        markerLayer = null;
                    }
                }

            }
            else if (selected_shape == "5 Markers")
            {
                if (markerLayer != null)
                {
                    map1.Layers.Remove(markerLayer);
                    markerLayer = null;
                }
            }
            else if (selected_shape == "One marker")
            {
                if (oneMarker != null)
                {
                    map1.Layers.Remove(oneMarkerLayer);
                    oneMarker = null;
                    oneMarkerLayer = null;
                    MarkerTxt = null;
                }
            }
        }
    }
}