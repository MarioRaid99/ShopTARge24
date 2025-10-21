using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopTARge24.Core.Domain
{
    public class FileToKindergarten
    {
        public Guid Id { get; set; }
        public string? ExistingFilePath { get; set; }
        public Guid? KindergartenId { get; set; }
    }
}