using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace LambdSeleniumTest.TestCases
{
   public class SeleniumBase
    {
        public TestContext testContextInstance;
        public ExtentReports extentReport;
        public ExtentTest extentTest;
        public string environment;
        public string newenvironment;
        public IWebDriver driver;
        public string browser;
        public string userName;
        public string password;
        public string Email;
        public static WebDriverWait wait;
        public void AssignmentObjectsInitiation()
        {
            environment = testContextInstance.Properties["LamdaTestURL"].ToString();
            newenvironment = testContextInstance.Properties["LamdaURL"].ToString();
            browser = testContextInstance.Properties["browser"].ToString();
            userName = testContextInstance.Properties["UserName"].ToString();
            password = testContextInstance.Properties["Password"].ToString();
            Email = testContextInstance.Properties["email"].ToString();
            //#if DEBUG
            //            siteCoreCSVpath = AutomationLibraries.RetrieveProjectFolderPath() + "\\TestData\\SitecoreAutomationTestData.csv";

            //#else
            //                        siteCoreCSVpath = AutomationLibraries.retrieveSolutionDirectory() + "\\TestData\\SitecoreAutomationTestData.csv";
            //#endif
        }

        public static void waitUntilElementExists(IWebDriver driver, By Element, int Time)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(Time));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(Element));
        }
        public static void SelectValueFromDropdownByText(IWebDriver driver, By element, String testdata)
        {
            IWebElement selectelement = driver.FindElement(element);
            SelectElement select = new SelectElement(selectelement);
            select.SelectByText(testdata);
        }
        public static void SelectValueFromDropdownByIndex(IWebDriver driver, By element, int index)
        {
            IWebElement selectelement = driver.FindElement(element);
            SelectElement select = new SelectElement(selectelement);
            select.SelectByIndex(index);

        }
        public static string getText(IWebDriver driver, By Element)
        {
            string text = driver.FindElement(Element).Text;
            return text;
        }
        public static string getAttributeValue(IWebDriver driver, By Element, string attributeName)
        {
            string attributeValue = driver.FindElement(Element).GetAttribute(attributeName);
            return attributeValue;
        }

        public static void WaitUntilAttributeValueEquals(IWebDriver driver, By Element, string attributeName, string attributeValue)
        {
            IWebElement webElement = driver.FindElement(Element);
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(120));
            wait.Until<IWebElement>((d) =>
            {
                if (webElement.GetAttribute(attributeName) == attributeValue)
                {
                    return webElement;
                }
                return null;
            });
        }
        public static void SelectOptionFromDropdownByValueAttribute(IWebDriver driver, By element, string attributeValue)
        {
            IWebElement selectelement = driver.FindElement(element);
            SelectElement select = new SelectElement(selectelement);
            select.SelectByValue(attributeValue);
        }

        public static bool isAlertPresent(IWebDriver driver)
        {
            try
            {
                driver.SwitchTo().Alert();
                return true;
            }
            catch (NoAlertPresentException ex)
            {
                return false;
            }
        }

        public static void waitUntilElementToBeClickable(IWebDriver driver, By Element, int Time)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(Time));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(Element));
        }
        public string ScreenshotforTestStep(IWebDriver driver, TestContext testContextInstance, string Stepname)
        {
            Screenshot ss = ((ITakesScreenshot)driver).GetScreenshot();
            string path = Directory.GetCurrentDirectory() + "\\" + testContextInstance.TestName + "_" + Stepname + ".png";
            ss.SaveAsFile(path);
            testContextInstance.AddResultFile(path);
            return path;
        }
        public static void waitForElementToBeVisible(IWebDriver driver, By Element, int Time)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(Time));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(Element));
        }
        public static void ExplicitWait(IWebDriver driver, IWebElement element)
        {
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(element));
        }
        public static void SendKeys(IWebDriver driver, By element, string testdata, bool clear)
        {
            WaitUntillElementIsVisible(driver, element, new TimeSpan(0, 1, 0));

            if (clear)
            {
                driver.FindElement(element).Clear();
                WaitForJQueryToLoad(driver);
                ThinkTime(2000);
                driver.FindElement(element).SendKeys(testdata);
            }
            else
            {
                driver.FindElement(element).SendKeys(testdata);
            }
        }
        public static int Depth = 1;

        public static Dictionary<int, int> CommandThinkTimes = new Dictionary<int, int>();

        public static int TotalThinkTime = 0;
        public static void ThinkTime(int milliseconds)

        {
            if (!CommandThinkTimes.ContainsKey((Depth)))

                CommandThinkTimes.Add(Depth, milliseconds);
            else if (Depth == 1)

                CommandThinkTimes[Depth] += milliseconds;
            else
                CommandThinkTimes[Depth] = milliseconds;
            TotalThinkTime += milliseconds;
            Thread.Sleep(milliseconds);
        }
        public static void clickWithWait(IWebDriver driver, By Element)
        {
            waitUntilElementToBeClickable(driver, Element, 120);
            driver.FindElement(Element).Click();
        }
        public static void waitForTitleIs(IWebDriver driver, string title, int Time)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(Time));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.TitleIs(title));
        }
        public static bool IsElementPresent(IWebDriver driver, By by)
        {
            try
            {
                Thread.Sleep(5000);
                driver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
        public static void ScrollElementIntoView(IWebDriver driver, By Element)
        {
            IWebElement scrollele = driver.FindElement(Element);
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", scrollele);
        }
        public static void WaitForJQueryToLoad(IWebDriver webDriver)
        {
            WebDriverWait wait = new WebDriverWait(webDriver, new TimeSpan(0, 2, 0));
            wait.Until(driver => (bool)((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));
            wait.Until(driver => (bool)((IJavaScriptExecutor)driver).ExecuteScript("return window.jQuery != undefined && jQuery.active == 0").Equals(true));
        }
        public static bool WaitUntillElementIsVisible(IWebDriver driver, By by, TimeSpan timeout)
        {
            WebDriverWait wait = new WebDriverWait(driver, timeout);
            bool success;

            wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(StaleElementReferenceException), typeof(InvalidOperationException));

            try
            {
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(by));

                success = true;
            }
            catch (NoSuchElementException)
            {
                success = false;
            }
            catch (WebDriverTimeoutException)
            {
                success = false;
            }
            catch (InvalidOperationException)
            {
                success = false;
            }
            return success;
        }
        //    public static void waitForTitleIs(IWebDriver driver, string title, int Time)
        //{
        //    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(Time));
        //    wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.TitleIs(title));
        //}

        public void StartReport()
        {
            try
            {
                extentReport = new ExtentReports();
                string reportpath = Directory.GetCurrentDirectory() + "\\" + testContextInstance.TestName + ".html";
                var extentHtml = new ExtentHtmlReporter(reportpath);
                extentReport.AttachReporter(extentHtml);
                extentTest = extentReport.CreateTest(testContextInstance.TestName);
                TestContext.AddResultFile(reportpath);
            }
            catch (Exception e)
            {
                Console.Out.WriteLine(e.StackTrace);
            }
        }
        public IWebDriver BrowserSelectionDriver(String browser)
        {
            switch (browser)
            {
                case "Chrome":
                    //driver = new ChromeDriver();
                    var options = new ChromeOptions();
                    options.AddArgument("--no-sandbox");
                    options.AddArgument("--incognito");
                    //driver = new ChromeDriver(options);
                    driver = new RemoteWebDriver(new Uri("https://" + "ambika1991ece" + ":" + "jHzhVeUyYcbM2uS79QfaDFTg0ln6GjFKGyUeW1kJdcIIX9cRcH" + "@hub.lambdatest.com/wd/hub"), options.ToCapabilities());

                    break;
                case "IE":
                    driver = new InternetExplorerDriver();
                    break;
                case "Firefox":
                    driver = new FirefoxDriver();
                    break;
            }
            return driver;
        }
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }
        public IWebDriver LaunchBrowser(string browser, string url)
        {
            if (driver == null)
            {
                driver = BrowserSelectionDriver(browser);
                //Start by opening LambdaTest Selenium Playground from //"https://www.lambdatest.com/automation-demos/"
                driver.Navigate().GoToUrl(url);
                driver.Manage().Window.Maximize();
            }
            return driver;
        }

        public void EndReport()
        {
            try
            {
                extentReport.Flush();
            }
            catch (Exception RuntimeBinderException)
            {
                Console.Out.WriteLine(RuntimeBinderException.StackTrace);
            }
        }
    }
}
