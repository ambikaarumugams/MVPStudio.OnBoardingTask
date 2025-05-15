using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using String = System.String;

namespace qa_dotnet_cucumber.Pages
{
    public class SkillPage
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;
      //  public IWebDriver Driver { get { return _driver; } }

        public SkillPage(IWebDriver driver)  //Inject the driver directly
        {
            _driver = driver;
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));
        }

        //Locators
        private readonly By _profileTab = By.XPath("//a[normalize-space()='Profile']");
        private readonly By _skillsTab = By.XPath("//a[normalize-space()='Skills']");

        //Add New Skills
        private readonly By _skillsTable = By.XPath("//table[@class='ui fixed table'][.//th[normalize-space(text())='Skill']]");
        //private readonly By AddNewButton = By.XPath(".//div[@class='ui teal button ' and normalize-space(text())='Add New']");
        private readonly By _addSkillsField = By.XPath("//input[@placeholder='Add Skill']");
        private readonly By _selectSkillLevel = By.XPath("//select[@name='level']");
        private readonly By _addButton = By.XPath("//input[@value='Add']");
        private readonly By _cancelButton = By.XPath("//input[@value='Cancel']");

        //Edit
        private readonly By _addSkillsForUpdateField = By.XPath(".//input[@type='text']");
        private readonly By _updateButton = By.XPath(".//input[@value='Update']");
        private readonly By _cancelUpdateButton = By.XPath("//span[@class='buttons-wrapper']//input[@value='Cancel']");

        //Action Methods
        public void NavigateToTheProfilePage()
        {
            //Profile 
            var profileElement = _wait.Until(ExpectedConditions.ElementToBeClickable(_profileTab));
            profileElement.Click();

            //Skills
            var skillsElement = _wait.Until(ExpectedConditions.ElementToBeClickable(_skillsTab));
            skillsElement.Click();
        }

        public void AddSkillAndLevel(string skill, string skillLevel)
        {
            EnterSkillAndLevelToAdd(skill, skillLevel);
            ClickAddButton();
        }

        public void EnterSkillAndLevelToAdd(string skill, string skillLevel)
        {
            //Add New Skills
            var skillsTable = _wait.Until(ExpectedConditions.ElementIsVisible(_skillsTable));
            var addNewElement = skillsTable.FindElement(By.XPath(".//div[@class='ui teal button']"));
            addNewElement.Click();

            //Enter Skills
            var addSkillsElement = _wait.Until(ExpectedConditions.ElementToBeClickable(_addSkillsField));
            addSkillsElement.SendKeys(skill);

            //Select Skill Level
            var selectSkillLevelDropDown = _wait.Until(ExpectedConditions.ElementToBeClickable(_selectSkillLevel));

            SelectElement selectElement = new SelectElement(selectSkillLevelDropDown);
            selectElement.SelectByText(skillLevel);
        }

        public void ClickAddButton()
        {
            //Click Add Button
            var addButton = _wait.Until(ExpectedConditions.ElementToBeClickable(_addButton));
            addButton.Click();
        }

        public void ClickCancelButton()
        {
            //Click Cancel Button
            var cancelButton = _wait.Until(ExpectedConditions.ElementToBeClickable(_cancelButton));
            cancelButton.Click();
        }

        public void UpdateSkillsAndLevel(string existingSkill, string skillToUpdate, string skillLevelToUpdate = "Beginner")
        {
            try
            {
                var skillsTable = _wait.Until(ExpectedConditions.ElementIsVisible(_skillsTable));
                var row = skillsTable.FindElement(By.XPath($".//tr[td[normalize-space(text())='{existingSkill}']]"));
                var editIcon = row.FindElement(By.XPath(".//i[@class='outline write icon']"));
                editIcon.Click();

                var editableRow = skillsTable.FindElement(By.XPath($".//tr[.//input[@type='text' and @value='{existingSkill}']]"));
                //Update the Skill
                var addSkillsForUpdateElement = editableRow.FindElement(_addSkillsForUpdateField);
                addSkillsForUpdateElement.Clear();
                Thread.Sleep(1000);
                addSkillsForUpdateElement.SendKeys(skillToUpdate);

                //Update Skill Level
                var selectSkillLevelDropDown = editableRow.FindElement(By.XPath(".//select[@name='level']"));

                SelectElement selectElement = new SelectElement(selectSkillLevelDropDown);
                selectElement.SelectByText(skillLevelToUpdate);
                editableRow.FindElement(_updateButton).Click();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public void EnterSkillsAndLevelToUpdate(string existingSkill, string skillToUpdate, string skillLevelToUpdate)
        {
            try
            {
                var skillsTable = _wait.Until(ExpectedConditions.ElementIsVisible(_skillsTable));
                var row = skillsTable.FindElement(By.XPath($".//tr[td[normalize-space(text())='{existingSkill}']]"));
                var editIcon = row.FindElement(By.XPath(".//i[@class='outline write icon']"));
                editIcon.Click();

                var editableRow = skillsTable.FindElement(By.XPath($".//tr[.//input[@type='text' and @value='{existingSkill}']]"));
                //Update the Skill
                var addSkillsForUpdateElement = editableRow.FindElement(_addSkillsForUpdateField);
                addSkillsForUpdateElement.Clear();
                Thread.Sleep(1000);
                addSkillsForUpdateElement.SendKeys(skillToUpdate);

                //Update Skill Level
                var selectSkillLevelDropDown = editableRow.FindElement(By.XPath(".//select[@name='level']"));

                SelectElement selectElement = new SelectElement(selectSkillLevelDropDown);
                selectElement.SelectByText(skillLevelToUpdate);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public string GetLevelOfSkill(string skill)
        {
            try
            {
                var skillsTable = _wait.Until(ExpectedConditions.ElementIsVisible(_skillsTable));
                var row = skillsTable.FindElement(By.XPath($".//tr[td[normalize-space(text())='{skill}']]"));
                var levelCell = row.FindElement(By.XPath("./td[2]"));
                return levelCell.Text.Trim();
            }
            catch
            {
                return string.Empty;
            }
        }

        public bool IsSkillNotAdded(string skill)
        {
            try
            {
                var skillsTable = _wait.Until(ExpectedConditions.ElementIsVisible(_skillsTable));
                var row = skillsTable.FindElement(By.XPath($".//tr[td[normalize-space(text())='{skill}']]")); 
                return false;
            }
            catch
            {
                return true;
            }
        }

        public void ClickCancelUpdate()  //CancelUpdate
        {
            var clickCancelUpdate = _wait.Until(ExpectedConditions.ElementToBeClickable(_cancelUpdateButton));
            clickCancelUpdate.Click();
        }

        public void DeleteSpecificSkill(string skillsToBeDeleted)    //To delete the specific skill 
        {
            if (string.IsNullOrEmpty(skillsToBeDeleted)) //To avoid object reference null exception
                throw new ArgumentException("Skill list is empty or null");

            var languageTable = _wait.Until(ExpectedConditions.ElementIsVisible(_skillsTable));
            var row = languageTable.FindElement(By.XPath($".//tr[td[text()='{skillsToBeDeleted}']]"));
            var deleteIconElement = row.FindElement(By.XPath(".//i[@class='remove icon']"));
            deleteIconElement.Click();
        }

        public void DeleteAllSkills()    //To delete all skills 
        {
            while (true)
            {
                var skillsTable = _wait.Until(ExpectedConditions.ElementIsVisible(_skillsTable));
                var originalWait = _driver.Manage().Timeouts().ImplicitWait;
                ResetWaitTo(2);
                var deleteElements = skillsTable.FindElements(By.XPath(".//i[@class='remove icon']"));
                ResetWaitTo(originalWait.Seconds);
                if (deleteElements.Count > 0)
                {
                    deleteElements[0].Click();
                    Thread.Sleep(500);
                }
                else
                {
                    break;
                }
            }
        }

        private void ResetWaitTo(int seconds)
        {
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(seconds);
        }

        public void LeaveTheSkillAndLevelEmptyWithCombinationsForAdd(string skill,string level)    //To leave the skill field empty for adding skills
        {
            //Click Add New Button
            var skillsTable = _wait.Until(ExpectedConditions.ElementIsVisible(_skillsTable));
            var addNewElement = skillsTable.FindElement(By.XPath(".//div[@class='ui teal button']"));
            addNewElement.Click();

            //Enter Skills
            var addSkillsElement = _wait.Until(ExpectedConditions.ElementToBeClickable(_addSkillsField));

            if (!string.IsNullOrWhiteSpace(skill))
            {
                addSkillsElement.SendKeys(skill);
            }
            
            //Select Skill Level
            var selectSkillLevelDropDown = _wait.Until(ExpectedConditions.ElementToBeClickable(_selectSkillLevel));

            SelectElement selectElement = new SelectElement(selectSkillLevelDropDown);
            if (!string.IsNullOrWhiteSpace(level))
            {
                selectElement.SelectByText("Intermediate");
                // selectElement.SelectByValue("");
            }
            ClickAddButton();
        }

  
        public void LeaveTheSkillAndLevelEmptyWithCombinationsForUpdate(string existingSkill,string skillToUpdate,string skillLevelToUpdate)    //To leave the skill field empty for updating skill
        {   
                   
            try
            {
                var skillsTable = _wait.Until(ExpectedConditions.ElementIsVisible(_skillsTable));
                var row = skillsTable.FindElement(By.XPath($".//tr[td[normalize-space(text())='{existingSkill}']]"));
                var editIcon = row.FindElement(By.XPath(".//i[@class='outline write icon']"));
                editIcon.Click();

                var editableRow = skillsTable.FindElement(By.XPath($".//tr[.//input[@type='text' and @value='{existingSkill}']]"));
                //Update the Skill
                var addSkillsForUpdateElement = editableRow.FindElement(_addSkillsForUpdateField);
                addSkillsForUpdateElement.SendKeys(Keys.Control + "a" + Keys.Delete);

                if (!string.IsNullOrWhiteSpace(skillToUpdate))
                {
                    addSkillsForUpdateElement.SendKeys(skillToUpdate);
                }

                //Update Skill Level
                var selectSkillLevelDropDown = editableRow.FindElement(By.XPath(".//select[@name='level']"));

                SelectElement selectElement = new SelectElement(selectSkillLevelDropDown);
                if (!string.IsNullOrWhiteSpace(skillLevelToUpdate))
                {
                    selectElement.SelectByText(skillLevelToUpdate);
                    // selectElement.SelectByValue("");
                }
                else
                {
                    selectElement.SelectByIndex(0);
                }
                editableRow.FindElement(_updateButton).Click();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public string GetSuccessMessageForAddSkill(string newSkill)  //To get the success message after adding skill for validation
        {
            var successMessageForAddSkill = _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath($"//div[@class='ns-box-inner' and contains(text(), '{newSkill} has been added to your skills')]")));
            return successMessageForAddSkill.Text;
        }

        public List<string> GetAllAddedSkillList()    //To get all the skills list after adding for validation
        {
            try
            {
                var skillsTable = _wait.Until(ExpectedConditions.ElementIsVisible(_skillsTable));
                var addedSkills = new List<string>();
                var rows = skillsTable.FindElements(By.XPath(".//tbody/tr"));
                foreach (var row in rows)
                {
                    var skillCell = row.FindElement(By.XPath("./td[1]"));
                    addedSkills.Add(skillCell.Text.Trim());
                }
                return addedSkills;
            }
            catch
            {
                return new List<string>();
            }
        }

        public string GetSuccessMessageForUpdateSkill(string updateSkill)   //To get success message after updating skills for validation
        {
            var successMessageForUpdateSkill = _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath($"//div[@class='ns-box-inner' and contains(text(), '{updateSkill} has been updated to your skills')]")));
            return successMessageForUpdateSkill.Text;
        }

        public List<string> GetAllUpdatedSkillsList()    //To get all the updated list for validation
        {
            try
            {
                var skillsTable = _wait.Until(ExpectedConditions.ElementIsVisible(_skillsTable));
                var updatedSkills = new List<string>();
                var rows = skillsTable.FindElements(By.XPath(".//tbody/tr"));
                foreach (var row in rows)
                {
                    var skillCell = row.FindElement(By.XPath("./td[1]"));
                    updatedSkills.Add(skillCell.Text.Trim());
                }
                return updatedSkills;
            }
            catch
            {
                return new List<string>();
            }
        }

        public string GetSuccessMessageForDeleteSkill(string skillToBeDeleted)    //To get success message after deleting a skill 
        {
            Thread.Sleep(1000);
            var successMessageForDelete = _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath($"//div[@class='ns-box-inner' and  contains(text(), '{skillToBeDeleted} has been deleted')]")));
            return successMessageForDelete.Text;
        }

        public bool IsSkillsTableEmpty()  //To check the table is empty after deleting all skills for validation
        {
            var skillsTable = _wait.Until(ExpectedConditions.ElementIsVisible(_skillsTable));
            var rows = skillsTable.FindElements(By.XPath(".//tbody/tr"));
            return rows.Count == 0;
        }

        public bool IsErrorMessageDisplayed(string error)  //Error Message for both and any of the fields empty
        {
            try
            {
                var popUpMessageElement = _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath($"//div[contains(@class, 'ns-box-inner') and contains(text(), '{error}')]")));
                return true;// Found the Error message
            }
            catch
            {
                return false;
            }
        }

        public string GetErrorMessage()
        {
            try
            {
                var popUpMessageElement = _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath($"//div[contains(@class, 'ns-type-error') and contains(@class, 'ns-show')]/div[@class='ns-box-inner']")));
                return popUpMessageElement.Text;// Found the Error message
            }
            catch
            {
                return String.Empty;
            }
        }

        public bool IsCancelButtonNotDisplayed()       //Check the visibility of cancel button 
        {
            try
            {
                var cancelButtonNotVisible = _wait.Until(ExpectedConditions.InvisibilityOfElementLocated(_cancelButton));
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void ExpireSession() //To delete the token to get the session timeout message
        {
            try
            {
                _driver.Manage().Cookies.DeleteCookieNamed("marsAuthToken");
            }
            catch
            {

            }
        }
    }
}




