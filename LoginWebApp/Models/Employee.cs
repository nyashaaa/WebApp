using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoginWebApp.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required]
        public string Position { get; set; }

        public string Department { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string Phone { get; set; }

        [Display(Name = "Date Hired")]
        [DataType(DataType.Date)]
        public DateTime DateHired { get; set; }

     
    }
}
