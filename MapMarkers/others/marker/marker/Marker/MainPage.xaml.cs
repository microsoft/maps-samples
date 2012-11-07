using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Marker.Resources;
using System.Diagnostics;
using Microsoft.Phone.Maps.Controls;
using System.Device.Location;
using System.Windows.Media.Imaging;
using System.Windows.Input;

namespace Marker
{
    public partial class MainPage : PhoneApplicationPage
    {
        bool _draggingNewMarker = false;
        bool _draggingExistingMarker = false;
        Image _markerBeingDragged = null;
        Dictionary<Image, MapOverlay> _markerList;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            System.Windows.Input.Touch.FrameReported += Touch_FrameReported;
            _markerList = new Dictionary<Image, MapOverlay>();
        }

        private void NewMarkerGreen_Hold(object sender, System.Windows.Input.GestureEventArgs e)
        {
            _draggingNewMarker = true;
            MarkerGreen_Hold(sender, e);
        }

        private void ExistingMarker_Hold(object sender, System.Windows.Input.GestureEventArgs e)
        {
            _draggingExistingMarker = true;
            _markerBeingDragged = sender as Image;
            _markerBeingDragged.Visibility = System.Windows.Visibility.Collapsed;
            MarkerGreen_Hold(sender, e);
        }

        private void MarkerGreen_Hold(object sender, System.Windows.Input.GestureEventArgs e)
        {
            TheMap.IsEnabled = false;
            MarkerGreen.Height = 200;
            MarkerGreen.Width = 200;

            moveMarker(e.GetPosition(TheMap));
            Debug.WriteLine("Hold");
        }

        void moveMarker(Point toPosition)
        {
            Thickness topCorner = (Thickness)MarkerGreen.GetValue(Grid.MarginProperty);

            // The center of the image we use as marker is somewhere in the middle of the image (100,80)

            topCorner.Left = toPosition.X - 100 ;
            topCorner.Top = toPosition.Y - 80 ;

            System.Windows.Thickness margin = topCorner;
            MarkerGreen.SetValue(Grid.MarginProperty, margin);
        }

        void Touch_FrameReported(object sender, System.Windows.Input.TouchFrameEventArgs e)
        {
            if (!(_draggingExistingMarker || _draggingNewMarker)) return;
            TouchPoint tp = e.GetPrimaryTouchPoint(TheMap);
            Debug.WriteLine("Touch " + tp.Action + " " + tp.Position);

            if (tp.Action == TouchAction.Move)
            {
                moveMarker(tp.Position);
            }

            if (tp.Action == TouchAction.Up)
            {
                TheMap.IsEnabled = true;
                MarkerGreen.Height = 100;
                MarkerGreen.Width = 100;

                // For the marker image used in this example, the end of the pin is located at the middle of the bottom line of the image
                // (XMax/2, YMax)

                Thickness topCorner = (Thickness)MarkerGreen.GetValue(Grid.MarginProperty);
                Point coordPoint = new Point(topCorner.Left+(200/2), topCorner.Top+200);
                GeoCoordinate coord = TheMap.ConvertViewportPointToGeoCoordinate(coordPoint);

                // Return the used marker to the toolbox window.
                MarkerGreen.SetValue(Grid.MarginProperty, new Thickness(0, 0, 0, 0));

                if (_draggingNewMarker)
                {
                    // Add a new marker on the map.
                    BitmapImage MarkerBitmap = new BitmapImage(new Uri("Marker.png", UriKind.Relative));
                    Image Marker = new Image();
                    Marker.Source = MarkerBitmap;
                    Marker.Visibility = System.Windows.Visibility.Visible;
                    Marker.Height = 100;
                    Marker.Width = 100;

                    MapOverlay pin = new MapOverlay();
                    pin.Content = Marker;
                    _markerList.Add(Marker, pin);

                    Debug.WriteLine("Pin dropped at " + coord);
                    pin.GeoCoordinate = coord;
                    pin.PositionOrigin = new Point(0.5, 1);
                    Marker.Hold += ExistingMarker_Hold;
                    MapLayer markerLayer = new MapLayer();
                    markerLayer.Add(pin);

                    TheMap.Layers.Add(markerLayer);

                }

                if (_draggingExistingMarker)
                {
                    // Change the geolocation of the map marker. 
                    MapOverlay pin = _markerList[_markerBeingDragged];
                    if (pin != null)
                    {
                        Debug.WriteLine("Pin dropped at " + coord);
                        pin.GeoCoordinate = coord;
                        _markerBeingDragged.Visibility = System.Windows.Visibility.Visible;
                    }
                }

                _draggingNewMarker = false;
                _draggingExistingMarker = false;
            }
        }
    }


}