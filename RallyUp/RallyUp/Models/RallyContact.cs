using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace RallyUp.Models
{
    public class RallyContact : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public string Number { get; set; }
        // 0 : No reply
        // 1 : Text received but no acceptance determined
        // 2 : Declined
        // 3 : Accepted
        public int Status { get; set; }
        public string StatusString { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public RallyContact(string Name, string Number)
        {
            this.Name = Name;
            this.Number = Number;
            this.Status = 0;
            this.StatusString = "―";
        }

        public void modifyStatus(int newStatus)
        {
            if (newStatus >= 0 && newStatus <= 3)
            {
                this.Status = newStatus;
                switch (newStatus)
                {
                    case 0:
                        this.StatusString = "―";
                        break;
                    case 1:
                        this.StatusString = "?";
                        break;
                    case 2:
                        this.StatusString = "✗";
                        break;
                    case 3:
                        this.StatusString = "✓";
                        break;
                }
                OnPropertyChanged("StatusString");
            }
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
