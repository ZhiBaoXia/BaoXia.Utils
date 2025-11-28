using System.Net.WebSockets;
using System.Text.Json;

namespace BaoXia.Utils.Models
{
        public class WebSocketReceiveDataResult
        {
                ////////////////////////////////////////////////
                // @自身属性
                ////////////////////////////////////////////////

                #region 自身属性

                public WebSocketReceiveResult? Result { get; set; }

                public byte[]? BytesReceived { get; set; }

                public string? StringReceived { get; set; }

                public JsonDocument? JsonDocumentReceived { get; set; }

                #endregion
        }
}
