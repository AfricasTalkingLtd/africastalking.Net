'Sending premium rated messages
Imports System
Imports System.Collections
Friend Class Smspremiumrate
    Public Shared Sub _Smspremiumrate()

        Dim username As String = "MyAfricasTalkingUsername"
        Dim apiKey As String = "MyAfricasTalkingAPIKey"

        Dim recipients As String = "+254711XXXYYY,+254733YYYZZZ"

        Dim message As String = "Get your daily message and that's how we roll."

        ' Specify your premium shortCode and keyword
        Dim shortCode As String = "XXXXX"
        Dim keyword As String = "premiumKeyword"

        ' Set the bulkSMSMode flag to 0 so that the subscriber get charged
        Dim bulkSMSMode As Integer = 0

        ' Create an array which would hold the following parameters:
        ' keyword: Your premium keyword,
        ' retryDurationInHours: The numbers of hours our API should retry to send the message 
        ' incase it doesn't go through. It is optional

        Dim options As New Hashtable()
        options("keyword") = keyword
        options("retryDurationInHours") = "No. of hours to retry sending message"

        Dim gateway As New AfricasTalkingGateway(username, apiKey)

        Try

            Dim results As Object = gateway.sendMessage(recipients, message, shortCode, bulkSMSMode, options)

            For Each result As Object In results
                Console.Write(CStr(result("number")) & ",")
                Console.Write(CStr(result("status")) & ",")
                Console.WriteLine(CStr(result("messageId")))
            Next result
        Catch e As AfricasTalkingGatewayException
            Console.WriteLine("Encountered an error: " & e.Message)
        End Try
    End Sub
End Class