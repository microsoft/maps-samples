/*
 * Copyright © 2012 Nokia Corporation. All rights reserved.
 * Nokia and Nokia Connecting People are registered trademarks of Nokia Corporation. 
 * Other product and company names mentioned herein may be trademarks
 * or trade names of their respective owners. 
 * See LICENSE.TXT for license information.
 */

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
using MapExplorer.Resources;

using Microsoft.Phone.Maps.Controls;
using System.Device.Location;

using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Maps.Services;
using System.Windows.Controls.Primitives;
using System.Windows.Data;


namespace MapExplorer
{
    public partial class MainPage : PhoneApplicationPage
    {
        ApplicationBarMenuItem AppBarColorModeMenuItem = null;
        ApplicationBarMenuItem AppBarLandmarksMenuItem = null;
        ApplicationBarMenuItem AppBarPedestrianFeaturesMenuItem = null;
        ApplicationBarMenuItem AppBarDirectionsMenuItem = null;
        ApplicationBarMenuItem AppBarAboutMenuItem = null;

        ProgressIndicator prog = null;

        Geoposition MyGeoPosition = null;
        List<GeoCoordinate> MyCoordinates = new List<GeoCoordinate>();

        MapRoute MyMapRoute = null;

        RouteQuery MyRouteQuery = null;
        GeocodeQuery MyGeocodeQuery = null;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            DataContext = App.Settings;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (isNewInstance)
            {
                isNewInstance = false;
            }

            DrawMapMarkers();
        }

