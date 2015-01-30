Feature: RetrieveEvents
	In order to ensure devices synchronise events from the server
	I want to make events from the server available for synchronisation with devices and update the devices with those events

	Scenario: Updating local device with events from a commit retreived from the server
		Given I have initialised the client synchronisation process with the server address of 'http://localhost:1234/' and client id of 'TestClient'
		And I have created a synchronisable commit with an id of E261A67D-2C00-4854-A0FF-6DEFA84A4276 and stream identified as '1157AC59-AD0D-4BF0-9CC1-238BDE2CEFB9' and client identified as 'TestClient'
		And I add a serialised event with an id of E261A67D-2C00-4854-A0FF-6DEFA84A4277 to the commit
		And I add a serialised event with an id of F261A67D-2C00-4854-A0FF-6DEFA84A4277 to the commit
		And I set the synchronisable commit to be returned from the server
		And I have created a new event session
		When I synchronise the client with the server
		Then the successful completion of the synchronisation should be signalled
		And the commits should have been retreived from the server with the address 'http://localhost:1234/'
		And the commits should have been retreived from the server from the beggining of time 
		When I use the commit in the session with an id of E261A67D-2C00-4854-A0FF-6DEFA84A4276
		Then the commit should be for a stream identified as '1157AC59-AD0D-4BF0-9CC1-238BDE2CEFB9'
		And the commit should contain an event with an id of E261A67D-2C00-4854-A0FF-6DEFA84A4277
		And the commit should contain an event with an id of F261A67D-2C00-4854-A0FF-6DEFA84A4277

	Scenario: Updating local device with two commits retreived from the server
		Given I have initialised the client synchronisation process with the server address of 'http://localhost:1234/' and client id of 'TestClient'
		And I have created a synchronisable commit with an id of E261A67D-2C00-4854-A0FF-6DEFA84A4276 and stream identified as '1157AC59-AD0D-4BF0-9CC1-238BDE2CEFB9' and client identified as 'TestClient'
		And I add a serialised event with an id of E261A67D-2C00-4854-A0FF-6DEFA84A4277 to the commit
		And I set the synchronisable commit to be returned from the server
		Given I have created a synchronisable commit with an id of F261A67D-2C00-4854-A0FF-6DEFA84A4276 and stream identified as '2157AC59-AD0D-4BF0-9CC1-238BDE2CEFB9' and client identified as 'TestClient'
		And I add a serialised event with an id of E261A67D-2C00-4854-A0FF-6DEFA84A4278 to the commit
		And I set the synchronisable commit to be returned from the server
		And I have created a new event session
		When I synchronise the client with the server
		Then the successful completion of the synchronisation should be signalled
		And the commits should have been retreived from the server with the address 'http://localhost:1234/'
		And the commits should have been retreived from the server from the beggining of time 
		When I use the commit in the session with an id of E261A67D-2C00-4854-A0FF-6DEFA84A4276
		Then the commit should be for a stream identified as '1157AC59-AD0D-4BF0-9CC1-238BDE2CEFB9'
		When I use the commit in the session with an id of F261A67D-2C00-4854-A0FF-6DEFA84A4276
		Then the commit should be for a stream identified as '2157AC59-AD0D-4BF0-9CC1-238BDE2CEFB9'
	
	Scenario: Updating local device when commits only exist for a different client
		Given I have initialised the client synchronisation process with the server address of 'http://localhost:1234/' and client id of 'TestClient'
		And I have created a synchronisable commit with an id of E261A67D-2C00-4854-A0FF-6DEFA84A4276 and stream identified as '1157AC59-AD0D-4BF0-9CC1-238BDE2CEFB9' and client identified as 'OtherTestClient'
		And I add a serialised event with an id of E261A67D-2C00-4854-A0FF-6DEFA84A4277 to the commit
		And I set the synchronisable commit to be returned from the server
		And I have created a new event session
		When I synchronise the client with the server
		Then a commit with an id of E261A67D-2C00-4854-A0FF-6DEFA84A4276 should not exist in the session
		
	Scenario: Updating local device with events from commits retreived from the server twice
		Given I have initialised the client synchronisation process with the server address of 'http://localhost:1234/' and client id of 'TestClient'
		And I have created a synchronisable commit with an id of A261A67D-2C00-4854-A0FF-6DEFA84A4276 and stream identified as '1157AC59-AD0D-4BF0-9CC1-238BDE2CEFB9' and client identified as 'TestClient'
		And I add a serialised event with an id of E261A67D-2C00-4854-A0FF-6DEFA84A4277 to the commit
		And I set the synchronisable commit to be returned from the server
		And I have created a new event session
		When I synchronise the client with the server
		And I synchronise the client with the server
		Then the commits should have been retreived from the server from the date of the last commit synchronised

	Scenario: Updating local device when server not available
		Given I have initialised the client synchronisation process with the server address of 'http://localhost:1234/' and client id of 'TestClient'
		And the server is unavailable
		When I synchronise the client with the server
		Then the unsuccessful completion of the synchronisation should be signalled
		And the successful completion of the synchronisation should not be signalled
		