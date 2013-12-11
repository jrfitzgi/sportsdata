using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;

namespace SportsData.Models
{
    public class NhlGameStatsBaseModelComparer : IEqualityComparer<NhlGameStatsBaseModel>
    {
        public bool Equals(NhlGameStatsBaseModel m1, NhlGameStatsBaseModel m2)
        {
            return
                m1.Date == m2.Date &&
                m1.Visitor == m2.Visitor &&
                m1.Home == m2.Home;
        }

        public int GetHashCode(NhlGameStatsBaseModel m)
        {
            return base.GetHashCode();
        }
    }
}
