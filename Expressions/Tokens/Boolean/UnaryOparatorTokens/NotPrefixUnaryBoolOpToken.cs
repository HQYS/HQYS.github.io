using System;

namespace Expressions.Tokens.Math
{
    internal class NotPrefixUnaryBoolOpToken : IUnaryOperatorToken
    {
        public static string Ident => "NOT";

        TokenTypeEnum IToken.Type => TokenTypeEnum.UnaryOperator;

        byte IUnaryOperatorToken.Priority => 5;


        string IToken.ToString()
        {
            return Ident;
        }

        public override string ToString()
        {
            return (this as IToken).ToString();
        }

        IConstantToken IUnaryOperatorToken.Calc(IConstantToken a)
        {
            if (a.DataType != DataTypeEnum.Boolean)
            {
                throw new Exception("Для операции логичесикого префиксного унарного отрицания ожидался параметр типа 'boolean'.");
            }
            bool result = !(a as IBooleanConstantToken).Value;
            return TokensRepository.GetBooleanConstantToken(result);
        }
    }
}
