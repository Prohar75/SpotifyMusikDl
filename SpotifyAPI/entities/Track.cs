using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyDownloader.entities
{
    public class Track
    {
        private string name;
        private string artist;

        public Track(string name, string artist)
        {
            this.name = name;
            this.artist = artist;
        }

        public string GetName()
        {
            return name;
        }

        public string GetArtist()
        {
            return artist;
        }
    }
}
