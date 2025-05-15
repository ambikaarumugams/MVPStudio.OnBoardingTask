@language
Feature: LanguageNegative

As a user, I shouldn't be able to add languages and it's level

Background:
	Given I navigate to the profile page as a registered user

@negative @invalid
Scenario Outline: As a user,  I shouldn't be able to add language with invalid input
	When I try to add a language "<Language>" with level "<LanguageLevel>"
	Then I should see the error message "<ErrorMessage>"

Examples:
	| Language   | LanguageLevel  | ErrorMessage     |
	| fghghjhkjk | Basic          | Invalid Language |
	| 4345679989 | Conversational | Invalid Language |
	| #@#$$%%^&& | Fluent         | Invalid Language |
	| <space>    | Basic          | Invalid Language |

@negative @invalid
Scenario: As a user,  I shouldn't be able to update language with invalid input
	When I Add the following language and select language level:
		| Language | LanguageLevel    |
		| Tamil    | Native/Bilingual |
		| English  | Fluent           |
		| French   | Basic            |
		| German   | Conversational   |
	And I update the following languages if they match the existing ones:
		| ExistingLanguage | LanguageToUpdate | LanguageLevelToUpdate |
		| Tamil            | fghghjhkjk       | Basic                 |
		| English          | 4345679989       | Conversational        |
		| French           | #@#$$%%^&&       | Fluent                |
		| German           | <space>          | Native/Bilingual      |
	Then I should see the "Invalid Language" message

@negative @valid
Scenario: As a user, I shouldn't be able to add when session expired
	When I want to add language as "Russian" and level as "Basic" when the session is expired
	Then I should see "undefined" error message

@negative @valid
Scenario: As a user, I shouldn't be able to update when session expired
	When I add language as "Greek" and level as "Basic" to update
	And I want to update language the existing language as"Greek", language to update as "Chinese",and level to update as "Basic" when the session is expired
	Then I should see "undefined" error message

@negative @valid
Scenario: As a user, I shouldn't be able to delete when session expired
	When I add language "Italian" and level as "Basic" to delete
	When I want to delete language "Italian" when the session is expired
	Then I should see "There is an error when deleting language" error message

@negative @valid	
Scenario: As a user, I should able to Cancel the Add operation
	When I click Add New button, enter the language "Spanish" and it's level "Native/Bilingual"
	Then I should able to Cancel the operation and verify that the language "Spanish" shouldn't be added

@negative @valid
Scenario: As a user, I should able to Cancel the Update operation
	When I add language "French" and it's level "Fluent"
	And I click edit icon of "French" and Update level to "Japanese" and level to "Native/Bilingual"
	And I click cancel
	Then the language "French" should remain unchanged with level "Fluent"

@negative
Scenario Outline: As a user, I shouldn't be able to add languages and it's level by giving either or both of the fields are empty
	When I add the languages "<Language>" and it's level "<Language Level>"in different combinations
	Then I should see the "<Error Message>"
Examples:
	| Language | Language Level | Error Message                   |
	|          | Basic          | Please enter language and level |
	| Bengali  |                | Please enter language and level |
	|          |                | Please enter language and level |

@negative
Scenario Outline: As a user, I shouldn't be able to update languages and it's level by giving either or both of the fields are empty
	When I add the language as "French" and level as "Fluent"
	When I update existing language "<ExistingLanguage>" to "<LanguageToUpdate>" with level "<LanguageLevelToUpdate>"
	Then I should see the "<ExpectedMessage>"
Examples:
	| ExistingLanguage | LanguageToUpdate | LanguageLevelToUpdate | ExpectedMessage                 |
	| French           |                  | Fluent                | Please enter language and level |
	| French           | Gujarati         |                       | Please enter language and level |
	| French           |                  |                       | Please enter language and level |

@negative
Scenario: As a user, I shouldn't able to add huge language name
	When I try to add huge language name of length 1000 and language level as "Basic"
	Then I should see warning message as "Language name is too long"

@destructive
Scenario: As a user, I shouldn't able to add the language name,if it's too long
	When I try to add huge language name of length 5000 and language level as "Basic"
	Then I should see warning message as "Language name is too long"

@negative @valid
Scenario: As a user, I shouldn't able to add the same language and different level in my profile
	When I add language as "Tamil" and level as "Native/Bilingual"
	And when I add the same language and choose different language level
		| Language | LanguageLevel  |
		| Tamil    | Conversational |
	Then I should able to see the "Duplicated data" in my profile

@negative @valid
Scenario: As a user, I shouldn't able to add the same language and same level
	When I add language as "Tamil" and level as "Native/Bilingual"
	And when I add the same language and choose same language level
		| Language | LanguageLevel    |
		| Tamil    | Native/Bilingual |
	Then I should able to see the "This language is already exist in your language list." in my profile

@negative @valid
Scenario: As a user, I shouldn't be able to Edit the existing languages by giving same language and same level in my profile
	When I add language as "Tamil" and level as "Native/Bilingual"
	And when I update the same language and same language level:
		| ExistingLanguage | LanguageToUpdate | LanguageLevelToUpdate |
		| Tamil            | Tamil            | Native/Bilingual      |
	Then I should able to see the "This language is already added to your language list." in my profile

@destructive
Scenario: As a user, I shouldn't be able to update large data as a language
	When I add language as "Turkish" and level as "Basic"
	When I update existing language "Turkish" with  huge language name of length 5000 and language level as "Basic" 
	Then I should see the error message " Language name is too long "









