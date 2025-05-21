using OpenQA.Selenium.BiDi.Modules.Log;
using qa_dotnet_cucumber.Pages;
using Reqnroll;

namespace qa_dotnet_cucumber.Steps
{
    [Binding]
    [Scope(Feature = "Skill")]
    [Scope(Feature = "SkillNegative")]
    public class SkillSteps
    {
        private readonly LoginPage _loginPage;
        private readonly NavigationHelper _navigationHelper;
        private readonly SkillPage _skillPage;
        private readonly ScenarioContext _scenarioContext;

        public SkillSteps(LoginPage loginPage, NavigationHelper navigationHelper, SkillPage skillPage, ScenarioContext scenarioContext) //Constructor
        {
            _loginPage = loginPage;
            _navigationHelper = navigationHelper;
            _skillPage = skillPage;
            _scenarioContext = scenarioContext;
        }

        [Given("I navigate to the profile page as a registered user")]  //Login and navigated to the profile page
        public void GivenINavigateToTheProfilePageAsARegisteredUser()
        {
            _navigationHelper.NavigateTo("Home");
            _loginPage.Login("ambikaarumugams@gmail.com", "AmbikaSenthil123");
            _skillPage.NavigateToTheProfilePage();
        }

        [When("I Add the following skills and select skill level:")]    //Add new skill and level
        public void WhenIAddTheFollowingSkillAndSelectSkillLevel(Table addSkillTable)   //Passing the data thro' the table
        {
            var skillsToAdd = addSkillTable.CreateSet<AddSkill>();  
            var actualAddSkills = new List<string>();
            var expectedAddSkills = new List<string>();
            foreach (var skill in skillsToAdd)
            {
                expectedAddSkills.Add(skill.Skill);
                _skillPage.AddSkillAndLevel(skill.Skill, skill.SkillLevel);
                var successMessageAfterAddingSkill = _skillPage.GetSuccessMessageForAddSkill(skill.Skill);
                actualAddSkills.Add(successMessageAfterAddingSkill);
            }
            _scenarioContext.Set(actualAddSkills, "ActualAddSkills");
            _scenarioContext.Set(expectedAddSkills, "ExpectedAddSkills");
            _scenarioContext.Set(expectedAddSkills, "SkillsToCleanup");  //Clean up for add
        }

        [When("I should see the skills and verify it has been added successfully")]  //Validation for add
        public void WhenIShouldSeeTheSkillsAndVerifyItHasBeenAddedSuccessfully()
        {
            var actualAddSkills = _scenarioContext.Get<List<string>>("ActualAddSkills");
            var expectedAddSkills = _scenarioContext.Get<List<string>>("ExpectedAddSkills");
            foreach (var expectedSkill in expectedAddSkills)
            {
                Assert.That(actualAddSkills.Any(actual => actual.Contains(expectedSkill)),
                    Is.True, $"Expected a message contains'{expectedSkill}',but not found");
            }
        }

        [Then("I should see the skills listed in my profile and verify it")] //Validation for expected and actual list
        public void ThenIShouldSeeTheSkillsListedInMyProfileAndVerifyIt()
        {
            var actualSkillList = _skillPage.GetAllAddedSkillList();
            var expectedSkillList = _scenarioContext.Get<List<string>>("ExpectedAddSkills");
            Assert.That(actualSkillList, Is.EqualTo(expectedSkillList), "There is a mismatch!"); 
        }

