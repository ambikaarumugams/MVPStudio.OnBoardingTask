using qa_dotnet_cucumber.Pages;
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

        [When("I Add the following language and select language level:")]
        public void WhenIAddTheFollowingLanguageAndSelectLanguageLevel(Table languageTable)    //Get the language table from the feature file
        {
            var languagesToAddList = languageTable.CreateSet<AddLanguage>();
            var actualAddLanguages = new List<string>();
            var expectedAddLanguages = new List<string>();
            foreach (var language in languagesToAddList)
            {
                expectedAddLanguages.Add(language.Language);      //add the input language as an expected and add it to the expectedAddLanguages List
                _languagePage.AddNewLanguageAndLevel(language.Language, language.LanguageLevel);
                var successMessageAfterLanguageIsBeingAdded = _languagePage.GetSuccessMessageForAddNewLanguage(language.Language);
                actualAddLanguages.Add(successMessageAfterLanguageIsBeingAdded);    // add the success message to the actualAddLanguages List
            }
            _scenarioContext.Set(actualAddLanguages, "ActualAddLanguages");   //Store it in scenarioContext for assertion 
            _scenarioContext.Set(expectedAddLanguages, "ExpectedAddLanguages");  //Store it in scenarioContext for assertion
            _scenarioContext.Set(expectedAddLanguages, "LanguagesToCleanup");    //Store the languages that I've added to retrieve it in hooks to clean up the stored data
          //_scenarioContext.Set(languageTable, "ExpectedTable");   // To set values in scenario context as a table for assertion
        }

        [When("I should see the languages and verify it has been added successfully")]   //Success message validation
        public void WhenIShouldSeeTheLanguagesAndVerifyItHasBeenAddedSuccessfully()
        {
            var actualLanguages = _scenarioContext.Get<List<string>>("ActualAddLanguages");     //Get the value by using key which I've stored in the previous step
            var expectedLanguages = _scenarioContext.Get<List<string>>("ExpectedAddLanguages");  //Get the value by using key which I've stored in the previous step

            foreach (var expectedLanguage in expectedLanguages)     //Assertion one by one
            {
                Assert.That(actualLanguages.Any(actual => actual.Contains(expectedLanguage)),
                Is.True, $"Expected message contains'{expectedLanguage}',but not found");
            }
        }

        [Then("I should see the languages listed in my profile and verify it")]   //Table data list validation after adding
        public void ThenIShouldSeeTheLanguagesListedInMyProfileAndVerifyIt()
        {
            var actual = _languagePage.GetAllAddedLanguages();
            var expectedAddLanguages = _scenarioContext.Get<List<string>>("ExpectedAddLanguages");
            Assert.That(actual, Is.EqualTo(expectedAddLanguages), "There is a mismatch");
            //var actual = _languagePage.GetAddedLanguagesAndLevel();
            //var expectedTable = _scenarioContext.Get<Table>("ExpectedTable");
            //expectedTable.CompareToSet(actual);
        }

        [When("I add the following languages and select their levels:")]     //To add languages to update the existing language
        public void WhenIAddTheFollowingLanguagesAndSelectTheirLevels(Table addLanguageToEditTable)
        {
            var addLanguageToEditList = addLanguageToEditTable.CreateSet<AddLanguage>();
            var actualAddLanguagesToEdit = new List<string>();
            var expectedAddLanguagesToEdit = new List<string>();
            foreach (var addLanguageToEdit in addLanguageToEditList)
            {
                expectedAddLanguagesToEdit.Add(addLanguageToEdit.Language);
                _languagePage.AddNewLanguageAndLevel(addLanguageToEdit.Language, addLanguageToEdit.LanguageLevel);
                var successMessage = _languagePage.GetSuccessMessageForAddNewLanguage(addLanguageToEdit.Language);
                actualAddLanguagesToEdit.Add(successMessage);
            }
            _scenarioContext.Set(expectedAddLanguagesToEdit, "ExpectedAddLanguagesToEdit");
            _scenarioContext.Set(actualAddLanguagesToEdit, "ActualAddLanguagesToEdit");
        }

        [Then("I should see the added language in the profile")]   //Validation for add
        public void ThenIShouldSeeTheAddedLanguageInTheProfile()
        {
            var actualAddLanguages = _scenarioContext.Get<List<string>>("ActualAddLanguagesToEdit");
            var expectedAddLanguages = _scenarioContext.Get<List<string>>("ExpectedAddLanguagesToEdit");
            foreach (var expectedAddLanguage in expectedAddLanguages)
            {
                Assert.That(actualAddLanguages.Any(actual => actual.Contains(expectedAddLanguage)), Is.True,
                  $"Expected message contains'{expectedAddLanguage}',but not found");
            }
        }

        [Then("I update the following languages if they match the existing ones:")]   //To update the existing languages 
        public void ThenIUpdateTheFollowingLanguagesIfTheyMatchTheExistingOnes(Table updateLanguageTable)
        {
            var languagesToUpdateList = updateLanguageTable.CreateSet<UpdateLanguage>();
            var actualUpdateLanguages = new List<string>();
            var expectedUpdateLanguages = new List<string>();
            foreach (var addUpdateList in languagesToUpdateList)
            {
                _languagePage.UpdateLanguageAndLevel(addUpdateList.ExistingLanguage, addUpdateList.LanguageToUpdate, addUpdateList.LanguageLevelToUpdate);
                var successMessageForUpdate = _languagePage.GetSuccessMessageForUpdate(addUpdateList.LanguageToUpdate);
                actualUpdateLanguages.Add(successMessageForUpdate);
                expectedUpdateLanguages.Add(addUpdateList.LanguageToUpdate);
            }
            _scenarioContext.Set(actualUpdateLanguages, "ActualUpdatedLanguages");
            _scenarioContext.Set(expectedUpdateLanguages, "ExpectedLanguagesToUpdate");
            _scenarioContext.Set(expectedUpdateLanguages, "LanguagesToCleanup");   //Clean up 
        }

        [Then("I should see a success message and the updated languages in my profile")]  //success message and table data list validation after updating
        public void ThenIShouldSeeASuccessMessageAndTheUpdatedLanguagesInMyProfile()
        {
            var actualUpdatedLanguages = _scenarioContext.Get<List<string>>("ActualUpdatedLanguages");
            var expectedUpdatedLanguages = _scenarioContext.Get<List<string>>("ExpectedLanguagesToUpdate");

            foreach (var expectedUpdateLanguage in expectedUpdatedLanguages)
            {
                Assert.That(actualUpdatedLanguages.Any(actual => actual.Contains(expectedUpdateLanguage)),
                    Is.True, $"Expected message contains'{expectedUpdateLanguage}',but not found");
            }
            Assert.That(_languagePage.GetAllUpdatedLanguages(), Is.SupersetOf(expectedUpdatedLanguages), "The language hasn't updated successfully");
        }

        [When("I click the delete icon corresponding to the following languages:")]   //To delete the languages
        public void WhenIClickTheDeleteIconCorrespondingToTheFollowingLanguages(Table deleteLanguageTable)
        {
            var languagesToDeleteList = deleteLanguageTable.CreateSet<DeleteLanguage>();
            var expectedDeleteLanguages = new List<string>();
            var actualDeleteLanguages = new List<string>();
            foreach (var languageToDelete in languagesToDeleteList)
            {
                expectedDeleteLanguages.Add(languageToDelete.LanguageToBeDeleted);
                _languagePage.DeleteSpecificLanguage(languageToDelete.LanguageToBeDeleted);
                var deleteSuccessMessage = _languagePage.GetSuccessMessageForDelete(languageToDelete.LanguageToBeDeleted);
                actualDeleteLanguages.Add(deleteSuccessMessage);
            }
            _scenarioContext.Set(actualDeleteLanguages, "ActualDeleteLanguages");
            _scenarioContext.Set(expectedDeleteLanguages, "ExpectedDeleteLanguages");
        }

        [Then("I should see a success message for each deleted language")]   //Validation for deleting the languages
        public void ThenIShouldSeeASuccessMessageForEachDeletedLanguage()
        {
            var actualDeletedLanguages = _scenarioContext.Get<List<string>>("ActualDeleteLanguages");
            var expectedDeleteLanguages = _scenarioContext.Get<List<string>>("ExpectedDeleteLanguages");
            foreach (var expectedDeleteLanguage in expectedDeleteLanguages)
            {
                Assert.That(actualDeletedLanguages.Any(actual => actual.Contains(expectedDeleteLanguage)),
               Is.True, $"Expected a message contains'{expectedDeleteLanguage}',but not found");
            }
        }

        [Then("the languages table should be empty if all languages have been deleted")]   //To delete all the languages and check the table is empty 
        public void ThenTheLanguagesTableShouldBeEmptyIfAllLanguagesHaveBeenDeleted()
        {
            _languagePage.DeleteAllLanguages();
            Assert.That(_languagePage.IsLanguageTableEmpty(), Is.True, "Language table is not empty after deletions.");
        }

        [When("I add language as {string} and level as {string}")]
        public void WhenIAddLanguageAsAndLevelAs(string language, string level) //Add the language and level
        {
            _languagePage.AddNewLanguageAndLevel(language, level);
            var successMessage = _languagePage.GetSuccessMessageForAddNewLanguage(language);
            Console.WriteLine(successMessage);
            _scenarioContext.Set(new List<string>{language},"ActualMessage");     //I'm storing this to clean up in different step
        }

        [When("when I update the language and language level:")]       //Update the existing language and select different language level
        public void WhenWhenIUpdateTheLanguageAndLanguageLevel(Table updateExistingLanguageAndDifferentLevelTable)
        {
            var updateExistingLanguageAndLevel = updateExistingLanguageAndDifferentLevelTable.CreateInstance<UpdateLanguage>();
            var actualSameLanguageAndDifferentLevelToUpdate = new List<string>();
            var expectedSameLanguageAndDifferentLevelForUpdate = new List<string>();

            _languagePage.UpdateLanguageAndLevel(updateExistingLanguageAndLevel.ExistingLanguage, updateExistingLanguageAndLevel.LanguageToUpdate, updateExistingLanguageAndLevel.LanguageLevelToUpdate);
            expectedSameLanguageAndDifferentLevelForUpdate.Add(updateExistingLanguageAndLevel.LanguageToUpdate);
            var successMessage = _languagePage.GetSuccessMessageForUpdate(updateExistingLanguageAndLevel.LanguageToUpdate);
            actualSameLanguageAndDifferentLevelToUpdate.Add(successMessage);

            _scenarioContext.Set(actualSameLanguageAndDifferentLevelToUpdate, "ActualSameLanguageAndDifferentLevelForUpdate");
            _scenarioContext.Set(expectedSameLanguageAndDifferentLevelForUpdate, "ExpectedSameLanguageAndDifferentLevelForUpdate");
            _scenarioContext.Set(expectedSameLanguageAndDifferentLevelForUpdate, "LanguagesToCleanup");    //Clean up
        }

        [Then("I should see the success message and updated language in my profile")]     //Success message for update the same language and different level
        public void ThenIShouldSeeTheSuccessMessageAndUpdatedLanguageInMyProfile()
        {
            var actualList = _scenarioContext.Get<List<string>>("ActualSameLanguageAndDifferentLevelForUpdate");
            var expectedList = _scenarioContext.Get<List<string>>("ExpectedSameLanguageAndDifferentLevelForUpdate");
            var actualMessage = actualList.FirstOrDefault();  
            var expectedMessage = expectedList.FirstOrDefault();
            Assert.That(actualMessage,Does.Contain(expectedMessage), $"Expected Message '{expectedMessage}' hasn't updated, but got '{actualMessage}'");
        }

        [When("I try to add a language {string} with level {string}")]     //Add invalid languages like random strings, numbers, special characters and empty space
        public void WhenITryToAddALanguageWithLevel(string language, string level)
        {

            if (language == "<space>")    //Condition for empty space
            {
                language = "    ";
            }

            _languagePage.AddNewLanguageAndLevel(language, level);
            var successMessage = _languagePage.GetSuccessMessageForAddNewLanguage(language);
            _scenarioContext.Set(successMessage, "ActualLanguages");
            _scenarioContext.Set(new List<string> { language }, "LanguagesToCleanup");  //Clean up 
        }

        [When("I add a language with {int} characters and level {string}")]     //Destructive testing to add language
        public void WhenIAddALanguageWithCharactersAndLevel(int length, string level)
        {
            var longLanguageName = new string('a', length);    //Passing the length to create a string
            _languagePage.AddNewLanguageAndLevel(longLanguageName,level);
            var successMessage = _languagePage.GetSuccessMessageForAddNewLanguage(longLanguageName);
            _scenarioContext.Set(successMessage, "ActualLanguages");
            _scenarioContext.Set(new List<string> { longLanguageName }, "LanguagesToCleanup");  //Clean up
        }

        [Then("I should see the error message {string}")]   //Validation for invalid languages
        public void ThenIShouldSeeTheErrorMessage(string expected)
        {
            var actualLanguages = _scenarioContext.Get<string>("ActualLanguages");
            Assert.That(actualLanguages.Contains(expected), Is.True, $"Expected message contains '{expected}', but got '{actualLanguages}'");
        }

        [When("I click Add New button, enter the language {string} and it's level {string}")]    //To cancel the add language and level
        public void WhenIClickAddNewButtonEnterTheLanguageAndItsLevel(string languageToCancel, string levelToCancel)
        {
            _languagePage.EnterLanguageAndLevelToCancelAdd(languageToCancel, levelToCancel);
        }

        [Then("I should able to Cancel the operation and verify that the language {string} shouldn't be added")]   //Validation message for cancel
        public void ThenIShouldAbleToCancelTheOperationAndVerifyThatTheLanguageShouldntBeAdded(string language)
        {
            _languagePage.ClickCancelButton();
            Assert.That(_languagePage.IsLanguageNotAdded(language), Is.True, $"{language} is added!");
        }

        [When("I add language {string} and it's level {string}")]
        public void WhenIAddLanguageAndItsLevel(string language, string level)
        {
            _languagePage.AddNewLanguageAndLevel(language, level);
            _scenarioContext.Set(new List<string>{language},"LanguagesToCleanup");   //Clean up
        }

        [When("I click edit icon of {string} and Update level to {string} and level to {string}")]    //To cancel the update
        public void WhenIClickEditIconOfAndUpdateLevelToAndLevelTo(string languageToUpdate, string language, string level)
        {
            _languagePage.EnterLanguageAndLevelToCancelForUpdate(languageToUpdate, language, level);
        }

        [When("I click cancel")]      //Click cancel update 
        public void WhenIClickCancel()
        {
            _languagePage.ClickCancelUpdate();
        }

        [Then("the language {string} should remain unchanged with level {string}")]   //Validation for cancel update
        public void ThenTheLanguageShouldRemainUnchangedWithLevel(string language, string level)
        {
            Assert.That(_languagePage.GetLevelOfLanguage(language), Is.EqualTo(level), $"{language} update hasn't cancelled!");
        }

        [Then("I should able to Cancel the operation and verify that no changes has happened")]   //Validation for cancel update
        public void ThenIShouldAbleToCancelTheOperationAndVerifyThatNoChangesHasHappened()
        {
            Assert.That(_languagePage.IsCancelButtonNotDisplayed(), Is.True, $"Cancel button is Displayed!");
        }
        
        [When("I want to add language as {string} and level as {string} when the session is expired")]  //Try to add a language when the session is expired
        public void WhenIWantToAddLanguageAsAndLevelAsWhenTheSessionIsExpired(string languageToAdd, string levelToAdd)
        {
            _languagePage.ExpireSession();
            _languagePage.AddNewLanguageAndLevel(languageToAdd, levelToAdd);
        }

        [When("I add language as {string} and level as {string} to update")]    //Add language to update when the session is expired
        public void WhenIAddLanguageAsAndLevelAsToUpdate(string language, string level)
        {
            _languagePage.AddNewLanguageAndLevel(language, level);
            _scenarioContext.Set(new List<string> { language }, "LanguagesToCleanup");
        }

        [When("I want to update language the existing language as{string}, language to update as {string},and level to update as {string} when the session is expired")]  //Try to update when session expired
        public void WhenIWantToUpdateLanguageTheExistingLanguageAsLanguageToUpdateAsAndLevelToUpdateAsWhenTheSessionIsExpired(string existingLanguage, string languageToUpdate, string levelToUpdate)
        {
            _languagePage.ExpireSession();
            _languagePage.UpdateLanguageAndLevel(existingLanguage, languageToUpdate, levelToUpdate);
            _languagePage.ClickCancelUpdate();    //Cancel the update so that we can do clean up for the existing language
        }

        [When("I add language {string} and level as {string} to delete")]   //Add Language to delete when the session is expired
        public void WhenIAddLanguageAndLevelAsToDelete(string language, string level)
        {
            _languagePage.AddNewLanguageAndLevel(language, level);
            _scenarioContext.Set(new List<string> { language }, "LanguagesToCleanup");  //Clean up
        }

        [When("I want to delete language {string} when the session is expired")] //Try to delete the language when session expired
        public void WhenIWantToDeleteLanguageWhenTheSessionIsExpired(string languageToDelete)
        {
            _languagePage.ExpireSession();
            _languagePage.DeleteSpecificLanguage(languageToDelete);
        }

        [Then("I should see {string} error message")]    //Validation for session expired
        public void ThenIShouldSeeErrorMessage(string error)
        {
            Assert.That(_languagePage.IsErrorMessageDisplayed(error), Is.True, $"Error Message '{error}' shouldn't be displayed");
        }

        [When("I add the languages {string} and it's level {string}in different combinations")]  //Add languages and level by leaving either one or both the fields empty
        public void WhenIAddTheLanguagesAndItsLevelInDifferentCombinations(string language, string level)
        {
            _languagePage.LeaveTheLanguageAndLevelEmptyWithCombinationsForAdd(language, level);
            var actualMessage = _languagePage.GetErrorMessage();
            _scenarioContext.Set(actualMessage, "ActualMessage");
        }

        [When("I add the language as {string} and level as {string}")]
        public void WhenIAddTheLanguageAsAndLevelAs(string language, string level) //Add language to either leave the language or level field empty and both
        {
            _languagePage.AddNewLanguageAndLevel(language, level);
        }

        [When("I update existing language {string} to {string} with level {string}")] //Update languages and level by leaving either one or both the fields empty
        public void WhenIUpdateExistingLanguageToWithLevel(string existingLanguage, string languageToUpdate, string levelToUpdate)
        {
            _languagePage.LeaveTheLanguageAndLevelEmptyWithCombinationsForUpdate(existingLanguage, languageToUpdate, levelToUpdate);
            var actualMessage = _languagePage.GetErrorMessage();
            _languagePage.ClickCancelUpdate();
            _scenarioContext.Set(actualMessage,"ActualMessage");
            _scenarioContext.Set(new List<string> {existingLanguage},"LanguagesToCleanup");     //To clean up existing language
        }

        [Then("I should see the {string}")]     //Validation for expected error message
        public void ThenIShouldSeeThe(string errorMessage)
        {
            var actualMessage = _scenarioContext.Get<string>("ActualMessage");
            Assert.That(actualMessage.Contains(errorMessage), Is.True, $"Expected message contains {errorMessage}, but got {actualMessage}");
        }

        [When("I try to add huge language name of length {int} and language level as {string}")]   //Huge language name
        public void WhenITryToAddHugeLanguageNameOfLengthAndLanguageLevelAsBasic(int length, string level)
        {
            string hugeLanguageName = new string('e', length);
            _languagePage.AddNewLanguageAndLevel(hugeLanguageName, level);
            var successMessage = _languagePage.GetSuccessMessageForAddNewLanguage(hugeLanguageName);
            _scenarioContext["ActualMessage"] = successMessage;
            _scenarioContext.Set(new List<string> { hugeLanguageName }, "LanguagesToCleanup");
        }

        [Then("I should see warning message as {string}")]    //Validation for huge language
        public void ThenIShouldSeeWarningMessageAs(string expected)
        {
            var actualMessage = _scenarioContext.Get<string>("ActualMessage");
            Assert.That(actualMessage.Contains(expected), Is.True, $"Expected message contains '{expected}', but got '{actualMessage}'");
        }

        [When("I update the following languages if they match the existing ones:")]     //Update Invalid language 
        public void WhenIUpdateTheFollowingLanguagesIfTheyMatchTheExistingOnes(Table updateInvalidLanguageTable)
        {
            var updateInvalidLanguagesList = updateInvalidLanguageTable.CreateSet<UpdateLanguage>();
            var actualLanguages = new List<string>();
            var expectedLanguages = new List<string>();
            foreach (var updateInvalidLanguage in updateInvalidLanguagesList)
            {
                if (string.Equals(updateInvalidLanguage.LanguageToUpdate, "<space>", StringComparison.OrdinalIgnoreCase))
                {
                    updateInvalidLanguage.LanguageToUpdate = "   ";
                }
                expectedLanguages.Add(updateInvalidLanguage.LanguageToUpdate);
                _languagePage.UpdateLanguageAndLevel(updateInvalidLanguage.ExistingLanguage, updateInvalidLanguage.LanguageToUpdate, updateInvalidLanguage.LanguageLevelToUpdate);
                var successMessageForUpdate = _languagePage.GetSuccessMessageForUpdate(updateInvalidLanguage.LanguageToUpdate);
                actualLanguages.Add(successMessageForUpdate);
            }
            _scenarioContext.Set(actualLanguages, "ActualLanguages");
            _scenarioContext.Set(expectedLanguages, "LanguagesToCleanup");  //Clean up the updated languages
        }

        [Then("I should see the {string} message")]
        public void ThenIShouldSeeTheMessage(string expected)
        {
            var actualList = _scenarioContext.Get<List<string>>("ActualLanguages");
            foreach (var actual in actualList)       //Multiple languages in the list. So, using foreach loop for validation 
            {
                Assert.That(actual.Contains(expected), Is.True,
                    $"Invalid language name was accepted '{actual}', not found {expected} error message");
            }
        }

        [When("when I add the same language and choose different language level")]  //Passing same language and different level (for step 1 to add language it shares different step)
        public void WhenWhenIAddTheSameLanguageAndChooseDifferentLanguageLevel(Table sameLanguageDifferentLevelTable)
        {
            var cleanUp = _scenarioContext.Get<List<string>>("ActualMessage");   //I retrieve the language to be cleaned up from the previous step (step 1)
            var sameLanguageDifferentLevel = sameLanguageDifferentLevelTable.CreateInstance<AddLanguage>();
            _languagePage.AddNewLanguageAndLevel(sameLanguageDifferentLevel.Language, sameLanguageDifferentLevel.LanguageLevel);
            var message = _languagePage.GetErrorMessage();  //When I add the same language second time it will show error message
            Console.WriteLine(message);
            _scenarioContext.Set(message, "Actual");  //I'm storing it for validation
            _scenarioContext.Set(cleanUp, "LanguagesToCleanup");
        }

        [When("when I add the same language and choose same language level")]   //Add same language and same level     
        public void WhenWhenIAddTheSameLanguageAndChooseSameLanguageLevel(Table sameLanguageSameLevelTable)
        {
            var cleanUp = _scenarioContext.Get<List<string>>("ActualMessage");   //I retrieve the language to be cleaned up from the previous step (step 1)
            var sameLanguageSameLevel = sameLanguageSameLevelTable.CreateInstance<AddLanguage>();
            _languagePage.AddNewLanguageAndLevel(sameLanguageSameLevel.Language, sameLanguageSameLevel.LanguageLevel);
            var message = _languagePage.GetErrorMessage();  //When I add the same language second time it will show error message
            Console.WriteLine(message);
            _scenarioContext.Set(message, "Actual");  //I'm storing it for validation
            _scenarioContext.Set(cleanUp, "LanguagesToCleanup");
        }

        [When("when I update the same language and same language level:")]  //Update same language and same level
        public void WhenWhenIUpdateTheSameLanguageAndSameLanguageLevel(Table sameLanguageSameLevelForUpdateTable)
        {
            var cleanUp = _scenarioContext.Get<List<string>>("ActualMessage");   //I retrieve the language to be cleaned up from the previous step (step 1)
            var sameLanguageSameLevel = sameLanguageSameLevelForUpdateTable.CreateInstance<UpdateLanguage>();
            _languagePage.UpdateLanguageAndLevel(sameLanguageSameLevel.ExistingLanguage,sameLanguageSameLevel.LanguageToUpdate,sameLanguageSameLevel.LanguageLevelToUpdate);
            var message = _languagePage.GetErrorMessage();  //When I add the same language second time it will show error message
            Console.WriteLine(message);
            _languagePage.ClickCancelUpdate();  //I got no such element exception for clean up. When I click the cancel update button, I could see the added language list.
            _scenarioContext.Set(message, "Actual");  //I'm storing it for validation
            _scenarioContext.Set(cleanUp, "LanguagesToCleanup");
        }

        [Then("I should able to see the {string} in my profile")]  //Validation 
        public void ThenIShouldAbleToSeeTheInMyProfile(string expectedMessage)
        {
            var actual = _scenarioContext.Get<string>("Actual");
            Console.WriteLine(actual);
            Assert.That(actual, Is.EqualTo(expectedMessage), $"Expected message {expectedMessage} isn't found");
        }

       [When("I update existing language {string} with  huge language name of length {int} and language level as {string}")]  //To update huge language name
        public void WhenIUpdateExistingLanguageWithHugeLanguageNameOfLengthAndLanguageLevelAs(string existingLanguage, int length, string level)
        {
            var longLanguageName = new string('f', length);
            _languagePage.UpdateLanguageAndLevel(existingLanguage, longLanguageName, level);
            var successMessage = _languagePage.GetSuccessMessageForUpdate(longLanguageName);
            _scenarioContext.Set(successMessage, "ActualLanguages");
            _scenarioContext.Set(new List<string> { longLanguageName }, "LanguagesToCleanup");
        }
    }
}

public class AddLanguage    //Property class to add language
{
    public string Language { get; set; }
    public string LanguageLevel { get; set; }
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