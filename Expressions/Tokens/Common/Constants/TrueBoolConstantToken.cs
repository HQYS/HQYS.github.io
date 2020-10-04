namespace Expressions.Tokens
{
    internal class TrueBoolConstantToken : IBooleanConstantToken
    {
        public static string Ident = "ИСТИНА";

        bool IBooleanConstantToken.Value => true;

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
