using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Entities
{
    public class PerformanceRating
    {
        private static readonly double MIN_SCORE = 0.0;
        private static readonly double MAX_SCORE = 5.0;
        public int UserId { get; private set; }
        public int TechnicianId { get; private set; }
        public DateTime Date { get; private set; }
        public double Score { get; private set; }
        public int PerformanceRatingId { get; private set; }
        private PerformanceRating() { }
        public PerformanceRating(DateTime date, double score, int superiorId, int technicianId)
        {
            ValidateScore(score);
            ValidateDate(date);
            UserId = superiorId;
            TechnicianId = technicianId;
            Date = date;
            Score = score;
        }

        private void ValidateScore(double score)
        {
            if (!(score >= MIN_SCORE && score <= MAX_SCORE))
                throw new ArgumentException($"Score must be between {MIN_SCORE} and {MAX_SCORE}");
        }

        private void ValidateDate(DateTime date)
        {
            if (date > DateTime.Now)
                throw new ArgumentException("Rating date cannot be in the future");

        }

        public bool IsExcellent() => Score >= 4.5;
        public bool IsPoor() => Score < 2.0;
    }
}
