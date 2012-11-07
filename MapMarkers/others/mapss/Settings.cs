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
using System.Text;
using System.Threading.Tasks;

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Device.Location;

using Microsoft.Phone.Maps.Controls;
using System.Windows;
using System.Windows.Data;
using Microsoft.Phone.Maps.Services;

namespace MapExplorer
{
    /// <summary>
    /// Model class for settings data
    /// </summary>
    public class Settings : INotifyPropertyChanged
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Settings()
        {
            _mapCartographicMode = MapCartographicMode.Road;
            _mapColorMode = MapColorMode.Light;
            _mapLandmarksEnabled = false;
            _mapPedestrianFeaturesEnabled = false;
            _mapZoomLevel = 2;
            _mapPitch = 0;
            _mapHeading = 0;
            _mapTravelMode = TravelMode.Driving;
        }

        /// <summary>
        /// Member variable for map cartographic mode property
        /// </summary>
        private MapCartographicMode _mapCartographicMode;

        /// <summary>
        /// Property for map cartographic mode
        /// </summary>
        public MapCartographicMode MapCartographicMode
        {
            get
            {
                return _mapCartographicMode;
            }
            set
            {
                if (_mapCartographicMode != value)
                {
                    _mapCartographicMode = value;
                    NotifyPropertyChanged("MapCartographicMode");
                    NotifyPropertyChanged("AllowNonAerialSettingsChange");
                }
            }
        }

        /// <summary>
        /// Member variable for map color mode property
        /// </summary>
        private MapColorMode _mapColorMode;

        /// <summary>
        /// Property for map color mode
        /// </summary>
        public MapColorMode MapColorMode
        {
            get
            {
                return _mapColorMode;
            }
            set
            {
                if (_mapColorMode != value)
                {
                    _mapColorMode = value;
                    NotifyPropertyChanged("MapColorMode");
                }
            }
        }

        /// <summary>
        /// Member variable for maps landmarks enabled property
        /// </summary>
        private bool _mapLandmarksEnabled;

        /// <summary>
        /// Property for map landmarks enabled
        /// </summary>
        public bool MapLandmarksEnabled
        {
            get
            {
                return _mapLandmarksEnabled;
            }
            set
            {
                if (_mapLandmarksEnabled != value)
                {
                    _mapLandmarksEnabled = value;
                    NotifyPropertyChanged("MapLandmarksEnabled");
                }
            }
        }

        /// <summary>
        /// Member variable for map pedestrian features enabled property
        /// </summary>
        private bool _mapPedestrianFeaturesEnabled;

        /// <summary>
        /// Property for map pedestrian features enabled
        /// </summary>
        public bool MapPedestrianFeaturesEnabled
        {
            get
            {
                return _mapPedestrianFeaturesEnabled;
            }
            set
            {
                if (_mapPedestrianFeaturesEnabled != value)
                {
                    _mapPedestrianFeaturesEnabled = value;
                    NotifyPropertyChanged("MapPedestrianFeaturesEnabled");
                }
            }
        }

        /// <summary>
        /// Member variable for map zoom level property
        /// </summary>
        private int _mapZoomLevel;

        /// <summary>
        /// Property for map zoom level
        /// </summary>
        public int MapZoomLevel
        {
            get
            {
                return _mapZoomLevel;
            }
            set
            {
                if (_mapZoomLevel != value)
                {
                    _mapZoomLevel = value;
                    NotifyPropertyChanged("MapZoomLevel");
                }
            }
        }

        /// <summary>
        /// Member variable for map pitch property
        /// </summary>
        private int _mapPitch;

        /// <summary>
        /// Property for map pitch
        /// </summary>
        public int MapPitch
        {
            get
            {
                return _mapPitch;
            }
            set
            {
                if (_mapPitch != value)
                {
                    _mapPitch = value;
                    NotifyPropertyChanged("MapPitch");
                }
            }
        }

        /// <summary>
        /// Member variable for map heading property
        /// </summary>
        private double _mapHeading;

        /// <summary>
        /// Property for map heading
        /// </summary>
        public double MapHeading
        {
            get
            {
                return _mapHeading;
            }
            set
            {
                if (_mapHeading != value)
                {
                    _mapHeading = value;
                    NotifyPropertyChanged("MapHeading");
                }
            }
        }

        /// <summary>
        /// Member variable for directions property
        /// </summary>
        private bool _directionsEnabled;

        /// <summary>
        /// Property for map landmarks enabled
        /// </summary>
        public bool DirectionsEnabled
        {
            get
            {
                return _directionsEnabled;
            }
            set
            {
                if (_directionsEnabled != value)
                {
                    _directionsEnabled = value;
                    NotifyPropertyChanged("DirectionsEnabled");
                }
            }
        }

