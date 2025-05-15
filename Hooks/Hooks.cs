using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Reqnroll;
using Reqnroll.BoDi;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using System.Text.Json;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using qa_dotnet_cucumber.Config;
using qa_dotnet_cucumber.Pages;

namespace qa_dotnet_cucumber.Hooks
{
    [Binding]
    public class Hooks
    {
        private readonly IObjectContainer _objectContainer;
        private static ExtentReports? _extent;
        private static ExtentSparkReporter? _htmlReporter;
        private static TestSettings _settings;
        private ExtentTest? _test;
        private static readonly object _reportLock = new object();
        private IWebDriver _driver;

        public static TestSettings Settings => _settings;

        public Hooks(IObjectContainer objectContainer)
        {
            _objectContainer = objectContainer;
        }

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            string currentDir = Directory.GetCurrentDirectory();
            string settingsPath = Path.Combine(currentDir, "settings.json");
            string json = File.ReadAllText(settingsPath);
            _settings = JsonSerializer.Deserialize<TestSettings>(json);

            // Get project root by navigating up from bin/Debug/net8.0
            string projectRoot = Path.GetFullPath(Path.Combine(currentDir, "..", ".."));
            string reportFileName = _settings.Report.Path.TrimStart('/'); // e.g., "TestReport.html"
            string reportPath = Path.Combine(projectRoot, reportFileName);

            _htmlReporter = new ExtentSparkReporter(reportPath);
            _extent = new ExtentReports();
            _extent.AttachReporter(_htmlReporter);
            _extent.AddSystemInfo("Environment", _settings.Environment.BaseUrl);
            _extent.AddSystemInfo("Browser", _settings.Browser.Type);
            Console.WriteLine($"BeforeTestRun started at {DateTime.Now}, Report Path: {reportPath}");
        }

        [BeforeScenario]
        public void BeforeScenario(ScenarioContext scenarioContext)
        {
            Console.WriteLine("Debugger launched.");
            Console.WriteLine($"Starting {scenarioContext.ScenarioInfo.Title} on Thread {Thread.CurrentThread.ManagedThreadId} at {DateTime.Now}");

            switch (_settings.Browser.Type.ToLower())     //Check the browser type which is in the settings.json
            {
                case "chrome":
                    new DriverManager().SetUpDriver(new ChromeConfig());  //download chrome driver
                    var chromeOptions = new ChromeOptions();    //chrome options for headless mode execution
                    if (_settings.Browser.Headless)
                    {
                        chromeOptions.AddArgument("--headless");
                    }
                    _driver = new ChromeDriver(chromeOptions);
                    break;

                case "firefox":
                    new DriverManager().SetUpDriver(new FirefoxConfig());
                    var firefoxOptions = new FirefoxOptions();
                    if (_settings.Browser.Headless)
                    {
                        firefoxOptions.AddArgument("--headless");
                    }
                    _driver = new FirefoxDriver(firefoxOptions);
                    break;

                case "edge":
                    new DriverManager().SetUpDriver(new EdgeConfig());
                    var edgeOptions = new EdgeOptions();
                    if (_settings.Browser.Headless)
                    {
                        edgeOptions.AddArgument("--headless");
                    }
                    _driver = new EdgeDriver();
                    break;

                default:
                    throw new ArgumentException($"Unsupported Browser:{_settings.Browser.Type}");
            }
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(_settings.Browser.TimeoutSeconds);
            _driver.Manage().Window.Maximize();

            _objectContainer.RegisterInstanceAs<IWebDriver>(_driver);
            _objectContainer.RegisterInstanceAs(new NavigationHelper(_driver));
            _objectContainer.RegisterInstanceAs(new LoginPage(_driver));
            _objectContainer.RegisterInstanceAs(new LanguagePage(_driver));
            _objectContainer.RegisterInstanceAs(new SkillPage(_driver));

            lock (_reportLock)
            {
                _test = _extent!.CreateTest(scenarioContext.ScenarioInfo.Title);
            }
            Console.WriteLine($"Created test: {scenarioContext.ScenarioInfo.Title} on Thread {Thread.CurrentThread.ManagedThreadId} at {DateTime.Now}");
        }

