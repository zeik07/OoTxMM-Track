using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Xml;

namespace OoTxMM_Track
{
    public partial class MainWindow : Window
    {       
        public MainWindow()
        {
            InitializeComponent();
            AddElement();
            DataContext = this;
        }
        public ObservableCollection<TabItem>? Tabs { get; set; }
        public bool ShowSkulls { get; set; } = true;
        public int totalChecks { get; set; } = 0;
        public void AddElement()
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
                            totalChecks += 1;
                        }
                        regionsList.Add(new Region { Header = region.Attributes?["name"]?.InnerText, Check = checksList });
                    }
                    Tabs.Add(new TabItem { Header = tab.Attributes?["name"]?.InnerText, Content = $"Total Checks: {totalChecks}", Region = regionsList });
                }
            }            
        }
    }
    
    public sealed class TabItem
    {
        public string? Header { get; set; }
        public string? Content { get; set; }
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