        [When("I add the following skills and select their levels:")]  //Add skills to update
        public void WhenIAddTheFollowingSkillsAndSelectTheirLevels(Table addSkillToEditTable)
        {
            var addSkillToEditList = addSkillToEditTable.CreateSet<AddSkill>();
            var actualAddSkillsToEdit = new List<string>();
            var expectedAddSkillsToEdit = new List<string>();
            foreach (var addSkillToEdit in addSkillToEditList)
            {
                expectedAddSkillsToEdit.Add(addSkillToEdit.Skill);
                _skillPage.AddSkillAndLevel(addSkillToEdit.Skill, addSkillToEdit.SkillLevel);
                var successMessage = _skillPage.GetSuccessMessageForAddSkill(addSkillToEdit.Skill);
                actualAddSkillsToEdit.Add(successMessage);
            }
            _scenarioContext.Set(actualAddSkillsToEdit, "ActualAddSkillsToEdit");
            _scenarioContext.Set(expectedAddSkillsToEdit, "ExpectedAddSkillsToEdit");
        }

        [Then("I should see the added skills in the profile")]  //Validation
        public void ThenIShouldSeeTheAddedSkillsInTheProfile()
        {
            var actualAddSkills = _scenarioContext.Get<List<string>>("ActualAddSkillsToEdit");
            var expectedAddSkills = _scenarioContext.Get<List<string>>("ExpectedAddSkillsToEdit");
            foreach (var expectedAddSkill in expectedAddSkills)
            {
                Assert.That(actualAddSkills.Any(actual => actual.Contains(expectedAddSkill)), Is.True, $"Expected message contains {expectedAddSkill}, but not found");
            }
        }

        [Then("I update the following skills if they match the existing ones:")]  //Update skills with existing one
        public void ThenIUpdateTheFollowingSkillsIfTheyMatchTheExistingOnes(DataTable updateSkillTable)
        {
            var skillsToUpdateList = updateSkillTable.CreateSet<UpdateSkill>();
            var actualUpdateSkills = new List<string>();
            var expectedUpdateSkills = new List<string>();
            foreach (var addUpdateSkill in skillsToUpdateList)
            {
                expectedUpdateSkills.Add(addUpdateSkill.SkillToUpdate);
                _skillPage.UpdateSkillsAndLevel(addUpdateSkill.ExistingSkill, addUpdateSkill.SkillToUpdate, addUpdateSkill.SkillLevelToUpdate);
                var successMessageForUpdate = _skillPage.GetSuccessMessageForUpdateSkill(addUpdateSkill.SkillToUpdate);
                actualUpdateSkills.Add(successMessageForUpdate);
            }
            _scenarioContext.Set(actualUpdateSkills, "ActualUpdatedSkills");
            _scenarioContext.Set(expectedUpdateSkills, "ExpectedUpdatedSkills");
            _scenarioContext.Set(expectedUpdateSkills, "SkillsToCleanup"); //Clean up update
        }

        [Then("I should see a success message and the updated skills in my profile")]  //Validation for update
        public void ThenIShouldSeeASuccessMessageAndTheUpdatedSkillsInMyProfile()
        {
            var actualUpdateSkills = _scenarioContext.Get<List<string>>("ActualUpdatedSkills");
            var expectedUpdateSkills = _scenarioContext.Get<List<string>>("ExpectedUpdatedSkills");
            foreach (var expectedUpdateSkill in expectedUpdateSkills)
            {
                Assert.That(actualUpdateSkills.Any(actual => actual.Contains(expectedUpdateSkill)), Is.True, $"Expected message contains {expectedUpdateSkill}, but not found");
            }
        }

        [When("I click the delete icon corresponding to the following skills:")]  //Delete the specific skills
        public void WhenIClickTheDeleteIconCorrespondingToTheFollowingSkills(Table skillsToBeDeletedTable)
        {
            var skillToDeleteList = skillsToBeDeletedTable.CreateSet<DeleteSkill>();
            var actualDeleteSkills = new List<string>();
            var expectedDeleteSkills = new List<string>();
            foreach (var skillToDelete in skillToDeleteList)
            {
                expectedDeleteSkills.Add(skillToDelete.SkillToBeDeleted);
                _skillPage.DeleteSpecificSkill(skillToDelete.SkillToBeDeleted);
                var deleteSuccessMessage = _skillPage.GetSuccessMessageForDeleteSkill(skillToDelete.SkillToBeDeleted);
                actualDeleteSkills.Add(deleteSuccessMessage);
            }
            _scenarioContext.Set(actualDeleteSkills, "ActualDeleteSkills");
            _scenarioContext.Set(expectedDeleteSkills, "ExpectedDeleteSkills");
        }

