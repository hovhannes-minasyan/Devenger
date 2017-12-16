using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger
{
    public partial class Person // Main Methods
    {
        public Person(string _username, string _password, string _name, int _id, int contacts)
        {

            this.Name = _name;
            this.Id = _id;
            this.password = _password;
            this.Username = _username;
            this.Contacts = contacts;

        }


        private static bool IsNew(string username)
        {
            for (int a = 0; a < Usernames.Count; a++)
            {
                if (username == Usernames[a])
                { return false; }
            }
            return true;
        }


        public static Person Login(string _username, string _password)
        {
            if (MyDatabase.RetrieveString(table, "Password", "Username", _username) == _password)
            {

                string name = MyDatabase.RetrieveString(table, "Name", "Username", _username);
                int id = MyDatabase.RetrieveInt(table, "ID", "Username", _username);
                int contactProduct = MyDatabase.RetrieveInt(table, "Contacts", "Username", _username);

                Person User = new Person(_username, _password, name, id, contactProduct);

                List<int> CList = Methods.GetPrimeFactors(contactProduct);
                User.ContactList = new List<Contact>();
                foreach (int ContactId in CList) { User.ContactList.Add(new Contact(ContactId)); }
                User.LoadConversations();
                Console.WriteLine("Logging in as {0}", _username);
                return User;
            }
            Console.WriteLine("Login Failed");
            return null;
        }

        public static bool SignUp(string username, string pass, string Name)
        {
            Usernames = new List<string>();
            MyDatabase.RetrieveInfo(table, "Username", ref Usernames);
            if (!IsNew(username))
            {
                return false;
            }
            List<int> idList = new List<int>();
            MyDatabase.RetrieveInfo(table, "ID", ref idList);
            new Person(username, pass, Name, Methods.GenerateAvailablePrime(idList), 1).InstallIntoDatabase();

            return true;
        }


        public Contact AddNewContact(int id)
        {
            if (!Methods.IsPrime(id) || Contacts % id == 0) return null; // check for the number to be possible to be an ID
            IDs = new List<int>();
            MyDatabase.RetrieveInfo(table, "ID", ref IDs);
            if (!Methods.IntBelongsToArray(id, IDs.ToArray(), IDs.Count - 1)) return null;
            this.AddContactToDatabase(id);
            int oldContact2 = MyDatabase.RetrieveInt(table, "Contacts", "ID", id);
            MyDatabase.ChangeInt(table, "Contacts", oldContact2 * this.Id, "ID", id);
            Conversation.AddConversationToDatabase(id, this.Id);
            this.Contacts *= id;
            return new Contact(id);
        }

        public Contact AddNewContact(string username)
        {
            Usernames = new List<string>();
            MyDatabase.RetrieveInfo(table, "Username", ref Usernames);
            foreach (string uname in Usernames)
            {
                if (uname == username && this.Username != username && username != "admin")
                {
                    int id = MyDatabase.RetrieveInt(table, "ID", "Username", username);
                    if (this.Contacts % id == 0) return null;
                    MyDatabase.ChangeInt(table, "Contacts", this.Contacts * id, "Username", this.Username);
                    int contacts = MyDatabase.RetrieveInt(table, "Contacts", "ID", id);
                    MyDatabase.ChangeInt(table, "Contacts", contacts * this.Id, "ID", id);
                    Conversation.AddConversationToDatabase(id, this.Id);
                    this.Contacts *= id;
                    return new Contact(id);

                }
            }
            return null;
        }

        public void DeleteContact(int id)
        {
            if (this.Contacts % id == 0)
            {
                this.Contacts /= id;
                MyDatabase.DeleteColumn(table, "C" + id * this.Id);
                int oldContact1 = MyDatabase.RetrieveInt(table, "Contacts", "ID", this.Id);
                int oldContact2 = MyDatabase.RetrieveInt(table, "Contacts", "ID", id);
                MyDatabase.ChangeInt(table, "Contacts", oldContact1 / id, "ID", this.Id);
                MyDatabase.ChangeInt(table, "Contacts", oldContact2 / id, "ID", id);
            }

        }

        public void LoadConversations()
        {
            //int i = 0;
            for (int i = 0; i < ContactList.Count; i++)
            {
                if (ContactList[i]== null) { ContactList.RemoveAt(i); i--; }
            }
            Conversations = new List<Conversation>();
            foreach (Contact c in ContactList)
            {
                
                Conversations.Add(new Conversation(this, c.Id));
                //Conversations[i]=
            }
        }

        public override string ToString()
        {
            return Username + Id.ToString();
        }

        public Conversation GetConversationById(int id)
        {
            foreach (Conversation convo in this.Conversations)
            {
                if (convo.ColumnName == "C"+(this.Id* id)) return convo;
            }
            return null;
        }
       
    }
}
