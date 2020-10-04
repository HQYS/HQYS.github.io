using Expressions.Math;
using Expressions.Tokens;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace ExpressionsTest
{
    [TestClass]
    public class MathConvertorTest 
    {
        [DataTestMethod]
        [DataRow("5", "5", DisplayName = "Парсим выражение: '5'.")]
        [DataRow("-5", "5 ¬", DisplayName = "Парсим выражение: '-5'.")]
        
        [DataRow("-5 * 3", "5 ¬ 3 *", DisplayName = "Парсим выражение: '-5 * 3'.")]
        [DataRow("3 * -5", "3 5 ¬ *", DisplayName = "Парсим выражение: '3 * -5'.")]
        [DataRow("3 * -5 + 4 ", "3 5 ¬ * 4 +", DisplayName = "Парсим выражение: '3 * -5 + 4 '.")]

        [DataRow("7 - 5", "7 5 -", DisplayName = "Парсим выражение: '7 - 5'.")]
        [DataRow("МАКС( 1 + 2 ; 3 + 4 ; 5 )", "1 2 + 3 4 + 5 3 МАКС", DisplayName = "Парсим выражение: 'МАКС( 1 + 2 , 3 + 4 , 5 )'.")]
        [DataRow(" 5 * МАКС( 1 + 2 ; 3 + 4 ; 5 )", "5 1 2 + 3 4 + 5 3 МАКС *", DisplayName = "Парсим выражение: ' 5 * МАКС( 1 + 2 , 3 + 4 , 5 )'.")]
        [DataRow(" 5 * МАКС( 1 + 2 ; МАКС(3 + 4 ; 7) - 6 ; 5 ) ", "5 1 2 + 3 4 + 7 2 МАКС 6 - 5 3 МАКС *", DisplayName = "Парсим выражение: ' 5 * МАКС( 1 + 2 , МАКС(3 + 4 , 7) - 6 , 5 ) '.")]

        [DataRow(" 5 * МАКС( 1 + PRT2 ; МАКС(3 + 4 ; - PRT7) - 6 ; 5 ) ", "5 1 PRT2 + 3 4 + PRT7 ¬ 2 МАКС 6 - 5 3 МАКС *", DisplayName = "Парсим выражение: ' 5 * МАКС( 1 + PRT2 , МАКС(3 + 4 , -PRT7) - 6 , 5 ) '.")]

        [DataRow("5 * ЕСЛИ( 2 > 3; 10; 5.0 > -3,0 * -5; 100; 1000)", "5 2 3 > 10 5 3 ¬ 5 ¬ * > 100 1000 5 ЕСЛИ *", DisplayName = "Парсим выражение: '5 * ЕСЛИ( 2 > 3, 10, 5 > -3 * -5, 100, 1000)'.")]

        [DataRow("МКС( 1, 2)", "", DisplayName = "Парсим выражение: 'МКС( 1, 2)'.")]

        public void TestConvertor(string expression, string trueResult)
        {
            MathConvertor convertor = new MathConvertor();            
            convertor.TryConvert(expression, out List<IToken> parsedFormula);

            string result = string.Join(" ", parsedFormula );
            string errMsg = $"Ожидали: '{trueResult}'; Получили: '{result}'; Ошибки: '{string.Join("; ", convertor.Errors)}'.";

            Assert.IsTrue(string.Equals(result, trueResult), errMsg);
        }
    }
}