        [Then("I should see a success message for each deleted skill")] //Validation for delete
        public void ThenIShouldSeeASuccessMessageForEachDeletedSkill()
        {
            var actualDeleteSkills = _scenarioContext.Get<List<string>>("ActualDeleteSkills");
            var expectedDeleteSkills = _scenarioContext.Get<List<string>>("ExpectedDeleteSkills");
            foreach (var expectedDeleteSkill in expectedDeleteSkills)
            {
                Assert.That(actualDeleteSkills.Any(actual => actual.Contains(expectedDeleteSkill)),
                    Is.True, $"Expected a message contains'{expectedDeleteSkill}',but not found");
            }
        }

        [Then("the skills table should be empty if all skills have been deleted")] //Delete all skills
        public void ThenTheSkillsTableShouldBeEmptyIfAllSkillsHaveBeenDeleted()
        {
            _skillPage.DeleteAllSkills();
            Assert.That(_skillPage.IsSkillsTableEmpty(), Is.True, $"Skill table is not empty after deletion");
        }

        [When("I add skill as {string} and level as {string}")] //Add skill
        public void WhenIAddSkillAsAndLevelAs(string skill, string level)
        {
            _skillPage.AddSkillAndLevel(skill, level);
            var successMessage = _skillPage.GetSuccessMessageForAddSkill(skill);
            Console.WriteLine(successMessage);
            _scenarioContext.Set(new List<string> { skill }, "ActualMessage");
        }

        [When("when I update the skill and skill level:")]   //Update existing skill with different level
        public void WhenWhenIUpdateTheSkillAndSkillLevel(Table updateExistingSkillAndDifferentLevelTable)
        {
            var updateExistingSkillAndDifferentLevelList = updateExistingSkillAndDifferentLevelTable.CreateInstance<UpdateSkill>();
            var actualSameSkillAndDifferentLevelToUpdate = new List<string>();
            var expectedSameSkillAndDifferentLevelToUpdate = new List<string>();

            _skillPage.UpdateSkillsAndLevel(updateExistingSkillAndDifferentLevelList.ExistingSkill, updateExistingSkillAndDifferentLevelList.SkillToUpdate, updateExistingSkillAndDifferentLevelList.SkillLevelToUpdate);
            expectedSameSkillAndDifferentLevelToUpdate.Add(updateExistingSkillAndDifferentLevelList.SkillToUpdate);
            var successMessage = _skillPage.GetSuccessMessageForUpdateSkill(updateExistingSkillAndDifferentLevelList.SkillToUpdate);
            actualSameSkillAndDifferentLevelToUpdate.Add(successMessage);

            _scenarioContext.Set(actualSameSkillAndDifferentLevelToUpdate, "ActualSameSkillAndDifferentLevelToUpdate");
            _scenarioContext.Set(expectedSameSkillAndDifferentLevelToUpdate, "ExpectedSkillAndDifferentLevelToUpdate");
            _scenarioContext.Set(expectedSameSkillAndDifferentLevelToUpdate, "SkillsToCleanup"); //Clean up
        }

        [Then("I should see the success message and updated skill in my profile")]  //Validation for update same skill with different level
        public void ThenIShouldSeeTheSuccessMessageAndUpdatedSkillInMyProfile()
        {
            var actualList = _scenarioContext.Get<List<string>>("ActualSameSkillAndDifferentLevelToUpdate");
            var expectedList = _scenarioContext.Get<List<string>>("ExpectedSkillAndDifferentLevelToUpdate");
            var actualMessage = actualList.FirstOrDefault();
            var expectedMessage = expectedList.FirstOrDefault();
            Assert.That(actualMessage, Does.Contain(expectedMessage), $"Expected Message '{expectedMessage}' hasn't updated, but got '{actualMessage}'");
        }

