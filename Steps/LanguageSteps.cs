using DocumentFormat.OpenXml.Drawing.Charts;
using qa_dotnet_cucumber.Pages;
using RazorEngine;
using Reqnroll;

namespace qa_dotnet_cucumber.Steps
{
    [Binding]
    [Scope(Feature = "Language")]
    [Scope(Feature = "LanguageNegative")]
    public class LanguageSteps
    {
        private readonly LoginPage _loginPage;
        private readonly NavigationHelper _navigationHelper;
        private readonly LanguagePage _languagePage;
        private readonly ScenarioContext _scenarioContext;

        //Constructor
        public LanguageSteps(LoginPage loginPage, NavigationHelper navigationHelper, LanguagePage languagePage, ScenarioContext scenarioContext)
        {
            _loginPage = loginPage;
            _navigationHelper = navigationHelper;
            _languagePage = languagePage;
            _scenarioContext = scenarioContext;
        }

        [Given("I navigate to the profile page as a registered user")]    //Login and navigated to the profile page
        public void GivenINavigateToTheProfilePageAsARegisteredUser()
        {
            _navigationHelper.NavigateTo("Home");
            _loginPage.Login("ambikaarumugams@gmail.com", "AmbikaSenthil123");
            _languagePage.NavigateToTheProfilePage();
        }

        [When("I Add the following New Language and select New Language level:")]   //Adding new language and it's level 
        public void WhenIAddTheFollowingNewLanguageAndSelectNewLanguageLevel(Table languageTable)
        {
            _languagePage.DeleteAllLanguages(); //Delete all the languages in the list before adding new

            var languagesToAdd = languageTable.CreateSet<AddLanguage>();
            var actualAddLanguages = new List<string>();
            var expectedAddLanguages = new List<string>();
            foreach (var addNewList in languagesToAdd)
            {
                expectedAddLanguages.Add(addNewList.NewLanguage);
                _languagePage.AddNewLanguageAndLevel(addNewList.NewLanguage, addNewList.NewLanguageLevel);
                var successMessageAfterLanguageIsBeingAdded = _languagePage.GetSuccessMessageForAddNew(addNewList.NewLanguage);
                actualAddLanguages.Add(successMessageAfterLanguageIsBeingAdded);
            }
            _scenarioContext.Set(actualAddLanguages, "ActualAddLanguages");
            _scenarioContext.Set(expectedAddLanguages, "ExpectedAddLanguages");
            _scenarioContext.Set(languageTable,"ExpectedTable");
        }

        [When("I should see the languages and verify it has been added successfully")]   //Success message validation
        public void WhenIShouldSeeTheLanguagesAndVerifyItHasBeenAddedSuccessfully()
        {
            var actualAddLanguages = _scenarioContext.Get<List<string>>("ActualAddLanguages");
            var expectedAddLanguages = _scenarioContext.Get<List<string>>("ExpectedAddLanguages");

            foreach (var expectedAddList in expectedAddLanguages)
            {
                Assert.That(actualAddLanguages.Any(actual => actual.Contains(expectedAddList)),
                Is.True, $"Expected a message contains'{expectedAddList}',but not found");
            }
        }

        [Then("I should see the languages listed in my profile and verify it")]   //Table data list validation after adding
        public void ThenIShouldSeeTheLanguagesListedInMyProfileAndVerifyIt()
        {
            //var actual = _languagePage.GetAllAddedLanguages();
            //var expectedAddLanguages = _scenarioContext.Get<List<string>>("ExpectedAddLanguages");
            //Assert.That(actual, Is.EqualTo(expectedAddLanguages), "There is a mismatch");
            var actual = _languagePage.GetAddedLanguagesAndLevel();
            var expectedTable = _scenarioContext.Get<Table>("ExpectedTable");
            expectedTable.CompareToSet(actual);
        }

        [When("I add the following languages and select their levels:")]
        public void WhenIAddTheFollowingLanguagesAndSelectTheirLevels(Table addLanguageToEditTable)
        {
            _languagePage.DeleteAllLanguages();
            var addLanguageToEdit = addLanguageToEditTable.CreateSet<AddLanguage>();
            var actualAddLanguages = new List<string>();
            var expectedAddLanguages = new List<string>();
            foreach (var addLanguage in addLanguageToEdit)
            {
                expectedAddLanguages.Add(addLanguage.NewLanguage);
               _languagePage.AddNewLanguageAndLevel(addLanguage.NewLanguage,addLanguage.NewLanguageLevel);
               var successMessage = _languagePage.GetSuccessMessageForAddNew(addLanguage.NewLanguage);
               actualAddLanguages.Add(successMessage);
            }
            _scenarioContext.Set(expectedAddLanguages,"ExpectedLanguagesForAdd");
            _scenarioContext.Set(actualAddLanguages,"ActualLanguagesForAdd");
        }

