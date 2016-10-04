using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xaml;
using System.Xml.Linq;
using XamlThemManager.Model;
using XamlReader = System.Windows.Markup.XamlReader;

namespace XamlThemManager.ViewModel
{
    class ResourceUsageTool
    {
        private List<string> _failedFileList;

        public ResourceUsageTool()
        {
            _failedFileList = new List<string>();
        }

        public ObservableCollection<ResourceUsage> GetResourceUsage(string folder)
        {
            var files = GetXamlFiles(folder);
           var resources= GetKeys(files);
           var resourceUsage = CheckUsage(resources, files);
            return new ObservableCollection<ResourceUsage>(resourceUsage);
        }

        public void RemoveUnusedResources(List<KeyValuePair<string, string>> resources)
        {
            foreach (var resource in resources)
            {
               var doc = XDocument.Load(resource.Value);
               XNamespace x = "http://schemas.microsoft.com/winfx/2006/xaml";
                KeyValuePair<string, string> res = resource;
                doc.Descendants().Where(d =>
               {
                   var attribute = d.Attribute(x + "Key");
                   return attribute != null && attribute.Value == res.Key;
               }).Remove();
               doc.Save(resource.Value);
            }
        }

        private List<ResourceUsage> GetKeys(IEnumerable<string> files)
        {
            var resources = new List<ResourceUsage>();
            foreach (var file in files)
            {
                //var str = File.ReadAllText(file);
                try
                {
                    var doc = XDocument.Load(file);
                    var elements = doc.Descendants();
                    XNamespace x = "http://schemas.microsoft.com/winfx/2006/xaml";
                    foreach (var xElement in elements)
                    {
                        var xKey = xElement.Attribute(x + "Key");
                        if (xKey != null)
                        {
                            var key = xKey.Value;
                            var targetType = xElement.Name.ToString();
                            var path = file;
                            resources.Add(new ResourceUsage(path, key, 0, targetType));
                        }
                    }
                }
                catch (Exception)
                {
                    _failedFileList.Add(file);
                }
            }
            File.WriteAllLines(@"ErrorLogger.txt", _failedFileList.ToArray());
            return resources;
        }

        public List<ResourceUsage> CheckUsage(List<ResourceUsage> resourcesList, IEnumerable<string> files)
        {
            foreach (var file in files)
            {
                var txt = File.ReadAllText(file);
                foreach (var resource in resourcesList)
                {
                    var resourceCheked = resource;
                    var staticKey = "{StaticResource " + resourceCheked.Key+"}";
                    var dynamicKey = "{DynamicResource " + resourceCheked.Key+"}";
                    var count = new Regex(staticKey).Matches(txt).Count;
                    resource.Usage = resource.Usage + count;
                    count = new Regex(dynamicKey).Matches(txt).Count;
                    resource.Usage = resource.Usage + count;
                }
            }
            return resourcesList;
        }

        private List<string> GetXamlFiles(string path)
        {
            if (!Directory.Exists(path))
            {
                MessageBox.Show("directory not found");
                return new List<string>();
            }
            var filePaths = Directory.GetFiles(path, "*.xaml", SearchOption.AllDirectories).ToList();
            return filePaths;
        }
    }
}
