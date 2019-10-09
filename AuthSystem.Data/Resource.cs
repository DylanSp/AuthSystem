using System;
using System.Collections.Generic;
using System.Text;

namespace AuthSystem.Data
{
    public struct Resource
    {
        public Guid Id { get; }
        public string Value { get; }
        
        public Resource(Guid id, string value)
        {
            Id = id;
            Value = value;
        }
    }
}
