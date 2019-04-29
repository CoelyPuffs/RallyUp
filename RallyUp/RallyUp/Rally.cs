using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace RallyUp
{
    public class Rally : INotifyPropertyChanged
    {
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public string invitation { get; set; }
        public List<Models.RallyContact> invitees { get; set; }
        public string inviteeSummary { get; set; }

        private int acceptedCount = 0;
        private int declinedCount = 0;
        private int undeterminedCount = 0;
        private int unrepliedCount = 0;

        public event PropertyChangedEventHandler PropertyChanged;

        public void refreshInviteeSummary()
        {
            acceptedCount = 0;
            declinedCount = 0;
            undeterminedCount = 0;
            unrepliedCount = 0;

            foreach (Models.RallyContact contact in invitees)
            {
                switch (contact.Status)
                {
                    case 0:
                        unrepliedCount++;
                        break;
                    case 1:
                        undeterminedCount++;
                        break;
                    case 2:
                        declinedCount++;
                        break;
                    case 3:
                        acceptedCount++;
                        break;
                }
            }
            inviteeSummary = "✓: " + acceptedCount.ToString() + ", ✗: " + declinedCount.ToString() + ", ?: " + undeterminedCount.ToString() + ", ―:" + unrepliedCount.ToString();
            OnPropertyChanged("inviteeSummary");
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
