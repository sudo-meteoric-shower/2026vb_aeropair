Imports System.Reflection
<Assembly: AssemblyTitle("航空合成实验室")>
<Assembly: AssemblyCompany("昕煜 冷")>
<Assembly: AssemblyDescription("航空构型与试验案例的交互式 VB 课程项目")>

Public Module Program
    <STAThread()> Public Sub Main()
        Application.EnableVisualStyles()
        Application.SetCompatibleTextRenderingDefault(False)
        Application.Run(New WelcomeForm())
    End Sub
End Module

Public Class LabWindow
    Inherits Form
    Protected Shared ReadOnly Ink As Color = Color.FromArgb(28, 58, 62)
    Protected Shared ReadOnly Teal As Color = Color.FromArgb(23, 101, 99)
    Protected Shared ReadOnly Paper As Color = Color.FromArgb(249, 248, 243)
    Public Sub New()
        Text = "航空合成实验室"
        StartPosition = FormStartPosition.CenterScreen
        Font = New Font("Microsoft YaHei UI", 10)
        ForeColor = Ink
        BackColor = Paper
        AutoScaleMode = AutoScaleMode.Dpi
        AutoScroll = True
        FormBorderStyle = FormBorderStyle.FixedSingle
        MaximizeBox = False
    End Sub

    Protected Function Caption(words As String, x As Integer, y As Integer, w As Integer, h As Integer,
                               Optional size As Single = 10, Optional bold As Boolean = False) As Label
        Dim label As New Label With {.Text = words, .Font = New Font(Font.FontFamily, size, If(bold, FontStyle.Bold, FontStyle.Regular)),
            .TextAlign = ContentAlignment.MiddleLeft}
        label.SetBounds(x, y, w, h)
        Controls.Add(label)
        Return label
    End Function

    Protected Function ActionButton(words As String, x As Integer, y As Integer, w As Integer, h As Integer) As Button
        Dim btn As New Button With {.Text = words, .BackColor = Color.White, .ForeColor = Ink,
            .FlatStyle = FlatStyle.Flat, .Cursor = Cursors.Hand, .UseVisualStyleBackColor = False}
        btn.FlatAppearance.BorderColor = Color.FromArgb(179, 203, 198)
        btn.SetBounds(x, y, w, h)
        Controls.Add(btn)
        Return btn
    End Function
End Class

Public Class WelcomeForm
    Inherits LabWindow
    Public Sub New()
        ClientSize = New Size(812, 386)
        Caption("航空合成实验室", 32, 62, 333, 62, 28, True)
        Caption("从两件部件开始，发现真实的飞行设计。", 34, 135, 315, 61, 14)
        Dim start As Button = ActionButton("进入实验室   →", 35, 231, 176, 46)
        start.BackColor = Teal
        start.ForeColor = Color.White
        Dim art As New Diagram With {.AccessibleName = "浮筒水上飞机侧视示意图"}
        art.SetBounds(365, 34, 415, 277)
        Controls.Add(art)
        Caption("浮筒水上飞机  /  构型示意", 365, 316, 415, 34)
        AddHandler start.Click, AddressOf EnterLab
        AcceptButton = start
    End Sub

    Private Sub EnterLab(sender As Object, e As EventArgs)
        Hide()
        Try
            Using lab As New LabForm()
                lab.ShowDialog()
            End Using
        Finally
            Close()
        End Try
    End Sub
End Class
