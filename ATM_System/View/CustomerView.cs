using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Customer = BO.Customer;
using sc = System.Console;
using bll = BLL.CustomerBLL;
using adminBll = BLL.AdminBLL;
using System.Globalization;

namespace View
{
    public class CustomerView
    {

        public static Customer c; //PASSWORD & ID ARE BOTH ENCRYPTED HERE 
        private static float inputBalance()
        {

            float balance;
            while (true)
            {
                try
                {
                    sc.Write("Enter Amount: ");
                    balance = float.Parse(sc.ReadLine());

                    break;
                }
                catch (FormatException)
                {
                    sc.WriteLine("Invalid ! Only  Digits are Allowed. ");
                }
            }

            return balance;
        }

        private static int inputInteger()
        {
            int n;
            while (true)
            {
                try
                {
                    n = Convert.ToInt32(sc.ReadLine());

                    break;
                }
                catch (FormatException)
                {
                    sc.Write("Invalid ! Only  Digits are Allowed. ");
                }
            }

            return n;
        }



        public static void Start()
        {
            while (true)
            {
                sc.Clear();

            MENU:
                sc.WriteLine("\t\t\t\tWELCOME " + c.Name);

                sc.WriteLine("Select An Option From Menu Below");
                sc.WriteLine("\t1----Withdraw Cash.");
                sc.WriteLine("\t2----Cash Transfer.");
                sc.WriteLine("\t3----Deposit Cash.");
                sc.WriteLine("\t4----Display Balance.");
                sc.WriteLine("\t5----Exit");

                char option;
                while (true)
                {
                    sc.Write("Enter : ");
                    string op = sc.ReadLine();
                    if (op != "")
                    {
                        option = op[0];
                        break;
                    }
                }

                sc.Clear();

                switch (option)
                {
                    case '1':
                        withdrawCash();
                        break;

                    case '2':
                        transferCash();
                        break;

                    case '3':
                        depositCash();
                        break;

                    case '4':
                        displayBalance();
                        break;

                    case '5':
                        return;

                    default:
                        sc.WriteLine("Invalid Choice Try Again !");
                        goto MENU;
                }
            }
        }

        private static void transferCash()
        {
            sc.Write("To Tranfer ");
            float amount = inputBalance();

            //Check if transaction is possible or not 
            if (amount < 0)
            {
                sc.WriteLine("\n Invalid Amount ! ");
                goto END;
            }
            if (c.Balance < amount)
            {
                sc.WriteLine("\nYou have Insufficient Balance for this Transaction ! ");
                goto END;
            }
            while ((amount % 500) != 0)
            {
                sc.Write("Invalid Please ");
                amount = inputBalance();
            }


            AGAIN:
            sc.Write("Enter Receiver Account Number : ");
            int acc = inputInteger();

            if(acc == c.Accountno)
            {
                sc.WriteLine("You can not Transfer Balance to Same Account ");
                goto END;
            }

            Customer r = adminBll.Exists(acc);

            if (r == null)
            {
                sc.WriteLine("This User Does not Exist. Try Again ");
                goto AGAIN;
            }

            sc.WriteLine("\nYou wish to deposit Rs {0} in account held by {1};\nIf this information is correct please re - enter the account number ", amount,r.Name);
            sc.Write("\nEnter : ");
            int con = inputInteger();

            if(con != acc)
            {
                sc.WriteLine("Your Request Can not be proceed ! ");
                goto END;
            }

            //Transfer amount and print receipt 
            c = bll.Transfer(c,r, amount);

            if (c != null)
            {
                //Receipt Printing  
                sc.WriteLine("\nCash Deposited Successfull ! ");
                sc.Write("\nEnter '1' to Print Receipt : ");

                string ch = sc.ReadLine();
                if (ch != "" && ch[0] == '1')
                {
                    printRecipt("Amount Transferred", c, amount);
                }
            }

            END:
            sc.WriteLine("\n\nPress Any Key To Continue !");
            sc.ReadKey();

            return;
        }

        private static void displayBalance()
        {
            sc.WriteLine("\n YOU CURRENT RECEIPT \n");
            DateTime dt = DateTime.Now;

            sc.WriteLine("Account # " + c.Accountno);
            sc.WriteLine("Date : " + dt.Day + "/" + dt.Month + "/" + dt.Year);
            sc.WriteLine("Balance : " + c.Balance);

            sc.WriteLine("\n\nPress Any Key To Continue !");
            sc.ReadKey();
            return;
        }

