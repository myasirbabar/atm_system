using System;
using System.Collections;
using sc = System.Console;
using Customer = BO.Customer;
using bll = BLL.AdminBLL;
using System.Globalization;

namespace View
{
    public class AdminView
    {
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


        //Input Password and Front-End Validations
        private static int inputPassword()
        {
            int password;
            while (true)
            {
                try
                {
                    sc.Write("Pin Code : ");
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

            return password;

        }


        //Input Balance and Front-End Validations
        private static float inputBalance(char n = 'y')
        {

            float balance;
            while (true)
            {
                try
                {
                    if (n != 'n') sc.Write("Starting Balance: ");

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


        public static void Start()
        {

            while (true)
            {
                sc.Clear();

            MENU:
                sc.WriteLine("\t\t\t\tWELCOME ADMIN ");

                sc.WriteLine("Select An Option From Menu Below");
                sc.WriteLine("\t1----Create New Account.");
                sc.WriteLine("\t2----Delete Existing Account.");
                sc.WriteLine("\t3----Update Account Information.");
                sc.WriteLine("\t4----Search for Account.");
                sc.WriteLine("\t5----View Reports");
                sc.WriteLine("\t6----Exit");

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
                        createAccount();
                        break;

                    case '2':
                        deleteAccount();
                        break;

                    case '3':
                        updateAccount();
                        break;

                    case '4':
                        searchUsers();
                        break;

                    case '5':
                        viewReports();
                        break;

                    case '6':
                        return;

                    default:
                        sc.WriteLine("Invalid Choice Try Again !");
                        goto MENU;
                }
            }

        }

        private static void viewReports()
        {
            ArrayList AL = new ArrayList();

            sc.WriteLine("Select a critera to View Reports ");
            sc.Write("\t1---- Accounts By Amount\n\t2---- Accounts By Date\nEnter : ");

            string op = sc.ReadLine();
            if (op != "" && (op[0] == '1' || op[0] == '2'))
            {
                if (op[0] == '1')
                {
                    float min, max;
                    sc.Write("Enter the minimum amount: ");
                    min = inputBalance('n');

                    sc.Write("Enter the maximum amount: ");
                    max = inputBalance('n');

                    AL = bll.getReportsByAmount(min, max);
                }
                else
                {
                    sc.Write("Enter Account Number : ");
                    int acc = inputInteger();

                    //Check if user exists or not 
                    Customer r = bll.Exists(acc);
                    if (r == null)
                    {
                        sc.WriteLine("This User Does not Exist. Try Again ");
                        goto END;
                    }

                    sc.WriteLine("The format for the date must be Day/Month/Year [DD/MM/YYYY]");

                    DateTime dtS, dtE;
                    //Validate input date 
                    while (true)
                    {
                        try
                        {
                            CultureInfo provider = CultureInfo.InvariantCulture;
                            sc.Write("Enter the Starting Date : ");
                            dtS = DateTime.ParseExact(sc.ReadLine(), "dd/MM/yyyy", provider);

                            break;
                        }
                        catch (Exception)
                        {
                            sc.WriteLine("Invalid Input Date\n");
                        }
                    }

                    while (true)
                    {
                        try
                        {
                            CultureInfo provider = CultureInfo.InvariantCulture;
                            sc.Write("Enter the Ending Date : ");
                            dtE = DateTime.ParseExact(sc.ReadLine(), "dd/MM/yyyy", provider);
                            break;
                        }
                        catch (Exception)
                        {
                            sc.WriteLine("Invalid Input Date\n");
                        }
                    }


                    AL = bll.getReportsByDate(r,dtS, dtE);
                }
            }
            else
            {
                sc.WriteLine("Invalid Choice ! ");
                goto END;
            }

            if(AL.Count == 0)
            {
                sc.WriteLine("\nNo Record Found ! ");
                goto END;
            }

            sc.WriteLine("\n\t\t\t\t====== SEARCH RESULTS ======\n");

            if (op[0] == '1')
            {
                sc.WriteLine("Account ID \tUser ID \t\tHolders Name \t\t\tType \t\tBalance \tStatus");
                sc.WriteLine("-----------------------------------------------------------------------------------------------------------------");
                foreach (Customer i in AL)
                {
                    sc.WriteLine(i.Accountno.ToString().PadRight(5) + "\t\t" + i.Id.PadRight(10) + "\t\t" + i.Name.PadRight(20) + "\t\t" + i.Type.PadRight(5) + "\t\t" + i.Balance.ToString().PadRight(7) + "\t\t" + i.Status);
                }

            }
            else
            {
                sc.WriteLine("Serial No \tTransaction Type \tUser ID \t\tHolders Name \t\t\tAmount \t\tDate");
                sc.WriteLine("-----------------------------------------------------------------------------------------------------------------------------");
                int i = 0;
                foreach (string[] d in AL)
                {
                    sc.WriteLine((++i).ToString().PadRight(10) + "\t" + d[0].PadRight(15) + "\t\t" + d[1].PadRight(10) + "\t\t" + d[2].PadRight(20) + "\t\t" + d[3].PadRight(5) + "\t\t" + d[4].Replace('-','/'));
                }
            }
            sc.WriteLine("-----------------------------------------------------------------------------------------------------------------------------");


        END:
            sc.Write("\nPress any key to continue !");
            sc.ReadKey();

            return;
        }

        private static void searchUsers()
        {
            Customer c = new Customer();// To hold Details 

            sc.WriteLine("\t\tSearch Menu");
            sc.Write("Account No : ");
            string acc = sc.ReadLine();
            if (acc != "")
            {
                try { c.Accountno = Convert.ToInt32(acc); }
                catch (FormatException) { sc.WriteLine("Invalid Input ! By default account number is ignored in search. "); c.Accountno = -1; }
            }
            else
            {
                c.Accountno = -1;
            }


            sc.Write("Login ID : ");
            c.Id = sc.ReadLine();

            sc.Write("Holder Name : ");
            c.Name = sc.ReadLine();

            sc.Write("Type ");
            sc.WriteLine("\t 1. Savings");
            sc.WriteLine("\t 2. Current");
            sc.Write("Enter : ");

            string op = sc.ReadLine();
            if (op == "")
                c.Type = "";
            else if (op[0] == '1')
                c.Type = "Savings";
            else if (op[0] == '2')
                c.Type = "Current";
            else
            {
                sc.WriteLine("Invalid Choice ! By default 'Null' has been set. ");
                c.Type = "";
            }

            sc.Write("Balance : ");
            string b = sc.ReadLine();
            if (b != "")
            {
                try { c.Balance = float.Parse(b); }
                catch (FormatException) { sc.WriteLine("Invalid Input ! By default Balance is ignored in search. "); c.Balance = -1; }
            }
            else
            {
                c.Balance = -1;
            }


            sc.Write("Status");
            sc.WriteLine("\t 1. Active");
            sc.WriteLine("\t 2. Disabled");
            sc.Write("Enter : ");

            op = sc.ReadLine();
            if (op == "")
                c.Status = "";
            else if (op[0] == '1')
                c.Status = "Active";
            else if (op[0] == '2')
                c.Status = "Disabled";
            else
            {
                sc.WriteLine("Invalid Choice ! By default 'Null' has been set. ");
                c.Status = "";
            }

            ArrayList list = bll.searchResults(c);
            if (list.Count == 0)
            {
                sc.WriteLine("No Results Found !!!");
            }
            else
            {
                sc.WriteLine("\n\t\t\t\t====== SEARCH RESULTS ======\n");
                sc.WriteLine("Account ID \tUser ID \t\tHolders Name \t\t\tType \t\tBalance \tStatus");
                sc.WriteLine("-----------------------------------------------------------------------------------------------------------------");
                foreach (Customer i in list)
                {
                    sc.WriteLine(i.Accountno.ToString().PadRight(5) + "\t\t" + i.Id.PadRight(10) + "\t\t" + i.Name.PadRight(20) + "\t\t" + i.Type.PadRight(5) + "\t\t" + i.Balance.ToString().PadRight(7) + "\t\t" + i.Status);
                }

                sc.WriteLine("-----------------------------------------------------------------------------------------------------------------");
            }

            sc.Write("\nPress any key to continue !");
            sc.ReadKey();

            return;
        }

        private static void updateAccount()
        {
            bool enL = false, enP = false;
            sc.Write("Enter Account Number to Update : ");
            int acc = inputInteger();

            Customer c = bll.Exists(acc);

            if (c == null)
            {
                sc.WriteLine("This User Does not exists ");
            }
            else
            {
                string id, name;
                int pass;

                sc.WriteLine("\t\t\tCurrent Information");
                sc.WriteLine("Account # " + c.Accountno);
                sc.WriteLine("Type : " + c.Type);
                sc.WriteLine("Holder : " + c.Name);
                sc.WriteLine("Balance : " + c.Balance);
                sc.WriteLine("Status : " + c.Status);


                sc.WriteLine("Please enter in the fields you wish to update(leave blank otherwise):");

                sc.Write("Login : ");
                id = sc.ReadLine();
                if (id != "") { c.Id = id;enL = true; }

                //PAssword & Validation
                while (true)
                {
                    sc.Write("Pin Code : ");
                    string test = sc.ReadLine();
                    if (test != "")
                    {
                        try
                        {
                            pass = Convert.ToInt32(test);
                            if (pass.ToString().Length > 5)
                            {
                                sc.WriteLine("Invalid Length !");
                                continue;
                            }
                        }
                        catch (FormatException) { sc.WriteLine("Only Digits Allowed ! "); continue; }

                        c.Password = pass;
                        enP = true;
                        break;
                    }
                    else
                    {
                        
                        break;
                    }
                }



                sc.Write("Holder Name : ");
                name = sc.ReadLine();
                if (name != "") { c.Name = name; }

                sc.WriteLine("Status");
                sc.WriteLine("\t 1. Active");
                sc.WriteLine("\t 2. Disabled");

                sc.Write("Enter : ");
                string op = sc.ReadLine();

                if(op == "")
                {
                    sc.WriteLine("Invalid Choice ! By default Old Value is Assigned ");
                }
                else if (op[0] == '1')
                    c.Status = "Active";
                else if (op[0] == '2')
                    c.Status = "Disabled";
                else
                {
                    sc.WriteLine("Invalid Choice ! By default Old Value is Assigned ");
                }


                if (bll.UpdateUser(c,enL,enP))
                {
                    sc.WriteLine("\n\nUser Upated Successfully !");
                }
                else
                {
                    sc.WriteLine("Error Updating User !");
                }

            }


            sc.Write("\nPress any key to continue !");
            sc.ReadKey();

            return;
        }

        private static void deleteAccount()
        {
            sc.Write("Enter Account Number to delete : ");
            int acc = inputInteger();

            Customer c = bll.Exists(acc);

            if (c == null)
            {
                sc.WriteLine("This User Does not exists ");
            }
            else
            {
                sc.WriteLine("You wish to delete the account held by '{0}'; If this information is correct please re - enter Account number", c.Name);
                sc.Write("Enter : ");
                int con = inputInteger();

                if (con == acc)
                {
                    if (bll.Delete(c))
                    {
                        sc.WriteLine("Account Deleted Successfully ! ");
                    }
                    else
                    {
                        sc.WriteLine("Error Deleting Account");
                    }
                }
                else
                {
                    sc.WriteLine("Sorry ! Your Request Cannot be proceeded ! ");
                }
            }



            sc.Write("\nPress any key to continue !");
            sc.ReadKey();

            return;
        }

        private static void createAccount()
        {
            string id, name, type, status;
            float balance;
            int pass;


            sc.Write("Login : ");
            id = sc.ReadLine();


            pass = inputPassword();


            sc.Write("Holders Name  : ");
            name = sc.ReadLine();


            sc.WriteLine("Type          ");
            sc.WriteLine("\t 1. Savings");
            sc.WriteLine("\t 2. Current");
            sc.Write("Enter : ");

            char op = sc.ReadLine()[0];
            if (op == '1')
                type = "Savings";
            else if (op == '2')
                type = "Current";
            else
            {
                sc.WriteLine("Invalid Choice ! By default 'Current' has been set. ");
                type = "Current";
            }

            balance = inputBalance();

            sc.WriteLine("Status        ");
            sc.WriteLine("\t 1. Active");
            sc.WriteLine("\t 2. Disabled");
            sc.Write("Enter : ");

            op = sc.ReadLine()[0];
            if (op == '1')
                status = "Active";
            else if (op == '2')
                status = "Disabled";
            else
            {
                sc.WriteLine("Invalid Choice ! By default 'Active' has been set. ");
                status = "Active";
            }

            Customer c = new Customer(id, pass, name, type, balance, status);
            int ret = bll.AddUser(c);
            if (ret != -1)
            {
                sc.WriteLine("User Added Successfully ! Account Number Assigned is : " + ret);

            }
            else
            {
                sc.WriteLine("User Already Exists");
            }

            sc.Write("\nPress any key to continue !");
            sc.ReadKey();

            return;
        }

    }
}
