Feature: SkillsNegative

As a user, I shouldn't be able to add skills and it's level

Background:
	Given I navigate to the profile page as a registered user

@negative
Scenario: As a user, I shouldn't able to add skills and it's level by giving skill field empty
	When I click Add New button, leave the skill field empty,choose the skill level and click the Add button
	Then I should see "Please enter skill and experience level" error message

Scenario: As a user, I shouldn't able to add skills and it's level by not choosing the skill level
	When I click Add New button, enter the skill field, not choosing the skill level and click the Add button
	Then I should see "Please enter skill and experience level" error message

Scenario: As a user, I shouldn't able to add skills by empty the skill field and not choosing the skill level
	When I click Add New button, empty the skill field, not choosing the skill level and click the Add button
	Then I should see "Please enter skill and experience level" error message

Scenario: As a user, I shouldn't able to update skills and it's level by giving skill field empty
	When I add skill "Teaching" and it's level "Intermediate"
	And I click edit icon of "Teaching", leave the skill field empty,choose the skill level and click the Update button
	Then I should see "Please enter skill and experience level" error message

Scenario: As a user, I shouldn't able to update skills and it's level by not choosing the skill level
	When I add skill "Teaching" and it's level "Intermediate"
	And I click edit icon of "Teaching", enter the skill field, not choosing the skill level and click the Update button
	Then I should see "Please enter skill and experience level" error message

Scenario: As a user, I shouldn't able to update skills by empty the skill field and not choosing the skill level
	When I add skill "Teaching" and it's level "Intermediate"
	And I click edit icon of "Teaching", empty the skill field, not choosing the skill level and click the Update button
	Then I should see "Please enter skill and experience level" error message

Scenario: As a user, I should able to Cancel the Add operation
	When I click Add New button, enter the Skill "Problem Solving" and it's level "Beginner"
	Then I should able to Cancel the operation and verify that the skill "Problem Solving" shouldn't be added

Scenario: As a user, I should able to Cancel the Update operation
	When I click Add New Skill "Problem Solving" with level "Beginner"
	And I click edit icon of "Problem Solving" and Update skill to "Teaching" and level to "Intermediate"
	And I click cancel
	Then the skill "Problem Solving" should remain unchanged with level "Beginner"

Scenario: As a user, I shouldn't able to add the same skill and different skill level times
	When I Add the same Skills and different SkillLevel:
		| NewSkills       | SkillLevel   |
		| Problem Solving | Beginner     |
		| Problem Solving | Intermediate |
	Then I should see the error message "Duplicated data"

Scenario: As a user, I shouldn't able to add the same skill and same skill level multiple times
	When I Add the same Skills and same SkillLevel:
		| NewSkills       | SkillLevel |
		| Problem Solving | Beginner   |
		| Problem Solving | Beginner   |
	Then I should see the error message "This skill is already exist in your skill list."

Scenario: As a user, I want to Edit the existing skills by giving same skill and same level in my profile
	When I update the skill "Communication" with same value
	Then I should see "This skill is already added to your skill list." error message

Scenario: As a user, I shouldn't be able to add when session expired
	When I want to add any skills when the session is expired
	Then I should see "undefined" error message

Scenario: As a user, I shouldn't be able to update when session expired
	When I want to update any skills when the session is expired
	Then I should see "undefined" error message

Scenario: As a user, I shouldn't be able to delete when session expired
	When I want to delete any skills when the session is expired
	Then I should see "There is an error when deleting skill" error message

