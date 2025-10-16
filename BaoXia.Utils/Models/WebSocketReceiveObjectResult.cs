namespace BaoXia.Utils.Models
{
	public class WebSocketReceiveObjectResult<ObjectType> : WebSocketReceiveDataResult
	{

		////////////////////////////////////////////////
		// @自身属性
		////////////////////////////////////////////////

		#region 自身属性

		public ObjectType? ObjectReceived { get; set; }

		#endregion
	}
}
