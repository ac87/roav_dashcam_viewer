using RoavVideoViewer.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RoavVideoViewer.Controls.Explorer
{
    /// <summary>
    /// Interaction logic for TripExplorerView.xaml
    /// </summary>
    public partial class TripExplorerView : UserControl
    {
        //private ObservableCollection<Video> videos;

        public TripExplorerView()
        {
            InitializeComponent();
        }

        //public Trip Trip
        //{
        //    get { return (Trip)GetValue(TripProperty); }
        //    set
        //    {
        //        SetValue(TripProperty, value);
        //        ListViewVideos.ItemsSource = value.Videos;
        //    }
        //}
   
        protected void HandleVideoDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var video = ((ListViewItem)sender).Content as Video;
            Curator.Instance.PlayNew((Trip)DataContext, video, true);
            e.Handled = true;
        }

        protected void HandleVideoRightClick(object sender, MouseButtonEventArgs e)
        {
            var video = ((ListViewItem)sender).Content as Video;
            Curator.Instance.PlayNew((Trip)DataContext, video, true);
        }
    }
}
