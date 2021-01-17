using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeGenerator.Models;

namespace CodeGenerator.Pages
{
    public interface ICommandGenerator
    {
        IDictionary<string,string> GeneratePropName<Type>(Type args) where Type : IEnumerable<IArgument>;
        IList<string> GeneratePropContext(IDictionary<string,string> bigArgs);

    }
}
