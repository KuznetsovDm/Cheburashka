using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Cheburashka.ViewModels
{
    public class UserViewModel : ViewModelBase
    {
        private readonly MainWindow _mainWindow;
        private readonly ObservableCollection<MessageViewModel> _messages;
        private bool _haveNewMessages;
        private string _newMessage;

        public UserViewModel(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
            _messages = new ObservableCollection<MessageViewModel>();
            SendCommand = new SimpleCommand(Send);
        }


        public IList<MessageViewModel> Messages => _messages;
        public string NickName { get; set; }
        public string LastMessage => _messages.LastOrDefault()?.Message;
        public DateTime? TimeLastMessage => _messages.LastOrDefault()?.SendTime;

        public string NewMessage
        {
            get => _newMessage;
            set
            {
                if (value == _newMessage)
                    return;
                _newMessage = value;
                OnPropertyChanged(_newMessage);
            }
        }

        public bool HaveNewMessages
        {
            get => _haveNewMessages;
            set
            {
                if (value == _haveNewMessages)
                    return;
                _haveNewMessages = value;
                OnPropertyChanged(nameof(HaveNewMessages));
            }
        }

        public ICommand SendCommand { get; }


        private void Send()
        {
            if (NewMessage == null || !NewMessage.Any())
                return;

            var message = new MessageViewModel()
            {
                IsYour = true,
                Message = NewMessage,
                SendTime = DateTime.Now,
            };
            NewMessage = null;
            _messages.Add(message);
            OnPropertyChanged(nameof(LastMessage));
        }

        public void AddMessage(MessageViewModel message)
        {
            _messages.Add(message);
            if (_mainWindow.Users.SelectedItem == this)
                HaveNewMessages = true;
            OnPropertyChanged(nameof(LastMessage));
        }
    }
}
