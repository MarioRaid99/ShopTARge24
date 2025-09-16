using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopTARge24.Core.Domain
{
    internal class Kinderkarten
    {
      public Guid id { get; set; }
      public string name { get; set; }
      public string GroupName { get; set; }
      public int ChildrenCount { get; set; }
      public string KindergartenName { get; set; }
      public string TecherName { get; set; }
      public DateTime CreatedAt { get; set; }
      public DateTime UpdatedAt { get; set; }
    }   
}