        [Then("I should see the added languages in the profile")]
        public void ThenIShouldSeeTheAddedLanguagesInTheProfile()
        {
            var actualList=_scenarioContext.Get<List<string>>("ActualLanguagesForAdd");
            var expectedList = _scenarioContext.Get<List<string>>("ExpectedLanguagesForAdd");
            foreach (var expected in expectedList)
            {
                Assert.That(actualList.Any(actual => actual.Contains(expected)), Is.True,
                  $"Expected a message contains'{expected}',but not found");
            }
        }

        [Then("I update the following languages if they match the existing ones:")]
        public void ThenIUpdateTheFollowingLanguagesIfTheyMatchTheExistingOnes(Table updateLanguageTable)
        {
            var languagesToUpdate = updateLanguageTable.CreateSet<UpdateLanguage>();
            var actualUpdatedLanguages = new List<string>();
            var expectedUpdatedLanguages = new List<string>();
            foreach (var addUpdateList in languagesToUpdate)
            {
                _languagePage.UpdateLanguageAndLevel(addUpdateList.ExistingLanguage, addUpdateList.LanguageToUpdate, addUpdateList.LanguageLevelToUpdate);
                var successMessageForUpdate = _languagePage.GetSuccessMessageForUpdate(addUpdateList.LanguageToUpdate);
                Console.WriteLine(successMessageForUpdate);
                actualUpdatedLanguages.Add(successMessageForUpdate);
                expectedUpdatedLanguages.Add(addUpdateList.LanguageToUpdate);
            }
            _scenarioContext.Set(actualUpdatedLanguages, "ActualUpdatedLanguages");
            _scenarioContext.Set(expectedUpdatedLanguages, "ExpectedUpdatedLanguages");
        }
        [Then("I should see a success message and the updated languages in my profile")]  //success message and table data list validation after updating
        public void ThenIShouldSeeASuccessMessageAndTheUpdatedLanguagesInMyProfile()
        {
            var actualUpdatedLanguages = _scenarioContext.Get<List<string>>("ActualUpdatedLanguages");
            var expectedUpdatedLanguages = _scenarioContext.Get<List<string>>("ExpectedUpdatedLanguages");

            foreach (var expectedUpdateLanguage in expectedUpdatedLanguages)
            {
                Assert.That(actualUpdatedLanguages.Any(actual => actual.Contains(expectedUpdateLanguage)),
                    Is.True, $"Expected a message contains'{expectedUpdateLanguage}',but not found");
            }
            Assert.That(_languagePage.GetAllUpdatedLanguages(), Is.SupersetOf(expectedUpdatedLanguages), "The language hasn't updated successfully");
        }

       [When("I click the delete icon corresponding to the following languages:")]   //To delete the languages
        public void WhenIClickTheDeleteIconCorrespondingToTheFollowingLanguages(Table deleteLanguageTable)
        {
            var languagesToDelete = deleteLanguageTable.CreateSet<DeleteLanguage>();
            var expectedLanguagesToDelete = new List<string>();
            var actualDeletedLanguages = new List<string>();
            foreach (var deleteList in languagesToDelete)
            {
                expectedLanguagesToDelete.Add(deleteList.LanguageToBeDeleted);
                _languagePage.DeleteSpecificLanguage(deleteList.LanguageToBeDeleted);
                var deleteSuccessMessage = _languagePage.GetSuccessMessageForDelete(deleteList.LanguageToBeDeleted);
                actualDeletedLanguages.Add(deleteSuccessMessage);
            }
            _scenarioContext.Set(actualDeletedLanguages, "ActualDeletedLanguages");
            _scenarioContext.Set(expectedLanguagesToDelete, "ExpectedLanguagesToDelete");
        }

