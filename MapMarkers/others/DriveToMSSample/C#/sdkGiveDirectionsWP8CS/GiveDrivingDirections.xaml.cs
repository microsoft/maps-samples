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
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using sdkGiveDirectionsWP8CS.Resources;

namespace sdkGiveDirectionsWP8CS
{
    public partial class GiveDrivingDirections : PhoneApplicationPage
    {
        public GiveDrivingDirections()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // Extract the arguments from the query string passed to the page.
            IDictionary<string, string> uriParameters = this.NavigationContext.QueryString;
            string destinationLatitude = uriParameters["latitude"];
            string destinationLongitude = uriParameters["longitude"];
            string destinationName = uriParameters["name"];

            // Display the requested destination.
            this.tbShowRequestedDestination.Text = AppResources.DrivingDirectionsDisplayPrefix + ":\r\n" +
                "\tname = " + destinationName + "\r\n" +
                "\tlatitude = " + destinationLatitude + "\r\n" +
                "\tlongitude = " + destinationLongitude;
        }
    }
}
