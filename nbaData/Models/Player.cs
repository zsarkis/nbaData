using System.Collections.Generic;

namespace nbaData.Models
{
    public class Player
    {
        public Player(string firstName, string lastName, Team teamName = null,
            double? heightFeet = null, double? heightInches = null, string position = null,
            double? weightPounds = null, int? id = null)
        {
            if (id != null)
            {
                this.id = (int) id;
            }

            first_name = firstName;
            last_name = lastName;
            team = teamName;
            height_feet = heightFeet;
            height_inches = heightInches;
            this.position = position;
            weight_pounds = weightPounds;
        }

        public int id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public double? height_feet { get; set; }
        public double? height_inches { get; set; }
        public string position { get; set; }
        public double? weight_pounds { get; set; }

        public Team team { get; set; }

        public ShootingStats shootingStatsShortTerm { get; set; }
        public ShootingStats shootingStatsMidTerm { get; set; }
        public ShootingStats shootingStatsLongTerm { get; set; }
    }

    public class Team
    {
        public int id { get; set; }
        public string abbreviation { get; set; }
        public string city { get; set; }
        public string conference { get; set; }
        public string division { get; set; }
        public string full_name { get; set; }
        public string name { get; set; }
    }

    public class ShootingStats
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

    public class BallDontLiePlayerResponse
    {
        public List<Player> data { get; set; }
        public Meta meta { get; set; }
    }

    public class BallDontLieSeasonStatsResponse
    {
        public List<ShootingStats> data { get; set; }
        public Meta meta { get; set; }
    }

    public class Meta
    {
        public int? total_pages { get; set; }
        public int? current_page { get; set; }
        public int? next_page { get; set; }
        public int? per_page { get; set; }
        public int? total_count { get; set; }
    }
}