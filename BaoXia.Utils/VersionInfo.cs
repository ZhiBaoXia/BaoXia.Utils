namespace BaoXia.Utils;

public class VersionInfo
{
	////////////////////////////////////////////////
	// @静态常量
	////////////////////////////////////////////////

	#region 静态常量

	public const char VersionSectionSplitChar = '.';

	#endregion


	////////////////////////////////////////////////
	// @自身属性
	////////////////////////////////////////////////

	#region 自身属性

	protected string _versionString = string.Empty;

	public string VersionString
	{
		get
		{

			return _versionString;
		}
		set
		{
			if (StringUtil.EqualsStrings(_versionString, value))
			{
				return;
			}

			_versionString = value;
			VersionSectionNumbers = DidCreateVersionSectionNumbersFromString(_versionString);
		}
	}

	public int[] VersionSectionNumbers { get; private set; } = [];

	public int VersionSectionsCount => VersionSectionNumbers.Length;

	#endregion


	////////////////////////////////////////////////
	// @类方法
	////////////////////////////////////////////////

	#region 类方法

	public static bool operator ==(VersionInfo? versionInfoA, VersionInfo? versionInfoB)
	{
		var isVersionInfoA_Null = versionInfoA is null;
		var isVersionInfoB_Null = versionInfoB is null;
		if (isVersionInfoA_Null
			&& isVersionInfoB_Null)
		{
			return true;
		}
		else if (isVersionInfoA_Null
			|| isVersionInfoB_Null)
		{
			return false;
		}
		return versionInfoA!.CompareTo(versionInfoB) == 0;
	}

	public static bool operator !=(VersionInfo? versionInfoA, VersionInfo? versionInfoB)
	{
		var isVersionInfoA_Null = versionInfoA is null;
		var isVersionInfoB_Null = versionInfoB is null;
		if (isVersionInfoA_Null
			&& isVersionInfoB_Null)
		{
			return false;
		}
		else if (isVersionInfoA_Null
			|| isVersionInfoB_Null)
		{
			return true;
		}
		return versionInfoA!.CompareTo(versionInfoB) != 0;
	}

	public static bool operator >(VersionInfo? versionInfoA, VersionInfo? versionInfoB)
	{
		var isVersionInfoA_Null = versionInfoA is null;
		var isVersionInfoB_Null = versionInfoB is null;
		if (isVersionInfoA_Null
			&& isVersionInfoB_Null)
		{
			return false;
		}
		else if (isVersionInfoA_Null)
		{
			return false;
		}
		return versionInfoA!.CompareTo(versionInfoB) > 0;
	}


	public static bool operator <(VersionInfo? versionInfoA, VersionInfo? versionInfoB)
	{
		var isVersionInfoA_Null = versionInfoA is null;
		var isVersionInfoB_Null = versionInfoB is null;
		if (isVersionInfoA_Null
			&& isVersionInfoB_Null)
		{
			return false;
		}
		else if (isVersionInfoA_Null)
		{
			return true;
		}
		return versionInfoA!.CompareTo(versionInfoB) < 0;
	}

	public static bool operator >=(VersionInfo? versionInfoA, VersionInfo? versionInfoB)
	{
		var isVersionInfoA_Null = versionInfoA is null;
		var isVersionInfoB_Null = versionInfoB is null;
		if (isVersionInfoA_Null
			&& isVersionInfoB_Null)
		{
			return true;
		}
		else if (isVersionInfoA_Null)
		{
			return false;
		}
		return versionInfoA!.CompareTo(versionInfoB) >= 0;
	}

	public static bool operator <=(VersionInfo? versionInfoA, VersionInfo? versionInfoB)
	{
		var isVersionInfoA_Null = versionInfoA is null;
		var isVersionInfoB_Null = versionInfoB is null;
		if (isVersionInfoA_Null
			&& isVersionInfoB_Null)
		{
			return true;
		}
		else if (isVersionInfoA_Null)
		{
			return true;
		}
		return versionInfoA!.CompareTo(versionInfoB) <= 0;
	}

	#endregion


	////////////////////////////////////////////////
	// @自身实现
	////////////////////////////////////////////////

	#region 自身实现

	public VersionInfo()
	{ }

	public VersionInfo(string versionString)
	{
		VersionString = versionString;
	}


	public int CompareTo(VersionInfo? anotherVersionInfo)
	{
		if (anotherVersionInfo == null)
		{
			return 1;
		}

		var versionSectionsCount = VersionSectionNumbers.Length;
		var anotherVersionSectionsCount = anotherVersionInfo.VersionSectionNumbers.Length;
		if (versionSectionsCount > anotherVersionSectionsCount)
		{
			return 1;
		}
		else if (versionSectionsCount < anotherVersionSectionsCount)
		{
			return -1;
		}

		for (var versionSectionIndex = 0;
		    versionSectionIndex < versionSectionsCount;
		    versionSectionIndex++)
		{
			var versionSectionNumber = VersionSectionNumbers[versionSectionIndex];
			var anotherVersionSectionNumber = anotherVersionInfo.VersionSectionNumbers[versionSectionIndex];
			if (versionSectionNumber > anotherVersionSectionNumber)
			{
				return 1;
			}
			if (versionSectionNumber < anotherVersionSectionNumber)
			{
				return -1;
			}
		}
		return 0;
	}

	#endregion


	////////////////////////////////////////////////
	// @事件节点
	////////////////////////////////////////////////

	#region 事件节点

	protected virtual int[] DidCreateVersionSectionNumbersFromString(string? versionString)
	{
		if (string.IsNullOrEmpty(versionString))
		{
			return [];
		}

		var versionSectionStrings = versionString.Split(VersionSectionSplitChar);
		if (versionSectionStrings.Length < 1)
		{
			return [];
		}

		var versionSectionNumbers = new int[versionSectionStrings.Length];
		for (var i = 0; i < versionSectionStrings.Length; i++)
		{
			_ = int.TryParse(versionSectionStrings[i], out var versionSectionNumber);
			// !!!
			versionSectionNumbers[i] = versionSectionNumber;
			// !!!
		}
		return versionSectionNumbers;
	}

	#endregion


	////////////////////////////////////////////////
	// @重载“Object”
	////////////////////////////////////////////////

	#region 重载“Object”

	public override bool Equals(object? obj)
	{
		if (obj is not VersionInfo anotherVersionInfo)
		{
			return false;
		}
		return CompareTo(anotherVersionInfo) == 0;
	}

	public override int GetHashCode()
	{
		return _versionString?.GetHashCode() ?? 0;
	}

	#endregion
}