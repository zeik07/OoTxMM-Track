using OoTxMM_Track.Model;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System;
using System.Windows.Media.Animation;

namespace OoTxMM_Track
{
    internal class ImportData
    {
        public int TotalChecks { get; set; } = 0;
        public void Import(ObservableCollection<Tab> Tabs, ObservableCollection<DisableChecks> DisableChecks)
        {
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
                            checksList.Add(new Check { CheckName = check.InnerText, CheckType = check.Name });                            
                            if (tab.Attributes?["name"]?.InnerText != "Settings")
                            {
                                DisableChecks.Add(new DisableChecks { CheckName = check.InnerText, CheckTag = region.Attributes?["name"]?.InnerText });
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
                                if (region.Attributes?["name"]?.InnerText == "Excluded Checks")
                                {
                                    foreach (var dCheck in DisableChecks)
                                    {
                                        checksList.Add(new Check { CheckName = dCheck.CheckName, CheckType = dCheck.CheckTag });
                                    }
                                }
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
            XmlSerializer xs = new(typeof(ObservableCollection<Tab>));

            using StreamWriter sr = new($"{Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent?.FullName}\\GameData.xml");
            using var xw = XmlWriter.Create(sr, new XmlWriterSettings { Indent = true, IndentChars = "\t" });
            xs.Serialize(xw, Tabs);

            using StreamWriter sr2 = new("GameData.xml");
            using var xw2 = XmlWriter.Create(sr2, new XmlWriterSettings { Indent = true, IndentChars = "\t" });
            xs.Serialize(xw2, Tabs);
        }
    }
}
