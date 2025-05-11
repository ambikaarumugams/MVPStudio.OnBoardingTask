Feature: Education

As a registered user, I would like to add education details into my profile page
So that others can see about my educational background


@tag1
Scenario: Add education details into my profile
	Given I navigate to the profile page as a registered user
	When I add the following education details:
	| College/UniversityName | Country of College/University | Title  | Degree                  | YearofGraduation |
	| Anna University        | India                         | B.Tech | Bachelor of Engineering | 2015             |
	| Bharathiyar University | India                         | MCA    | Computer Applications   | 2019             |
	Then I should be able see the education details
	
	