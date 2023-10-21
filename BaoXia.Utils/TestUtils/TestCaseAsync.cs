using System.Threading.Tasks;

namespace BaoXia.Utils.TestUtils
{
        public abstract class TestCaseAsync : TestCase
        {

                ////////////////////////////////////////////////
                // @事件节点
                ////////////////////////////////////////////////

                #region 事件节点
                protected abstract Task DidTestAsync();

                #endregion


                ////////////////////////////////////////////////
                // @实现“TestCase”
                ////////////////////////////////////////////////

                #region 实现“TestCase”
                protected override void DidTest()
                {
                        DidTestAsync().Wait();
                }

                #endregion
        }
}

