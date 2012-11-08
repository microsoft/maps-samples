using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using DraggableMarker.Resources;
using Microsoft.Phone.Maps.Controls;
using System.Diagnostics;
using System.Windows.Controls.Primitives;
using System.Device.Location;
using System.Collections.ObjectModel;
using System.Windows.Shapes;
using System.Windows.Input;
using System.Windows.Media;

namespace DraggableMarker
{
    public partial class MainPage : PhoneApplicationPage
    {
        bool draggingNow = false;
        MapOverlay oneMarker = null;
        

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            System.Windows.Input.Touch.FrameReported += Touch_FrameReported;

            MapLayer oneMarkerLayer = new MapLayer();
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
            TextBlock MarkerTxt = new TextBlock { Text = "Drag" };
            MarkerTxt.HorizontalAlignment = HorizontalAlignment.Center;
            Canvas.SetLeft(MarkerTxt, 10);
            Canvas.SetTop(MarkerTxt, 5);
            Canvas.SetZIndex(MarkerTxt, 5);

            canCan.Children.Add(MarkerTxt);
            oneMarker.Content = canCan;

            oneMarker.PositionOrigin = new Point(0.5, 0.5);
            oneMarker.GeoCoordinate = new GeoCoordinate(60.35, 24.60);
            MarkerTxt.MouseLeftButtonDown += marker_MouseLeftButtonDown;
 
              
            oneMarkerLayer.Add(oneMarker);
            map1.Layers.Add(oneMarkerLayer);
            map1.Center = oneMarker.GeoCoordinate;
        }

        void Touch_FrameReported(object sender, System.Windows.Input.TouchFrameEventArgs e)
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

        void marker_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("oneMarker_MouseLeftButtonDown");
            if (oneMarker != null)
            {
                draggingNow = true;
                map1.IsEnabled = false;
            }
        }
    }
}