using System.Collections.ObjectModel;

namespace OoTxMM_Track.Model
{
    public sealed class Tab
    {
        public string? TabName { get; set; }
        public string? Content { get; set; }
        public string? Index { get; set; } = "0";
        public ObservableCollection<Region>? Regions { get; set; }
    }
}
