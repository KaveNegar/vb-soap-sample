Imports System
Imports System.Collections.Generic
Imports System.Collections.Specialized
Imports System.Linq
Imports System.Net
Imports System.Text
Imports Jayrock.Json

Module Program
	Public Const ApiKey As String = "YOUR-APIKEY"
	Sub Main()
		Dim msg As New Models.Message()
		msg.Sender = "10006707323323"
		msg.Receptor = "YOUR-NUMBER"
		msg.Text = "خدمات پیام کوتاه کاوه نگار"
		Dim sendResult() As Long = Send(msg)
		If sendResult Is Nothing Then Return
		For Each f In sendResult
			' Save f in somewhere to fetch status later
			Console.WriteLine("SmsRefrenceID:" & f.ToString())
		Next
		Dim smsstatus = Status(sendResult)
		For Each st In smsstatus
			Select Case st
				Case 1
					Console.WriteLine("Queued to send")
				Case 2
					Console.WriteLine("Scheduled")
				Case 4
					Console.WriteLine("Sent to Operator")
				Case 5
					Console.WriteLine("Sent to Operator")
				Case 10
					Console.WriteLine("Sent to Receptor (Delivered)")
				Case 11
					Console.WriteLine("Undelivered")
				Case 13
					Console.WriteLine("Cancelled")
				Case 14
					Console.WriteLine("Receptor number has blocked..")
				Case 100
					Console.WriteLine("Refrence ID not found")
			End Select
		Next
		Console.WriteLine("Press Enter to close")
		Console.ReadKey()
	End Sub
	Public Function Send(message As Models.Message) As Long()
		Try
			Dim kv As New Kavenegar.v1
			Dim refstatuscode As Long
			Dim refstatusmsg As String = String.Empty
			Dim refSmsId = kv.SendSimpleByApikey(ApiKey, message.Sender, message.Text, message.Receptor.ToString().Split(","), 0, 1, refstatuscode, refstatusmsg)
			If refstatuscode <> 200 Then
				Console.WriteLine(String.Format("Returned [{0}] Message:[{1}]", refstatuscode, refstatusmsg))
				Return Nothing
			End If
			Return refSmsId
		Catch ex As Exception
			Console.WriteLine(ex.Message)
			Return Nothing
		End Try
	End Function
	Public Function Status(ids As Long()) As Integer()
		Try
			Dim kv As New Kavenegar.v1
			Dim refstatuscode As Long
			Dim refstatusmsg As String = String.Empty
			Return kv.GetStatusByApikey(ApiKey, ids, refstatuscode, refstatusmsg)
		Catch ex As Exception
			Return Nothing
		End Try
	End Function


End Module