Imports System.Drawing.Drawing2D

'原理示意图。坐标采用统一的画布尺度，在不同大小的图片框内等比绘制。
Public Class Diagram
    Inherits PictureBox
    Public Scene As String = "浮筒水上飞机"
    Public LeftId As String = ""
    Public RightId As String = ""
    Public Folded As Boolean

    '在同一尺度绘出两件部件，便于从不同组合中认出同一件物品。
    Private Shared Sub DrawPart(g As Graphics, id As String, x As Integer, pen As Pen, fill As Brush)
        If id = "" Then Return
        Dim state As GraphicsState = g.Save()
        g.TranslateTransform(x, 24)
        Select Case id
            Case "A", "B"
                Dim tall As Integer = If(id = "A", 28, 15)
                g.FillEllipse(Brushes.WhiteSmoke, 0, 49, 130, tall)
                g.DrawEllipse(pen, 0, 49, 130, tall)
                Shape(g, pen, fill, 2, 52, 0, 27, 10, 32, 27, 51)
                g.FillEllipse(fill, 78, 43, 29, 12)
            Case "C", "D"
                Dim tip As Integer = If(id = "C", 53, 83)
                Shape(g, pen, Brushes.WhiteSmoke, 0, tip, 55, 38, 75, 38, 130, tip, 128, tip + 5, 70, 56, 60, 56, 2, tip + 5)
            Case "E", "F"
                Dim upper As Integer = If(id = "E", 17, 46)
                Using profile As New GraphicsPath()
                    profile.AddBezier(4, 66, 4, upper, 61, upper, 130, 64)
                    profile.AddBezier(130, 64, 56, 61, 6, 89, 4, 66)
                    g.FillPath(fill, profile)
                    g.DrawPath(pen, profile)
                End Using
            Case "G", "H"
                If id = "G" Then
                    Shape(g, pen, Brushes.WhiteSmoke, 0, 75, 54, 60, 61, 74, 5, 86)
                    Shape(g, pen, fill, 55, 62, 102, 22, 110, 26, 63, 76)
                    g.DrawEllipse(pen, 52, 60, 13, 13)
                Else
                    g.DrawBezier(pen, 0, 76, 63, 74, 93, 63, 126, 20)
                    g.DrawBezier(pen, 0, 82, 63, 83, 97, 61, 126, 20)
                End If
            Case "I"
                g.FillRectangle(fill, 27, 39, 65, 43)
                g.DrawRectangle(pen, 27, 39, 65, 43)
                g.DrawLine(pen, 93, 60, 118, 60)
                For i As Integer = 37 To 78 Step 10
                    g.DrawLine(pen, i, 43, i, 78)
                Next
            Case "J"
                Using blade As New SolidBrush(Color.DarkSlateGray)
                    g.TranslateTransform(65, 60)
                    For i As Integer = 0 To 2
                        g.FillEllipse(blade, -5, -48, 13, 49)
                        g.RotateTransform(120)
                    Next
                    g.FillEllipse(fill, -7, -7, 14, 14)
                End Using
            Case "K", "L"
                g.FillRectangle(If(id = "K", Brushes.CadetBlue, fill), 23, 33, 83, 49)
                g.DrawRectangle(pen, 23, 33, 83, 49)
                If id = "K" Then
                    For i As Integer = 40 To 100 Step 17
                        g.DrawLine(Pens.White, i, 33, i, 82)
                    Next
                    g.DrawLine(Pens.White, 23, 57, 106, 57)
                Else
                    g.DrawRectangle(pen, 106, 48, 7, 19)
                    g.DrawLine(pen, 74, 57, 90, 57)
                    g.DrawLine(pen, 82, 49, 82, 65)
                End If
            Case "M", "O", "浮筒水上飞机"
                If id = "O" Then
                    g.DrawBezier(pen, 3, 68, 14, 91, 81, 81, 127, 77)
                    g.DrawLine(pen, 49, 78, 60, 43)
                Else
                    Shape(g, pen, fill, 2, 62, 105, 62, 129, 50, 118, 70, 68, 80, 65, 75, 19, 78)
                End If
            Case "N"
                g.DrawLine(pen, 81, 25, 60, 65)
                g.FillEllipse(Brushes.DarkSlateGray, 39, 57, 37, 37)
                g.FillEllipse(fill, 51, 69, 13, 13)
            Case "P"
                Shape(g, pen, Brushes.WhiteSmoke, 0, 26, 31, 42, 96, 42, 128, 26, 128, 92, 96, 77, 31, 77, 0, 92)
                For i As Integer = 52 To 68 Step 8
                    g.DrawLine(Pens.CadetBlue, 12, i, 115, i)
                Next
            Case "Q"
                Shape(g, pen, Brushes.WhiteSmoke, 0, 78, 65, 58, 130, 78, 129, 84, 65, 72, 1, 84)
                g.FillEllipse(Brushes.WhiteSmoke, 58, 22, 14, 73)
                g.DrawEllipse(pen, 58, 22, 14, 73)
                Shape(g, pen, fill, 35, 43, 65, 36, 95, 43, 94, 48, 65, 43, 36, 48)
            Case "R"
                Shape(g, pen, Brushes.WhiteSmoke, 7, 68, 118, 61, 125, 69, 13, 85)
                Shape(g, pen, fill, 118, 62, 108, 27, 115, 26, 127, 69)
            Case "S"
                Shape(g, pen, Brushes.WhiteSmoke, 4, 59, 19, 45, 58, 47, 124, 61, 106, 67, 23, 68)
                Shape(g, pen, fill, 93, 65, 126, 83, 120, 88, 83, 67)
            Case "T"
                Shape(g, pen, Brushes.WhiteSmoke, 2, 39, 43, 29, 125, 40, 48, 44, 6, 47)
                g.DrawLine(pen, 57, 44, 57, 61)
                g.FillEllipse(fill, 23, 60, 84, 24)
                g.DrawEllipse(pen, 23, 60, 84, 24)
            Case "U"
                Shape(g, pen, Brushes.WhiteSmoke, 22, 27, 106, 27, 112, 32, 28, 38)
                g.DrawLine(pen, 43, 36, 64, 73)
                g.DrawLine(pen, 82, 33, 64, 73)
                g.FillEllipse(Brushes.DarkSlateGray, 48, 60, 32, 32)
                g.FillEllipse(fill, 58, 70, 12, 12)
            Case "V"
                Shape(g, pen, Brushes.WhiteSmoke, 15, 44, 95, 46, 108, 55, 108, 72, 95, 80, 15, 82)
                g.FillEllipse(fill, 8, 44, 19, 38)
                g.DrawEllipse(pen, 8, 44, 19, 38)
                For y As Integer = 58 To 70 Step 6
                    g.DrawLine(pen, 108, y, 129, y)
                Next
            Case "W"
                Shape(g, pen, Brushes.WhiteSmoke, 65, 27, 1, 86, 129, 86)
                g.DrawLine(pen, 65, 18, 65, 98)
            Case "X"
                Shape(g, pen, Brushes.WhiteSmoke, 4, 60, 126, 60, 117, 75, 13, 75)
                Shape(g, pen, fill, 9, 69, 14, 33, 25, 29, 25, 70)
                Shape(g, pen, fill, 104, 70, 105, 29, 116, 33, 120, 69)
            Case "Y"
                Shape(g, pen, fill, 12, 70, 112, 44, 120, 55, 20, 80)
                g.DrawLine(pen, 10, 63, 122, 63)
                g.DrawArc(pen, 51, 43, 29, 36, 210, 250)
                Shape(g, pen, Brushes.DarkSlateGray, 65, 76, 72, 82, 75, 73)
        End Select
        g.Restore(state)
        Using labelFont As New Font("Microsoft YaHei UI", 12, FontStyle.Regular, GraphicsUnit.Pixel)
            g.DrawString(PartName(id), labelFont, Brushes.DarkSlateGray, CSng(x + 12), 132.0F)
        End Using
    End Sub

    Public Sub New()
        SetStyle(ControlStyles.UserPaint Or ControlStyles.AllPaintingInWmPaint Or
                 ControlStyles.OptimizedDoubleBuffer Or ControlStyles.ResizeRedraw, True)
        BackColor = Color.FromArgb(222, 239, 235)
    End Sub

    Private Shared Sub Shape(g As Graphics, outline As Pen, fill As Brush, ParamArray xy() As Integer)
        Dim points(xy.Length \ 2 - 1) As Point
        For i As Integer = 0 To points.Length - 1
            points(i) = New Point(xy(2 * i), xy(2 * i + 1))
        Next
        g.FillPolygon(fill, points)
        g.DrawPolygon(outline, points)
    End Sub

    '构型以俯视图绘制。选中的部件决定翼形、动力、起落装置和试验环境。
    Private Sub DrawLayout(g As Graphics, edge As Pen, accent As Brush, glass As Brush)
        Dim parts As String = LeftId & RightId
        Dim delta As Boolean = parts.Contains("W")
        Dim span As Integer = If(parts.Contains("C") OrElse parts.Contains("B"), 164, 125)
        Dim tip As Integer = If(delta, 105, If(parts.Contains("D"), 118, 82))
        Dim chord As Integer = If(delta, 57, If(parts.Contains("E"), 27, If(parts.Contains("F"), 12, 20)))
        If parts.Contains("P") Then
            g.DrawRectangle(Pens.CadetBlue, 24, 14, 392, 151)
            For x As Integer = 46 To 398 Step 32
                g.DrawLine(Pens.CadetBlue, x, 23, x, 143)
                g.DrawLine(Pens.CadetBlue, x, 143, x - 3, 136)
            Next
        End If
        For Each side As Integer In New Integer() {-1, 1}
            Dim x As Integer = 220 + side * span
            Shape(g, edge, Brushes.WhiteSmoke, 220, 62, x, tip, x, tip + 7, 220, 62 + chord)
            If parts.Contains("G") Then
                Dim joint As Integer = 220 + side * span * 2 \ 3
                Dim y As Integer = 62 + (tip - 62) * 2 \ 3
                g.DrawLine(edge, joint, y, joint, y + 13)
                g.FillEllipse(accent, joint - 3, y + 3, 6, 6)
            End If
            If parts.Contains("H") Then g.DrawBezier(edge, x - side * 22, tip, x, tip, x, tip - 10, x + side * 8, tip - 22)
            If parts.Contains("R") Then Shape(g, edge, accent, x, tip, x - side * 8, tip - 17, x - side * 3, tip - 19, x + side * 4, tip + 7)
            If parts.Contains("S") Then
                Dim rootEdge As Integer = 62 + chord
                Dim edgeSlope As Integer = tip + 7 - rootEdge
                Dim inner As Integer = rootEdge + edgeSlope * 19 \ span
                Dim outer As Integer = rootEdge + edgeSlope * 76 \ span
                Shape(g, edge, accent, 220 + side * 19, inner, 220 + side * 76, outer, 220 + side * 76, outer + 7, 220 + side * 19, inner + 7)
            End If
            If parts.Contains("T") Then
                Dim podX As Integer = 220 + side * 49
                g.FillEllipse(accent, podX - 5, 70, 10, 31)
                g.DrawEllipse(edge, podX - 5, 70, 10, 31)
            End If
            If parts.Contains("K") Then
                For j As Integer = 1 To 4
                    Dim cx As Integer = 220 + side * (20 + j * 12)
                    Dim cy As Integer = 66 + (tip - 62) * Math.Abs(cx - 220) \ span
                    g.FillRectangle(Brushes.CadetBlue, cx - 5, cy, 10, 7)
                Next
            End If
            If parts.Contains("M") OrElse parts.Contains("O") Then
                Dim gear As Integer = 220 + side * 32
                g.DrawLine(edge, 220, 90, gear, 100)
                If parts.Contains("M") Then
                    g.FillEllipse(glass, gear - 7, 71, 14, 65)
                    g.DrawEllipse(edge, gear - 7, 71, 14, 65)
                End If
                If parts.Contains("O") Then g.DrawBezier(edge, gear + side * 7, 83, gear + side * 13, 72, gear + side * 13, 105, gear + side * 13, 132)
            End If
            If parts.Contains("N") OrElse parts.Contains("U") Then
                g.DrawLine(edge, 220, 90, 220 + side * 30, 97)
                g.FillEllipse(Brushes.DarkSlateGray, 216 + side * 30, 89, 8, 16)
            End If
        Next
        Shape(g, edge, Brushes.WhiteSmoke, 220, 126, 258, 141, 258, 147, 220, 140, 182, 147, 182, 141)
        If parts.Contains("X") Then
            Shape(g, edge, accent, 182, 144, 183, 125, 190, 121, 190, 144)
            Shape(g, edge, accent, 258, 144, 257, 125, 250, 121, 250, 144)
        End If
        Shape(g, edge, Brushes.WhiteSmoke, 220, 24, 231, 41, 231, 84, 223, 142, 217, 142, 209, 84, 209, 41)
        g.FillEllipse(glass, 213, 41, 14, 24)
        If parts.Contains("Q") Then Shape(g, edge, accent, 168, 47, 220, 40, 272, 47, 272, 52, 220, 49, 168, 52)
        If parts.Contains("L") Then
            g.FillRectangle(accent, 213, 71, 14, 22)
            g.DrawRectangle(edge, 213, 71, 14, 22)
        End If
        If parts.Contains("I") Then g.FillRectangle(accent, 213, 27, 14, 13)
        If parts.Contains("J") OrElse parts.Contains("Y") Then Shape(g, edge, accent, 183, 27, 217, 20, 258, 18, 256, 23, 224, 25, 185, 31)
        If parts.Contains("V") Then
            For Each x As Integer In New Integer() {188, 239}
                g.FillRectangle(glass, x, 82, 13, 38)
                g.DrawRectangle(edge, x, 82, 13, 38)
                g.DrawLine(edge, x + 4, 122, x + 4, 131)
                g.DrawLine(edge, x + 9, 122, x + 9, 131)
            Next
        End If
    End Sub

    Private Sub DrawJetStudy(g As Graphics, edge As Pen, accent As Brush, glass As Brush)
        Using labelFont As New Font("Microsoft YaHei UI", 15, GraphicsUnit.Pixel),
              flow As New Pen(Color.CadetBlue, 2),
              arrow As New AdjustableArrowCap(4, 5),
              hidden As New Pen(Color.CadetBlue, 2)
            flow.CustomEndCap = arrow
            hidden.DashStyle = DashStyle.Dash
            Select Case Scene
                Case "喷气布局"
                    Shape(g, edge, Brushes.WhiteSmoke, 220, 58, 50, 99, 56, 113, 220, 91, 384, 113, 390, 99)
                    Shape(g, edge, Brushes.WhiteSmoke, 220, 123, 155, 142, 160, 151, 220, 143, 280, 151, 285, 142)
                    g.FillEllipse(Brushes.WhiteSmoke, 208, 20, 24, 130)
                    g.DrawEllipse(edge, 208, 20, 24, 130)
                    For Each x As Integer In New Integer() {104, 160, 280, 336}
                        g.FillEllipse(accent, x - 10, 76, 20, 46)
                        g.DrawEllipse(edge, x - 10, 76, 20, 46)
                    Next
                    g.DrawString("内侧发动机", labelFont, Brushes.DarkSlateGray, 123, 139)
                    g.DrawString("外侧发动机", labelFont, Brushes.DarkSlateGray, 302, 139)
                    g.DrawLine(flow, 154, 137, 160, 117)
                    g.DrawLine(flow, 334, 137, 336, 117)
                Case "翼下吊架"
                    Using section As New GraphicsPath()
                        section.AddBezier(56, 64, 51, 24, 151, 24, 383, 62)
                        section.AddBezier(383, 62, 256, 64, 70, 92, 56, 64)
                        g.FillPath(Brushes.WhiteSmoke, section)
                        g.DrawPath(edge, section)
                    End Using
                    g.FillRectangle(glass, 151, 51, 13, 22)
                    g.DrawRectangle(edge, 151, 51, 13, 22)
                    Shape(g, edge, accent, 155, 72, 168, 70, 196, 104, 152, 104)
                    g.FillEllipse(Brushes.WhiteSmoke, 92, 98, 172, 54)
                    g.DrawEllipse(edge, 92, 98, 172, 54)
                    g.DrawEllipse(edge, 96, 102, 26, 46)
                    g.DrawLine(flow, 298, 91, 182, 89)
                    g.DrawString("机翼剖面", labelFont, Brushes.DarkSlateGray, 100, 10)
                    g.DrawString("吊架", labelFont, Brushes.DarkSlateGray, 304, 80)
                    g.DrawString("发动机短舱", labelFont, Brushes.DarkSlateGray, 139, 115)
                Case "两种动力"
                    g.DrawLine(hidden, 220, 29, 220, 155)
                    g.FillRectangle(glass, 82, 67, 68, 41)
                    g.DrawRectangle(edge, 82, 67, 68, 41)
                    g.DrawLine(edge, 65, 88, 82, 88)
                    Shape(g, edge, accent, 64, 88, 57, 49, 65, 45, 72, 87, 78, 125, 70, 129)
                    For y As Integer = 71 To 105 Step 17
                        g.DrawLine(flow, 157, y, 193, y)
                        g.DrawLine(flow, 364, y, 410, y)
                    Next
                    Shape(g, edge, Brushes.WhiteSmoke, 259, 59, 351, 70, 363, 77, 363, 101, 351, 108, 259, 118)
                    g.DrawEllipse(edge, 250, 59, 18, 59)
                    g.DrawString("电动螺旋桨", labelFont, Brushes.DarkSlateGray, 73, 138)
                    g.DrawString("喷气发动机", labelFont, Brushes.DarkSlateGray, 267, 138)
                Case "主轮轨迹"
                    Shape(g, edge, Brushes.WhiteSmoke, 37, 65, 220, 75, 403, 65, 403, 76, 220, 88, 37, 76)
                    g.FillEllipse(Brushes.WhiteSmoke, 194, 44, 52, 56)
                    g.DrawEllipse(edge, 194, 44, 52, 56)
                    For Each side As Integer In New Integer() {-1, 1}
                        Dim engine As Integer = 220 + side * 128
                        g.DrawLine(edge, engine, 72, engine, 89)
                        g.FillEllipse(glass, engine - 19, 83, 38, 38)
                        g.DrawEllipse(edge, engine - 19, 83, 38, 38)
                        g.DrawEllipse(edge, engine - 11, 91, 22, 22)
                        Dim wheel As Integer = 220 + side * 56
                        Dim stowed As Integer = 220 + side * 32
                        g.DrawLine(edge, wheel, 79, wheel, 139)
                        g.FillEllipse(accent, wheel - 8, 128, 16, 27)
                        g.DrawEllipse(edge, wheel - 8, 128, 16, 27)
                        g.DrawEllipse(hidden, stowed - 12, 81, 24, 11)
                        g.DrawArc(flow, Math.Min(wheel, stowed) - 17, 85, 47, 63, If(side < 0, 113, 37), If(side < 0, -105, -105))
                    Next
                    g.DrawString("发动机", labelFont, Brushes.DarkSlateGray, 66, 130)
                    g.DrawString("主轮", labelFont, Brushes.DarkSlateGray, 203, 142)
                    g.DrawString("轮舱", labelFont, Brushes.DarkSlateGray, 205, 18)
                Case "短舱测力"
                    g.DrawRectangle(edge, 25, 24, 390, 132)
                    For y As Integer = 46 To 132 Step 22
                        g.DrawLine(flow, 41, y, 127, y)
                        g.DrawLine(flow, 315, y, 398, y)
                    Next
                    g.FillEllipse(Brushes.WhiteSmoke, 141, 55, 155, 61)
                    g.DrawEllipse(edge, 141, 55, 155, 61)
                    g.DrawEllipse(edge, 145, 61, 24, 49)
                    Shape(g, edge, accent, 211, 154, 213, 116, 228, 110, 228, 154)
                    g.DrawString("短舱模型", labelFont, Brushes.DarkSlateGray, 180, 75)
                Case "机腹吊舱"
                    Shape(g, edge, Brushes.WhiteSmoke, 43, 84, 75, 65, 285, 65, 327, 58, 358, 44, 369, 48, 344, 78, 308, 91, 72, 93)
                    Shape(g, edge, Brushes.WhiteSmoke, 318, 62, 339, 25, 351, 25, 349, 60)
                    g.FillEllipse(glass, 251, 57, 64, 23)
                    g.DrawEllipse(edge, 251, 57, 64, 23)
                    Shape(g, edge, accent, 146, 92, 229, 92, 235, 105, 223, 116, 156, 116, 141, 107)
                    g.DrawLine(flow, 282, 36, 283, 54)
                    g.DrawLine(flow, 184, 142, 184, 118)
                    g.DrawString("尾部发动机", labelFont, Brushes.DarkSlateGray, 244, 16)
                    g.DrawString("机腹仪器舱", labelFont, Brushes.DarkSlateGray, 149, 147)
                Case "双尾翼喷气"
                    Shape(g, edge, Brushes.WhiteSmoke, 220, 32, 239, 44, 390, 112, 377, 126, 290, 112, 267, 137, 173, 137, 150, 112, 63, 126, 50, 112, 201, 44)
                    For Each x As Integer In New Integer() {199, 225}
                        g.FillEllipse(glass, x, 73, 16, 59)
                        g.DrawEllipse(edge, x, 73, 16, 59)
                        g.DrawLine(flow, x + 8, 133, x + 8, 156)
                    Next
                    Shape(g, edge, accent, 174, 90, 188, 100, 188, 135, 170, 131)
                    Shape(g, edge, accent, 266, 90, 252, 100, 252, 135, 270, 131)
                    g.DrawString("尾翼", labelFont, Brushes.DarkSlateGray, 109, 142)
                    g.DrawLine(flow, 146, 149, 174, 128)
                    g.DrawString("发动机", labelFont, Brushes.DarkSlateGray, 271, 52)
                    g.DrawLine(flow, 270, 67, 234, 83)
            End Select
        End Using
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)
        Dim g As Graphics = e.Graphics
        g.Clear(BackColor)
        g.SmoothingMode = SmoothingMode.AntiAlias
        Using grid As New Pen(Color.FromArgb(198, 222, 216))
            For i As Integer = 0 To Width Step 24
                g.DrawLine(grid, i, 0, i, Height)
            Next
            For i As Integer = 0 To Height Step 24
                g.DrawLine(grid, 0, i, Width, i)
            Next
        End Using
        Dim scale As Single = Math.Min(Width / 440.0F, Height / 180.0F)
        g.TranslateTransform((Width - 440 * scale) / 2, (Height - 180 * scale) / 2)
        g.ScaleTransform(scale, scale)
        Using edge As New Pen(Color.FromArgb(21, 65, 70), 2.2F),
              accent As New SolidBrush(Color.FromArgb(237, 154, 74)),
              glass As New SolidBrush(Color.FromArgb(152, 198, 194))
            If Scene = "部件" Then
                DrawPart(g, LeftId, If(RightId = "", 155, 38), edge, accent)
                If RightId <> "" Then
                    g.DrawLine(edge, 211, 84, 229, 84)
                    g.DrawLine(edge, 220, 75, 220, 93)
                    DrawPart(g, RightId, 273, edge, accent)
                End If
            ElseIf Scene = "喷气布局" OrElse Scene = "翼下吊架" OrElse Scene = "两种动力" OrElse
                   Scene = "主轮轨迹" OrElse Scene = "短舱测力" OrElse Scene = "机腹吊舱" OrElse Scene = "双尾翼喷气" Then
                DrawJetStudy(g, edge, accent, glass)
            ElseIf Scene = "三角翼客机" Then
                For Each side As Integer In New Integer() {-1, 1}
                    Using wing As New GraphicsPath()
                        wing.AddBezier(220, 43, 220 + side * 16, 70, 220 + side * 53, 94, 220 + side * 98, 132)
                        wing.AddLine(220 + side * 98, 132, 220, 119)
                        wing.CloseFigure()
                        g.FillPath(Brushes.WhiteSmoke, wing)
                        g.DrawPath(edge, wing)
                    End Using
                    For j As Integer = 0 To 1
                        Dim x As Integer = 220 + side * (24 + j * 10)
                        g.FillRectangle(accent, x - 4, 101, 8, 31)
                        g.DrawRectangle(edge, x - 4, 101, 8, 31)
                    Next
                Next
                Shape(g, edge, Brushes.WhiteSmoke, 220, 10, 228, 43, 229, 117, 220, 165, 211, 117, 212, 43)
                g.FillEllipse(glass, 215, 30, 10, 10)
            ElseIf Scene = "双垂尾轻型机" Then
                Shape(g, edge, Brushes.WhiteSmoke, 105, 80, 220, 67, 335, 80, 335, 93, 220, 89, 105, 93)
                Shape(g, edge, Brushes.WhiteSmoke, 171, 128, 220, 117, 269, 128, 269, 138, 220, 135, 171, 138)
                Shape(g, edge, accent, 171, 135, 172, 107, 181, 103, 181, 136)
                Shape(g, edge, accent, 269, 135, 268, 107, 259, 103, 259, 136)
                Shape(g, edge, Brushes.WhiteSmoke, 220, 35, 232, 49, 230, 92, 222, 139, 218, 139, 210, 92, 208, 49)
                g.FillEllipse(glass, 213, 50, 14, 23)
                Shape(g, edge, accent, 195, 38, 219, 32, 245, 28, 245, 33, 221, 37, 195, 43)
                For Each x As Integer In New Integer() {201, 232}
                    g.FillEllipse(Brushes.DarkSlateGray, x, 88, 7, 13)
                Next
                g.FillEllipse(Brushes.DarkSlateGray, 218, 42, 4, 8)
            ElseIf Scene = "后掠推式" Then
                '机鼻朝上，前翼、后掠主翼和尾推桨按实际相对位置布置。
                Shape(g, edge, Brushes.WhiteSmoke, 220, 75, 379, 124, 380, 136, 227, 107, 213, 107, 60, 136, 61, 124)
                Shape(g, edge, Brushes.WhiteSmoke, 162, 47, 220, 41, 278, 47, 278, 53, 220, 50, 162, 53)
                Shape(g, edge, Brushes.WhiteSmoke, 220, 14, 229, 38, 234, 85, 228, 123, 212, 123, 206, 85, 211, 38)
                g.FillEllipse(glass, 212, 50, 16, 35)
                g.DrawLine(edge, 220, 123, 220, 132)
                Shape(g, edge, accent, 183, 136, 215, 129, 259, 127, 256, 132, 223, 135, 184, 140)
                g.DrawLine(edge, 61, 124, 56, 108)
                g.DrawLine(edge, 379, 124, 384, 108)
            ElseIf Scene = "构型" Then
                DrawLayout(g, edge, accent, glass)
            ElseIf Scene = "翼内电池舱" OrElse Scene = "翼内轮舱" Then
                Using section As New GraphicsPath()
                    section.AddBezier(43, 96, 43, 29, 171, 34, 397, 102)
                    section.AddBezier(397, 102, 229, 89, 66, 151, 43, 96)
                    g.FillPath(Brushes.WhiteSmoke, section)
                    g.DrawPath(edge, section)
                End Using
                If Scene = "翼内电池舱" Then
                    For i As Integer = 85 To 163 Step 26
                        g.FillRectangle(accent, i, 71, 22, 28)
                        g.DrawRectangle(edge, i, 71, 22, 28)
                    Next
                Else
                    g.FillEllipse(Brushes.DarkSlateGray, 115, 61, 42, 42)
                    g.FillEllipse(glass, 128, 74, 16, 16)
                    g.DrawLine(edge, 136, 82, 186, 68)
                End If
            ElseIf Scene = "电动螺旋桨" Then
                g.FillRectangle(glass, 140, 71, 110, 50)
                g.DrawRectangle(edge, 140, 71, 110, 50)
                g.DrawLine(edge, 251, 96, 284, 96)
                Shape(g, edge, accent, 280, 96, 267, 36, 281, 45, 287, 93, 299, 156, 285, 148)
                g.FillEllipse(Brushes.DarkSlateGray, 277, 89, 14, 14)
            ElseIf Scene = "白天充电的机载电源" Then
                DrawPart(g, "K", 50, edge, accent)
                DrawPart(g, "L", 270, edge, accent)
                g.DrawLine(edge, 165, 81, 272, 81)
                Shape(g, edge, accent, 262, 76, 273, 81, 262, 86)
            ElseIf Scene = "太阳能飞翼" Then
                Shape(g, edge, Brushes.CadetBlue, 12, 75, 429, 75, 419, 101, 22, 101)
                For i As Integer = 24 To 412 Step 13
                    g.DrawLine(Pens.WhiteSmoke, i, 76, i, 99)
                Next
                For i As Integer = 31 To 408 Step 29
                    g.FillEllipse(accent, i, 68, 5, 12)
                    g.DrawLine(edge, i - 6, 68, i + 10, 68)
                Next
                Dim parts As String = LeftId & RightId
                If parts.Contains("T") OrElse parts.Contains("U") Then
                    For i As Integer = 80 To 360 Step 70
                        g.FillEllipse(glass, i - 5, 97, 10, 24)
                        g.DrawEllipse(edge, i - 5, 97, 10, 24)
                        If parts.Contains("U") Then
                            g.FillEllipse(Brushes.DarkSlateGray, i - 4, 101, 8, 5)
                            g.FillEllipse(Brushes.DarkSlateGray, i - 4, 118, 8, 7)
                        End If
                    Next
                End If
            ElseIf Scene = "折叠翼" Then
                Dim top As Integer = If(Folded, 34, 94)
                Dim dx As Integer = If(Folded, 14, 98)
                Shape(g, edge, Brushes.WhiteSmoke, 119, 88, 322, 88, 317, 103, 124, 103)
                Shape(g, edge, accent, 119, 88, 119 - dx, top, 119 - dx, top + 6, 124, 103)
                Shape(g, edge, accent, 322, 88, 322 + dx, top, 322 + dx, top + 6, 317, 103)
                g.DrawEllipse(edge, 115, 87, 10, 10)
                g.DrawEllipse(edge, 316, 87, 10, 10)
            ElseIf Scene = "翼型风洞试验" Then
                For i As Integer = 35 To 155 Step 20
                    Dim d As Integer = If(i < 90, -15, 12)
                    g.DrawBezier(Pens.CadetBlue, 32, i, 138, i, 128, i + d, 214, i + d)
                    g.DrawBezier(Pens.CadetBlue, 214, i + d, 266, i + d, 346, i, 409, i)
                Next
                Using section As New GraphicsPath()
                    section.AddBezier(88, 96, 93, 54, 227, 67, 350, 93)
                    section.AddBezier(350, 93, 220, 83, 116, 113, 88, 96)
                    g.FillPath(accent, section)
                    g.DrawPath(edge, section)
                End Using
            ElseIf Scene = "动力滑翔机" Then
                '俯视图突出大展弦比机翼及机鼻螺旋桨。
                Shape(g, edge, Brushes.WhiteSmoke, 212, 82, 22, 93, 21, 86, 185, 63, 213, 61, 230, 61, 254, 63, 419, 86, 418, 93, 230, 82)
                Shape(g, edge, Brushes.WhiteSmoke, 220, 24, 229, 36, 231, 66, 224, 139, 244, 144, 246, 150, 220, 147, 194, 150, 196, 144, 216, 139, 209, 67, 211, 36)
                g.FillEllipse(glass, 214, 43, 13, 34)
                g.DrawEllipse(edge, 214, 43, 13, 34)
                Shape(g, edge, accent, 187, 27, 219, 21, 252, 20, 251, 24, 220, 26, 188, 31)
                If LeftId = "R" OrElse RightId = "R" Then
                    Shape(g, edge, accent, 21, 87, 12, 69, 18, 70, 28, 90)
                    Shape(g, edge, accent, 419, 87, 428, 69, 422, 70, 412, 90)
                End If
                If LeftId = "Y" OrElse RightId = "Y" Then
                    Shape(g, edge, accent, 281, 43, 337, 25, 342, 31, 286, 49)
                    g.DrawLine(edge, 278, 37, 346, 37)
                    g.DrawArc(edge, 299, 21, 24, 31, 215, 240)
                    Shape(g, edge, Brushes.DarkSlateGray, 309, 49, 315, 53, 318, 46)
                End If
            Else
                '侧视图中用橙色区分浮筒，用轮廓线表示浮筒支柱。
                Shape(g, edge, Brushes.WhiteSmoke, 50, 76, 119, 78, 166, 55, 235, 55, 261, 70, 323, 73, 349, 81, 346, 92, 295, 100, 186, 100, 114, 87, 55, 86)
                Shape(g, edge, glass, 170, 60, 204, 60, 204, 75, 142, 76)
                Shape(g, edge, glass, 211, 60, 232, 60, 251, 73, 211, 74)
                Shape(g, edge, Brushes.WhiteSmoke, 49, 78, 38, 43, 50, 45, 89, 77)
                Shape(g, edge, Brushes.WhiteSmoke, 157, 57, 207, 45, 282, 44, 313, 49, 230, 58)
                If Scene = "浮筒与襟翼" Then
                    Shape(g, edge, accent, 157, 57, 212, 57, 180, 67, 150, 65)
                End If
                g.DrawLine(edge, 353, 59, 353, 114)
                g.DrawLine(edge, 347, 85, 357, 85)
                g.DrawLine(edge, 169, 99, 193, 126)
                g.DrawLine(edge, 228, 99, 211, 125)
                g.DrawLine(edge, 270, 98, 285, 125)
                Shape(g, edge, accent, 111, 124, 309, 124, 338, 115, 323, 134, 235, 141, 233, 137, 149, 138)
                If Scene = "水陆两栖飞机" Then
                    For Each x As Integer In New Integer() {187, 291}
                        g.DrawLine(edge, x, 134, x + 4, 146)
                        g.FillEllipse(Brushes.DarkSlateGray, x - 4, 142, 16, 16)
                        g.FillEllipse(Brushes.WhiteSmoke, x + 1, 147, 6, 6)
                    Next
                    g.DrawLine(Pens.CadetBlue, 75, 159, 369, 159)
                Else
                    For i As Integer = 0 To 2
                        g.DrawBezier(Pens.CadetBlue, 72 + 91 * i, 150, 99 + 91 * i, 145, 121 + 91 * i, 156, 157 + 91 * i, 149)
                    Next
                End If
            End If
        End Using
    End Sub
End Class
