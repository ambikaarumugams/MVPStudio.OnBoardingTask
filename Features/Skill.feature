@skill
Feature: Skill
As a registered user, I would like to add the skills into my profile page
So that others can see which skills I know

Background:
	Given I navigate to the profile page as a registered user

@positive @valid
Scenario: As  user, I should able to add skills into the profile
	When I Add the following skills and select skill level:
		| Skill           | SkillLevel   |
		| Problem Solving | Beginner     |
		| Team work       | Intermediate |
		| Communication   | Expert       |
		| Adaptability    | Intermediate |
	And I should see the skills and verify it has been added successfully
	Then I should see the skills listed in my profile and verify it
	
@positive @valid
Scenario: As  user, I should able to edit existing skills in the user profile
	When I add the following skills and select their levels:
		| Skill         | SkillLevel   |
		| Communication | Expert       |
		| Adaptability  | Intermediate |
	Then I should see the added skills in the profile
	And I update the following skills if they match the existing ones:
		| ExistingSkill | SkillToUpdate   | SkillLevelToUpdate |
		| Communication | Public Speaking | Expert             |
		| Adaptability  | Technical       | Expert             |
	Then I should see a success message and the updated skills in my profile

@positive @valid
Scenario:  As  user, I should able to delete existing skills from the user profile
	When I add the following skills and select their levels:
		| Skill          | SkillLevel   |
		| Manual Testing | Expert       |
		| API Testing    | Intermediate |
	Then I should see the added skills in the profile
	When I click the delete icon corresponding to the following skills:
		| SkillToBeDeleted |
		| Manual Testing   |
		| API Testing      |
	Then I should see a success message for each deleted skill
	And the skills table should be empty if all skills have been deleted

@negative @vaild
Scenario: As a user, I want to Edit the existing skills by giving same skill and different level in my profile
	When I add skill as "Java" and level as "Expert"
	And when I update the skill and skill level:
		| ExistingSkill | SkillToUpdate | SkillLevelToUpdate |
		| Java          | Java          | Intermediate       |
	Then I should see the success message and updated skill in my profile

Scenario: Add multiple skills using external file
    When I add all skills from CSV
    Then all skills should be added successfully

			



  