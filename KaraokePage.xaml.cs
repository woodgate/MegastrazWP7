using System.Windows;
using Microsoft.Phone.Controls;

namespace MegaStarzWP7
{
    public partial class KaraokePage : PhoneApplicationPage
    {

        #region CTOR

        public KaraokePage()
        {
            DataContext = ((App) Application.Current).SongList;
            InitializeComponent();
        }

        #endregion

    }
}