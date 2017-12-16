using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger
{
  
    public class Conversation
    {
        private static string table = "Conversations";
        private static DatabaseConnection MyDatabase = new DatabaseConnection();


        public readonly string ColumnName;
        private string Name;
        public List<string> MessageList = new List<string>();
        public int LastMessageId { get; set; }
        public Conversation(Person user, Person reciever)
        {
            LastMessageId = 1;
            ColumnName = "C" + (user.Id * reciever.Id);

            MyDatabase.RetrieveInfo(table, ColumnName, ref MessageList);
            for (int a = 0; a < MessageList.Count;)
            {
                if (MessageList[a] == null || MessageList[a] == "")
                {
                    MessageList.RemoveAt(a);
                    continue;
                }
                a++;
            }
            this.LastMessageId = MessageList.Count+1;
            Name = user.Name;
        }

        public Conversation(Person user, int recieverId)
        {
            LastMessageId = 1;
            ColumnName = "C" + (user.Id * recieverId);
            MyDatabase.RetrieveInfo(table, ColumnName, ref MessageList);
            for (int a = 0; a < MessageList.Count;)
            {
                if (MessageList[a] == null || MessageList[a] == "")
                {
                    MessageList.RemoveAt(a);
                    //Console.WriteLine("Item deleted at 'a' = {0} ", a);
                    continue;
                }
                a++;
            }
            this.LastMessageId = MessageList.Count+1;
            Name = user.Name;
        }

        public static void ResetTable()
        {
            MyDatabase.DeleteTable(table);
            //MyDatabase.CreateTable(table, "C1", "varchar(255)");
            MyDatabase.CreateTable(table, "Count", "int");
            for (int a = 1; a < 50; a++)
            {
                MyDatabase.Insert(table, "Count", a.ToString());
            }
            MyDatabase.AddColumn(table, "NewsFeed", "varchar(255)");
        }

        public void ModifyMessages()
        {
            Console.WriteLine("Last Message Id was {0}", LastMessageId);
            LastMessageId += MyDatabase.GetItemsBeforeNull(table, ColumnName, "Count", ">=", LastMessageId, ref MessageList);
            Console.WriteLine("Message modified");
            Console.WriteLine("Last Message Id is {0}",LastMessageId);

        }

        public static void AddConversationToDatabase(int id1, int id2)
        {
            MyDatabase.AddColumn(table, "C" + (id1 * id2), "varchar(255)");
            Console.WriteLine("Conversation {0} was added to DB", "C" + (id1 * id2));
        }


        public void SendMessage(string message)
        {
            ModifyMessages();
            MyDatabase.ChangeString(table, ColumnName, Name + ": " + message, "Count", LastMessageId);
            Console.WriteLine(message);
            ModifyMessages();
        }

        public static void Initialize(DatabaseConnection conntection)
        {
            MyDatabase=conntection;
        }

        public override string ToString()
        {
            StringBuilder str = new StringBuilder();
            foreach (string message in MessageList)
            {
                str.Append(message + "\n");
            }
            return str.ToString();
        }

        public void Update()
        {
            MessageList.Clear();
            MyDatabase.RetrieveInfo(table, ColumnName, ref MessageList);
            MessageList.RemoveAll(item => item == "");
            LastMessageId = MessageList.Count+1;
        }

    }
}
