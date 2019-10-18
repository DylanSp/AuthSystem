using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthSystem.DTOs
{
    public class ResourceDTO
    {
        public Guid ResourceId { get; set; }
        public ResourceDetailsDTO ResourceDetails { get; set; }
    }
}
