using System;
using System.Collections;
using System.Globalization;
using System.IO;
using Customer = BO.Customer;

namespace DAL
{
    public class Db
    {

        //Return Customer Object Based on Login ID 
        public static Customer ReadById(string id)
        {
            try
            {
                FileStream fs = new FileStream("record.csv", FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(fs);

                sr.BaseStream.Seek(0, SeekOrigin.Begin);
                string str = sr.ReadLine();
                while (str != null && str != "")
                {
                    string[] data = str.Split(',');
                    if (id == data[1])
                    {
                        sr.Close();
                        fs.Close();


                        return new Customer(data[1], Convert.ToInt32(data[2]), data[3], data[4], float.Parse(data[5]), data[6], Convert.ToInt32(data[0]));

                    }
                    str = sr.ReadLine();
                }
                sr.Close();
                fs.Close();
            }
            catch (Exception)
            {

                Console.WriteLine("Sorry There is an error in connecting to Server !  Try Again\n\n");
                //Environment.Exit(404);
            }

            return null;
        }


        public static ArrayList searchResults(ArrayList d, ArrayList index)
        {
            ArrayList list = new ArrayList();

            try
            {
                FileStream fs = new FileStream("record.csv", FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(fs);

                sr.BaseStream.Seek(0, SeekOrigin.Begin);
                string str = sr.ReadLine();
                while (str != null && str != "")
                {
                    string[] data = str.Split(',');

                    //Iterate over Array and check if all constraints are met
                    bool flag = false;
                    for (int i = 0; i < d.Count; i++)
                    {
                        if (data[(int)index[i]] == d[i].ToString())
                        {
                            flag = true;
                            continue;
                        }
                        flag = false;
                        break;
                    }
                    
                    //Flag will be true iff all constraints are full filled 
                    if (flag)
                    {
                        Customer n = new Customer(data[1], Convert.ToInt32(data[2]), data[3], data[4], float.Parse(data[5]), data[6], Convert.ToInt32(data[0]));
                        list.Add(n);
                    }

                    str = sr.ReadLine();
                }

                sr.Close();
                fs.Close();


            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Sorry There is an error in connecting to Server !  Try Again\n\n");
            }

            return list;
        }

        public static float getTransactionSum(string id, string date)
        {
            float sum = 0;
            try
            {
                FileStream fs = new FileStream("TransactionRecord.csv", FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(fs);

                sr.BaseStream.Seek(0, SeekOrigin.Begin);
                string str = sr.ReadLine();
                while (str != null && str != "")
                {
                    string[] data = str.Split(',');

                    //If Login Id of customer is equal
                    if (data[0] == "Cash Withdrawal"  && data[1] == id)
                    {
                        //Parsing string date from file to Actual Date and same for the argument passed Date to Compare
                        int t = DateTime.Compare(DateTime.Parse(data[4]), DateTime.Parse(date));

                        //If Transaction date lies in between then proceed 
                        if (t == 0)
                            sum += float.Parse(data[3]);

                    }

                    str = sr.ReadLine();
                }
                sr.Close();
                fs.Close();

            }
            catch (FileNotFoundException)
            {

                Console.WriteLine("Sorry There is an error in connecting to Server !  Try Again\n\n");
                //Environment.Exit(404);
            }

            return sum;
        }

        public static ArrayList getCustomerTransactions(Customer r, DateTime s, DateTime e)
        {
            ArrayList AL = new ArrayList();
            CultureInfo provider = CultureInfo.InvariantCulture;

            try
            {
                FileStream fs = new FileStream("TransactionRecord.csv", FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(fs);

                sr.BaseStream.Seek(0, SeekOrigin.Begin);
                string str = sr.ReadLine();
                while (str != null && str != "")
                {
                    string[] data = str.Split(',');

                    //If Login Id of customer is equal
                    if(data[1] == r.Id) 
                    {
                        //Parsing string date from file to Actual Date and same for the argument passed Date to Compare
                        int t1 = DateTime.Compare(DateTime.Parse(data[4]), DateTime.Parse(s.ToString("dd/MM/yyyy")));
                        int t2 = DateTime.Compare(DateTime.Parse(data[4]), DateTime.Parse(e.ToString("dd/MM/yyyy")));
                        
                        //If Transaction date lies in between then proceed 
                        if (t1 > 0 && t2 < 0)
                            AL.Add(data);

                    }

                    str = sr.ReadLine();
                }
                sr.Close();
                fs.Close();

                return AL;
            }
            catch (FileNotFoundException)
            {

                Console.WriteLine("Sorry There is an error in connecting to Server !  Try Again\n\n");
                //Environment.Exit(404);
            }

            return null;
        }

        public static ArrayList getAllUsers()
        {
            ArrayList AL = new ArrayList();
            try
            {
                FileStream fs = new FileStream("record.csv", FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(fs);

                sr.BaseStream.Seek(0, SeekOrigin.Begin);
                string str = sr.ReadLine();
                while (str != null && str != "")
                {
                    string[] data = str.Split(',');

                    AL.Add(new Customer(data[1], Convert.ToInt32(data[2]), data[3], data[4], float.Parse(data[5]), data[6], Convert.ToInt32(data[0])));

                    str = sr.ReadLine();
                }
                sr.Close();
                fs.Close();
            }
            catch (FileNotFoundException)
            {

                Console.WriteLine("Sorry There is an error in connecting to Server !  Try Again\n\n");
                //Environment.Exit(404);
            }

            return AL;
        }

        //Delete Account 
        public static bool Delete(int accountno)
        {
            try
            {
                //Opening Orignal file & Creating a copy to save records

                FileStream fs = new FileStream("record.csv", FileMode.Open, FileAccess.Read);
                FileStream fsn = new FileStream("record_copy.csv", FileMode.Append, FileAccess.Write);

                StreamReader sr = new StreamReader(fs);
                StreamWriter srn = new StreamWriter(fsn);

                sr.BaseStream.Seek(0, SeekOrigin.Begin);
                string str = sr.ReadLine();
                while (str != null && str != "")
                {
                    string[] data = str.Split(',');

                    //If Acc is equal to one to delete, simply skip writing to copy file
                    if (accountno == Convert.ToInt32(data[0]))
                    {
                        str = sr.ReadLine();
                        continue;
                    }

                    srn.WriteLine(data[0] + "," + data[1] + "," + data[2] + "," + data[3] + "," + data[4] + "," + data[5] + "," + data[6]);
                    str = sr.ReadLine();
                }

                sr.Close();
                fs.Close();

                srn.Close();
                fsn.Close();

                //Remove Orignal file and rename copy to orignal 
                File.Delete("record.csv");
                File.Move("record_copy.csv", "record.csv");


            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Sorry There is an error in connecting to Server !  Try Again\n\n");
            }


            return true;
        }

        //Return Customer Object Based on Account Number
        public static Customer ReadByAcc(int acc)
        {
            try
            {
                FileStream fs = new FileStream("record.csv", FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(fs);

                sr.BaseStream.Seek(0, SeekOrigin.Begin);
                string str = sr.ReadLine();
                while (str != null && str != "")
                {
                    string[] data = str.Split(',');
                    if (acc == Convert.ToInt32(data[0]))
                    {
                        sr.Close();
                        fs.Close();


                        return new Customer(data[1], Convert.ToInt32(data[2]), data[3], data[4], float.Parse(data[5]), data[6], Convert.ToInt32(data[0]));

                    }
                    str = sr.ReadLine();
                }

                sr.Close();
                fs.Close();
            }
            catch (FileNotFoundException)
            {
                //Console.WriteLine("Sorry There is an error in connecting to Server !  Try Again\n\n");
                //Environment.Exit(404);
            }

            return null;
        }


        //Insert New Record in the dataBase
        public static bool Insert(Customer c)
        {
            try
            {
                FileStream fs = new FileStream("record.csv", FileMode.Append, FileAccess.Write);
                StreamWriter sr = new StreamWriter(fs);

                sr.WriteLine(c.Accountno + "," + c.Id + "," + c.Password + "," + c.Name + "," + c.Type + "," + c.Balance + "," + c.Status);

                sr.Close();
                fs.Close();

                return true;
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Sorry There is an error in connecting to Server !  Try Again\n\n");
                //Environment.Exit(404);
            }

            return false;
        }

        //Get Last Account Number to insert new Record
        public static int GetLastAcc()
        {
            try
            {
                FileStream fs = new FileStream("record.csv", FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(fs);

                sr.BaseStream.Seek(0, SeekOrigin.Begin);
                string str = sr.ReadLine();

                //If This is the first user to get Registered. i.e file is empty
                if (str == null)
                {
                    fs.Close();
                    sr.Close();

                    return 0;
                }

                string[] data = null;
                while (str != null && str != "")
                {
                    data = str.Split(',');
                    str = sr.ReadLine();
                }

                sr.Close();
                fs.Close();


                return Convert.ToInt32(data[0]);
            }
            catch (FileNotFoundException)
            {
                //Console.WriteLine("Sorry There is an error in connecting to Server !  Try Again\n\n");
                //Environment.Exit(404);
            }

            return 0;
        }


        //Update Account Details of a customer 
        public static bool UpdateAccount(Customer c)
        {

            try
            {
                //Opening Orignal file & Creating a copy to save records

                FileStream fs = new FileStream("record.csv", FileMode.Open, FileAccess.Read);
                FileStream fsn = new FileStream("record_copy.csv", FileMode.Append, FileAccess.Write);

                StreamReader sr = new StreamReader(fs);
                StreamWriter srn = new StreamWriter(fsn);

                sr.BaseStream.Seek(0, SeekOrigin.Begin);
                string str = sr.ReadLine();
                while (str != null && str != "")
                {
                    string[] data = str.Split(',');

                    //If Acc is equal to one to Update, simply update values
                    if (c.Accountno == Convert.ToInt32(data[0]))
                    {
                        data[1] = c.Id;
                        data[2] = c.Password.ToString();
                        data[3] = c.Name;
                        data[5] = c.Balance.ToString();
                        data[6] = c.Status;
                    }

                    srn.WriteLine(data[0] + "," + data[1] + "," + data[2] + "," + data[3] + "," + data[4] + "," + data[5] + "," + data[6]);
                    str = sr.ReadLine();
                }

                sr.Close();
                fs.Close();

                srn.Close();
                fsn.Close();

                //Remove Orignal file and rename copy to orignal 

                File.Delete("record.csv");
                File.Move("record_copy.csv", "record.csv");

                return true;

            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Sorry There is an error in connecting to Server !  Try Again\n\n");
            }

            return false;
        }


        //To make record of transactions
        public static bool saveTransaction(string type, Customer c, float amount, string date)
        {
            try
            {
                FileStream fs = new FileStream("TransactionRecord.csv", FileMode.Append, FileAccess.Write);
                StreamWriter sr = new StreamWriter(fs);

                sr.WriteLine(type + "," + c.Id + "," + c.Name + "," + amount + "," + date.Replace('-','/'));

                sr.Close();
                fs.Close();

                return true;
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Sorry There is an error in connecting to Server !  Try Again\n\n");
                //Environment.Exit(404);
            }

            return false;
        }

    }
}
