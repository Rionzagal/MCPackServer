namespace MCPackServer.Models
{
    public enum Operators
    {
        Contains,
        Equal,
        NotEqual,
        StartsWith,
        EndsWith,
        GreaterThan,
        IGreaterThan,
        LesserThan,
        ILesserThan,
        Between
    }

    public enum Conditions
    {
        And,
        Or
    }

    public class WhereFilter
    {
        public WhereFilter()
        {

        }
        public string Field { get; set; } = string.Empty;
        public object? Value { get; set; } = string.Empty;
        public Operators Operator { get; set; } = Operators.Contains;
        public Conditions Condition { get; set; } = Conditions.And;
        public object? MinValue { get; set; }
        public object? MaxValue { get; set; }
    }
}
