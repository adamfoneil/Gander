using OpenQA.Selenium;

namespace Gander.Library
{
    public abstract class TestStep
    {
        public void Execute(string role, IWebDriver driver, Application application, Environment environment)
        {
            OnExecute(role, driver, application, environment);
            
            foreach (var a in Assertions)
            {
                bool result = a.Evaluate(application, environment);
                if (!result) throw new TestException(this, driver.GetType().Name, a.FailMessage);
            }
            
        }

        protected abstract void OnExecute(string role, IWebDriver driver, Application application, Environment environment);

        public Assertion[] Assertions { get; set; }
    }
}