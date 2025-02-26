﻿using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace calculator
{
    public partial class MainWindow : Window
    {
        private readonly StringBuilder currentInput = new("0");
        private double? previousValue = null;
        private double? newValue = null;
        private string? currentOperation = null;
        private bool operationPerformed = false;

        public MainWindow()
        {
            InitializeComponent();
            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            // Update the display, ensuring negative zero is converted to zero
            if (currentInput.ToString() == "-0")
            {
                currentInput.Clear();
                currentInput.Append("0");
            }
            DisplayLabel.Content = currentInput.ToString();
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
            currentOperation = null;
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

            if (!currentInput.ToString().Contains(','))
            {
                currentInput.Append(',');
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
                number /= 100;
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
            else
            {
                MessageBox.Show("Cannot calculate the reciprocal of zero.");
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
            else
            {
                MessageBox.Show("Cannot calculate the square root of a negative number.");
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
                        newValue = previousValue.Value + number;
                        break;
                    case "-":
                        newValue = previousValue.Value - number;
                        break;
                    case "*":
                        newValue = previousValue.Value * number;
                        break;
                    case "/":
                        if (number != 0)
                        {
                            newValue = previousValue.Value / number;
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
                currentInput.Append(newValue.Value.ToString());
                UpdateDisplay();
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
            if (e.Key == Key.Escape)
            {
                ClickClear(null, null);
            }
            else if (e.Key == Key.Enter || e.Key == Key.Return)
            {
                ClickEqual(null, null);
            }
            else if (e.Key == Key.Back)
            {
                ClickBackspace(null, null);
            }
            else if (e.Key == Key.Decimal || e.Key == Key.OemPeriod)
            {
                ClickDecimal(null, null);
            }
            else
            {
                HandleNumberOrOperationKeys(e.Key);
            }
        }

        private void HandleNumberOrOperationKeys(Key key)
        {
            if (key >= Key.D0 && key <= Key.D9)
            {
                ClickNumberDirect((key - Key.D0).ToString());
            }
            else if (key >= Key.NumPad0 && key <= Key.NumPad9)
            {
                ClickNumberDirect((key - Key.NumPad0).ToString());
            }
            else if (key == Key.Add || key == Key.OemPlus)
            {
                ClickOperationDirect("+");
            }
            else if (key == Key.Subtract || key == Key.OemMinus)
            {
                ClickOperationDirect("-");
            }
            else if (key == Key.Multiply)
            {
                ClickOperationDirect("*");
            }
            else if (key == Key.Divide)
            {
                ClickOperationDirect("/");
            }
            else if (key == Key.Decimal || key == Key.OemPeriod)
            {
                ClickDecimal(null, null);
            }
        }

        private void ClickNumberDirect(string number)
        {
            if (operationPerformed)
            {
                currentInput.Clear();
                operationPerformed = false;
            }

            if (currentInput.ToString() == "0")
            {
                currentInput.Clear();
            }

            currentInput.Append(number);
            UpdateDisplay();
        }

        private void ClickOperationDirect(string operation)
        {
            currentOperation = operation;

            if (double.TryParse(currentInput.ToString(), out double number))
            {
                previousValue = number;
                currentInput.Clear();
            }
        }
    }
}