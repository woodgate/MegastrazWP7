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
using Facebook;
using Megastar.Client.Library;
using Megastar.RestServices.Library.Entities;
using Microsoft.Phone.Controls;

namespace MegaStarzWP7
{
    public partial class Login : PhoneApplicationPage
    {
        //Facebook Application Settings //TODO: Get from settings
        private const string _appId = "150708541673346";
        private readonly string[] _extendedPermissions = new[] { "user_about_me", "user_birthday", "email" }; //,"user_birthday","email"

        private FacebookClient _fbClient;
        private bool _loggedIn;

        public Login()
        {
            InitializeComponent();
            _fbClient = new FacebookClient();
            FacebookLoginBrowser.Loaded += new RoutedEventHandler(FacebookLoginBrowser_Loaded);
        }

        // This handles the display a little and also creates the initial URL to navigate to in the browser control
        private void LoginToFacebook()
        {
            TitlePanel.Visibility = Visibility.Visible;
            FacebookLoginBrowser.Visibility = Visibility.Visible;
            
            var loginParameters = new Dictionary<string, object>
                                      {
                                          { "response_type", "token" }
                                          // { "display", "touch" } // by default for wp7 builds only (in Facebook.dll), display is set to touch.
                                      };

            var navigateUrl = FacebookOAuthClient.GetLoginUrl(_appId, null, _extendedPermissions, loginParameters);

            FacebookLoginBrowser.Navigate(navigateUrl);
        }

        // Browser control is loaded and fully ready for use
        void FacebookLoginBrowser_Loaded(object sender, RoutedEventArgs e)
        {
            if (!_loggedIn)
            {
                LoginToFacebook();
            }
        }

        private void FacebookLoginBrowser_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            FacebookOAuthResult oauthResult;
            if (FacebookOAuthResult.TryParse(e.Uri, out oauthResult))
            {
                if (oauthResult.IsSuccess)
                {
                    _fbClient = new FacebookClient(oauthResult.AccessToken);
                    _loggedIn = true;
                    loginSucceeded();
                }
                else
                {
                    MessageBox.Show(oauthResult.ErrorDescription);
                }
            }
        }

        // At this point we have an access token so we can get information from facebook
        private void loginSucceeded()
        {
            TitlePanel.Visibility = Visibility.Visible;
            FacebookLoginBrowser.Visibility = Visibility.Collapsed;

            var client = new MegaStarzClient(); 

            client.GetTicketAsync(new GetTicketRequest()
                                      {
                                          AccessToken = _fbClient.AccessToken
                                      }, 
                                   (response) =>
                                       {
                                           if (response != null)
                                           {
                                               ((App) Application.Current).starTicket = response.Ticket.ticket;
                                               ((App) Application.Current).star = response.Star;

                                               NavigationService.Navigate(new Uri("/SongListPage.xaml", UriKind.Relative));
                                           
                                           }
                                           else
                                           {
                                               Dispatcher.BeginInvoke(() => MessageBox.Show("Could not contact Azure server. Please try again later"));
                                           }
                                       });

        }
    }
}