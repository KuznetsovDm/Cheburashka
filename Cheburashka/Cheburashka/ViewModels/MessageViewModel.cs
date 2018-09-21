using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cheburashka.ViewModels
{
    public class MessageViewModel : ViewModelBase
    {
        private DateTime? _sendTime;
        private DateTime? _resvTime;
        private bool _hasError;
        private string _error;

        public bool IsYour { get; set; }
        public string Message { get; set; }

        public DateTime? SendTime
        {
            get => _sendTime;
            set
            {
                if (value == _sendTime)
                    return;
                _sendTime = value;
                OnPropertyChanged(nameof(SendTime));
            }
        }

        public DateTime? ResvTime
        {
            get => _resvTime;
            set
            {
                if (value == _resvTime)
                    return;
                _resvTime = value;
                OnPropertyChanged(nameof(ResvTime));
            }
        }

        public bool HasError
        {
            get => _hasError;
            set
            {
                if (value == _hasError)
                    return;
                _hasError = value;
                OnPropertyChanged(nameof(HasError));
            }
        }

        public string Error
        {
            get => _error;
            set
            {
                if (value == _error)
                    return;
                _error = value;
                OnPropertyChanged(nameof(Error));
            }
        }
    }
}
