using System.Collections.Generic;

namespace CombatZone.Scoreboard
{

    #region Scoreboard Sorter
    public class ScoreboardSorter : IComparer<ScoreboardEntry.PlayerScoreData>
    {

        public int Compare(ScoreboardEntry.PlayerScoreData scoreEntry1, ScoreboardEntry.PlayerScoreData scoreEntry2)
        {
            if (scoreEntry1.totalScore < scoreEntry2.totalScore)
            {
                return 1;
            }
            else if (scoreEntry1.totalScore > scoreEntry2.totalScore)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }

    }
    #endregion

}