using System;
using System.Collections.Generic;
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

        private List<Matchup> _Cache = new List<Matchup>();


        public abstract string GetProviderName();
        public abstract string GetProviderHomeUrl();
        protected abstract IEnumerable<Matchup> GetMatchupsInner(string champion);


        public async Task<IEnumerable<Matchup>> GetMatchupsForChampionAsync(string champion)
            => await Task.Run(() => GetMatchupsForChampion(champion));
        
        public IEnumerable<Matchup> GetMatchupsForChampion(string champion)
        {
            var cache = _Cache.Where(o => o.Champion == champion);

            if (!cache.Any())
            {
                cache = GetMatchupsInner(champion);
                _Cache.AddRange(cache);
            }

            return cache;
        }
    }
}
