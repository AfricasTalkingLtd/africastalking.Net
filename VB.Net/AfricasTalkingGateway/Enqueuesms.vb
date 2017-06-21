'Message queueing 
Imports System
Imports System.Collections
Public Class Enqueuesms
    Public Shared Sub _Enqueuesms()

        Dim username As String = "MyAfricasTalkingUsername"
        Dim apiKey As String = "MyAfricasTalkingAPIKey"

        Dim recipients As String = "+254711XXXYYY,+254733YYYZZZ"

        Dim message As String = "I'm a lumberjack and its ok, I sleep all night and I work all day"

        Dim from As String = Nothing '$from = "shortCode or senderId";

        Dim bulkSMSMode As Integer = 1 ' This should always be 1 for bulk messages

        ' enqueue flag is used to queue messages incase you are sending a high volume.
        ' The default value is 0.
        Dim options As New Hashtable()
        options("enqueue") = 1

        Dim gateway As New AfricasTalkingGateway(username, apiKey)
        ' NOTE: If connecting to the sandbox, please add the sandbox flag to the constructor:
        '          ***********************************************************************************
        '                                   ****SANDBOX****            
        '         ************************************************************************************
        ' Dim gateway As New AfricasTalkingGateway(username, apiKey, "environment")
        ' Any gateway errors will be captured by our custom Exception class below,
        Try


            Dim results As Object = gateway.sendMessage(recipients, message, from, bulkSMSMode, options)

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
