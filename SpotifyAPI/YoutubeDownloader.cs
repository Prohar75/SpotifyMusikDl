using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace SpotifyDownloader
{
    public class YouTubeApiSettings
    {
        public string ApiKey { get; set; }
        public string ApplicationName { get; set; }
    }
    public class YoutubeDownloader
    {
        /// <summary>
        /// Searches YouTube for a video and downloads its audio as MP3
        /// </summary>
        /// <returns>Task representing the async operation</returns>
        public static async Task SearchAndDownload(string prompt)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("config.json")
                .Build();

            var youtubeSettings = configuration.GetSection("YouTubeApiSettings").Get<YouTubeApiSettings>();

            if (string.IsNullOrEmpty(youtubeSettings?.ApiKey))
            {
                throw new Exception("YouTube API key is missing in configuration");
            }

            // Initialize YouTube API service with an API key
            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = youtubeSettings.ApiKey,
                ApplicationName = youtubeSettings.ApplicationName
            });

            // Configure search request
            var searchRequest = youtubeService.Search.List("snippet");
            searchRequest.Q = prompt;          // Set search query
            searchRequest.MaxResults = 1;      // Only get top result

            // Execute search and get first video ID
            var searchResponse = await searchRequest.ExecuteAsync();
            string videoId = null;

            // Extract video ID from search results
            foreach (var searchResult in searchResponse.Items)
            {
                videoId = searchResult.Id.VideoId;
                if (videoId != null) break;     // Take first valid ID
            }

            // Construct YouTube video URL
            string videoUrl = $"https://www.youtube.com/watch?v={videoId}";

            // Set up file paths
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string ytDlpPath = "Resources\\yt-dlp.exe";        // YouTube downloader tool
            string ffmpegPath = "Resources\\ffmpeg.exe";       // Audio conversion tool
            string outputFilePath = $"soundtracks\\{prompt}.mp3"; // Output file

            // Validate required tools exist
            if (!File.Exists(ytDlpPath))
            {
                Console.WriteLine("Error: yt-dlp.exe not found!");
                return;
            }

            if (!File.Exists(ffmpegPath))
            {
                Console.WriteLine("Error: ffmpeg.exe not found!");
                return;
            }

            // Build yt-dlp command arguments:
            // -x: extract audio
            // --audio-format mp3: convert to MP3
            // -o: output file path
            string arguments = $"--ffmpeg-location \"{ffmpegPath}\" -x --audio-format mp3 -o \"{outputFilePath}\" {videoUrl}";

            // Configure process startup
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = ytDlpPath,
                Arguments = arguments,
                RedirectStandardOutput = true,   // Capture output
                RedirectStandardError = true,    // Capture errors
                UseShellExecute = false,         // Don't use system shell
                CreateNoWindow = true            // No console window
            };

            // Console output positions
            int outputLine = 20;
            int errorLine = outputLine + 1;
            int statusLine = errorLine + 1;

            // Start and manage the download process
            using (Process process = new Process { StartInfo = startInfo })
            {
                // Handle output data
                process.OutputDataReceived += (sender, args) =>
                {
                    if (!string.IsNullOrEmpty(args.Data))
                    {
                        Console.SetCursorPosition(0, outputLine);
                        Console.Write(new string(' ', Console.WindowWidth - 1)); // Clear line
                        Console.SetCursorPosition(0, outputLine);
                        Console.WriteLine($"Output: {args.Data}".PadRight(Console.WindowWidth - 1));
                    }
                };

                // Handle error data
                process.ErrorDataReceived += (sender, args) =>
                {
                    if (!string.IsNullOrEmpty(args.Data))
                    {
                        Console.SetCursorPosition(0, errorLine);
                        Console.Write(new string(' ', Console.WindowWidth - 1));
                        Console.SetCursorPosition(0, errorLine);
                        Console.WriteLine($"Error: {args.Data}".PadRight(Console.WindowWidth - 1));
                    }
                };

                // Start process and begin async output reading
                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                // Show status
                Console.SetCursorPosition(0, statusLine);
                Console.WriteLine("Processing...".PadRight(Console.WindowWidth - 1));

                // Wait for process to complete
                process.WaitForExit();

                // Show completion status
                Console.SetCursorPosition(0, statusLine);
                Console.WriteLine($"Completed with exit code: {process.ExitCode}".PadRight(Console.WindowWidth - 1));
            }
        }
    }
}