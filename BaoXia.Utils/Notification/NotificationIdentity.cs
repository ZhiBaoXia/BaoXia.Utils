using System.Collections.Generic;

namespace BaoXia.Utils.Notification
{
        public class NotificationIdentity
        {
                ////////////////////////////////////////////////
                // @自身属性，业务相关
                ////////////////////////////////////////////////

                #region 自身属性，业务相关

                public string? QueueName { get; set; }

                public string Name { get; set; }

                public IEnumerable<string>? TagNames { get; set; }

                #endregion


                ////////////////////////////////////////////////
                // @自身实现
                ////////////////////////////////////////////////

                #region 自身实现

                public NotificationIdentity(
                        string? queueName,
                        string name,
                        IEnumerable<string>? tagNames)
                {
                        this.QueueName = queueName;
                        this.Name = name;
                        this.TagNames = tagNames;
                }

                public NotificationIdentity(
                        NotificationIdentity notificationIdentity)
                {
                        this.QueueName = notificationIdentity.QueueName;
                        this.Name = notificationIdentity.Name;
                        this.TagNames = notificationIdentity.TagNames;
                }

                #endregion
        }
}
