using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamlThemManager.Model
{
    public class ResourceUsage
    {
        public string FileName { get; set; }
        public string TargetType { get; set; }
        public string Key { get; set; }
        public int Usage { get; set; }

        public ResourceUsage(string fileName, string key, int usage,string targetType)
        {
            FileName = fileName;
            Key = key;
            Usage = usage;
            TargetType = targetType;
        }
    }
}
