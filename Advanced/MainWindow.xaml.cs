using System;
using System.Windows;
using System.Windows.Controls;
using System.Data;
using System.Windows.Input;

namespace Calculator
{
    public partial class MainWindow : Window
    {
        //stores input from user
        private string input = "";

        public MainWindow()
        {
            InitializeComponent();
        }

        //handles button clicks for numbers & decimal point
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                string value = btn.Content.ToString();

                // Prevent multiple decimals in a single number
                if (value == ".")
                {
                    //splits input by operators
                    string[] numbers = input.Split(new char[] { '+', '-', '*', '/' });

                    //ensure last number does not already hav a decimal point
                    if (!numbers[numbers.Length - 1].Contains("."))
                    {
                        //adds decimal to input
                        input += value;
                    }
                }
                else
                {
                    //adds number to input
                    input += value;
                }

                //updates display
                Display.Text = input;
            }
        }

        //handles operation button clicks
        private void Operator_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                string op = btn.Content.ToString();

                // Allow negative numbers as the first input
                if (string.IsNullOrEmpty(input) && op == "-")
                {
                    
                    input += op;
                }

                //otherwise only add operator if there is a number entered
                if (!string.IsNullOrEmpty(input))
                {
                    input += op;
                }

                //update display
                Display.Text = input;
            }
        }

        //handles = button click & does the math
        private void Equals_Click(object sender, RoutedEventArgs e)
        {
            // If there is no input, return
            if (string.IsNullOrEmpty(input))
            {
                return;
            }

            // Check for dividing by zero
            if (input.Contains("/0"))
            {
                Display.Text = "Error";
                input = "";
                return;
            }

            try
            {
                // evaluate the expression 
                var result = new DataTable().Compute(input, null);
                string resultStr = result.ToString();

                // Handle invalid results
                if (resultStr == "∞" || resultStr == "Infinity" || resultStr == "-∞" || resultStr == "NaN")
                {
                    Display.Text = "Error";
                    input = "";
                }
                else
                {
                    // Update display and store result
                    Display.Text = resultStr;
                    input = resultStr;
                }
            }
            catch
            {
                // Catch any other errors in evaluation and reset input
                Display.Text = "Error";
                input = "";
            }
        }

        //handles clear button
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            input = "";
            Display.Text = "";
        }

        // Handles keyboard input for number entry and operations (NUMPAD ONLY)
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) //numpad numbers
            {
                input += (e.Key - Key.NumPad0).ToString(); //converts to number
            }

            //removes last entered character
            else if (e.Key == Key.Back && input.Length > 0) // Backspace
            {
                input = input.Substring(0, input.Length - 1);
            }

            else if (e.Key == Key.Subtract) // "-" 
            {
                if (string.IsNullOrEmpty(input))
                {
                    // Allow a negative sign as the first character
                    input += "-";
                }
                else
                {
                    input += "-";
                }
            }

            //handles enter key by calling equals_click
            else if (e.Key == Key.Enter) // Enter/Return key
            {
                Equals_Click(this, null);
                return;
            }

            else if (e.Key == Key.C) // Handle 'C' key to clear input
            {
                Clear_Click(this, null);
                return;
            }


            //don't allow any other operators if input is cirrently empty
            else if (string.IsNullOrEmpty(input))
            {
                return;
            }

            //operators
            else if (e.Key == Key.Add) input += "+"; // "+"
            else if (e.Key == Key.Multiply) input += "*"; // "*"
            else if (e.Key == Key.Divide) input += "/"; // "/" 
            


            //decimal input
            else if (e.Key == Key.Decimal) // Numpad "."
            {
                //prevent multiple decimals in one number
                string[] numbers = input.Split(new char[] { '+', '-', '*', '/' });
                if (!numbers[numbers.Length - 1].Contains("."))
                {
                    input += ".";
                }
            }


            
            //update display
            Display.Text = input;
        }
    }
}
