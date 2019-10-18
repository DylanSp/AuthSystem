using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthSystem.DTOs
{
    public class PermissionGrantDetailsDTO
    {
        public Guid ResourceId { get; set; }
        public string Username { get; set; }
        public string PermissionType { get; set; }
    }
}
