using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection.Emit;
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

namespace WpfApp2
{
    public partial class MainWindow : Window
    {
        enum Actions
        {
            Add,
            Sub,
            Mul,
            Div
        }

        double? operand = null;
        bool _eBClick = false;
        bool _aBClick = false;

        
        double? result = null;
        Actions action;
        bool equalButtonClick
        {
            get => _eBClick;
            set
            {
                _eBClick = value;
                if (value) actionButtonClick = false;
            }
        }
        bool actionButtonClick
        {
            get => _aBClick;
            set 
            {
                _aBClick = value;
                if (value) equalButtonClick = false;
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            Enter.Text = "0";
        }

        private void DigitButtonClick(object sender, RoutedEventArgs e)
        {
            if (equalButtonClick)
            {
                operand = null;
                result = null;
                equalButtonClick = false;
                Enter.Text = "0";
                History.Text = string.Empty;
            }
            else if (actionButtonClick)
            {
                operand = null;
                actionButtonClick = false;
                Enter.Text = "0";
            }

           
            switch ((sender as Button)?.Content)
            {
                case "<":
                    if (Enter.Text.Length <= 1) Enter.Text = "0";
                    else Enter.Text = Enter.Text[..^1];
                    break;
                case string enterChar:
                    if (enterChar == "," && Enter.Text.Contains(',')) break;
                    if (Enter.Text.Length == 1 && Enter.Text[0] == '0' && enterChar != ",") Enter.Text = enterChar;
                    else Enter.Text += enterChar;
                    break;
            }
        }

        private void ActionButton_Click(object sender, RoutedEventArgs e)
        {
            Button? button = sender as Button;
            string? actionString = button?.Content.ToString();
            if (result == null)
            {
                try { result = double.Parse(Enter.Text); }
                catch 
                {
                    Enter.Text = "0";
                    result = 0;
                }
                History.AppendText(result.ToString());
                History.AppendText(actionString);

            }
            else if (operand == null)
            {
                operand = double.Parse(Enter.Text);
                if (!Carculate(action)) return;
                Enter.Text = result.ToString();
                if (History.Text[^1] == '=') History.Text = History.Text[..^1];
                else History.AppendText(operand.ToString());
                History.AppendText(actionString);
            }
            else History.Text = History.Text[..^1] + actionString;


            actionButtonClick = true;
            action = Enum.Parse<Actions>(button.Tag.ToString());

        }
        

        private void EqualButtonClick(object sender, RoutedEventArgs e)
        {
            if (actionButtonClick) operand = null;
            if (result == null) return;
            operand ??= double.Parse(Enter.Text);
            if(!Carculate(action)) return;
            History.AppendText(operand.ToString());
            History.AppendText("=");
            Enter.Text = result.ToString();
            equalButtonClick = true;
        }

       

        private void CCEButtonClick(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).Content.ToString() == "C")  Clear();
            else 
            { }
        }

        private bool Carculate(Actions actions)
        {
            switch(actions)
            {
                case Actions.Add: result += operand; break;
                case Actions.Div:

                    if (operand == 0)
                    {
                        Clear();
                        Enter.Text = "Can not divide by zero";
                        return false;
                    }
                    else result /= operand;
                    break;
                case Actions.Mul: result *= operand; break;
                case Actions.Sub: result -= operand; break;
            }
            return true;
        }

        private void Clear()
        {
            operand = null;
            result = null;
            equalButtonClick = true;
            Enter.Text = "0";
            History.Text = string.Empty;
            actionButtonClick = false;
        }
    }
}
