
' Sending onDemand premium messages
Imports System
Imports System.Collections
Friend Class Smspremiumondemand
    Public Shared Sub _Smspremiumondemand()

        Dim username As String = "MyAfricasTalkingUsername"
        Dim apiKey As String = "MyAfricasTalkingAPIKey"

        Dim recipients As String = "+254711XXXYYY,+254733YYYZZZ"

        Dim message As String = "Get your daily message and thats how we roll."

        Dim shortCode As String = "XXXXX"
        Dim keyword As String = "premiumKeyword" ' string keyword = null;

        Dim bulkSMSMode As Integer = 0

        ' Create a hashtable which would hold the parameters keyword, retryDurationInHours and linkId
        ' linkId is received from the message sent by subscriber to your onDemand service
        Dim linkId As String = "messageLinkId"

        Dim options As New Hashtable()
        options("keyword") = keyword
        options("linkId") = linkId
        options("retryDurationInHours") = "No.of hours [to] retry sending message"

        Dim gateway As New AfricasTalkingGateway(username, apiKey)

        Try

            Dim results As Object = gateway.sendMessage(recipients, message, shortCode, bulkSMSMode, options)

            For Each result As Object In results
                Console.Write(CStr(result("number")) & ",")
                Console.Write(CStr(result("status")) & ",")
                Console.Write(CStr(result("messageId")) & ",")
            Next result
        Catch e As AfricasTalkingGatewayException
            Console.WriteLine("Encountered an error: " & e.Message)
        End Try
        Console.ReadLine()
    End Sub
End Class