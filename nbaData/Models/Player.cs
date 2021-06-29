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
                this.id = (int)id;
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
        public double threePointVolume { get; set; }
        public double threePointPercentage { get; set; }
        public double twoPointVolume { get; set; }
        public double twoPointPercentage { get; set; }
    }

    public class BallDontLieResponse
    {
        public List<Player> data { get; set; }
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