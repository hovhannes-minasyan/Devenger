using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Sql;
using System.Data.SqlClient;
namespace Messenger
{
    public class DatabaseConnection
    {
        private bool isOpen;
        private SqlCommand myCommand;
        private SqlConnection myConnection = new SqlConnection("user id=artachok6969base;" +
                                       "password=Gm14x~~r49Bv;server=mssql6.gear.host;" +
                                       //"Trusted_Connection=yes;" +
                                       "database=artachok6969base; " +
                                       "connection timeout=30");
        /*
        public static MySqlConnection mycon = new MySqlConnection("user id=artachok6969;" +
                                       "password=qwerty99; server=artachok.000webhostapp.com;" +
                                       //"Trusted_Connection=yes;" +
                                       "database=webhostartachok; " +
                                       "connection timeout=30");
*/

        private SqlDataReader dataReader;
        public void Excecute()
        {
            if (this.isOpen)
            {
                try { myCommand.ExecuteNonQuery(); }
                catch (Exception e) { Console.WriteLine(e); }
            }
        }

        public void Open()
        {
            try
            {

                myConnection.Open();
                //MessageBox.Show("Opened");
                Console.WriteLine("Connection Opened");
                isOpen = true;
                myCommand = new SqlCommand("Command String", myConnection);
            }
            catch (Exception e)
            {
                //Console.WriteLine("It is open dumbass");
                isOpen = false;
                //MessageBox.Show(e.ToString());
                Console.WriteLine(e.ToString());
            }
        }
        public void CreateTable(string name)
        {
            myCommand.CommandText = "CREATE TABLE " + name + " (creator Bit);";
            Excecute();

            //        myCommand.CommandText = @"CREATE TABLE Persons (
            //PersonID int,
            //LastName varchar(255),
            //FirstName varchar(255),
            //Address varchar(255),
            //City varchar(255) ); ";

        }
        public void CreateTable(string table, string column, string type)
        {
            myCommand.CommandText = "CREATE TABLE " + table + " (" + column + " " + type + ");";
            Excecute();
        }



        public void AddColumn(string table, string columnName, string columnType)
        {
            myCommand.CommandText = "ALTER TABLE " + table + " ADD " + columnName + " " + columnType + ";";
            Excecute();
        }

        public void DeleteTable(string tableName)
        {
            myCommand.CommandText = "DROP TABLE " + tableName;
            Excecute();
        }


        public void Insert(string table, params string[] ColumnsValues)
        {

            StringBuilder str = new StringBuilder();
            str.Append("INSERT INTO " + table + " (");

            for (int a = 0; a < ColumnsValues.Length / 2 - 1; a++)
            {
                str.Append(ColumnsValues[a] + ", ");
            }
            str.Append(ColumnsValues[ColumnsValues.Length / 2 - 1] + ") VALUES (");

            for (int a = ColumnsValues.Length / 2; a < ColumnsValues.Length - 1; a++)
            {
                str.Append(ColumnsValues[a] + ", ");
            }
            str.Append(ColumnsValues[ColumnsValues.Length - 1] + "); ");

            //Console.WriteLine(str);
            myCommand = new SqlCommand(str.ToString(), myConnection);
            //myCommand.CommandText = str.ToString();
            Excecute();
        }


        public void DeleteColumn(string table, string column)
        {
            myCommand.CommandText = "ALTER TABLE " + table + "DROP COLUMN " + column + ";";
            Excecute();
        }

        public void Exp()
        {
            string x = "INSERT INTO People (creator,Name,ID) VALUES (1,2,1)";
            //Console.WriteLine(x);
            myCommand.CommandText = x;
            //Console.WriteLine(x);
            Excecute();
        }

        public void DeleteRow(string table, string column, string value, bool isString = false)
        {
            if (isString)
                myCommand.CommandText = "DELETE FROM " + table + " WHERE " + column + "='" + value + "';";
            else
                myCommand.CommandText = "DELETE FROM " + table + " WHERE " + column + "=" + value + ";";
            Excecute();
        }


        public void RetrieveInfo(string table, string column, ref List<string> output)
        {
            myCommand.CommandText = "SELECT " + column + " FROM " + table + ";";
            dataReader = myCommand.ExecuteReader();
            while (dataReader.Read())
            {
                output.Add(dataReader[0].ToString());
            }
            dataReader.Close();
        }
        public void RetrieveInfo(string table, string column, ref List<int> output)
        {

            myCommand.CommandText = "SELECT " + column + " FROM " + table + ";";

            dataReader = myCommand.ExecuteReader();

            while (dataReader.Read())
            {
                output.Add(Convert.ToInt32(dataReader[0]));
            }
            dataReader.Close();
        }

