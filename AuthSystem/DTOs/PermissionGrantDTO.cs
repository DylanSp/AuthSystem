using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthSystem.DTOs
{
    public class PermissionGrantDTO
    {
        public Guid PermissionGrantId { get; set; }
        public PermissionGrantDetailsDTO PermissionGrantDetails { get; set; }
    }
}
