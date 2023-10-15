using System;
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

namespace Caltucalor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    internal class CalculateString
    {
        List<string> _expressionList;

        public CalculateString(List<string> expressionList)
        {
            try
            {
                if (expressionList == null)
                    throw new ArgumentNullException();

                _expressionList = expressionList;
            }
            catch(ArgumentNullException ex)
            {
                throw ex;
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        public CalculateString(string expressionList)
        {
            try
            {
                if (expressionList == null)
                    throw new ArgumentNullException();

                _expressionList = expressionList.Split(' ').ToList();
            }
            catch (ArgumentNullException ex)
            {
                throw ex;
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        private bool IsDigitStr(string number)
        {
            foreach(char c in number)
            {
                if (!char.IsDigit(c))
                    return false;
            }
            return true;
        }

        private double CalculateSubExpression(string operation, double number1, double number2)
        {
            try
            {
                switch (operation)
                {
                    case "+":
                        return number1 + number2;
                    case "-":
                        return number1 - number2;
                    case "*":
                        return number1 * number2;
                    case "/":
                        return number1 / number2;
                }
            }
            catch(DivideByZeroException ex) { throw ex; }

            return 0.0;
        }

        private void CalculateAndReplaceMultDev(List<string> expressionListBuffer)
        {
            double temp = 0.0;
            try
            {
                for (int i = 1; i < expressionListBuffer.Count - 1; i += 2)
                {
                    if (expressionListBuffer[i] == "*" || expressionListBuffer[i] == "/")
                    {
                        temp = CalculateSubExpression(expressionListBuffer[i],
                            Convert.ToDouble(expressionListBuffer[i - 1]), Convert.ToDouble(expressionListBuffer[i + 1]));

                        expressionListBuffer[i - 1] = " ";
                        expressionListBuffer[i] = temp.ToString();
                        expressionListBuffer[i + 1] = " ";
                    }
                }
                expressionListBuffer.RemoveAll(x => x == " ");

            }
            catch(DivideByZeroException ex)
            {
                throw ex;
            }
        }

        private double CalculateAddSubst(List<string> expressionListBuffer)
        {
            double temp = 0.0;

            if (expressionListBuffer.Count == 1)
                return Convert.ToDouble(expressionListBuffer[0]); // If in expression line we have only one number

            for(int i = 1; i < expressionListBuffer.Count - 1; i += 2)
            {
                temp = CalculateSubExpression(expressionListBuffer[i],
                        Convert.ToDouble(expressionListBuffer[i - 1]), Convert.ToDouble(expressionListBuffer[i + 1]));

                expressionListBuffer[i - 1] = " ";
                expressionListBuffer[i] = " ";
                expressionListBuffer[i + 1] = temp.ToString();
            }

            expressionListBuffer.Clear();
            return temp;
        }

        public double Calculate()
        {
            List<string> expressionListBuffer = _expressionList;
            try
            {
                expressionListBuffer.RemoveAll(x => x == "");

                if (!IsDigitStr(expressionListBuffer.Last())) // If expression line has view: "1 + 2 + 3 +"
                    expressionListBuffer.RemoveAt(expressionListBuffer.Count-1);

                if (expressionListBuffer.Contains("*") || expressionListBuffer.Contains("/"))
                    CalculateAndReplaceMultDev(expressionListBuffer);
            }
            catch(DivideByZeroException ex)
            {
                throw ex;
            }

            return CalculateAddSubst(expressionListBuffer);
        }
    }



    public partial class MainWindow : Window
    {
        private static List<string> Numbers = new List<string> { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
        private CalculateString calculate;
        private string _proccess = "";
        private string _result = "";

        public MainWindow()
        {
            InitializeComponent();
        }

        private bool CheckUpdate(string literal)
        {
            if (_proccess == "" && !Numbers.Contains(literal))
                return false;

            switch (literal) {
                case ".":
                    return Numbers.Contains(_proccess[_proccess.Length - 1].ToString()) ? true : false;
                case " - ":
                case " / ":
                case " * ":
                case " + ":
                    return Numbers.Contains(_proccess[_proccess.Length - 1].ToString()) ? true : false;
            }

            return true;
        }

        private void AddProccessString(string literal)
        {
            if (CheckUpdate(literal))
            {
                _proccess += literal;
                Proccess.Text = _proccess;
            }
        }

        private void RemoveProccessString()
        {
            if (_proccess.Length > 0)
            {
                if (_proccess[_proccess.Length - 1].ToString() == " ")
                    _proccess = _proccess.Remove(_proccess.Length - 3);
                else
                    _proccess = _proccess.Remove(_proccess.Length - 1);

                Proccess.Text = _proccess;
            }
        }

        private void Clear(bool all)
        {
            if (all)
                _result = _proccess = Proccess.Text = Result.Text = "";
            else
                _result = Result.Text = "";
        }

        private void Calculate()
        {
            try
            {
                calculate = new CalculateString(_proccess);
                Result.Text = calculate.Calculate().ToString();
            }
            catch(DivideByZeroException)
            {
                Result.Text = "ERROR (Divide by zero)";
            }
        }


        private void Button_Click_Num0(object sender, RoutedEventArgs e) => AddProccessString("0");
        private void Button_Click_Num1(object sender, RoutedEventArgs e) => AddProccessString("1");
        private void Button_Click_Num2(object sender, RoutedEventArgs e) => AddProccessString("2");
        private void Button_Click_Num3(object sender, RoutedEventArgs e) => AddProccessString("3");
        private void Button_Click_Num4(object sender, RoutedEventArgs e) => AddProccessString("4");
        private void Button_Click_Num5(object sender, RoutedEventArgs e) => AddProccessString("5");
        private void Button_Click_Num6(object sender, RoutedEventArgs e) => AddProccessString("6");
        private void Button_Click_Num7(object sender, RoutedEventArgs e) => AddProccessString("7");
        private void Button_Click_Num8(object sender, RoutedEventArgs e) => AddProccessString("8");
        private void Button_Click_Num9(object sender, RoutedEventArgs e) => AddProccessString("9");

        private void Button_Click_Point(object sender, RoutedEventArgs e) => AddProccessString(".");

        private void Button_Click_Devide(object sender, RoutedEventArgs e) => AddProccessString(" / ");
        private void Button_Click_Mult(object sender, RoutedEventArgs e) => AddProccessString(" * ");
        private void Button_Click_Subtract(object sender, RoutedEventArgs e) => AddProccessString(" - ");
        private void Button_Click_Add(object sender, RoutedEventArgs e) => AddProccessString(" + ");


        private void Button_Click_CE(object sender, RoutedEventArgs e) => Clear(false);
        private void Button_Click_C(object sender, RoutedEventArgs e) => Clear(true);
        private void Button_Click_Less(object sender, RoutedEventArgs e) => RemoveProccessString();
        private void Button_Click_Equals(object sender, RoutedEventArgs e) => Calculate();
    }
}
