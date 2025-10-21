using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopTARge24.Core.Dto
{
    public class FileApiDto
    {
        public string? ExistingFilePath;

        public Guid Id { get; set; }
        public string? ExistingFilepath { get; set; }
        public Guid? SpaceshipId { get; set; }
    }
}