using Cheburashka.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Cheburashka
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public readonly ObservableCollection<UserViewModel> UsersCollection;
        private Thread _network;

        public MainWindow()
        {
            InitializeComponent();

            UsersCollection = new ObservableCollection<UserViewModel>();
            Users.ItemsSource = UsersCollection;
            Users.SelectionChanged += (s, e) => ((UserViewModel)Users.SelectedItem).HaveNewMessages = false;

            UsersCollection.Add(new UserViewModel(this) { NickName = "Nick" });
        }

        private void Connect(object sender, RoutedEventArgs e)
        {
            if (_network == null)
            {
                _network = new Thread(new ThreadStart(CreateConnection));
                _network.Start();


                Login.IsReadOnly = true;
                Ip.IsReadOnly = true;
                Port.IsReadOnly = true;

                ConnectButton.Content = "Отключиться";
            }
            else
            {

                _network = null;

                Login.IsReadOnly = false;
                Ip.IsReadOnly = false;
                Port.IsReadOnly = false;

                ConnectButton.Content = "Подключиться";
            }
        }

        private void CreateConnection()
        {

        }
    }

    public class ScrollingListBox : ListBox
    {
        protected override void OnItemsChanged(System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems == null)
                return;
            int newItemCount = e.NewItems.Count;

            if (newItemCount > 0)
                this.ScrollIntoView(e.NewItems[newItemCount - 1]);

            base.OnItemsChanged(e);
        }
    }
}
