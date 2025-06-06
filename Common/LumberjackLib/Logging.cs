﻿namespace Lumberjack.Interface
{
    public class Logging
    {
        private static Logging m_instance;

        public static Logging Instance 
        {
            get 
            {
                if (m_instance == null) 
                    m_instance = new Logging();

                return m_instance;
            }
        }

        private List<ILoggingChannel> m_loggingChannels = new List<ILoggingChannel>();

        protected Logging()
        {

        }

        public void RegisterChannel(ILoggingChannel channel)
        {
            m_loggingChannels.Add(channel);
        }

        public void UnregisterChannel(ILoggingChannel channel)
        {
            if (m_loggingChannels.Contains(channel))
            {
                channel.Close();
                m_loggingChannels.Remove(channel);
            }
        }

        public void LogMessage(LogLevel level, string message)
        {
            LogMessage(level, message, string.Empty);
        }

        public void LogMessage(LogLevel level, string message, string component)
        {
            foreach (ILoggingChannel channel in m_loggingChannels)
            {
                channel.LogMessage(level, message, component);
            }
        }

        public void CloseAllLogs()
        {
            foreach (ILoggingChannel channel in m_loggingChannels)
            {
                channel.Close();
            }

            m_loggingChannels.Clear();
        }

        public static void LogUserMessage(string message)
        {
            LogUserMessage(message, string.Empty);
        }

        public static void LogUserMessage(string message, string component)
        {
            Instance.LogMessage(LogLevel.UserInfo, message, component);
        }

        public static void LogInfo(string message)
        {
            LogInfo(message, string.Empty);
        }

        public static void LogInfo(string message, string component)
        {
            Instance.LogMessage(LogLevel.Info, message, component);
        }

        public static void LogWarning(string message)
        {
            LogWarning(message, string.Empty);
        }

        public static void LogWarning(string message, string component)
        {
            Instance.LogMessage(LogLevel.Warning, message, component);
        }

        public static void LogError(string message)
        {
            LogError(message, string.Empty);
        }

        public static void LogError(string message, string component)
        {
            Instance.LogMessage(LogLevel.Error, message, component);
        }

        public static void Close()
        {
            Instance.CloseAllLogs();

        }

        public static ILoggingChannel CreateLogFile(string filepath, LogLevel filter = LogLevel.All)
        {
            return CreateLogFile(filepath, string.Empty, filter);
        }

        public static ILoggingChannel CreateLogFile(string filepath, string component, LogLevel filter = LogLevel.All)
        {
            LogChannelFile logChannel = new LogChannelFile(filepath, filter);
            logChannel.ComponentFilter = component;

            Instance.RegisterChannel(logChannel);
            return logChannel;
        }

        public static ILoggingChannel CreateConsoleLog(LogLevel filter = LogLevel.UserInfo)
        {
            LogChannelConsole logChannel = new LogChannelConsole(filter);
            Instance.RegisterChannel(logChannel);

            return logChannel;
        }

    }
}
