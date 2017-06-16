Imports System
Imports System.Collections
Imports System.Text
Imports System.Web
Imports System.IO
Imports System.Net
Imports System.Web.Script.Serialization
Imports System.Net.Http
Imports Newtonsoft.Json
Imports System.Collections.Generic
Imports System.Linq

'Note add System.Web.Script.Serialization, using System.Net.Http, Newtonsoft.Json, sytem.Net.Http.Formatting extension reference to the sln
Public Class AfricasTalkingGatewayException
	Inherits Exception

	Public Sub New(ByVal message As String)
		MyBase.New(message)
	End Sub
	Public Sub New(ByVal ex As Exception)
		MyBase.New(ex.Message, ex)

	End Sub
End Class
Public Class AfricasTalkingGateway
	Private _username As String
	Private _apiKey As String
	Private _environment As String
	Private responseCode As Integer
	Private serializer As JavaScriptSerializer

	'Change the debug flag to true to view the full response
	Private DEBUG As Boolean = False

	Public Sub New(ByVal username_ As String, ByVal apiKey_ As String)
		_username = username_
		_apiKey = apiKey_
		_environment = "production"
		serializer = New JavaScriptSerializer()
	End Sub

	Public Sub New(ByVal username As String, ByVal apiKey As String, ByVal environment As String)
		_username = username
		_apiKey = apiKey
		_environment = environment
		serializer = New JavaScriptSerializer()
	End Sub

    Public  Function sendMessage(ByVal to_ As String, ByVal message_ As String, Optional ByVal from_ As String = Nothing, Optional ByVal bulkSMSMode_ As Integer = 1, Optional ByVal options_ As Hashtable = Nothing) As Object
        Dim data As New Hashtable()
        data("username") = _username
        data("to") = to_
        data("message") = message_

        If from_ IsNot Nothing Then
            data("from") = from_
            data("bulkSMSMode") = Convert.ToString(bulkSMSMode_)

            If options_ IsNot Nothing Then
                If options_.Contains("keyword") Then
                    data("keyword") = options_("keyword")
                End If

                If options_.Contains("linkId") Then
                    data("linkId") = options_("linkId")
                End If

                If options_.Contains("enqueue") Then
                    data("enqueue") = options_("enqueue")
                End If

                If options_.Contains("retryDurationInHours") Then
                    data("retryDurationInHours") = options_("retryDurationInHours")
                End If
            End If
        End If

        Dim response As String = sendPostRequest(data, SMS_URLString)
        If responseCode = CInt(HttpStatusCode.Created) Then
            Dim json = serializer.Deserialize(Of Object)(response)
            Dim recipients As Object = json("SMSMessageData")("Recipients")
            If recipients.Length > 0 Then
                Return recipients
            End If
            Throw New AfricasTalkingGatewayException(CType(json("SMSMessageData")("Message"), String))

        End If
        Throw New AfricasTalkingGatewayException(response)
    End Function
    Public Function fetchMessages(ByVal lastReceivedId_ As Integer) As Object
        Dim url As String = SMS_URLString & "?username=" & _username & "&lastReceivedId=" & Convert.ToString(lastReceivedId_)
        Dim response As String = sendGetRequest(url)
        If responseCode = CInt(HttpStatusCode.OK) Then
            Dim json As Object = serializer.DeserializeObject(response)
            Return json("SMSMessageData")("Messages")
        End If
        Throw New AfricasTalkingGatewayException(response)
    End Function

    Public Function createSubscription(ByVal phoneNumber_ As String, ByVal shortCode_ As String, ByVal keyword_ As String) As Object
		If phoneNumber_.Length = 0 OrElse shortCode_.Length = 0 OrElse keyword_.Length = 0 Then
			Throw New AfricasTalkingGatewayException("Please supply phone number, short code and keyword")
		End If
		Dim data_ As New Hashtable()
		data_ ("username") = _username
		data_ ("phoneNumber") = phoneNumber_
		data_ ("shortCode") = shortCode_
		data_ ("keyword") = keyword_
		Dim urlString As String = SUBSCRIPTION_URLString & "/create"
		Dim response As String = sendPostRequest(data_, urlString)
		If responseCode = CInt(HttpStatusCode.Created) Then
            Dim json As Object = serializer.Deserialize(Of Object)(response)
            Return json
		End If
		Throw New AfricasTalkingGatewayException(response)
	End Function
    Public Function deleteSubscription(ByVal phoneNumber_ As String, ByVal shortCode_ As String, ByVal keyword_ As String) As Object
        If phoneNumber_.Length = 0 OrElse shortCode_.Length = 0 OrElse keyword_.Length = 0 Then
            Throw New AfricasTalkingGatewayException("Please supply phone number, short code and keyword")
        End If
        Dim data_ As New Hashtable()
        data_("username") = _username
        data_("phoneNumber") = phoneNumber_
        data_("shortCode") = shortCode_
        data_("keyword") = keyword_
        Dim urlString As String = SUBSCRIPTION_URLString & "/delete"
        Dim response As String = sendPostRequest(data_, urlString)
        If responseCode = CInt(HttpStatusCode.Created) Then
            Dim json As Object = serializer.Deserialize(Of Object)(response)
            Return json
        End If
        Throw New AfricasTalkingGatewayException(response)
    End Function
    Public Function [call](ByVal from_ As String, ByVal to_ As String) As Object
        Dim data As New Hashtable()
        data("username") = _username
        data("from") = from_
        data("to") = to_
        Dim urlString As String = VOICE_URLString & "/call"
        Dim response As String = sendPostRequest(data, urlString)
        Dim json As Object = serializer.Deserialize(Of Object)(response)
        If CStr(json("errorMessage")) = "None" Then
            Return json("entries")
        End If
        Throw New AfricasTalkingGatewayException(CType(json("errorMessage"), String))
    End Function
    Public Function getNumQueuedCalls(ByVal phoneNumber_ As String, Optional ByVal queueName_ As String = Nothing) As Integer
        Dim data As New Hashtable()
        data("username") = _username
        data("phoneNumbers") = phoneNumber_
        If queueName_ IsNot Nothing Then
            data("queueName") = queueName_
        End If

        Dim urlString As String = VOICE_URLString & "/queueStatus"
        Dim response As String = sendPostRequest(data, urlString)
        Dim json As Object = serializer.Deserialize(Of Object)(response)
        If CStr(json("errorMessage")) = "None" Then
            Return json("entries")
        End If
        Throw New AfricasTalkingGatewayException(CType(json("errorMessage"), String))
    End Function
    Public Sub uploadMediaFile(ByVal url_ As String)
		Dim data As New Hashtable()
		data ("username") = _username
		data ("url") = url_

		Dim urlString As String = VOICE_URLString & "/mediaUpload"
		Dim response As String = sendPostRequest(data, urlString)
        Dim json As Object = serializer.Deserialize(Of Object)(response)
        If CStr(json("errorMessage")) <> "None" Then
            Throw New AfricasTalkingGatewayException(CType(json("errorMessage"), String))
        End If
	End Sub
    Public Function sendAirtime(ByVal recipients_ As ArrayList) As Object
        Dim urlString As String = AIRTIME_URLString & "/send"
        Dim recipients As String = (New JavaScriptSerializer()).Serialize(recipients_)
        Dim data As New Hashtable()
        data("username") = _username
        data("recipients") = recipients
        Dim response As String = sendPostRequest(data, urlString)
        If responseCode = CInt(HttpStatusCode.Created) Then
            Dim json As Object = serializer.Deserialize(Of Object)(response)
            If json("responses").Count > 0 Then
                Return json("responses")
            End If
            Throw New AfricasTalkingGatewayException(CType(json("errorMessage"), String))
        End If
        Throw New AfricasTalkingGatewayException(response)
    End Function

    Public Function getUserData() As Object
		Dim urlString As String = USERDATA_URLString & "?username=" & _username
		Dim response As String = sendGetRequest(urlString)
		If responseCode = CInt(HttpStatusCode.OK) Then
            Dim json As Object = serializer.Deserialize(Of Object)(response)
            Return json ("UserData")
		End If
		Throw New AfricasTalkingGatewayException(response)
	End Function

	Private Function sendPostRequest(ByVal dataMap_ As Hashtable, ByVal urlString_ As String) As String
		Try
			Dim dataStr As String = ""
			For Each key As String In dataMap_.Keys
				If dataStr.Length > 0 Then
					dataStr &= "&"
				End If
				Dim value As String = DirectCast(dataMap_(key), String)
				dataStr &= HttpUtility.UrlEncode(key, Encoding.UTF8)
				dataStr &= "=" & HttpUtility.UrlEncode(value, Encoding.UTF8)
			Next key

			Dim byteArray() As Byte = Encoding.UTF8.GetBytes(dataStr)

			System.Net.ServicePointManager.ServerCertificateValidationCallback = AddressOf RemoteCertificateValidationCallback
			Dim webRequest As HttpWebRequest = CType(System.Net.WebRequest.Create(urlString_), HttpWebRequest)

			webRequest.Method = "POST"
			webRequest.ContentType = "application/x-www-form-urlencoded"
			webRequest.ContentLength = byteArray.Length
			webRequest.Accept = "application/json"

			webRequest.Headers.Add("apiKey", _apiKey)

			Dim webpageStream As Stream = webRequest.GetRequestStream()
			webpageStream.Write(byteArray, 0, byteArray.Length)
			webpageStream.Close()

			Dim httpResponse As HttpWebResponse = CType(webRequest.GetResponse(), HttpWebResponse)
			responseCode = CInt(httpResponse.StatusCode)
			Dim webpageReader As New StreamReader(httpResponse.GetResponseStream())
			Dim response As String = webpageReader.ReadToEnd()

			If DEBUG Then
				Console.WriteLine("Full response: " & response)
			End If

			Return response

		Catch ex As WebException
			If ex.Response Is Nothing Then
				Throw New AfricasTalkingGatewayException(ex.Message)
			End If
			Using stream = ex.Response.GetResponseStream()
			Using reader = New StreamReader(stream)
				Dim response As String = reader.ReadToEnd()

				If DEBUG Then
					Console.WriteLine("Full response: " & response)
				End If

						Return response
			End Using
			End Using

		Catch ex As AfricasTalkingGatewayException
			Throw ex
		End Try
	End Function
	Private Function sendGetRequest(ByVal urlString_ As String) As String
		Try
			System.Net.ServicePointManager.ServerCertificateValidationCallback = AddressOf RemoteCertificateValidationCallback

			Dim webRequest As HttpWebRequest = CType(System.Net.WebRequest.Create(urlString_), HttpWebRequest)
			webRequest.Method = "GET"
			webRequest.Accept = "application/json"
			webRequest.Headers.Add("apiKey", _apiKey)

			Dim httpResponse As HttpWebResponse = CType(webRequest.GetResponse(), HttpWebResponse)
			responseCode = CInt(httpResponse.StatusCode)
			Dim webpageReader As New StreamReader(httpResponse.GetResponseStream())

			Dim response As String = webpageReader.ReadToEnd()

			If DEBUG Then
				Console.WriteLine("Full response: " & response)
			End If

			Return response


		Catch ex As WebException
			If ex.Response Is Nothing Then
				Throw New AfricasTalkingGatewayException(ex.Message)
			End If

			Using stream = ex.Response.GetResponseStream()
			Using reader = New StreamReader(stream)
				Dim response As String = reader.ReadToEnd()

				If DEBUG Then
					Console.WriteLine("Full response: " & response)
				End If

				Return response
			End Using
			End Using

		Catch ex As AfricasTalkingGatewayException
			Throw ex
		End Try
	End Function
	Public Function PostAsJson(ByVal dataMap As CheckOutData, ByVal url As String) As String
		Dim client = New HttpClient()

		client.DefaultRequestHeaders.Add("apiKey", _apiKey)
		Dim result = client.PostAsJsonAsync(Of CheckOutData)(url, dataMap).Result
		result.EnsureSuccessStatusCode()

		Dim stringResult = result.Content.ReadAsStringAsync().Result
		Return stringResult

	End Function
	Public Class CheckOutData
		Public Property username() As String
		Public Property productName() As String
		Public Property phoneNumber() As String
		Public Property currencyCode() As String
		Public Property amount() As Decimal
		Public Property providerChannel() As String
	End Class
    Public Function initiateMobilePaymentCheckout(ByVal productName_ As String, ByVal phoneNumber_ As String, ByVal currencyCode_ As String, ByVal amount_ As Integer, ByVal providerChannel_ As String) As Object
        Dim CheckOutData = New CheckOutData() With {
            .username = _username,
            .productName = productName_,
            .phoneNumber = phoneNumber_,
            .currencyCode = currencyCode_,
            .amount = amount_,
            .providerChannel = providerChannel_
        }

        Try
            Dim checkOutresponse As String = PostAsJson(CheckOutData, PAYMENTS_URLString)
            Return checkOutresponse
        Catch ex As Exception
            Throw New AfricasTalkingGatewayException(ex)
        End Try
    End Function
    Public Function PostB2BJson(ByVal dataMap As B2BData, ByVal url As String) As String
		Dim client = New HttpClient()

		client.DefaultRequestHeaders.Add("apiKey", _apiKey)
		Dim result = client.PostAsJsonAsync(Of B2BData)(url, dataMap).Result
		result.EnsureSuccessStatusCode()

		Dim stringResult = result.Content.ReadAsStringAsync().Result
		Return stringResult

   End Function
	Public Class B2BData
		Public Property Username() As String
		Public Property ProductName() As String
		Public Property Provider() As String
		Public Property TransferType() As String
		Public Property CurrencyCode() As String
		Public Property Amount() As Decimal
		Public Property DestinationChannel() As String
		Public Property DestinationAccount() As String
	End Class
    Public Function MobileB2B(ByVal productName As String, ByVal provider As String, ByVal transferType As String, ByVal currencyCode As String, ByVal amount As Integer, ByVal destinationChannel As String, ByVal destinationAccount As String) As Object
        Dim b2BData = New B2BData() With {
            .Username = _username,
            .ProductName = productName,
            .Provider = provider,
            .TransferType = transferType,
            .CurrencyCode = currencyCode,
            .Amount = amount,
            .DestinationChannel = destinationChannel,
            .DestinationAccount = destinationAccount
        }

        Try
            Dim b2Bresponse As String = PostB2BJson(b2BData, PAYMENTS_B2B_URLString)
            Return b2Bresponse
        Catch ex As Exception
            Throw New AfricasTalkingGatewayException(ex)
        End Try
    End Function
    'INSTANT VB NOTE: In the following line, Instant VB substituted 'Object' for 'dynamic' - this will work in VB with Option Strict Off:
    Public Function MobilePaymentB2CRequest(ByVal productName As String, ByVal recipients As IList(Of MobilePaymentB2CRecipient)) As Object
        Dim requestBody = New RequestBody With {
            .ProductName = productName,
            .UserName = _username,
            .Recipients = recipients.ToList()
        }

        Console.WriteLine("Raw Request: " & "requestBody")
        Dim response = Post(requestBody, Me.PaymentsB2CUrlString)
		Return response
	End Function
	Public Function Post(ByVal body As RequestBody, ByVal url As String) As DataResult
		Dim httpClient = New HttpClient()
		httpClient.DefaultRequestHeaders.Add("apiKey", _apiKey)
		Dim result = httpClient.PostAsJsonAsync(url, body).Result
		result.EnsureSuccessStatusCode()
		Dim res = result.Content.ReadAsAsync(Of DataResult)()
		Return res.Result

	End Function
	Private Function RemoteCertificateValidationCallback(ByVal sender As Object, ByVal certificate As System.Security.Cryptography.X509Certificates.X509Certificate, ByVal chain As System.Security.Cryptography.X509Certificates.X509Chain, ByVal errors As System.Net.Security.SslPolicyErrors) As Boolean
		Return True
	End Function
	Private ReadOnly Property ApiHost() As String
		Get
			Return (If(String.ReferenceEquals(_environment, "sandbox"), "https://api.sandbox.africastalking.com", "https://api.africastalking.com"))
		End Get
	End Property
	Private ReadOnly Property PaymentHost() As String
		Get
			Return (If(String.ReferenceEquals(_environment, "sandbox"), "https://payments.sandbox.africastalking.com", "https://payments.africastalking.com"))
		End Get

	End Property
	Private ReadOnly Property SMS_URLString() As String
		Get
			Return ApiHost & "/version1/messaging"
		End Get
	End Property
	Private ReadOnly Property VOICE_URLString() As String
		Get
			Return (If(String.ReferenceEquals(_environment, "sandbox"), "https://voice.sandbox.africastalking.com", "https://voice.africastalking.com"))
		End Get
	End Property

	Private ReadOnly Property SUBSCRIPTION_URLString() As String
		Get
			Return ApiHost & "/version1/subscription"
		End Get
	End Property

	Private ReadOnly Property USERDATA_URLString() As String
		Get
			Return ApiHost & "/version1/user"
		End Get
	End Property
	Private ReadOnly Property AIRTIME_URLString() As String
		Get
			Return ApiHost & "/version1/airtime"
		End Get
	End Property
	Private ReadOnly Property PAYMENTS_URLString() As String
		Get
			Return PaymentHost & "/mobile/checkout/request"
		End Get
	End Property
	Private ReadOnly Property PAYMENTS_B2B_URLString() As String
		Get
			Return PaymentHost & "/mobile/b2b/request"
		End Get
	End Property
	Private ReadOnly Property PaymentsB2CUrlString() As String
		Get
			Return PaymentHost & "/mobile/b2c/request"
		End Get
	End Property

