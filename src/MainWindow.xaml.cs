using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;

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
    }
}
