using System;
using System.Collections.Generic;

namespace Program
{
    public class Formula
    {
        public Formula(string formula)
        {
            var firstLine = formula.Substring(0, formula.IndexOf("\r\n", StringComparison.Ordinal));
            var firstLineLeft = GetLeftSide(firstLine);
            FirstLineLeft = GetExpressionsRecursive(firstLineLeft);

            var secondLine = formula.Substring(formula.IndexOf("\r\n", StringComparison.Ordinal) + 2);
            var secondLineLeft = GetLeftSide(secondLine);
            SecondLineLeft = GetExpressionsRecursive(secondLineLeft);
        }

        public Formula()
        {
            FirstLineLeft = new List<Expression>();
            SecondLineLeft = new List<Expression>();
        }

        private static List<Expression> GetExpressionsRecursive(string line)
        {
            var array = new[] {'+','-'};
            var list = new List<Expression>();
            var index = 0;
            var newIndex = line.IndexOfAny(array);
            var expr = line.Substring(index, newIndex);
            while (!string.IsNullOrEmpty(expr))
            {
                list.Add(new Expression(expr, Array.IndexOf(array, line[index])));
                index = newIndex;
                newIndex = line.IndexOfAny(array, index + 1);
                expr = (newIndex >= 0) ? line.Substring(index + 1, newIndex): null;
            }

            list.Add(new Expression(line.Substring(index + 1), Array.IndexOf(array, line[index])));
            return list;
        }

        private static string GetLeftSide(string line)
        {
            return line.Substring(0, line.IndexOf('='));
        }

        public List<Expression> FirstLineLeft { get; set; }
        public string FirstLineRight { get; set; }
        public List<Expression> SecondLineLeft { get; set; }
        public string SecondLineRight { get; set; }
    }
    
}
