#if DEBUG_OVERWRAP
using UnityEngine;
using System.Diagnostics;

using UnityDebug = UnityEngine.Debug;
using UnityObject = UnityEngine.Object;

/// <summary>
/// 条件付きのUnityEngine.Debugクラス
/// </summary>
/// <remarks>
/// "DEBUG_OVERWRAP"を定義する事でクラスを有効化.
/// →有効時にログを出力する場合には"DEBUG_OVERWRAP___"を定義する必要がある.
/// ※注意点としては、この状態でColosoleに表示されるログをダブルクリックしてもジャンプ先がこのクラスになってしまう事.
/// 
/// 上記の現象がある為に常時有効化はせずに、基本的にはリリースビルドと言ったログを完全に無効化したい時に使用する想定となる.
/// </remarks>
public static class Debug
{
    // ==============================================
#region // Properties

    public static bool developerConsoleVisible
    {
        get 
        {
            return UnityDebug.developerConsoleVisible; 
        }
        set 
        {
            UnityDebug.developerConsoleVisible = value; 
        }
    }

    public static bool isDebugBuild
    {
        get 
        {
            return UnityDebug.isDebugBuild; 
        }
    }

    public static ILogger logger
    {
        get
        {
            return UnityDebug.unityLogger;
        }
    }

#endregion // Properties

    // ==============================================
#region // Assert

    [Conditional("UNITY_ASSERTIONS")]
    public static void Assert(bool condition)
    {
        UnityDebug.Assert(condition);
    }

    [Conditional("UNITY_ASSERTIONS")]
    public static void Assert(bool condition, UnityObject context)
    {
        UnityDebug.Assert(condition, context);
    }

    [Conditional("UNITY_ASSERTIONS")]
    public static void Assert(bool condition, object message)
    {
        UnityDebug.Assert(condition, message);
    }

    [Conditional("UNITY_ASSERTIONS")]
    public static void Assert(bool condition, string message)
    {
        UnityDebug.Assert(condition, message);
    }

    [Conditional("UNITY_ASSERTIONS")]
    public static void Assert(bool condition, object message, UnityObject context)
    {
        UnityDebug.Assert(condition, message, context);
    }

    [Conditional("UNITY_ASSERTIONS")]
    public static void Assert(bool condition, string message, UnityObject context)
    {
        UnityDebug.Assert(condition, message, context);
    }

    [Conditional("UNITY_ASSERTIONS")]
    public static void AssertFormat(bool condition, string format, params object[] args)
    {
        UnityDebug.AssertFormat(condition, format, args);
    }

    [Conditional("UNITY_ASSERTIONS")]
    public static void AssertFormat(bool condition, UnityObject context, string format, params object[] args)
    {
        UnityDebug.AssertFormat(condition, context, format, args);
    }

#endregion  // Assert

    // ==============================================
#region // Other

    [Conditional("DEBUG_OVERWRAP___")]
    public static void Break()
    {
        UnityDebug.Break();
    }

    [Conditional("DEBUG_OVERWRAP___")]
    public static void ClearDeveloperConsole()
    {
        UnityDebug.ClearDeveloperConsole();
    }

    [Conditional("DEBUG_OVERWRAP___")]
    public static void DebugBreak()
    {
        UnityDebug.DebugBreak();
    }

#endregion  // Other

    // ==============================================
#region DrawLine

    [Conditional("DEBUG_OVERWRAP___")]
    public static void DrawLine(Vector3 start, Vector3 end)
    {
        UnityDebug.DrawLine(start, end);
    }

    [Conditional("DEBUG_OVERWRAP___")]
    public static void DrawLine(Vector3 start, Vector3 end, Color color)
    {
        UnityDebug.DrawLine(start, end, color);
    }

    [Conditional("DEBUG_OVERWRAP___")]
    public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration)
    {
        UnityDebug.DrawLine(start, end, color, duration);
    }

    [Conditional("DEBUG_OVERWRAP___")]
    public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration, bool depthTest)
    {
        UnityDebug.DrawLine(start, end, color, duration, depthTest);
    }

#endregion

    // ==============================================
