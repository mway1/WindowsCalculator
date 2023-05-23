using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace WindowsCalculator.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            foreach(UIElement element in Main.Children)
            {
                if(element is Button)
                {
                    ((Button)element).Click += Button_Click;
                }
            }
        }

        private void Button_Click(object sender,RoutedEventArgs e)
        {
            string str = (string)((Button)e.OriginalSource).Content;

            if(str  == "C")
            {
                TB_Text.Text = ""; 
            }
            else if(str == "--")
            {
                TB_Text.Text = TB_Text.Text.Substring(0,TB_Text.Text.Length - 1);
            }
            else if(str == "=")
            {
                string value = new DataTable().Compute(TB_Text.Text, null).ToString();
                TB_Text.Text = value;
            }
            else TB_Text.Text += str;
        }
    }
}
