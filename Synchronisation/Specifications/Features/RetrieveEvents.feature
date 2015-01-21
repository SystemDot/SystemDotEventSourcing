Feature: RetrieveEvents
	In order to ensure devices synchronise events from the server
	I want to make events from the server available for synchronisation with devices and update the devices with those events

	Scenario: Retrieving commits from the server and deserialising its events
		Given I have created a new event session
		And I have created an event in the session with an id of E261A67D-2C00-4854-A0FF-6DEFA84A4276 for the stream identified as 1157AC59-AD0D-4BF0-9CC1-238BDE2CEFB9
		And I have created an event in the session with an id of F261A67D-2C00-4854-A0FF-6DEFA84A4276 for the stream identified as 2157AC59-AD0D-4BF0-9CC1-238BDE2CEFB9
		And I commit the session with the id DAD11DA9-64C6-4955-AF82-F12B66FBAF3B
		When I request events for synchronisation
		And I use the first synchronisable commit requested
		And I use the first commit in the event session
		And I deserialise the synchronisable commit events
		Then the synchronisable commit should have the same id as the commit
		And the synchronisable commit should be for the same stream as the commit
		And the synchronisable commit should be for the same date and time as the commit
		And the deserialised events should contain an event an id of E261A67D-2C00-4854-A0FF-6DEFA84A4276
		When I use the second synchronisable commit requested
		And I use the second commit in the event session
		And I deserialise the synchronisable commit events
		Then the synchronisable commit should have the same id as the commit
		And the synchronisable commit should be for the same stream as the commit
		And the synchronisable commit should be for the same date and time as the commit
		And the deserialised events should contain an event an id of F261A67D-2C00-4854-A0FF-6DEFA84A4276

	@TestClientSynchronisation
	Scenario: Updating local device with events from commits retreived from the server
		Given I have initialised the client synchronisation process with the server address and client id of 'TestClient'
		And I have created a synchronisable commit with an id of E261A67D-2C00-4854-A0FF-6DEFA84A4276 and stream identified as '1157AC59-AD0D-4BF0-9CC1-238BDE2CEFB9' for the current date and time
		And I add a serialised event with an id of E261A67D-2C00-4854-A0FF-6DEFA84A4277 to the commit
		And I add a serialised event with an id of F261A67D-2C00-4854-A0FF-6DEFA84A4277 to the commit
		And I set the synchronisable commit to be returned from the server
		Given I have created a synchronisable commit with an id of F261A67D-2C00-4854-A0FF-6DEFA84A4276 and stream identified as '2157AC59-AD0D-4BF0-9CC1-238BDE2CEFB9' for the current date and time
		And I add a serialised event with an id of E261A67D-2C00-4854-A0FF-6DEFA84A4278 to the commit
		And I add a serialised event with an id of F261A67D-2C00-4854-A0FF-6DEFA84A4278 to the commit
		And I set the synchronisable commit to be returned from the server
		And I have created a new event session
		When I synchronise the client with the server with client id 'TestClient'
		Then the end of the synchronisation should be signalled
		When I use the commit in the session with an id of E261A67D-2C00-4854-A0FF-6DEFA84A4276
		Then the commit should be for a stream identified as '1157AC59-AD0D-4BF0-9CC1-238BDE2CEFB9'
		And the commit should contain an event with an id of E261A67D-2C00-4854-A0FF-6DEFA84A4277
		And the commit should contain an event with an id of F261A67D-2C00-4854-A0FF-6DEFA84A4277
		When I use the commit in the session with an id of F261A67D-2C00-4854-A0FF-6DEFA84A4276
		Then the commit should be for a stream identified as '2157AC59-AD0D-4BF0-9CC1-238BDE2CEFB9'
		And the commit should contain an event with an id of E261A67D-2C00-4854-A0FF-6DEFA84A4278
		And the commit should contain an event with an id of F261A67D-2C00-4854-A0FF-6DEFA84A4278

	@TestClientSynchronisation
	Scenario: Updating local device with events from commits retreived from the server twice
		Given I have initialised the client synchronisation process with the server address and client id of 'TestClient'
		And I have created a synchronisable commit with an id of A261A67D-2C00-4854-A0FF-6DEFA84A4276 and stream identified as '1157AC59-AD0D-4BF0-9CC1-238BDE2CEFB9' for the current date and time
		And I add a serialised event with an id of E261A67D-2C00-4854-A0FF-6DEFA84A4277 to the commit
		And I set the synchronisable commit to be returned from the server
		And I have created a new event session
		When I synchronise the client with the server with client id 'TestClient'
		Given I have set the current date and time back 1 millisecond
		And I have created a synchronisable commit with an id of B261A67D-2C00-4854-A0FF-6DEFA84A4276 and stream identified as '2157AC59-AD0D-4BF0-9CC1-238BDE2CEFB9' for the current date and time
		When I synchronise the client with the server with client id 'TestClient'
		Then there should be a commit in the session with an id of A261A67D-2C00-4854-A0FF-6DEFA84A4276
		And there should not be a commit in the session with an id of B261A67D-2C00-4854-A0FF-6DEFA84A4276