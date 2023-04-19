
using System;

namespace BaoXia.Utils.Cache
{
        public class ObjectPoolItemContainer<ObjectType> : IDisposable where ObjectType : class
        {

                ////////////////////////////////////////////////
                // @自身属性
                ////////////////////////////////////////////////

                #region 自身属性


                public ObjectPool<ObjectType> _ownerPool;
                public ObjectPool<ObjectType> OwnerPool => _ownerPool;

                public ObjectType Item { get; set; }

                #endregion


                ////////////////////////////////////////////////
                // @自身实现
                ////////////////////////////////////////////////

                #region 自身实现

                public ObjectPoolItemContainer(
                        ObjectPool<ObjectType> ownerPool,
                        ObjectType item)
                {
                        _ownerPool = ownerPool;

                        this.Item = item;
                }

                ~ObjectPoolItemContainer()
                {
                        var item = this.Item;
                        if (item != null)
                        {
                                _ownerPool.ReleaseObject(item);
                        }
                }

                #endregion


                ////////////////////////////////////////////////
                // @实现“Dispose”
                ////////////////////////////////////////////////

                #region 实现“接口名称”

                public void Dispose()
                {
                        var item = this.Item;
                        if (item != null)
                        {
                                _ownerPool.ReleaseObject(item);
                        }

                        ////////////////////////////////////////////////
                        GC.SuppressFinalize(this);
                        ////////////////////////////////////////////////
                }

                #endregion
        }
}
