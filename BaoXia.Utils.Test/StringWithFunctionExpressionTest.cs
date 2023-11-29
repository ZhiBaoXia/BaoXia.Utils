using BaoXia.Utils.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BaoXia.Utils.Test
{
	[TestClass]
	public class StringWithFunctionExpressionTest
	{
		private static readonly int RandomValue = 414670617;// new Random().Next();

		private static readonly StringWithFunctionExpression.FunctionExpressionInfo[] StringFunctionDefines
			= {
			new StringWithFunctionExpression.FunctionExpressionInfo(
				"#Random",
				"(",
				")#"),

			new StringWithFunctionExpression.FunctionExpressionInfo(
				"#DateTime",
				"(",
				")#"),

			new StringWithFunctionExpression.FunctionExpressionInfo(
				"#ImageExtensionName",
				"(",
				")#"),
			};


		[TestMethod]
		public void FunctionExpressionTest()
		{
			var randomValue = RandomValue;
			var randomValueStringLength = 10;



			var functionResultString
				= StringWithFunctionExpression
				.CreateStringByComputeFunctionExpression(
				"#Random(" + randomValueStringLength + ")#",
				StringFunctionDefines,
				this.DidInvokeFunction);
			// !!!!
			Assert.IsTrue(functionResultString?.Length == randomValueStringLength);
			Assert.IsTrue(functionResultString?.EndsWith(randomValue.ToString()));
			// !!!



			functionResultString
				 = StringWithFunctionExpression
				 .CreateStringByComputeFunctionExpression(
				 "#Random ( " + randomValueStringLength + " ) #",
				 StringFunctionDefines,
				 this.DidInvokeFunction);
			// !!!!
			Assert.IsTrue(functionResultString?.Length == randomValueStringLength);
			Assert.IsTrue(functionResultString?.EndsWith(randomValue.ToString()));
			// !!!
		}


		////////////////////////////////////////////////
		// @事件节点
		////////////////////////////////////////////////

		#region 事件节点


		private string? DidInvokeFunction(StringWithFunctionExpression.FunctionExpressionInfo functionExpressionInfo)
		{
			string? functionResult = null;
			if (functionExpressionInfo.Name.Equals("#Random"))
			{
				var randomValueLength = 0;
				var functionParams = functionExpressionInfo.Params;
				if (functionParams?.Length > 0)
				{
					// !!!
					_ = int.TryParse(functionParams[0], out randomValueLength);
					// !!!
				}
				// !!!
				functionResult = RandomValue.ToString();
				// !!!
				if (randomValueLength > 0)
				{
					functionResult = functionResult.StringByRetainRightCharsToLength(
						'0',
						randomValueLength);
				}
			}
			return functionResult;
		}

		#endregion


	}
}
