using RoavVideoViewer.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Accord.Video.FFMPEG;
using System.Drawing.Imaging;
using System.Drawing;
using System.Linq;
using RoavVideoViewer.Helpers;

namespace RoavVideoViewer
{
    public class Curator
    {
        private static Curator _instance;

        public static Curator Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Curator();
                return _instance;
            }
        }

        public ObservableCollection<Trip> TripCollection { get; } = new ObservableCollection<Trip>();

        public MapViewer MapView { get; internal set; }

        public VideoViewer VideoView { get; internal set; }

        private Trip _currentTrip;
        private Video _currentVideo;        

        //private DateTime _dateLoaded;
        
        private string _folder;

        private int _index;

        public void LoadFolder(string folder)
        {
            TripCollection.Clear();

            _folder = folder;
            DirectoryInfo dir = new DirectoryInfo(folder);

            DateTime currentDate = DateTime.MinValue;

            Trip trip = null;

            if (!Directory.Exists(folder))            
                return;

            foreach (var file in dir.GetFiles("*.mp4"))
            {
                string fileName = Path.GetFileNameWithoutExtension(file.FullName);
                var fileStartDateTime = DateTime.ParseExact(fileName.Substring(0, fileName.LastIndexOf("_")), "yyyy_MMdd_HHmmss", null);
                
                if (currentDate.Date != fileStartDateTime.Date)
                {
                    if (trip != null)
                        TripCollection.Add(trip);
                    trip = new Trip();
                }
                else if (fileStartDateTime.Subtract(currentDate) > TimeSpan.FromMinutes(10))
                {
                    TripCollection.Add(trip);
                    trip = new Trip();
                }
                currentDate = fileStartDateTime;

                string jpg = Utils.GetFileNameWithoutExt(file.FullName) + ".jpg";

                if (!File.Exists(jpg)) {

                    try
                    {
                        VideoFileReader reader = new VideoFileReader();
                        reader.Open(file.FullName);
                        Bitmap frame = reader.ReadVideoFrame();
                        reader.Close();

                        frame.Save(jpg, ImageFormat.Jpeg);
                    }
                    catch (Exception ex)
                    {

                    }
                }                

                if (trip.StartDateTime > currentDate)
                    trip.StartDateTime = currentDate;
                if (trip.EndDateTime < currentDate)
                    trip.EndDateTime = currentDate;

                Video video = new Video() { FilePath = folder + file.Name, Date = fileStartDateTime, ThumbnailPath = jpg };

                trip.Videos.Add(video);
            }

            if (trip != null)
                TripCollection.Add(trip);
        }

        internal void PlayNew(Trip trip)
        {
            PlayNew(trip, trip.Videos[0], true);
        }

        public void PlayNew(Trip trip, Video video, bool jumpTo = false)
        {
            _currentTrip = trip;        
            MapView.AddTripTrails(trip);

            PlayVideo(video, jumpTo);          
        }

        public void PlayNext(Video video, bool jumpTo = false, int secondsOffset = 0)
        {
            PlayVideo(video, jumpTo, secondsOffset);
        }

        private void PlayVideo(Video video, bool jumpTo = false, int secondsOffset = 0)
        {
            _index = _currentTrip.Videos.IndexOf(video);

            //if (_currentVideo != null)
            //    _currentVideo.IsPlaying = false;
            _currentVideo = video;
            //_currentVideo.IsPlaying = true;

            VideoView.Play(video, secondsOffset);
            //MapView.AddTrail(video.TrailItems.Values.ToList());

            if (jumpTo && video.TrailItems.ContainsKey(0))
                MapView.MoveTo(video.TrailItems[0].Latitude, video.TrailItems[0].Longitude);
        }        

        public TrailItem UpdateVideoPosition(int seconds)
        {
            if (_currentVideo.TrailItems != null)
            {
                if (_currentVideo.TrailItems.ContainsKey(seconds))
                {
                    TrailItem trailItem = _currentVideo.TrailItems[seconds];
                    if (trailItem.Latitude != 0 && trailItem.Longitude != 0)
                    {
                        MapView.AddPoint(trailItem.Latitude, trailItem.Longitude, trailItem.SpeedMph);
                        return trailItem;
                    }
                }
            }

            return null;
        }

        internal void PlayPrevious()
        {
            if (_currentTrip != null && _index != -1 && _index > 0)
                PlayNext(_currentTrip.Videos[_index - 1], false, 0);
        }

        internal void PlayNext()
        {
            if (_currentTrip != null && _index != -1 && _index < _currentTrip.Videos.Count - 1)
                PlayNext(_currentTrip.Videos[_index + 1], false, 1);
        }
    }
}
