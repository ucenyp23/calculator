using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace calculator
{
    public partial class MainWindow : Window
    {
        private StringBuilder currentInput = new StringBuilder("0");
        private double? previousValue = null;
        private string currentOperation = string.Empty;
        private bool operationPerformed = false;

        public MainWindow()
        {
            InitializeComponent();
            Number.Content = currentInput.ToString();
        }

        private void UpdateDisplay()
        {
            Number.Content = currentInput.ToString();
        }

        private void ClearEntry()
        {
            currentInput.Clear();
            currentInput.Append("0");
            UpdateDisplay();
        }

        private void ClearAll()
        {
            currentInput.Clear();
            previousValue = null;
            currentOperation = string.Empty;
            currentInput.Append("0");
            operationPerformed = false;
            UpdateDisplay();
        }

        private void ClickNumber(object sender, RoutedEventArgs e)
        {
            if (operationPerformed)
            {
                currentInput.Clear();
                operationPerformed = false;
            }

            Button button = sender as Button;
            if (currentInput.ToString() == "0")
            {
                currentInput.Clear();
            }
            currentInput.Append(button.Content);
            UpdateDisplay();
        }

        private void ClickDecimal(object sender, RoutedEventArgs e)
        {
            if (operationPerformed)
            {
                currentInput.Clear();
                operationPerformed = false;
            }

            if (!currentInput.ToString().Contains("."))
            {
                currentInput.Append(".");
            }
            UpdateDisplay();
        }

        private void ClickSignToggle(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(currentInput.ToString(), out double number))
            {
                number = -number;
                currentInput.Clear();
                currentInput.Append(number.ToString());
                UpdateDisplay();
            }
        }

        private void ClickPercent(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(currentInput.ToString(), out double number))
            {
                number = number / 100;
                currentInput.Clear();
                currentInput.Append(number.ToString());
                UpdateDisplay();
            }
        }

        private void ClickBackspace(object sender, RoutedEventArgs e)
        {
            if (currentInput.Length > 1)
            {
                currentInput.Length -= 1;
            }
            else
            {
                currentInput.Clear();
                currentInput.Append("0");
            }
            UpdateDisplay();
        }

        private void ClickReciprocal(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(currentInput.ToString(), out double number) && number != 0)
            {
                number = 1 / number;
                currentInput.Clear();
                currentInput.Append(number.ToString());
                UpdateDisplay();
            }
        }

        private void ClickSquare(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(currentInput.ToString(), out double number))
            {
                number = Math.Pow(number, 2);
                currentInput.Clear();
                currentInput.Append(number.ToString());
                UpdateDisplay();
            }
        }

        private void ClickSquareRoot(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(currentInput.ToString(), out double number) && number >= 0)
            {
                number = Math.Sqrt(number);
                currentInput.Clear();
                currentInput.Append(number.ToString());
                UpdateDisplay();
            }
        }

        private void ClickOperation(object sender, RoutedEventArgs e)
        {
            if (operationPerformed)
            {
                currentInput.Clear();
                operationPerformed = false;
            }

            Button button = sender as Button;
            if (previousValue.HasValue)
            {
                PerformCalculation();
            }
            currentOperation = button.Content.ToString();
            if (double.TryParse(currentInput.ToString(), out double number))
            {
                previousValue = number;
                currentInput.Clear();
            }
        }

        private void PerformCalculation()
        {
            if (double.TryParse(currentInput.ToString(), out double number) && previousValue.HasValue)
            {
                switch (currentOperation)
                {
                    case "+":
                        previousValue = previousValue.Value + number;
                        break;
                    case "-":
                        previousValue = previousValue.Value - number;
                        break;
                    case "*":
                        previousValue = previousValue.Value * number;
                        break;
                    case "/":
                        if (number != 0)
                        {
                            previousValue = previousValue.Value / number;
                        }
                        else
                        {
                            MessageBox.Show("Cannot divide by zero.");
                            ClearAll();
                            return;
                        }
                        break;
                }
                currentInput.Clear();
                currentInput.Append(previousValue.Value.ToString());
                UpdateDisplay();
                currentOperation = string.Empty;
                previousValue = null;
                operationPerformed = true;
            }
        }

        private void ClickEqual(object sender, RoutedEventArgs e)
        {
            PerformCalculation();
            operationPerformed = true;
        }

        private void ClickClear(object sender, RoutedEventArgs e)
        {
            ClearAll();
        }

        private void ClickClearEntry(object sender, RoutedEventArgs e)
        {
            ClearEntry();
        }

        // Keyboard input handling
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            // Handle different key inputs
            if (e.Key == Key.Escape)
            {
                // Handle escape key (Clear All)
                ClickClear(null, null);
            }
            else if (e.Key == Key.Enter || e.Key == Key.Return)
            {
                // Handle Enter/Return key (Equals)
                ClickEqual(null, null);
            }
            else if (e.Key == Key.Back)
            {
                // Handle backspace key
                ClickBackspace(null, null);
            }
            else if (e.Key == Key.Decimal || e.Key == Key.OemPeriod)
            {
                // Handle decimal key
                ClickDecimal(null, null);
            }
            else
            {
                // Handle number and operation keys
                HandleNumberOrOperationKeys(e.Key);
            }
        }

        private void HandleNumberOrOperationKeys(Key key)
        {
            string keyString = key.ToString();

            // Handle number keys (main keyboard numbers and keypad numbers)
            if (key >= Key.D0 && key <= Key.D9)
            {
                // Handle number keys (0-9 on main keyboard)
                ClickNumber(new Button { Content = (key - Key.D0).ToString() }, null);
            }
            else if (key >= Key.NumPad0 && key <= Key.NumPad9)
            {
                // Handle number keys on the numeric keypad
                ClickNumber(new Button { Content = (key - Key.NumPad0).ToString() }, null);
            }
            // Handle operation keys
            else if ("+-*/".Contains(key.ToString()))
            {
                ClickOperation(new Button { Content = key.ToString() }, null);
            }
        }
    }
}
