
namespace SportsData.Nhl.Query
{
    public class HockeySeason
    {
        public string OneYearName { get; set; }
        public string TwoYearName { get; set; }

        public HockeySeason(string oneYearName, string twoYearName)
        {
            this.OneYearName = oneYearName;
            this.TwoYearName = twoYearName;
        }
    }
}
