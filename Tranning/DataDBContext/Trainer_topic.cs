using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Tranning.DataDBContext
{
    public class Trainer_topic
    {
        [Key]
        //public int id { get; set; }
        [ForeignKey("trainer_id")]
        public int trainer_id { get; set; }

        [ForeignKey("topic_id")]
        public int topic_id { get; set; }

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