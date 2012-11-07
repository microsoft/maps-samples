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
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace sdkGiveDirectionsWP8CS
{
    class DirectionsRequestUriMapper : UriMapperBase
    {
        public override Uri MapUri(Uri uri)
        {
            const string DRIVING_PROTOCOL = "ms-drive-to";
            const string WALKING_PROTOCOL = "ms-walk-to";
            const string DRIVING_DIRECTIONS_PAGE = "GiveDrivingDirections.xaml";
            const string WALKING_DIRECTIONS_PAGE = "GiveWalkingDirections.xaml";

            string tempUri = Uri.UnescapeDataString(uri.ToString());

            // Does the Uri contain a request for driving directions or walking directions?
            if (tempUri.Contains(DRIVING_PROTOCOL) || tempUri.Contains(WALKING_PROTOCOL))
            {
                // Parse the Uri.
                char[] uriDelimiters = { '?', '=', '&' };
                string[] uriParameters = tempUri.Split(uriDelimiters);
                string destLatitude = uriParameters[4];
                string destLongitude = uriParameters[6];
                string destName = uriParameters[8];

                // Map the request for directions to the correct page of the app.
                string destinationPage;
                if (tempUri.Contains(DRIVING_PROTOCOL))
                {
                    destinationPage = "/" + DRIVING_DIRECTIONS_PAGE + "?";
                }
                else
                {
                    destinationPage = "/" + WALKING_DIRECTIONS_PAGE + "?";
                }
                string pageUri = destinationPage +
                    "latitude=" + destLatitude + "&" +
                    "longitude=" + destLongitude + "&" +
                    "name=" + destName;

                return new Uri(pageUri, UriKind.Relative);
            }
            // Otherwise, handle the Uri normally.
            return uri;
        }
    }
}
