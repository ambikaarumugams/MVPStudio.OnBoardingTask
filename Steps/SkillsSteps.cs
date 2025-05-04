using qa_dotnet_cucumber.Pages;
using Reqnroll;


namespace qa_dotnet_cucumber.Steps
{
    [Binding]
    [Scope(Feature = "Skills")]
    [Scope(Feature = "SkillsNegative")]
    public class SkillsSteps
    {
        private readonly LoginPage _loginPage;
        private readonly NavigationHelper _navigationHelper;
        private readonly SkillsPage _skillsPage;
        private readonly ScenarioContext _scenarioContext;

        public SkillsSteps(LoginPage loginPage, NavigationHelper navigationHelper, SkillsPage skillsPage, ScenarioContext scenarioContext)
        {
            _loginPage = loginPage;
            _navigationHelper = navigationHelper;
            _skillsPage = skillsPage;
            _scenarioContext = scenarioContext;
        }

        [Given("I navigate to the profile page as a registered user")]
        public void GivenINavigateToTheProfilePageAsARegisteredUser()
        {
            _navigationHelper.NavigateTo("Home");
            _loginPage.Login("ambikaarumugams@gmail.com", "AmbikaSenthil123");
            _skillsPage.NavigateToTheProfilePage();
        }

        [When("I Add the following NewSkills and select SkillLevel:")]
        public void WhenIAddTheFollowingNewSkillsAndSelectSkillLevel(Table addSkillsTable)
        {
            _skillsPage.DeleteAllSkills();
            var skillsToAdd = addSkillsTable.CreateSet<AddSkills>();
            var _actualAddedSkills = new List<string>();
            var _expectedSkillsToAdd = new List<string>();
            foreach (var skill in skillsToAdd)
            {
                _expectedSkillsToAdd.Add(skill.NewSkills);
                _skillsPage.AddNewSkillsAndLevel(skill.NewSkills, skill.SkillLevel);
                var successMessageAfterAddingSkill = _skillsPage.GetSuccessMessageForAddSkill(skill.NewSkills);
                _actualAddedSkills.Add(successMessageAfterAddingSkill);
            }
            _scenarioContext.Set(_actualAddedSkills, "ActualSkillsAdded");
            _scenarioContext.Set(_expectedSkillsToAdd, "ExpectedSkillsToAdd");
        }

        [When("I should see the skills and verify it has been added successfully")]
        public void WhenIShouldSeeTheSkillsAndVerifyItHasBeenAddedSuccessfully()
        {
            var _actualAddedSkills = _scenarioContext.Get<List<string>>("ActualSkillsAdded");
            var _expectedSkillsToAdd = _scenarioContext.Get<List<string>>("ExpectedSkillsToAdd");
            foreach (var skill in _expectedSkillsToAdd)
            {
                Assert.That(_actualAddedSkills.Any(actual => actual.Contains(skill)),
                Is.True, $"Expected a message contains'{skill}',but not found");
            }
        }

        [Then("I should see the skills listed in my profile and verify it")]
        public void ThenIShouldSeeTheSkillsListedInMyProfileAndVerifyIt()
        {
            var _actualSkills = _skillsPage.GetAllAddedSkillList();
            var _expectedSkillsToAdd = _scenarioContext.Get<List<string>>("ExpectedSkillsToAdd");
            Assert.That(_actualSkills, Is.EqualTo(_expectedSkillsToAdd), "There is a mismatch!");
        }

        [When("I update the skill and skill level:")]
        public void WhenIUpdateTheSkillAndSkillLevel(Table updateSkillsTable)
        {
            var skillsToUpdate = updateSkillsTable.CreateSet<UpdateSkills>();
            var _actualUpdatedSkills = new List<string>();
            var _expectedSkillsToUpdate = new List<string>();
            foreach (var updateSkill in skillsToUpdate)
            {
                _expectedSkillsToUpdate.Add(updateSkill.SkillToUpdate);
                _skillsPage.UpdateSkillsAndLevel(updateSkill.ExistingSkill, updateSkill.SkillToUpdate, updateSkill.SkillLevelToUpdate);
                var successMessageAfterUpdatingSkill = _skillsPage.GetUpdatedSkillSuccessMessage(updateSkill.SkillToUpdate);
                _actualUpdatedSkills.Add(successMessageAfterUpdatingSkill);
            }
            _scenarioContext.Set(_actualUpdatedSkills, "ActualSkillsUpdated");
            _scenarioContext.Set(_expectedSkillsToUpdate, "ExpectedSkillsToUpdate");
        }

        [Then("I should see the success message and updated skills in my profile")]
        public void ThenIShouldSeeTheSuccessMessageAndUpdatedSkillsInMyProfile()
        {
            var _actualUpdatedSkills = _scenarioContext.Get<List<string>>("ActualSkillsUpdated");
            var _expectedSkillsToUpdate = _scenarioContext.Get<List<string>>("ExpectedSkillsToUpdate");
            foreach (var UpdatedSkill in _expectedSkillsToUpdate)
            {
                Assert.That(_actualUpdatedSkills.Any(actual => actual.Contains(UpdatedSkill)),
                Is.True, $"Expected a message contains'{UpdatedSkill}',but not found");
            }
            Assert.That(_skillsPage.GetAllUpdatedSkillsList(), Is.SupersetOf(_expectedSkillsToUpdate), "The skills haven't updated successfully");
        }

