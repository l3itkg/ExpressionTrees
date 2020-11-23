namespace Domain.Dto
{
    public class StaffDto
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string OrderBy { get; set; }

        public int? StaffCategoryId { get; set; }

        public bool? IsOfficial { get; set; }
    }
}