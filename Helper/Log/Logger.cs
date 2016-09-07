using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace QuanLyTruyenTranh.Helper.Log
{
    public class Logger : ILogger
    {
        private static ILog log;

        static Logger()
        {
            log = LogManager.GetLogger(typeof(Logger));
            GlobalContext.Properties["host"] = Environment.MachineName;
        }

        public Logger(String type)
        {
            log = LogManager.GetLogger(type);
        }

        #region ILogger Members
        public void LogException(Exception exception)
        {

            log.Error(exception.Message, exception);
        }

        public void LogError(string message)
        {
            log.Error(message);

        }

        public void LogWarning(string message)
        {
            log.Warn(message);

        }

        public void LogInfoMessage(string message)
        {
            log.Info(message);
        }

        public void LogWarning(string message, Exception exception)
        {
            log.Warn(message, exception);
        }

        public void LogDebug(string message, Exception exception)
        {
            log.Debug(message, exception);
        }

        public void LogError(string message, Exception exception)
        {
            log.Error(message, exception);
        }

        public void LogFatal(string message)
        {
            log.Fatal(message);
        }

        public void LogFatal(string message, Exception exception)
        {
            log.Fatal(message, exception);
        }

        public void LogInfo(string message)
        {
            log.Info(message);
        }

        public void LogInfo(string message, Exception exception)
        {
            log.Info(message, exception);
        }

        #endregion
    }
}
