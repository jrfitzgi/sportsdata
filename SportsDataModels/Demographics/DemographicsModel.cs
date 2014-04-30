using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportsData.Models
{
    public class DemographicsModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime LastUpdated { get; set; }

        public int Zip { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int MedianIncome { get; set; }
        public int MedianIncomeRank { get; set; }
        public double CostOfLivingIndex { get; set; }
        public int CostOfLivingRank { get; set; }
        public double MedianMortgageToIncomeRatio { get; set; }
        public int MedianMortgageToIncomeRank { get; set; }
        public double OwnerOccupiedHomesPercent { get; set; }
        public int OwnerOccupiedHomesRank { get; set; }
        public double MedianRoomsInHome { get; set; }
        public int MedianRoomsInHomeRank { get; set; }
        public double CollegeDegreePercent { get; set; }
        public int CollegeDegreeRank { get; set; }
        public double ProfessionalPercent { get; set; }
        public int ProfessionalRank { get; set; }
        public int Population { get; set; }
        public int PopulationRank { get; set; }
        public double AverageHouseholdSize { get; set; }
        public int AverageHouseholdSizeRank { get; set; }
        public double MedianAge { get; set; }
        public int MedianAgeRank { get; set; }
        public double MaleToFemaleRatio { get; set; }
        public int MaleToFemaleRank { get; set; }
        public double MarriedPercent { get; set; }
        public int MarriedRank { get; set; }
        public double DivorcedPercent { get; set; }
        public int DivorcedRank { get; set; }
        public double WhitePercent { get; set; }
        public int WhiteRank { get; set; }
        public double BlackPercent { get; set; }
        public int BlackRank { get; set; }
        public double AsianPercent { get; set; }
        public int AsianRank { get; set; }
        public double HispanicEthnicityPercent { get; set; }
        public int HispanicEthnicityRank { get; set; }

    }
}

