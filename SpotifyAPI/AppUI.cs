using SpotifyDownloader.entities;

namespace SpotifyDownloader
{
    /// <summary>
    /// Handles all user interface rendering for the Spotify Downloader application
    /// </summary>
    internal class AppUI
    {
        /// <summary>
        /// Draws the main application screen with logo and input prompt
        /// </summary>
        public static void DrawMainScreen()
        {
            Console.Clear();
            Console.SetBufferSize(120, 30);  // Set fixed console size
            Console.CursorVisible = true;    // Show cursor for input

            // Draw the main decorative frame
            Console.WriteLine(
            "\n\n\t\t\t▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄" +
            "\n\t\t\t█▒░ ╔═══════════════════════════════════════════════════════════╗ ░▒█" +
            "\n\t\t\t█▒░ ║                                                           ║ ░▒█" +
            "\n\t\t\t█▒░ ║                                                           ║ ░▒█" +
            "\n\t\t\t█▒░ ║                                                           ║ ░▒█" +
            "\n\t\t\t█▒░ ║                                                           ║ ░▒█" +
            "\n\t\t\t█▒░ ║                                                           ║ ░▒█" +
            "\n\t\t\t█▒░ ║                                                           ║ ░▒█" +
            "\n\t\t\t█▒░ ║                                                           ║ ░▒█" +
            "\n\t\t\t█▒░ ║                                                           ║ ░▒█" +
            "\n\t\t\t█▒░ ║                                                           ║ ░▒█" +
            "\n\t\t\t█▒░ ║                                                           ║ ░▒█" +
            "\n\t\t\t█▒░ ║                                                           ║ ░▒█" +
            "\n\t\t\t█▒░ ║                                                           ║ ░▒█" +
            "\n\t\t\t█▒░ ║                                                           ║ ░▒█" +
            "\n\t\t\t█▒░ ╚═══════════════════════════════════════════════════════════╝ ░▒█" +
            "\n\t\t\t▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀" +
            "\n\t\t\t╔═══════════════════════════════════════════════════════════════════╗" +
            "\n\t\t\t║                                                                   ║" +
            "\n\t\t\t╚═══════════════════════════════════════════════════════════════════╝");

            // Draw all screen components
            DrawAppLogo();      // Application logo (Spotify + MDL text art)
            DrawAuthor();       // "by Prohar" credit
            DrawInputPrompt();  // Input instructions
        }

        private static void DrawAppLogo()
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.SetCursorPosition(32, 5);
            Console.WriteLine("███████╗██████╗  ██████╗ ████████╗██╗███████╗██╗   ██╗");
            Console.SetCursorPosition(32, 6);
            Console.WriteLine("██╔════╝██╔══██╗██╔═══██╗╚══██╔══╝██║██╔════╝╚██╗ ██╔╝");
            Console.SetCursorPosition(32, 7);
            Console.WriteLine("███████╗██████╔╝██║   ██║   ██║   ██║█████╗   ╚████╔╝");
            Console.SetCursorPosition(32, 8);
            Console.WriteLine("╚════██║██╔═══╝ ██║   ██║   ██║   ██║██╔══╝    ╚██╔╝");
            Console.SetCursorPosition(32, 9);
            Console.WriteLine("███████║██║     ╚██████╔╝   ██║   ██║██║        ██║");
            Console.SetCursorPosition(32, 10);
            Console.WriteLine("╚══════╝╚═╝      ╚═════╝    ╚═╝   ╚═╝╚═╝        ╚═╝");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.SetCursorPosition(56, 11);
            Console.WriteLine("███╗   ███╗██████╗ ██╗");
            Console.SetCursorPosition(56, 12);
            Console.WriteLine("████╗ ████║██╔══██╗██║");
            Console.SetCursorPosition(56, 13);
            Console.WriteLine("▓▓╔▓▓▓▓╔▓▓║▓▓║  ▓▓║▓▓║");
            Console.SetCursorPosition(56, 14);
            Console.WriteLine("▒▒║╚▒▒╔╝▒▒║▒▒║  ▒▒║▒▒║");
            Console.SetCursorPosition(56, 15);
            Console.WriteLine("░░║ ╚═╝ ░░║░░░░░░╔╝░░░░░░░╗");
            Console.SetCursorPosition(56, 16);
            Console.WriteLine("╚═╝     ╚═╝╚═════╝ ╚══════╝");
            Console.ForegroundColor = ConsoleColor.White;
        }

