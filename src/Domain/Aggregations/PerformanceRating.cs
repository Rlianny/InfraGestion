using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Aggregations
{
    public class PerformanceRating
    {
        public int UserID { get; private set; }
        public int TechnicianID { get; private set; }
        public DateTime Date { get; private set; }
        public double Score { get; private set; }
        public int PerformanceRatingID { get; private set; }
        private PerformanceRating() { }
        public PerformanceRating(DateTime date, double score, int superiorID, int technicianID)
        {
            UserID = superiorID;
            TechnicianID = technicianID;
            Date = date;
            Score = score;
        }
    }
}
