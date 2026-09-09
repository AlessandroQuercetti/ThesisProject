using System;
using System.IO;
using UnityEngine;

public class FileLogger : MonoBehaviour
{
    private StreamWriter logWriter;
    private string logFilePath;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        try
        {
            string logDirectory = Path.Combine(Application.persistentDataPath, "Logs");

            if (!Directory.Exists(logDirectory))
                Directory.CreateDirectory(logDirectory);

            string fileName = $"log_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.txt";

            logFilePath = Path.Combine(logDirectory, fileName);

            logWriter = new StreamWriter(logFilePath, true);
            logWriter.AutoFlush = true;

            Application.logMessageReceived += HandleLog;


            logWriter.WriteLine("========================================");
            logWriter.WriteLine($"SESSION START {DateTime.Now}");
            logWriter.WriteLine("========================================");

            Debug.Log($"Version: {Application.version}");
            Debug.Log($"Platform: {Application.platform}");

            Debug.Log($"Logger started. File: {logFilePath}");

        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to initialize logger: {e}");
        }
    }

    private void HandleLog(string logString, string stackTrace, LogType type)
    {
        if (logWriter == null)
            return;

        try
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");

            logWriter.WriteLine($"[{timestamp}] [{type}] {logString}");

            if (type == LogType.Error ||
                type == LogType.Exception ||
                type == LogType.Assert)
            {
                logWriter.WriteLine(stackTrace);
            }
        }
        catch
        {
            // avoid log loop in case of errors while writing. 
        }
    }

    private void OnApplicationQuit()
    {
        Cleanup();
    }

    private void OnDestroy()
    {
        Cleanup();
    }

    private void Cleanup()
    {
        Application.logMessageReceived -= HandleLog;

        if (logWriter != null)
        {
            logWriter.Flush();
            logWriter.Close();
            logWriter = null;
        }
    }
}