        [When("I try to add a skill {string} with level {string}")]  //Add invalid skills
        public void WhenITryToAddASkillWithLevel(string skill, string level)
        {
            if (skill == "<space>")
            {
                skill = "    ";
            }
            _skillPage.AddSkillAndLevel(skill, level);

            var successMessage = _skillPage.GetSuccessMessageForAddSkill(skill);
            _scenarioContext.Set(successMessage, "ActualSkills");
            _scenarioContext.Set(new List<string> { skill }, "SkillsToCleanup"); //Clean up
        }

        [Then("I should see the error message {string}")]   //Validation for invalid skill
        public void ThenIShouldSeeTheErrorMessage(string expected)
        {
            var actualSkills = _scenarioContext.Get<string>("ActualSkills");
            Assert.That(actualSkills.Contains(expected), Is.True, $"Expected message contains '{expected}', but got '{actualSkills}'");
        }

        [When("I update the following skills if they match the existing ones:")] //Update the invalid skill
        public void WhenIUpdateTheFollowingSkillsIfTheyMatchTheExistingOnes(Table updateInvalidSkillTable)
        {
            var updateInvalidSkillsList = updateInvalidSkillTable.CreateSet<UpdateSkill>();
            var actualSkills = new List<string>();
            var expectedSkills = new List<string>();
            foreach (var updateInvalidSkill in updateInvalidSkillsList)
            {
                if (string.Equals(updateInvalidSkill.SkillToUpdate, "<space>", StringComparison.OrdinalIgnoreCase))
                {
                    updateInvalidSkill.SkillToUpdate = "   ";
                }
                expectedSkills.Add(updateInvalidSkill.SkillToUpdate);
                _skillPage.UpdateSkillsAndLevel(updateInvalidSkill.ExistingSkill, updateInvalidSkill.SkillToUpdate, updateInvalidSkill.SkillLevelToUpdate);
                var successMessageForUpdate = _skillPage.GetSuccessMessageForUpdateSkill(updateInvalidSkill.SkillToUpdate);
                actualSkills.Add(successMessageForUpdate);
            }
            _scenarioContext.Set(actualSkills, "ActualSkills");
            _scenarioContext.Set(expectedSkills, "SkillsToCleanup");  //Clean up 
        }

        [Then("I should see the {string} message")]  //Validation
        public void ThenIShouldSeeTheMessage(string expected)
        {
            var actualList = _scenarioContext.Get<List<string>>("ActualSkills");
            foreach (var actual in actualList)       //Multiple skills in the list. So, using foreach loop for validation 
            {
                Assert.That(actual.Contains(expected), Is.True,
                    $"Invalid language name was accepted '{actual}', not found {expected} error message");
            }
        }

        [When("I want to add skill as {string} and level as {string} when the session is expired")]  //Add skill during session expired
        public void WhenIWantToAddSkillAsAndLevelAsWhenTheSessionIsExpired(string skillToAdd, string skillLevelToAdd)
        {
            _skillPage.ExpireSession();
            _skillPage.AddSkillAndLevel(skillToAdd, skillLevelToAdd);
        }

        [When("I add skill as {string} and level as {string} to update")]  
        public void WhenIAddSkillAsAndLevelAsToUpdate(string skill, string level)
        {
            _skillPage.AddSkillAndLevel(skill, level);
            _scenarioContext.Set(new List<string> { skill }, "SkillsToCleanup");
        }