        private static void DrawAuthor()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(32, 16);
            Console.WriteLine("by Prohar");
        }

        private static void DrawInputPrompt()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.SetCursorPosition(26, 20);
            Console.WriteLine("type playlist/soundtrack link here....");
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(26, 20); // Position cursor for input
        }

        /// <summary>
        /// Formats text to fit within a specified width, adding ellipsis if truncated
        /// </summary>
        /// <returns>Formatted string</returns>
        private static string PrintWrapped(string input, int maxCharsPerLine)
        {
            string line = "";
            // Build line character by character up to max length
            for (int i = 0; i < maxCharsPerLine; i++)
            {
                if (i < input.Length)
                {
                    line += input[i];
                }
                else
                {
                    break;
                }
            }
            // Add ellipsis if text was truncated
            if (input.Length > maxCharsPerLine)
            {
                line += "...";
            }
            return line;
        }

        /// <summary>
        /// Draws the track confirmation screen with list of tracks
        /// </summary>
        public static void DrawConfirmationScreen(Track[] playlistNames)
        {
                Console.Clear();
                Console.CursorVisible = false;

                // Adjust console buffer size based on track count
                if (playlistNames.Length * 3 + 20 < 30)
                    Console.SetBufferSize(120, 30);
                else
                    Console.SetBufferSize(120, playlistNames.Length * 3 + 20);

                // Draw confirmation dialog frame
                Console.WriteLine(
                               "\t\t\t\t▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄\n" +
                               "\t\t\t\t█▓▒░                                                 ░▒▓█\n" +
                               "\t\t\t\t█▓▒░              Is this data correct?              ░▒▓█\n" +
                               "\t\t\t\t█▓▒░                                                 ░▒▓█\n" +
                               "\t\t\t\t█▓▒░            yes                   no             ░▒▓█\n" +
                               "\t\t\t\t█▓▒░                                                 ░▒▓█\n" +
                               "\t\t\t\t▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀\n" +
                               "\n\n\t\t\t\t\t+---------------------------------------+");

                // Display each track with formatted name and artist
                foreach (var trackName in playlistNames)
                {
                    Console.WriteLine($"\t\t\t\t\t|Name   - {PrintWrapped(trackName.GetName(), 25)}\n\t\t\t\t\t|Artist - {PrintWrapped(trackName.GetArtist(), 25)}");
                    Console.WriteLine("\t\t\t\t\t+---------------------------------------+");
                }

                // Draw right border
                for (int i = 9; i <= playlistNames.Length * 3 + 9; i++)
                {
                    Console.SetCursorPosition(80, i);
                    if (i % 3 == 0) Console.Write('+');
                    else Console.Write('|');
                }

                Console.SetCursorPosition(0, 0);
        }

        public static void YesResponse()
        {
            Console.SetCursorPosition(68, 4);
            Console.WriteLine("  no  ");
            Console.SetCursorPosition(46, 4);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("< yes >");
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(0, 0);
        }

        public static void NoResponse()
        {
            Console.SetCursorPosition(46, 4);
            Console.WriteLine("  yes  ");
            Console.SetCursorPosition(68, 4);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("< no >");
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(0, 0);
        }

        /// <summary>
        /// Draws the download progress screen with progress bar
        /// </summary>
        public static void DrawDownloadProgress(int current, int total, Track track, double progressRatio)
        {
            Console.Clear();
            // Draw progress frame
            Console.WriteLine(
                "\n\n\n\n\n\n\n\n\n\n" +
                "\t\t\t    Start Downloading...\n" +
                $"\t\t\t  ╔══════════════════════════════════════════════════════════════════╗\n" +
                $"\t\t\t  ║                                                                  ║\n" +
                $"\t\t\t  ╚══════════════════════════════════════════════════════════════════╝\n");

            // Calculate and draw progress bar
            int barWidth = 64;
            int filledWidth = (int)(barWidth * progressRatio);
            string progressBar = new string('█', filledWidth);

            Console.SetCursorPosition(28, 12);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"{progressBar}");
            Console.ForegroundColor = ConsoleColor.White;

            // Display current track info
            Console.SetCursorPosition(28, 10);
            Console.WriteLine($"Downloading {current}/{total} :  {track.GetName()} - {track.GetArtist()}\n");
        }

        /// <summary>
        /// Draws the download completion message
        /// </summary>
        public static void DrawDownloadComplete()
        {
            Console.SetCursorPosition(28, 10);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"Download Completed Successfully!                                                                      ");
            Console.ForegroundColor = ConsoleColor.White;

            Console.SetCursorPosition(40, 15);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"press \"Enter\" to return to main menu.");
            Console.ForegroundColor = ConsoleColor.White;
        }

        /// <summary>
        /// Draws an error message
        /// </summary>
        public static void DrawError( string message)
        {
            Console.Clear();
            Console.CursorVisible = false;
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(
               "\n\n\n\n\n\n\n\n\t\t\t\t\t\t\tError:");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"\n\t\t\t\t\t\t{message}");

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"\n\t\t\t\t\t\t 'Enter' to continue.");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}