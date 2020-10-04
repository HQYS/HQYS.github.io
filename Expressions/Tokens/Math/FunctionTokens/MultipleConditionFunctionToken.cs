using System;
using System.Collections.Generic;
using System.Linq;

namespace Expressions.Tokens.Math
{
    internal class MultipleConditionFunctionToken : IFunctionToken
    {
        public static string Name => "ЕСЛИ";

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

            if (arguments == null || !arguments.Any())
            {
                throw new Exception($"Не заданы параметры функции '{Name}'.");
            }

            if (arguments.Count < 3 || arguments.Count % 2 == 0)
            {
                throw new Exception($"Количество параметров функции '{Name}' должно быть нечетным и больше или равно 3.");
            }

            int cnt = arguments.Count / 2;
            for (int i = 0; i < cnt; i += 2)
            {
                if (arguments[i].DataType != DataTypeEnum.Boolean || arguments[i + 1].DataType != DataTypeEnum.Decimal)
                {
                    throw new Exception($"Типы и порядок параметров функции '{Name}' должны быть вида 'одна или несколько пар условие:boolean и значение:decimal, и последние значение:decimal'.");
                }

                if ((arguments[i] as IBooleanConstantToken).Value)
                {
                    return arguments[i + 1];
                }
            }

            IConstantToken last = arguments.Last();
            if (last.DataType != DataTypeEnum.Decimal)
            {
                throw new Exception($"Типы и порядок параметров функции '{Name}' должны быть вида 'одна или несколько пар условие:boolean и значение:decimal, и последние значение:decimal'.");
            }
            return last;            
        }
    }
}
