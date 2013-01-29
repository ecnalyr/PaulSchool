using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [UIHint("IsChecked")]
        public bool Experienced { get; set; }

        [UIHint("IsChecked")]
        public bool ExhibitsUnderstanding { get; set; }

        public string RecommendersThoughts { get; set; }

        [UIHint("IsChecked")]
        public bool SignedByRecommender { get; set; }

        [UIHint("IsChecked")]
        public bool SignedByApplicant { get; set; }

        public DateTime DateFiled { get; set; }
    }
}
