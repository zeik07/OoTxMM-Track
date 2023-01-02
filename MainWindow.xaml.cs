using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Xml;
using System.Xml.Serialization;

namespace OoTxMM_Track
{
    public partial class MainWindow : Window
    {       
        public MainWindow()
        {            
            AddElement();
            DataContext = this;
            InitializeComponent();
        }
        public ObservableCollection<TabItem>? Tabs { get; set; }
        public bool ShowSkulls { get; set; } = true;
        public int TotalChecks { get; set; } = 0;
        public void AddElement()
        {
            if (!File.Exists("SaveData.xml"))
            {
                Tabs = new ObservableCollection<TabItem>();
                XmlDocument gameData = new();
                gameData.Load($@"{Directory.GetCurrentDirectory()}\GameData.xml");

                if (gameData.DocumentElement != null)
                {
                    foreach (XmlNode tab in gameData.DocumentElement.ChildNodes)
                    {
                        ObservableCollection<Region> regionsList = new();
                        foreach (XmlNode region in tab.ChildNodes)
                        {
                            ObservableCollection<Checks> checksList = new();
                            foreach (XmlNode check in region.ChildNodes)
                            {
                                if (check.Name == "skull" && ShowSkulls is false)
                                {
                                    continue;
                                }
                                checksList.Add(new Checks { Name = check.InnerText });
                                if (tab.Attributes?["name"]?.InnerText != "Settings")
                                {
                                    TotalChecks += 1;
                                }
                            }
                            regionsList.Add(new Region { Header = region.Attributes?["name"]?.InnerText, Check = checksList });
                        }
                        Tabs.Add(new TabItem { Header = tab.Attributes?["name"]?.InnerText, Index = "0", Region = regionsList });
                    }
                }
                foreach (TabItem t in Tabs)
                {
                    t.Content = $"Total Checks: {TotalChecks}";
                }
            }
            else
            {
                XmlSerializer xs = new XmlSerializer(typeof(ObservableCollection<TabItem>));
                Stream reader = new FileStream("SaveData.xml", FileMode.Open);
                if (reader != null && xs != null)
                {
                    Tabs = (ObservableCollection<TabItem>?)xs.Deserialize(reader);
                }                
            }
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            XmlSerializer xs = new XmlSerializer(typeof(ObservableCollection<TabItem>));
            using (StreamWriter wr = new StreamWriter("SaveData.xml"))
            {
                xs.Serialize(wr, Tabs);
            }
        }
        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            if (Tabs != null)
            {
                foreach (TabItem tab in Tabs)
                {
                    if (tab.Region != null)
                    {
                        foreach (Region reg in tab.Region)
                        {
                            if (reg.Check != null)
                            {
                                foreach (Checks check in reg.Check)
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
            var left = Application.Current.MainWindow.Left;
            var top = Application.Current.MainWindow.Top;
            MainWindow newWin = new MainWindow
            {
                Left = left,
                Top = top
            };
            this.Close();
            newWin.Show();            
        }
    }
    public sealed class TabItem
    {
        public string? Header { get; set; }
        public string? Content { get; set; }
        public string? Index { get; set; } = "0";
        public ObservableCollection<Region>? Region { get; set; }
    }

    public class Region
    {
        public string? Header { get; set; }
        public ObservableCollection<Checks>? Check { get; set; }
    }
    public class Checks
    {
        public string? Name { get; set; }
        public bool? IsChecked { get; set; } = false;
    }

}
