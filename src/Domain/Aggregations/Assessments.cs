using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Aggregations
{
    public class Assessments
    {
        public Assessments(Guid userID, Guid technicianID, DateTime date, double score)
        {
            UserID = userID;
            TechnicianID = technicianID;
            Date = date;
            Score = score;
        }

        public Guid UserID { get; set; }
        public Guid TechnicianID { get; set; }
        public DateTime Date { get; set; }
        public double Score { get; set; }
    }
}
