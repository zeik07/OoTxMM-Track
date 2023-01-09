using System.Collections.ObjectModel;

namespace OoTxMM_Track.Model
{
    public class Region
    {
        public string? RegionName { get; set; }
        public string? RegionType { get; set; }
        public ObservableCollection<Check>? Checks { get; set; }
    }
}
