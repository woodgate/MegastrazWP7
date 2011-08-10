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

            //Browser Loaded Event Handler
            FacebookLoginBrowser.Loaded += new RoutedEventHandler(FacebookLoginBrowser_Loaded);
        }

        /// <summary>
        /// Perform Login to facebook.
        /// The function generates login url and navigates the browser to it
        /// </summary>
        private void LoginToFacebook()
        {
            TitlePanel.Visibility = Visibility.Visible;
            FacebookLoginBrowser.Visibility = Visibility.Visible;
            
            //Build facebook URL
            var loginParameters = new Dictionary<string, object>
                                      {
                                          { "response_type", "token" }
                                          // { "display", "touch" } // by default for wp7 builds only (in Facebook.dll), display is set to touch.
                                      };

            var navigateUrl = FacebookOAuthClient.GetLoginUrl(_appId, null, _extendedPermissions, loginParameters);

            //Navigate to facebook URL
            FacebookLoginBrowser.Navigate(navigateUrl);
        }

        /// <summary>
        /// Browser Loaded Event Handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void FacebookLoginBrowser_Loaded(object sender, RoutedEventArgs e)
        {
            if (!_loggedIn)
            {
                LoginToFacebook();
            }
        }

        /// <summary>
        /// Browser Navigated Event Handler
        /// Check if we have an OAuth Token and parse it
        /// When We have a good token call loginSucceeded()
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Login Success. Get a ticket from the Azure Cloud
        /// </summary>
        private void loginSucceeded()
        {
            TitlePanel.Visibility = Visibility.Visible;
            FacebookLoginBrowser.Visibility = Visibility.Collapsed;

            var client = new MegaStarzClient("http://localhost:81/Services/WP7/"); 

            //Get a ticket from the clod
            client.GetTicketAsync(new GetTicketRequest()
                                      {
                                          AccessToken = _fbClient.AccessToken
                                      }, 
                                   (response) =>        //Callback when we have a ticket
                                       {
                                           if (response != null)
                                           {
                                               //Save ticket & star object
                                               ((App) Application.Current).starTicket = response.Ticket.ticket;
                                               ((App) Application.Current).star = response.Star;

                                               //Navigate to SongList Page
                                               NavigationService.Navigate(new Uri("/SongListPage.xaml", UriKind.Relative));
                                           
                                           }
                                           else //Azure could error
                                           {
                                               Dispatcher.BeginInvoke(() => MessageBox.Show("Could not contact Azure server. Please try again later"));
                                           }
                                       });

        }
    }
}