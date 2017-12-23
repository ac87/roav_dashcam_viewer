using System;
using System.Collections.Generic;
using System.IO;
using RoavVideoViewer.Helpers;

namespace RoavVideoViewer.Model
{
    public class Trip
    {
        public DateTime StartDateTime { get; set; } = DateTime.MaxValue;

        public DateTime EndDateTime { get; set; } = DateTime.MinValue;

        public string Date { get { return StartDateTime.ToString("dddd d MMMM yyyy"); } }

        public string StartTime { get { return StartDateTime.ToShortTimeString(); } }

        public string EndTime { get { return EndDateTime.ToShortTimeString(); } }

        public List<Video> Videos { get; set; } = new List<Video>();

        public String ThumbnailPath
        {
            get
            {
                if (Videos[0] != null && File.Exists(Videos[0].ThumbnailPath))
                    return Videos[0].ThumbnailPath;
                return Utils.GetNoThumbnailPath();
            }
        }
    }
}
