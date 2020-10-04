namespace Expressions.Tokens
{
    internal class FalseBoolConstantToken : IBooleanConstantToken
    {
        public static string Ident = "ЛОЖЬ";

        bool IBooleanConstantToken.Value => false;

        TokenTypeEnum IToken.Type => TokenTypeEnum.Constant;

        DataTypeEnum IConstantToken.DataType => DataTypeEnum.Boolean;

        string IBooleanConstantToken.Ident => Ident;

        string IToken.ToString()
        {
            return Ident;
        }

        public override string ToString()
        {
            return (this as IToken).ToString();
        }
    }
}
