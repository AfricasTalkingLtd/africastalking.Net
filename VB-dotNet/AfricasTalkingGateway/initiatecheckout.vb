Imports System
Imports System.Collections

Friend Class initiatecheckout
    Public Shared Sub Initiatecheckout()
        'Create an instance of our awesome gateway class and pass your credentials
        Dim username As String = "kanyi"
        Dim apiKey As String = "9eb01a98435e1d0cff3e8f6e5c408fa330add662bfcc0a7e9ddcd09b2a0c62fa"
        ' Specify the name of your Africa's Talking payment product
        Dim productName As String = "kanyi"
        ' The phone number of the customer checking out
        Dim phoneNumber As String = "+254700833493"
        ' The 3-Letter ISO currency code for the checkout amount
        Dim currencyCode As String = "KES"
        ' The checkout amount
        Dim amount As Integer = 500
        ' The provider Channel - Optional
        Dim providerChannel As String = "105296"

        ' Create a new instance of our awesome gateway class
        Dim gateway As New AfricasTalkingGateway(username, apiKey)

        ' NOTE: If connecting to the sandbox, please add the sandbox flag to the constructor:
        '''            ***********************************************************************************
        '''                                    ****SANDBOX****            
        '''            *************************************************************************************
        ' AfricasTalkingGateway gateway = new AfricasTalkingGateway(username, apiKey, "environment");
        ' Dim gateway As New AfricasTalkingGateway(username, apiKey, "environment")
        ' Any gateway errors will be captured by our custom Exception class below,

        Try
            ' Initiate the checkout. If successful, you will get back a json response

            Dim checkoutResponse As Object = gateway.initiateMobilePaymentCheckout(productName, phoneNumber, currencyCode, amount, providerChannel)

            Console.WriteLine(checkoutResponse)

        Catch e As AfricasTalkingGatewayException
            Console.WriteLine("Encountered an error: " & e.Message)
        End Try
        Console.Read()
    End Sub
End Class

