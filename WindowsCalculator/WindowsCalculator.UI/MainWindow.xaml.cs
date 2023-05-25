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
using System.Threading;

namespace WindowsCalculator.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Queue<string> QueueRequests = new Queue<string>();
        private Queue<string> QueueResults = new Queue<string>();
        private int calculationTime = 0;

        public MainWindow()
        {
            InitializeComponent();
            foreach (UIElement element in Main.Children)
            {
                if (element is Button)
                {
                    ((Button)element).Click += Button_Click;
                }
            }
        }
        public void AddRequest(string request)
        {
            QueueRequests.Enqueue(request);
        }

        public void SetCalculationTime(string setTime)
        {
            if (int.TryParse(setTime, out calculationTime))
            {
                 
            }
            else
            {
                MessageBox.Show("Неверный формат числа!");
            }
        }

        public void StartProcessing()
        {
            Thread thread = new Thread(ProcessRequests);
            thread.Start();
        }


        private void ProcessRequests()
        {
            string result = "";
            while (true)
            {
                if (QueueRequests.Count > 0)
                {
                    string request = QueueRequests.Dequeue();

                    result = PerformCalculation(request);
                    QueueResults.Enqueue(result);
                    Dispatcher.Invoke(() =>
                    {
                        LB_Request.Items.Add("QueueRequestSize: " + QueueRequests.Count);
                        LB_Request.Items.Add("Result of calc: " + result);
                        
                    });
                }

                Thread.Sleep(calculationTime * 1000);
            }
        }

        private string PerformCalculation(string request)
        {
            try
            {
            string result = new DataTable().Compute(request, null).ToString();
            
                return result;

            }
            catch (SyntaxErrorException)
            {
                return "Ошибка в синтаксисе выражения!";
            }
            catch (DivideByZeroException)
            {
                return "Деление на ноль!";
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string str = (string)((Button)e.OriginalSource).Content;
            List<string> requsts = new List<string>();

            try
            {
                if (str == "C")
                {
                    TB_Text_ResultsOfCalc.Text = "";
                }
                else if (str == "--")
                {
                    TB_Text_ResultsOfCalc.Text = TB_Text_ResultsOfCalc.Text.Substring(0, TB_Text_ResultsOfCalc.Text.Length - 1);
                }
                else if (str == "AddRequest")
                {
                    if (TB_Text_ResultsOfCalc.Text.Length > 0)
                    {
                        if (TB_caclTime.Text.Length > 0)
                        {
                            AddRequest(TB_Text_ResultsOfCalc.Text);
                            SetCalculationTime(TB_caclTime.Text);
                            TB_Text_ResultsOfCalc.Text = "";
                            TB_caclTime.Text = "";

                        }else MessageBox.Show("Укажите время выполнения вычисления");
                    }
                    else MessageBox.Show("Вы не ввели пример");
                }
                else if (str == "StartCalc")
                {
                    StartProcessing();
                }
                else TB_Text_ResultsOfCalc.Text += str;
            }
            catch (Exception)
            {
                MessageBox.Show("На ноль делить нельзя!");
            }
        }
        }
}

