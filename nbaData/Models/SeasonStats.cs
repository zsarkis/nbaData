using System.Collections.Generic;

namespace nbaData.Models
{
    public class SeasonStats
    {
        public int games_played { get; set; }
        public int player_id { get; set; }
        public int season { get; set; }
        public string min { get; set; }
        public double fgm { get; set; }
        public double fga { get; set; }
        public double fg3m { get; set; }
        public double fg3a { get; set; }
        public double ftm { get; set; }
        public double fta { get; set; }
        public double oreb { get; set; }
        public double dreb { get; set; }
        public double reb { get; set; }
        public double ast { get; set; }
        public double stl { get; set; }
        public double blk { get; set; }
        public double turnover { get; set; }
        public double pf { get; set; }
        public double pts { get; set; }
        public double fg_pct { get; set; }
        public double fg3_pct { get; set; }
        public double ft_pct { get; set; }
    }

    public class BallDontLieSeasonStatsResponse
    {
        public List<SeasonStats> data { get; set; }
        public Meta meta { get; set; }
    }
}