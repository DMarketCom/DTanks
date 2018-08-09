#if UNITY_EDITOR
using UnityEditor;

#else
using UnityEngine;

#endif

namespace SHLibrary.Utils
{
    public static class UnityUtils
    {
        public static void ApplicationQuit()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