        /// <summary>
        /// Member variable for route property
        /// </summary>
        private bool _routeEnabled;

        /// <summary>
        /// Property for map route enabled
        /// </summary>
        public bool RouteEnabled
        {
            get
            {
                return _routeEnabled;
            }
            set
            {
                if (_routeEnabled != value)
                {
                    _routeEnabled = value;
                    NotifyPropertyChanged("RouteEnabled");
                }
            }
        }

        /// <summary>
        /// Member variable for travel mode property
        /// </summary>
        private TravelMode _mapTravelMode;

        /// <summary>
        /// Property for map travel mode
        /// </summary>
        public TravelMode MapTravelMode
        {
            get
            {
                return _mapTravelMode;
            }
            set
            {
                if (_mapTravelMode != value)
                {
                    _mapTravelMode = value;
                    NotifyPropertyChanged("MapTravelMode");
                    NotifyPropertyChanged("RouteByDriving");
                    NotifyPropertyChanged("RouteByWalking");
                }
            }
        }

        /// <summary>
        /// Properties for map travel modes
        /// </summary>
        public bool RouteByDriving
        {
            get
            {
                return (_mapTravelMode == TravelMode.Driving);
            }
        }
        public bool RouteByWalking
        {
            get
            {
                return (_mapTravelMode == TravelMode.Walking);
            }
        }

        /// <summary>
        /// Implementation of PropertyChanged event of INotifyPropertyChanged
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Helper method for emitting PropertyChanged event
        /// </summary>
        /// <param name="propertyName">Name of the property that has changed</param>
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    // Custom class implements the IValueConverter interface.
    public class MapCartographicModeToIndexConverter : IValueConverter
    {

        #region IValueConverter Members

        // Define the Convert method to change a CartographicMode object to an index.
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            // value is the data from the source object.
            int mode = (int)value;
            int index = 0;
            switch (mode)
            {
                case (int)MapCartographicMode.Road:
                    index = 0;
                    break;
                case (int)MapCartographicMode.Aerial:
                    index = 1;
                    break;
                case (int)MapCartographicMode.Hybrid:
                    index = 2;
                    break;
                case (int)MapCartographicMode.Terrain:
                    index = 3;
                    break;
                default:
                    index = 0;
                    break;
            }
            // Return the value to pass to the target.
            return index;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            // value is the data from the source object.
            int index = (int)value;
            MapCartographicMode mode;
            switch (index)
            {
                case 0:
                    mode = MapCartographicMode.Road;
                    break;
                case 1:
                    mode = MapCartographicMode.Aerial;
                    break;
                case 2:
                    mode = MapCartographicMode.Hybrid;
                    break;
                case 3:
                    mode = MapCartographicMode.Terrain;
                    break;
                default:
                    mode = MapCartographicMode.Road;
                    break;
            }
            // Return the value to pass to the target.
            return mode;
        }

        #endregion
    }

    // Custom class implements the IValueConverter interface.
    public class MapColorModeToBooleanConverter : IValueConverter
    {

        #region IValueConverter Members

        // Define the Convert method to change a CartographicMode object to an index.
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            // value is the data from the source object.
            int mode = (int)value;
            bool ret = false;
            if (mode == (int)MapColorMode.Dark)
            {
                ret = true;
            }

            // Return the value to pass to the target.
            return ret;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            // value is the data from the source object.
            bool flag = (bool)value;
            MapColorMode mode = MapColorMode.Light;
            if (flag)
            {
                mode = MapColorMode.Dark;
            }
            // Return the value to pass to the target.
            return mode;
        }

        #endregion
    }

    // Custom class implements the IValueConverter interface.
    public class MapTravelModeToIndexConverter : IValueConverter
    {

        #region IValueConverter Members

        // Define the Convert method to change a CartographicMode object to an index.
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            // value is the data from the source object.
            int mode = (int)value;
            int index = 0;
            switch (mode)
            {
                case (int)TravelMode.Driving:
                    index = 0;
                    break;
                case (int)TravelMode.Walking:
                    index = 1;
                    break;
                default:
                    index = 0;
                    break;
            }
            // Return the value to pass to the target.
            return index;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            // value is the data from the source object.
            int index = (int)value;
            TravelMode mode;
            switch (index)
            {
                case 0:
                    mode = TravelMode.Driving;
                    break;
                case 1:
                    mode = TravelMode.Walking;
                    break;
                default:
                    mode = TravelMode.Driving;
                    break;
            }
            // Return the value to pass to the target.
            return mode;
        }

        #endregion
    }

}