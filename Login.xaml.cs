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

namespace MegaStarzWP7
{
    public partial class Login : PhoneApplicationPage
    {
        //Facebook Application Settings //TODO: Get from settings
        private const string _appId = "{150708541673346}";   
        private readonly string[] _extendedPermissions = new[] { "user_about_me" };

        public Login()
        {
            InitializeComponent();
        }
    }
}