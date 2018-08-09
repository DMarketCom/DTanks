using UnityEngine;

namespace SHLibrary.Logging
{
    public interface ILogColorSetter
    {
        Color GetColor(string chanel);
    }
}