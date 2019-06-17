namespace Program
{
    public class Derivatives
    {
        public static Expression Get(Expression expr)
        {
            if (expr.Value == Constants.Ets)
            {
                if(expr.Prev != null) expr.Prev.Value = "λ";
                return expr;
            }
            if (expr.Prev == null)
            {
                expr.Prev = new Expression("De",0);
                return expr;
            }
            else
            {
                if (expr.Prev.Func == Enums.Func.Sqr)
                {
                    expr.Prev.Level = expr.Prev.Level > 1 ? expr.Level + 1 : 2;
                }
                else
                {
                    expr.Prev.Func = Enums.Func.Sqr;
                    expr.Prev.Level = 2;
                }

                return expr;
            }
        }
    }
}
