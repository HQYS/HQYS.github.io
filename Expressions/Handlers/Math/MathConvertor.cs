using Expressions.Tokens.Math;
using Expressions.Tokens;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Expressions.Math
{
    public class MathConvertor : IConvertor
    {

        private static readonly List<string> _binaryOperators = new List<string>()
        {
             AdditionBinaryMathOpToken.Ident,
             SubtractionBOperatorToken.Ident,
             MultiplicationBinaryMathOpToken.Ident,
             DivisionBinaryMathOpToken.Ident,
             ExponentBinaryMathOpToken.Ident,
            };

        private static readonly List<char> _cmpOperatorChars = new List<char>() { '<', '>', '=' };

        private static readonly List<string> _comparisionOperators = new List<string>()
        {
            EqualComparisionOperatorToken.Ident,
            MoreComparisionOperatorToken.Ident,
            LessComparisionOperatorToken.Ident,
            MoreMoreOrEqualComparisionOperatorToken.Ident,
            LessOrEqualComparisionOperatorToken.Ident,
            NotEqualComparisionOperatorToken.Ident,
            };

        private static readonly List<string> _functions = new List<string>()
        {
            MaxFunctionToken.Name,
            MinFunctionToken.Name,
            MultipleConditionFunctionToken.Name,
        };

        public List<string> Errors { get; private set; } = new List<string>();

        private IEnumerable<IToken> GetToken(string input)
        {
            int pos = 0;
            IToken prev = null;

            while (pos < input.Length)
            {
                char ch = input[pos];

                if (_binaryOperators.Contains(string.Empty + ch)) // бинарнные арифметические операторы и унарные операторы
                {
                    pos++;

                    if (ch == '-' && IsPrevNotValueToken(prev))
                    {
                        prev = TokensRepository.SubtractionPrefixMathOperator;
                        yield return prev;
                    }
                    else
                    {
                        prev = TokensRepository.GetBinaryMathOpToken(string.Empty + ch);
                        yield return prev;
                    }
                }
                else if (_cmpOperatorChars.Contains(ch)) // операторы сравнения
                {
                    pos++;
                    string cmpOperator = string.Empty + ch;
                    char nextCh = input[pos];
                    if (_cmpOperatorChars.Contains(nextCh) && _comparisionOperators.Contains(cmpOperator + nextCh))
                    {
                        pos++;
                        cmpOperator += nextCh;
                    }
                    prev = TokensRepository.GetComparisionOperatorToken(cmpOperator);
                    yield return prev;
                }
                else if (ch == ';')
                {
                    pos++;
                    prev = TokensRepository.ArgumentSeparartor;
                    yield return prev;
                }
                else if (ch == '(')
                {
                    pos++;
                    prev = TokensRepository.OpenBracket;
                    yield return prev;
                }
                else if (ch == ')')
                {
                    pos++;
                    prev = TokensRepository.CloseBracket;
                    yield return prev;
                }
                else if (Char.IsDigit(ch)) // числовые константы
                {
                    int cur = pos;
                    while (cur < input.Length && (Char.IsDigit(input[cur]) || input[cur] == ',' || input[cur] == '.'))
                        cur++;

                    string valueStr = input.Substring(pos, cur - pos);
                    pos = cur;

                    if (TryParseAnySeparator(valueStr, out decimal value))
                    {
                        prev = TokensRepository.GetDecimalConstantToken(value);
                        yield return prev;
                    }
                    else
                    {
                        Errors.Add($"Не возможно перобразовать строку '{valueStr}' в число.");
                        yield break;
                    }
                }
                else if (Char.IsLetter(ch)) // переменные или функции
                {
                    int cur = pos;
                    while (cur < input.Length && Char.IsLetter(input[cur]) || Char.IsDigit(input[cur]))
                        cur++;

                    string name = input.Substring(pos, cur - pos);
                    pos = cur;

                    if (cur < input.Length && input[cur] == '(') // функции
                    {
                        string functionName = name.ToUpper();
                        if (!_functions.Any(f => f == functionName))
                        {
                            Errors.Add($"Не допустимое имя функции '{name}('.");
                            yield break;
                        }

                        prev = TokensRepository.GetFunctionToken(functionName);
                        yield return prev;
                    }
                    else // переменные
                    {
                        prev = TokensRepository.GetVariableToken(name);
                        yield return prev;
                    }
                }
                else
                {
                    pos++;
                }

            }
        }

        private bool IsPrevNotValueToken(IToken prev)
        {
            return
                prev == null || 
                prev != null &&
                    prev.Type == TokenTypeEnum.BinaryOperator ||
                    prev.Type == TokenTypeEnum.ComparisonOperator || 
                    prev.Type == TokenTypeEnum.OpenBracket || 
                    prev.Type == TokenTypeEnum.ArgumentSeparator;
        }

        private bool TryParseAnySeparator(string source, out decimal value)
        {
            value = 0;
            if (string.IsNullOrEmpty(source))
                return false;
            source = (CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator == ",")
                ? source.Replace(".", ",")
                : source.Replace(",", ".");
            return decimal.TryParse(source, NumberStyles.Float, CultureInfo.CurrentCulture, out value);
        }

        public bool TryConvert(string input, out List<IToken> parsedFormula)
        {
            parsedFormula = new List<IToken>();
            Stack<IToken> stack = new Stack<IToken>();

            try
            {
                int argumentCount = 0;

                // последовательно обработаем все токены
                foreach (IToken token in GetToken(input))
                {
                    if (token.Type == TokenTypeEnum.Constant || token.Type == TokenTypeEnum.Variable) // константа или переменная
                    {
                        parsedFormula.Add(token);
                        continue;
                    }

                    if (token.Type == TokenTypeEnum.Function) // функция
                    {
                        argumentCount = 1;
                        stack.Push(token);
                        continue;
                    }

                    if (token.Type == TokenTypeEnum.ArgumentSeparator) // разделитель аргументов функции
                    {
                        argumentCount++;
                        while (stack.Any() && stack.Peek().Type != TokenTypeEnum.OpenBracket)
                        {
                            IToken t = stack.Pop();
                            if (t.Type == TokenTypeEnum.ArgumentSeparator)
                                argumentCount++;
                            parsedFormula.Add(t);
                        }

                        if (!stack.Any() || stack.Peek().Type != TokenTypeEnum.OpenBracket)
                        {
                            Errors.Add($"В выражении пропущена открывающая скобка.");
                            break;
                        }
                        continue;
                    }

                    // Операторы
                    if (token.Type == TokenTypeEnum.BinaryOperator || token.Type == TokenTypeEnum.UnaryOperator || token.Type == TokenTypeEnum.ComparisonOperator)
                    {
                        while (stack.Any() && token.GetPriority() <= stack.Peek().GetPriority())
                        {
                            parsedFormula.Add(stack.Pop());
                        }
                        stack.Push(token);
                        continue;
                    }

                    if (token.Type == TokenTypeEnum.OpenBracket) // Открывющая скобка
                    {
                        stack.Push(token);
                        continue;
                    }

                    if (token.Type == TokenTypeEnum.CloseBracket) // Закрывющая скобка
                    {
                        while (stack.Any() && stack.Peek().Type != TokenTypeEnum.OpenBracket)
                        {
                            parsedFormula.Add(stack.Pop());
                        }

                        if (!stack.Any() || stack.Peek().Type != TokenTypeEnum.OpenBracket)
                        {
                            Errors.Add($"В выражении пропущена открывающая скобка.");
                            break;
                        }

                        stack.Pop(); // выкинем '('  OpenBracket

                        if (stack.Any() && stack.Peek().Type == TokenTypeEnum.Function) // добавим функцию и количество аргументов
                        {
                            parsedFormula.Add(TokensRepository.GetDecimalConstantToken(argumentCount));
                            parsedFormula.Add(stack.Pop());
                        }
                        continue;
                    }
                }

                while (stack.Any()) // оставшиеся токены в стэке
                {
                    IToken token = stack.Pop();

                    if (token.Type == TokenTypeEnum.OpenBracket)
                    {
                        Errors.Add($"В выражении пропущена закрывающая скобка.");
                        break;
                    }
                    if (token.Type == TokenTypeEnum.CloseBracket)
                    {
                        Errors.Add($"В выражении пропущена открывающая скобка.");
                        break;
                    }

                    parsedFormula.Add(token);
                }
            }
            catch (Exception ex)
            {
                Errors.Add($"Во время разбора формулы произошла ошибка - {ex.Message}.");
            }

            return !Errors.Any();
        }


    }
}
