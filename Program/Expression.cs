namespace Program
{
    public class Expression
    {
        public string Value { get; set; }
        public Enums.Func Func { get; set; }
        public string Number { get; set; }
        public Expression Prev { get; set; }
        public bool IsPlus { get; set; }
        public int Level { get; set; }
        public Expression(string value, int isPlus)
        {
            Func = Enums.Func.No;
            var index = value.ToLower().IndexOf('x');
            if (index >= 0)
            {
                Value = "X";
                Number = value.Substring(index + 1);
                var prev = value.Substring(0, index);
                if (!string.IsNullOrEmpty(prev)) Prev = new Expression(prev, 0);
            }
            else
            {
                Value = value;
            }

            IsPlus = (isPlus <= 0);
        }
    }
}
