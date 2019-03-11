using System;
using System.Collections.Generic;
using System.Text;

namespace RallyUp.Models
{
    class RallyContact
    {
        public string Name { get; set; }
        public string Number { get; set; }

        public RallyContact(string Name, string Number)
        {
            this.Name = Name;
            this.Number = Number;
        }
    }
}