        [When("I click the delete icon corresponding to the following skills:")]
        public void WhenIClickTheDeleteIconCorrespondingToTheFollowingSkills(Table skillsToBeDeleted)
        {
            var skillsToDelete = skillsToBeDeleted.CreateSet<DeleteSkills>();
            var _actualDeletedSkills = new List<string>();
            var _expectedSkillsToDelete = new List<string>();
            foreach (var deleteSkill in skillsToDelete)
            {
                _expectedSkillsToDelete.Add(deleteSkill.SkillsToBeDeleted);
                _skillsPage.DeleteSpecificSkills(deleteSkill.SkillsToBeDeleted);
                var successMessageAfterDeletingSkill = _skillsPage.GetSuccessMessageForDeleteSkill(deleteSkill.SkillsToBeDeleted);
                _actualDeletedSkills.Add(successMessageAfterDeletingSkill);
            }
            _scenarioContext.Set(_actualDeletedSkills, "ActualDeletedSkills");
            _scenarioContext.Set(_expectedSkillsToDelete, "ExpectedSkillsToDelete");
        }

        [Then("I should see a success message for each deleted skill")]
        public void ThenIShouldSeeASuccessMessageForEachDeletedSkill()
        {
            var _actualDeletedSkills = _scenarioContext.Get<List<string>>("ActualDeletedSkills");
            var _expectedSkillsToDelete = _scenarioContext.Get<List<string>>("ExpectedSkillsToDelete");
            foreach (var DeletedSkill in _expectedSkillsToDelete)
            {
                Assert.That(_actualDeletedSkills.Any(actual => actual.Contains(DeletedSkill)),
                Is.True, $"Expected a message contains'{DeletedSkill}',but not found");
            }
        }

        [Then("the skills table should be empty if all skills have been deleted")]
        public void ThenTheSkillsTableShouldBeEmptyIfAllSkillsHaveBeenDeleted()
        {
            _skillsPage.DeleteAllSkills();
            Thread.Sleep(500);
            Assert.That(_skillsPage.IsSkillsTableEmpty(), Is.True, "Skills table is not empty after deletion");
        }

        [When("I click Add New button, leave the skill field empty,choose the skill level and click the Add button")]
        public void WhenIClickAddNewButtonLeaveTheSkillFieldEmptyChooseTheSkillLevelAndClickTheAddButton()
        {
            _skillsPage.DeleteAllSkills();
            _skillsPage.LeaveTheSkillFieldEmptyForAdd();
            _skillsPage.ClickAddButton();
        }

        [Then("I should see {string} error message")]
        public void ThenIShouldSeeErrorMessage(string error)
        {
            Assert.That(_skillsPage.IsErrorMessageDisplayed(error), Is.True, $"Error Message {error} should be displayed");
        }

        [When("I click Add New button, enter the skill field, not choosing the skill level and click the Add button")]
        public void WhenIClickAddNewButtonEnterTheSkillFieldNotChoosingTheSkillLevelAndClickTheAddButton()
        {
            _skillsPage.DeleteAllSkills();
            _skillsPage.NotChoosingSkillLevelForAdd();
            _skillsPage.ClickAddButton();
        }

        [When("I click Add New button, empty the skill field, not choosing the skill level and click the Add button")]
        public void WhenIClickAddNewButtonEmptyTheSkillFieldNotChoosingTheSkillLevelAndClickTheAddButton()
        {
            _skillsPage.DeleteAllSkills();
            _skillsPage.LeaveTheSkillFieldEmptyAndNotChoosingSkillLevelForAdd();
            _skillsPage.ClickAddButton();
        }

        [When("I add skill {string} and it's level {string}")]
        public void WhenIAddSkillAndItsLevel(string skill, string level)
        {
            _skillsPage.DeleteAllSkills();
            _skillsPage.AddNewSkillsAndLevel(skill,level);
        }

        [When("I click edit icon of {string}, leave the skill field empty,choose the skill level and click the Update button")]
        public void WhenIClickEditIconOfLeaveTheSkillFieldEmptyChooseTheSkillLevelAndClickTheUpdateButton(string existingSkill)
        {
            _skillsPage.LeaveTheSkillFieldEmptyForUpdate(existingSkill);
        }

        [When("I click edit icon of {string}, enter the skill field, not choosing the skill level and click the Update button")]
        public void WhenIClickEditIconOfEnterTheSkillFieldNotChoosingTheSkillLevelAndClickTheUpdateButton(string existingSkill)
        {
            _skillsPage.NotChoosingSkillLevelForUpdate(existingSkill);
        }

        [When("I click edit icon of {string}, empty the skill field, not choosing the skill level and click the Update button")]
        public void WhenIClickEditIconOfEmptyTheSkillFieldNotChoosingTheSkillLevelAndClickTheUpdateButton(string existingSkill)
        {
            _skillsPage.LeaveTheSkillFieldEmptyAndNotChoosingSkillLevelForUpdate(existingSkill);
        }

