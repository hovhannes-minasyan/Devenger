using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Messenger
{
    class ContactButton: System.Windows.Controls.Button
    {
        public readonly int ID;
        public ContactButton(int id)
        {
            ID = id;
            this.Background = Brushes.Transparent;
            BorderThickness = new System.Windows.Thickness(0);
        }
        
    }
}
