namespace Expressions.Tokens
{
    internal interface IVariableToken : IToken
    {
        string Ident { get; }
    }
}
