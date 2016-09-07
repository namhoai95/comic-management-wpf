using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyTruyenTranh.Helper.Log
{
    public interface ILogger
    {
        void LogException(Exception exception);
        void LogError(string message);
        void LogWarning(string message);
        void LogInfoMessage(string message);
        void LogWarning(string message, Exception exception);
        void LogDebug(string message, Exception exception);
        void LogError(string message, Exception exception);
        void LogFatal(string message);
        void LogFatal(string message, Exception exception);
        void LogInfo(string message);
        void LogInfo(string message, Exception exception);
    }
}
