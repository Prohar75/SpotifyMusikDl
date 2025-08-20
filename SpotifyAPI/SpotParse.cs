using Microsoft.Extensions.Configuration;
using SpotifyAPI.Web;
using SpotifyDownloader;
using SpotifyDownloader.entities;
using System.Text;
using System.Text.RegularExpressions;



public class SpotifyApiSettings
{
        public string ApiKey { get; set; }
        public string ApiSecret { get; set; }
}
class SpotParse
{
    private Track[] trackNames;
    protected SpotParse() { }

    /// <summary>
    /// Creates a list of sound-tracks
    /// </summary>
    protected async Task SetTrackList(string spotifyLink)
    {
        var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("config.json")
                    .Build();

        var spotifySettings = configuration.GetSection("SpotifyApiSettings").Get<SpotifyApiSettings>();

        if (string.IsNullOrEmpty(spotifySettings?.ApiKey))
        {
            throw new Exception("Spotify API key is missing in configuration");
        }

        var config = SpotifyClientConfig.CreateDefault();
        var request = new ClientCredentialsRequest(spotifySettings.ApiKey, spotifySettings.ApiSecret);
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
                    tracksList.Add(new Track(track.Name, string.Join(", ", track.Artists.Select(a => a.Name)))); // Adding an element with name of soundtrack and name of artist 
                }
            }
        }
        else if (isAlbum)
        {
            var albumTracks = await spotify.Albums.Get(spotifyId);
            foreach (var track in albumTracks.Tracks.Items)
            {
                tracksList.Add(new Track(track.Name, string.Join(", ", track.Artists.Select(a => a.Name)))); // Adding an element with name of soundtrack and name of artist 
            }
        }
        else if (isTrack)
        {
            var oneTrack = await spotify.Tracks.Get(spotifyId);
            tracksList.Add(new Track(oneTrack.Name, string.Join(", ", oneTrack.Artists.Select(a => a.Name)))); // Adding an element with name of soundtrack and name of artist 
        }
        else
        {
            throw new Exception("Invalid type of link.");
            return;
        }

        // Store results
        trackNames = tracksList.ToArray();
    }

    ///<summary>
    /// Parses Spotify link
    ///</summary>
    ///<returns>Returns an ID of soundtrack/album/playlist</returns>
    protected static string ParseLink(string spotifyLink)
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
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;
        string clientId = "671c8f05acea48c7b078077c93a1585b";
        string clientSecret = "16ffaec6e4924ed8a1518baed1cad33c";
        do // loop program
        {
            // Main screen UI
            AppUI.DrawMainScreen();

            try
            {
                // Enter link,parse it and then get a list or just a sound-track name
                string playlistId = Console.ReadLine();
                SpotParse parser = new SpotParse();
                await parser.SetTrackList(playlistId);
                var playlistNames = parser.GetTrackList();

                // Confirmation screen UI
                AppUI.DrawConfirmationScreen(playlistNames);

                bool exit = false;
                bool answer = true;
                do
                {
                    // 'Yes' or 'No' choise
                    ConsoleKeyInfo keyInfo = Console.ReadKey();
                    switch (keyInfo.Key)
                    {
                        case ConsoleKey.LeftArrow:
                            AppUI.YesResponse();
                            answer = true; //continues the loop
                            break;
                        case ConsoleKey.RightArrow:
                            AppUI.NoResponse(); // breaks the loop
                            answer = false;
                            break;
                        case ConsoleKey.Enter:
                            if (answer == false)
                            {
                                exit = true;
                                break;
                            }
                            else
                            {
                                int totalTracks = playlistNames.Length;
                                for (int i = 0; i <= totalTracks; i++)
                                {
                                    if (i < totalTracks)
                                    {
                                        double progressRatio = ((double)(i + 1)) / totalTracks; // Calculating percentage of maximum (totalTracks)
                                        AppUI.DrawDownloadProgress(i + 1, totalTracks, playlistNames[i], progressRatio); // Drawing progress bar based on previous calculations
                                        await YoutubeDownloader.SearchAndDownload($"{playlistNames[i].GetName()} - {playlistNames[i].GetArtist()}"); // Searcing and Downloading songs to .mp3
                                    }
                                    else
                                    {
                                        AppUI.DrawDownloadComplete(); // Download-Complete screen
                                    }
                                }
                                while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
                                exit = true;
                                break;
                            }
                        default:
                            break;
                    }
                } while (exit == false);
            }
            catch (Exception ex)
            {
                AppUI.DrawError(ex.Message); // Draws error message
                while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
            }
        } while (true);
    }
}