
namespace ShopTARge24.Core.Domain
{
    public class Kindergartens
    {

        public Guid Id { get; set; }
        public string GroupName { get; set; }
        public int ChildrenCount { get; set; }
        public string KindergartenName { get; set; }
        public string TeacherName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}