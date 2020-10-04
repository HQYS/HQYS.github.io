namespace Expressions.Tokens
{
    internal interface IBooleanConstantToken : IConstantToken
    {
        string Ident { get; }

        bool Value { get; }
    }
}
