using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Program
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(Formula.Text)) return;
            Viewer.Children.Clear();
            var formula = Formula.Text;
            var parsedFormula = new Formula(formula);

            var parsedFormula2 = new Formula();
            foreach (var expression in parsedFormula.FirstLineLeft)
            {
                var derivatedExpression = Derivatives.Get(expression);
                parsedFormula2.FirstLineLeft.Add(derivatedExpression);
            }
            ShowText(parsedFormula2, "Туындысын табамыз");

            var parsedFormula3 = new Formula ();
            foreach (var expression in parsedFormula2.FirstLineLeft)
            {
                var sameExpression = parsedFormula.SecondLineLeft
                    .FirstOrDefault(x =>
                        ((x.Prev != null && expression.Prev != null && x.Prev.Func == expression.Prev.Func && x.Prev.Value == expression.Prev.Value && x.Prev.Func == expression.Prev.Func) ||
                         (x.Prev == null && expression.Prev == null))
                        && x.Func == expression.Func && x.Value == expression.Value && x.Number == expression.Number);
                if (sameExpression != null)
                {
                    parsedFormula3.SecondLineLeft.Add(sameExpression);
                    parsedFormula.SecondLineLeft.Remove(sameExpression);
                    var nextExpression = parsedFormula.SecondLineLeft.First();
                    nextExpression.IsPlus = !nextExpression.IsPlus;
                    parsedFormula3.FirstLineLeft.Add(nextExpression);
                    parsedFormula3.SecondLineLeft.Add(nextExpression);
                }
                else
                {
                    parsedFormula3.FirstLineLeft.Add(expression);
                }
            }
            ShowText(parsedFormula3, "Екінші жолдан");

            var parsedFormula4 = new Formula{FirstLineLeft = parsedFormula3.FirstLineLeft};
            foreach (var expression in parsedFormula4.FirstLineLeft)
            {
                if (expression.Value.ToLower() == "x") expression.Value = "X~";
            }
            ShowText(parsedFormula4, "X1 = X~");

            var parsedFormula5 = new Formula { FirstLineLeft = parsedFormula4.FirstLineLeft };
            foreach (var expression in parsedFormula5.FirstLineLeft)
            {
                if (expression.Value.ToLower() == "x~")
                {
                    expression.Value = Constants.Ets;
                    expression.Number = string.Empty;
                }
            }
            ShowText(parsedFormula5, $"X~ = {Constants.Ets}");

            var parsedFormula6 = new Formula();
            foreach (var expression in parsedFormula5.FirstLineLeft)
            {
                var derivatedExpression = Derivatives.Get(expression);
                parsedFormula6.FirstLineLeft.Add(derivatedExpression);
            }
            ShowText(parsedFormula6, $"({Constants.Ets})' = L{Constants.Ets}");

            var parsedFormula7 = new Formula();
            foreach (var expression in parsedFormula6.FirstLineLeft)
            {
                expression.Value = expression.Prev != null ? string.Empty : "1";
                parsedFormula7.FirstLineLeft.Add(expression);
            }
            ShowText(parsedFormula7, string.Empty);

            var parsedFormula8 = new Formula();
            foreach (var expression in parsedFormula7.FirstLineLeft)
            {
                if (expression.Prev == null)
                {
                    expression.IsPlus = !expression.IsPlus;
                    _right = expression;
                }
                else
                {
                    _left = expression;
                }
                parsedFormula8.SecondLineLeft.Add(expression);
            }
            ShowText(parsedFormula8, string.Empty);
            Result();
        }

        private Expression _left;
        private Expression _right;
        private void Result()
        {
            var right = Convert.ToInt32(_right.Value);
            right *= _right.IsPlus ? 1 : -1;
            if (_left.Prev?.Func == Enums.Func.Sqr)
            {
                var result = Math.Sqrt(right);
            }
        }

        private void ShowText(Formula formula, string title)
        {
            var text = new TextBlock{Text = title, Background = new SolidColorBrush(Colors.White), Padding = new Thickness(20,0,0,10)};
            text.SetValue(TextBlock.FontWeightProperty, FontWeights.Bold);
            Viewer.Children.Add(text);

            var allText = new StringBuilder();
            if (formula.FirstLineLeft.Any())
            {
                var text1 = new StringBuilder();
                foreach (var expression in formula.FirstLineLeft)
                {
                    var value = $"{expression.Value}{expression.Number}";
                    if (expression.Func == Enums.Func.Sqr) value = $"{value}^{expression.Level}";
                    if (expression.Prev != null)
                    {
                        if (expression.Prev.Func == Enums.Func.Sqr)
                            value = $"{expression.Prev.Value}^{expression.Prev.Level}{value}";
                        else value = $"{expression.Prev.Value}{value}";
                    }

                    if (!string.IsNullOrEmpty(text1.ToString()))
                    {
                        value = expression.IsPlus ? $"+{value}" : $"-{value}";
                    }
                    else
                    {
                        value = expression.IsPlus ? $"{value}" : $"-{value}";
                    }

                    text1.Append(value);
                }

                text1.Append("=0");
                allText.AppendLine(text1.ToString());
            }

            if (formula.SecondLineLeft.Any())
            {
                var text2 = new StringBuilder();
                foreach (var expression in formula.SecondLineLeft)
                {
                    var value = $"{expression.Value}{expression.Number}";
                    if (expression.Func == Enums.Func.Sqr) value = $"{value}^{expression.Level}";
                    if (expression.Prev != null)
                    {
                        if (expression.Prev.Func == Enums.Func.Sqr)
                            value = $"{expression.Prev.Value}^{expression.Prev.Level}{value}";
                        else value = $"{expression.Prev.Value}{value}";
                    }

                    if (!string.IsNullOrEmpty(text2.ToString()))
                    {
                        value = expression.IsPlus ? $"={value}" : $"=-{value}";
                    }
                    else
                    {
                        value = expression.IsPlus ? $"{value}" : $"-{value}";
                    }

                    text2.Append(value);
                }

                allText.AppendLine(text2.ToString());
            }

            Viewer.Children.Add(new TextBlock{Text = allText.ToString(), Background = new SolidColorBrush(Colors.White), Padding = new Thickness(20, 0, 0, 0) });
        }
    }
}
