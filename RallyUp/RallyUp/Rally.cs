using System;
using System.Collections.Generic;
using System.Text;

namespace RallyUp
{
    public class Rally
    {
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public string invitation { get; set; }
        public List<Models.RallyContact> invitees { get; set; }
    }
}
