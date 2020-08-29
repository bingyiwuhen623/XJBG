using UnityEngine;
using System;
using System.IO;
using System.Diagnostics;
using System.Collections;

public class ExceptionDispose : MonoBehaviour
{
    // 是否作为异常处理者
    public bool IsHandler => GameDefine.IS_EXCEPTION_HANDLER;
    // 当异常发生时是否退出程序
    public bool IsQuitWhenException => GameDefine.IS_QUIT_WHEN_EXCEPTION;

    void Awake()
    {
        // 注册异常处理委托
        if (IsHandler)
        {
#pragma warning disable CS0618 // 类型或成员已过时
            Application.RegisterLogCallback(Handler);
#pragma warning restore CS0618 // 类型或成员已过时
        }
    }

    void OnDestory()
    {
#pragma warning disable CS0618 // 类型或成员已过时
        // 清除注册
        Application.RegisterLogCallback(null);
#pragma warning restore CS0618 // 类型或成员已过时
    }

    void Handler(string logString, string stackTrace, LogType type)
    {
        if (type == LogType.Error || type == LogType.Exception || type == LogType.Assert)
        {
            //string logPath = LogPath + "\" + DateTime.Now.ToString( "yyyy_MM_dd HH_mm_ss" ) + ".log";
            //// 打印日志
            //if (Directory.Exists(LogPath))
            //{
            //    File.AppendAllText(logPath, "[time]:" + DateTime.Now.ToString() + "rn");
            //    File.AppendAllText(logPath, "[type]:" + type.ToString() + "rn");
            //    File.AppendAllText(logPath, "[exception message]:" + logString + "rn");
            //    File.AppendAllText(logPath, "[stack trace]:" + stackTrace + "rn");
            //}
            //// 启动bug反馈程序
            //if (File.Exists(BugExePath))
            //{
            //    ProcessStartInfo pros = new ProcessStartInfo();
            //    pros.FileName = BugExePath;
            //    pros.Arguments = """ + logPath + """;
            //    Process pro = new Process();
            //    pro.StartInfo = pros;
            //    pro.Start();
            //}
            // 退出程序
            if (IsQuitWhenException)
            {
                Application.Quit();
            }
        }
    }
}