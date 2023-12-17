using System.ComponentModel.DataAnnotations;
using Tranning.Validations;

namespace Tranning.Models
{
    public class UserModel
    {
        public List<UserDetail> UserDetailLists { get; set; }
    }

    public class UserDetail
    {
        public int id { get; set; }

        [Required(ErrorMessage = "Enter role, please")]
        public int role_id { get; set; }

        [Required(ErrorMessage = "Enter extra code, please")]
        public string extra_code { get; set; }

        [Required(ErrorMessage = "Enter username, please")]
        public string username { get; set; }

        [Required(ErrorMessage = "Enter password, please")]
        public string password { get; set; }

        [Required(ErrorMessage = "Enter email, please")]
        public string email { get; set; }

        [Required(ErrorMessage = "Enter phone, please")]
        public string phone { get; set; }

        public string? address { get; set; }

        [Required(ErrorMessage = "Enter gender, please")]
        public int gender { get; set; }

        public DateTime? birthday { get; set; }

        public string? avatar { get; set; }

        public DateTime? last_login { get; set; }

        public DateTime? last_logout { get; set; }

        [Required(ErrorMessage = "Choose Status, please")]
        public string status { get; set; }

        public DateTime? created_at { get; set; }

        public DateTime? updated_at { get; set; }

        public DateTime? deleted_at { get; set; }

        [Required(ErrorMessage = "Enter full_name, please")]
        public string full_name { get; set; }

        public string? education { get; set; }

        public string? programming_laguague { get; set; }

        public int? toeic_score { get; set; }

        public string? experience { get; set; }

        public string? department { get; set; }
    }
}
