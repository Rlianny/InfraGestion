using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Aggregations
{
    public class Assessments
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
        public virtual Entities.User? User { get; set; }
        public virtual Entities.Technician? Technician { get; set; }
        public Assessments(DateTime date, double score)
        {
            this.date = date;
            this.score = score;
        }
    }
}