        [AfterStep]
        public void AfterStep(ScenarioContext scenarioContext)
        {
            var stepType = scenarioContext.StepContext.StepInfo.StepDefinitionType.ToString();
            var stepText = scenarioContext.StepContext.StepInfo.Text;
            lock (_reportLock)
            {
                if (scenarioContext.TestError == null)
                {
                    _test!.Log(Status.Pass, $"{stepType} {stepText}");
                    Console.WriteLine($"Logged pass: {stepType} {stepText} on Thread {Thread.CurrentThread.ManagedThreadId} at {DateTime.Now}");
                }
                else
                {
                    var driver = _objectContainer.Resolve<IWebDriver>();
                    var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
                    var screenshotPath = Path.Combine(Directory.GetCurrentDirectory(), $"Screenshot_{DateTime.Now.Ticks}_{Thread.CurrentThread.ManagedThreadId}.png");
                    screenshot.SaveAsFile(screenshotPath);
                    _test!.Log(Status.Fail, $"{stepType} {stepText}", MediaEntityBuilder.CreateScreenCaptureFromPath(screenshotPath).Build());
                    Console.WriteLine($"Logged fail with screenshot: {screenshotPath} on Thread {Thread.CurrentThread.ManagedThreadId} at {DateTime.Now}");
                }
            }
        }

        [AfterScenario]
        public void CleanUpAfterScenario(ScenarioContext scenarioContext, FeatureContext featureContext)
        {
            try
            {
                try
                {
                    if (featureContext.FeatureInfo.Tags.Contains("language"))     //Check if the feature tag name is language
                    {
                        if (scenarioContext.TryGetValue("LanguagesToCleanup", out List<string>? languages))   //Get the value of "LanguagesToCleanup" 
                        {
                            if (languages != null && languages.Any())        //Check if the languages is not null and it has any value
                            {
                                var languagePage = _objectContainer.Resolve<LanguagePage>();   //retrieve the language page
                                foreach (var language in languages)  //Delete the languages after each scenario which we've given as input
                                {
                                    languagePage.DeleteSpecificLanguage(language);
                                }
                                Console.WriteLine($"Deleted {languages.Count} languages for this scenario");  //Check the count of languages deleted
                            }
                            else
                            {
                                Console.WriteLine("Clean up skipped: Language list is empty."); //Clean up skipped
                            }
                        }
                    }
                    else if (featureContext.FeatureInfo.Tags.Contains("skill"))  //Check if the feature tag name is skill
                    {
                        if (scenarioContext.TryGetValue("SkillsToCleanup", out List<string>? skills))  //Get the value of "SkillsToCleanup"
                        {
                            if (skills != null && skills.Any())
                            {
                                var skillsPage = _objectContainer.Resolve<SkillPage>();
                                foreach (var skill in skills)   //Delete the skill after each scenario 
                                {
                                    skillsPage.DeleteSpecificSkill(skill);
                                }
                                Console.WriteLine($"Deleted {skills.Count} skills for this scenario");
                            }
                            else
                            {
                                Console.WriteLine("Clean up skipped: Skill list is empty");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Clean up failed:{ex.Message}");
                }

                var driver = _objectContainer.Resolve<IWebDriver>();
                if (driver != null)
                {
                    Console.WriteLine($"Cleaning up on Thread{Thread.CurrentThread.ManagedThreadId} at {DateTime.Now}");
                    driver?.Quit();
                    driver?.Dispose();
                    Console.WriteLine($"Finished scenario on Thread {Thread.CurrentThread.ManagedThreadId} at {DateTime.Now}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Clean up failed:{ex.Message}");
            }
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            lock (_reportLock)
            {
                Console.WriteLine("AfterTestRun executed - Flushing report to: " + _settings.Report.Path + " at " + DateTime.Now);
                _extent!.Flush();
            }
        }
    }
}