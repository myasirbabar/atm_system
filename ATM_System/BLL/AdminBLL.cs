using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using Dal = DAL.Db;

namespace BLL
{
    public class AdminBLL
    {
        public static int AddUser(Customer c)
        {
            //Encrypting ID to check if user exist in DataBase
            c.Id = MainBLL.cryptionLogin(c.Id);

            //Check if user Already Exists 
            if (Dal.ReadById(c.Id) != null)
            {
                return -1;
            }

            //Get the last Account no and insert Record in DataBase
            c.Accountno = Dal.GetLastAcc() + 1;

            //Encrypting Password to Insert Record ! Login Already Encrypted
            c.Password = MainBLL.cryptionPassword(c.Password.ToString());

            if (Dal.Insert(c))
            {
                return c.Accountno;
            }

            return -1;

        }

        //Check if User Exists in DataBase or Not
        public static Customer Exists(int acc)
        {
            return Dal.ReadByAcc(acc);
        }

        //Delete User From DataBase
        public static bool Delete(Customer c)
        {
            return Dal.Delete(c.Accountno);
        }

        //Update Details Of user
        public static bool UpdateUser(Customer c, bool enL,bool enP)
        {
            //Encrypting Login and Password to update 
            if(enP)
                c.Password = MainBLL.cryptionPassword(c.Password.ToString());
            if(enL)
                c.Id = MainBLL.cryptionLogin(c.Id);

            return Dal.UpdateAccount(c);
        }

        //Search 
        public static ArrayList searchResults(Customer c)
        {
            ArrayList index = new ArrayList();//To get index of search criteria values.
            ArrayList data = new ArrayList(); // To get actual data on which search will be done.

            //Filling index and Data Arrays 
            if(c.Accountno != -1)
            {
                index.Add(0);
                data.Add(c.Accountno);
            }

            if (c.Id != "")
            {
                c.Id = MainBLL.cryptionLogin(c.Id);
                index.Add(1);
                data.Add(c.Id);
            }

            if (c.Name != "")
            {
                index.Add(3);
                data.Add(c.Name);
            }

            if (c.Type != "")
            {
                index.Add(4);
                data.Add(c.Type);
            }

            if (c.Balance != -1)
            {
                index.Add(5);
                data.Add(c.Balance);
            }

            if (c.Status != "")
            {
                index.Add(6);
                data.Add(c.Status);
            }

            ArrayList temp = Dal.searchResults(data,index);
            ArrayList temp2 = new ArrayList();

            // Decrypting the login so to display Correctly
            foreach(Customer n in temp)
            {
                n.Id = MainBLL.cryptionLogin(n.Id);
                temp2.Add(n);
            }

            return temp2;
        }


        public static ArrayList getReportsByAmount(float min, float max)
        {
            ArrayList all = Dal.getAllUsers();//Return all users
            ArrayList res = new ArrayList();

            //Check which user meet criteria and then add to Array
            foreach(Customer c in all)
            {
                if(c.Balance >= min && c.Balance <= max)
                {
                    //Decrypting Id to Show to User

                    c.Id = MainBLL.cryptionLogin(c.Id);
                    res.Add(c);
                }
            }

            return res;
        }

        public static ArrayList getReportsByDate(Customer r,DateTime dtS, DateTime dtE)
        {
            ArrayList temp = Dal.getCustomerTransactions(r,dtS, dtE);
            ArrayList res = new ArrayList();

            foreach(string[] d in temp)
            {
                //Decrypting Id to display correctly 
                d[1] = MainBLL.cryptionLogin(d[1]);
                res.Add(d);
            }

            return res;
        }
    }
}
