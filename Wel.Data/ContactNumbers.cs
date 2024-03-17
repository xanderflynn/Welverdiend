using System;
using System.Collections.Generic;
using System.Text;

namespace Wel.Data
{
    public class ContactNumbers
    {
        public ContactNumbers(string contactName, string Number1, string Number2, string lowCategory, string highCategory)
        {
            this.number1 = Number1;
            this.number2 = Number2;
            this.HighCategory = highCategory;
            this.LowCategory = lowCategory;
            this.ContactName = contactName;

        }


        public string number1 { get; set; }
        public string number2 { get; set; }
        public string HighCategory { get; set; }
        public string LowCategory { get; set; }
        public string ContactName { get; set; }
    }
}
