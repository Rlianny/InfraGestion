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
        public virtual Entities.User? User { get; private set; }
        public virtual Entities.Technician? Technician { get; private set; }

        private PerformanceRating() { }
        public PerformanceRating(User superior, Technician technician, DateTime date, double score)
        {
            User = superior;
            Technician = technician;
            UserID = superior.UserID;
            TechnicianID = technician.UserID;
            Date = date;
            Score = score;
        }
    }
}
