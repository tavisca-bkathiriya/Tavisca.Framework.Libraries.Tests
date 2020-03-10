using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tavisca.Libraries.Logging.Tests.Utilities;
using Tavisca.Platform.Common.Logging;
using Tavisca.Platform.Common.Plugins.Json;

namespace Tavisca.Libraries.Logging.Tests.Logging
{
    [TestClass]
    public class LoggerTests
    {
        [TestMethod]
        public void Should_Log_Api_Log()
        {
            var id = Convert.ToString(Guid.NewGuid());
            var apiLog = Utility.GetApiLog();
            apiLog.Id = id;

            ILogFormatter formatter = JsonLogFormatter.Instance;
            var redisSink = Utility.GetRedisSink();

            IApplicationLogWriter applicationLogWriter = new LogWriter(formatter, redisSink);
            Logger.Initialize(applicationLogWriter);
            Logger.WriteLogAsync(apiLog).GetAwaiter().GetResult();
            //Thread.Sleep(40000);

            var logData = Utility.GetEsLogDataById(id);
            var esLogId = string.Empty;
            logData.TryGetValue("id", out esLogId);
            Assert.AreEqual(id, esLogId); 
        }
    }
}
