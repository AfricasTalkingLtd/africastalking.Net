	' Sending Messages using sender id/short code
	Imports System
	Imports System.Collections
	Friend Class Smsshortcode
    Public Shared Sub _Smsshortcode()

        Dim username As String = "MyAfricasTalkingUsername"
        Dim apiKey As String = "MyAfricasTalkingAPIKey"

        Dim recipients As String = "+254711XXXYYY,+254733YYYZZZ"

        Dim message As String = "I'm a lumberjack and its ok, I sleep all night and I work all day"

        ' Specify your AfricasTalking shortCode or sender id
        Dim from As String = "shortCode or senderId"

        Dim gateway As New AfricasTalkingGateway(username, apiKey)

        Try

            Dim results As Object = gateway.sendMessage(recipients, message, from)

            For Each result As Object In results
                Console.Write(CStr(result("number")) & ",")
                Console.Write(CStr(result("status")) & ",")
                Console.Write(CStr(result("messageId")) & ",")
                Console.WriteLine(CStr(result("cost")))
            Next result
        Catch e As AfricasTalkingGatewayException
            Console.WriteLine("Encountered an error: " & e.Message)
        End Try
        Console.Read()
    End Sub
End Class