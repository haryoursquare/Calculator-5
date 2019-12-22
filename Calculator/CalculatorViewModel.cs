using System;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace Calculator
{
    public class CalculatorViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        public CalculatorViewModel()
        {
            DigitCommand = new Command<string>(
                execute: (string arg) =>
                {
                    ErrorMessage = "";
                    WriteInput(arg);

                }, canExecute: (string arg) =>
                {
                    return string.IsNullOrEmpty(ErrorMessage);
                });

            OperatorCommand = new Command<string>(
                execute: (string arg) =>
                {
                    ErrorMessage = "";
                    ProcessOperatorCommand(arg);
                });
        }

        private void WriteInput(string value)
        {
            if (string.IsNullOrEmpty(OperatorInput))
                TopInput += decimal.Parse(value);
            else
                BottomInput += decimal.Parse(value);
        }

        private void ProcessOperatorCommand(string value)
        {
            try
            {
                switch (value)
                {
                    case "Clear":
                        TopInput = "";
                        OperatorInput = "";
                        BottomInput = "";
                        Answer = 0;
                        ErrorMessage = "";
                        break;
                    case "Calculate":
                        if (!string.IsNullOrEmpty(OperatorInput))
                        {
                            switch (OperatorInput)
                            {
                                case "+":
                                    Answer = Math.Round(TopInputDecimal() + BottomInputDecimal(), 2);
                                    break;
                                case "-":
                                    Answer = Math.Round(TopInputDecimal() - BottomInputDecimal(), 2);
                                    break;
                                case "*":
                                    Answer = Math.Round(TopInputDecimal() * BottomInputDecimal(), 2);
                                    break;
                                case "/":
                                    Answer = Math.Round(TopInputDecimal() / BottomInputDecimal(), 2);
                                    break;
                            }
                        }
                        else
                        {
                            Answer = TopInputDecimal();
                        }

                        TopInput = Answer.ToString();

                        break;
                    case "+":
                    case "-":
                    case "*":
                    case "/":
                        BottomInput = "";
                        OperatorInput = value;
                        break;
                }
            }
            catch (ArithmeticException e)
            {
                ErrorMessage = e.Message;
            }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
            }
            
        }

        decimal answer = 0;
        public decimal Answer
        {
            private set
            {
                if (answer != value)
                {
                    answer = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Answer"));
                }
            }
            get
            {
                return answer;
            }
        }

        string topInput = "";
        public string TopInput
        {
            private set
            {
                if (topInput != value)
                {
                    topInput = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TopInput"));
                }
            } 
            get
            {
                return topInput;
            }
        }
        public decimal TopInputDecimal() =>  decimal.Parse(TopInput);

        string operatorInput = "";
        public string OperatorInput
        {
            private set
            {
                if (operatorInput != value)
                {
                    operatorInput = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("OperatorInput"));
                }
            }
            get
            {
                return operatorInput;
            }
        }

        string bottomInput = "";
        public string BottomInput
        {
            private set
            {
                if (bottomInput != value)
                {
                    bottomInput = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("BottomInput"));
                }
            }
            get
            {
                return bottomInput;
            }
        }
        public decimal BottomInputDecimal() => decimal.Parse(BottomInput);

        string errorMessage = "";
        public string ErrorMessage
        {
            private set
            {
                if (errorMessage != value)
                {
                    errorMessage = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ErrorMessage"));
                }
            }
            get
            {
                return errorMessage;
            }
        }

        public ICommand DigitCommand { private set; get; }
        public ICommand OperatorCommand { private set; get; }
    }
}