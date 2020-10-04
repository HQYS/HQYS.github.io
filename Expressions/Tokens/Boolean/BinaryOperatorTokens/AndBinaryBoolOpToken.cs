using System;

namespace Expressions.Tokens.Boolean
{
    internal class AndBinaryBoolOpToken : IBinaryBoolOpToken
    {
        public static string Ident => "AND";

        string IBinaryOperatorToken.Ident => Ident;

        TokenTypeEnum IToken.Type => TokenTypeEnum.BinaryOperator;

        BinaryBoolOpTypeEnum IBinaryBoolOpToken.OperationType => BinaryBoolOpTypeEnum.And;

        byte IBinaryOperatorToken.Priority => 2;

        string IToken.ToString()
        {
            return Ident.ToString();
        }

        public override string ToString()
        {
            return (this as IToken).ToString();
        }

        IConstantToken IBinaryOperatorToken.Calc(IConstantToken a, IConstantToken b)
        {
            if (a.DataType != DataTypeEnum.Boolean || b.DataType != DataTypeEnum.Boolean)
            {
                throw new Exception("Для операции логического 'И'ожидались параметры типа 'boolean'.");
            }
            bool result = (a as IBooleanConstantToken).Value && (b as IBooleanConstantToken).Value;
            return TokensRepository.GetBooleanConstantToken(result);
        }

    }
}
