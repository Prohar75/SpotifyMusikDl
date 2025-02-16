using SpotifyAPI.Web;
using System.Text.RegularExpressions;
using SpotifyDownloader.entities;
using SpotifyDownloader;

class SpotParse
{
    private string clientId;
    private string clientSecret;
    private Track[] trackNames;
    protected SpotParse(string clientId, string clientSecret)
    {
        this.clientId = clientId;
        this.clientSecret = clientSecret;
    }

    protected async Task SetTrackList(string spotifyLink)
    {
        var config = SpotifyClientConfig.CreateDefault();
        var request = new ClientCredentialsRequest(clientId, clientSecret);
        var response = await new OAuthClient(config).RequestToken(request);

        var spotify = new SpotifyClient(config.WithToken(response.AccessToken));

        // Check if it's a playlist or album
        string spotifyId = ParseLink(spotifyLink);
        bool isPlaylist = spotifyLink.Contains("playlist");
        bool isAlbum = spotifyLink.Contains("album");
        bool isTrack = spotifyLink.Contains("track");

        List<Track> tracksList = new List<Track>();

        if (isPlaylist)
        {
            var playlistTracks = await spotify.Playlists.GetItems(spotifyId);
            foreach (var item in playlistTracks.Items)
            {
                var track = item.Track as FullTrack;
                if (track != null)
                {
                    tracksList.Add(new Track(track.Name, string.Join(", ", track.Artists.Select(a => a.Name))));
                }
            }
        }
        else if (isAlbum)
        {
            var albumTracks = await spotify.Albums.Get(spotifyId);
            foreach (var track in albumTracks.Tracks.Items)
            {
                tracksList.Add(new Track(track.Name, string.Join(", ", track.Artists.Select(a => a.Name))));
            }
        }
        else if (isTrack)
        {
            var oneTrack = await spotify.Tracks.Get(spotifyId);
            tracksList.Add(new Track(oneTrack.Name, string.Join(", ", oneTrack.Artists.Select(a => a.Name))));
        }
        else
        {
            Console.WriteLine("❌ Invalid Spotify link! Must be a playlist or album.");
            return;
        }

        // Store results
        trackNames = tracksList.ToArray();
    }

    protected string ParseLink(string spotifyLink)
    {
        string pattern = @"(?:playlist|album|track)/([A-Za-z0-9]+)"; 
        Match match = Regex.Match(spotifyLink, pattern);

        if (match.Success)
        {
            return match.Groups[1].Value; // Returns only the extracted ID
        }

        return spotifyLink; // If input is already an ID, return it as is
    }


    protected Track[] GetTrackList()
    {
        return trackNames;
    }
    

    static async Task Main(string[] args)
    {
        try
        {
            string clientId = "671c8f05acea48c7b078077c93a1585b";
            string clientSecret = "16ffaec6e4924ed8a1518baed1cad33c";

            Console.Write("Playlist Link: ");
            string playlistId = Console.ReadLine();
        
            SpotParse parser = new SpotParse(clientId, clientSecret);
            await parser.SetTrackList(playlistId);
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            var playlistNames = parser.GetTrackList();
            Console.WriteLine("|----------------------");
            foreach (var trackName in playlistNames)
            {
                Console.WriteLine($"|Name   - {trackName.GetName()}\n|Artist - {trackName.GetArtist()}");
                Console.WriteLine("|----------------------");
            }
            Console.Write("Does this data correct? y/n: ");
            bool exit = false;
            do
            {
                string answer = Console.ReadLine();
                switch (answer)
                {
                    case "y":
                        foreach (var trackName in playlistNames)
                        {
                            await YoutubeDownloader.Search($"{trackName.GetName()} - {trackName.GetArtist()}");
                        }
                        exit = true;
                        break;
                    case "n":
                        exit = true;
                        return;
                    default:
                        Console.Write("Incorrect type of answer. You can use only y/n: ");
                        break;
                }
            } while (exit == false);
        }
        catch (System.Exception e)
        {
            Console.Write($"Error: {e.Message}");
        }
    }
}