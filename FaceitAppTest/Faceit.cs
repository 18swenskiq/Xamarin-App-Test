using FaceitAPI.Models;
using FaceitAPI.Types;

namespace FaceitAPI.SeachPlayers
{
    public class FaceitPlayerSearchResponse
    {
        public string PlayerId { get; set; }
        public string Avatar { get; set; }
        public string Country { get; set; }
        public string Nickname { get; set; }
        public bool Verified { get; set; }
    }

    public class SearchFaceitPlayers
    {
        public FaceitPlayerSearchResponse Search(string apikey, string name)
        {
            Authorization authorization = new Authorization(apikey);
            Faceit faceit = new Faceit(authorization);

            Search search = faceit.GetObject<Search>();

            Paging<SimplePlayer> players = search.GetPlayers(name, limit: 1);

            FaceitPlayerSearchResponse fpsr = new FaceitPlayerSearchResponse();
            foreach (var player in players.Items)
            {
                fpsr.PlayerId = player.PlayerId;
                fpsr.Avatar = player.Avatar;
                fpsr.Country = player.Country;
                fpsr.Nickname = player.Nickname;
                fpsr.Verified = (bool)player.Verified;
            }
            return fpsr;
        }
    }
}

