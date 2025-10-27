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
        public DateTime Date
        {
            get { return date; }
            private set { }
        }
        private DateTime date;
        public double Score
        {
            get { return score; }
            private set { }
        }
        private double score;
        public virtual Entities.User? User { get; private set; }
        public virtual Entities.Technician? Technician { get; private set; }

        private PerformanceRating() { }
        public PerformanceRating(User superior, User technician, DateTime date, double score)
        {
            User = superior;
            Technician = technician as Technician;
            UserID = superior.UserID;
            TechnicianID = technician.UserID;
            this.date = date;
            this.score = score;
        }
    }
}
