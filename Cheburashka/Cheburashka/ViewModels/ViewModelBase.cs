using System.ComponentModel;

namespace Cheburashka.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;


        protected void OnPropertyChanged(string prop = "")
        {
            var c = PropertyChanged;
            c?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
