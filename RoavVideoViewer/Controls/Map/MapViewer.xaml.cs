using Mapsui;
using Mapsui.Geometries;
using Mapsui.Layers;
using Mapsui.Projection;
using Mapsui.Providers;
using Mapsui.Styles;
using RoavVideoViewer.Model;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BruTile.Predefined;
using Mapsui.UI;
using RoavVideoViewer.Controls.Map;
using Point = Mapsui.Geometries.Point;
using Video = RoavVideoViewer.Model.Video;

namespace RoavVideoViewer
{
    /// <summary>
    /// Interaction logic for MapViewer.xaml
    /// </summary>
    public partial class MapViewer : UserControl
    {
        private readonly Map _map;

        private ILayer _tripTrailLayer;
        private ILayer _tripInfoLayer;

        private readonly ILayer _positionLayer;

        private bool _track = true;

        public bool Track
        {
            get {  return _track; }
            set
            {
                ButtonTrack.Visibility = value ? Visibility.Collapsed : Visibility.Visible;
                _track = value;
            }
        }

        public MapViewer()
        {
            InitializeComponent();
            _map = MapControl.Map;

            System.Windows.Media.Color color = (System.Windows.Media.Color) FindResource("WindowBackgroundColor");
            _map.BackColor = Color.FromArgb(255, color.R, color.G, color.B);

            MapControl.PreviewMouseDown += MapControl_PreviewMouseDown;
            MapControl.PreviewMouseWheel += MapControl_PreviewMouseWheel;

            //_map.Layers.Add(OpenStreetMap.CreateTileLayer());
            _map.Layers.Add(new TileLayer(KnownTileSources.Create(KnownTileSource.EsriWorldDarkGrayBase)) { Name = "Map" });
            //_map.Layers.Add(new TileLayer(KnownTileSources.Create(KnownTileSource.BingAerial)) { Name = "Bing Aerial" });

            _positionLayer = CreatePositionLayer();
            _map.Layers.Add(_positionLayer);
            
            _map.Info += MapOnInfo;
        }

        private void MapOnInfo(object sender, InfoEventArgs infoEventArgs)
        {
            if (infoEventArgs.Feature != null && infoEventArgs.Feature["Video"] != null)
            {
                Video video = (Video) infoEventArgs.Feature["Video"];
                Curator.Instance.PlayNext(video);
            }
        }

        private void MapControl_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            Track = false;
        }

        private void MapControl_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Track = false;
        }

        internal void MoveToArea(double latitude, double longitude, double offset = 0.01)
        {            
            MapControl.ZoomToBox(SphericalMercator.FromLonLat(longitude - offset, latitude - offset), SphericalMercator.FromLonLat(longitude + offset, latitude + offset));                        
        }

        internal void MoveToPoints(List<Mapsui.Geometries.Point> points)
        {
            MapControl.Map.NavigateTo(new BoundingBox(points));
        }

        internal void MoveTo(double latitude, double longitude, int zoom = -1)
        {
            if (latitude == 0 && longitude == 0)
                return;

            var sphericalMercatorCoordinate = SphericalMercator.FromLonLat(longitude, latitude);
            _map.NavigateTo(sphericalMercatorCoordinate);
            if (zoom > 0)
                _map.NavigateTo(_map.Resolutions[zoom]);
        }

        public void AddTripTrails(Trip trip)
        {
            if (_tripTrailLayer != null)
                _map.Layers.Remove(_tripTrailLayer);

            if (_tripInfoLayer != null)
                _map.Layers.Remove(_tripInfoLayer);

            Features trailFeatures = new Features();
            Features infoFeatures = new Features();

            int lastSpeedColorIndex = -1;
            Point lastPoint = null;
            List<Point> points = new List<Point>();

            foreach (Video video in trip.Videos)
            {
                bool first = true;

                foreach (TrailItem item in video.TrailItems.Values.ToList())
                {
                    var location = SphericalMercator.FromLonLat(item.Longitude, item.Latitude);

                    if (first)
                    {
                        infoFeatures.Add(new Feature()
                        {
                            Geometry = location,
                            ["Video"] = video
                        });
                        first = false;
                    }

                    if (lastPoint != null && location.Distance(lastPoint) < 35)
                        continue;

                    lastPoint = location;
                    int index = MapColors.GetStyleIndexFromSpeed(item.SpeedMph);
                        
                    if (index != lastSpeedColorIndex)
                    {
                        if (lastSpeedColorIndex != -1)
                            points.Add(location);

                        if (points.Count > 0)
                        {
                            List<IStyle> styleList = new List<IStyle> { MapColors.LineStyles[lastSpeedColorIndex] };
                            trailFeatures.Add(new Feature()
                            {
                                Geometry = new LineString(points),
                                Styles = styleList
                            });
                        }
                        points.Clear();
                    }

                    points.Add(location);
                    lastSpeedColorIndex = index;
                }
            }

            _tripTrailLayer = new MemoryLayer
            {
                DataSource = new MemoryProvider() { Features = trailFeatures },
                Name = "TripLayer",
                Style = null
            };

            _tripInfoLayer = new MemoryLayer
            {
                DataSource = new MemoryProvider() {Features = infoFeatures},
                Name = "MoviesLayer",
                Style = new SymbolStyle
                {
                    SymbolScale = 0.6,
                    SymbolOffset = new Offset(0, 0),
                    SymbolType = SymbolType.Rectangle,
                    Line = new Pen(Color.Black, 2),
                    Fill = new Brush(Color.White),
                    Opacity = 0.75
                }
            };

            _map.Layers.Insert(1, _tripTrailLayer);

            _map.Layers.Insert(1, _tripInfoLayer);
            _map.InfoLayers.Add(_tripInfoLayer);
        }

        public void AddPoint(double lat, double lon)
        {
            if (_positionLayer != null)
            {
                MemoryProvider dataSource = ((MemoryProvider)((MemoryLayer)_positionLayer).DataSource);
                dataSource.Features.Clear();                
                dataSource.Features.Add(new Feature() { Geometry = SphericalMercator.FromLonLat(lon, lat) });
                MapControl.RefreshData();

                if (_track)
                    MoveToArea(lat, lon);
            }
        }

        public static ILayer CreatePositionLayer()
        {
            return new MemoryLayer
            {
                DataSource = new MemoryProvider(),
                Name = "PositionLayer",
                Style = new SymbolStyle { SymbolScale = 0.4, SymbolOffset = new Offset(0, 0), SymbolType = SymbolType.Ellipse, Fill = new Mapsui.Styles.Brush(Mapsui.Styles.Color.Blue) },
            };
        }

        private void ButtonTrack_Click(object sender, RoutedEventArgs e)
        {
            Track = true;
        }
    }
}
