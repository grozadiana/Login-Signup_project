using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

[TestClass]
public class BMRWebsiteTests
{
    private IWebDriver driver;

    public BMRWebsiteTests()
    { 
    }

    private string baseUrl = "https://www.calculator.net/bmr-calculator.html";

    [TestInitialize]
    public void Initialize()
    {
        driver = new ChromeDriver();
        driver.Manage().Window.Maximize();
    }

    [TestMethod]
    public void CalculateBMRWithValidData()
    {
        driver.Navigate().GoToUrl(baseUrl);

        IWebElement ageField = driver.FindElement(By.Id("cage"));
        ageField.SendKeys("25");

        IWebElement weightField = driver.FindElement(By.Id("cheightmeter"));
        weightField.SendKeys("150");

        IWebElement heightField = driver.FindElement(By.Id("ckg"));
        heightField.SendKeys("70");
    }

    [TestMethod]
    public void CalculateBMRWithInvalidData()
    {
        driver.Navigate().GoToUrl(baseUrl);

        IWebElement ageField = driver.FindElement(By.Id("cage"));
        ageField.SendKeys("invalid");

        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        IWebElement errorElement = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id='cageifcErr']")));
        Assert.IsTrue(errorElement.Text.Contains("positive numbers only"));
    }

    [TestMethod]
    public void CalculateBMRButtonClick()
    {

        driver.Navigate().GoToUrl(baseUrl);

        FillBMRForm("25", "150", "70");

        ClickCalculateButton();

    }

    [TestMethod]
    public void ClearBMRFormButtonClick()
    {
        driver.Navigate().GoToUrl(baseUrl);

        FillBMRForm("25", "150", "70");

        ClickClearButton();


    }

    [TestMethod]
    public void SelectMaleGenderRadioButton()
    {
        driver.Navigate().GoToUrl(baseUrl);

        SelectGender("m");

        Assert.IsTrue(IsGenderSelected("m"));
    }

    [TestMethod]
    public void SelectFemaleGenderRadioButton()
    {
        driver.Navigate().GoToUrl(baseUrl);

        SelectGender("f");

        Assert.IsTrue(IsGenderSelected("f"));
    }

    [TestMethod]
    public void NavigateToBodyFatCalculator()
    {
        driver.Navigate().GoToUrl(baseUrl);

        ClickBodyFatCalculatorButton();

        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
        wait.Until(ExpectedConditions.UrlContains("body-fat-calculator"));

        Assert.IsTrue(driver.Url.Contains("body-fat-calculator"));
    }

    [TestMethod]
    public void NavigateToCalorieCalculator()
    {
        driver.Navigate().GoToUrl(baseUrl);

        ClickCalorieCalculatorButton();

        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
        wait.Until(ExpectedConditions.UrlContains("calorie-calculator"));

        Assert.IsTrue(driver.Url.Contains("calorie-calculator"));
    }

    [TestMethod]
    public void ExpandSettingsContent()
    {
        driver.Navigate().GoToUrl(baseUrl);

        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
        By settingsButtonLocator = By.CssSelector("#ccsettingtitle > b > a");

        IWebElement settingsButton = wait.Until(ExpectedConditions.ElementToBeClickable(settingsButtonLocator));
        settingsButton.Click();

        By settingsContentLocator = By.Id("ccsettingcontent");
        wait.Until(ExpectedConditions.ElementIsVisible(settingsContentLocator));

        IWebElement settingsContent = driver.FindElement(settingsContentLocator);
        Assert.IsTrue(settingsContent.Displayed);
    }

    [TestMethod]
    public void SearchBarTest()
    {
        driver.Navigate().GoToUrl(baseUrl);

        string searchTerm = "Body Fat Calculator";
        IWebElement searchBar = driver.FindElement(By.Name("calcSearchTerm"));
        searchBar.SendKeys(searchTerm);

        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        By searchResultsLocator = By.CssSelector("#calcSearchOut > div:nth-child(1)");
        wait.Until(ExpectedConditions.ElementIsVisible(searchResultsLocator));

        searchBar.SendKeys(Keys.Enter);

        By searchResultsPageLocator = By.CssSelector("#calcSearchOut > div:nth-child(1)");
        wait.Until(ExpectedConditions.ElementIsVisible(searchResultsPageLocator));

        IWebElement searchResultsPage = driver.FindElement(searchResultsPageLocator);
        Assert.IsTrue(searchResultsPage.Text.Contains(searchTerm));
    }


    [TestCleanup]
    public void Cleanup()
    {
        driver.Quit();
    }

    private void FillBMRForm(string age, string weight, string height)
    {
        IWebElement ageField = driver.FindElement(By.Id("cage"));
        ageField.SendKeys(age);

        IWebElement weightField = driver.FindElement(By.Id("cheightmeter"));
        weightField.SendKeys(weight);

        IWebElement heightField = driver.FindElement(By.Id("ckg"));
        heightField.SendKeys(height);
    }

    private void ClickCalculateButton()
    {
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        IWebElement calculateButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("#content > div.leftinput > div.panel2 > form > table:nth-child(4) > tbody > tr:nth-child(2) > td > input[type=submit]:nth-child(1)")));
        calculateButton.Click();
    }

    private void ClickClearButton()
    {
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
        By clearButtonLocator = By.CssSelector("#content > div.leftinput > div.panel2 > form > table:nth-child(4) > tbody > tr:nth-child(2) > td > input[type=button]:nth-child(2)");
    }

    private void SelectGender(string gender)
    {
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
        By genderButtonLocator;
        if (gender.ToLower() == "m")
        {
            genderButtonLocator = By.XPath("//*[@id='calinputtable']/tbody/tr[2]/td[2]/label[1]/span");
        }
        else if (gender.ToLower() == "f")
        {
            genderButtonLocator = By.XPath("//*[@id='calinputtable']/tbody/tr[2]/td[2]/label[2]/span");
        }
        else
        {
            throw new ArgumentException("Invalid gender value. Use 'm' for male or 'f' for female.");
        }

        IWebElement genderButton = wait.Until(ExpectedConditions.ElementToBeClickable(genderButtonLocator));
        genderButton.Click();


    }

    private bool IsGenderSelected(string gender)
    {
        By genderButtonLocator = By.CssSelector($"input[name='csex'][value='{gender}']");
        IWebElement genderButton = driver.FindElement(genderButtonLocator);

        return genderButton.Selected;
    }

    private void ClickBodyFatCalculatorButton()
    {
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
        By bodyFatCalculatorButtonLocator = By.CssSelector("#content > fieldset > a:nth-child(2)");

        IWebElement bodyFatCalculatorButton = wait.Until(ExpectedConditions.ElementToBeClickable(bodyFatCalculatorButtonLocator));
        bodyFatCalculatorButton.Click();
    }

    private void ClickCalorieCalculatorButton()
    {
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
        By calorieCalculatorButtonLocator = By.CssSelector("#content > fieldset > a:nth-child(3)");

        IWebElement calorieCalculatorButton = wait.Until(ExpectedConditions.ElementToBeClickable(calorieCalculatorButtonLocator));
        calorieCalculatorButton.Click();
    }
}