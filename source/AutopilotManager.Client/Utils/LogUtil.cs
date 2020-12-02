using System;

namespace AutopilotManager.Utils
{
    // just a super simple logging class

    public class LogUtil
    {
        private bool _debug;
        private bool _trace;

        public LogUtil()
        {
            _debug = false;
            _trace = false;
        }

        public bool DebugMode 
        {
            get { return _debug; }
            set { _debug = value; }
        }

        public bool TraceMode
        {
            get { return _trace; }
            set { _trace = value; }
        }

        public void WriteInfo(string text)
        {
            Console.WriteLine(text);
        }

        public void WriteInfoNoLinebreak(string text)
        {
            Console.Write(text);
        }

        public void WriteDebug(string text)
        {
            if (_debug)
            {
                Console.WriteLine(text);
            }
        }

        public void WriteTrace(string text)
        {
            if (_trace)
            {
                Console.WriteLine(text);
            }
        }
    }
}
