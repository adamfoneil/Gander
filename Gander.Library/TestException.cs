using System;

namespace Gander.Library
{
    public class TestException : Exception
    {
        private readonly TestStep _step;
        private readonly string _browserName;        

        public TestException(TestStep step, string browserName, string message) : base(message)
        {
            _step = step;
            _browserName = browserName;            
        }

        public TestStep Test { get { return _step; } }

        public long MillesecondsElapsed { get; set; }

        public string BrowserName { get { return _browserName; } }
    }
}