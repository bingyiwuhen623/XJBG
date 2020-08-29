using System;
using System.ComponentModel;
using System.Diagnostics;
using UnityEngine.Bindings;
using UnityEngine.Internal;

namespace UnityEngine
{
    // 摘要:
    //     Class containing methods to ease debugging while developing a game.
    public class Debuger
    {
        private static bool ShowLog => GameDefine.SHOW_LOG;
        // 摘要:
        //     Reports whether the development console is visible. The development console cannot
        //     be made to appear using:
        public static bool DeveloperConsoleVisible
        {
            get { return Debug.developerConsoleVisible; }
            set { Debug.developerConsoleVisible = value; }
        }
        // 摘要:
        //     Get default debug logger.
        public static ILogger UnityLogger
        {
            get { return Debug.unityLogger; }
        }
        // 摘要:
        //     In the Build Settings dialog there is a check box called "Development Build".
        public static bool IsDebugBuild
        {
            get { return Debug.isDebugBuild; }
        }
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Debug.logger is obsolete. Please use Debug.unityLogger instead (UnityUpgradable) -> unityLogger")]
        public static ILogger Logger
        {
            get { return Debug.logger; }
        }
        [Conditional("UNITY_ASSERTIONS")]
        public static void Assert(bool condition, string message, Object context)
        {
            if (ShowLog)
            {
                Debug.Assert(condition, message, context);
            }
        }
        // 摘要:
        //     Assert a condition and logs an error message to the Unity console on failure.
        //
        // 参数:
        //   condition:
        //     Condition you expect to be true.
        //
        //   context:
        //     Object to which the message applies.
        //
        //   message:
        //     String or object to be converted to string representation for display.
        [Conditional("UNITY_ASSERTIONS")]
        public static void Assert(bool condition, object message, Object context)
        {
            if (ShowLog)
            {
                Debug.Assert(condition, message, context);
            }
        }
        [Conditional("UNITY_ASSERTIONS")]
        public static void Assert(bool condition, string message)
        {
            if (ShowLog)
            {
                Debug.Assert(condition, message);
            }
        }
        // 摘要:
        //     Assert a condition and logs an error message to the Unity console on failure.
        //
        // 参数:
        //   condition:
        //     Condition you expect to be true.
        //
        //   context:
        //     Object to which the message applies.
        //
        //   message:
        //     String or object to be converted to string representation for display.
        [Conditional("UNITY_ASSERTIONS")]
        public static void Assert(bool condition, object message)
        {
            if (ShowLog)
            {
                Debug.Assert(condition, message);
            }
        }
        // 摘要:
        //     Assert a condition and logs an error message to the Unity console on failure.
        //
        // 参数:
        //   condition:
        //     Condition you expect to be true.
        //
        //   context:
        //     Object to which the message applies.
        //
        //   message:
        //     String or object to be converted to string representation for display.
        [Conditional("UNITY_ASSERTIONS")]
        public static void Assert(bool condition, Object context)
        {
            if (ShowLog)
            {
                Debug.Assert(condition, context);
            }
        }
        // 摘要:
        //     Assert a condition and logs an error message to the Unity console on failure.
        //
        // 参数:
        //   condition:
        //     Condition you expect to be true.
        //
        //   context:
        //     Object to which the message applies.
        //
        //   message:
        //     String or object to be converted to string representation for display.
        [Conditional("UNITY_ASSERTIONS")]
        public static void Assert(bool condition)
        {
            if (ShowLog)
            {
                Debug.Assert(condition);
            }
        }
        [Conditional("UNITY_ASSERTIONS")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Assert(bool, string, params object[]) is obsolete. Use AssertFormat(bool, string, params object[]) (UnityUpgradable) -> AssertFormat(*)", true)]
        public static void Assert(bool condition, string format, params object[] args)
        {
            if (ShowLog)
            {
                Debug.Assert(condition, format, args);
            }
        }
        // 摘要:
        //     Assert a condition and logs a formatted error message to the Unity console on
        //     failure.
        //
        // 参数:
        //   condition:
        //     Condition you expect to be true.
        //
        //   format:
        //     A composite format string.
        //
        //   args:
        //     Format arguments.
        //
        //   context:
        //     Object to which the message applies.
        [Conditional("UNITY_ASSERTIONS")]
        public static void AssertFormat(bool condition, string format, params object[] args)
        {
            if (ShowLog)
            {
                Debug.AssertFormat(condition, format, args);
            }
        }
        // 摘要:
        //     Assert a condition and logs a formatted error message to the Unity console on
        //     failure.
        //
        // 参数:
        //   condition:
        //     Condition you expect to be true.
        //
        //   format:
        //     A composite format string.
        //
        //   args:
        //     Format arguments.
        //
        //   context:
        //     Object to which the message applies.
        [Conditional("UNITY_ASSERTIONS")]
        public static void AssertFormat(bool condition, Object context, string format, params object[] args)
        {
            if (ShowLog)
            {
                Debug.AssertFormat(condition, context, format, args);
            }
        }
        // 摘要:
        //     Pauses the editor.
        public static void Break()
        {
            if (ShowLog)
            {
                Debug.Break();
            }
        }
        // 摘要:
        //     Clears errors from the developer console.
        public static void ClearDeveloperConsole()
        {
            if (ShowLog)
            {
                Debug.ClearDeveloperConsole();
            }
        }
        public static void DebugBreak()
        {
            if (ShowLog)
            {
                Debug.DebugBreak();
            }
        }
        // 摘要:
        //     Draws a line between specified start and end points.
        //
        // 参数:
        //   start:
        //     Point in world space where the line should start.
        //
        //   end:
        //     Point in world space where the line should end.
        //
        //   color:
        //     Color of the line.
        //
        //   duration:
        //     How long the line should be visible for.
        //
        //   depthTest:
        //     Should the line be obscured by objects closer to the camera?
        [ExcludeFromDocs]
        public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration)
        {
            if (ShowLog)
            {
                Debug.DrawLine(start, end, color, duration);
            }
        }
        // 摘要:
        //     Draws a line between specified start and end points.
        //
        // 参数:
        //   start:
        //     Point in world space where the line should start.
        //
        //   end:
        //     Point in world space where the line should end.
        //
        //   color:
        //     Color of the line.
        //
        //   duration:
        //     How long the line should be visible for.
        //
        //   depthTest:
        //     Should the line be obscured by objects closer to the camera?
        [ExcludeFromDocs]
        public static void DrawLine(Vector3 start, Vector3 end, Color color)
        {
            if (ShowLog)
            {
                Debug.DrawLine(start, end, color);
            }
        }
        // 摘要:
        //     Draws a line between specified start and end points.
        //
        // 参数:
        //   start:
        //     Point in world space where the line should start.
        //
        //   end:
        //     Point in world space where the line should end.
        //
        //   color:
        //     Color of the line.
        //
        //   duration:
        //     How long the line should be visible for.
        //
        //   depthTest:
        //     Should the line be obscured by objects closer to the camera?
        public static void DrawLine(Vector3 start, Vector3 end, [Internal.DefaultValue("Color.white")] Color color, [Internal.DefaultValue("0.0f")] float duration, [Internal.DefaultValue("true")] bool depthTest)
        {
            if (ShowLog)
            {
                Debug.DrawLine(start, end, color, duration, depthTest);
            }
        }
        // 摘要:
        //     Draws a line between specified start and end points.
        //
        // 参数:
        //   start:
        //     Point in world space where the line should start.
        //
        //   end:
        //     Point in world space where the line should end.
        //
        //   color:
        //     Color of the line.
        //
        //   duration:
        //     How long the line should be visible for.
        //
        //   depthTest:
        //     Should the line be obscured by objects closer to the camera?
        [ExcludeFromDocs]
        public static void DrawLine(Vector3 start, Vector3 end)
        {
            if (ShowLog)
            {
                Debug.DrawLine(start, end);
            }
        }
        // 摘要:
        //     Draws a line from start to start + dir in world coordinates.
        //
        // 参数:
        //   start:
        //     Point in world space where the ray should start.
        //
        //   dir:
        //     Direction and length of the ray.
        //
        //   color:
        //     Color of the drawn line.
        //
        //   duration:
        //     How long the line will be visible for (in seconds).
        //
        //   depthTest:
        //     Should the line be obscured by other objects closer to the camera?
        [ExcludeFromDocs]
        public static void DrawRay(Vector3 start, Vector3 dir, Color color, float duration)
        {
            if (ShowLog)
            {
                Debug.DrawRay(start, dir, color, duration);
            }
        }
        // 摘要:
        //     Draws a line from start to start + dir in world coordinates.
        //
        // 参数:
        //   start:
        //     Point in world space where the ray should start.
        //
        //   dir:
        //     Direction and length of the ray.
        //
        //   color:
        //     Color of the drawn line.
        //
        //   duration:
        //     How long the line will be visible for (in seconds).
        //
        //   depthTest:
        //     Should the line be obscured by other objects closer to the camera?
        public static void DrawRay(Vector3 start, Vector3 dir, [Internal.DefaultValue("Color.white")] Color color, [Internal.DefaultValue("0.0f")] float duration, [Internal.DefaultValue("true")] bool depthTest)
        {
            if (ShowLog)
            {
                Debug.DrawRay(start, dir, color, duration, depthTest);
            }
        }
        // 摘要:
        //     Draws a line from start to start + dir in world coordinates.
        //
        // 参数:
        //   start:
        //     Point in world space where the ray should start.
        //
        //   dir:
        //     Direction and length of the ray.
        //
        //   color:
        //     Color of the drawn line.
        //
        //   duration:
        //     How long the line will be visible for (in seconds).
        //
        //   depthTest:
        //     Should the line be obscured by other objects closer to the camera?
        [ExcludeFromDocs]
        public static void DrawRay(Vector3 start, Vector3 dir)
        {
            if (ShowLog)
            {
                Debug.DrawRay(start, dir);
            }
        }
        // 摘要:
        //     Draws a line from start to start + dir in world coordinates.
        //
        // 参数:
        //   start:
        //     Point in world space where the ray should start.
        //
        //   dir:
        //     Direction and length of the ray.
        //
        //   color:
        //     Color of the drawn line.
        //
        //   duration:
        //     How long the line will be visible for (in seconds).
        //
        //   depthTest:
        //     Should the line be obscured by other objects closer to the camera?
        [ExcludeFromDocs]
        public static void DrawRay(Vector3 start, Vector3 dir, Color color)
        {
            if (ShowLog)
            {
                Debug.DrawRay(start, dir, color);
            }
        }
        // 摘要:
        //     Logs message to the Unity Console.
        //
        // 参数:
        //   message:
        //     String or object to be converted to string representation for display.
        //
        //   context:
        //     Object to which the message applies.
        public static void Log(object message, Object context)
        {
            if (ShowLog)
            {
                Debug.Log(message, context);
            }
        }
        // 摘要:
        //     Logs message to the Unity Console.
        //
        // 参数:
        //   message:
        //     String or object to be converted to string representation for display.
        //
        //   context:
        //     Object to which the message applies.
        public static void Log(object message)
        {
            if (ShowLog)
            {
                Debug.Log(message);
            }
        }
        // 摘要:
        //     A variant of Debug.Log that logs an assertion message to the console.
        //
        // 参数:
        //   message:
        //     String or object to be converted to string representation for display.
        //
        //   context:
        //     Object to which the message applies.
        [Conditional("UNITY_ASSERTIONS")]
        public static void LogAssertion(object message, Object context)
        {
            if (ShowLog)
            {
                Debug.LogAssertion(message, context);
            }
        }
        // 摘要:
        //     A variant of Debug.Log that logs an assertion message to the console.
        //
        // 参数:
        //   message:
        //     String or object to be converted to string representation for display.
        //
        //   context:
        //     Object to which the message applies.
        [Conditional("UNITY_ASSERTIONS")]
        public static void LogAssertion(object message)
        {
            if (ShowLog)
            {
                Debug.LogAssertion(message);
            }
        }
        // 摘要:
        //     Logs a formatted assertion message to the Unity console.
        //
        // 参数:
        //   format:
        //     A composite format string.
        //
        //   args:
        //     Format arguments.
        //
        //   context:
        //     Object to which the message applies.
        [Conditional("UNITY_ASSERTIONS")]
        public static void LogAssertionFormat(Object context, string format, params object[] args)
        {
            if (ShowLog)
            {
                Debug.LogAssertionFormat(context, format, args);
            }
        }
        // 摘要:
        //     Logs a formatted assertion message to the Unity console.
        //
        // 参数:
        //   format:
        //     A composite format string.
        //
        //   args:
        //     Format arguments.
        //
        //   context:
        //     Object to which the message applies.
        [Conditional("UNITY_ASSERTIONS")]
        public static void LogAssertionFormat(string format, params object[] args)
        {
            if (ShowLog)
            {
                Debug.LogAssertionFormat(format, args);
            }
        }
        // 摘要:
        //     A variant of Debug.Log that logs an error message to the console.
        //
        // 参数:
        //   message:
        //     String or object to be converted to string representation for display.
        //
        //   context:
        //     Object to which the message applies.
        public static void LogError(object message, Object context)
        {
            if (ShowLog)
            {
                Debug.LogError(message, context);
            }
        }
        // 摘要:
        //     A variant of Debug.Log that logs an error message to the console.
        //
        // 参数:
        //   message:
        //     String or object to be converted to string representation for display.
        //
        //   context:
        //     Object to which the message applies.
        public static void LogError(object message)
        {
            if (ShowLog)
            {
                Debug.LogError(message);
            }
        }
        // 摘要:
        //     Logs a formatted error message to the Unity console.
        //
        // 参数:
        //   format:
        //     A composite format string.
        //
        //   args:
        //     Format arguments.
        //
        //   context:
        //     Object to which the message applies.
        public static void LogErrorFormat(string format, params object[] args)
        {
            if (ShowLog)
            {
                Debug.LogErrorFormat(format, args);
            }
        }
        // 摘要:
        //     Logs a formatted error message to the Unity console.
        //
        // 参数:
        //   format:
        //     A composite format string.
        //
        //   args:
        //     Format arguments.
        //
        //   context:
        //     Object to which the message applies.
        public static void LogErrorFormat(Object context, string format, params object[] args)
        {
            if (ShowLog)
            {
                Debug.LogErrorFormat(context, format, args);
            }
        }
        // 摘要:
        //     A variant of Debug.Log that logs an error message to the console.
        //
        // 参数:
        //   context:
        //     Object to which the message applies.
        //
        //   exception:
        //     Runtime Exception.
        public static void LogException(Exception exception, Object context)
        {
            if (ShowLog)
            {
                Debug.LogException(exception, context);
            }
        }
        // 摘要:
        //     A variant of Debug.Log that logs an error message to the console.
        //
        // 参数:
        //   context:
        //     Object to which the message applies.
        //
        //   exception:
        //     Runtime Exception.
        public static void LogException(Exception exception)
        {
            if (ShowLog)
            {
                Debug.LogException(exception);
            }
        }
        // 摘要:
        //     Logs a formatted message to the Unity Console.
        //
        // 参数:
        //   format:
        //     A composite format string.
        //
        //   args:
        //     Format arguments.
        //
        //   context:
        //     Object to which the message applies.
        public static void LogFormat(Object context, string format, params object[] args)
        {
            if (ShowLog)
            {
                Debug.LogFormat(context, format, args);
            }
        }
        // 摘要:
        //     Logs a formatted message to the Unity Console.
        //
        // 参数:
        //   format:
        //     A composite format string.
        //
        //   args:
        //     Format arguments.
        //
        //   context:
        //     Object to which the message applies.
        public static void LogFormat(string format, params object[] args)
        {
            if (ShowLog)
            {
                Debug.LogFormat(format, args);
            }
        }
        // 摘要:
        //     A variant of Debug.Log that logs a warning message to the console.
        //
        // 参数:
        //   message:
        //     String or object to be converted to string representation for display.
        //
        //   context:
        //     Object to which the message applies.
        public static void LogWarning(object message)
        {
            if (ShowLog)
            {
                Debug.LogWarning(message);
            }
        }
        // 摘要:
        //     A variant of Debug.Log that logs a warning message to the console.
        //
        // 参数:
        //   message:
        //     String or object to be converted to string representation for display.
        //
        //   context:
        //     Object to which the message applies.
        public static void LogWarning(object message, Object context)
        {
            if (ShowLog)
            {
                Debug.LogWarning(message, context);
            }
        }
        // 摘要:
        //     Logs a formatted warning message to the Unity Console.
        //
        // 参数:
        //   format:
        //     A composite format string.
        //
        //   args:
        //     Format arguments.
        //
        //   context:
        //     Object to which the message applies.
        public static void LogWarningFormat(string format, params object[] args)
        {
            if (ShowLog)
            {
                Debug.LogWarningFormat(format, args);
            }
        }
        // 摘要:
        //     Logs a formatted warning message to the Unity Console.
        //
        // 参数:
        //   format:
        //     A composite format string.
        //
        //   args:
        //     Format arguments.
        //
        //   context:
        //     Object to which the message applies.
        public static void LogWarningFormat(Object context, string format, params object[] args)
        {
            if (ShowLog)
            {
                Debug.LogWarningFormat(context, format, args);
            }
        }
    }
}