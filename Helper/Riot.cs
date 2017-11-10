using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Helper
{
    public static class Riot
    {
        private static IDictionary<string, object> _Cache = new Dictionary<string, object>();

        /// <summary>
        /// Make a GET request to the URL.<para />
        /// It can replace the following variables: {ver}
        /// </summary>
        /// <param name="url">The URL</param>
        public static async Task<string> MakeRequest(string url, bool version = true)
        {
            if (version)
                url = url.Replace("{ver}", await GetLatestVersionAsync());

            WebRequest req = WebRequest.Create(url);
            var response = await req.GetResponseAsync();

            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                string str = await reader.ReadToEndAsync();
                return str;
            }
        }

        /// <summary>
        /// Tries to get key from cache. If it fails, execute the value function and store it
        /// </summary>
        /// <typeparam name="T">Type of returned value</typeparam>
        /// <param name="key">Cache key</param>
        /// <param name="value">Function that returns the expected value</param>
        private static async Task<T> TryGetCache<T>(string key, Func<Task<T>> value = null)
        {
            if (!_Cache.ContainsKey(key))
            {
                if (value == null)
                    return default(T);

                _Cache[key] = await value();
            }

            return (T)_Cache[key];
        }

        /// <summary>
        /// Get the latest LoL version
        /// </summary>
        public static async Task<string> GetLatestVersionAsync()
        {
            return await TryGetCache("LatestVersion", async () =>
            {
                string jsonStr = await MakeRequest("http://ddragon.leagueoflegends.com/api/versions.json", false);
                string[] json = JsonConvert.DeserializeObject<string[]>(jsonStr);

                return json.First();
            });
        }

        /// <summary>
        /// Get the names of all the champions
        /// </summary>
        public static async Task<string[]> GetChampionNamesAsync()
        {
            return await TryGetCache("ChampionNames", async () =>
            {
                string jsonStr = await MakeRequest("http://ddragon.leagueoflegends.com/cdn/{ver}/data/en_US/champion.json");
                JObject json = JsonConvert.DeserializeObject(jsonStr) as JObject;

                var data = json.Children().Last().Children().Single();
                List<string> ret = new List<string>();

                foreach (JProperty item in data)
                {
                    string champ = item.Name;
                    ret.Add(champ);
                }

                return ret.ToArray();
            });
        }

        /// <summary>
        /// See <see cref="GetChampionNamesAsync"/>
        /// </summary>
        public static string[] GetChampionNames()
        {
            return _Cache.TryGetValue("ChampionNames", out var val) ? val as string[] :
                GetChampionNamesAsync().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Get all the champions' square avatars
        /// </summary>
        public static async Task<Dictionary<string, Image>> GetChampionImagesAsync(int squareSize = -1)
        {
            string cacheName = "ChampionSquares" + (squareSize > 0 ? squareSize.ToString() : "");

            return (await TryGetCache(cacheName, async () =>
            {
                Dictionary<string, Image> ret = new Dictionary<string, Image>();

                string cachePath = "./data/imgCache/champSquare";
                if (!Directory.Exists(cachePath))
                    Directory.CreateDirectory(cachePath);

                WebClient client = new WebClient();

                foreach (var item in await GetChampionNamesAsync())
                {
                    try
                    {
                        string url =
                            $"http://ddragon.leagueoflegends.com/cdn/{await GetLatestVersionAsync()}/img/champion/{item}.png";
                        string cacheFile = cachePath + "/" + item + ".png";

                        if (!File.Exists(cacheFile))
                        {
                            await client.DownloadFileTaskAsync(url, cacheFile);
                        }

                        var image = Image.FromFile(cacheFile);

                        if (squareSize > 0)
                            image = ResizeBitmap(image as Bitmap, squareSize, squareSize);

                        ret.Add(item, image);
                    }
                    catch (Exception)
                    {
#if DEBUG
                        throw;
#else
                        continue;
#endif
                    }
                }

                return ret;
            })).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        public static Bitmap ResizeBitmap(this Bitmap Target, int width, int height)
        {
            lock (Target)
            {
                var destRect = new Rectangle(0, 0, width, height);
                var destImage = new Bitmap(width, height);

                destImage.SetResolution(Target.HorizontalResolution, Target.VerticalResolution);

                using (var graphics = Graphics.FromImage(destImage))
                {
                    graphics.CompositingMode = CompositingMode.SourceCopy;
                    graphics.CompositingQuality = CompositingQuality.HighSpeed;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.SmoothingMode = SmoothingMode.HighQuality;
                    graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                    using (var wrapMode = new ImageAttributes())
                    {
                        wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                        graphics.DrawImage(Target, destRect, 0, 0, Target.Width, Target.Height, GraphicsUnit.Pixel, wrapMode);
                    }
                }

                return destImage;
            }
        }
    }
}
