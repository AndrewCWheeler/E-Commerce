using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace E_Commerce.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [MinLength(2,ErrorMessage="First Name must contain at least 2 characters.")]
        public string FirstName { get; set; }

        [Required]
        [MinLength(2,ErrorMessage="Last Name must contain at least 2 characters.")]
        public string LastName { get; set; }
        
        [Required]
        [EmailAddress(ErrorMessage="Invalid email address")]
        public string Email { get; set; }

        [Required]
        [MinLength(2,ErrorMessage="Company Name must be at least 2 characters.")]
        public string CompanyName { get; set; }

        [Required(ErrorMessage="Please provide an address for your company.")]
        public string Address { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [RegularExpression("^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z]).{8,32}$",ErrorMessage="Password must be at least 8 characters and contain at least one uppercase character and number.")]
        public string Password { get; set; }

        [NotMapped]
        [Compare("Password", ErrorMessage="Passwords do not match.")]
        [DataType(DataType.Password)]
        public string Confirm { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}