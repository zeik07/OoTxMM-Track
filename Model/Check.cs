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
                if (_isChecked == null)
                {
                    _isChecked = false;
                }
                return _isChecked;
            }
            set
            {
                _isChecked = value;
                OnPropertyChanged();
            }
        }
    }
}
