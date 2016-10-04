using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xaml;
using System.Xml;
using Microsoft.Win32;
using XamlThemManager.Model;
using XamlWriter = System.Windows.Markup.XamlWriter;

namespace XamlThemManager.ViewModel
{
    class MainViewModel:BaseViewModel
    {
        private int _totalResourcesCount;
        private int _totalUnusedResourcesCount;
        private string _path;
        private bool _isBusy;

        public ObservableCollection<KeyValuePair<string, List<DictionaryEntry>>> ThemeDectionary { get; set; }
        public ObservableCollection<ResourceUsage> ResourceUsageList { get; set; }
        public int ThemeCounter { get; set; }
        public MainViewModel()
        {
            ThemeDectionary = new ObservableCollection<KeyValuePair<string, List<DictionaryEntry>>>();
            ResourceUsageList=new ObservableCollection<ResourceUsage>();
            ThemeCounter = 1;
            Path = @"C:\Users\CodeValue\Documents\Visual Studio 2013\Projects\ThemeTesterProj\ThemeTesterProj";
        }
        public ICommand OpenFileCommand
        {
            get { return new DelegateCommand(OpenFile); }
        }
        public ICommand ExportToXamlThemeCommand
        {
            get { return new DelegateCommand(ExportToXamlTheme); }
        }
        public ICommand GetResourceUsageCommand
        {
            get { return new DelegateCommand(GetResourceUsage); }
        }

        public ICommand RemoveUnuseResourcesCommand
        {
            get { return new DelegateCommand(RemoveUnuseResources); }
        }

        private async void RemoveUnuseResources()
        {
            var usage = new ResourceUsageTool();
            var resourceList = (from resourceUsage in ResourceUsageList where resourceUsage.Usage == 0 select new KeyValuePair<string, string>(resourceUsage.Key, resourceUsage.FileName)).ToList();
            IsBusy = true;
            await Task.Run(() => usage.RemoveUnusedResources(resourceList));
            GetResourceUsage();
            IsBusy = false;
        }

        public int TotalResourcesCount
        {
            get
            {
                return _totalResourcesCount;
            }
            set
            {
                _totalResourcesCount = value;
                RaisePropertyChangedEvent("TotalResourcesCount");
            }
        }

        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }
            set
            {
                _isBusy = value;
                RaisePropertyChangedEvent("IsBusy");
            }
        }
        public int TotalUnusedResourcesCount
        {
            get
            {
                return _totalUnusedResourcesCount;
            }
            set
            {
                _totalUnusedResourcesCount = value;
                RaisePropertyChangedEvent("TotalUnusedResourcesCount");
            }
        }

        public string Path
        {
            get
            {
                return _path;
            }
            set
            {
                _path = value;
                RaisePropertyChangedEvent("Path");
            }
        }
        private async void GetResourceUsage()
        {
            var usage = new ResourceUsageTool();
            IsBusy = true;
            
            var res = await Task.Run(() => usage.GetResourceUsage(Path));
            ResourceUsageList.Clear();
            foreach (var resourceUsage in res)
            {
                ResourceUsageList.Add(resourceUsage);
            }
            TotalResourcesCount = ResourceUsageList.Count;
            TotalUnusedResourcesCount = ResourceUsageList.Count(x => x.Usage == 0);
            IsBusy = false;
        }

        private void OpenFile()
        {
            ThemeDectionary.Clear();
            var openFileDialog1 = new OpenFileDialog
            {
                InitialDirectory =
                    @"C:\Users\CodeValue\Documents\Visual Studio 2013\Projects\ThemeTesterProj\ThemeTesterProj",
                Filter = "Xaml files (*.xaml)|*.xaml",
                FilterIndex = 2,
                RestoreDirectory = true
            };

            if (openFileDialog1.ShowDialog() == true)
            {
                try
                {
                  var temp = ParseXaml(openFileDialog1.FileName);
                    foreach (var keyValuePair in temp)
                    {
                        ThemeDectionary.Add(keyValuePair);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private IEnumerable<KeyValuePair<string, List<DictionaryEntry>>> ParseXaml(string fileName)
        {
            var doc = XamlServices.Load(fileName);
            var collection = new ObservableCollection<KeyValuePair<string, List<DictionaryEntry>>>();
            var resourcedictionary = doc as ResourceDictionary;
            if (resourcedictionary != null)
            {
                foreach (var item in resourcedictionary)
                {
                    if (item is DictionaryEntry)
                    {
                        var resource = (DictionaryEntry)item;
                        if (!resource.Value.ToString().Contains("Gradient"))
                        {
                            collection.Add(new KeyValuePair<string, List<DictionaryEntry>>(resource.Key.ToString(), new List<DictionaryEntry>() { resource, new DictionaryEntry() }));
                        }
                    }
                }
            }
            return collection;
        }

        private void ExportToXamlTheme()
        {
            var colorDictionary= new ResourceDictionary();
            for (int i = 0; i < ThemeCounter; i++)
            {
                foreach (var theme in ThemeDectionary)
                {
                    var key = theme.Key + "Color";
                    var value = theme.Value[i].Value;
                    colorDictionary.Add(key, value);
                }
                
            var myStrXaml = XamlWriter.Save(colorDictionary);
            FileStream fs = File.Create("1.xaml");

            var sw = new StreamWriter(fs);

            sw.Write(myStrXaml);

            sw.Close();

            fs.Close();
            }
            colorDictionary.Clear();
            foreach (var theme in ThemeDectionary)
            {
                var key = theme.Key;
                var value = "{StaticResource " + theme.Key + "Color}";
                colorDictionary.Add(key, value);
            }
            var strXaml = XamlWriter.Save(colorDictionary);
            FileStream fileStream = File.Create("ThemBrushes.xaml");

            var stream = new StreamWriter(fileStream);

            stream.Write(strXaml);

            stream.Close();

            fileStream.Close();
            
        }
    }
}
