using UnityEngine;

namespace SHLibrary.Logging
{
    public interface ILogFilter
    {
        bool IsNeedShow(string chanel, LogType logType);
    }
}