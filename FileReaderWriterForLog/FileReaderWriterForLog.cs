using System;
using System.IO;

namespace FileReaderWriterForLog
{

    /// <summary>Log Base</summary>
    abstract public class LogBase : FileReaderWriter.FileReaderWriter
    {
        /// <summary>Write</summary>
        /// <param name="msg">message</param>
        /// <param name="bAppend">Append or Create</param>
        /// <returns>success</returns>
        new protected bool Write(string msg, bool bAppend, bool startWithDateTime = false)
        {
            if (startWithDateTime)
            {
                msg = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff\t") + msg;
            }
            return base.Write(msg, bAppend);
        }

        /// <summary>WriteStartWithDateTime</summary>
        /// <param name="msg">message</param>
        /// <param name="bAppend">Append or Create</param>
        /// <returns>success</returns>
        protected bool WriteStartWithDateTime(string msg, bool bAppend)
        {
            return Write(msg, bAppend, true);
        }
    }

    /// <summary>Log Monthly </summary>
    abstract public class LogMonthly : LogBase
    {
        /// <summary>Check Last Write Time Is Same Month </summary>
        /// <param name="path">Full File Path</param>
        /// <returns>true : SameMonth ; false : not SameMonth or file not found</returns>
        private bool CheckLastWriteTimeIsSameMonth(string path)
        {
            if (File.Exists(path))
            {
                FileInfo FI = new FileInfo(path);

                //相同即為同一月
                bool bSameMonth = string.Compare(FI.LastWriteTime.ToString("yyyyMM"), DateTime.Now.ToString("yyyyMM")) == 0;

                return bSameMonth;
            }
            return false;
        }

        /// <summary>Write</summary>
        /// <param name="msg">message</param>
        public void Write(string msg)
        {
            bool bAppend = CheckLastWriteTimeIsSameMonth(FullFilePath);
            base.WriteStartWithDateTime(msg, bAppend);
        }

    }

    /// <summary>Log Daily </summary>
    abstract public class LogDaily : LogBase
    {
        /// <summary>Check Last Write Time Is Today For </summary>
        /// <param name="path">Full File Path</param>
        /// <returns>true : is today ; false : not today or file not found</returns>
        private bool CheckLastWriteTimeIsToday(string path)
        {
            if (File.Exists(path))
            {
                FileInfo FI = new FileInfo(path);

                //ToShortDateString為日期,相同即為同一天
                bool bToday = string.Compare(FI.LastWriteTime.ToShortDateString(), DateTime.Now.ToShortDateString()) == 0;

                return bToday;
            }
            return false;
        }

        /// <summary>Write</summary>
        /// <param name="msg">message</param>
        /// <returns>success</returns>
        public bool Write(string msg)
        {
            bool bAppend = CheckLastWriteTimeIsToday(FullFilePath);
            return base.WriteStartWithDateTime(msg, bAppend);
        }

    }
}