End Class
Public Class MobilePaymentB2CRecipient
	<JsonProperty("phoneNumber")>
	Public Property PhoneNumber() As String
	<JsonProperty("currencyCode")>
	Public Property CurrencyCode() As String
	<JsonProperty("amount")>
	Public Property Amount() As Decimal
	Private privateMetadata As Dictionary(Of String, String)
	<JsonProperty("metadata")>
	Public Property Metadata() As Dictionary(Of String, String)
		Get
			Return privateMetadata
		End Get
		Private Set(ByVal value As Dictionary(Of String, String))
			privateMetadata = value
		End Set
	End Property
	Public Sub New(ByVal phoneNumber As String, ByVal currencyCode As String, ByVal amount As Decimal)
		Me.PhoneNumber = phoneNumber
		Me.CurrencyCode = currencyCode
		Me.Amount = amount
		Metadata = New Dictionary(Of String, String)()
	End Sub
	Public Sub AddMetadata(ByVal key As String, ByVal value As String)
		Me.Metadata.Add(key, value)
	End Sub
	Public Function ToJson() As String
		Dim json = JsonConvert.SerializeObject(Me)
		Return json
	End Function
End Class
Public Class RequestBody
	Public Sub New()
		Me.Recipients = New List(Of MobilePaymentB2CRecipient)()
	End Sub
	<JsonProperty("username")>
	Public Property UserName() As String
	<JsonProperty("productName")>
	Public Property ProductName() As String
	<JsonProperty("recipients")>
	Public Property Recipients() As List(Of MobilePaymentB2CRecipient)
	Public Overrides Function ToString() As String
		Dim json = JsonConvert.SerializeObject(Me)
		Return json
	End Function
End Class
Public Class Entry

	<JsonProperty("phoneNumber")>
	Public Property phoneNumber() As String

	<JsonProperty("provider")>
	Public Property provider() As String

	<JsonProperty("providerChannel")>
	Public Property providerChannel() As String

	<JsonProperty("transactionFee")>
	Public Property transactionFee() As String

	<JsonProperty("status")>
	Public Property status() As String

	<JsonProperty("value")>
	Public Property value() As String

	<JsonProperty("transactionId")>
	Public Property transactionId() As String
End Class
Public Class DataResult
	<JsonProperty("numQueued")>
	Public Property numQueued() As Integer

	<JsonProperty("entries")>
	Public Property entries() As IList(Of Entry)

	<JsonProperty("totalValue")>
	Public Property totalValue() As String

	<JsonProperty("totalTransactionFee")>
	Public Property totalTransactionFee() As String

	Public Overrides Function ToString() As String
		Dim result = JsonConvert.SerializeObject(Me)
		Return result
	End Function
End Class


