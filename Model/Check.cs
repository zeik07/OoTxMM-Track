namespace OoTxMM_Track.Model
{
    public class Check : ObservableObject
    {
        public string? CheckName { get; set; }
        public string? CheckType { get; set; }
        public bool? _isChecked;
        public bool? IsChecked
        {
            get
            {
                _isChecked ??= false;
                return _isChecked;
            }
            set
            {
                _isChecked = value;
                OnPropertyChanged();
            }
        }
        public string? _isVisible;
        public string? IsVisible
        {
            get
            {
                _isVisible ??= "Visible";
                return _isVisible;
            }
            set
            {
                _isVisible = value;
                OnPropertyChanged();
            }
        }
    }
}
