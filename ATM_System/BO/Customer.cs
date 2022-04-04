using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class Customer
    {
        private string id;
        private string name;
        private int password;
        private int accountno;
        private float balance;
        private string status;
        private string type;



        public Customer(string i = "", int pass = -1, string n= "", string t="", float b=-1, string s="", int acc = -1)
        {
            accountno = acc;
            id = i;
            name = n;
            password = pass;
            balance = b;
            status = s;
            type = t;
        }

        public string Id
        {
            set { id = value; }
            get { return id; }
        }
        public int Password
        {
            set { password = value; }
            get { return password; }
        }
        public float Balance
        {
            set { balance = value; }
            get { return balance; }
        }

        public string Name
        {
            set { name = value; }
            get { return name; }
        }
        public int Accountno
        {
            set { accountno = value; }
            get { return accountno; }
        }
        public string Status
        {
            set { status = value; }
            get { return status; }
        }
        public string Type
        {
            set { type = value; }
            get { return type; }
        }


    }
}
