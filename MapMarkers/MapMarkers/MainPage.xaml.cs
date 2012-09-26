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
        Popup PopupP;
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
            AddSelectionPopUp();

            PopupP = new Popup();

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

        private void AddSelectionPopUp()
        {
            Popup SelectionPopupP = new Popup();


            StackPanel horpanel = new StackPanel();
            horpanel.Orientation = System.Windows.Controls.Orientation.Horizontal;
            horpanel.Background = new SolidColorBrush(Colors.Black);
            horpanel.Opacity = 0.7;

            Button buttonPolygon = new Button();
            buttonPolygon.Content = "One marker";
            buttonPolygon.Margin = new Thickness(5.0);
            buttonPolygon.Click += new RoutedEventHandler(Button_marker_Click);

            Button buttonRectangle = new Button();
            buttonRectangle.Content = "5 Markers";
            buttonRectangle.Margin = new Thickness(5.0);
            buttonRectangle.Click += new RoutedEventHandler(Button_markers_Click);

            horpanel.Children.Add(buttonPolygon);
            horpanel.Children.Add(buttonRectangle);
            SelectionPopupP.Child = horpanel;

            // Set where the popup will show up on the screen.
            SelectionPopupP.VerticalOffset = 30;
            SelectionPopupP.HorizontalOffset = 20;

            // Open the popup.
            SelectionPopupP.IsOpen = true;

        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("We are loaded");
        }

        private void Button_marker_Click(object sender, RoutedEventArgs e)
        {
            selected_shape = "One marker";
            OpenPopUpSelection(true);
        }

        private void Button_markers_Click(object sender, RoutedEventArgs e)
        {
            selected_shape = "5 Markers";
            OpenPopUpSelection(false);
        }

        private void OpenPopUpSelection(bool oneMarker)
        {
            // Create some content to show in the popup. Typically you would 
            // create a user control.
            Border border = new Border();
            border.BorderBrush = new SolidColorBrush(Colors.Black);
            border.BorderThickness = new Thickness(5.0);


            StackPanel panel1 = new StackPanel();
            panel1.Background = new SolidColorBrush(Colors.Black);
            panel1.Opacity = 0.7;

            TextBlock textBox1 = new TextBlock();

            textBox1.Text = selected_shape;
            textBox1.FontSize = 60;
            textBox1.Width = 250;
            textBox1.Foreground = new SolidColorBrush(Colors.Blue);
            textBox1.Opacity = 1;

            ListBox listBox1 = new ListBox();

            listBox1.FontSize = 35;
            listBox1.Foreground = new SolidColorBrush(Colors.White);
            listBox1.Opacity = 1;

            listBox1.Items.Add("Add");
            listBox1.Items.Add("Remove");

            if (!oneMarker)
            {
                listBox1.Items.Add("Remove one marker");
            }

            listBox1.Items.Add("Fit to View");

            //      if ((markerLayer != null) && (oneMarker != null))
            {
                listBox1.Items.Add("Fit All to View");
            }

            if (oneMarker)
            {
                listBox1.Items.Add("Change text");
            }


            listBox1.SelectionChanged += SelectionChangedEventHandler;

            Button button1 = new Button();
            button1.Content = "Close";
            button1.Margin = new Thickness(5.0);
            button1.Foreground = new SolidColorBrush(Colors.White);
            button1.Opacity = 1;
            button1.Click += new RoutedEventHandler(Cancel_Click);

            panel1.Children.Add(textBox1);
            panel1.Children.Add(listBox1);
            panel1.Children.Add(button1);
            border.Child = panel1;

            // Set the Child property of Popup to the border 
            // which contains a stackpanel, textblock and button.
            PopupP.Child = border;

            // Set where the popup will show up on the screen.
            PopupP.VerticalOffset = 100;
            PopupP.HorizontalOffset = 50;

            // Open the popup.
            PopupP.IsOpen = true;

        }

        void Cancel_Click(object sender, RoutedEventArgs e)
        {
            // Close the popup.
            PopupP.IsOpen = false;
            Debug.WriteLine("Cancel_Click ");
        }

        void SelectionChangedEventHandler(Object sender, SelectionChangedEventArgs e)
        {

            String sellected = (sender as ListBox).SelectedItem.ToString();

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