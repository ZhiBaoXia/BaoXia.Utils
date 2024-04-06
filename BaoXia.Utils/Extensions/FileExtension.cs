namespace BaoXia.Utils.Extensions;

public static class FileExtension
{
	public static string? CreateFilePathNotExistedBySameFileNameIndexWithDirectoryPath(
		string directoryPath,
		string? fileName,
		string? fileExtensionName,
		string spliterBetweenFileNameAndIndex = "_")
	{
		if (directoryPath.Length < 1)
		{
			return null;
		}

		if (fileName == null)
		{
			fileName = string.Empty;
		}
		if (fileExtensionName == null)
		{
			fileExtensionName = string.Empty;
		}

		directoryPath = directoryPath.ToFileSystemDirectoryPath();
		if (!System.IO.Directory.Exists(directoryPath))
		{
			System.IO.Directory.CreateDirectory(directoryPath);
		}
		if (fileExtensionName == null)
		{
			fileExtensionName = "";
		}
		else if (fileExtensionName.Length > 0
			&& !fileExtensionName.StartsWith("."))
		{
			fileExtensionName = "." + fileExtensionName;
		}

		var fileIndex = 0;
		string filePath;
		do
		{
			if (fileIndex <= 0)
			{
				filePath
					= directoryPath
					+ fileName
					+ fileExtensionName;
			}
			else
			{
				filePath
					= directoryPath
					+ fileName + spliterBetweenFileNameAndIndex + fileIndex
					+ fileExtensionName;
			}
			fileIndex++;
		} while (filePath?.Length > 0
		&& System.IO.File.Exists(filePath));
		return filePath;
	}

	public static string? CreateFilePathNotExistedBySameFileNameIndexWithFilePath(string filePath)
	{
		if (filePath.Length < 1)
		{
			return null;
		}

		var dictionaryPath = filePath.ToFileSystemDirectoryPath(true);
		var fileName = filePath.ToFileName(false);
		if (fileName == null)
		{
			return null;
		}
		var fileExtensionName = filePath.ToFileExtensionName(true);

		return FileExtension.CreateFilePathNotExistedBySameFileNameIndexWithDirectoryPath(
			dictionaryPath,
			fileName,
			fileExtensionName);
	}
}