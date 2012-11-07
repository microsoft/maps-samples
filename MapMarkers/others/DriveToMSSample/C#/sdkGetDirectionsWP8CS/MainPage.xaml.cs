/* 
    Copyright (c) 2012 Microsoft Corporation.  All rights reserved.
    Use of this sample source code is subject to the terms of the Microsoft license 
    agreement under which you licensed this sample source code and is provided AS-IS.
    If you did not accept the terms of the license agreement, you are not authorized 
    to use this sample source code.  For the terms of the license, please see the 
    license agreement between you and Microsoft.
  
    To see all Code Samples for Windows Phone, visit http://go.microsoft.com/fwlink/?LinkID=219604 
  
*/
using System;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using sdkGetDirectionsWP8CS.Resources;

namespace sdkGetDirectionsWP8CS
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Test both navigation Uri schemes.
        private const string DRIVING_PROTOCOL = "ms-drive-to";
        private const string WALKING_PROTOCOL = "ms-walk-to";

        // Use Seattle as the default destination.
        private const string SEATTLE_LATITUDE = "47.37";
        private const string SEATTLE_LONGITUDE = "-122.20";
        private const string SEATTLE_NAME = "Seattle, WA";

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        // Single event handler for both driving and walking directions.
        private void GetDirections(object sender, EventArgs e)
        {
            string uriProtocol = null;
            string uri;

            // Driving or walking directions?
            string buttonClicked = ((Button)sender).Name;
            switch (buttonClicked)
            {
                case "btnGetDrivingDirections":
                    uriProtocol = DRIVING_PROTOCOL;
                    break;
                case "btnGetWalkingDirections":
                    uriProtocol = WALKING_PROTOCOL;
                    break;
                default:
                    uriProtocol = DRIVING_PROTOCOL;
                    break;
            }

            // Assemble the Uri for the request.
            uri = uriProtocol + ":?" +
                "destination.latitude=" + SEATTLE_LATITUDE + "&" +
                "destination.longitude=" + SEATTLE_LONGITUDE + "&" +
                "destination.name=" + SEATTLE_NAME;

            // Display the Uri.
            tbShowRequestUri.Text = AppResources.UriDisplayPrefix + ":\n" + uri;

            // Make the request.
            RequestDirections(uri);
        }

        async void RequestDirections(string uri)
        {
            // Make the request.
            var success = await Windows.System.Launcher.LaunchUriAsync(new Uri(uri));
            if (success)
            {
                // Request succeeded.
            }
            else
            {
                // Request failed.
            }
        }

    }
}
