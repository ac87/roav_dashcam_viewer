namespace RoavVideoViewer.Model
{
    public class TrailItem
    {
        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public bool Fixed { get; set; }

        public int SatCount { get; set; }

        public double Heading { get; set; }

        public double SpeedKph { get; set; }

        public double SpeedMph { get { return SpeedKph * 0.621371; } }
    }
}
