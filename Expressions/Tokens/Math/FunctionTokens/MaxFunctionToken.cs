﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Expressions.Tokens.Math
{
    internal class MaxFunctionToken : IFunctionToken
    {
        public static string Name => "МАКС";

        private readonly List<string> _errors = new List<string>();

        TokenTypeEnum IToken.Type => TokenTypeEnum.Function;

        string IFunctionToken.Name => Name;

        List<string> IFunctionToken.Errors => _errors;

        string IToken.ToString()
        {
            return Name;
        }

        public override string ToString()
        {
            return (this as IToken).ToString();
        }

        IConstantToken IFunctionToken.Calc(List<IConstantToken> arguments)
        {
            if (arguments != null && arguments.Any() && !arguments.Any(a => a.DataType != DataTypeEnum.Decimal))
            {
                decimal result = arguments.Select(a => (a as IDecimalConstantToken).Value).Max();                
                return TokensRepository.GetDecimalConstantToken(result, false);
            }
            else
            {
                throw new Exception("В качестве параметров функции 'МАХ' ожидались значения типа 'decimal'.");
            }
        }
    }
}
