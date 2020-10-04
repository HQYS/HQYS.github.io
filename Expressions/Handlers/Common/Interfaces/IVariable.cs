namespace Expressions
{
    public interface IVariable
    {
        string Ident { get; }

        DataTypeEnum DataType { get; }
       
    }
}
