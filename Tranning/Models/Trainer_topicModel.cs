using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Tranning.Validations;
using Tranning.DataDBContext;

namespace Tranning.Models
{
    public class Trainer_topicModel
    {
        public List<Trainer_topicModelDetail> Trainer_topicDetailLists { get; set; }
    }

    public class Trainer_topicModelDetail
    {
        //public int id { get; set; }

        [Required(ErrorMessage = "Choose Trainer, please")]
        public int trainer_id { get; set; }

        [Required(ErrorMessage = "Choose Topic, please")]
        public int topic_id { get; set; }

        [Required(ErrorMessage = "Choose Status, please")]
        public string status { get; set; }

        public DateTime? created_at { get; set; }

        public DateTime? updated_at { get; set; }

        public DateTime? deleted_at { get; set; }

        public string? trainerName { get; set; }

        public string? topicName { get; set; }  
    }
}
