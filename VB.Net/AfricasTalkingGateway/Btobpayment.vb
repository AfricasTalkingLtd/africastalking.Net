
Public Class BtoBpayment
    Public Shared Sub _BtoBpayment()
        'Specify your credentials
        Dim username As String = "MyAfricasTalkingUsername"
        Dim apiKey As String = "MyAfricasTalkingAPIKey"
        'Create an instance of our awesome gateway class and pass your credentials
        ' Specify the name of your Africa's Talking payment product
        Dim productName As String = "myPaymentProductName"
        ' The phone number of the customer checking out
        Dim provider As String = "myPaymentProvider"
        ' Transfer Type
        Dim transferType As String = "transferType"
        ' The 3-Letter ISO currency code for the b2b amount
        Dim currencyCode As String = "KES"
        ' The amount
        Dim amount As Integer = 100
        ' The destination Channel - Optional
        Dim destinationChannel As String = "partnerBusinessChannel"
        ' The destination Account
        Dim destinationAccount As String = "partnerBusinessAccount"
        ' Create a new instance of our awesome gateway class
        Dim gateway As New AfricasTalkingGateway(username, apiKey)        
        ' NOTE: If connecting to the sandbox, please add the sandbox flag to the constructor:
        '          ***********************************************************************************
        '                                   ****SANDBOX****            
        '           *************************************************************************************
        ' Dim gateway As New AfricasTalkingGateway(username, apiKey, "environment")
         ' Any gateway errors will be captured by our custom Exception class below,
        Try
            ' Initiate the b2b request. If successful, you will get back a json response
            Dim b2bResponse As Object = gateway.MobileB2B(productName, provider, transferType, currencyCode, amount, destinationChannel, destinationAccount)
            Console.WriteLine(b2bResponse)
        Catch e As AfricasTalkingGatewayException
            Console.WriteLine("Encountered an error: " & e.Message)
        End Try
        Console.Read()
    End Sub
End Class
