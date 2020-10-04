namespace Expressions.Tokens
{
    internal class VariableToken : IVariableToken
    {
        private readonly string _ident;
        
        TokenTypeEnum IToken.Type => TokenTypeEnum.Variable;
        
        string IVariableToken.Ident => _ident;

        public VariableToken(string ident)
        {
            _ident = ident;
        }

        string IToken.ToString()
        {
            return _ident;
        }

        public override string ToString()
        {
            return (this as IToken).ToString();
        }

    }
}
