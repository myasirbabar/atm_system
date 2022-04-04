using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dal = DAL.Db;
using Customer = BO.Customer;
using sc = System.Console;


namespace BLL
{
    public class MainBLL
    {
        public static string cryptionLogin(string login)
        {
            char[] alpha = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
            char[] Alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            char[] numeric = "0123456789".ToCharArray();
            string cryption = "";

            for (int i = 0; i < login.Length; i++)
            {
                for (int j = 0, k = alpha.Length - 1, m = 0, n = numeric.Length - 1; j < alpha.Length; j++, k--)
                {
                    if (login[i] == alpha[j])
                    {
                        cryption += alpha[k];
                        break;
                    }
                    else if (login[i] == Alpha[j])
                    {
                        cryption += Alpha[k];
                        break;
                    }
                    if (m < numeric.Length)
                    {
                        if (login[i] == numeric[m])
                        {
                            cryption += numeric[n];
                        }
                        m++; n--;

                    }
                }
            }
            
            
            return cryption;
        }

        public static int cryptionPassword(string Password)
        {
            char[] alpha = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
            char[] numeric = "0123456789".ToCharArray();
            string cryption = "";

            for (int i = 0; i < Password.Length; i++)
            {
                for (int m = 0, n = numeric.Length - 1; m < numeric.Length; m++, n--)
                {
                    if (Password[i] == numeric[m])
                    {
                        cryption += numeric[n];
                        break;
                    }
                }
            }
            return Convert.ToInt32(cryption);

        }

        public static bool init(string id, int password, out string msg, ref string type, ref Customer c)
        {
            //Check if user is admin 
            if (id == "pucitAdmin" && password == 9101)
            {
                type = "admin";
                msg = "Logged Out Successfully ";
                return true;
            }

            //Otherwise search for customer
            else
            {
                //Encrypting Id To check if user exist.
                id = cryptionLogin(id);

                //Communicate with DAL to check user, User is tested based on Encrypted Id
                c = Dal.ReadById(id);

                //-> User is available
                if (c == null)
                {
                    msg = "This Login Does not Exist ! Try Again ";
                    return false;
                }

                //Decrypt Password to verify
                c.Password = cryptionPassword(c.Password.ToString());

                //-> User is inActive
                if (c.Status == "Disabled")
                {
                    msg = "Your Account is Currently Disabled ! Contact Admin";
                    return false;
                }

         

                //-> User password Incorrect
                else if (password != c.Password)
                {
                    int i = 2;
                    bool flag = true;

                    while (i > 0)
                    {
                        sc.WriteLine("Incorrect Password ! Attempts Left : " + i);
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
                                sc.WriteLine("Invalid Password ! Only  Digits are Allowed. ");
                            }
                        }

                        if (password == c.Password)
                        {
                            flag = false;
                            break;
                        }

                        i--;
                    }


                    //Encrypt Password Again
                    c.Password = cryptionPassword(c.Password.ToString());
                    if (!flag)
                    {
                        type = "cust";
                        msg = "Logged Out Successfully ";
                        return true;
                    }
                    else
                    {
                        //Update Status & Return 
                        c.Status = "Disabled";

                        if (Dal.UpdateAccount(c))
                        {
                            msg = "Your Account is Blocked ! Contact Admin";
                            return false;
                        }
                    }
                }

                //All Correct
                else
                {
                    //Encrypt Password Again
                    c.Password = cryptionPassword(c.Password.ToString());

                    type = "cust";
                    msg = "Logged Out Successfully ";
                    return true;
                }
            }

            msg = "Logged Out Successfully ";
            return true;
        }
    }
}
