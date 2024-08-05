using System.IO.Pipelines;
using System.Text;
using System.Threading.Tasks;

namespace BaoXia.Utils.Extensions;

public static class PipeReaderExtension
{
	////////////////////////////////////////////////
	// @类方法
	////////////////////////////////////////////////

	#region 类方法

	public static async Task<string?> ReadStringAsync(
		this PipeReader pipeReader,
		System.Text.Encoding? textEncoding = null)
	{
		var originalContent = await pipeReader.ReadAsync();

		textEncoding ??= System.Text.Encoding.UTF8;
		var stringContent = textEncoding.GetString(originalContent.Buffer);
		{ }
		return stringContent;
	}

	#endregion
}