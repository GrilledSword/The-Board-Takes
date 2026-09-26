using UnityEngine;

namespace BoardTakes.Core
{
    public static class BoardLog
    {
        const string Prefix = "[BoardTakes] ";

        public static void Info(string message) => Debug.Log(Prefix + message);
        public static void Warn(string message) => Debug.LogWarning(Prefix + message);
        public static void Error(string message) => Debug.LogError(Prefix + message);
    }
}
