using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using CodeGenerator.Models;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

namespace CodeGenerator.Pages
{
    public partial class CommandGenerator : ICommandGenerator
    {
        private string _nameSpace;
        private string _counstructorName;
        private string _fileName;
        private string _className;
        private IList<Arguments> _argments = new List<Arguments>();
        private string _text;
        /// <summary>
        /// プロパティ名生成
        /// </summary>
        /// <typeparam name="Type"></typeparam>
        /// <param name="args">型、引数名、生成フラグ</param>
        /// <returns>型、プロパティ名</returns>
        public IDictionary<string, string> GeneratePropName<Type>(Type args) where Type : IEnumerable<IArgument>
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

        /*
        public string GenerateConstructor()
        {
            var oneLine = "@\t\tpublic %Constructor%(";
            string arg = string.Empty;
            foreach(var argument in _argments)
            {
                if (_argments.IndexOf(argument) == _argments.Count - 1)
                {
                    arg += $"@%{argument.Type}% %{argument.Name}%)\n";
                }
                arg += $"%{argument.Type}% %{argument.Name}%,";
            }
            arg += "@\t";
        }
      */
        /// <summary>
        /// コンストラクタの引数欄追加
        /// </summary>
        private void Add()
        {
            _argments.Add(new Arguments());
        }

        /// <summary>
        /// アップロードファイル読み込み
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private async Task ReadUploadFile(InputFileChangeEventArgs e)
        {
            var stream = e.File.OpenReadStream();
            using (var reader = new StreamReader(stream))
            {
                 _text = await reader.ReadToEndAsync();
            }

        }

        private async Task OnDownload()
        {
            var text = BuildText();
            await JSRuntime.InvokeVoidAsync("downloadFile",text,_fileName);
        }

        /// <summary>
        /// テンプレートに文字を埋め込む
        /// </summary>
        /// <returns></returns>
        private string BuildText()
        {
            var argmentList = _argments.Select(s => s.Type + " " + s.Name).ToList();
            var arguments = Create(argmentList, "argument");

            var contructorList = _argments.Select(s => char.ToUpper(s.Name[0]) + s.Name.Substring(1) + "=" + s.Name).ToList();
            var constructors = Create(contructorList, "constructor");

            var propertyList = _argments.Select(s =>"public" + " " 
                                                 + s.Type + " "
                                                 + char.ToUpper(s.Name[0]) 
                                                 + s.Name.Substring(1)
                                                 + " "+ "{ get; set; }" )
                                                .ToList();

            var properties = Create(propertyList, "property");

            var buildedText = _text.Replace("%NameSpace%", _nameSpace)
                                   .Replace("%ClassName%", _className)
                                   .Replace("%Constructor%", _counstructorName)
                                   .Replace("%Type Argument%", arguments)
                                   .Replace("%InsertProperty%", constructors)
                                   .Replace("%SetProperty%", properties);
            
            return buildedText;
        }

        private string Create(IList<string> collection, string option)
        {
            string text = string.Empty;
            for(int i = 0; i < collection.Count; i++)
            {
                if(i == collection.Count - 1)
                {
                    option += "End";
                }
                switch(option)
                {
                    case "argument":
                        text += collection[i] + ",";
                        break;
                    case "argumentEnd":
                        text += collection[i];
                        break;
                    case "constructor":
                        text += collection[i] + ";\n\t\t\t";
                        break;
                    case "constructorEnd":
                        text += collection[i] + ";";
                        break;
                    case "property":
                        text += collection[i] + "\n\t\t";
                        break;
                    case "propertyEnd":
                        text += collection[i];
                        break;
                    default:
                        break;
                }

            }
            return text;
        }




    }
}
