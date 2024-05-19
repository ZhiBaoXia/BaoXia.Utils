using BaoXia.Utils.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace BaoXia.Utils.Test.ExtensionsTest
{
        [TestClass]
        public class FileExtensionTest
        {
                [TestMethod]
                public void CreateFilePathNotExistedBySameFileNameIndexWithDirectoryPath()
                {
                        var tmpFileDirectoryPath
                                = System.IO.Path.GetTempPath().ToFileSystemDirectoryPath();
                        var newFilePaths = new List<string>();
                        for (var testIndex = 0;
                        testIndex < 3;
                        testIndex++)
                        {
                                var newFilePath = FileExtension.CreateFilePathNotExistedBySameFileNameIndexWithDirectoryPath(
                                        tmpFileDirectoryPath,
                                        "newFile",
                                        "test");
                                if (newFilePath?.Length > 0)
                                {
                                        // 新文件之前没生成过：
                                        Assert.IsFalse(newFilePaths.Contains(newFilePath));
                                        {
                                                newFilePaths.Add(newFilePath);
                                        }
                                        // 新文件路径不存在：
                                        Assert.IsFalse(System.IO.File.Exists(newFilePath));
                                }
                                else
                                {
                                        Assert.Fail();
                                }
                                System.IO.File.WriteAllText(newFilePath, "test");
                        }
                }

                [TestMethod]
                public void CreateFilePathNotExistedBySameFileNameIndexWithFilePath()
                {
                        var tmpFileDirectoryPath
                                = System.IO.Path.GetTempPath().ToFileSystemDirectoryPath();
                        var filePath = tmpFileDirectoryPath + "newFile.test";
                        var newFilePaths = new List<string>();
                        for (var testIndex = 0;
                        testIndex < 3;
                        testIndex++)
                        {
                                var newFilePath = FileExtension.CreateFilePathNotExistedBySameFileNameIndexWithFilePath(filePath);
                                if (newFilePath?.Length > 0)
                                {
                                        // 新文件之前没生成过：
                                        Assert.IsFalse(newFilePaths.Contains(newFilePath));
                                        {
                                                newFilePaths.Add(newFilePath);
                                        }
                                        // 新文件路径不存在：
                                        Assert.IsFalse(System.IO.File.Exists(newFilePath));
                                }
                                else
                                {
                                        Assert.Fail();
                                }
                                System.IO.File.WriteAllText(newFilePath, "test");
                        }
                }
        }
}
