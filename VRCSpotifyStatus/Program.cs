using System.Net;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Reflection;

namespace VRCSpotifyStatus
{
    internal class Program
    {
        private static protected string s_apiKey { get; set; } = "JlE5Jldo5Jibnk5O5hTx6XVqsJu4WJ26"; //ApiKey Never Changes
        public static string VRCUserEndpoint { get; set; } = "https://vrchat.com/api/1/users/";

        /* Changing the Status you must send the string that you want as a payload like 
                                                statusDescription: yourstring  so json */

        static void Main(string[] args)
        {
            Console.Title = "VRCSpotifyStatus";
            new Config();
            Console.ReadLine();
        }

        private static HttpWebRequest? _webReq { get; set; } = null;
        private static string _jsonStringObject { get; set; } = string.Empty;
        public static void PutReq(string url, string status)
        {
            try
            {
                _jsonStringObject = JsonConvert.SerializeObject((object)new { statusDescription = status });
                _webReq = (HttpWebRequest)WebRequest.Create(url);
                _webReq.CookieContainer = new CookieContainer();
                _webReq.CookieContainer.Add(new Cookie() { Name = "apiKey", Value = s_apiKey, Domain = "vrchat.com" });
                _webReq.CookieContainer.Add(new Cookie() { Name = "auth", Value = Config.s_json.AuthCookie, Domain = "vrchat.com" });
                _webReq.UserAgent = " Mozilla/5.0 (Linux; Android 6.0; Nexus 5 Build/MRA58N) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.5112.102 Mobile Safari/537.36";
                _webReq.ContentType = "application/json";
                _webReq.Accept = "Accept=application/json";
                _webReq.SendChunked = false;
                _webReq.ContentLength = _jsonStringObject.Length;
                _webReq.Method = "PUT";
                using (var writer = new StreamWriter(_webReq.GetRequestStream()))
                    writer.Write(_jsonStringObject);
                _webReq.GetResponse();
            }
            catch(Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex);
            }
        }
    }
}
