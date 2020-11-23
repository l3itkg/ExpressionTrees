using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace Domain.Entities
{
    public class Staff
    {
        public static readonly Expression<Func<Staff, bool>> ToShowExpression = x => x.IsActive && x.IsOfficial;

        public static readonly Expression<Func<Staff, bool>> IsActiveExpression = x => x.IsActive;

        public int Id { get; set; }

        [Required]
        [MaxLength(128)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(128)]
        public string LastName { get; set; }

        public bool IsActive { get; set; }

        public bool IsOfficial { get; set; }

        public int StaffCategoryId { get; set; }

        public bool ToShow => ToShowExpression.AsFunc()(this);

        public virtual StaffCategory StaffCategory { get; set; }
    }
}
