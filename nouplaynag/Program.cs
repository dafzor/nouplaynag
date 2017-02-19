using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;
using System.IO;

namespace nouplaynag
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();

            // Retrives the first parameter that should be the path to the game executable.
            if (args.Length < 1)
            {
                MessageBox.Show("Must provide game exe path as parameter", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Try to work around people not putting paths surounded with ""
            var game_exe = String.Join(" ", args);
            game_exe = Path.GetFullPath(game_exe);
            Directory.SetCurrentDirectory(Path.GetDirectoryName(game_exe));


            // Start initial process
            var launch_process = Process.Start(game_exe);
            launch_process.WaitForExit();

            // Wait for 30s then look for the process again, this is needed because the way uplay launches a game
            // the game.exe starts uplaylauncher which then starts the game again.
            Thread.Sleep(30000);

            var game_name = Path.GetFileNameWithoutExtension(game_exe);
            var game_process = Process.GetProcessesByName(game_name)[0];
            game_process.WaitForExit();

            // Wait for the new game process to exit plus a few seconds so Uplay actually shows up.
            Thread.Sleep(2000);

            // Request uplay to close.
            SendCloseMessage("upc");


        }

        static void SendCloseMessage(string processName)
        {
            var process = Process.GetProcessesByName(processName)[0];
            SendMessage(process.MainWindowHandle, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
        }

        public static uint WM_CLOSE = 0x10;
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);
    }
}
