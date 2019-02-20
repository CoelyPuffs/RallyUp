using System;
using System.Collections.Generic;
using System.Text;

namespace RallyUp.Models
{
    class Contact
    {
        public string Name { get; set; }
        public string Number { get; set; }
        public bool Selected { get; set; } = false;

        public Contact(string Name, string Number)
        {
            this.Name = Name;
            this.Number = Number;
        }
    }
}
