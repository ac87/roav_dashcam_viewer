using System.Windows;

namespace RoavVideoViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {       
        public const string Folder = @"C:\Users\Tony\Desktop\MOVIE\";

        public MainWindow()
        {
            InitializeComponent();

            Curator curator = Curator.Instance;
            curator.MapView = MapView;
            curator.VideoView = VideoView;
            curator.LoadFolder(Folder);

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
