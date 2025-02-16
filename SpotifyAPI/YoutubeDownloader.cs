using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using VideoLibrary;

namespace SpotifyDownloader
{
    public class YoutubeDownloader
    {
        public static async Task Search(string prompt)
        {
            try
            {
                var youtubeService = new YouTubeService(new BaseClientService.Initializer()
                {
                    ApiKey = "AIzaSyAbj0W2avtbubv9ScKsi86f8RTK3cEYgk4",
                    ApplicationName = "Youtube-searchlink"
                });

                var searchRequest = youtubeService.Search.List("snippet");
                searchRequest.Q = prompt;
                searchRequest.MaxResults = 1;

                var searchResponse = await searchRequest.ExecuteAsync();
                string videoId = null;

                foreach (var searchResult in searchResponse.Items)
                {
                    videoId = searchResult.Id.VideoId;
                    if (videoId != null) break;
                }

                string videoUrl = $"https://www.youtube.com/watch?v={videoId}";

                // Set the output file path

                string basePath = AppDomain.CurrentDomain.BaseDirectory;

                string ytDlpPath = "Resources\\yt-dlp.exe";
                string ffmpegPath = "Resources\\ffmpeg.exe";
                string outputFilePath = $"soundtracks\\{prompt}.mp3";

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

                // Build the yt-dlp command
                string arguments = $"--ffmpeg-location \"{ffmpegPath}\"   -x --audio-format mp3 -o \"{outputFilePath}\" {videoUrl}";

                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = ytDlpPath,
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                Console.WriteLine($"Downloading {prompt}");
                using (Process process = new Process { StartInfo = startInfo })
                {
                    //process.OutputDataReceived += (sender, args) => Console.WriteLine(args.Data);
                    //process.ErrorDataReceived += (sender, args) => Console.WriteLine(args.Data);

                    process.Start();
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();
                    process.WaitForExit();
                }

                Console.WriteLine($"{prompt} downloaded successfully: " + outputFilePath);
            }catch(System.Exception e)
            {
                Console.Error.WriteLine($"Error while download {prompt}: {e.Message}.");
            }
        }
    }
}
