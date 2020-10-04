using Expressions.Tokens.Math;
using Expressions.Tokens;
using Expressions.Tokens.Boolean;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Expressions.Boolean
{
    public class BoolConvertor : IConvertor
    {

        private static readonly List<string> _binaryOperators = new List<string>()
        {
             AndBinaryBoolOpToken.Ident,
             OrBinaryBoolOpToken.Ident,
             XorBinaryBoolOpToken.Ident,
            };


        public List<string> Errors { get; private set; } = new List<string>();

        private IEnumerable<IToken> GetToken(string input)
        {
            int pos = 0;

            while (pos < input.Length)
            {
                char ch = input[pos];

                if (ch == '(')
                {
                    pos++;
                    yield return TokensRepository.OpenBracket; 
                }
                else if (ch == ')')
                {
                    pos++;
                    yield return TokensRepository.CloseBracket;
                }
                else if (Char.IsLetter(ch))
                {
                    int cur = pos;
                    while (cur < input.Length && (Char.IsLetter(input[cur]) || Char.IsDigit(input[cur])))
                        cur++;
                    string word = input.Substring(pos, cur - pos).ToUpper();
                    pos = cur;

                    if (word == NotPrefixUnaryBoolOpToken.Ident)
                    {
                        yield return TokensRepository.NotPrefixUnaryBoolOpToken;
                    }
                    else if (_binaryOperators.Contains(word))
                    {
                        yield return TokensRepository.GetBinaryBoolOpToken(word);
                    }
                    else if (word == TokensRepository.TrueConstant.Ident)
                    {
                        yield return TokensRepository.TrueConstant;
                    }
                    else if (word == TokensRepository.FalseConstant.Ident)
                    {
                        yield return TokensRepository.FalseConstant;
                    }
                    else
                    {
                        yield return TokensRepository.GetVariableToken(word);
                    }
                }
                else if (Char.IsDigit(ch))
                {
                    Errors.Add("Числовые константы не допустимы в логических выражениях.");
                    yield break;
                }
                else
                {
                    pos++;
                }
            }
        }

        public bool TryConvert(string input, out List<IToken> parsedFormula)
        {
            parsedFormula = new List<IToken>();
            Stack<IToken> stack = new Stack<IToken>();

            try
            {
                // последовательно обработаем все токены
                foreach (IToken token in GetToken(input))
                {
                    if (token.Type == TokenTypeEnum.Constant || token.Type == TokenTypeEnum.Variable) // константа или переменная
                    {
                        parsedFormula.Add(token);
                        continue;
                    }

                    // Операторы
                    if (token.Type == TokenTypeEnum.BinaryOperator || token.Type == TokenTypeEnum.UnaryOperator )
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
