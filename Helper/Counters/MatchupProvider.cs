using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Counters
{
    public abstract class MatchupProvider
    {
        public struct Matchup
        {
            public enum MatchupType
            {
                WeakAgainst,
                StrongAgainst
            }

            public string Champion;
            public string Against;
            public MatchupType Type;

            public Matchup(string champ, string against, MatchupType type)
            {
                this.Champion = champ;
                this.Against = against;
                this.Type = type;
            }
        }

        private List<Matchup> _Cache = null;


        public abstract string GetProviderName();
        public abstract string GetProviderHomeUrl();
        protected abstract IEnumerable<Matchup> GetMatchupsInner(string champion);


        public async Task<IEnumerable<Matchup>> GetMatchupsForChampionAsync(string champion)
            => await Task.Run(() => GetMatchupsForChampion(champion));
        
        public IEnumerable<Matchup> GetMatchupsForChampion(string champion)
        {
            if (_Cache == null)
                LoadCache();

            var cache = _Cache.Where(o => o.Champion == champion);

            if (!cache.Any())
            {
                cache = GetMatchupsInner(champion);
                _Cache.AddRange(cache);

                SaveCache();
            }

            return cache;
        }

        private void LoadCache()
        {
            string provider = this.GetType().Name;
            string filePath = Path.Combine("data", "dataCache", provider + ".json");

            if (!File.Exists(filePath))
            {
                _Cache = new List<Matchup>();
                SaveCache();
                return;
            }

            _Cache = JsonConvert.DeserializeObject<List<Matchup>>(File.ReadAllText(filePath));
        }

        private void SaveCache()
        {
            if (_Cache == null)
                return;

            string provider = this.GetType().Name;
            string filePath = Path.GetFullPath(Path.Combine("data", "dataCache", provider + ".json"));
            string directory = Path.GetDirectoryName(filePath);

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            File.WriteAllText(filePath, JsonConvert.SerializeObject(_Cache));
        }
    }
}
