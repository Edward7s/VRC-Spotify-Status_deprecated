using Newtonsoft.Json;
using Microsoft.Win32;
using System.Reflection;
namespace VRCSpotifyStatus
{
    internal class Config
    {
        internal protected static Json s_json { get; set; }
        public class Json
        {
            public string? UserId { get; set; }
            public string? AuthCookie { get; set; }

        }
        private string _fileName { get; } = "\\VRCSpotifyConfig.json";
        public Config()
        {
            try
            {
                RegistryKey startup = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                if (startup.GetValue("VRCSpotifyManager") == null)
                    startup.SetValue("VRCSpotifyManager", Directory.GetCurrentDirectory() + "\\VRCSpotifyStatus.exe");



                if (!File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + _fileName))
                {
                    File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + _fileName, JsonConvert.SerializeObject(new Config.Json()
                    {
                        AuthCookie = String.Empty,
                        UserId = String.Empty,
                    }));
                    return;
                }
                s_json = JsonConvert.DeserializeObject<Config.Json>(File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + _fileName));
                Console.ForegroundColor = ConsoleColor.Red;
                if (s_json.AuthCookie == String.Empty)
                    Console.WriteLine("Please Add Your AuthCookie To The: " + Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" + _fileName + "  Then Restart The App");

                if (s_json.UserId == String.Empty)
                    Console.WriteLine("Please Add Your UserId To The: " + Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" + _fileName + "  Then Restart The App");
                Console.ForegroundColor = ConsoleColor.Green;

                new Spotify();
            }
            catch(Exception ex) { Console.WriteLine(ex); }
         

        }

    }
}
