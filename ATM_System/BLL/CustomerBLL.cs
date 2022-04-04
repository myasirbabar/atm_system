using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using Dal = DAL.Db;

namespace BLL
{
    public class CustomerBLL
    {
        
        public static Customer Withdraw(Customer c, float amount)
        {
            //Update Account Balance
            c.Balance = c.Balance - amount;
            if (Dal.UpdateAccount(c))
            {
                DateTime dt = DateTime.Now;

                //Save Transaction in DataBase
                if(Dal.saveTransaction("Cash Withdrawal", c, amount, dt.ToString("dd/MM/yyyy"))) { return c; }
            }

            return null;
        }

        public static Customer Deposit(Customer c, float amount)
        {
            //Update Account Balance
            c.Balance = c.Balance + amount;
            if (Dal.UpdateAccount(c))
            {
                DateTime dt = DateTime.Now;

                //Save Transaction in DataBase
                if (Dal.saveTransaction("Deposit", c, amount, dt.ToString("dd/MM/yyyy"))) { return c; }
            }

            return null;
        }

        public static Customer Transfer(Customer c, Customer r, float amount)
        {
            //Update Balance of both sender and receiver 
            c.Balance = c.Balance - amount;
            r.Balance = r.Balance + amount;

            if (Dal.UpdateAccount(c) && Dal.UpdateAccount(r))
            {
                DateTime dt = DateTime.Now;

                //Save Transaction in DataBase
                if (Dal.saveTransaction("Transferred", c, amount, dt.ToString("dd/MM/yyyy")) && Dal.saveTransaction("Received", r, amount, dt.ToString("dd/MM/yyyy"))) { 
                    return c; 
                }
            }

            return null;
        }

        public static bool exceedsLimit(Customer c, string date, float amount)
        {
            float oldSum = Dal.getTransactionSum(c.Id, date);

            if (oldSum + amount > 20000)
                return true;

            return false;
        }
    }
}
