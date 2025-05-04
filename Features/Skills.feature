Feature: Skills
As a registered user, I would like to add the skills into my profile page
So that others can see which skills I know

Background:
	Given I navigate to the profile page as a registered user
	When I Add the following NewSkills and select SkillLevel:
		| NewSkills       | SkillLevel   |
		| Problem Solving | Beginner     |
		| Team work       | Intermediate |
		| Communication   | Expert       |
		| Adaptability    | Intermediate |
	And I should see the skills and verify it has been added successfully
	Then I should see the skills listed in my profile and verify it

Scenario: Add skills into the profile
	
Scenario: As a user, I want to Edit the existing skills in my profile
	When I update the skill and skill level:
		| ExistingSkill | SkillToUpdate   | SkillLevelToUpdate |
		| Communication | Public Speaking | Expert             |
		| Adaptability  | Technical       | Expert             |
	Then I should see the success message and updated skills in my profile

Scenario: As a user, I want to delete the existing skill from my profile
	When I click the delete icon corresponding to the following skills:
		| SkillsToBeDeleted |
		| Team work         |
		| Communication     |
	Then I should see a success message for each deleted skill
	And the skills table should be empty if all skills have been deleted

Scenario: As a user, I want to Edit the existing skills by giving same skill and different level in my profile
	When I update the skill and skill level:
		| ExistingSkill | SkillToUpdate | SkillLevelToUpdate |
		| Communication | Communication | Intermediate       |
	Then I should see the success message and updated skills in my profile
	   


			



  