#region DrawRay

    [Conditional("DEBUG_OVERWRAP___")]
    public static void DrawRay(Vector3 start, Vector3 dir)
    {
        UnityDebug.DrawRay(start, dir);
    }

    [Conditional("DEBUG_OVERWRAP___")]
    public static void DrawRay(Vector3 start, Vector3 dir, Color color)
    {
        UnityDebug.DrawRay(start, dir, color);
    }

    [Conditional("DEBUG_OVERWRAP___")]
    public static void DrawRay(Vector3 start, Vector3 dir, Color color, float duration)
    {
        UnityDebug.DrawRay(start, dir, color, duration);
    }

    [Conditional("DEBUG_OVERWRAP___")]
    public static void DrawRay(Vector3 start, Vector3 dir, Color color, float duration, bool depthTest)
    {
        UnityDebug.DrawRay(start, dir, color, duration, depthTest);
    }

#endregion

    // ==============================================
#region // Log

    [Conditional("DEBUG_OVERWRAP___")]
    public static void Log(object message)
    {
        UnityDebug.Log(message);
    }

    [Conditional("DEBUG_OVERWRAP___")]
    public static void Log(object message, UnityObject context)
    {
        UnityDebug.Log(message, context);
    }

    [Conditional("UNITY_ASSERTIONS")]
    public static void LogAssertion(object message)
    {
        UnityDebug.LogAssertion(message);
    }

    [Conditional("UNITY_ASSERTIONS")]
    public static void LogAssertion(object message, UnityObject context)
    {
        UnityDebug.LogAssertion(message, context);
    }

    [Conditional("UNITY_ASSERTIONS")]
    public static void LogAssertionFormat(string format, params object[] args)
    {
        UnityDebug.LogAssertionFormat(format, args);
    }

    [Conditional("UNITY_ASSERTIONS")]
    public static void LogAssertionFormat(UnityObject context, string format, params object[] args)
    {
        UnityDebug.LogAssertionFormat(context, format, args);
    }

    [Conditional("DEBUG_OVERWRAP___")]
    public static void LogError(object message)
    {
        UnityDebug.LogError(message);
    }

    [Conditional("DEBUG_OVERWRAP___")]
    public static void LogError(object message, UnityObject context)
    {
        UnityDebug.LogError(message, context);
    }

    [Conditional("DEBUG_OVERWRAP___")]
    public static void LogErrorFormat(string format, params object[] args)
    {

        UnityDebug.LogErrorFormat(format, args);
    }

    [Conditional("DEBUG_OVERWRAP___")]
    public static void LogErrorFormat(UnityObject context, string format, params object[] args)
    {
        UnityDebug.LogErrorFormat(context, format, args);
    }

    [Conditional("DEBUG_OVERWRAP___")]
    public static void LogException(System.Exception exception)
    {
        UnityDebug.LogException(exception);
    }

    [Conditional("DEBUG_OVERWRAP___")]
    public static void LogException(System.Exception exception, UnityObject context)
    {
        UnityDebug.LogException(exception, context);
    }

    [Conditional("DEBUG_OVERWRAP___")]
    public static void LogFormat(string format, params object[] args)
    {
        UnityDebug.LogFormat(format, args);
    }

    [Conditional("DEBUG_OVERWRAP___")]
    public static void LogFormat(UnityObject context, string format, params object[] args)
    {
        UnityDebug.LogFormat(context, format, args);
    }

    [Conditional("DEBUG_OVERWRAP___")]
    public static void LogWarning(object message)
    {
        UnityDebug.LogWarning(message);
    }

    [Conditional("DEBUG_OVERWRAP___")]
    public static void LogWarning(object message, UnityObject context)
    {
        UnityDebug.LogWarning(message, context);
    }

    [Conditional("DEBUG_OVERWRAP___")]
    public static void LogWarningFormat(string format, params object[] args)
    {
        UnityDebug.LogWarningFormat(format, args);
    }

    [Conditional("DEBUG_OVERWRAP___")]
    public static void LogWarningFormat(UnityObject context, string format, params object[] args)
    {
        UnityDebug.LogWarningFormat(context, format, args);
    }

#endregion  // Log
}
#endif