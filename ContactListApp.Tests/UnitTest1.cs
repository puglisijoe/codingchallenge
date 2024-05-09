using System;
using ContactListApp.Tests;
using FluentAssertions;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using ExpectedConditions = SeleniumExtras.WaitHelpers.ExpectedConditions;

namespace ContactListApp.Tests {
    public class Util {
        public string Title => GetDriver.Title;
        public IWebDriver GetDriver { get; private set; }

        public void Init_Browser() {
            GetDriver = new ChromeDriver();
            GetDriver.Manage().Window.Maximize();
        }

        public void Goto(string url) {
            GetDriver.Url = url;
        }

        public void Close() {
            GetDriver.Quit();
        }

        public IWebElement WaitForElementVisible(By locator) {
            WebDriverWait wait = new WebDriverWait(GetDriver, TimeSpan.FromSeconds(10));
            IWebElement element = wait.Until(ExpectedConditions.ElementIsVisible(locator));
            return element;
        }

        public bool ClickElement(By locator) {
            bool returnValue = false;
            try {
                WaitForElementVisible(locator).Click();
                returnValue = true;
            }
            catch (NoSuchElementException e) {
                TestContext.WriteLine("Element " + locator + "not found on page " + GetDriver.Title);
                returnValue = false;
            }
            catch (Exception e) {
                TestContext.WriteLine("Unknown error " + e.Message + " occurred on page " + GetDriver.Title);
                returnValue = false;
            }

            return returnValue;
        }

        public bool IsElementVisible(By locator) {
            bool returnValue = false;
            try {
                returnValue = WaitForElementVisible(locator).Displayed;
            }
            catch (NoSuchElementException e) {
                TestContext.WriteLine("Element " + locator + "not found on page " + GetDriver.Title);
                returnValue = false;
            }
            catch (Exception e) {
                TestContext.WriteLine("Unknown error " + e.Message + " occurred on page " + GetDriver.Title);
                returnValue = false;
            }

            return returnValue;
        }
    }
}

public class Tests {
    private const string test_url = "https://www.duckduckgo.com";
    private readonly Util util = new Util();
    private IWebDriver _driver;

    private readonly By pageImageClass =
        By.XPath(
            "/html/body/div/main/article/div[1]/div[1]/div[2]/div/header/div/section[3]/nav/ul/li/button/svg/path");

    [SetUp]
    public void Setup() {
        util.Init_Browser();
    }

    [Test]
    public void Test1() {
        Assert.Pass();
    }


    [Test]
    public void TestHomePage() {
        TestContext.WriteLine(string.Format("Launching App {0}", test_url));
        util.Goto(test_url);

        isHomePageLoaded().Should().BeTrue();
        TestContext.WriteLine("App is launched successfully");
    }


    private bool isHomePageLoaded() {
        return util.ClickElement(pageImageClass);
    }
}