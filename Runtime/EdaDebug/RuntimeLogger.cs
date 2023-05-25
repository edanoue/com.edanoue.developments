// Copyright Edanoue, Inc. All Rights Reserved.

#nullable enable
using System;
using System.Runtime.CompilerServices;
using Edanoue.Developments.Internal;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Edanoue
{
    partial class EdaDebug
    {
        private static EdaDebugConsoleRuntimeDrawer? _consoleDrawer;

        /// <summary>
        /// Get default runtime logger.
        /// </summary>
        public static ILogger RuntimeLogger
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
        } = new Logger(new EdaRuntimeLogHandler());

        private static bool CheckConsoleDrawer()
        {
            if (_consoleDrawer != null)
            {
                return true;
            }

            _consoleDrawer = Object.FindFirstObjectByType<EdaDebugConsoleRuntimeDrawer>();
            return _consoleDrawer != null;
        }

        private static void LogRuntime(string text)
        {
            if (CheckConsoleDrawer())
            {
                _consoleDrawer!.RegisterText(text);
            }
        }

        public sealed class EdaRuntimeLogHandler : ILogHandler
        {
            void ILogHandler.LogFormat(LogType logType, Object context, string format, params object[] args)
            {
                LogRuntime($"[{logType}] {string.Format(format, args)}");
                Debug.unityLogger.LogFormat(logType, context, format, args);
            }

            void ILogHandler.LogException(Exception exception, Object context)
            {
                Debug.unityLogger.LogException(exception, context);
            }
        }
    }
}