using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using LambdSeleniumTest.TestCases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LambdSeleniumTest.PageObjects
{
    class SeleniumTestPO : SeleniumBase
    {
        //Login elements
        public By UsernameInputField = By.Id("username");
        public By PwdInputField = By.Name("password");
        public By LoginButton = By.ClassName("applynow");
        public By CookiesButton = By.XPath("//a[contains(text(),'I ACCEPT')]");
        //Elements to Populate data after Login
        public By YourEmailTextField = By.XPath("//input[@id='developer-name']");
        public By PopulateButton = By.XPath("//input[@id='populate']");
        //Elements for Feedback Form
        public By OnceAYear_RadioButton = By.XPath("//input[@value='Once a year']");
        public By CustomerService_Checkbox = By.XPath("//input[@id='customer-service']");
        public By Select_Payment = By.XPath("//select[@id='preferred-payment']");
        public By ECommerce_Checkbox = By.XPath("//input[@id='tried-ecom']");
        public By Slider = By.XPath("//div[contains(@class,'ui-slider-horizontal')]");
        public By Feedback_textfield = By.XPath("//textarea[@id='comments']");
        public By SliderVal = By.XPath("//div[contains(@class,'ui-slider-horizontal')]//span");
        public By CICDHeader = By.XPath("//h2[contains(text(),'Integrations With CI/CD Tools')]");
        public By CICDJenkins = By.XPath("//img[@alt='LambdaTest Jenkins integration']");
        public By UploadButton = By.XPath("//div[@class='upload-img']//following-sibling::label");
        public By SubmitButton = By.XPath("//button[@id='submit-button']");
        public By ThankYouMsg = By.XPath("//h1[contains(text(),'Thank you!')]");
        public By FinalMsg = By.XPath("//p[contains(text(),'You have successfully submitted the form.')]");

        public static WebDriverWait Wait;

        //Login with the Username and password
        public void SignInInsideLambdaTest(IWebDriver driver, string userName, string pwd)
        {
            if (IsElementPresent(driver, CookiesButton))
            {
                clickWithWait(driver, CookiesButton);
                WaitForJQueryToLoad(driver);
            }
            waitForElementToBeVisible(driver, UsernameInputField, 60);
            SendKeys(driver, UsernameInputField, userName, true);
            SendKeys(driver, PwdInputField, pwd, true);
            // ScrollElementIntoView(driver, LoginButton);
            clickWithWait(driver, LoginButton);
            WaitForJQueryToLoad(driver);

        }
        public void FillingFeedbackData(IWebDriver driver)
        {
            waitUntilElementExists(driver, OnceAYear_RadioButton, 60);
            //ScrollElementIntoView(driver, OnceAYear_RadioButton);
            clickWithWait(driver, OnceAYear_RadioButton);
            clickWithWait(driver, CustomerService_Checkbox);
            clickWithWait(driver, ECommerce_Checkbox);
            SelectValueFromDropdownByIndex(driver, Select_Payment, 1);
            Actions action = new Actions(driver);
            action.ClickAndHold(driver.FindElement(Slider));
            action.MoveByOffset(200, 0).Build().Perform();
            waitUntilElementExists(driver, Feedback_textfield, 60);
            SendKeys(driver, Feedback_textfield, "My Assignment for the Certification", true);

        }
        public void WaitForTitleIs(IWebDriver driver, string title, int Time)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(Time));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.TitleIs(title));
        }
        //Method for Populating Data
        public void PopulatingData(IWebDriver driver, string email, string Title)
        {
            waitForTitleIs(driver, Title, 120);
            SendKeys(driver, YourEmailTextField, email, true);
            clickWithWait(driver, PopulateButton);
        }

    }
}
