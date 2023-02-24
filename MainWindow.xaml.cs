using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml;
using System.Xml.Serialization;
using ModernWpf;
using OoTxMM_Track.Model;

namespace OoTxMM_Track
{
    public partial class MainWindow : Window
    {
        private readonly string DirPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\OoTxMM-Track\\";
        public ObservableCollection<Tab>? Tabs { get; set; }
        public ObservableCollection<DisableChecks>? DisableChecks { get; set; }
        public MainWindow()
        {
            AddElement();
            DataContext = this;
            InitializeComponent();
        }
        public void AddElement()
        {
            Tabs = new();
            DisableChecks = new();
            if (!Directory.Exists(DirPath))
            {
                Directory.CreateDirectory(DirPath);
            }
            if (!File.Exists($"{DirPath}SaveData.xml"))
            {
#if(DEBUG)
                ImportData imp = new();
                imp.Import(Tabs, DisableChecks);
#endif
#if (!DEBUG)
                XmlSerializer xs = new(typeof(ObservableCollection<Tab>));

                Stream s = File.OpenRead("GameData.xml");
                if (s != null && xs != null)
                {
                    Tabs = (ObservableCollection<Tab>?)xs.Deserialize(s);
                }
#endif
            }
            else
            {
                XmlSerializer xs = new(typeof(ObservableCollection<Tab>));
                Stream s = new FileStream($"{DirPath}SaveData.xml", FileMode.Open);
                if (s != null && xs != null)
                {
                    Tabs = (ObservableCollection<Tab>?)xs.Deserialize(s);
                }
            }
        }
        private void Checkbox_Click(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            UIElement? parentElement = checkBox.Parent as UIElement;
            var tabNames = Tabs;
            for (int i = 1; i <= 6; i++)
            {
                parentElement = VisualTreeHelper.GetParent(parentElement) as UIElement;
            }
            if (parentElement != null && ((ContentPresenter)parentElement).Content != null)
            {
                var regionName = ((Region)((ContentPresenter)parentElement).Content).RegionName;

                if (regionName == "Excluded Checks" && checkBox.IsChecked == true)
                {
                    var tabName = tabNames?.FirstOrDefault(tab => tab.Regions!.Any(region => region.RegionName == (string)checkBox.Tag));
                    var regName = tabName?.Regions?.FirstOrDefault(region => region.RegionName == (string)checkBox.Tag);
                    if (regName != null && regName.Checks != null)
                    {
                        foreach (Check check in regName.Checks)
                        {
                            if (check.CheckName == (string)checkBox.Content)
                            {
                                check.IsVisible = "Collapsed";
                            }
                        }
                    }
                }
                else if (regionName == "Excluded Checks" && checkBox.IsChecked == false)
                {
                    var tabName = tabNames?.FirstOrDefault(tab => tab.Regions!.Any(region => region.RegionName == (string)checkBox.Tag));
                    var regName = tabName?.Regions?.FirstOrDefault(region => region.RegionName == (string)checkBox.Tag);
                    if (regName != null && regName.Checks != null)
                    {
                        foreach (Check check in regName.Checks)
                        {
                            if (check.CheckName == (string)checkBox.Content)
                            {
                                check.IsVisible = "Visible";
                            }
                        }
                    }
                }
            }

            if (checkBox.Content != null && (String)checkBox.Content == "Hide Skulls" && Tabs != null)
            {
                foreach (Tab tab in Tabs)
                {
                    foreach (Region reg in tab.Regions!)
                    {
                        foreach (Check check in reg.Checks!)
                        {
                            if (check.CheckType != null && (String)check.CheckType == "skull")
                            {
                                if (checkBox.IsChecked == true)
                                {
                                    check.IsVisible = "Collapsed";
                                }
                                else
                                {
                                    check.IsVisible = "Visible";
                                }
                            }
                        }
                    }
                }
            }
            if (checkBox.Content != null && (String)checkBox.Content == "Hide Fairies" && Tabs != null)
            {
                foreach (Tab tab in Tabs)
                {
                    foreach (Region reg in tab.Regions!)
                    {
                        foreach (Check check in reg.Checks!)
                        {
                            if (check.CheckType != null && (String)check.CheckType == "fairy")
                            {
                                if (checkBox.IsChecked == true)
                                {
                                    check.IsVisible = "Collapsed";
                                }
                                else
                                {
                                    check.IsVisible = "Visible";
                                }
                            }
                        }
                    }
                }
            }
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            XmlSerializer xs = new(typeof(ObservableCollection<Tab>));
            using StreamWriter sr = new($"{DirPath}SaveData.xml");
            using var xw = XmlWriter.Create(sr, new XmlWriterSettings { Indent = true, IndentChars = "\t" });
            xs.Serialize(xw, Tabs);
        }
        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            if (Tabs != null)
            {
                foreach (Tab tab in Tabs)
                {
                    if (tab.Regions != null)
                    {
                        foreach (Region reg in tab.Regions)
                        {
                            if (reg.Checks != null)
                            {
                                foreach (Check check in reg.Checks)
                                {
                                    check.IsChecked = false;
                                }
                            }
                        }
                    }
                }
            }
            if (File.Exists($"{DirPath}SaveData.xml"))
            {
                File.Delete($"{DirPath}SaveData.xml");
            }
        }
    }
}
