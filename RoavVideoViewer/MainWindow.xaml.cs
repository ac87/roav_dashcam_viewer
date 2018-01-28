using System;
using System.Globalization;
using System.IO;
using System.Windows;

namespace RoavVideoViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {       
        public static string HardDriveRoot = null;        

        public MainWindow()
        {
            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach (var drive in drives)
            {
                if (drive.IsReady && drive.VolumeLabel == "DASHCAM")
                {
                    HardDriveRoot = drive.RootDirectory.ToString();
                    break;
                }
            }

            if (HardDriveRoot == null)
            {
                throw new Exception("Hard disk not found");
            }

            InitializeComponent();                                  
          
            Curator curator = Curator.Instance;
            curator.MapView = MapView;
            curator.VideoView = VideoView;            

            ExplorerView.Trips = curator.TripCollection;        

            DataContext = this;
        }
        
        public void SetMaximised(bool maximise)
        {
            ColumnSeperatorColumnDefinition.Width = new GridLength(maximise ? 0 : 5);
            ExplorerColumnDefinition.Width = new GridLength(maximise ? 0 : 250);
        }

        internal void SetMapVisible(bool visible)
        {
            RowSeperatorRowDefinition.Height = new GridLength(visible ? 5 : 0);
            MapRowDefinition.Height = new GridLength(visible ? 300 : 0);
        }
    }
}
