Imports System.IO
Imports System.Reflection

Public Class Recipe
    Public First As String
    Public Second As String
    Public Title As String
    Public Detail As String
    Public Kind As String

    Public Function Fits(a As String, b As String) As Boolean
        Return (a = First AndAlso b = Second) OrElse (a = Second AndAlso b = First)
    End Function

    Public ReadOnly Property Category As String
        Get
            Select Case Kind
                Case "float", "amph", "helios", "stemme", "longez"
                    Return "真实案例"
                Case "试验"
                    Return "试验设想"
                Case Else
                    Return "设计推想"
            End Select
        End Get
    End Property
End Class

Public Module Catalog
    Public ReadOnly Parts As String() = {"轻型机身", "滑翔机身", "细长的机翼", "向后斜的机翼", "厚机翼", "薄机翼", "可折叠接头", "会弯曲的翼尖", "电动机", "螺旋桨", "太阳能电池片", "蓄电池", "浮筒", "可收起的轮子", "滑橇", "低速风洞"}
    Public ReadOnly Hints As String() = {"轻型飞机的机身", "滑翔机细长的机身", "从一侧翼尖到另一侧距离长", "从上方看，机翼向后斜", "机翼从侧面看比较厚", "机翼从侧面看比较薄", "能让两段部件折起来", "受力时能弯曲的机翼末端", "把电能变成转动", "旋转时推动空气", "把阳光转换为电能", "储存电能", "让飞机浮在水面上", "起飞后能收起来的机轮", "能在雪面上滑行的支撑件", "用可控气流观察模型"}
    Public ReadOnly Rules As New List(Of Recipe)

    Public Sub LoadCatalog()
        If Rules.Count > 0 Then Return
        Using reader As New StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("catalog.txt"), System.Text.Encoding.UTF8)
            While Not reader.EndOfStream
                Dim columns As String() = reader.ReadLine().Split("|"c)
                Dim pair As String() = columns(0).Split(" "c)
                Rules.Add(New Recipe With {.First = pair(0), .Second = pair(1), .Title = columns(1), .Detail = columns(2), .Kind = If(columns.Length > 3, columns(columns.Length - 1), "")})
            End While
        End Using
    End Sub

    Public Function PartName(id As String) As String
        If id.Length = 1 Then Return Parts(Asc(id) - Asc("A"))
        Return id
    End Function

    Public Function Matches(a As String, b As String) As List(Of Recipe)
        Return Rules.FindAll(Function(item) item.Fits(a, b))
    End Function
End Module
