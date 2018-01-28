using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using RoavVideoViewer.Model;
using System;
using System.Globalization;

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

            var date = DateTime.Now;
            ComboBoxYear.SelectedItem = date.Year.ToString();

            var month = date.ToString("MMM", CultureInfo.InvariantCulture);
            ComboBoxMonth.SelectedItem = month;            
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

        private void ComboBoxMonth_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxYear.SelectedItem != null && ComboBoxMonth.SelectedItem != null)
                Curator.Instance.LoadFolder(MainWindow.HardDriveRoot + ComboBoxYear.SelectedItem.ToString() + "\\" + ComboBoxMonth.SelectedItem.ToString() + "\\");
        }
    }
}
