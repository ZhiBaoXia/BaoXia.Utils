using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BaoXia.Utils.Test
{
	[TestClass]
	public class IDisposableTest
	{
		static bool _isObjectWithoutDestructorCalled_Destructor = false;
		static bool _isObjectWithoutDestructorCalled_Dispose = false;

		public class ObjectWithoutDestructor : IDisposable
		{
			public string? Name { get; set; } = "objectName";

			~ObjectWithoutDestructor()
			{
				this.Dispose();

				_isObjectWithoutDestructorCalled_Destructor = true;
			}

			public void Dispose()
			{
				this.Name = null;

				GC.SuppressFinalize(this);
				_isObjectWithoutDestructorCalled_Dispose = true;
			}
		}

		[TestMethod]
		public void IDisposableWithoutDestructor()
		{
			Task.Run(() =>
				{
					var testObject = new ObjectWithoutDestructor();
				})
				.Wait();

			// !!!
			GC.Collect();
			// !!!

			while (_isObjectWithoutDestructorCalled_Destructor != true)
			{
				Thread.Sleep(1);
			}

			Assert.IsTrue(_isObjectWithoutDestructorCalled_Destructor);
			Assert.IsTrue(_isObjectWithoutDestructorCalled_Dispose);
		}
	}
}
