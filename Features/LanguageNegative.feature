Feature: LanguageNegative

As a user, I shouldn't be able to add languages and it's level

Background:
	Given I navigate to the profile page as a registered user

@negative
Scenario: As a user, I shouldn't able to add languages and it's level by giving language field empty
	When I click Add New button, leave the language field empty,choose the language level and click the Add button
	Then I should see "Please enter language and level" error message

Scenario: As a user, I shouldn't able to add languages and it's level by not choosing the language level
	When I click Add New button, enter the language field, not choosing the language level and click the Add button
	Then I should see "Please enter language and level" error message

Scenario: As a user, I shouldn't able to add languages by empty the language field and not choosing the language level
	When I click Add New button, empty the language field, not choosing the language level and click the Add button
	Then I should see "Please enter language and level" error message

Scenario: As a user, I shouldn't able to update languages and it's level by giving language field empty
	When I add language "French" and it's level "Fluent"
	And I click edit icon of "French", leave the language field empty,choose the language level and click the Update button
	Then I should see "Please enter language and level" error message

Scenario: As a user, I shouldn't able to update languages and it's level by not choosing the language level
	When I add language "French" and it's level "Fluent"
	And I click edit icon of "French", enter the language field, not choosing the language level and click the Update button
	Then I should see "Please enter language and level" error message

Scenario: As a user, I shouldn't able to update languages by empty the language field and not choosing the language level
	When I add language "French" and it's level "Fluent"
	And I click edit icon of "French", empty the language field, not choosing the language level and click the Update button
	Then I should see "Please enter language and level" error message

Scenario: As a user, I should able to Cancel the Add operation
	When I click Add New button, enter the language "Spanish" and it's level "Native/Bilingual"
	Then I should able to Cancel the operation and verify that the language "Spanish" shouldn't be added

Scenario: As a user, I should able to Cancel the Update operation
	When I add language "French" and it's level "Fluent"
	And I click edit icon of "French" and Update level to "Japanese" and level to "Native/Bilingual"
	And I click cancel
	Then the language "French" should remain unchanged with level "Fluent"

Scenario: As a user, I shouldn't able to add the same language and different level
	When I Add the following New Language and select New Language level:
		| NewLanguage | NewLanguageLevel |
		| Tamil       | Native/Bilingual |
		| Tamil       | Fluent           |
	Then I should see "Duplicated data" error message

Scenario: As a user, I shouldn't able to add the same language and same level
	When I Add an existing language and select a language level:
		| NewLanguage | NewLanguageLevel |
		| Tamil       | Native/Bilingual |
		| Tamil       | Native/Bilingual |
	Then I should see "This language is already exist in your language list" error message

Scenario: As a user, I want to Edit the existing languages by giving same language and same level in my profile
	When I update the language "Tamil" with same value
	Then I should see "This language is already added to your language list." error message

Scenario: As a user, I shouldn't be able to add when session expired
	When I want to add language as "Russian" and level as "Basic" when the session is expired
	Then I should see "undefined" error message

Scenario: As a user, I shouldn't be able to update when session expired
    When I add language as "Greek" and level as "Basic"
	And I want to update language the existing language as"Greek", language to update as "Chinese",and level to update as "Basic" when the session is expired
	Then I should see "undefined" error message

Scenario: As a user, I shouldn't be able to delete when session expired
   When  I add language "Italian" and level as "Basic"
	When I want to delete language "Italian" when the session is expired
	Then I should see "There is an error when deleting language" error message
