Feature: Login
  As a user, I want to log in to the application to access restricted content.

Background: 
   Given I am on the home page
 
   Scenario: Perform a successful login
   When I enter valid username and valid password
   Then I should see the successful message 

   Scenario: Perform login with invalid credentials
   When I enter invalid username and invalid password
   Then I should see "Confirm your email" error message and "Send Verification Email" notification
   When I click "Send Verification Email" button I should see "Email Verification Failed" message

  Scenario: Fail login with invalid username
   When I enter invalid username and valid password
   Then I should see "Confirm your email" error message and "Send Verification Email" notification
   When I click "Send Verification Email" button I should see "Email Verification Failed" message
    
  Scenario: Fail login with invalid password
   When I enter a valid username and invalid password
   Then I should see "Confirm your email" error message and "Send Verification Email" notification
   When I click "Send Verification Email" button I should see "Email Verification Sent" message
    
  Scenario: Fail login with empty credentials
   When I enter empty credentials
   Then I should see "Please enter a valid email address" and "Password must be at least 6 characters" validation message

  Scenario: Fail login with empty username
   When I enter empty credentials
   Then I should see "Please enter a valid email address" validation message

  Scenario: Fail login with empty password
   When I enter empty credentials
   Then I should see "Password must be at least 6 characters" validation message

Scenario: Fail login with NoSql operators
When I enter valid username and password as NoSql operator
Then I should see "Confirm your email" error message and "Send Verification Email" notification
When I click "Send Verification Email" button I should see "Email Verification Sent" message