        [When("I want to update skill the existing skill as{string}, skill to update as {string},and level to update as {string} when the session is expired")]  //Update skill during session expired
        public void WhenIWantToUpdateSkillTheExistingSkillAsSkillToUpdateAsAndLevelToUpdateAsWhenTheSessionIsExpired(string existingSkill, string skillToUpdate, string skillLevelToUpdate)
        {
            _skillPage.ExpireSession();
            _skillPage.UpdateSkillsAndLevel(existingSkill, skillToUpdate, skillLevelToUpdate);
            _skillPage.ClickCancelUpdate();    //Cancel the update so that we can do clean up for the existing skill
        }

        [When("I add skill {string} and level as {string} to delete")]
        public void WhenIAddSkillAndLevelAsToDelete(string skill, string level)
        {
            _skillPage.AddSkillAndLevel(skill, level);
            _scenarioContext.Set(new List<string> { skill }, "SkillsToCleanup");  //Clean up
        }

        [When("I want to delete skill {string} when the session is expired")]  //Delete skill during session expired
        public void WhenIWantToDeleteSkillWhenTheSessionIsExpired(string skillsToDelete)
        {
            _skillPage.ExpireSession();
            _skillPage.DeleteSpecificSkill(skillsToDelete);
        }

        [Then("I should see {string} error message")]  //Validation for session expired
        public void ThenIShouldSeeErrorMessage(string errorMessage)
        {
            Assert.That(_skillPage.IsErrorMessageDisplayed(errorMessage), Is.True, $"Error message {errorMessage} shouldn't be displayed");
        }

        [When("I click Add New button, enter the skill {string} and it's level {string}")]  //Add skill to perform cancel add
        public void WhenIClickAddNewButtonEnterTheSkillAndItsLevel(string skillToCancel, string skillLevelToCancel)
        {
            _skillPage.EnterSkillAndLevelToAdd(skillToCancel, skillLevelToCancel);
        }

        [Then("I should able to Cancel the operation and verify that the skill {string} shouldn't be added")]  //Cancel add skills
        public void ThenIShouldAbleToCancelTheOperationAndVerifyThatTheSkillShouldntBeAdded(string skill)
        {
            _skillPage.ClickCancelButton();
            Assert.That(_skillPage.IsSkillNotAdded(skill), Is.True, $"{skill} is added!");
        }

        [When("I add skill {string} and it's level {string}")]    //Add skill to cancel update
        public void WhenIAddSkillAndItsLevel(string skill, string level)
        {
            _skillPage.AddSkillAndLevel(skill, level);
            _scenarioContext.Set(new List<string> { skill }, "SkillsToCleanup");
        }

        [When("I click edit icon of {string} and Update level to {string} and level to {string}")]   //enter skill to update
        public void WhenIClickEditIconOfAndUpdateLevelToAndLevelTo(string existingSkill, string skillToUpdate, string skillLevelToUpdate)
        {
            _skillPage.EnterSkillsAndLevelToUpdate(existingSkill, skillLevelToUpdate, skillLevelToUpdate);
        }

        [When("I click cancel")]  //Click cancel update button
        public void WhenIClickCancel()
        {
            _skillPage.ClickCancelUpdate();
        }

        [Then("the skill {string} should remain unchanged with level {string}")]  //Validation for cancel
        public void ThenTheSkillShouldRemainUnchangedWithLevel(string skill, string level)
        {
            Assert.That(_skillPage.GetLevelOfSkill(skill), Is.EqualTo(level), $"{skill} update hasn't cancelled");
        }

        [When("I add the skills {string} and it's level {string}in different combinations")]  //Leave the skill, level empty or both for add
        public void WhenIAddTheSkillsAndItsLevelInDifferentCombinations(string skill, string level)
        {
            _skillPage.LeaveTheSkillAndLevelEmptyWithCombinationsForAdd(skill, level);
            var actualMessage = _skillPage.GetErrorMessage();
            _scenarioContext.Set(actualMessage, "ActualMessage");

        }
        [When("I add the Skill as {string} and level as {string}")]
        public void WhenIAddTheSkillAsAndLevelAs(string skill, string level)
        {
            _skillPage.AddSkillAndLevel(skill, level);
        }

