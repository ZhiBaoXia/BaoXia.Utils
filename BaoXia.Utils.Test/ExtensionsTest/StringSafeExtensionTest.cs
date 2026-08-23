using BaoXia.Utils.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BaoXia.Utils.Test.ExtensionsTest;

[TestClass]
public class StringSafeExtensionTest
{
	[TestMethod]
	public void IsHackingStringBySqlInjectionTest()
	{
		string?[] safeStrings =
		[
			null,
			string.Empty,
			"O'Reilly",
			"请选择一个字段，然后从列表中确认。",
			"Please select a value from the list.",
			"Use '--' as the separator.",
			"C# / SQL developer"
		];
		foreach (var safeString in safeStrings)
		{
			Assert.IsFalse(
				safeString.IsHackingStringBySqlInjection(),
				safeString);
		}

		string[] hackingStrings =
		[
			"admin'--",
			"' OR 1=1 --",
			"' OR 'x' LIKE 'x' #",
			"1 UNION SELECT password FROM users",
			"1 UNION/**/ALL/**/SELECT password FROM users",
			"1; DROP TABLE users",
			"1; DELETE FROM users",
			"DELETE FROM users",
			"INSERT INTO users(name) VALUES('test')",
			"SELECT password FROM users",
			"UPDATE users SET role = 'admin'",
			"1'; WAITFOR DELAY '00:00:05'--",
			"1 AND SLEEP(5)",
			"1 AND pg_sleep(5)",
			"1 UNION SELECT LOAD_FILE('/etc/passwd')",
			"1 UNION SELECT table_name FROM information_schema.tables",
			"%2527%2520OR%25201%253D1--",
			"&#39; OR 1=1 --",
			"54D911F3ADF748748ACAFED293164051\" union select 1-- ",
			"' union select 1,2,3,4,5,6,7,8,9,10-- ",
			"\" union select 1,2,3,4,5-- ",
		];
		foreach (var hackingString in hackingStrings)
		{
			Assert.IsTrue(hackingString.IsHackingStringBySqlInjection(), hackingString);
		}
		var a = 3;
	}
}
