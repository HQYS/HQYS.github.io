namespace Expressions
{
    public interface IDecimalVariable : IVariable
    {
        decimal Value { get; set; }
    }
}
