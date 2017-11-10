using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Counters
{
    public class ProviderLolCounter : MatchupProvider
    {
        protected override IEnumerable<Matchup> GetMatchupsInner(string champion)
        {
            champion = ToCamelCase(champion);

            //Download HTML
            WebClient client = new WebClient();
            string html = client.DownloadString("http://lolcounter.com/champions/" + champion);

            //Load HTML
            HtmlDocument doc = new HtmlDocument()
            {
                OptionFixNestedTags = true,
                OptionCheckSyntax = false
            };
            doc.LoadHtml(html);

            //Parse elements
            if (doc.DocumentNode != null)
            {
                var weak = GetCounters(doc.DocumentNode, "weak");
                var strong = GetCounters(doc.DocumentNode, "strong");

                foreach (var item in weak)
                {
                    yield return new Matchup(champion, ToCamelCase(item), Matchup.MatchupType.WeakAgainst);
                }

                foreach (var item in strong)
                {
                    yield return new Matchup(champion, ToCamelCase(item), Matchup.MatchupType.StrongAgainst);
                }
            }
        }

        private List<string> GetCounters(HtmlNode doc, string weakStrong)
        {
            var nodes = GetNodes(doc, weakStrong);
            List<string> champions = new List<string>();

            foreach (var item in nodes)
            {
                var node = item.SelectSingleNode("div[@class='left theinfo']/a[@class='left']/div");
                string champ = node.InnerText;

                champions.Add(champ);
            }

            return champions;
        }

        private HtmlNodeCollection GetNodes(HtmlNode root, string weakStrong)
        {
            weakStrong = weakStrong.ToLower();
            return root.SelectNodes(
                    "//body" +
                    "/div[@id='master-block']" +
                    "/div[@id='maincontent']" +
                    "/div[@id='sub-master']" +
                    "/div[@id='championPage']" +
                    "/div[@class='block3 _all']" +
                    "/div[@class='weak-strong']" +
                   $"/div[@class='{weakStrong}']" +
                   $"/div[@class='{weakStrong}-block']" +
                    "/div[@class='champ-block']");
        }

        public override string GetProviderName()
        {
            return "SoloMid's LoL Counter";
        }

        public override string GetProviderHomeUrl()
        {
            return "www.lolcounter.com";
        }

        private static string ToCamelCase(string str)
        {
            return (str[0].ToString().ToUpper() + str.Substring(1)).Replace(" ", "");
        }
    }
}
