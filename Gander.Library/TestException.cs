using System;

namespace Gander.Library
{
    public class TestException : Exception
    {
        private readonly Test _test;
        private readonly string _browserName;
        private readonly long _elapsedTime;

        public TestException(Test test, string browserName, long elapsedTime, string message) : base(message)
        {
            _test = test;
            _browserName = browserName;
            _elapsedTime = elapsedTime;
        }

        public Test Test { get { return _test; } }

        public string BrowserName { get { return _browserName; } }

        /// <summary>
        /// Milleseconds elapsed
        /// </summary>
        public long ElapsedTime { get { return _elapsedTime; } }
    }
}