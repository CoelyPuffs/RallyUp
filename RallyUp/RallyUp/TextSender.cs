using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;

using Plugin.Messaging;

namespace RallyUp
{
    public class TextSender
    {
        private Queue<List<string>> backlog = new Queue<List<string>>();
        private ISmsTask sender = CrossMessaging.Current.SmsMessenger;
        private Timer Timer = new Timer(3000);

        public TextSender()
        {
            Timer.Elapsed += OnTimedEvent;
            Timer.AutoReset = true;
            Timer.Enabled = true;
        }

        public void sendText(string recipient, string textMessage)
        {
            backlog.Enqueue(new List<string> { recipient, textMessage});
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            if(backlog.Count > 0)
            {
                sender.SendSmsInBackground(backlog.Peek()[0], backlog.Peek()[1]);
                backlog.Dequeue();
            }
        }

        public bool GetBacklogEmpty()
        {
            return (backlog.Count <= 0);
        }
    }
}
