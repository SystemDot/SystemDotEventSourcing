Feature: PostEvents
	In order to ensure devices synchronise events to the server
	I want to make events from the client available for synchronisation to server and update the server with those events

Scenario: Updating server with events from commits from the client
	Given I have created a synchronisable commit with an id of E261A67D-2C00-4854-A0FF-6DEFA84A4276 and stream identified as '1157AC59-AD0D-4BF0-9CC1-238BDE2CEFB9' and client identified as 'TestClient'
	And I add an event origin for the local machine as a header of the synchronisable commit
	And I add a serialised event with an id of E261A67D-2C00-4854-A0FF-6DEFA84A4277 to the commit
	And I add a serialised event with an id of F261A67D-2C00-4854-A0FF-6DEFA84A4277 to the commit
	And I have created a synchronisable commit with an id of F261A67D-2C00-4854-A0FF-6DEFA84A4276 and stream identified as '2157AC59-AD0D-4BF0-9CC1-238BDE2CEFB9' and client identified as 'TestClient'
	And I add an event origin for the local machine as a header of the synchronisable commit
	And I add a serialised event with an id of F261A67D-2C00-4854-A0FF-6DEFA84A4277 to the commit
	And I add a serialised event with an id of A261A67D-2C00-4854-A0FF-6DEFA84A4277 to the commit
	And I have created a new event session
	When I synchronise the server with the synchronisable commits
	And I use the first commit in the event session
	Then the commit should be for a stream identified as '1157AC59-AD0D-4BF0-9CC1-238BDE2CEFB9'
	And the commit should contain an event with an id of E261A67D-2C00-4854-A0FF-6DEFA84A4277
	And the commit should contain an event with an id of F261A67D-2C00-4854-A0FF-6DEFA84A4277
	When I use the second commit in the event session
	Then the commit should be for a stream identified as '2157AC59-AD0D-4BF0-9CC1-238BDE2CEFB9'
	And the commit should contain an event with an id of F261A67D-2C00-4854-A0FF-6DEFA84A4277
	And the commit should contain an event with an id of A261A67D-2C00-4854-A0FF-6DEFA84A4277
	And none of the posted events should be dispatched