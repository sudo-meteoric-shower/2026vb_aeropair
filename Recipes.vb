Imports System.IO
Imports System.Reflection

Public Class Recipe
    Public First As String
    Public Second As String
    Public Title As String
    Public Detail As String
    Public Kind As String
    Public Illustration As String
    Public Photo As String
    Public PhotoCredit As String
    Public PhotoSource As String

    Public Function Fits(a As String, b As String) As Boolean
        Return (a = First AndAlso b = Second) OrElse (a = Second AndAlso b = First)
    End Function

    Public ReadOnly Property Category As String
        Get
            Select Case Kind
                Case "float", "amph", "helios", "stemme", "longez", "twinotter", "concorde", "ercoupe"
                    Return "真实案例"
                Case "试验"
                    Return "试验设想"
                Case Else
                    Return "设计推想"
            End Select
        End Get
    End Property
End Class

Public Class PartCard
    Public Id As String
    Public Section As String
    Public Name As String
    Public Hint As String
End Class

Public Module Catalog
    Public ReadOnly Parts As New List(Of PartCard)
    Private ReadOnly PartsById As New Dictionary(Of String, PartCard)(StringComparer.Ordinal)
    Public ReadOnly Rules As New List(Of Recipe)

    Public Sub LoadCatalog()
        If Rules.Count > 0 Then Return
        Using reader As New StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("parts.txt"), System.Text.Encoding.UTF8)
            While Not reader.EndOfStream
                Dim fields As String() = reader.ReadLine().Split("|"c)
                Dim part As New PartCard With {.Id = fields(0), .Section = fields(1), .Name = fields(2), .Hint = fields(3)}
                PartsById.Add(part.Id, part)
                Parts.Add(part)
            End While
        End Using
        Using reader As New StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("catalog.txt"), System.Text.Encoding.UTF8)
            While Not reader.EndOfStream
                Dim columns As String() = reader.ReadLine().Split("|"c)
                Dim pair As String() = columns(0).Split(" "c)
                Rules.Add(New Recipe With {
                    .First = pair(0), .Second = pair(1), .Title = columns(1), .Detail = columns(2),
                    .Kind = If(columns.Length > 3, columns(3), ""),
                    .Illustration = If(columns.Length > 4, columns(4), ""),
                    .Photo = If(columns.Length > 5, columns(5), ""),
                    .PhotoCredit = If(columns.Length > 6, columns(6), ""),
                    .PhotoSource = If(columns.Length > 7, columns(7), "")
                })
            End While
        End Using
    End Sub

    Public Function PartName(id As String) As String
        Dim part As PartCard = Nothing
        If PartsById.TryGetValue(id, part) Then Return part.Name
        Return id
    End Function

    Public Function Matches(a As String, b As String) As List(Of Recipe)
        Return Rules.FindAll(Function(item) item.Fits(a, b))
    End Function
End Module
