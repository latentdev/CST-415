using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using ClientModel;
using Protocol;
using System.Collections.ObjectModel;

namespace Client_UI
{
    class MainWindowVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;


        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private Client model = new Client();

        public MainWindowVM()
        {
            model.DataReceived += Model_DataReceived;
            Record = new ObservableCollection<Message>();
        }

        private void Model_DataReceived(object sender, EventArgs e)
        {
            record.Add(new Message() { msg_type = model.response.msg_type, service_name = model.response.service_name, port = model.response.port, status = model.response.status });
        }

        private ObservableCollection<Message> record;
        public ObservableCollection<Message> Record
        {
            get
            {
                return record;
            }
            set
            {
                record = value; NotifyPropertyChanged();
            }
        }
        public RelayCommand RequestPortCommand { get { return new RelayCommand((x) => RequestPort(x)); } }
        private void RequestPort(object x)
        {
            //ReadOnly = true;
            model.RequestPort();
        }

        public RelayCommand LookUpPortCommand { get { return new RelayCommand((x) => LookUpPort(x)); } }
        private void LookUpPort(object x)
        {
            //ReadOnly = true;
            model.LookUpPort(40000);
        }
    }
}
