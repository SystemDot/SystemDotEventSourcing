Feature: PullEvents
	In order to ensure devices synchronise events from the server
	I want to make events from the server available for synchronisation with devices and update the devices with those events

	Scenario: Updating local device with events from a commit retreived from the server
		Given I have created a synchronisable commit with an id of E261A67D-2C00-4854-A0FF-6DEFA84A4276 and stream identified as '1157AC59-AD0D-4BF0-9CC1-238BDE2CEFB9' and client identified as 'TestClient'
		And I add an event origin for the local machine as a header of the synchronisable commit
		And I add a serialised event with an id of E261A67D-2C00-4854-A0FF-6DEFA84A4277 to the commit
		And I add a serialised event with an id of F261A67D-2C00-4854-A0FF-6DEFA84A4277 to the commit
		And I set the synchronisable commit to be returned from the server
		And I have created a new event session
		When I synchronise commits for the client id 'TestClient'
		Then the commits should have been retreived from the server with the correct address
		And the commits should have been retreived from the server from the beggining of time 
		When I use the commit in the session with an id of E261A67D-2C00-4854-A0FF-6DEFA84A4276
		Then the commit should be for a stream identified as '1157AC59-AD0D-4BF0-9CC1-238BDE2CEFB9'
		And the commit should contain an event with an id of E261A67D-2C00-4854-A0FF-6DEFA84A4277
		And the commit should contain an event with an id of F261A67D-2C00-4854-A0FF-6DEFA84A4277
		And the successful completion of the synchronisation should be signalled with the date of the last pull commit
		And none of the pulled events should be dispatched

	Scenario: local commits should get dispatched
		Given I have created a new event session
		And I add an event origin for the local machine as a header for the stream identified as 1157AC59-AD0D-4BF0-9CC1-238BDE2CEFB9 in the bucket identified as 'TestClient'
		And I have created an event in the session with an id of E261A67D-2C00-4854-A0FF-6DEFA84A4276 for the stream identified as 1157AC59-AD0D-4BF0-9CC1-238BDE2CEFB9 in the bucket identified as 'TestClient'
		And I have created an event in the session with an id of F261A67D-2C00-4854-A0FF-6DEFA84A4276 for the stream identified as 1157AC59-AD0D-4BF0-9CC1-238BDE2CEFB9 in the bucket identified as 'TestClient'
		And I commit the session with the id DAD11DA9-64C6-4955-AF82-F12B66FBAF3B
		Then events should be dispatched

	Scenario: Updating local device with two commits retreived from the server
		Given I have created a synchronisable commit with an id of E261A67D-2C00-4854-A0FF-6DEFA84A4276 and stream identified as '1157AC59-AD0D-4BF0-9CC1-238BDE2CEFB9' and client identified as 'TestClient'
		And I add an event origin for the local machine as a header of the synchronisable commit
		And I add a serialised event with an id of E261A67D-2C00-4854-A0FF-6DEFA84A4277 to the commit
		And I set the synchronisable commit to be returned from the server
		And I have created a synchronisable commit with an id of F261A67D-2C00-4854-A0FF-6DEFA84A4276 and stream identified as '2157AC59-AD0D-4BF0-9CC1-238BDE2CEFB9' and client identified as 'TestClient'
		And I add an event origin for the local machine as a header of the synchronisable commit
		And I add a serialised event with an id of E261A67D-2C00-4854-A0FF-6DEFA84A4278 to the commit
		And I set the synchronisable commit to be returned from the server
		And I have created a new event session
		When I synchronise commits for the client id 'TestClient'
		Then the commits should have been retreived from the server with the correct address
		And the commits should have been retreived from the server from the beggining of time 
		When I use the commit in the session with an id of E261A67D-2C00-4854-A0FF-6DEFA84A4276
		Then the commit should be for a stream identified as '1157AC59-AD0D-4BF0-9CC1-238BDE2CEFB9'
		When I use the commit in the session with an id of F261A67D-2C00-4854-A0FF-6DEFA84A4276
		Then the commit should be for a stream identified as '2157AC59-AD0D-4BF0-9CC1-238BDE2CEFB9'
	
	Scenario: Updating local device when commits only exist for a different client
		Given I have created a synchronisable commit with an id of E261A67D-2C00-4854-A0FF-6DEFA84A4276 and stream identified as '1157AC59-AD0D-4BF0-9CC1-238BDE2CEFB9' and client identified as 'OtherTestClient'
		And I add a serialised event with an id of E261A67D-2C00-4854-A0FF-6DEFA84A4277 to the commit
		And I set the synchronisable commit to be returned from the server
		And I have created a new event session
		When I synchronise commits for the client id 'TestClient'
		Then a commit with an id of E261A67D-2C00-4854-A0FF-6DEFA84A4276 should not exist in the session

	Scenario: Updating local device when server not available
		Given the server is unavailable
		When I synchronise commits for the client id 'TestClient'
		Then the successful completion of the synchronisation should not be signalled
		And the unsuccessful completion of the synchronisation should be signalled
		