        private void SearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (SearchTextBox.Text.Length > 0)
                {
                    AppBarDirectionsMenuItem.IsEnabled = false;
                    if (!App.Settings.RouteEnabled)
                    {
                        DirectionsTitleRowDefinition.Height = new GridLength(0);
                        DirectionsRowDefinition.Height = new GridLength(0);
                        if (MyMapRoute != null)
                        {
                            MyMap.RemoveRoute(MyMapRoute);
                        }
                        ShowProgress(AppResources.SearchingProgressText);
                    }
                    else
                    {
                        if (MyGeoPosition == null)
                        {
                            MessageBox.Show(AppResources.NoCurrentLocationMessageBoxText);
                            return;
                        }
                        else
                        {
                            ShowProgress(AppResources.CalculatingRouteProgressText);
                        }
                    }
                    SearchForTerm(SearchTextBox.Text);
                    this.Focus();
                }
            }
        }

        private void SearchForTerm(String searchTerm)
        {
            MyGeocodeQuery = new GeocodeQuery();
            MyGeocodeQuery.SearchTerm = searchTerm;
            if (MyGeoPosition == null)
            {
                MyGeocodeQuery.GeoCoordinate = new GeoCoordinate(0, 0);
            }
            else
            {
                MyGeocodeQuery.GeoCoordinate = new GeoCoordinate(MyGeoPosition.Coordinate.Latitude, MyGeoPosition.Coordinate.Longitude);
            }

            MyGeocodeQuery.QueryCompleted += MyGeocodeQuery_QueryCompleted;
            MyGeocodeQuery.QueryAsync();
        }

        private void SearchTextBox_LostFocus(object sender, EventArgs e)
        {
            SearchTextBox.Visibility = Visibility.Collapsed;
        }

        void MyGeocodeQuery_QueryCompleted(object sender, QueryCompletedEventArgs<IList<MapLocation>> e)
        {
            if (e.Error == null)
            {
                if (e.Result.Count > 0)
                {
                    MyCoordinates.Clear();

                    // Add my position to be the first in MyCoordinates
                    if (MyGeoPosition == null)
                    {
                        MyCoordinates.Add(new GeoCoordinate(0, 0));
                    }
                    else
                    {
                        MyCoordinates.Add(new GeoCoordinate(MyGeoPosition.Coordinate.Latitude, MyGeoPosition.Coordinate.Longitude));
                    }

                    if (App.Settings.RouteEnabled)
                    {
                        // Route to first search result
                        MyCoordinates.Add(e.Result[0].GeoCoordinate);

                        // Create a route from current position to destination
                        MyRouteQuery = new RouteQuery();
                        MyRouteQuery.TravelMode = App.Settings.MapTravelMode;
                        MyRouteQuery.Waypoints = MyCoordinates;
                        MyRouteQuery.QueryCompleted += MyRouteQuery_QueryCompleted;
                        MyRouteQuery.QueryAsync();
                        // MyGeocodeQuery.Dispose();
                    }
                    else
                    {
                        // Add all results to MyCoordinates for drawing the map markers
                        for (int i = 0; i < e.Result.Count; i++)
                        {
                            MyCoordinates.Add(e.Result[i].GeoCoordinate);
                        }

                        // Just center on the result if route is not wanted.
                        MyMap.SetView(e.Result[0].GeoCoordinate, 10, MapAnimationKind.Parabolic);
                        HideProgress();
                    }

                    DrawMapMarkers();
                }
                else
                {
                    HideProgress();
                    MessageBox.Show(AppResources.NoMatchFoundMessageBoxText);
                }
            }
        }

        void MyRouteQuery_QueryCompleted(object sender, QueryCompletedEventArgs<Route> e)
        {
            if (e.Error == null)
            {
                if (MyMapRoute != null)
                {
                    MyMap.RemoveRoute(MyMapRoute);
                }

                Route MyRoute = e.Result;
                MyMapRoute = new MapRoute(MyRoute);

                AppBarDirectionsMenuItem.IsEnabled = true;
                MyMap.AddRoute(MyMapRoute);

                List<string> RouteList = new List<string>();
                foreach (RouteLeg leg in MyRoute.Legs)
                {
                    foreach (RouteManeuver maneuver in leg.Maneuvers)
                    {
                        RouteList.Add(maneuver.InstructionText);
                    }
                }
                RouteLLS.ItemsSource = RouteList;
            }

            HideProgress();
        }

        private void DrawMapMarker(GeoCoordinate coordinate, Color color)
        {
            // Create a map marker
            Polygon MyPolygon = new Polygon();
            MyPolygon.Points.Add(new Point(0, 0));
            MyPolygon.Points.Add(new Point(0, 75));
            MyPolygon.Points.Add(new Point(25, 0));
            MyPolygon.Fill = new SolidColorBrush(color);

            //Create a MapOverlay and add marker.
            MapOverlay MyOverlay = new MapOverlay();
            MyOverlay.Content = MyPolygon;
            MyOverlay.GeoCoordinate = new GeoCoordinate(coordinate.Latitude, coordinate.Longitude);
            MyOverlay.PositionOrigin = new Point(0.0, 1.0);

            // Add overlay to map.
            MapLayer MyLayer = new MapLayer();
            MyLayer.Add(MyOverlay);
            MyMap.Layers.Add(MyLayer);
        }

        private void DrawMapMarkers()
        {
            MyMap.Layers.Clear();

            // Draw marker for current position
            if (MyGeoPosition != null)
            {
                DrawMapMarker(new GeoCoordinate(MyGeoPosition.Coordinate.Latitude, MyGeoPosition.Coordinate.Longitude), Colors.Red);
            }

            // Draw markers for destination
            for (int i = 1; i < MyCoordinates.Count; i++)
            {
                DrawMapMarker(MyCoordinates[i], Colors.Blue);
            }
        }

        private void Search_Click(object sender, EventArgs e)
        {
            App.Settings.RouteEnabled = false;
            SearchTextBox.Visibility = Visibility.Visible;
            SearchTextBox.Focus();
        }

        private void Route_Click(object sender, EventArgs e)
        {
            App.Settings.RouteEnabled = true;
            SearchTextBox.Visibility = Visibility.Visible;
            SearchTextBox.Focus();
        }

        private void LocateMe_Click(object sender, EventArgs e)
        {
            if (isLocationAllowed)
            {
                GetCurrentLocation();
            }
            else
            {
                LocationPanel.Visibility = Visibility.Visible;
            }
        }

        private void About_Click(object sender, EventArgs e)
        {
            // Clear map layers to avoid map markers briefly shown on top of about page 
            MyMap.Layers.Clear();
            NavigationService.Navigate(new Uri("/AboutPage.xaml", UriKind.Relative));
        }

        private void ColorMode_Click(object sender, EventArgs e)
        {
            if (App.Settings.MapColorMode == MapColorMode.Dark)
            {
                App.Settings.MapColorMode = MapColorMode.Light;
                AppBarColorModeMenuItem.Text = AppResources.ColorModeDarkMenuItemText;
            }
            else
            {
                App.Settings.MapColorMode = MapColorMode.Dark;
                AppBarColorModeMenuItem.Text = AppResources.ColorModeLightMenuItemText;
            }
        }

        private void Landmarks_Click(object sender, EventArgs e)
        {
            App.Settings.MapLandmarksEnabled = !App.Settings.MapLandmarksEnabled;
            if (App.Settings.MapLandmarksEnabled)
            {
                AppBarLandmarksMenuItem.Text = AppResources.LandmarksOffMenuItemText;
            }
            else
            {
                AppBarLandmarksMenuItem.Text = AppResources.LandmarksOnMenuItemText;
            }
        }

        private void PedestrianFeatures_Click(object sender, EventArgs e)
        {
            App.Settings.MapPedestrianFeaturesEnabled = !App.Settings.MapPedestrianFeaturesEnabled;
            if (App.Settings.MapPedestrianFeaturesEnabled)
            {
                AppBarPedestrianFeaturesMenuItem.Text = AppResources.PedestrianFeaturesOffMenuItemText;
            }
            else
            {
                AppBarPedestrianFeaturesMenuItem.Text = AppResources.PedestrianFeaturesOnMenuItemText;
            }
        }

        private void Directions_Click(object sender, EventArgs e)
        {
            App.Settings.DirectionsEnabled = !App.Settings.DirectionsEnabled;
            if (App.Settings.DirectionsEnabled)
            {
                AppBarDirectionsMenuItem.Text = AppResources.DirectionsOffMenuItemText;
                DirectionsTitleRowDefinition.Height = GridLength.Auto;
                DirectionsRowDefinition.Height = new GridLength(2, GridUnitType.Star);
                ModePanel.Visibility = Visibility.Collapsed;
                HeadingPanel.Visibility = Visibility.Collapsed;
                PitchPanel.Visibility = Visibility.Collapsed;
            }
            else
            {
                AppBarDirectionsMenuItem.Text = AppResources.DirectionsOnMenuItemText;
                DirectionsTitleRowDefinition.Height = new GridLength(0);
                DirectionsRowDefinition.Height = new GridLength(0);
                ModePanel.Visibility = Visibility.Visible;
                HeadingPanel.Visibility = Visibility.Visible;
                PitchPanel.Visibility = Visibility.Visible;
            }
        }

        private void AllowLocation_Click(object sender, EventArgs e)
        {
            LocationPanel.Visibility = Visibility.Collapsed;
            isLocationAllowed = true;
            BuildApplicationBar();
            GetCurrentLocation();
        }

        private void CancelLocation_Click(object sender, EventArgs e)
        {
            LocationPanel.Visibility = Visibility.Collapsed;
            BuildApplicationBar();
        }

        private void RoadButton_Click(object sender, EventArgs e)
        {
            App.Settings.MapCartographicMode = MapCartographicMode.Road;
        }
        private void AerialButton_Click(object sender, EventArgs e)
        {
            App.Settings.MapCartographicMode = MapCartographicMode.Aerial;
        }
        private void HybridButton_Click(object sender, EventArgs e)
        {
            App.Settings.MapCartographicMode = MapCartographicMode.Hybrid;
        }
        private void TerrainButton_Click(object sender, EventArgs e)
        {
            App.Settings.MapCartographicMode = MapCartographicMode.Terrain;
        }

        private void DriveButton_Click(object sender, EventArgs e)
        {
            App.Settings.MapTravelMode = TravelMode.Driving;
            ShowProgress(AppResources.CalculatingRouteProgressText);
            MyGeocodeQuery.QueryAsync();
        }
        private void WalkButton_Click(object sender, EventArgs e)
        {
            App.Settings.MapTravelMode = TravelMode.Walking;
            ShowProgress(AppResources.CalculatingRouteProgressText);
            MyGeocodeQuery.QueryAsync();
        }

        private void HeadingValueChanged(object sender, EventArgs e)
        {
            if (HeadingSlider != null)
            {
                double value = HeadingSlider.Value;
                if (value > 360) value -= 360;
                App.Settings.MapHeading = value;
            }
        }

        // Helper function to get current location
        private async void GetCurrentLocation()
        {
            ShowProgress(AppResources.GettingLocationProgressText);
            Geolocator geolocator = new Geolocator();
            geolocator.DesiredAccuracyInMeters = 10;
            try
            {
                MyGeoPosition = await geolocator.GetGeopositionAsync(TimeSpan.FromMinutes(1), TimeSpan.FromSeconds(10));

                Dispatcher.BeginInvoke(() =>
                {
                    GeoCoordinate myCoordinate = new GeoCoordinate(MyGeoPosition.Coordinate.Latitude, MyGeoPosition.Coordinate.Longitude);
                    MyMap.SetView(myCoordinate, 10, MapAnimationKind.Parabolic);
                    DrawMapMarkers();
                    HideProgress();
                });
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show(AppResources.LocationDisabledMessageBoxText);
            }
            catch (Exception ex)
            {
                // something else happened acquring the location
                // ex.HResult can be read to know the specific error code but it is not recommended
            }
        }

        // Helper function to build a localized ApplicationBar
        private void BuildApplicationBar()
        {
            // Set the page's ApplicationBar to a new instance of ApplicationBar.    
            ApplicationBar = new ApplicationBar();

            ApplicationBar.Mode = ApplicationBarMode.Default;
            ApplicationBar.Opacity = 1.0;
            ApplicationBar.IsVisible = true;
            ApplicationBar.IsMenuEnabled = true;

            // Create a new buttons and set the text values to the localized strings from AppResources.
            ApplicationBarIconButton appBarSearchButton = new ApplicationBarIconButton(new Uri("/Assets/appbar.feature.search.rest.png", UriKind.Relative));
            appBarSearchButton.Text = AppResources.SearchMenuButtonText;
            appBarSearchButton.Click += new EventHandler(Search_Click);
            ApplicationBar.Buttons.Add(appBarSearchButton);

            ApplicationBarIconButton appBarRouteButton = new ApplicationBarIconButton(new Uri("/Assets/appbar.show.route.png", UriKind.Relative));
            appBarRouteButton.Text = AppResources.RouteMenuButtonText;
            appBarRouteButton.Click += new EventHandler(Route_Click);
            ApplicationBar.Buttons.Add(appBarRouteButton);

            ApplicationBarIconButton appBarLocateMeButton = new ApplicationBarIconButton(new Uri("/Assets/appbar.locate.me.png", UriKind.Relative));
            appBarLocateMeButton.Text = AppResources.LocateMeMenuButtonText;
            appBarLocateMeButton.Click += new EventHandler(LocateMe_Click);
            ApplicationBar.Buttons.Add(appBarLocateMeButton);

            // Create a new menu items with the localized strings from AppResources.
            AppBarColorModeMenuItem = new ApplicationBarMenuItem(AppResources.ColorModeDarkMenuItemText);
            AppBarColorModeMenuItem.Click += new EventHandler(ColorMode_Click);
            ApplicationBar.MenuItems.Add(AppBarColorModeMenuItem);

            AppBarLandmarksMenuItem = new ApplicationBarMenuItem(AppResources.LandmarksOnMenuItemText);
            AppBarLandmarksMenuItem.Click += new EventHandler(Landmarks_Click);
            ApplicationBar.MenuItems.Add(AppBarLandmarksMenuItem);

            AppBarPedestrianFeaturesMenuItem = new ApplicationBarMenuItem(AppResources.PedestrianFeaturesOnMenuItemText);
            AppBarPedestrianFeaturesMenuItem.Click += new EventHandler(PedestrianFeatures_Click);
            ApplicationBar.MenuItems.Add(AppBarPedestrianFeaturesMenuItem);

            AppBarDirectionsMenuItem = new ApplicationBarMenuItem(AppResources.DirectionsOnMenuItemText);
            AppBarDirectionsMenuItem.Click += new EventHandler(Directions_Click);
            AppBarDirectionsMenuItem.IsEnabled = false;
            ApplicationBar.MenuItems.Add(AppBarDirectionsMenuItem);

            AppBarAboutMenuItem = new ApplicationBarMenuItem(AppResources.AboutMenuItemText);
            AppBarAboutMenuItem.Click += new EventHandler(About_Click);
            ApplicationBar.MenuItems.Add(AppBarAboutMenuItem);
        }

        // Helper function to show progress indicator in system tray
        private void ShowProgress(String msg)
        {
            if (prog == null)
            {
                prog = new ProgressIndicator();
                prog.IsIndeterminate = true;
            }
            prog.Text = msg;
            prog.IsVisible = true;
            SystemTray.SetProgressIndicator(this, prog);
        }

        // Helper function to hide progress indicator in system tray
        private void HideProgress()
        {
            prog.IsVisible = false;
            SystemTray.SetProgressIndicator(this, prog);
        }

        /// <summary>
        /// True when this object instance has been just created, otherwise false
        /// </summary>
        private bool isNewInstance = true;

        /// <summary>
        /// True when access to user location is allowed, otherwise false
        /// </summary>
        private bool isLocationAllowed = false;
    }
}