using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace XpertGroceryManager.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Customer name cannot be longer than 100 characters.")]
        public string Name { get; set; }

        public string MemberNumber { get; set; }

        public string MemberType { get; set; }

        [Phone]
        public string Phone { get; set; }

        [StringLength(180, ErrorMessage = "Address cannot be longer than 180 characters.")]
        public string Address { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public virtual ICollection<Sales> Sales { get; set; } 
    }
}
