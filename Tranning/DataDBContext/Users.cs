using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Tranning.DataDBContext
{
    public class Users
    {
        [Key]
        public int id { get; set; }

        [ForeignKey("role_id")]
        public int role_id { get; set; }

        [Column("extra_code", TypeName = "Varchar(50)"), Required]
        public string extra_code { get; set; }

        [Column("username", TypeName = "Varchar(50)"), Required]
        public string username { get; set; }

        [Column("password", TypeName = "Varchar(50)"), Required]
        public string password { get; set; }

        [Column("email", TypeName = "Varchar(50)"), Required]
        public string email { get; set; }

        [Column("phone", TypeName = "Varchar(20)"), Required]
        public string phone { get; set; }

        [Column("address", TypeName = "Varchar(50)"), AllowNull]
        public string? address { get; set; }

        [Column("gender", TypeName = "Integer"), Required]
        public int gender { get; set; }

        [Column("birthday"), AllowNull]
        public DateTime? birthday { get; set; }

        [Column("avatar", TypeName = "Varchar(120)"), AllowNull]
        public string? avatar { get; set; }

        [Column("last_login"), AllowNull]
        public DateTime? last_login { get; set; }

        [Column("last_logout"), AllowNull]
        public DateTime? last_logout { get; set; }

        [Column("status", TypeName = "Varchar(50)"), Required]
        public string status { get; set; }

        [AllowNull]
        public DateTime? created_at { get; set; }
        [AllowNull]
        public DateTime? updated_at { get; set; }
        [AllowNull]
        public DateTime? deleted_at { get; set; }

        [Column("full_name", TypeName = "Varchar(50)"), Required]
        public string full_name { get; set; }

        [Column("education", TypeName = "Varchar(50)"), AllowNull]
        public string? education { get; set; }

        [Column("programming_laguague", TypeName = "Varchar(50)"), AllowNull]
        public string? programming_laguague { get; set; }

        [Column("toeic_score", TypeName = "Integer"), AllowNull]
        public int? toeic_score { get; set; }

        [Column("experience"), AllowNull]
        public string? experience { get; set; }

        [Column("department", TypeName = "Varchar(50)"), AllowNull]
        public string? department { get; set; }
    }
}

