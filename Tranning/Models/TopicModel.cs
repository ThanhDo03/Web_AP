using System.ComponentModel.DataAnnotations;
using Tranning.Validations;

namespace Tranning.Models
{
    public class TopicModel
    {
        public List<TopicDetail> TopicDetailLists { get; set; }
    }

    public class TopicDetail
    {
        public int id { get; set; }

        [Required(ErrorMessage = "Enter Course, please.")]
        public int course_id { get; set; }

        [Required(ErrorMessage = "Enter name, please.")]
        public string name { get; set; }

        public string? description { get; set; }
        public string? videos { get; set; }
        public string? documents { get; set; }
        public string? attach_file { get; set; }
        [Required(ErrorMessage = "Enter video, please.")]
        [AllowedExtensionFile(new string[] { ".mp3", ".mp4" })]
        public IFormFile fileVideos { get; set; }


        [Required(ErrorMessage = "Enter document, please.")]
        [AllowedExtensionFile(new string[] { ".docx", ".pdf" })]
        public IFormFile fileDocuments { get; set; }


        [Required(ErrorMessage = "Choose file, please")]
        [AllowedExtensionFile(new string[] { ".png", ".jpg", ".jpeg" })]
        [AllowedSizeFile(300 * 1024 * 1024)]
        public IFormFile? fileAttach_file { get; set; }

        [Required(ErrorMessage = "Choose Status, please.")]
        public string status { get; set; }
        public DateTime? created_at { get; set; }

        public DateTime? updated_at { get; set; }

        public DateTime? deleted_at { get; set; }

        public string? courseName { get; set; }

    }
}
