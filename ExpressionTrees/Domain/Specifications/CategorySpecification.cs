using Domain.Entities;

namespace Domain.Specifications
{
    public static class CategorySpecification
    {
        public static readonly Spec<StaffCategory> IsHigh = new Spec<StaffCategory>(x => x.IsHighest);
    }
}
