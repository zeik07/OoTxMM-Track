using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
using System.Xml.Serialization;
using OoTxMM_Track.Model;

namespace OoTxMM_Track
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<Tab>? Tabs { get; set; }
        public bool ShowSkulls { get; set; } = true;
        public bool ShowFairies { get; set; } = true;
        public int TotalChecks { get; set; } = 0;
        public MainWindow()
        {            
            AddElement();
            DataContext = this;
            InitializeComponent();
        }
        public void AddElement()
        {
            if (!File.Exists("SaveData.xml"))
            {
#if(DEBUG)
                Tabs = new ObservableCollection<Tab>();
                XmlDocument gameData = new();
                
                gameData.Load($@"{Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent?.FullName}\ImportData.xml");

                if (gameData.DocumentElement != null)
                {
                    foreach (XmlNode tab in gameData.DocumentElement.ChildNodes)
                    {
                        ObservableCollection<Region> regionsList = new();
                        foreach (XmlNode region in tab.ChildNodes)
                        {
                            ObservableCollection<Check> checksList = new();
                            foreach (XmlNode check in region.ChildNodes)
                            {                                
                                if (check.Attributes?["name"]?.InnerText == "off")
                                {
                                    continue;
                                }
                                checksList.Add(new Check { CheckName = check.InnerText, CheckType=check.Name });
                                if (tab.Attributes?["name"]?.InnerText != "Settings")
                                {
                                    TotalChecks += 1;
                                }
                            }
                            string? rt = region.Attributes?["type"]?.InnerText;
                            if (rt == null)
                            {
                                rt = "overworld";
                                if (tab.Attributes?["name"]?.InnerText == "Settings")
                                {
                                    rt = "settings";
                                }
                            }
                            regionsList.Add(new Region { RegionName = region.Attributes?["name"]?.InnerText, RegionType = rt, Checks = checksList });
                        }
                        Tabs.Add(new Tab { TabName = tab.Attributes?["name"]?.InnerText, Index = "0", Regions = regionsList });
                    }
                }
                foreach (Tab t in Tabs)
                {
                    t.Content = $"Total Checks: {TotalChecks}";
                }
                XmlSerializer xs = new XmlSerializer(typeof(ObservableCollection<Tab>));

                using StreamWriter sr = new StreamWriter("GameData.xml");
                using var xw = XmlWriter.Create(sr, new XmlWriterSettings { Indent = true, IndentChars = "\t" });
                xs.Serialize(xw, Tabs);
#endif
#if (!DEBUG)
                XmlSerializer xs = new XmlSerializer(typeof(ObservableCollection<Tab>));
                Stream s = new FileStream("GameData.xml", FileMode.Open);
                if (s != null && xs != null)
                {
                    Tabs = (ObservableCollection<Tab>?)xs.Deserialize(s);
                }
#endif
            }
            else
            {
                XmlSerializer xs = new XmlSerializer(typeof(ObservableCollection<Tab>));
                Stream s = new FileStream("SaveData.xml", FileMode.Open);
                if (s != null && xs != null)
                {
                    Tabs = (ObservableCollection<Tab>?)xs.Deserialize(s);
                }                
            }
        }
        private void Checkbox_Click(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;            
            if (checkBox.IsChecked == true && checkBox.Content != null && (String)checkBox.Content == "Hide Skulls")
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
                                        if (check.CheckType != null && (String)check.CheckType == "skull")
                                        {
                                            check.IsVisible = "Hidden";
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (checkBox.IsChecked == false && checkBox.Content != null && (String)checkBox.Content == "Hide Skulls")
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
                                        if (check.CheckType != null && (String)check.CheckType == "skull")
                                        {
                                            check.IsVisible = "Visible";
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (checkBox.IsChecked == true && checkBox.Content != null && (String)checkBox.Content == "Hide Fairies")
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
                                        if (check.CheckType != null && (String)check.CheckType == "fairy")
                                        {
                                            check.IsVisible = "Hidden";
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (checkBox.IsChecked == false && checkBox.Content != null && (String)checkBox.Content == "Hide Fairies")
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
                                        if (check.CheckType != null && (String)check.CheckType == "fairy")
                                        {
                                            check.IsVisible = "Visible";
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            XmlSerializer xs = new XmlSerializer(typeof(ObservableCollection<Tab>));

            using StreamWriter sr = new StreamWriter("SaveData.xml");
            using var xw = XmlWriter.Create(sr, new XmlWriterSettings { Indent = true, IndentChars = "\t"});
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
            if (File.Exists("SaveData.xml"))
            {
                File.Delete("SaveData.xml");
            }
        }
    }
}
