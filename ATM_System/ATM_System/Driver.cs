using System;
using sc = System.Console;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bll = BLL.MainBLL;
using Customer = BO.Customer;
using View;

namespace ATM_System
{
    class Driver
    {
        public static void Main(string[] args)
        {

            string id;
            int password;

            sc.WriteLine("\t\t\t WELCOME ! ");

        START:

            //Input LOGIN ID
            sc.WriteLine("Simply Press Enter In Login Input To Exit Application !");
            sc.Write("\nLogin ID : ");
            id = sc.ReadLine();

            if (id == "")
            {
                goto END;
            }

            //Input Password and Front-End Validations
            while (true)
            {
                try
                {
                    sc.Write("Password : ");
                    password = Convert.ToInt32(sc.ReadLine());
                    if (password.ToString().Length > 5)
                    {
                        sc.WriteLine("Invalid Password Length ");
                        continue;
                    }
                    break;
                }
                catch (FormatException)
                {
                    sc.WriteLine("Invalid ! Only  Digits are Allowed. ");
                }
            }

            //Checking User 
            string msg, type = "";
            Customer c = null;
            bool flag = bll.init(id, password, out msg, ref type, ref c);
            if (flag)
            {
                if (type == "admin")
                    View.AdminView.Start();

                else if (type == "cust")
                {
                    CustomerView.c = c;
                    CustomerView.Start();
                }
            }
            sc.WriteLine(msg);

            goto START;

        END:
            sc.WriteLine("\n\nGOOD BYE !");

            sc.WriteLine("Press any key to Terminate ");
            sc.ReadKey();
        }

    }
}
