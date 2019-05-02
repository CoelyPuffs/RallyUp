using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using System.ComponentModel;

namespace RallyUp
{
    public class RallyTimer : INotifyPropertyChanged
    {
        //DateTime Start;
        DateTime End;
        Timer Timer;
        public string TimerString { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public RallyTimer(DateTime end)
        {
            //this.Start = start;
            this.End = end;
            Timer = new Timer(1000);
            Timer.Elapsed += OnTimedEvent;
            Timer.AutoReset = true;
            Timer.Enabled = true;
            Xamarin.Forms.MessagingCenter.Send<RallyTimer>(this, "RallyStarted");
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            updateString(e.SignalTime);
        }

        private void updateString(DateTime currentTime)
        {
            if (End.CompareTo(currentTime) <= 0)
            {
                Timer.Enabled = false;
                TimerString = "00:00";
                Xamarin.Forms.MessagingCenter.Send<RallyTimer>(this, "RallyEnded");
            }
            else
            {
                TimeSpan timeDiff = End.Subtract(currentTime);
                TimerString = timeDiff.Minutes.ToString("D2") + ":" + timeDiff.Seconds.ToString("D2");
            }
            OnPropertyChanged("TimerString");
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
