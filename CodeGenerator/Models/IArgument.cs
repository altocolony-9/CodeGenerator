using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeGenerator.Models
{
    public interface IArgument
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public bool IsCancel { get; set; }
    }
}
