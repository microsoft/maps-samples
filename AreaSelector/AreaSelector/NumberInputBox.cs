using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace AreaSelector
{
    public class NumberInputBox : TextBox
    {
        public enum InputType
        {
            Integer = 0,
            IntegerPositive = 1,
            Float = 2,
            FloatPositive = 3,
        }

        public InputType Type { get; set; }
        public int MaxValue { get; set; }
        public int MinValue { get; set; }
        public bool ValueOk { get; set; }

        public NumberInputBox()
            : base()
        {
            TextChanged += TextChangedEvent;
            this.InputScope = new InputScope()
            {
                Names = { new InputScopeName() { NameValue = InputScopeNameValue.Number } }
            };

            this.Type = NumberInputBox.InputType.Integer;
            this.MaxValue = this.MinValue = 0;
            ValueOk = false;
        }

        String ParseDouble(string input)
        {
            if (input.Length > 0)
            {
                String outBut = "";
                bool foundSeparator = false;

                for (int i = 0; i < input.Length; i++)
                {
                    if ((this.Type == NumberInputBox.InputType.Integer
                       || this.Type == NumberInputBox.InputType.Float)
                    &&((i == 0) && input[0] == '-')){
                        outBut = outBut + input[i];
                    }
                    else if ((input[i] >= '0' && input[i] <= '9'))
                    {
                        outBut = outBut + input[i];
                    }
                    else if (this.Type == NumberInputBox.InputType.Float
                           || this.Type == NumberInputBox.InputType.FloatPositive)
                    {
                        if (foundSeparator == false)
                       {
                            string Sepa = CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator;
                            if (Sepa.Length > 0)
                            {
                                if (Sepa[0] == input[i] || '.' == input[i] || ',' == input[i])
                                {
                                    outBut = outBut + Sepa[0];
                                    foundSeparator = true;
                                }
                            }
                        }
                    }
                }

                return outBut;
            }
            else
            {
                return "";
            }
        }

        private void TextChangedEvent(object sender, TextChangedEventArgs e)
        {
            this.Text = ParseDouble(this.Text);
            this.SelectionStart = this.Text.Length;

            if(this.MaxValue != this.MinValue)
            {
                double d;
                if (double.TryParse(this.Text, out d))
                {
                    if (d < this.MinValue || d > this.MaxValue)
                    {
                        this.ValueOk = false;
                        this.Foreground = new SolidColorBrush(Colors.Red);
                    }
                    else
                    {
                        this.ValueOk = true;
                        this.Foreground = new SolidColorBrush(Colors.Black);
                    }
                }
            }
        }
    }
}