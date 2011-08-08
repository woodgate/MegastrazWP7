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
using MegaStarzWP7.ViewModels;
using Microsoft.Phone.Controls;

namespace MegaStarzWP7
{
    public partial class MainPage : PhoneApplicationPage
    {
        private MegaStarzViewModelcs songViewModel;
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            songViewModel = new MegaStarzViewModelcs();
        }
    }
}