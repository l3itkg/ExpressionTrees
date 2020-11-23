using Domain.Entities;

namespace Domain.Specifications
{
    public static class StaffSpecification
    {
        public static readonly Spec<Staff> IsActiveSpec = new Spec<Staff>(x => x.IsActive);

        public static readonly Spec<Staff> IsOfficialSpec = new Spec<Staff>(x => x.IsOfficial);
    }
}
