using System;
using System.Threading.Tasks;

namespace BaoXia.Utils
{
	public class CodesAtFunctionEnd : IDisposable, IAsyncDisposable
	{

		////////////////////////////////////////////////
		// @自身属性
		////////////////////////////////////////////////

		#region 自身属性

		public Action? Codes { get; set; }
		public Func<Task>? CodesAsync { get; set; }

		#endregion

		////////////////////////////////////////////////
		// @自身实现
		////////////////////////////////////////////////

		#region 自身实现

		public CodesAtFunctionEnd(Action codes)
		{
			this.Codes = codes;
		}

		public CodesAtFunctionEnd(Func<Task>? codesAsync)
		{
			this.CodesAsync = codesAsync;
		}

		#endregion

		////////////////////////////////////////////////
		// @实现“IDisposable”
		////////////////////////////////////////////////

		#region 实现“IDisposable”

		public void Dispose()
		{
			if (this.Codes != null)
			{
				this.Codes.Invoke();
			}
			if (this.CodesAsync != null)
			{
				this.CodesAsync.Invoke().Wait();
			}

			GC.SuppressFinalize(this);
		}

		public async ValueTask DisposeAsync()
		{
			if (this.Codes != null)
			{
				this.Codes.Invoke();
			}
			if (this.CodesAsync != null)
			{
				await this.CodesAsync.Invoke();
			}

			GC.SuppressFinalize(this);
		}

		#endregion
	}
}
