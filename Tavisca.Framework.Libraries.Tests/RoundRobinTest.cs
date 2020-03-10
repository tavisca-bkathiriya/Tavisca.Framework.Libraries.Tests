using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Tavisca.Platform.Common.Internal;

namespace Tavisca.Framework.Libraries.Tests
{
    [TestClass]
    public class RoundRobinTest
    {

        [TestMethod]
        public void Tasks_Should_Be_Add_in_RoundRobin_Manner()
        {
            //Arrange
            var threadId = new List<int>();
            var lockObject = new Object();
            var waitHandle = new CountdownEvent(10);
            RoundRobinPool roundRobinPool = new RoundRobinPool(3);

            //Act
            roundRobinPool.Enqueue(() => getTask(lockObject, waitHandle, threadId));
            roundRobinPool.Enqueue(() => getTask(lockObject, waitHandle, threadId));
            roundRobinPool.Enqueue(() => getTask(lockObject, waitHandle, threadId));
            roundRobinPool.Enqueue(() => getTask(lockObject, waitHandle, threadId));
            roundRobinPool.Enqueue(() => getTask(lockObject, waitHandle, threadId));
            roundRobinPool.Enqueue(() => getTask(lockObject, waitHandle, threadId));
            roundRobinPool.Enqueue(() => getTask(lockObject, waitHandle, threadId));
            roundRobinPool.Enqueue(() => getTask(lockObject, waitHandle, threadId));
            roundRobinPool.Enqueue(() => getTask(lockObject, waitHandle, threadId));
            roundRobinPool.Enqueue(() => getTask(lockObject, waitHandle, threadId));
            waitHandle.Wait();


            //Assert
            var threadCounts = threadId.GroupBy(x => x).Select(x => x.Count()).OrderByDescending(x => x);
            Assert.AreEqual(3, threadCounts.Count());
            Assert.AreEqual(4, threadCounts.FirstOrDefault());
            Assert.AreEqual(3, threadCounts.LastOrDefault());
        }

        private void getTask(Object lockObject, CountdownEvent waitHandle, List<int> threadId)
        {
            lock (lockObject)
            {
                threadId.Add(Thread.CurrentThread.ManagedThreadId);
            }
            waitHandle.Signal();
        }
    }
}
