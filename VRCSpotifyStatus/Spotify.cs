using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
namespace VRCSpotifyStatus
{
    internal class Spotify
    {
        public Spotify()
        {
            if (Config.s_json.AuthCookie == String.Empty || Config.s_json.UserId == String.Empty) return;
            Console.WriteLine("Starting Loops For Spotify.");
            Task.Run(SpootifyLoop);
        }

        private Process? _spotify { get; set; }
        private string _lastSong { get; set; } = string.Empty;
        private bool CheckPaused(string title)
        {
            if (title == "Spotify Free" || title == "Spotify Premium")
                return true;
            return false;
        }

        private Action SpootifyLoop()
        {
            while (true)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                Thread.Sleep(3000);
                if (Process.GetProcesses().FirstOrDefault(x => x.ProcessName == "VRChat") == null)
                {
                    Thread.Sleep(10000);
                    continue;
                }
                _spotify = Process.GetProcesses().FirstOrDefault(x => x.ProcessName == "Spotify");
                if (_spotify == null)
                {
                    if (_lastSong == "0Invalid") continue;
                    _lastSong = "0Invalid";
                    Program.PutReq(Program.VRCUserEndpoint + Config.s_json.UserId, "Not listening to music.");
                    continue;
                }
                if (_spotify.MainWindowTitle == _lastSong) continue;
                _lastSong = _spotify.MainWindowTitle;
                Program.PutReq(Program.VRCUserEndpoint + Config.s_json.UserId, CheckPaused(_spotify.MainWindowTitle) ? "Spotify Paused." : (_spotify.MainWindowTitle.Length >= 32 ?  _spotify.MainWindowTitle.Remove(0, _spotify.MainWindowTitle.Length -32) : _spotify.MainWindowTitle));
            }
            return null;
        }
    }
}
