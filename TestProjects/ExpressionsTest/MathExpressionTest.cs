using Expressions;
using Expressions.Math;
using Expressions.Tokens;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace ExpressionsTest
{
    [TestClass]
    public class MathExpressionTest
    {
        [DataTestMethod]
        [DataRow("5", 5.0, DisplayName = "Считаем выражение: '5'.")]
        [DataRow("-5", -5.0, DisplayName = "Считаем выражение: '-5'.")]
        [DataRow("7 - 5", 2.0, DisplayName = "Считаем выражение: '7 - 5'.")]
        [DataRow("5 - 7", -2.0, DisplayName = "Считаем выражение: '5 - 7'.")]
        [DataRow("-3 * -5", 15.0, DisplayName = "Считаем выражение: '-3 * -5'.")]        
        [DataRow("МАКС( 1 + 2 ; 3 + 4 ; 5 )", 7.0, DisplayName = "Считаем выражение: 'МАКС( 1 + 2 , 3 + 4 , 5 )'.")]
        [DataRow(" 5 * МАКС( 1 + 2 ; 3 + 4 ; 5 )", 35.0, DisplayName = "Считаем выражение: ' 5 * МАКС( 1 + 2 , 3 + 4 , 5 )'.")]
        [DataRow(" 5 * МАКС( 1 + 2 ; МАКС(3 + 4 ; 7) - 6 ; 5 ) ", 25.0, DisplayName = "Считаем выражение: ' 5 * МАКС( 1 + 2 , МАКС(3 + 4 , 7) - 6 , 5 ) '.")]
        [DataRow(" 5 * ЕСЛИ( 2,0 > 3.0; 10; 5 > -3 * -5; 100; 1000) ", 5000.0, DisplayName = "Считаем выражение: ' 5 * ЕСЛИ( 2,0 > 3.0; 10; 5 > -3 * -5; 100; 1000) '.")]

        public void TestCalculation(string formula, double trueResult)
        {
            IConvertor convertor = new MathConvertor();
            if (convertor.TryConvert(formula, out List<IToken> parsedFormula))
            {
                IExpression expression = new MathExpression("Test", parsedFormula, null);
                if (expression.TryCalc(new List<IVariable>(), out IVariable result))
                {
                    string errMsg = $"Ожидали: '{trueResult}'; Получили: '{result}'.";

                    Assert.IsTrue(decimal.Equals((result as IDecimalVariable).Value, (decimal)trueResult), errMsg);
                }
                else
                {
                    Assert.Fail(string.Join("; ", expression.Errors));
                }
            }
            else
            {
                Assert.Fail(string.Join("; ", convertor.Errors));
            }
        }

        [DataTestMethod]
        [DataRow("МКС( 1, 2)", DisplayName = "Считаем выражение: 'МКС( 1, 2)'.")]

        public void TestNotCorrectCalculation(string formula)
        {
            IConvertor convertor = new MathConvertor();
            bool flag = convertor.TryConvert(formula, out List<IToken> parsedFormula);
            Assert.IsFalse(flag, "Ожидали ошибку.");
            if (!flag)
                return;
            IExpression expression = new MathExpression("Test", parsedFormula, null);
            Assert.IsFalse(expression.TryCalc(new List<IVariable>(), out _), "Ожидали ошибку.");
        }

    }
}
