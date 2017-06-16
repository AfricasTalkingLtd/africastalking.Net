Option Infer On

Imports System
Imports System.Collections.Generic

Public Class TestMobilePaymentB2C
    Public Shared Sub MobilePaymentB2C()
        'Specify your credentials
        'string username = "allanwenzslaus";
        'string apiKey   = "538957ab288c6a224a676ad0a2fbd59dd5f3a4624655a388abaf1337ced8851d";
        Dim username As String = "kanyi"
        Dim apiKey As String = "9eb01a98435e1d0cff3e8f6e5c408fa330add662bfcc0a7e9ddcd09b2a0c62fa"

        'NOTE: If connecting to the sandbox, please use your sandbox login credentials

        'Create an instance of our awesome gateway class and pass your credentials
        Dim gateway As New AfricasTalkingGateway(username, apiKey)

        ' NOTE: If connecting to the sandbox, please add the sandbox flag to the constructor:
        '                            <summary>
        '***********************************************************************************
        '                  ****SANDBOX****
        'Dim AfricasTalkingGateway gateway As new AfricasTalkingGateway(username, apiKey,"environment");
        ' *************************************************************************************
        '                       </summary>

        ' Specify the name of your Africa's Talking payment product
        Dim productName As String = "kanyi"

        ' The 3-Letter ISO currency code for the checkout amount
        Dim currencyCode As String = "KES"

        ' Provide the details of a mobile money recipient
        Dim recipient1 As New MobilePaymentB2CRecipient("+254700833493", "KES", 10D)
        recipient1.AddMetadata("name", "Clerk")
        recipient1.AddMetadata("reason", "May Salary")

        ' You can provide up to 10 recipients at a time
        ' Dim recipient2 As New MobilePaymentB2CRecipient("+254741740563", "KES", 10D)
        'recipient2.AddMetadata("name", "Accountant")
        'recipient2.AddMetadata("reason", "May Salary")

        ' Put the recipients into an array
        Dim recipients As IList(Of MobilePaymentB2CRecipient) = New List(Of MobilePaymentB2CRecipient)()
        recipients.Add(recipient1)
        'recipients.Add(recipient2)
        Try
            Dim responses = gateway.MobilePaymentB2CRequest(productName, recipients)
            Console.WriteLine(responses)
        Catch ex As Exception
            Console.WriteLine("Received error response: " & ex.Message)
        End Try
        Console.ReadLine()
    End Sub
End Class
