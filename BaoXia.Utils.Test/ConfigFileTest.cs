﻿using BaoXia.Utils.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace BaoXia.Utils.Test
{
	public class ObjectProperty
	{
		public string? Name { get; set; }

		public int Value { get; set; }
	}

	public class TestConfigFile : BaoXia.Utils.ConfigFile
	{
		public int IntValue { get; set; }

		public float FloatValue { get; set; }

		public double DoubleValue { get; set; }

		public string? StringValue { get; set; }

		public DateTime DateTimeValue { get; set; }

		public string[]? StringArrayValue { get; set; }

		public int[]? IntArrayValue { get; set; }

		public List<float>? FloatArrayValue { get; set; }

		public ObjectProperty[]? ObjectArrayValue { get; set; }
	}

	[TestClass]
	public class ConfigFileTest
	{
		[TestMethod]
		public void ConfigFileLoadAndSaveWithConfigOptionsPathTest()
		{
			var tmpFileDirectoryPath
				= System.IO.Path.GetTempPath().ToFileSystemDirectoryPath()
				+ "BaoXia.Utils.Test";

			var configFileForLoad = new TestConfigFile();
			{
				configFileForLoad.AddConfigFileChangedEvent((configFile) =>
				{
					if (configFile is not TestConfigFile testConfigFile)
					{
						Assert.Fail();
						return;
					}

					if (testConfigFile.IntValue == 1)
					{
						Assert.AreEqual(1, testConfigFile.IntValue);
						Assert.AreEqual(2.0F, testConfigFile.FloatValue);
						Assert.AreEqual(3.0, testConfigFile.DoubleValue);
						Assert.AreEqual("4.0", testConfigFile.StringValue);
					}
					else
					{
						Assert.AreEqual(2, testConfigFile.IntValue);
						Assert.AreEqual(3.0F, testConfigFile.FloatValue);
						Assert.AreEqual(4.0, testConfigFile.DoubleValue);
						Assert.AreEqual("4.0+1.0", testConfigFile.StringValue);
					}
				});
			}
			{
				ConfigFile.InitializeWithConfigFilesDirectoryPath(tmpFileDirectoryPath);
			}
			System.IO.File.Delete(configFileForLoad.FilePath!);

			var configFileForSave = new TestConfigFile
			{
				IntValue = 1,
				FloatValue = 2.0F,
				DoubleValue = 3.0,
				StringValue = "4.0",
				DateTimeValue = new DateTime(2021, 3, 27, 1, 2, 3),

				StringArrayValue = ["a", "b", "c"],

				IntArrayValue = [1, 2, 3],

				FloatArrayValue = [1.0F, 2.0F, 3.0F],

				ObjectArrayValue =
				[
					new()
					{
						Name = "Name_1",
						Value = 1
					},
					new()
					{
						Name = "Name_2",
						Value = 2
					}
				],
				// !!!
				FilePath = configFileForLoad.FilePath
			};
			Assert.IsTrue(configFileForSave.Save());
			// !!!

			// !!!
			Assert.IsTrue(configFileForSave.Save(true, out var configBackupFilePath));
			Assert.IsTrue(System.IO.File.Exists(configBackupFilePath));
			// !!!


			// !!!⚠ 等待半秒中，等文件系统通知配置文件变动 ⚠!!!
			const double maxSecondsToWaitFileChanged = 60.0;
			var beginTimeToWaitFileChanged = DateTime.Now;
			while (configFileForLoad.IntValue != configFileForSave.IntValue
				&& (DateTime.Now - beginTimeToWaitFileChanged).TotalSeconds < maxSecondsToWaitFileChanged)
			{
				System.Threading.Thread.Sleep(500);
			}
			System.Threading.Thread.Sleep(500);
			{
				// !!!
				Assert.AreEqual(configFileForSave.IntValue, configFileForLoad.IntValue);
				Assert.AreEqual(configFileForSave.FloatValue, configFileForLoad.FloatValue);
				Assert.AreEqual(configFileForSave.DoubleValue, configFileForLoad.DoubleValue);
				Assert.IsTrue(configFileForLoad.StringValue?.Equals(configFileForSave.StringValue));
				Assert.AreEqual(configFileForSave.DateTimeValue, configFileForLoad.DateTimeValue);
				// !!!
			}

			{
				configFileForSave.IntValue += 1;
				configFileForSave.FloatValue += 1.0F;
				configFileForSave.DoubleValue += 1.0;
				configFileForSave.StringValue += "+1.0";
				configFileForSave.DateTimeValue = configFileForSave.DateTimeValue
					.AddSeconds(1);
			}
			// !!!
			Assert.IsTrue(configFileForSave.Save());
			// !!!

			// !!!⚠ 等待半秒中，等文件系统通知配置文件变动 ⚠!!!
			beginTimeToWaitFileChanged = DateTime.Now;
			while (configFileForLoad.IntValue != configFileForSave.IntValue
				&& (DateTime.Now - beginTimeToWaitFileChanged).TotalSeconds < maxSecondsToWaitFileChanged)
			{
				System.Threading.Thread.Sleep(500);
			}
			System.Threading.Thread.Sleep(500);
			{
				// !!!
				Assert.AreEqual(configFileForSave.IntValue, configFileForLoad.IntValue);
				Assert.AreEqual(configFileForSave.FloatValue, configFileForLoad.FloatValue);
				Assert.AreEqual(configFileForSave.DoubleValue, configFileForLoad.DoubleValue);
				Assert.IsTrue(configFileForLoad.StringValue?.Equals(configFileForSave.StringValue));
				Assert.AreEqual(configFileForSave.DateTimeValue, configFileForLoad.DateTimeValue);
				// !!!
			}
		}

		[TestMethod]
		public void ConfigFileSecondExtensionNameTest()
		{
			var tmpFileDirectoryPath
				= System.IO.Path.GetTempPath().ToFileSystemDirectoryPath()
				+ "BaoXia.Utils.Test";

			var configFileForLoad = new TestConfigFile();
			{
				ConfigFile.InitializeWithConfigFilesDirectoryPath(tmpFileDirectoryPath);
			}

			var lastEnvironmentConfigSymbol = BaoXia.Utils.Environment.EnviromentName;
			{
				string originalConfigFilePath = configFileForLoad.FilePath!;
				string finalConfigFile = configFileForLoad.FinalFilePath!;
				{
					// !!!
					Assert.IsTrue(originalConfigFilePath.IsEquals(finalConfigFile));
					// !!!
				}


				BaoXia.Utils.Environment.EnviromentName = EnvironmentNameDefault.Development;
				{
					var configFileName = "." + ConfigFile.ConfigFileExtensionName;
					var finalConfigFileAccurate = originalConfigFilePath.Replace(
						configFileName,
						"." + Environment.EnviromentName + configFileName);

					////////////////////////////////////////////////
					System.IO.File.Delete(finalConfigFileAccurate);
					////////////////////////////////////////////////
					// !!!
					finalConfigFile = configFileForLoad.FinalFilePath!;
					Assert.IsTrue(originalConfigFilePath.IsEquals(finalConfigFile));
					// !!!

					////////////////////////////////////////////////
					System.IO.File.Create(finalConfigFileAccurate);
					////////////////////////////////////////////////
					// !!!
					finalConfigFile = configFileForLoad.FinalFilePath!;
					Assert.AreNotEqual(true, originalConfigFilePath.IsEquals(finalConfigFile));
					Assert.IsTrue(finalConfigFileAccurate.IsEquals(finalConfigFile));
					// !!!
				}
			}
			BaoXia.Utils.Environment.EnviromentName = lastEnvironmentConfigSymbol;
		}
	}
}
