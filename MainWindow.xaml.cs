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
        private readonly string DirPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\OoTxMM-Track\\";
        public ObservableCollection<Tab>? Tabs { get; set; }
        public MainWindow()
        {            
            AddElement();
            DataContext = this;
            InitializeComponent();
        }
        public void AddElement()
        {
            Tabs = new();
            if (!Directory.Exists(DirPath))
            {
                Directory.CreateDirectory(DirPath);
            }
            if (!File.Exists($"{DirPath}SaveData.xml"))
            {
#if(DEBUG)
                ImportData imp = new();
                imp.Import(Tabs);
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
                                            check.IsVisible = "Collapsed";
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
                                            check.IsVisible = "Collapsed";
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
            XmlSerializer xs = new(typeof(ObservableCollection<Tab>));
            using StreamWriter sr = new($"{DirPath}SaveData.xml");
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
            if (File.Exists($"{DirPath}SaveData.xml"))
            {
                File.Delete($"{DirPath}SaveData.xml");
            }
        }
    }
}
