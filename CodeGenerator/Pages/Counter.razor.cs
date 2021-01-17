using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using CodeGenerator.Models;

namespace CodeGenerator.Pages
{
    public partial class Counter : ICommandGenerator
    {
        private string _nameSpace;
        private string _counstructorName;
        private string _fileName;
        private string _className;
        private IList<Arguments> _argments = new List<Arguments>();

        private string _folderPath;

        /// <summary>
        /// プロパティ名生成
        /// </summary>
        /// <typeparam name="Type"></typeparam>
        /// <param name="args">型、引数名、生成フラグ</param>
        /// <returns>型、プロパティ名</returns>
        public IDictionary<string,string> GeneratePropName<Type>(Type args) where Type : IEnumerable<IArgument>
        {
            Dictionary<string, string> bigPropName = new Dictionary<string, string>();
            if (args == null)
            {
                return null;
            }
            foreach(var arg in args)
            {
                if(!arg.IsCancel)
                {
                    bigPropName.Add(arg.Type ,char.ToUpper(arg.Name[0]) + arg.Name.Substring(1));
                }
            }
            return bigPropName;
        }
            
        /// <summary>
        /// プロパティコード生成
        /// </summary>
        /// <param name="bigArgs">プロパティの（型、名前）群</param>
        /// <returns>プロパティコード一覧</returns>
        public IList<string> GeneratePropContext(IDictionary<string,string> bigArgs)
        {
            List<string> propContext = new List<string>();
            string accessModify = "@\t\tpublic";
            string type;
            string name;
            string getSet = "@{ get; set; }\n";

            foreach(var bigArg in bigArgs)
            {
                type = bigArg.Key;
                name = bigArg.Value;
                string[] all = {accessModify, type, name, getSet};

                propContext.Add(string.Join(" ", all));
            }

            return propContext;
        }
        //public int MyProperty { get; set; }

        /// <summary>
        /// コンストラクタの引数欄追加
        /// </summary>
        private void Add()
        {
            _argments.Add(new Arguments());
        }

       
    }
}
