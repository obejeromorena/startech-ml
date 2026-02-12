using System;
using System.IO;
using System.Text;

namespace StartechML.Core.Utils
{
    public abstract class Logger
    {
        private static string? _logPath;
        private static int _lineLength = 80;

        // Modos de log
        public enum Mode
        {
            Info,
            Error
        }

        // Opciones simples
        public enum Options
        {
            Y,
            N
        }

        // Setea la carpeta donde se escriben los logs
        public static void SetLogPath(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            _logPath = path;
        }

        // Setea largo de línea (estético)
        public static void SetLogLine(int length)
        {
            _lineLength = length;
        }

        // Escribe el log
        public static void Write(string message, string showDate, string showTime, string mode)
        {
            if (string.IsNullOrEmpty(_logPath))
                throw new Exception("Logger no inicializado. Falta SetLogPath.");

            var fileName = $"Log_{DateTime.Now:yyyyMMdd}.txt";
            var filePath = Path.Combine(_logPath, fileName);

            var sb = new StringBuilder();

            if (showDate == Options.Y.ToString())
                sb.Append(DateTime.Now.ToString("yyyy-MM-dd"));

            if (showTime == Options.Y.ToString())
                sb.Append(" " + DateTime.Now.ToString("HH:mm:ss"));

            sb.Append($" | {mode.ToUpper()} | ");
            sb.Append(message);

            sb.AppendLine();
            sb.AppendLine(new string('-', _lineLength));

            File.AppendAllText(filePath, sb.ToString());
        }
    }
}