        [When("I update existing skill {string} to {string} with level {string}")] //Leave the skill, level empty or both for update
        public void WhenIUpdateExistingSkillToWithLevel(string existingSkill, string skillToUpdate, string skillLevelToUpdate)
        {
            _skillPage.LeaveTheSkillAndLevelEmptyWithCombinationsForUpdate(existingSkill, skillToUpdate, skillLevelToUpdate);
            var actualMessage = _skillPage.GetErrorMessage();
            _skillPage.ClickCancelUpdate();
            _scenarioContext.Set(actualMessage, "ActualMessage");
            _scenarioContext.Set(new List<string> { existingSkill }, "SkillsToCleanup");  //Clean up
        }

        [Then("I should see the {string}")]     //Validation for either of the field empty or both
        public void ThenIShouldSeeThe(string errorMessage)
        {
            var actualMessage = _scenarioContext.Get<string>("ActualMessage");
            Assert.That(actualMessage.Contains(errorMessage), Is.True, $"Expected message contains {errorMessage}, but got {actualMessage}");
        }

        [When("I try to add huge skill name of length {int} and skill level as {string}")]   //Add huge name as invalid input
        public void WhenITryToAddHugeSkillNameOfLengthAndSkillLevelAs(int length, string level)
        {
            var hugeSkillName = new string('g', length);
            _skillPage.AddSkillAndLevel(hugeSkillName, level);
            var successMessage = _skillPage.GetSuccessMessageForAddSkill(hugeSkillName);
            _scenarioContext.Set(successMessage, "ActualSkill");
            _scenarioContext.Set(new List<string> { hugeSkillName }, "SkillsToCleanup");  //Clean up
        }

        [When("I update existing skill {string} with  huge skill name of length {int} and skill level as {string}")]  //Update with huge skill name
        public void WhenIUpdateExistingSkillWithHugeSkillNameOfLengthAndSkillLevelAs(string existingSkill, int length, string level)
        {
            var longSkillName = new string('f', length);
            _skillPage.UpdateSkillsAndLevel(existingSkill, longSkillName, level);
            var successMessage = _skillPage.GetSuccessMessageForUpdateSkill(longSkillName);
            _scenarioContext.Set(successMessage, "ActualSkills");
            _scenarioContext.Set(new List<string> { longSkillName }, "SkillsToCleanup");
        }

        [Then("I should see warning message as {string}")]    //Validation
        public void ThenIShouldSeeWarningMessageAs(string expected)
        {
            var actualSkill = _scenarioContext.Get<string>("ActualSkill");
            Assert.That(actualSkill.Contains(expected), Is.True, $"Expected message contains '{expected}', but got '{actualSkill}'");
        }

        [When("when I add the same skill and choose different skill level")]   //Add same skill, different level
        public void WhenWhenIAddTheSameSkillAndChooseDifferentSkillLevel(Table sameSkillDifferentLevelTable)
        {
            var cleanUp = _scenarioContext.Get<List<string>>("ActualMessage");   //I retrieve the skill to be cleaned up from the previous step (step 1)
            var sameSkillDifferentLevel = sameSkillDifferentLevelTable.CreateInstance<AddSkill>();
            _skillPage.AddSkillAndLevel(sameSkillDifferentLevel.Skill, sameSkillDifferentLevel.SkillLevel);
            var message = _skillPage.GetErrorMessage();  //When I add the same skill second time it will show error message
            Console.WriteLine(message);
            _scenarioContext.Set(message, "Actual");  //I'm storing it for validation
            _scenarioContext.Set(cleanUp, "SkillsToCleanup");  //Clean up
        }

