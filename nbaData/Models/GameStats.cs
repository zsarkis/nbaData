using System.Collections.Generic;

namespace nbaData.Models
{
    public class GameStats
    {
        public int id;
        public int ast;
        public int blk;
        public int dreb;
        public double fg3_pct;
        public int fg3a;
        public int fg3m;
        public double fg_pct;
        public int fga;
        public int fgm;
        public double ft_pct;
        public int fta;
        public int ftm;
        public Game game;
        public string min;
        public int oreb;
        public int pf;
        public Player player;
        public int pts;
        public int reb;
        public int stl;
        Team team;
        public int turnover;
    }
    
    public class Game
    {
        public int id;
        public string date;
        public int home_team_id;
        public int home_team_score;
        public int period;
        public bool postseason;
        public int season;

        public string status;

        //TODO: verify time in a live game
        public string time;
        public int visitor_team_id;
        public int visitor_team_score;
    }

    public class BallDontLieGameStatsResponse
    {
        public List<GameStats> data { get; set; }
        public Meta meta { get; set; }
    }
}