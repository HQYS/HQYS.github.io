using Expressions.Tokens;
using System.Collections.Generic;

namespace Expressions
{
    public interface IConvertor
    {
        List<string> Errors { get; }

        bool TryConvert(string input, out List<IToken> parsedFormula);
    }
}
