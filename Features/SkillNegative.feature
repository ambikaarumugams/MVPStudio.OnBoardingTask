
@skill
Feature: SkillNegative

As a user, I shouldn't be able to add skills and it's level

Background:
	Given I navigate to the profile page as a registered user

@negative @invalid
Scenario Outline: As a user, I shouldn't able to add skill with invalid input
	When I try to add a skill "<Skill>" with level "<SkillLevel>"
	Then I should see the error message "<ErrorMessage>"

Examples:
	| Skill      | SkillLevel   | ErrorMessage  |
	| fghghjhkjk | Beginner     | Invalid skill |
	| 4345679989 | Intermediate | Invalid skill |
	| #@#$$%%^&& | Expert       | Invalid skill |
	| <space>    | Beginner     | Invalid skill |

@negative @invalid
Scenario: As a user, I shouldn't able to update Skill with invalid input
	When I Add the following skills and select skill level:
		| Skill | SkillLevel   |
		| Java  | Expert       |
		| SQL   | Beginner     |
		| C#    | Intermediate |
		| API   | Intermediate |
	And I update the following skills if they match the existing ones:
		| ExistingSkill | SkillToUpdate | SkillLevelToUpdate |
		| Java          | fhhjhkjklkl   | Expert             |
		| SQL           | !#!$%%^&&*    | Beginner           |
		| C#            | 65656788      | Intermediate       |
		| API           | <space>       | Intermediate       |
	Then I should see the "Invalid skill" message

@negative @valid
Scenario: As a user, I shouldn't able to add when session expired
	When I want to add skill as "C#" and level as "Beginner" when the session is expired
	Then I should see "undefined" error message

@negative @valid
Scenario: As a user, I shouldn't able to update when session expired
	When I add skill as "C#" and level as "Expert" to update
	And I want to update skill the existing skill as"C#", skill to update as "Java",and level to update as "Intermediate" when the session is expired
	Then I should see "undefined" error message

@negative @valid
Scenario: As a user, I shouldn't able to delete when session expired
	When I add skill "Java" and level as "Beginner" to delete
	When I want to delete skill "Java" when the session is expired
	Then I should see "There is an error when deleting skill" error message

@negative @valid
Scenario: As a user, I should able to Cancel the Add operation
	When I click Add New button, enter the skill "API" and it's level "Intermediate"
	Then I should able to Cancel the operation and verify that the skill "API" shouldn't be added

@negative @valid
Scenario: As a user, I should able to Cancel the Update operation
	When I add skill "C#" and it's level "Expert"
	And I click edit icon of "C#" and Update level to "Java" and level to "Intermediate"
	And I click cancel
	Then the skill "C#" should remain unchanged with level "Expert"

@negative
Scenario Outline: As a user, I shouldn't be able to add skills and it's level by giving either or both of the fields are empty
	When I add the skills "<Skill>" and it's level "<Skill Level>"in different combinations
	Then I should see the "<Error Message>"
Examples:
	| Skill    | Skill Level | Error Message                           |
	|          | Beginner    | Please enter skill and experience level |
	| Teaching |             | Please enter skill and experience level |
	|          |             | Please enter skill and experience level |

@negative
Scenario Outline: As a user, I shouldn't be able to update skills and it's level by giving either or both of the fields are empty
	When I add the Skill as "Painting" and level as "Beginner"
	When I update existing skill "<ExistingSkill>" to "<SkillToUpdate>" with level "<SkillLevelToUpdate>"
	Then I should see the "<ExpectedMessage>"
Examples:
	| ExistingSkill | SkillToUpdate | SkillLevelToUpdate | ExpectedMessage                         |
	| Painting      |               | Expert             | Please enter skill and experience level |
	| Painting      | Dancing       |                    | Please enter skill and experience level |
	| Painting      |               |                    | Please enter skill and experience level |

@negative
Scenario: As a user, I shouldn't able to add huge Skill name
	When I try to add huge skill name of length 1000 and skill level as "Beginner"
	Then I should see warning message as "Skill name is too long"

@destructive
Scenario: As a user, I shouldn't able to add the Skill name,if it's too long
	When I try to add huge skill name of length 5000 and skill level as "Intermediate"
	Then I should see warning message as "Skill name is too long"

@negative @valid
Scenario: As a user, I shouldn't able to add the same Skill and different level in my profile
	When I add skill as "Cooking" and level as "Beginner"
	And when I add the same skill and choose different skill level
		| Skill   | SkillLevel |
		| Cooking | Expert     |
	Then I should able to see the "Duplicated data" in my profile

@negative @valid
Scenario: As a user, I shouldn't able to add the same skill and same level
	When I add skill as "Baking" and level as "Beginner"
	And when I add the same skill and choose same skill level
		| Skill  | SkillLevel |
		| Baking | Beginner   |
	Then I should able to see the "This skill is already exist in your skill list." in my profile

@negative @valid
Scenario: As a user, I shouldn't be able to Edit the existing skills by giving same skill and same level in my profile
	When I add skill as "C" and level as "Expert"
	And when I update the same skill and same skill level:
		| ExistingSkill | SkillToUpdate | SkillLevelToUpdate |
		| C             | C             | Expert             |
	Then I should able to see the "This skill is already added to your skill list." in my profile

@destructive
Scenario: As a user, I shouldn't be able to update large data as a skill
	When I add skill as "C#" and level as "Beginner"
	When I update existing skill "C#" with  huge skill name of length 5000 and skill level as "Expert"
	Then I should see the error message " Skill name is too long "















