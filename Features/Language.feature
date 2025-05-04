Feature: Language

As a registered user, I would like to add the languages into my profile page
So that others can see which languages I know

Background:
	Given I navigate to the profile page as a registered user
@positive @valid
Scenario: Add languages into the profile
	When I Add the following New Language and select New Language level:
		| NewLanguage | NewLanguageLevel |
		| Tamil       | Native/Bilingual |
		| English     | Fluent           |
		| French      | Basic            |
		| German      | Conversational   |
	And I should see the languages and verify it has been added successfully
	Then I should see the languages listed in my profile and verify it
	
@positive @valid
Scenario: Edit existing languages in the user profile
	When I add the following languages and select their levels:
		| NewLanguage | NewLanguageLevel |
		| Malayalam   | Basic            |
		| Urdu        | Conversational   |
	Then I should see the added languages in the profile
	And I update the following languages if they match the existing ones:
		| ExistingLanguage | LanguageToUpdate | LanguageLevelToUpdate |
		| Malayalam        | Hindi            | Conversational        |
		| Urdu             | Chinese          | Basic                 |
	Then I should see a success message and the updated languages in my profile

@positive @valid
Scenario: Delete existing languages from the user profile
	When I add the following languages and select their levels:
		| NewLanguage | NewLanguageLevel |
		| Kannada     | Basic            |
		| Sinhalese   | Conversational   |
	Then I should see the added languages in the profile
	When I click the delete icon corresponding to the following languages:
		| LanguageToBeDeleted |
		| Kannada             |
		| Sinhalese           |
	Then I should see a success message for each deleted language
	And the languages table should be empty if all languages have been deleted

@negative @vaild
Scenario: As a user, I want to Edit the existing languages by giving same language and different level in my profile
	When I update the language and language level:
		| ExistingLanguage | LanguageToUpdate | LanguageLevelToUpdate |
		| Tamil            | Tamil            | Conversational        |
	Then I should see the success message and updated language in my profile

Scenario Outline: As a user, I want to add language with invalid input
	When I try to add a language "<NewLanguage>" with level "<NewLanguageLevel>"
	Then I should see the error message "<ErrorMessage>"

Examples:
	| NewLanguage | NewLanguageLevel | ErrorMessage     |
	| fghghjhkjk  | Basic            | Invalid Language |
	| 4345679989  | Conversational   | Invalid Language |
	| #@#$$%%^&&  | Fluent           | Invalid Language |
	|             | Basic            | Invalid Language |


