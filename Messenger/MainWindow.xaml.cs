using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
//using System.Timers;
using System.Windows.Threading;
using System.IO;
namespace Messenger
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Person ActiveUser;
        Conversation ActiveConversation;
        DispatcherTimer dispatcherTimer;
        DatabaseConnection MyConnection;
        public MainWindow()
        {
            MyConnection = new DatabaseConnection();
            MyConnection.Open();

            InitializeComponent();

            Conversation.Initialize(MyConnection);
            Person.Initialize(MyConnection);
            NewsFeed.Initialize(MyConnection);
            Contact.Initialize(MyConnection);

            //Conversation.ResetTable();
            //Person.ResetTable();

            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval =new TimeSpan(0,0,5);
            dispatcherTimer.Tick += Update;
            LILogin.KeyDown += new KeyEventHandler(LoginTime_KeyDown);
            LIPassword.KeyDown += new KeyEventHandler(LoginTime_KeyDown);
            SULogin.KeyDown += new KeyEventHandler(SignupTime_KeyDown);
            SUName.KeyDown += new KeyEventHandler(SignupTime_KeyDown);
            SUPassword.KeyDown += new KeyEventHandler(SignupTime_KeyDown);
            NewConfessionTextBox.KeyDown += new KeyEventHandler(NewConfessionEvent);
            NewMessageTextBox.KeyDown += new KeyEventHandler(NewMessageEvent);
            NewContactTextBox.KeyDown += new KeyEventHandler(NewContactEvent);

            //string RunningPath = AppDomain.CurrentDomain.BaseDirectory;
            //string filename = string.Format("{0}Resources\\back3.jpg",System.IO.Path.GetFullPath(System.IO.Path.Combine(RunningPath, @"..\..\")));
            //MainViewImageBrush.ImageSource = new BitmapImage(new Uri(filename, UriKind.Relative)); 

            //BackgroundImageBrush.ImageSource = new BitmapImage(new Uri(filename, UriKind.Relative));
            this.WindowState = WindowState.Maximized;
            // #62D6FC
            BrushConverter BrushColorConverter = new BrushConverter();
            //#E5E4EA
            //#FAF1EC
            //LoginSignUpBorder.Background= BrushColorConverter.ConvertFromString("#FEFEFE") as SolidColorBrush;
            //MainBorder.Background = BrushColorConverter.ConvertFromString("#FEFEFE") as SolidColorBrush;
            MainBorder.Background = BrushColorConverter.ConvertFromString("#98B1C4") as SolidColorBrush;
            LoginSignUpBorder.Background= BrushColorConverter.ConvertFromString("#98B1C4") as SolidColorBrush;
        }

        private void NewMessageEvent(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;
            TextBox messageBox = sender as TextBox;
            if (messageBox.Text == "" || ActiveConversation==null) return;
            ActiveConversation.SendMessage(messageBox.Text.Replace("'","%%" ).Replace( "\"","%%%"));
            messageBox.Text = "";
            Update(null, null);
           
        }
        private void NewContactEvent(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;
            TextBox messageBox = sender as TextBox;
            if (messageBox.Text == "") return;
            Contact newContact = ActiveUser.AddNewContact(messageBox.Text);
            messageBox.Text = "";
            if (newContact == null) return;
            ActiveUser.ContactList.Add(newContact);
           
        }
        private void NewConfessionEvent(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;
            TextBox messageBox = sender as TextBox;
            if (messageBox.Text == "") return;
            NewsFeed.AddNewPost(messageBox.Text.Replace("'", "%%").Replace("\"", "%%%"));
            messageBox.Text = "";
        }
        private void LoginTime_KeyDown(object sender,KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                LIButton_Click(null, null);
            }
        }
        private void SignupTime_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;
            SUButton_Click(null, null);
        }
        private void LIButton_Click(object sender, RoutedEventArgs e) //Login
        {
            if (LILogin.Text == "" && LIPassword.Password == "") return;
            ActiveUser = Person.Login(LILogin.Text, LIPassword.Password);
            if (ActiveUser == null) return;
            LoginSignUpBorder.Visibility = Visibility.Collapsed;
            MainBorder.Visibility = Visibility.Visible;
            Update(null,null);
            dispatcherTimer.Start();
            LILogin.Text = "";
            LIPassword.Password = "";

        }

        private void SUButton_Click(object sender, RoutedEventArgs e) //Sign Up
        {
            if (SULogin.Text == "" || SUPassword.Password == "" || SUName.Text == "") return;
            Person.SignUp(SULogin.Text, SUPassword.Password, SUName.Text);
            SUName.Text = "";
            SULogin.Text = "";
            SUPassword.Password = "";
        }

        private void Update(object sender, EventArgs e)
        {
            int newContacts=MyConnection.RetrieveInt("People", "Contacts", "ID", ActiveUser.Id);
            if (newContacts != ActiveUser.Contacts)
            {
                List<int> addedContacts = Methods.GetPrimeFactors(newContacts / ActiveUser.Contacts);
                foreach (int id in addedContacts)
                {
                    ActiveUser.ContactList.Add(new Contact(id));
                }
            }
            BrushConverter BrushColorConverter = new BrushConverter();
            NewsFeed.Update();
            ActiveUser.LoadConversations();
            ContactsOfUser.Children.Clear();
            MessagesOfUser.Children.Clear();
            GlobalNewsFeed.Children.Clear();
            foreach (Contact contact in ActiveUser.ContactList) // Draw contact Button
            {
                ContactButton cbutton = new ContactButton(contact.Id);
                cbutton.Content =contact.Name+" ("+contact.Username+")";
                cbutton.Click += Cbutton_Click;
                ContactsOfUser.Children.Add(cbutton);
                if (ActiveConversation!=null && "C" + contact.Id * ActiveUser.Id == ActiveConversation.ColumnName)
                {
                    cbutton.Background = BrushColorConverter.ConvertFromString("#5DBCD2") as SolidColorBrush;
                }
            }
            foreach (string message in NewsFeed.Posts) //Draw newsfeed
            {
                Border border = new Border();
                
                TextBlock tb = new TextBlock();
                tb.Text = message.Replace("%%","'").Replace("%%%", "\"");
                border.CornerRadius = new CornerRadius(10);
                border.MaxWidth = 250;
                border.Padding = new Thickness(5, 0, 8, 0);
                border.Margin = new Thickness(0, 5, 0, 5);
                tb.FontSize = 13;
                tb.TextWrapping = TextWrapping.Wrap;
                tb.Foreground = Brushes.White;
                border.Child = tb;
                border.Background = BrushColorConverter.ConvertFromString("#0084FF") as SolidColorBrush;
                GlobalNewsFeed.Children.Add(border);
            }

            if (ActiveConversation == null) return;
            ActiveConversation.Update();
            foreach (string message in ActiveConversation.MessageList)
            {
                if (message == null) continue;
                Border border = new Border();
                TextBlock tb = new TextBlock();
                if ( message.Length>=ActiveUser.Name.Length+2 && message.Substring(0, ActiveUser.Name.Length + 2) == ActiveUser.Name + ": ")
                {
                    border.HorizontalAlignment = HorizontalAlignment.Right;
                    tb.TextAlignment = TextAlignment.Right;
                    //tb.HorizontalAlignment = HorizontalAlignment.Right;
                    
                    tb.Text = (message.Substring(ActiveUser.Name.Length + 1, message.Length - (ActiveUser.Name.Length + 1))).Replace("%%", "'").Replace("%%%", "\"");
                    border.Background = BrushColorConverter.ConvertFromString("#0084FF") as SolidColorBrush;
                    // Message By User
                }
                else
                {
                    border.Background = BrushColorConverter.ConvertFromString("red") as SolidColorBrush;
                    tb.Text = message.Replace("%%", "'").Replace("%%%", "\"");
                    border.HorizontalAlignment = HorizontalAlignment.Left;
                }
                border.Margin = new Thickness(0, 5, 0, 0);
                border.CornerRadius = new CornerRadius(10);
                border.MaxWidth = 200;
                border.Padding = new Thickness(5,0,8,0);
                tb.FontSize = 13;
                tb.TextWrapping = TextWrapping.Wrap;
                tb.Foreground = Brushes.White;
                border.Child = tb;
                MessagesOfUser.Children.Add(border);
                
            }

        }

        private void Cbutton_Click(object sender, RoutedEventArgs e)
        {
            ContactButton cbutton = sender as ContactButton;
            ActiveConversation = ActiveUser.GetConversationById(cbutton.ID);
            Update(null, null);
            //cbutton.Background = new BrushConverter().ConvertFromString("#5DBCD2") as SolidColorBrush;
            //cbutton.Background = (new BrushConverter()).ConvertFromString("orange") as SolidColorBrush;
            //throw new NotImplementedException();
        }
    }
}
