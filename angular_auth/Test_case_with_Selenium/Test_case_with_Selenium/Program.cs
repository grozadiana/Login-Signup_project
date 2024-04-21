using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

[TestClass]
public class SinsayWebsiteTests
{
    private IWebDriver driver;

    [TestInitialize]
    public void Initialize()
    {
        // Configurare inițială, deschideți browserul
        driver = new ChromeDriver();
        driver.Manage().Window.Maximize();
    }

    [TestMethod]
    public void TestLoginAndLogout()
    {
        // Deschideți pagina de start
        driver.Navigate().GoToUrl("https://www.sinsay.com/ro/ro/");

        // Identificați elementele și completați informațiile de conectare
        IWebElement loginButton = driver.FindElement(By.CssSelector(".login-btn"));
        loginButton.Click();

        IWebElement usernameField = driver.FindElement(By.Id("login-email"));
        usernameField.SendKeys("your_username");

        IWebElement passwordField = driver.FindElement(By.Id("login-password"));
        passwordField.SendKeys("your_password");

        IWebElement submitButton = driver.FindElement(By.Id("login-button"));
        submitButton.Click();

        // Validați că sunteți conectat
        IWebElement userMenu = driver.FindElement(By.CssSelector(".user-menu"));
        Assert.IsTrue(userMenu.Displayed);

        // Efectuați acțiuni suplimentare, cum ar fi navigarea în alte secțiuni și apoi deconectarea
        // ...

        // Apelați o metodă pentru deconectare
        Logout();

        // Validați că sunteți deconectat
        Assert.IsTrue(loginButton.Displayed);
    }

    private void Logout()
    {
        // Implementați acțiunile pentru deconectare
        // De exemplu, faceți clic pe butonul de deconectare și validați că sunteți redirecționat la pagina de conectare sau acasă.
    }

    [TestCleanup]
    public void Cleanup()
    {
        // Închideți browserul după fiecare test
        driver.Quit();
    }
}