        [When("when I add the same skill and choose same skill level")]  //Add same skill, same level
        public void WhenWhenIAddTheSameSkillAndChooseSameSkillLevel(Table sameSkillSameLevelTable)
        {
            var cleanUp = _scenarioContext.Get<List<string>>("ActualMessage");   //I retrieve the skill to be cleaned up from the previous step (step 1)
            var sameSkillSameLevel = sameSkillSameLevelTable.CreateInstance<AddSkill>();
            _skillPage.AddSkillAndLevel(sameSkillSameLevel.Skill, sameSkillSameLevel.SkillLevel);
            var message = _skillPage.GetErrorMessage();  //When I add the same skill second time it will show error message
            Console.WriteLine(message);
            _scenarioContext.Set(message, "Actual");  //I'm storing it for validation
            _scenarioContext.Set(cleanUp, "SkillsToCleanup");   //Clean up
        }

        [When("when I update the same skill and same skill level:")]   //Update same skill, same level
        public void WhenWhenIUpdateTheSameSkillAndSameSkillLevel(Table sameSkillSameLevelForUpdateTable)
        {
            var cleanUp = _scenarioContext.Get<List<string>>("ActualMessage");   //I retrieve the skill to be cleaned up from the previous step (step 1)
            var sameSkillSameLevel = sameSkillSameLevelForUpdateTable.CreateInstance<UpdateSkill>();
            _skillPage.UpdateSkillsAndLevel(sameSkillSameLevel.ExistingSkill, sameSkillSameLevel.SkillToUpdate, sameSkillSameLevel.SkillLevelToUpdate);
            var message = _skillPage.GetErrorMessage();  //When I add the same skill second time it will show error message
            Console.WriteLine(message);
            _skillPage.ClickCancelUpdate();  //I got no such element exception for clean up. When I click the cancel update button, I could see the added skill list.
            _scenarioContext.Set(message, "Actual");  //I'm storing it for validation
            _scenarioContext.Set(cleanUp, "SkillsToCleanup");
        }

        [Then("I should able to see the {string} in my profile")]
        public void ThenIShouldAbleToSeeTheInMyProfile(string expected)
        {
            var actual = _scenarioContext.Get<string>("Actual");
            Assert.That(actual, Is.EqualTo(expected), $"Expected message {expected} is not found");
        }

        [When("I add all skills from CSV")]  //Add skills using csv file
        public void WhenIAddAllSkillsFromCSV()
        {
            var actualList = new List<string>();
            var expectedList = new List<string>();
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData", "skills.csv"); //Path of the skills.csv file
            var lines = File.ReadAllLines(filePath).Skip(1); //skip the header
            foreach (var singleLine in lines)
            {
                var parts = singleLine.Split(',');
                var skill = parts[0].Trim(); //Skill
                var level = parts[1].Trim(); //Level
                expectedList.Add(skill);
                _skillPage.AddSkillAndLevel(skill, level);   //Add skill
                var successMessage = _skillPage.GetSuccessMessageForAddSkill(skill);
                actualList.Add(successMessage);
            }
            _scenarioContext.Set(expectedList, "ExpectedList");
            _scenarioContext.Set(actualList, "ActualList");
            _scenarioContext.Set(expectedList, "SkillsToCleanup"); //Clean up
        }

        [Then("all skills should be added successfully")]   //Validation
        public void ThenAllSkillsShouldBeAddedSuccessfully()
        {
            var actualList = _scenarioContext.Get<List<string>>("ActualList");
            var expectedList = _scenarioContext.Get<List<string>>("ExpectedList");
            foreach (var expected in expectedList)
            {
                Assert.That(actualList.Any(actual => actual.Contains(expected)), Is.True,
                    $"Actual doesn't contain {expected}");
            }
        }
    }
}

public class AddSkill
{
    public string Skill { get; set; }
    public string SkillLevel { get; set; }
}

public class UpdateSkill
{
    public string ExistingSkill { get; set; }
    public string SkillToUpdate { get; set; }
    public string SkillLevelToUpdate { get; set; }
}

public class DeleteSkill
{
    public string SkillToBeDeleted { get; set; }
}