        [Then("I should see a success message for each deleted language")]   //Success message for deleting the languages
        public void ThenIShouldSeeASuccessMessageForEachDeletedLanguage()
        {
            var actualDeletedLanguages = _scenarioContext.Get<List<string>>("ActualDeletedLanguages");
            var expectedLanguagesToDelete = _scenarioContext.Get<List<string>>("ExpectedLanguagesToDelete");
            foreach (var expectedLanguageToDelete in expectedLanguagesToDelete)
            {
                Assert.That(actualDeletedLanguages.Any(actual => actual.Contains(expectedLanguageToDelete)),
               Is.True, $"Expected a message contains'{expectedLanguageToDelete}',but not found");
            }
        }

        [Then("the languages table should be empty if all languages have been deleted")]   //To delete all the languages and check the table is empty 
        public void ThenTheLanguagesTableShouldBeEmptyIfAllLanguagesHaveBeenDeleted()
        {
            _languagePage.DeleteAllLanguages();
            Assert.That(_languagePage.IsLanguageTableEmpty(), Is.True, "Language table is not empty after deletions.");
        }

        [When("I Add an existing language and select a language level:")]
        public void WhenIAddAnExistingLanguageAndSelectALanguageLevel(Table languageTable)
        {
            _languagePage.DeleteAllLanguages(); //Delete all the languages in the list before adding new

            var languagesToAdd = languageTable.CreateSet<AddLanguage>();
            foreach (var addNewList in languagesToAdd)
            {
                _languagePage.AddNewLanguageAndLevel(addNewList.NewLanguage, addNewList.NewLanguageLevel);
            }
        }

        [When("I click Add New button, leave the language field empty,choose the language level and click the Add button")]
        public void WhenIClickAddNewButtonLeaveTheLanguageFieldEmptyChooseTheLanguageLevelAndClickTheAddButton()
        {
            _languagePage.DeleteAllLanguages();
            _languagePage.LeaveTheLanguageFieldEmptyForAdd();
        }

        [When("I click Add New button, enter the language field, not choosing the language level and click the Add button")]
        public void WhenIClickAddNewButtonEnterTheLanguageFieldNotChoosingTheLanguageLevelAndClickTheAddButton()
        {
            _languagePage.DeleteAllLanguages();
            _languagePage.NotChoosingLanguageLevelForAdd();
        }

        [When("I click Add New button, empty the language field, not choosing the language level and click the Add button")]
        public void WhenIClickAddNewButtonEmptyTheLanguageFieldNotChoosingTheLanguageLevelAndClickTheAddButton()
        {
            _languagePage.DeleteAllLanguages();
            _languagePage.LeaveTheLanguageFieldEmptyAndNotChoosingLanguageLevelForAdd();
        }

        [When("I add language {string} and it's level {string}")]
        public void WhenIAddLanguageAndItsLevel(string language, string level)
        {
            _languagePage.DeleteAllLanguages();
            _languagePage.AddNewLanguageAndLevel(language, level);
            Thread.Sleep(3000);
        }

        [When("I click edit icon of {string}, leave the language field empty,choose the language level and click the Update button")]
        public void WhenIClickEditIconOfLeaveTheLanguageFieldEmptyChooseTheLanguageLevelAndClickTheUpdateButton(string existingLanguage)
        {
            _languagePage.LeaveTheLanguageFieldEmptyForUpdate(existingLanguage);
        }

        [When("I click edit icon of {string}, enter the language field, not choosing the language level and click the Update button")]
        public void WhenIClickEditIconOfEnterTheLanguageFieldNotChoosingTheLanguageLevelAndClickTheUpdateButton(string existingLanguage)
        {
            _languagePage.NotChoosingLanguageLevelForUpdate(existingLanguage);
        }

        [When("I click edit icon of {string}, empty the language field, not choosing the language level and click the Update button")]
        public void WhenIClickEditIconOfEmptyTheLanguageFieldNotChoosingTheLanguageLevelAndClickTheUpdateButton(string existingLanguage)
        {
            _languagePage.LeaveTheLanguageFieldEmptyAndNotChoosingLanguageLevelForUpdate(existingLanguage);
        }

        [Then("I should see {string} error message")]
        public void ThenIShouldSeeErrorMessage(string error)
        {
            Assert.That(_languagePage.IsErrorMessageDisplayed(error), Is.True, $"Error Message '{error}' shouldn't be displayed");
        }

        [When("I click Add New button, enter the language {string} and it's level {string}")]
        public void WhenIClickAddNewButtonEnterTheLanguageAndItsLevel(string languageToCancel, string levelToCancel)
        {
            _languagePage.DeleteAllLanguages();
            _languagePage.EnterNewLanguageAndLevelToAdd(languageToCancel, levelToCancel);
        }

