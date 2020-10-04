using Expressions.Tokens.Math;

namespace Expressions.Tokens
{
    internal class DecimalConstantToken : IDecimalConstantToken
    {
        private readonly decimal _value;

        decimal IDecimalConstantToken.Value => _value;

        TokenTypeEnum IToken.Type => TokenTypeEnum.Constant;

        DataTypeEnum IConstantToken.DataType => DataTypeEnum.Decimal;

        public DecimalConstantToken(decimal value)
        {
            _value = value;
        }

        string IToken.ToString()
        {
            return _value.ToString();
        }

        public override string ToString()
        {
            return (this as IToken).ToString();
        }
    }
}
