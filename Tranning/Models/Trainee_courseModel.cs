using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Tranning.Validations;
using Tranning.DataDBContext;

namespace Tranning.Models
{
    public class Trainee_courseModel
    {
        public List<Trainee_courseModelDetail> Trainee_courseDetailLists { get; set; }
    }

    public class Trainee_courseModelDetail
    {
        //public int id { get; set; }

        [Required(ErrorMessage = "Choose Trainee, please")]
        public int trainee_id { get; set; }

        [Required(ErrorMessage = "Choose Course, please")]
        public int course_id { get; set; }

        [Required(ErrorMessage = "Choose Status, please")]
        public string status { get; set; }

        public DateTime? created_at { get; set; }

        public DateTime? updated_at { get; set; }

        public DateTime? deleted_at { get; set; }

        public string? traineeName { get; set; }

        public string? courseName { get; set; }
    }
}