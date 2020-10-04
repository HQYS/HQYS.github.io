using Expressions.Tokens;
using System.Collections.Generic;

namespace Expressions.Tokens.Math
{
    internal interface IFunctionToken : IToken
    {
        string Name { get; }

        List<string> Errors { get; }

        IConstantToken Calc(List<IConstantToken> arguments);
    }
}
