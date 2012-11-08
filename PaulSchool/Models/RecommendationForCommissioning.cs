using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PaulSchool.Models
{
    public class RecommendationForCommissioning
    {
        public int Id { get; set; }

        public Student Student { get; set; }

        public string RecommendersFirstName { get; set; }

        public string RecommendersMidName { get; set; }

        public string RecommendersLastName { get; set; }

        public bool ActiveInRecommendersParish { get; set; }

        public bool Experienced { get; set; }

        public bool ExhibitsUnderstanding { get; set; }

        public string RecommendersThoughts { get; set; }

        public bool SignedByRecommender { get; set; }

        public bool SignedByApplicant { get; set; }

        public DateTime DateFiled { get; set; }
    }
}
