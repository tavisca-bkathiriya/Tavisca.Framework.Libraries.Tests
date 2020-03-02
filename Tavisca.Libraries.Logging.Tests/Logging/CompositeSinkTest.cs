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
    public class CompositeSinkTest
    {
        [TestMethod]
        public void Should_Log_Api_Log_Redis_Primary()
        {
            var id = Convert.ToString(Guid.NewGuid());
            var apiLog = Utility.GetApiLog();
            apiLog.Id = id;
            var formatter = JsonLogFormatter.Instance;
            var firehoseSink = Utility.GetFirehoseSink();
            var redisSink = Utility.GetRedisSink();
            var compositeSink = Utility.GetCompositeSink(formatter, redisSink, firehoseSink);

            var logWriter = new LogWriter(formatter, compositeSink);
            logWriter.WriteAsync(apiLog).GetAwaiter().GetResult();
            Thread.Sleep(60000);

            var logData = Utility.GetEsLogDataById(id);
            var esLogId = string.Empty;
            logData.TryGetValue("id", out esLogId);

            Assert.AreEqual(id, esLogId);
        }

        [TestMethod]
        public void Should_Log_Api_Log_Firehose_Primary()
        {
            var id = Convert.ToString(Guid.NewGuid());
            var apiLog = Utility.GetApiLog();
            apiLog.Id = id;
            var formatter = JsonLogFormatter.Instance;
            var firehoseSink = Utility.GetFirehoseSink();
            var redisSink = Utility.GetRedisSink();
            var compositeSink = Utility.GetCompositeSink(formatter, redisSink, firehoseSink);

            var logWriter = new LogWriter(formatter, compositeSink);
            logWriter.WriteAsync(apiLog).GetAwaiter().GetResult();
            Thread.Sleep(60000);

            var logData = Utility.GetEsLogDataById(id);
            var esLogId = string.Empty;
            logData.TryGetValue("id", out esLogId);

            Assert.AreEqual(id, esLogId); //TODO: Manually verify logs using secondary sink (Use expired firehose credentials) 
        }
    }
}
