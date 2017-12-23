using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using RoavVideoViewer.Model;

namespace RoavVideoViewer.Controls.Explorer
{
    /// <summary>
    /// Interaction logic for TripExplorerView.xaml
    /// </summary>
    public partial class MonthExplorerView : UserControl
    {
        private ObservableCollection<Trip> _trips;

        public MonthExplorerView()
        {
            InitializeComponent();
        }

        public ObservableCollection<Trip> Trips
        {
            get { return _trips; }
            set
            {
                _trips = value;
                ListViewTrips.ItemsSource = _trips;

                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ListViewTrips.ItemsSource);
                PropertyGroupDescription groupDescription = new PropertyGroupDescription("Date");
                view.GroupDescriptions.Add(groupDescription);
            }
        }

        protected void HandleTripDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var trip = ((ListViewItem)sender).Content as Trip;
            Curator.Instance.PlayNew((Trip)trip);
        }
    }
}
