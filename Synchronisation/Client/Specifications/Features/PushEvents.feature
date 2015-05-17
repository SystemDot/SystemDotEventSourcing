Feature: PushEvents
	In order to ensure devices synchronise events to the server
	I want to make events from the client available for synchronisation to server and update the server with those events

Scenario: Updating server with events from a commit from the client
	Given I have created a new event session
	And I add an event origin for the local machine as a header for the stream identified as 1157AC59-AD0D-4BF0-9CC1-238BDE2CEFB9 in the bucket identified as 'TestClient'
	And I have created an event in the session with an id of E261A67D-2C00-4854-A0FF-6DEFA84A4276 for the stream identified as 1157AC59-AD0D-4BF0-9CC1-238BDE2CEFB9 in the bucket identified as 'TestClient'
	And I have created an event in the session with an id of F261A67D-2C00-4854-A0FF-6DEFA84A4276 for the stream identified as 1157AC59-AD0D-4BF0-9CC1-238BDE2CEFB9 in the bucket identified as 'TestClient'
	And I commit the session with the id DAD11DA9-64C6-4955-AF82-F12B66FBAF3B
	When I push events to the server for client id 'TestClient'
	And I use the first synchronisable commit posted
	And I use the first commit in the event session
	And I deserialise the synchronisable commit events
	Then the commits should have been posted to the server with the correct address
	And the synchronisable commit should have the same id as the commit
	And the synchronisable commit should be for the same stream as the commit
	And the synchronisable commit should be for the same client as the commit
	And the synchronisable commit should be for the same date and time as the commit
	And the deserialised events should contain an event an id of E261A67D-2C00-4854-A0FF-6DEFA84A4276
	And the deserialised events should contain an event an id of F261A67D-2C00-4854-A0FF-6DEFA84A4276
	And the successful completion of the synchronisation should be signalled with the date of the last commit

Scenario: Updating server with events from a commit from the client for an event that exists for another client id
	Given I have created a new event session
	And I add an event origin for the local machine as a header for the stream identified as 1157AC59-AD0D-4BF0-9CC1-238BDE2CEFB9 in the bucket identified as 'TestClient'
	And I have created an event in the session with an id of E261A67D-2C00-4854-A0FF-6DEFA84A4276 for the stream identified as 1157AC59-AD0D-4BF0-9CC1-238BDE2CEFB9 in the bucket identified as 'TestClient'
	And I have created an event in the session with an id of F261A67D-2C00-4854-A0FF-6DEFA84A4276 for the stream identified as 1157AC59-AD0D-4BF0-9CC1-238BDE2CEFB9 in the bucket identified as 'OtherTestClient'
	And I commit the session with the id DAD11DA9-64C6-4955-AF82-F12B66FBAF3B
	When I push events to the server for client id 'TestClient'
	And I use the first synchronisable commit posted
	And I use the first commit in the event session
	And I deserialise the synchronisable commit events
	Then the deserialised events should not contain an event an id of F261A67D-2C00-4854-A0FF-6DEFA84A4276

Scenario: Updating server with events from a commit from the client where the commit was not made on the current machine
	Given I have created a new event session
	And I add an event origin for another machine as a header for the stream identified as 1157AC59-AD0D-4BF0-9CC1-238BDE2CEFB9 in the bucket identified as 'TestClient'
	And I have created an event in the session with an id of E261A67D-2C00-4854-A0FF-6DEFA84A4276 for the stream identified as 1157AC59-AD0D-4BF0-9CC1-238BDE2CEFB9 in the bucket identified as 'TestClient'
	And I commit the session with the id DAD11DA9-64C6-4955-AF82-F12B66FBAF3B
	When I push events to the server for client id 'TestClient'
	Then No commits should be pushed

Scenario: Updating server when server not available
	Given the server is unavailable
	When I push events to the server for client id 'TestClient'
	Then the successful completion of the synchronisation should not be signalled
	And the unsuccessful completion of the synchronisation should be signalled
		