        private static void depositCash()
        {
            sc.Write("To Deposit ");
            float amount = inputBalance();

            if (amount < 0)
            {
                sc.WriteLine("\n Invalid Amount ! ");
                goto END;
            }

            //Update amount and print receipt 
            c = bll.Deposit(c, amount);
            if (c != null)
            {
                //Receipt Printing  
                sc.WriteLine("Cash Deposited Successfull ! ");
                sc.Write("Enter '1' to Print Receipt : ");

                string ch = sc.ReadLine();
                if (ch != "" && ch[0] == '1')
                {
                    printRecipt("Deposited", c, amount);
                }
            }

            END:
            sc.WriteLine("\n\nPress Any Key To Continue !");
            sc.ReadKey();

            return;
        }

        private static void withdrawCash()
        {
            float amount;
        MENU:
            sc.WriteLine("Please select a mode of withdrawal");
            sc.WriteLine("\ta) Fast Cash");
            sc.WriteLine("\tb) Normal Cash");

            char option;
            while (true)
            {
                sc.Write("Enter : ");
                string op = sc.ReadLine();
                if (op != "")
                {
                    option = op[0];
                    break;
                }
            }

            sc.Clear();
            if (option == 'a')
            {
                sc.WriteLine("Select An Amount to Withdraw");

                sc.WriteLine("\t1----500");
                sc.WriteLine("\t2----1000");
                sc.WriteLine("\t3----2000");
                sc.WriteLine("\t4----5000");
                sc.WriteLine("\t5----10000");
                sc.WriteLine("\t6----15000");
                sc.WriteLine("\t7----20000");

                int i;
                while (true)
                {
                    sc.Write("Enter : ");
                    string op = sc.ReadLine();
                    if (op != "")
                    {
                        try
                        {
                            i = Convert.ToInt32(op[0]) - 48;
                            if (i < 1 || i > 7)
                                continue;
                        }
                        catch (FormatException)
                        {
                            sc.WriteLine("Invalid entry");
                            continue;
                        }
                        break;
                    }
                }
                int[] array = { 500, 1000, 2000, 5000, 10000, 15000, 20000 };
                amount = array[i - 1];

                sc.Write("Are You Sure You Want To Withdraw 'Rs . {0}' ? (Y/N) : ", amount);
                string s = sc.ReadLine();
                if(s == "" || s[0] != 'Y')
                {
                    goto END;
                }
            }

            else if (option == 'b')
            {
                sc.Write("To withdraw ");
                amount = inputBalance();

            }

            else
            {
                sc.WriteLine("Invalid Choice Try Again !");
                goto MENU;
            }


            //Check if transaction is possible or not 
            if (amount < 0)
            {
                sc.WriteLine("\n Invalid Amount ! ");
                goto END;
            }
            if (c.Balance < amount)
            {
                sc.WriteLine("\nYou have Insufficient Balance for this Transaction ! ");
                goto END;
            }

            if (bll.exceedsLimit(c, DateTime.Now.ToString("dd/MM/yyyy"), amount)){
                sc.WriteLine("\nYour Transaction Limit Has Exceeded !");
                goto END;
            }

            //Update amount and print receipt 
            c = bll.Withdraw(c, amount);
            if (c != null)
            {
                //Receipt Printing  
                sc.WriteLine("Transaction Successfull ! ");
                sc.Write("Enter '1' to Print Receipt : ");

                string ch = sc.ReadLine();
                if (ch != "" && ch[0] == '1')
                {
                    printRecipt("Withdrawn", c, amount);
                }
            }

            END:
            sc.WriteLine("\n\nPress Any Key To Continue !");
            sc.ReadKey();

            return;
        }

        private static void printRecipt(string v, Customer c, float balance)
        {
            //Get Current Date and Time
            DateTime dt = DateTime.Now;

            sc.WriteLine("\n\t\t\t TRANSACTION RECEIPT\n");
            sc.WriteLine("Account # " + c.Accountno);
            sc.WriteLine("Date : " + dt.Day + "/" + dt.Month + "/" + dt.Year);//Printing Date Accordingly 
            sc.WriteLine(v + " : " + balance);
            sc.WriteLine("Balance : " + c.Balance);

            return;
        }
    }
}
