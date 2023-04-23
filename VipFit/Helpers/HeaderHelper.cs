namespace VipFit.Helpers
{
    using CommunityToolkit.Mvvm.ComponentModel;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public class HeaderHelper : ObservableRecipient, INotifyPropertyChanged
    {
        private string text;

        public string Text
        {
            get => text;
            set => Set(ref text, value);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Set<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return;
            }

            storage = value;
            OnPropertyChanged(propertyName);
        }

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
