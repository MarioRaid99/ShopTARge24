using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ShopTARge24.Core.Dto
{
    public class KindergartenDto
    {
        public Guid? Id { get; set; }
        public string? KindergartenName { get; set; }
        public string? GroupName { get; set; }
        public string? TeacherName { get; set; }
        public int? ChildCount { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<IFormFile> Files { get; set; }
        public IEnumerable<KindergartenFileToDatabaseDto> Image { get; set; }
            = new List<KindergartenFileToDatabaseDto>();
    }
}