        [When("I click Add New Skill {string} with level {string}")]
        public void WhenIClickAddNewSkillWithLevel(string skill, string level)
        {
            _skillsPage.DeleteAllSkills();
            _skillsPage.AddNewSkillsAndLevel(skill, level);
        }

        [When("I click edit icon of {string} and Update skill to {string} and level to {string}")]
        public void WhenIClickEditIconOfAndUpdateSkillToAndLevelTo(string skillToUpdate, string skill, string level)
        {
            _skillsPage.EnterSkillsAndLevelToUpdate(skillToUpdate, skill, level);
        }

        [When("I click cancel")]
        public void WhenIClickCancel()
        {
            _skillsPage.ClickCancelUpdate();
        }

        [Then("the skill {string} should remain unchanged with level {string}")]
        public void ThenTheSkillShouldRemainUnchangedWithLevel(string skill, string level)
        {
            Assert.That(_skillsPage.GetLevelOfSkill(skill),Is.EqualTo(level)  , $"Skill is Updated!");
        }

        [When("I click Add New button, enter the Skill {string} and it's level {string}")]
        public void WhenIClickAddNewButtonEnterTheSkillAndItsLevel(string skill, string level)
        {
            _skillsPage.DeleteAllSkills();
            _skillsPage.EnterNewSkillsAndLevelToAdd(skill, level);
        }

        [Then("I should able to Cancel the operation and verify that the skill {string} shouldn't be added")]
        public void ThenIShouldAbleToCancelTheOperationAndVerifyThatTheSkillShouldntBeAdded(string skill)
        {
            _skillsPage.ClickCancelButton();
            Assert.That(_skillsPage.IsSkillNotAdded(skill),Is.True,$"{skill} is Added!");
        }

        [When("I Add the same Skills and different SkillLevel:")]
        public void WhenIAddTheSameSkillsAndDifferentSkillLevel(Table skillsTable)
        {
            _skillsPage.DeleteAllSkills();
            var skillsToAdd = skillsTable.CreateSet<AddSkills>();
            foreach (var skill in skillsToAdd)
            {
                _skillsPage.AddNewSkillsAndLevel(skill.NewSkills, skill.SkillLevel);
            }
        }

        [When("I Add the same Skills and same SkillLevel:")]
        public void WhenIAddTheSameSkillsAndSameSkillLevel(Table skillsTable)
        {
            _skillsPage.DeleteAllSkills();
            var skillsToAdd = skillsTable.CreateSet<AddSkills>();
            foreach (var skill in skillsToAdd)
            {
                _skillsPage.AddNewSkillsAndLevel(skill.NewSkills, skill.SkillLevel);
            }
        }

        [Then("I should able to Cancel the operation and verify that no changes has happened")]
        public void ThenIShouldAbleToCancelTheOperationAndVerifyThatNoChangesHasHappened()
        {
            Assert.That(_skillsPage.IsCancelButtonNotDisplayed(), Is.True, $"Cancel button is Displayed!");
        }

        [Then("I should see the error message {string}")]
        public void ThenIShouldSeeTheErrorMessage(string errorMessage)
        {
            Assert.That(_skillsPage.IsErrorMessageDisplayed(errorMessage), Is.True, $"Error Message {errorMessage} should be displayed");
        }

        [When("I update the skill {string} with same value")]
        public void WhenIUpdateTheSkillWithSameValue(string skill)
        {
            _skillsPage.DeleteAllSkills();
            _skillsPage.AddNewSkillsAndLevel(skill);
            _skillsPage.UpdateSkillsAndLevel(skill, skill);
        }

        [When("I want to add any skills when the session is expired")]
        public void WhenIWantToAddAnySkillsWhenTheSessionIsExpired()
        {
            _skillsPage.DeleteAllSkills();
            _skillsPage.ExpireSession();
            _skillsPage.AddNewSkillsAndLevel("Gardening");
        }

        [When("I want to update any skills when the session is expired")]
        public void WhenIWantToUpdateAnySkillsWhenTheSessionIsExpired()
        {
           _skillsPage.DeleteAllSkills();
           _skillsPage.AddNewSkillsAndLevel("Gardening");
           _skillsPage.ExpireSession();
           _skillsPage.UpdateSkillsAndLevel("Gardening","Landscaping");
        }

        [When("I want to delete any skills when the session is expired")]
        public void WhenIWantToDeleteAnySkillsWhenTheSessionIsExpired()
        {
            _skillsPage.DeleteAllSkills();
            _skillsPage.AddNewSkillsAndLevel("Gardening");
            _skillsPage.ExpireSession();
            _skillsPage.DeleteSpecificSkills("Gardening");
        }
        
        private class AddSkills
        {
            public string NewSkills { get; set; }
            public string SkillLevel { get; set; }
        }

        private class UpdateSkills
        {
            public string ExistingSkill { get; set; }
            public string SkillToUpdate { get; set; }
            public string SkillLevelToUpdate { get; set; }
        }

        private class DeleteSkills
        {
            public string SkillsToBeDeleted { get; set; }
        }
    }
}
