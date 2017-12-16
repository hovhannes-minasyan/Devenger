using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger
{
    public class NewsFeed
    {
        static string table = "Conversations";
        static string ColumnName = "NewsFeed";
        public static List<string> Posts = new List<string>();
        private static int LastMessageId = 1;
        private static DatabaseConnection MyDatabase;
        public static void Update()
        {
            LastMessageId += MyDatabase.GetItemsBeforeNull(table, ColumnName, "Count", ">=", LastMessageId, ref Posts);
        }
        public static void Initialize(DatabaseConnection conntection)
        {

            MyDatabase= conntection;
            //MyDatabase.AddColumn(table, ColumnName, "varchar(255)");
            Update();
        }

        public static void AddNewPost(string message)
        {
            Update();
            MyDatabase.ChangeString(table, ColumnName,message,"Count",LastMessageId);
            Console.WriteLine("Post made at row {0}",LastMessageId);
            Update();
        }

    }
}
