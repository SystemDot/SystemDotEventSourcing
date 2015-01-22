Feature: RetrieveEvents
	In order to ensure devices synchronise events from the server
	I want to make events from the server available for synchronisation with devices and update the devices with those events

	Scenario: Updating local device with events from commits retreived from the server
		Given I have initialised the client synchronisation process with the server address of 'http://localhost:1234/' and client id of 'TestClient'
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
		And the commits should have been retreived from the server with the address 'http://localhost:1234/'
		And the commits should have been retreived from the server from the beggining of time 
		When I use the commit in the session with an id of E261A67D-2C00-4854-A0FF-6DEFA84A4276
		Then the commit should be for a stream identified as '1157AC59-AD0D-4BF0-9CC1-238BDE2CEFB9'
		And the commit should contain an event with an id of E261A67D-2C00-4854-A0FF-6DEFA84A4277
		And the commit should contain an event with an id of F261A67D-2C00-4854-A0FF-6DEFA84A4277
		When I use the commit in the session with an id of F261A67D-2C00-4854-A0FF-6DEFA84A4276
		Then the commit should be for a stream identified as '2157AC59-AD0D-4BF0-9CC1-238BDE2CEFB9'
		And the commit should contain an event with an id of E261A67D-2C00-4854-A0FF-6DEFA84A4278
		And the commit should contain an event with an id of F261A67D-2C00-4854-A0FF-6DEFA84A4278

	Scenario: Updating local device with events from commits retreived from the server twice
		Given I have initialised the client synchronisation process with the server address of 'http://localhost:1234/' and client id of 'TestClient'
		And I have created a synchronisable commit with an id of A261A67D-2C00-4854-A0FF-6DEFA84A4276 and stream identified as '1157AC59-AD0D-4BF0-9CC1-238BDE2CEFB9' for the current date and time
		And I add a serialised event with an id of E261A67D-2C00-4854-A0FF-6DEFA84A4277 to the commit
		And I set the synchronisable commit to be returned from the server
		And I have created a new event session
		When I synchronise the client with the server with client id 'TestClient'
		And I synchronise the client with the server with client id 'TestClient'
		Then the commits should have been retreived from the server from the date of the last commit synchronised