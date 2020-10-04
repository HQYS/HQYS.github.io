namespace Expressions.Tokens
{
    internal interface IConstantToken : IToken
    {
        DataTypeEnum DataType { get; }
    }
}
