Imports System.Reflection

Public Class LabForm
    Inherits LabWindow

    Private ReadOnly cards As New List(Of Button)
    Private ReadOnly groups As New List(Of Label)
    Private ReadOnly sectionCards As New Dictionary(Of String, List(Of Button))(StringComparer.Ordinal)
    Private ReadOnly tips As New ToolTip()
    Private ReadOnly seen As New List(Of String)
    Private first As String = ""
    Private second As String = ""
    Private choices As New List(Of Recipe)
    Private position As Integer
    Private selectedLabel, resultTitle, resultType, detail, credit, heading, instructions, gallery As Label
    Private canvas As Diagram
    Private photo As PictureBox
    Private ReadOnly source As New LinkLabel With {.AutoSize = False, .TextAlign = ContentAlignment.MiddleRight}
    Private compare, nextResult, chain, history, clear As Button
    Private synthesize As Button
    Private revealed As Boolean
    Private ready As Boolean

    Public Sub New()
        LoadCatalog()
        Text = "航空合成实验室 · 构型工作台"
        FormBorderStyle = FormBorderStyle.Sizable
        MaximizeBox = True
        MinimumSize = New Size(790, 580)
        ClientSize = New Size(1200, 670)
        WindowState = FormWindowState.Maximized
        heading = Caption("航空合成实验室", 0, 0, 100, 42, 22, True)
        instructions = Caption("选两件部件，再点右侧的「合成」。", 0, 0, 100, 28, 10)
        For Each part As PartCard In Parts
            If Not sectionCards.ContainsKey(part.Section) Then
                groups.Add(Caption(part.Section, 0, 0, 100, 24, 10, True))
                sectionCards.Add(part.Section, New List(Of Button))
            End If
            Dim b As Button = ActionButton(part.Name, 0, 0, 10, 10)
            b.Tag = part.Id
            b.Name = "part" & part.Id
            tips.SetToolTip(b, part.Hint)
            AddHandler b.Click, AddressOf PartClicked
            cards.Add(b)
            sectionCards(part.Section).Add(b)
        Next
        selectedLabel = Caption("", 0, 0, 100, 44, 11, True)
        selectedLabel.Name = "selection"
        resultType = Caption("", 0, 0, 100, 25, 11, True)
        resultType.ForeColor = Teal
        resultTitle = Caption("", 0, 0, 100, 46, 20, True)
        resultTitle.Name = "resultTitle"
        detail = Caption("", 0, 0, 100, 76, 11)
        canvas = New Diagram With {.AccessibleName = "构型或部件示意图"}
        photo = New PictureBox With {.Name = "photo", .SizeMode = PictureBoxSizeMode.Zoom, .BackColor = Color.FromArgb(233, 238, 234), .Cursor = Cursors.Hand}
        Controls.AddRange(New Control() {canvas, photo})
        AddHandler photo.Click, AddressOf ZoomPhoto
        credit = Caption("", 0, 0, 100, 50, 9)
        source.LinkColor = Teal
        Controls.Add(source)
        AddHandler source.LinkClicked, Sub()
                                           Try
                                               System.Diagnostics.Process.Start(CStr(source.Tag))
                                           Catch ex As System.ComponentModel.Win32Exception
                                               MsgBox(CStr(source.Tag), MsgBoxStyle.Information, "资料网址")
                                           End Try
                                       End Sub
        synthesize = ActionButton("合成", 0, 0, 135, 42)
        synthesize.Name = "synthesize"
        synthesize.BackColor = Teal
        synthesize.ForeColor = Color.White
        AddHandler synthesize.Click, Sub()
                                         revealed = True
                                         ShowResult()
                                     End Sub
        gallery = Caption("", 0, 0, 100, 36, 10)
        compare = ActionButton("查看实机照片", 0, 0, 100, 34)
        compare.Name = "compare"
        nextResult = ActionButton("另一种结果", 0, 0, 100, 34)
        nextResult.Name = "nextResult"
        chain = ActionButton("用成果继续合成", 0, 0, 100, 34)
        chain.Name = "chain"
        history = ActionButton("发现记录", 0, 0, 100, 34)
        clear = ActionButton("重新选取", 0, 0, 100, 34)
        AddHandler compare.Click, AddressOf ToggleImage
        AddHandler nextResult.Click, Sub()
                                         position = (position + 1) Mod choices.Count
                                         ShowResult()
                                     End Sub
        AddHandler chain.Click, Sub()
                                    first = "浮筒水上飞机"
                                    SelectParts("")
                                End Sub
        AddHandler clear.Click, Sub()
                                    first = ""
                                    SelectParts("")
                                End Sub
        AddHandler history.Click, AddressOf OpenHistory
        ready = True
        SelectParts("")
    End Sub

    Private Sub PartClicked(sender As Object, e As EventArgs)
        Dim id As String = CStr(DirectCast(sender, Button).Tag)
        If first = "" OrElse first = id Then
            first = If(first = id, "", id)
            SelectParts("")
        Else
            SelectParts(id)
        End If
    End Sub

    Private Sub SelectParts(id As String)
        second = id
        revealed = False
        choices = Matches(first, second)
        position = 0
        For Each card As Button In cards
            Dim key As String = CStr(card.Tag)
            Dim active As Boolean = key = first OrElse key = second
            card.BackColor = If(active, Teal, Color.White)
            card.ForeColor = If(active, Color.White, Ink)
            Dim fits As Boolean = Matches(first, key).Count > 0
            card.FlatAppearance.BorderColor = If(fits, Teal, Color.FromArgb(195, 203, 197))
            card.FlatAppearance.BorderSize = If(fits, 2, 1)
        Next
        selectedLabel.Text = If(first = "", "第一件   待选择", "已固定   " & PartName(first)) & If(second = "", "", "   +   " & PartName(second))
        clear.Enabled = first <> ""
        ShowResult()
    End Sub

    Private Sub ShowResult()
        photo.Visible = False
        If photo.Image IsNot Nothing Then
            photo.Image.Dispose()
            photo.Image = Nothing
        End If
        compare.Visible = False
        chain.Visible = False
        synthesize.Visible = second <> "" AndAlso Not revealed
        nextResult.Visible = revealed AndAlso choices.Count > 1
        canvas.Folded = False
        canvas.LeftId = first
        canvas.RightId = second
        credit.Text = ""
        source.Visible = False
        If second <> "" AndAlso Not revealed Then
            resultType.Text = "已选部件"
            resultTitle.Text = PartName(first) & " ＋ " & PartName(second)
            detail.Text = "点击「合成」查看结果，也可以继续更换第二件。"
            canvas.Scene = "部件"
        ElseIf choices.Count = 0 Then
            resultType.Text = "组合工作台"
            resultTitle.Text = If(first = "", "从哪一件开始？", If(second = "", "再选一件部件", "这对搭配还没有收录"))
            detail.Text = If(first = "", "可以从「细长的机翼」开始，再试「太阳能电池片」或「可折叠接头」，看看同一副机翼能走向哪些设计。", "绿色边框表示有收录搭配。第一件会保留，直接点另一件就能换一组。再点第一件可取消固定。")
            If first <> "" Then
                Dim partners As Integer = cards.FindAll(Function(b) Matches(first, CStr(b.Tag)).Count > 0).Count
                detail.Text = "「" & PartName(first) & "」有 " & partners.ToString() & " 个搭档，已用绿色边框标出。直接点选就能换搭档，点「重新选取」可换第一件。"
            End If
            canvas.Scene = If(first = "", "动力滑翔机", "部件")
        Else
            Dim r As Recipe = choices(position)
            resultType.Text = r.Category & If(choices.Count > 1, "   ·   " & (position + 1).ToString() & " / " & choices.Count.ToString(), "")
            resultTitle.Text = r.Title
            detail.Text = r.Detail
            Dim record As String = PartName(first) & " + " & PartName(second) & "  →  " & r.Title
            Dim reverse As String = PartName(second) & " + " & PartName(first) & "  →  " & r.Title
            If Not seen.Contains(record) AndAlso Not seen.Contains(reverse) Then seen.Add(record)
            canvas.Scene = If(r.Title = "翼内电池舱" OrElse r.Title = "翼内轮舱" OrElse r.Title = "电动螺旋桨" OrElse r.Title = "白天充电的机载电源", r.Title, "构型")
            Dim resource As String = ""
            Dim url As String = ""
            Select Case r.Kind
                Case "longez"
                    resource = "LongEZ"
                    canvas.Scene = "后掠推式"
                    url = "https://ntrs.nasa.gov/api/citations/19840018592/downloads/19840018592.pdf#page=84"
                    credit.Text = "Rutan Long-EZ  ·  NASA 图 6，照片 L-81-9758"
                Case "helios"
                    url = "https://www.nasa.gov/image-article/helios-prototype-flying-wing/"
                    resource = "Helios"
                    canvas.Scene = "太阳能飞翼"
                    credit.Text = "NASA Helios 原型机  ·  摄影 NASA / Nick Galante"
                Case "float", "twinotter"
                    url = "https://vgls.sdp.sirsidynix.net.au/client/search/asset/1016505"
                    resource = "TwinOtter"
                    canvas.Scene = "浮筒水上飞机"
                    credit.Text = "DHC-6 Twin Otter  ·  维多利亚州航空报告第 81 号，图版 1"
                    If r.Kind = "twinotter" Then
                        url = "https://www.tsb.gc.ca/eng/rapports-reports/aviation/2000/a00p0210/a00p0210.html"
                        canvas.Scene = "浮筒与襟翼"
                    End If
                Case "amph"
                    url = "https://vgls.sdp.sirsidynix.net.au/client/search/asset/1016505"
                    resource = "Cessna"
                    canvas.Scene = r.Title
                    credit.Text = "Cessna U206G Turbine  ·  照片 Coulson Flying Tankers，图版 3"
                Case "stemme"
                    url = "https://www.stemme.com/s12.html"
                    resource = "Stemme"
                    canvas.Scene = "动力滑翔机"
                    credit.Text = "Twin Voyager S12 型号页配图  ·  Stemme GmbH"
                Case "折叠"
                    canvas.Scene = "折叠翼"
                    compare.Visible = True
                    compare.Text = "收起翼尖"
                Case "试验"
                    If (first = "P" OrElse second = "P") AndAlso (first = "F" OrElse second = "F" OrElse first = "E" OrElse second = "E") Then canvas.Scene = "翼型风洞试验"
            End Select
            If resource = "" Then
                '推想与试验展示一架相关实机，图注写明可以对照的具体特征。
                Dim pair As String = r.First & r.Second
                Select Case True
                    Case pair.Contains("M"), pair.Contains("N"), pair.Contains("O"), pair.Contains("U")
                        resource = "Cessna"
                        credit.Text = "起落装置参考 · Cessna U206G 的浮筒与机轮"
                        url = "https://vgls.sdp.sirsidynix.net.au/client/search/asset/1016505"
                    Case pair.Contains("D"), pair.Contains("P"), pair.Contains("Q"), pair.Contains("R")
                        resource = "LongEZ"
                        credit.Text = "气动外形参考 · NASA 层流飞行试验用 Long-EZ"
                        url = "https://ntrs.nasa.gov/api/citations/19840018592/downloads/19840018592.pdf#page=84"
                    Case pair.Contains("K"), pair.Contains("I"), pair.Contains("L"), pair.Contains("H"), pair.Contains("T")
                        resource = "Helios"
                        credit.Text = "翼面与动力布局参考 · NASA Helios"
                        url = "https://www.nasa.gov/image-article/helios-prototype-flying-wing/"
                    Case Else
                        resource = "Stemme"
                        credit.Text = "机翼与机身布局参考 · Stemme S12"
                        url = "https://www.stemme.com/s12.html"
                End Select
                source.Text = "参考机型 ↗"
            ElseIf url <> "" Then
                source.Text = "案例出处 ↗"
            End If
            source.Tag = url
            source.Visible = url <> ""
            If resource <> "" Then
                Using stream As IO.Stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resource & ".jpg")
                    If stream IsNot Nothing Then
                        Using original As Image = Image.FromStream(stream)
                            photo.Image = New Bitmap(original)
                        End Using
                        photo.Visible = True
                        photo.AccessibleName = credit.Text
                    End If
                End Using
            End If
            chain.Visible = r.Kind = "float"
        End If
        gallery.Text = "本次已发现 " & seen.Count.ToString() & " 条组合记录"
        Arrange()
        canvas.Invalidate()
    End Sub

    Private Sub OpenHistory(sender As Object, e As EventArgs)
        If seen.Count = 0 Then
            MsgBox("还没有发现。试试细长的机翼和太阳能电池片。", MsgBoxStyle.Information, "发现记录")
            Return
        End If
        Using book As New Form With {.Text = "本次发现记录", .StartPosition = FormStartPosition.CenterParent, .ClientSize = New Size(600, 360), .Font = Me.Font}
            Dim entries As New ListBox With {.Dock = DockStyle.Fill, .HorizontalScrollbar = True}
            entries.Items.AddRange(seen.ToArray())
            book.Controls.Add(entries)
            book.ShowDialog(Me)
        End Using
    End Sub

    Private Sub ToggleImage(sender As Object, e As EventArgs)
        If canvas.Scene = "折叠翼" Then
            canvas.Folded = Not canvas.Folded
            compare.Text = If(canvas.Folded, "展开翼尖", "收起翼尖")
            canvas.Invalidate()
        End If
    End Sub

    Private Sub ZoomPhoto(sender As Object, e As EventArgs)
        If photo.Image Is Nothing Then Return
        Using viewer As New Form With {.Text = credit.Text, .StartPosition = FormStartPosition.CenterParent, .ClientSize = New Size(820, 500)}
            viewer.Controls.Add(New PictureBox With {.Image = photo.Image, .SizeMode = PictureBoxSizeMode.Zoom, .Dock = DockStyle.Fill})
            viewer.ShowDialog(Me)
        End Using
    End Sub

    Protected Overrides Sub OnResize(e As EventArgs)
        MyBase.OnResize(e)
        If ready Then Arrange()
    End Sub

    Private Sub Arrange()
        Dim w As Integer = Math.Max(940, ClientSize.Width - If(VerticalScroll.Visible, 18, 0))
        Dim h As Integer = Math.Max(610, ClientSize.Height)
        Dim left As Integer = Math.Max(500, CInt(w * 0.44))
        Dim rx As Integer = left + 28
        Dim rw As Integer = w - rx - 24
        heading.SetBounds(24, 12, left, 46)
        instructions.SetBounds(26, 61, left - 12, 30)
        Dim y As Integer = 101
        Const cols As Integer = 4
        Dim cw As Integer = (left - 36) \ cols
        For Each section As Label In groups
            section.SetBounds(26, y, left - 30, 21)
            Dim entries As List(Of Button) = sectionCards(section.Text)
            For j As Integer = 0 To entries.Count - 1
                entries(j).SetBounds(24 + (j Mod cols) * cw, y + 23 + (j \ cols) * 32, cw - 5, 29)
            Next
            y += 61 + ((entries.Count - 1) \ cols) * 32
        Next
        selectedLabel.SetBounds(24, y, left - 28, 38)
        clear.SetBounds(24, y + 42, 105, 34)
        history.SetBounds(138, y + 42, 105, 34)
        gallery.SetBounds(24, y + 83, left - 30, 30)
        resultType.SetBounds(rx, 28, rw, 28)
        resultTitle.SetBounds(rx, 62, rw, 50)
        detail.SetBounds(rx, 118, rw, 80)
        Dim imageHeight As Integer = Math.Min(390, Math.Max(200, h - 387))
        Dim cwImage As Integer = If(photo.Visible, (rw - 8) \ 2, rw)
        canvas.SetBounds(rx, 212, cwImage, imageHeight)
        If photo.Visible Then photo.SetBounds(rx + cwImage + 8, 212, rw - cwImage - 8, imageHeight)
        credit.SetBounds(rx, 216 + imageHeight, rw - If(source.Visible, 102, 0), 41)
        source.SetBounds(rx + rw - 100, 216 + imageHeight, 100, 41)
        Dim bx As Integer = rx
        synthesize.SetBounds(rx, 263 + imageHeight, 138, 42)
        For Each b As Button In New Button() {compare, nextResult, chain}
            If b.Visible Then
                b.SetBounds(bx, 263 + imageHeight, 128, 36)
                bx += 136
            End If
        Next
    End Sub
End Class
