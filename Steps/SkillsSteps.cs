using qa_dotnet_cucumber.Pages;
using Reqnroll;
using System.Linq;


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

        public SkillSteps(LoginPage loginPage, NavigationHelper navigationHelper, SkillPage skillPage, ScenarioContext scenarioContext)
        {
            _loginPage = loginPage;
            _navigationHelper = navigationHelper;
            _skillPage = skillPage;
            _scenarioContext = scenarioContext;
        }
        [Given("I navigate to the profile page as a registered user")]
        public void GivenINavigateToTheProfilePageAsARegisteredUser()
        {
            _navigationHelper.NavigateTo("Home");
            _loginPage.Login("ambikaarumugams@gmail.com", "AmbikaSenthil123");
            _skillPage.NavigateToTheProfilePage();
        }

        [When("I Add the following skills and select skill level:")]
        public void WhenIAddTheFollowingSkillsAndSelectSkillLevel(Table addSkillTable)
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
            _scenarioContext.Set(expectedAddSkills, "SkillsToCleanup");
        }

        [When("I should see the skills and verify it has been added successfully")]
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

        [Then("I should see the skills listed in my profile and verify it")]
        public void ThenIShouldSeeTheSkillsListedInMyProfileAndVerifyIt()
        {
            var actualSkillList = _skillPage.GetAllAddedSkillList();
            var expectedSkillList = _scenarioContext.Get<List<string>>("ExpectedAddSkills");
            Assert.That(actualSkillList, Is.EqualTo(expectedSkillList), "There is a mismatch!");  //Validation for expected and actual list
        }

        [When("I add the following skills and select their levels:")]
        public void WhenIAddTheFollowingSkillsAndSelectTheirLevels(Table addSkillToEditTable)
        {
            var addSkillToEditList = addSkillToEditTable.CreateSet<AddSkill>();
            var actualAddSkillsToEdit = new List<string>();
            var expectedAddSkillsToEdit = new List<string>();
            foreach (var addSkillToEdit in addSkillToEditList)
            {
                expectedAddSkillsToEdit.Add(addSkillToEdit.Skill);
                _skillPage.AddSkillAndLevel(addSkillToEdit.Skill,addSkillToEdit.SkillLevel);
                var successMessage = _skillPage.GetSuccessMessageForAddSkill(addSkillToEdit.Skill);
                actualAddSkillsToEdit.Add(successMessage);
            }
            _scenarioContext.Set(actualAddSkillsToEdit,"ActualAddSkillsToEdit");
            _scenarioContext.Set(expectedAddSkillsToEdit,"ExpectedAddSkillsToEdit");
        }

        [Then("I should see the added skills in the profile")]
        public void ThenIShouldSeeTheAddedSkillsInTheProfile()
        {
            var actualAddSkills = _scenarioContext.Get<List<string>>("ActualAddSkillsToEdit");
            var expectedAddSkills = _scenarioContext.Get<List<string>>("ExpectedAddSkillsToEdit");
            foreach (var expectedAddSkill in expectedAddSkills)
            {
                Assert.That(actualAddSkills.Any(actual=>actual.Contains(expectedAddSkill)),Is.True,$"Expected message contains {expectedAddSkill}, but not found");
            }
        }

        [Then("I update the following skills if they match the existing ones:")]
        public void ThenIUpdateTheFollowingSkillsIfTheyMatchTheExistingOnes(DataTable updateSkillTable)
        {
            var skillsToUpdateList= updateSkillTable.CreateSet<UpdateSkill>();
            var actualUpdateSkills = new List<string>();
            var expectedUpdateSkills = new List<string>();
            foreach (var addUpdateSkill in skillsToUpdateList)
            {
                expectedUpdateSkills.Add(addUpdateSkill.SkillToUpdate);
                _skillPage.UpdateSkillsAndLevel(addUpdateSkill.ExistingSkill,addUpdateSkill.SkillToUpdate,addUpdateSkill.SkillLevelToUpdate);
                var successMessageForUpdate = _skillPage.GetSuccessMessageForUpdateSkill(addUpdateSkill.SkillToUpdate);
                actualUpdateSkills.Add(successMessageForUpdate);
            }
            _scenarioContext.Set(actualUpdateSkills,"ActualUpdatedSkills");
            _scenarioContext.Set(expectedUpdateSkills,"ExpectedUpdatedSkills");
            _scenarioContext.Set(expectedUpdateSkills,"SkillsToCleanup");
        }

        [Then("I should see a success message and the updated skills in my profile")]
        public void ThenIShouldSeeASuccessMessageAndTheUpdatedSkillsInMyProfile()
        {
            var actualUpdateSkills = _scenarioContext.Get<List<string>>("ActualUpdatedSkills");
            var expectedUpdateSkills = _scenarioContext.Get<List<string>>("ExpectedUpdatedSkills");
            foreach (var expectedUpdateSkill in expectedUpdateSkills)
            {
                Assert.That(actualUpdateSkills.Any(actual => actual.Contains(expectedUpdateSkill)), Is.True,$"Expected message contains {expectedUpdateSkill}, but not found");
            }
           
        }

        [When("I click the delete icon corresponding to the following skills:")]
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
            _scenarioContext.Set(actualDeleteSkills,"ActualDeleteSkills");
            _scenarioContext.Set(expectedDeleteSkills,"ExpectedDeleteSkills");
        }

        [Then("I should see a success message for each deleted skill")]
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

        [Then("the skills table should be empty if all skills have been deleted")]
        public void ThenTheSkillsTableShouldBeEmptyIfAllSkillsHaveBeenDeleted()
        {
           _skillPage.DeleteAllSkills();
           Assert.That(_skillPage.IsSkillsTableEmpty(),Is.True,$"Skill table is not empty after deletion");
        }

        [When("I add skill as {string} and level as {string}")]
        public void WhenIAddSkillAsAndLevelAs(string skill, string skillLevel)
        {
            _skillPage.AddSkillAndLevel(skill,skillLevel);
            var successMessage = _skillPage.GetSuccessMessageForAddSkill(skill);
            Console.WriteLine(successMessage);
            _scenarioContext.Set(new List<string>{skill},"ActualMessage");
        }

        [When("when I update the skill and skill level:")]
        public void WhenWhenIUpdateTheSkillAndSkillLevel(Table updateExistingSkillAndDifferentLevelTable)
        {
            var updateExistingSkillAndDifferentLevelList=updateExistingSkillAndDifferentLevelTable.CreateInstance<UpdateSkill>();
            var actualSameSkillAndDifferentLevelToUpdate = new List<string>();
            var expectedSameSkillAndDifferentLevelToUpdate = new List<string>();

            _skillPage.UpdateSkillsAndLevel(updateExistingSkillAndDifferentLevelList.ExistingSkill,updateExistingSkillAndDifferentLevelList.SkillToUpdate,updateExistingSkillAndDifferentLevelList.SkillLevelToUpdate);
             expectedSameSkillAndDifferentLevelToUpdate.Add(updateExistingSkillAndDifferentLevelList.SkillToUpdate);
             var successMessage = _skillPage.GetSuccessMessageForUpdateSkill(updateExistingSkillAndDifferentLevelList.SkillToUpdate);
            actualSameSkillAndDifferentLevelToUpdate.Add(successMessage);


            _scenarioContext.Set(actualSameSkillAndDifferentLevelToUpdate, "ActualSameSkillAndDifferentLevelToUpdate");
            _scenarioContext.Set(expectedSameSkillAndDifferentLevelToUpdate,"ExpectedSkillAndDifferentLevelToUpdate");
            _scenarioContext.Set(expectedSameSkillAndDifferentLevelToUpdate,"SkillsToCleanup");
        }

        [Then("I should see the success message and updated skill in my profile")]
        public void ThenIShouldSeeTheSuccessMessageAndUpdatedSkillInMyProfile()
        {
            var actualList = _scenarioContext.Get<List<string>>("ActualSameSkillAndDifferentLevelToUpdate");
            var expectedList = _scenarioContext.Get<List<string>>("ExpectedSkillAndDifferentLevelToUpdate");
            var actualMessage = actualList.FirstOrDefault();
            var expectedMessage = expectedList.FirstOrDefault();
            Assert.That(actualMessage, Does.Contain(expectedMessage), $"Expected Message '{expectedMessage}' hasn't updated, but got '{actualMessage}'");
        }

       



       
        //[When("I click Add New button, leave the skill field empty,choose the skill level and click the Add button")]
        //public void WhenIClickAddNewButtonLeaveTheSkillFieldEmptyChooseTheSkillLevelAndClickTheAddButton()
        //{
        //    _skillsPage.DeleteAllSkills();
        //    _skillsPage.LeaveTheSkillFieldEmptyForAdd();
        //    _skillsPage.ClickAddButton();
        //}

        //[Then("I should see {string} error message")]
        //public void ThenIShouldSeeErrorMessage(string error)
        //{
        //    Assert.That(_skillsPage.IsErrorMessageDisplayed(error), Is.True, $"Error Message {error} should be displayed");
        //}

        //[When("I click Add New button, enter the skill field, not choosing the skill level and click the Add button")]
        //public void WhenIClickAddNewButtonEnterTheSkillFieldNotChoosingTheSkillLevelAndClickTheAddButton()
        //{
        //    _skillsPage.DeleteAllSkills();
        //    _skillsPage.NotChoosingSkillLevelForAdd();
        //    _skillsPage.ClickAddButton();
        //}

        //[When("I click Add New button, empty the skill field, not choosing the skill level and click the Add button")]
        //public void WhenIClickAddNewButtonEmptyTheSkillFieldNotChoosingTheSkillLevelAndClickTheAddButton()
        //{
        //    _skillsPage.DeleteAllSkills();
        //    _skillsPage.LeaveTheSkillFieldEmptyAndNotChoosingSkillLevelForAdd();
        //    _skillsPage.ClickAddButton();
        //}

        //[When("I add skill {string} and it's level {string}")]
        //public void WhenIAddSkillAndItsLevel(string skill, string level)
        //{
        //    _skillsPage.DeleteAllSkills();
        //    _skillsPage.AddNewSkillsAndLevel(skill,level);
        //}

        //[When("I click edit icon of {string}, leave the skill field empty,choose the skill level and click the Update button")]
        //public void WhenIClickEditIconOfLeaveTheSkillFieldEmptyChooseTheSkillLevelAndClickTheUpdateButton(string existingSkill)
        //{
        //    _skillsPage.LeaveTheSkillFieldEmptyForUpdate(existingSkill);
        //}

        //[When("I click edit icon of {string}, enter the skill field, not choosing the skill level and click the Update button")]
        //public void WhenIClickEditIconOfEnterTheSkillFieldNotChoosingTheSkillLevelAndClickTheUpdateButton(string existingSkill)
        //{
        //    _skillsPage.NotChoosingSkillLevelForUpdate(existingSkill);
        //}

        //[When("I click edit icon of {string}, empty the skill field, not choosing the skill level and click the Update button")]
        //public void WhenIClickEditIconOfEmptyTheSkillFieldNotChoosingTheSkillLevelAndClickTheUpdateButton(string existingSkill)
        //{
        //    _skillsPage.LeaveTheSkillFieldEmptyAndNotChoosingSkillLevelForUpdate(existingSkill);
        //}

        //[When("I click Add New Skill {string} with level {string}")]
        //public void WhenIClickAddNewSkillWithLevel(string skill, string level)
        //{
        //    _skillsPage.DeleteAllSkills();
        //    _skillsPage.AddNewSkillsAndLevel(skill, level);
        //}

        //[When("I click edit icon of {string} and Update skill to {string} and level to {string}")]
        //public void WhenIClickEditIconOfAndUpdateSkillToAndLevelTo(string skillToUpdate, string skill, string level)
        //{
        //    _skillsPage.EnterSkillsAndLevelToUpdate(skillToUpdate, skill, level);
        //}

        //[When("I click cancel")]
        //public void WhenIClickCancel()
        //{
        //    _skillsPage.ClickCancelUpdate();
        //}

        //[Then("the skill {string} should remain unchanged with level {string}")]
        //public void ThenTheSkillShouldRemainUnchangedWithLevel(string skill, string level)
        //{
        //    Assert.That(_skillsPage.GetLevelOfSkill(skill),Is.EqualTo(level)  , $"Skill is Updated!");
        //}

        //[When("I click Add New button, enter the Skill {string} and it's level {string}")]
        //public void WhenIClickAddNewButtonEnterTheSkillAndItsLevel(string skill, string level)
        //{
        //    _skillsPage.DeleteAllSkills();
        //    _skillsPage.EnterNewSkillsAndLevelToAdd(skill, level);
        //}

        //[Then("I should able to Cancel the operation and verify that the skill {string} shouldn't be added")]
        //public void ThenIShouldAbleToCancelTheOperationAndVerifyThatTheSkillShouldntBeAdded(string skill)
        //{
        //    _skillsPage.ClickCancelButton();
        //    Assert.That(_skillsPage.IsSkillNotAdded(skill),Is.True,$"{skill} is Added!");
        //}

        //[When("I Add the same Skills and different SkillLevel:")]
        //public void WhenIAddTheSameSkillsAndDifferentSkillLevel(Table skillsTable)
        //{
        //    _skillsPage.DeleteAllSkills();
        //    var skillsToAdd = skillsTable.CreateSet<AddSkills>();
        //    foreach (var skill in skillsToAdd)
        //    {
        //        _skillsPage.AddNewSkillsAndLevel(skill.NewSkills, skill.SkillLevel);
        //    }
        //}

        //[When("I Add the same Skills and same SkillLevel:")]
        //public void WhenIAddTheSameSkillsAndSameSkillLevel(Table skillsTable)
        //{
        //    _skillsPage.DeleteAllSkills();
        //    var skillsToAdd = skillsTable.CreateSet<AddSkills>();
        //    foreach (var skill in skillsToAdd)
        //    {
        //        _skillsPage.AddNewSkillsAndLevel(skill.NewSkills, skill.SkillLevel);
        //    }
        //}

        //[Then("I should able to Cancel the operation and verify that no changes has happened")]
        //public void ThenIShouldAbleToCancelTheOperationAndVerifyThatNoChangesHasHappened()
        //{
        //    Assert.That(_skillsPage.IsCancelButtonNotDisplayed(), Is.True, $"Cancel button is Displayed!");
        //}

        //[Then("I should see the error message {string}")]
        //public void ThenIShouldSeeTheErrorMessage(string errorMessage)
        //{
        //    Assert.That(_skillsPage.IsErrorMessageDisplayed(errorMessage), Is.True, $"Error Message {errorMessage} should be displayed");
        //}

        //[When("I update the skill {string} with same value")]
        //public void WhenIUpdateTheSkillWithSameValue(string skill)
        //{
        //    _skillsPage.DeleteAllSkills();
        //    _skillsPage.AddNewSkillsAndLevel(skill);
        //    _skillsPage.UpdateSkillsAndLevel(skill, skill);
        //}

        //[When("I want to add any skills when the session is expired")]
        //public void WhenIWantToAddAnySkillsWhenTheSessionIsExpired()
        //{
        //    _skillsPage.DeleteAllSkills();
        //    _skillsPage.ExpireSession();
        //    _skillsPage.AddNewSkillsAndLevel("Gardening");
        //}

        //[When("I want to update any skills when the session is expired")]
        //public void WhenIWantToUpdateAnySkillsWhenTheSessionIsExpired()
        //{
        //   _skillsPage.DeleteAllSkills();
        //   _skillsPage.AddNewSkillsAndLevel("Gardening");
        //   _skillsPage.ExpireSession();
        //   _skillsPage.UpdateSkillsAndLevel("Gardening","Landscaping");
        //}

        //[When("I want to delete any skills when the session is expired")]
        //public void WhenIWantToDeleteAnySkillsWhenTheSessionIsExpired()
        //{
        //    _skillsPage.DeleteAllSkills();
        //    _skillsPage.AddNewSkillsAndLevel("Gardening");
        //    _skillsPage.ExpireSession();
        //    _skillsPage.DeleteSpecificSkill("Gardening");
        //}
     
       


        private class AddSkill
        {
            public string Skill { get; set; }
            public string SkillLevel { get; set; }
        }

        private class UpdateSkill
        {
            public string ExistingSkill { get; set; }
            public string SkillToUpdate { get; set; }
            public string SkillLevelToUpdate { get; set; }
        }

        private class DeleteSkill
        {
            public string SkillToBeDeleted { get; set; }
        }
    }
}
