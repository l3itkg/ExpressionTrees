using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class StaffCategory
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        public bool IsHighest { get; set; }

        public virtual ICollection<Staff> Staff { get; set; }
    }
}