        public void RetrieveInfo(string table, string column, ref List<bool> output)
        {

            myCommand.CommandText = "SELECT " + column + " FROM " + table + ";";
            dataReader = myCommand.ExecuteReader();
            while (dataReader.Read())
            {
                output.Add(Convert.ToBoolean(dataReader[0]));
            }
            dataReader.Close();
        }

        public string RetrieveString(string table, string fromColumn, string conditionalColumn, string value)
        {
            string output = "";

            myCommand.CommandText = "SELECT " + fromColumn + " FROM " + table + " WHERE " + conditionalColumn + "='" + value + "';";
            dataReader = myCommand.ExecuteReader();
            if (dataReader.Read())
                output = dataReader[0].ToString();
            dataReader.Close();
            return output;
        }
        public int RetrieveInt(string table, string fromColumn, string conditionalColumn, string value)
        {
            int output = 0;
            myCommand.CommandText = "SELECT " + fromColumn + " FROM " + table + " WHERE " + conditionalColumn + "='" + value + "';";
            dataReader = myCommand.ExecuteReader();
            if (dataReader.Read())
                output = Convert.ToInt32(dataReader[0]);
            dataReader.Close();
            return output;
        }

        public int RetrieveInt(string table, string fromColumn, string conditionalColumn, int value)
        {
            int output = 0;
            myCommand.CommandText = "SELECT " + fromColumn + " FROM " + table + " WHERE " + conditionalColumn + "=" + value + ";";
            dataReader = myCommand.ExecuteReader();
            if (dataReader.Read())
                output = Convert.ToInt32(dataReader[0]);
            dataReader.Close();
            return output;
        }

        public int RetrieveLastInt(string table, string column)
        {
            int output = 0;

            myCommand.CommandText = "SELECT " + column + " FROM " + table + ";";
            dataReader = myCommand.ExecuteReader();
            while (dataReader.Read())
                output = Convert.ToInt32(dataReader[0]);
            dataReader.Close();
            return output;
        }

        public void ChangeString(string table, string column, string value, string conditinColumn, string conditionValue)
        {
            //insert into People (t3,t4) values ("vay" ,"qu");
            //myCommand.CommandText = "INSERT INTO " + table + "(" + column + ") VALUES ('" + value + "');";
            myCommand.CommandText = "UPDATE " + table + " SET " + column + "='" + value + "' WHERE " + conditinColumn + "='" + conditionValue + "';";
            Excecute();
        }

        public void ChangeString(string table, string column, string value, string conditinColumn, int conditionValue)
        {
            //insert into People (t3,t4) values ("vay" ,"qu");
            //myCommand.CommandText = "INSERT INTO " + table + "(" + column + ") VALUES ('" + value + "');";
            myCommand.CommandText = "UPDATE " + table + " SET " + column + "='" + value + "' WHERE " + conditinColumn + "=" + conditionValue + ";";
            Excecute();
        }

        public void ChangeInt(string table, string column, int value, string conditinColumn, int conditionValue)
        {
            myCommand.CommandText = "UPDATE " + table + " SET " + column + "='" + value + "' WHERE " + conditinColumn + "=" + conditionValue + ";";
            Excecute();
        }

        public void ChangeInt(string table, string column, int value, string conditinColumn, string conditionValue)
        {
            myCommand.CommandText = "UPDATE " + table + " SET " + column + "='" + value + "' WHERE " + conditinColumn + "='" + conditionValue + "';";
            Excecute();
        }
        public int GetItemsBeforeNull(string table, string column, string conditionColumn, string relation, int conditionValue, ref List<string> ListOfString, int startingPoint = 0)
        {
            int output = startingPoint;
            myCommand.CommandText = "SELECT " + column + " FROM " + table + " WHERE " + conditionColumn + relation + conditionValue + ";";
            dataReader = myCommand.ExecuteReader();
            while (dataReader.Read() && dataReader[0].ToString() != "")
            {
                ListOfString.Add(dataReader[0].ToString());
                output++;
            }
            dataReader.Close();
            return output;
        }


        public void RetrieveContact(string table, string conditionColumn, int ConditionValue, ref string username, ref string name)
        {
            myCommand.CommandText = "SELECT * FROM " + table + " WHERE " + conditionColumn + "=" + ConditionValue + ";";
            dataReader = myCommand.ExecuteReader();

            if (dataReader.Read())
            {

                username = dataReader["Username"].ToString();
                name = dataReader["Name"].ToString();

            }
            dataReader.Close();
        }

        public void RetrieveContact(string table, string conditionColumn, string ConditionValue, ref int id, ref string name)
        {
            myCommand.CommandText = "SELECT * FROM " + table + " WHERE " + conditionColumn + "='" + ConditionValue + "';";
            dataReader = myCommand.ExecuteReader();

            if (dataReader.Read())
            {
                name = (dataReader["Name"].ToString());
                id = Convert.ToInt32(dataReader["ID"]);
            }
            dataReader.Close();
        }


    }
}
