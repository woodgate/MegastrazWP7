﻿using System;
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
using MegaStarzWP7.ViewModels;
namespace MegaStarzWP7
{
    public partial class SongListPage : PhoneApplicationPage
    {
        public SongListPage()
        {
            DataContext = ((App) Application.Current).SongList;
            InitializeComponent();
        }
    }
}