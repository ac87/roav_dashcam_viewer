using System;
using System.Collections.Generic;
using System.IO;
using RoavVideoViewer.Helpers;

namespace RoavVideoViewer.Model
{
    public class Video
    {
        public string FilePath { get; set; }   

        public DateTime Date { get; set; }

        public String StartTime { get { return Date.ToShortTimeString(); } }

        public String ThumbnailPath { get; set; }

        private Dictionary<int, TrailItem> _trailItems;

        public Dictionary<int, TrailItem> TrailItems
        {
            get
            {
                if (_trailItems == null)
                {
                    _trailItems = new Dictionary<int, TrailItem>();
                    LoadTrail();
                }
                return _trailItems;
            }
        } 

        public void LoadTrail()
        {
            if (String.IsNullOrEmpty(FilePath))
                return;

            string infoFile = Utils.GetFileNameWithoutExt(FilePath) + ".info";
            if (!File.Exists(infoFile))
                return;

            DateTime startDateTime = DateTime.MinValue;
            
            var lines = File.ReadAllLines(infoFile);
            foreach (string line in lines)
            {
                TrailItem trailItem = new TrailItem();
                var split = line.Split(new[] { ',' });                               

                var dateTime = DateTime.ParseExact(split[0], "yyyyMMdd_HH:mm:ss.fff", null);
                if (startDateTime == DateTime.MinValue)
                    startDateTime = dateTime;

                int seconds = (int)Math.Floor(dateTime.Subtract(startDateTime).TotalSeconds);
                trailItem.Latitude = double.Parse(split[1]);
                trailItem.Longitude = double.Parse(split[2]);
                trailItem.Fixed = int.Parse(split[3]) == 1;
                trailItem.SatCount = int.Parse(split[4]);
                
                trailItem.SpeedKph = double.Parse(split[6]);
                trailItem.Heading = double.Parse(split[7]);

                if (!_trailItems.ContainsKey(seconds))
                    _trailItems.Add(seconds, trailItem);
            }
        }
    }
}
