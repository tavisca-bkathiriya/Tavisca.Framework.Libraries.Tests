using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tavisca.Platform.Common;

namespace Tavisca.Libraries.LockManagement.Tests
{
    [TestClass]
    public class AsyncLockTest
    {
        [TestMethod]
        public void AsyncLock_Should_Give_Serialise_Access_To_Object_In_Asynchronous_Way()
        {
            AsyncLock asyncLock = new AsyncLock();
            CountdownEvent waitHandle = new CountdownEvent(2);
            Func<Task<DateTime>> asyncLockAction = async () =>
            {
                using (await asyncLock.LockAsync())
                {
                    Thread.Sleep(2000);
                    waitHandle.Signal();
                    return DateTime.Now;
                }
            };
            DateTime threadTime1 = DateTime.Now, threadTime2 = DateTime.Now;
            Parallel.Invoke(async () => { threadTime1 = await asyncLockAction(); }, async () => { threadTime2 = await asyncLockAction(); });
            waitHandle.Wait();
            var timeDiff = Math.Abs((threadTime2 - threadTime1).TotalMilliseconds);
            Assert.IsTrue(timeDiff >= 2000);
            Assert.IsTrue(timeDiff <= 4000);
        }
    }
}
