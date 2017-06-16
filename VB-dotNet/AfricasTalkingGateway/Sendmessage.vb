Imports System
Imports System.Collections
Public Class Sendmessage
    Public Shared Sub Main()

        ' Specify your login credentials
        Dim username As String = "MyAfricasTalkingUsername"
        Dim apiKey As String = "MyAfricasTalkingAPIKey"
        ' Specify the numbers that you want to send to in a comma-separated list
        ' Please ensure you include the country code (+254 for Kenya in this case)
        Dim recipients As String = "+254700YYYXXX"
        ' And of course we want our recipients to know what we really do
        Dim message As String = "I'm a lumberjack and its ok, I sleep all night and I work all day"

        ' Create a new instance of our awesome gateway class
        Dim gateway As New AfricasTalkingGateway(username, apiKey)
        ' Any gateway errors will be captured by our custom Exception class below,
        ' so wrap the call in a try-catch block   
        Try
            ' Thats it, hit send and we'll take care of the rest

            Dim results As Object = gateway.sendMessage(recipients, message)
            For Each result As Object In results
                Console.Write(CStr(result("number")) & ",")
                Console.Write(CStr(result("status")) & ",") ' status is either "Success" or "error message"
                Console.Write(CStr(result("messageId")) & ",")
                Console.WriteLine(CStr(result("cost")))
            Next result
        Catch e As AfricasTalkingGatewayException
            Console.WriteLine("Encountered an error: " & e.Message)
        End Try
        Console.Read()
    End Sub
End Class
