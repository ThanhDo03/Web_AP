using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Tranning.DataDBContext
{
    public class Trainee_course
    {
        [Key]
        //public int id { get; set; }
        [ForeignKey("trainee_id")]
        public int trainee_id { get; set; }

        [ForeignKey("course_id")]
        public int course_id { get; set; }

        [Column("status", TypeName = "Varchar(50)"), Required]
        public string status { get; set; }

        [AllowNull]
        public DateTime? created_at { get; set; }
        [AllowNull]
        public DateTime? updated_at { get; set; }
        [AllowNull]
        public DateTime? deleted_at { get; set; }


    }
}