        [Then("I should able to Cancel the operation and verify that the language {string} shouldn't be added")]
        public void ThenIShouldAbleToCancelTheOperationAndVerifyThatTheLanguageShouldntBeAdded(string language)
        {
            _languagePage.ClickCancelButton();
            Assert.That(_languagePage.IsLanguageNotAdded(language), Is.True, $"{language} is added!");
        }

        [When("I click edit icon of {string} and Update level to {string} and level to {string}")]
        public void WhenIClickEditIconOfAndUpdateLevelToAndLevelTo(string languageToUpdate, string language, string level)
        {
            _languagePage.EnterLanguageAndLevelToUpdate(languageToUpdate, language, level);
        }

        [When("I click cancel")]
        public void WhenIClickCancel()
        {
            _languagePage.ClickCancelUpdate();
        }

        [Then("the language {string} should remain unchanged with level {string}")]
        public void ThenTheLanguageShouldRemainUnchangedWithLevel(string language, string level)
        {
           Assert.That(_languagePage.GetLevelOfLanguage(language), Is.EqualTo(level), $"{language} is updated!");
        }

        [Then("I should able to Cancel the operation and verify that no changes has happened")]
        public void ThenIShouldAbleToCancelTheOperationAndVerifyThatNoChangesHasHappened()
        {
            Assert.That(_languagePage.IsCancelButtonNotDisplayed(), Is.True, $"Cancel button is Displayed!");
        }

        [When("I update the language {string} with same value")]
        public void WhenIUpdateTheLanguageWithSameValue(string newLanguage)
        {
            _languagePage.DeleteAllLanguages();
            _languagePage.AddNewLanguageAndLevel(newLanguage, "Basic");
            _languagePage.UpdateLanguageAndLevelWithSameValue(newLanguage, newLanguage);
        }
        [When("I want to add language as {string} and level as {string} when the session is expired")]
        public void WhenIWantToAddLanguageAsAndLevelAsWhenTheSessionIsExpired(string languageToAdd, string levelToAdd)
        {
            _languagePage.DeleteAllLanguages();
            _languagePage.ExpireSession();
            _languagePage.AddNewLanguageAndLevel(languageToAdd, levelToAdd);
        }

        [When("I add language as {string} and level as {string}")]
        public void WhenIAddLanguageAsAndLevelAs(string oldLanguage, string oldLevel)
        {
            _languagePage.DeleteAllLanguages();
            _languagePage.AddNewLanguageAndLevel(oldLanguage, oldLevel);

        }

        [When("I want to update language the existing language as{string}, language to update as {string},and level to update as {string} when the session is expired")]
        public void WhenIWantToUpdateLanguageTheExistingLanguageAsLanguageToUpdateAsAndLevelToUpdateAsWhenTheSessionIsExpired(string existingLanguage, string languageToUpdate, string levelToUpdate)
        {
            _languagePage.ExpireSession();
            _languagePage.UpdateLanguageAndLevel(existingLanguage, languageToUpdate, levelToUpdate);
        }
        [When("I add language {string} and level as {string}")]
        public void WhenIAddLanguageAndLevelAs(string language, string level)
        {
            _languagePage.DeleteAllLanguages();
            _languagePage.AddNewLanguageAndLevel(language, level);
        }

        [When("I want to delete language {string} when the session is expired")]
        public void WhenIWantToDeleteLanguageWhenTheSessionIsExpired(string languageToDelete)
        {
            _languagePage.ExpireSession();
            _languagePage.DeleteSpecificLanguage(languageToDelete);
        }

        [When("I try to add a language {string} with level {string}")]
        public void WhenITryToAddALanguageWithLevel(string language, string level)
        {
            _languagePage.DeleteAllLanguages();
           
            _languagePage.AddNewLanguageAndLevel(language,level);
        }

        [Then("I should see the error message {string}")]
        public void ThenIShouldSeeTheErrorMessage(string p0)
        {
            throw new PendingStepException();
        }


    }
}

public class AddLanguage    //Property class to add language
{
    public string NewLanguage { get; set; }
    public string NewLanguageLevel { get; set; }
}

public class UpdateLanguage    //Property class to update language
{
    public string ExistingLanguage { get; set; }
    public string LanguageToUpdate { get; set; }
    public string LanguageLevelToUpdate { get; set; }
}

public class DeleteLanguage   //Property class to delete language
{
    public string LanguageToBeDeleted { get; set; }
}