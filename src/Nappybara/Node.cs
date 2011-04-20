using System;
using OpenQA.Selenium;

namespace Nappybara
{
    public class Node
    {
        private readonly Driver driver;

        public Node(Driver driver)
        {
            this.driver = driver;
        }

        public string Id { get; set; }
        public string Text { get; set; }
        public IWebElement UnderlyingNode { get; set; }

        public void Click()
        {
            driver.Click(this);
        }
    }
}