namespace BaoXia.Utils.Models
{
        public class ByteArray
        {

                ////////////////////////////////////////////////
                // @自身属性
                ////////////////////////////////////////////////

                #region 自身属性

                public string? Name { get; set; }

                public string? FileName { get; set; }

                public byte[]? Bytes { get; set; }

                #endregion


                ////////////////////////////////////////////////
                // @自身实现
                ////////////////////////////////////////////////

                #region 自身实现

                public ByteArray()
                { }

                public ByteArray(
                        string name,
                        string fileName,
                        byte[] bytes)
                {
                        this.Name = name;
                        this.FileName = fileName;
                        this.Bytes = bytes;
                }

                #endregion
        }
}
