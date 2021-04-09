using System;
using System.IO;
using System.Net;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using LambdSeleniumTest.PageObjects;
using LambdSeleniumTest.TestCases;

namespace LambdSeleniumTest
{
    [TestClass]
    public class SeleniumTest : SeleniumBase
    {
        SeleniumTestPO objSelenium101PO = new SeleniumTestPO();

        [TestInitialize]
        public void TestInitiate()
        {
            AssignmentObjectsInitiation();
            StartReport();
            LaunchBrowser("browser", environment);
        }
        [TestMethod]
        public void TestScenario()
        {
            try
            {
                //Login inside Lambda
                objSelenium101PO.SignInInsideLambdaTest(driver, userName, password);
                //Populating data after sign in
                objSelenium101PO.PopulatingData(driver, Email, "Selenium Playground | LambdaTest");
                //Handling Alert Pop Up
                //string Alertpopup = driver.SwitchTo().Alert().Text;
                IAlert UserAlert = driver.SwitchTo().Alert();
                string alertText = UserAlert.Text;
                Console.WriteLine("Alert text is " + alertText);
                UserAlert.Accept();
                //Filling Feedback data
                objSelenium101PO.FillingFeedbackData(driver);
                string sliderPercent = getAttributeValue(driver, objSelenium101PO.SliderVal, "style");
                Assert.IsTrue(sliderPercent.Contains("left: 88.888"));
                //Navigating to New URL in New tab
                ((IJavaScriptExecutor)driver).ExecuteScript("window.open();");
                driver.SwitchTo().Window(driver.WindowHandles[1]);
                driver.Navigate().GoToUrl(newenvironment);
                WaitForJQueryToLoad(driver);
                if (IsElementPresent(driver, objSelenium101PO.CookiesButton))
                {
                    clickWithWait(driver, objSelenium101PO.CookiesButton);
                    WaitForJQueryToLoad(driver);
                }
                //Scrolling to Jenkins tab
                waitUntilElementExists(driver, objSelenium101PO.CICDHeader, 120);
                ScrollElementIntoView(driver, objSelenium101PO.CICDHeader);
                //downloading Jenkins Image
                string JenkinsURL = getAttributeValue(driver, objSelenium101PO.CICDJenkins, "src");
                string fileName = JenkinsURL.Split('/')[JenkinsURL.Split('/').Length - 1];
                string filePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                ((IJavaScriptExecutor)driver).ExecuteScript("window.open();");
                driver.SwitchTo().Window(driver.WindowHandles[2]);
                driver.Navigate().GoToUrl(JenkinsURL);
                WebClient webClient = new WebClient();
                webClient.DownloadFile(JenkinsURL, fileName);
                driver.SwitchTo().Window(driver.WindowHandles[0]);
                IWebElement element = driver.FindElement(By.Id("file"));
                element.SendKeys(filePath + "\\" + fileName);

                IAlert UploadAlert = driver.SwitchTo().Alert();
                string UploadAlertText = UploadAlert.Text;
                Console.WriteLine("Alert text is " + UploadAlertText);
                UploadAlert.Accept();

                string uploadedFile = element.GetAttribute("value");
                Assert.IsTrue(fileName == Path.GetFileName(uploadedFile));
                Assert.IsTrue(UploadAlertText.Contains("your image upload sucessfully!!"));
                ScrollElementIntoView(driver, objSelenium101PO.UploadButton);
                clickWithWait(driver, objSelenium101PO.SubmitButton);
                waitUntilElementExists(driver, objSelenium101PO.ThankYouMsg, 90);
                string Finalmsg = getText(driver, objSelenium101PO.FinalMsg);
                Assert.IsTrue(Finalmsg.Contains("You have successfully submitted the form."));
                driver.Close();
                driver.Quit();
            }
            catch (Exception e)
            {
                string Stepscreenshot = ScreenshotforTestStep(driver, testContextInstance, "Failed step");
                extentTest.Fail(e.StackTrace);
                Console.Out.WriteLine(e.StackTrace);
                throw e;
            }


        }
    }
}
