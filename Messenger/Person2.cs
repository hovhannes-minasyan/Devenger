using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger
{
    partial class Person    // Database and Properties  
    {
        // private static Random rand = new Random();
        private static string table = "People";
        private static List<string> Usernames = new List<string>();
        private static DatabaseConnection MyDatabase = new DatabaseConnection();
        private static List<int> IDs;

        public string Name { get; set; }

        private string password;
        public int Id { get; set; }
        public string Username { get; set; }
        public int Contacts;
        public List<Contact> ContactList;

        public List<Conversation> Conversations;
        private string sqlUsername { get { return "'" + Username + "'"; } }
        private string sqlPassword { get { return "'" + password + "'"; } }
        private string sqlName { get { return "'" + Name + "'"; } }

        

        public static void ResetTable()
        {
            MyDatabase.DeleteTable("People");
            MyDatabase.CreateTable("People");
            MyDatabase.AddColumn("People", "Username", "varchar(255)");
            MyDatabase.AddColumn("People", "Password", "varchar(255)");
            MyDatabase.AddColumn("People", "Name", "varchar(255)");
            MyDatabase.AddColumn("People", "ID", "int");
            MyDatabase.AddColumn("People", "Contacts", "int");
            SignUp("admin", "qwerty", "admin");
        }


        private void AddContactToDatabase(int ContactId)
        {

            if (this.Contacts % ContactId != 0)
            {
                this.Contacts *= ContactId;
                this.ContactList.Add(new Contact(ContactId));
                MyDatabase.ChangeInt(table, "Contacts", Contacts, "Username", Username);
            }
        }



        private void InstallIntoDatabase()
        {
            //                 table     (true)   
            MyDatabase.Insert("People", "creator", "Username", "Password", "Name", "ID", "Contacts",
                "1", sqlUsername, sqlPassword, sqlName, Id.ToString(), "1");
        }

        public static void Initialize(DatabaseConnection conntection)
        {
            MyDatabase=conntection;
            //MyDatabase.RetrieveInfo("People", "Username", ref Usernames);
        }






    }

}
