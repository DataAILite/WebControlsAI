Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Text
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.Control
Imports System.Reflection
Imports System.Drawing
Imports System.IO
Imports HTC = System.Web.UI.HtmlControls
Imports Newtonsoft.Json


<DefaultProperty("Text"), ToolboxData("<{0}:MessageControl runat=server></{0}:MessageControl>")>
Public Class MessageControl
    Inherits WebControl

#Region "Variables"

    Private tblMessage As New HTC.HtmlTable
    Private trMessage As New HTC.HtmlTableRow
    Private tdImage As New HTC.HtmlTableCell
    Private tdMessage As New HTC.HtmlTableCell

    Private btnOK As New HTC.HtmlButton
    Private btnYes As New HTC.HtmlButton
    Private btnNo As New HTC.HtmlButton
    Private btnRetry As New HTC.HtmlButton
    Private btnAbort As New HTC.HtmlButton
    Private btnIgnore As New HTC.HtmlButton
    Private btnCancel As New HTC.HtmlButton
    Private btnOther1 As New HTC.HtmlButton
    Private btnOther2 As New HTC.HtmlButton

    Private divMsgBackGround As New HTC.HtmlGenericControl("div")
    Private divMsgBox As New HTC.HtmlGenericControl("div")
    Private divMsgBoxHeading As New HTC.HtmlGenericControl("div")
    Private divX As New HTC.HtmlGenericControl("div")
    Private divMsgBody As New HTC.HtmlGenericControl("div")
    Private divMsgBoxMessage As New HTC.HtmlGenericControl("div")
    Private divButtons As New HTC.HtmlGenericControl("div")

    Private imgInfo As New HTC.HtmlImage
    Private imgError As New HTC.HtmlImage
    Private imgWarning As New HTC.HtmlImage
    Private imgQuestion As New HTC.HtmlImage
    Private imgStop As New HTC.HtmlImage

#End Region

#Region "Enums"
    Public Enum MessageDefaultButton
        OK = 0
        Cancel = 1
        Yes = 2
        No = 3
        Retry = 4
        Abort = 5
        Ignore = 6
        Other1 = 7
        Other2 = 8
        None = 9
    End Enum
    Public Enum MessageIcon
        [Error] = 0
        Warning = 1
        Information = 2
        None = 3
        Question = 4
        [Stop] = 5
    End Enum
    Public Enum Buttons
        OK = 0
        OKCancel = 1
        YesNo = 2
        RetryCancel = 3
        YesNoCancel = 4
        AbortRetryIgnore = 5
        Other = 6
        OtherCancel = 7
        OtherOther = 8
        OtherOtherCancel = 9
    End Enum
    Public Enum MessageResult
        OK = 0
        Cancel = 1
        Yes = 2
        No = 3
        Retry = 4
        Ignore = 5
        Abort = 6
        None = 7
        Other1 = 8
        Other2 = 9
    End Enum
    Public Enum HeaderAlign
        Left = 0
        Center = 1
        right = 2
    End Enum
#End Region

#Region "Properties"
    <Bindable(True),
     Category("Appearance"),
     DefaultValue(""),
     Localizable(True)>
    Property Text() As String
        Get
            Dim s As String = CStr(ViewState("Text"))
            If s Is Nothing Then
                Return String.Empty
            Else
                Return s
            End If
        End Get

        Set(ByVal Value As String)
            ViewState("Text") = Value
        End Set
    End Property

    <Category("Heading Appearance"), DefaultValue("center")>
    Public Property HeadingAlignment As HeaderAlign
        Get
            If ViewState("HeadingAlignment") IsNot Nothing Then
                Return CType(ViewState("HeadingAlignment"), HeaderAlign)
            Else
                ViewState("HeadingAlignment") = HeaderAlign.Center
                Return HeaderAlign.Center
            End If

        End Get
        Set(value As HeaderAlign)
            ViewState("HeadingAlignment") = value
        End Set
    End Property

    <Category("Heading Appearance"), DefaultValue(GetType(Unit), "20px")>
    Public Property HeadingHeight As Unit
        Get
            If ViewState("HeadingHeight") IsNot Nothing Then
                Return CType(ViewState("HeadingHeight"), Unit)
            Else
                ViewState("HeadingHeight") = Unit.Parse("20px")
                Return Unit.Parse("20px")
            End If

        End Get
        Set(value As Unit)
            ViewState("HeadingHeight") = value
        End Set
    End Property

    <Category("Heading Appearance"), DefaultValue(GetType(Drawing.Color), "LightGray")>
    Property HeadingBackColor As Color
        Get
            If ViewState("HeadingBackColor") IsNot Nothing Then
                Return CType(ViewState("HeadingBackColor"), Color)
            Else
                ViewState("HeadingBackColor") = Color.LightGray
                Return Color.LightGray
            End If
        End Get
        Set(value As Color)
            ViewState("HeadingBackColor") = value
        End Set
    End Property
    <Category("Heading Appearance"), DefaultValue(GetType(Drawing.Color), "Black")>
    Property HeadingForeColor As Color
        Get
            If ViewState("HeadingForeColor") IsNot Nothing Then
                Return CType(ViewState("HeadingForeColor"), Color)
            Else
                ViewState("HeadingForeColor") = Color.Black
                Return Color.Black
            End If
        End Get
        Set(value As Color)
            ViewState("HeadingForeColor") = value
        End Set
    End Property
    <Category("Heading Appearance")>
    Property HeadingText As String
        Get
            If ViewState("HeadingText") IsNot Nothing Then
                Return CStr(ViewState("HeadingText"))
            Else
                Return String.Empty
            End If
        End Get
        Set(value As String)
            ViewState("HeadingText") = value
        End Set
    End Property
    <Category("Appearance"), DefaultValue(GetType(Drawing.Color), "#3753fc")>
    Property MessageBackgroundColor As Color
        Get
            If ViewState("MessageBackgroundColor") IsNot Nothing Then
                Return CType(ViewState("MessageBackgroundColor"), Color)
            Else
                ViewState("MessageBackgroundColor") = ColorTranslator.FromHtml("#3753fc")
                Return ColorTranslator.FromHtml("#3753fc")
            End If
        End Get
        Set(value As Color)
            ViewState("HeadingForeColor") = value
            'Me.Style("background-color") = ColorToString(value)
        End Set

    End Property
    <Browsable(False)>
    Public Property ShowMessage As Boolean
        Get
            If ViewState("ShowMessage") IsNot Nothing Then
                Return CBool(ViewState("ShowMessage"))
            Else
                Return False
            End If
        End Get
        Set(value As Boolean)
            ViewState("ShowMessage") = value
        End Set
    End Property
    <Browsable(False)>
    Public Property MessageTag As String
        Get
            If ViewState("MessageTag") Is Nothing Then
                ViewState("MessageTag") = ""
            End If
            Return Convert.ToString(ViewState("MessageTag"))
        End Get
        Set(value As String)
            ViewState("MessageTag") = value
        End Set
    End Property
    <Browsable(False)>
    Public Property OtherButtonText1 As String
        Get
            If ViewState("OtherButtonText1") Is Nothing Then
                ViewState("OtherButtonText1") = ""
            End If
            Return Convert.ToString(ViewState("OtherButtonText1"))
        End Get
        Set(value As String)
            ViewState("OtherButtonText1") = value
            'btnOther1.InnerText = value
        End Set
    End Property
    <Browsable(False)>
    Public Property OtherButtonText2 As String
        Get
            If ViewState("OtherButtonText2") Is Nothing Then
                ViewState("OtherButtonText2") = ""
            End If
            Return Convert.ToString(ViewState("OtherButtonText2"))
        End Get
        Set(value As String)
            ViewState("OtherButtonText2") = value
            'btnOther2.InnerText = value
        End Set
    End Property
    <Category("Behavior"), DefaultValue(GetType(MessageDefaultButton), "None")>
    Public Property DefaultButton As MessageDefaultButton
        Get
            If ViewState("DefaultButton") Is Nothing Then
                ViewState("DefaultButton") = MessageDefaultButton.None
            End If
            Return CType(ViewState("DefaultButton"), MessageDefaultButton)

        End Get
        Set(value As MessageDefaultButton)
            ViewState("DefaultButton") = value
        End Set
    End Property
    <Browsable(False)>
    Public Property VisibleButtons As Buttons
        Get
            If ViewState("VisibleButtons") Is Nothing Then
                Return Buttons.OK
            Else
                Return CType(ViewState("VisibleButtons"), Buttons)
            End If
        End Get
        Set(value As Buttons)
            ViewState("VisibleButtons") = value
        End Set
    End Property
    <Browsable(False)>
    Public Property VisibleIcon As MessageIcon
        Get
            If ViewState("VisibleIcon") Is Nothing Then
                Return MessageIcon.Information
            Else
                Return CType(ViewState("VisibleIcon"), MessageIcon)
            End If
        End Get
        Set(value As MessageIcon)
            ViewState("VisibleIcon") = value
        End Set
    End Property

#End Region

#Region "Classes"
    Public Class MessageEventArgs
        Inherits System.EventArgs

        Private mTag As String = ""
        Private mResult As MessageResult = MessageResult.None

        Public Sub New(MsgResult As MessageResult, MsgTag As String)
            mResult = MsgResult
            mTag = MsgTag
        End Sub
        Public ReadOnly Property Result As MessageResult
            Get
                Return mResult
            End Get
        End Property
        Public ReadOnly Property Tag As String
            Get
                Return mTag
            End Get
        End Property

    End Class

    'Public Class MessageItem
    '    Private mMessageText As String = ""
    '    Private mIcon As MessageIcon = MessageIcon.None

    '    Public Sub New(msg As String, msgIcon As MessageIcon)
    '        mMessageText = msg
    '        mIcon = msgIcon
    '    End Sub
    '    Public Property Icon As MessageIcon
    '        Get
    '            Return mIcon
    '        End Get
    '        Set(value As MessageIcon)
    '            mIcon = value
    '        End Set
    '    End Property

    'Public Property MessageText As String
    '        Get
    '            Return mMessageText
    '        End Get
    '        Set(value As String)
    '            mMessageText = value
    '        End Set
    '    End Property

    'End Class

#End Region

#Region "Overrides"

    Protected Overrides Sub OnInit(e As EventArgs)
        MyBase.OnInit(e)
        EmbedStyleSheet(Me.GetType)
    End Sub

    Protected Overrides Sub OnLoad(e As EventArgs)

        Dim target As String = Page.Request("__EVENTTARGET")
        Dim data As String = Page.Request("__EVENTARGUMENT")

        If target IsNot Nothing AndAlso data IsNot Nothing AndAlso target = Me.ClientID Then
            If data.ToLower() = "close" Then
                ShowMessage = False
                divMsgBackGround.Style("display") = "none"
            ElseIf data.ToLower.StartsWith("button click") Then
                Dim params As String() = Split(data, "~")
                Dim msgResult As MessageResult = ToMessageResult(params(1))
                Dim msgArgs As New MessageEventArgs(msgResult, MessageTag)
                OnMessageResulted(msgArgs)
                ShowMessage = False
                divMsgBackGround.Style("display") = "none"
            End If
        End If

        btnOK.Attributes.Add("onclick", "OnMsgButtonClick(""" & Me.ClientID & """);")
        btnOK.Attributes.Add("data-buttonresult", "OK")
        btnCancel.Attributes.Add("onclick", "OnMsgButtonClick(""" & Me.ClientID & """);")
        btnCancel.Attributes.Add("data-buttonresult", "Cancel")
        btnAbort.Attributes.Add("onclick", "OnMsgButtonClick(""" & Me.ClientID & """);")
        btnAbort.Attributes.Add("data-buttonresult", "Abort")
        btnYes.Attributes.Add("onclick", "OnMsgButtonClick(""" & Me.ClientID & """);")
        btnYes.Attributes.Add("data-buttonresult", "Yes")
        btnNo.Attributes.Add("onclick", "OnMsgButtonClick(""" & Me.ClientID & """);")
        btnNo.Attributes.Add("data-buttonresult", "No")
        btnIgnore.Attributes.Add("onclick", "OnMsgButtonClick(""" & Me.ClientID & """);")
        btnIgnore.Attributes.Add("data-buttonresult", "Ignore")
        btnRetry.Attributes.Add("onclick", "OnMsgButtonClick(""" & Me.ClientID & """);")
        btnRetry.Attributes.Add("data-buttonresult", "Retry")
        btnOther1.Attributes.Add("onclick", "OnMsgButtonClick(""" & Me.ClientID & """);")
        btnOther1.Attributes.Add("data-buttonresult", "Other1")
        btnOther2.Attributes.Add("onclick", "OnMsgButtonClick(""" & Me.ClientID & """);")
        btnOther2.Attributes.Add("data-buttonresult", "Other2")

        MyBase.OnLoad(e)


    End Sub
    Protected Overrides Sub RenderContents(ByVal writer As HtmlTextWriter)
        If DesignMode Then
            writer.Write(ClientID)
            Return
        End If
        MyBase.RenderContents(writer)
    End Sub
    Protected Overrides Sub AddAttributesToRender(writer As HtmlTextWriter)
        MyBase.AddAttributesToRender(writer)
    End Sub

    Protected Overrides Sub CreateChildControls()
        Me.Controls.Clear()
        divMsgBackGround.ID = ClientID & "_MsgBackGround"
        divMsgBackGround.Attributes.Add("class", "PopupBackGround")
        divMsgBackGround.Style("height") = "100%"
        divMsgBackGround.Style("width") = "100%"
        divMsgBackGround.Style("position") = "absolute"
        divMsgBackGround.Style("top") = "0px"
        divMsgBackGround.Style("left") = "0px"
        divMsgBackGround.Controls.Clear()
        If Not ShowMessage Then
            divMsgBackGround.Style("display") = "none"
        Else
            divMsgBackGround.Style("display") = ""
        End If

        Me.Controls.Add(divMsgBackGround)

        divMsgBox.ID = ClientID & "_MsgBox"
        divMsgBox.Style("background-color") = ColorToString(BackColor)
        divMsgBox.Attributes.Add("class", "PopupMsg relative-middle")
        divMsgBox.Attributes.Add("tabindex", "0")
        divMsgBox.Style("display") = "inline-block"

        divMsgBoxHeading.ID = ClientID & "_MsgBoxHeading"
        divMsgBoxHeading.Style("font-size") = "small"
        Dim align As String = "center"
        Select Case HeadingAlignment
            Case HeaderAlign.Left
                align = "left"
            Case HeaderAlign.right
                align = "right"
        End Select
        divMsgBoxHeading.Style("text-align") = align
        divMsgBoxHeading.Style("line-height") = HeadingHeight.ToString
        divMsgBoxHeading.Style("height") = HeadingHeight.ToString
        divMsgBoxHeading.Style("width") = "100%"
        divMsgBoxHeading.Style("border-bottom") = "1px solid"
        divMsgBoxHeading.Style("border-bottom-color") = ColorToString(BorderColor)
        divMsgBoxHeading.Style("background-color") = ColorToString(HeadingBackColor)
        divMsgBoxHeading.Style("color") = ColorToString(HeadingForeColor)
        If HeadingText <> String.Empty Then
            divMsgBoxHeading.InnerText = HeadingText
        Else
            divMsgBoxHeading.InnerText = "Message"
        End If

        divMsgBox.Controls.Add(divMsgBoxHeading)

        divX.ID = ClientID & "_divX"
        divX.Attributes.Add("class", "close")
        divX.Attributes.Add("title", "close dialog")
        divX.Attributes.Add("onclick", "CloseMessage(""" & Me.ClientID & """)")
        divX.InnerHtml = "&times"
        divMsgBoxHeading.Controls.Add(divX)

        divMsgBody.ID = ClientID & "_divMsgBoxBody"
        divMsgBody.Attributes.Add("class", "clearfix")
        divMsgBody.Style("margin") = "5px"
        divMsgBox.Controls.Add(divMsgBody)

        tblMessage.ID = ClientID & "_tblMessage"

        trMessage.ID = ClientID & "_trMessage"

        tdImage.ID = ClientID & "_tdImage"
        tdImage.Style("vertical-align") = "top"

        imgError.ID = ClientID & "_imgError"
        imgError.Src = Me.Page.ClientScript.GetWebResourceUrl(GetType(MessageControl), "MyWebControls.picError.Image.png")
        imgError.Visible = False

        imgWarning.ID = ClientID & "_imgWarning"
        imgWarning.Src = Me.Page.ClientScript.GetWebResourceUrl(GetType(MessageControl), "MyWebControls.picExclaim.Image.png")
        imgWarning.Visible = False

        imgInfo.ID = ClientID & "_imgInfo"
        imgInfo.Src = Me.Page.ClientScript.GetWebResourceUrl(GetType(MessageControl), "MyWebControls.picInfo.Image.png")
        imgInfo.Visible = False

        imgQuestion.ID = ClientID & "_imgQuestion"
        imgQuestion.Src = Me.Page.ClientScript.GetWebResourceUrl(GetType(MessageControl), "MyWebControls.picQuestion.Image.gif")
        imgQuestion.Visible = False

        imgStop.ID = ClientID & "_imgStop"
        imgStop.Src = Me.Page.ClientScript.GetWebResourceUrl(GetType(MessageControl), "MyWebControls.Stop.gif")
        imgStop.Visible = False

        tdMessage.ID = ClientID & "_tdMessage"
        tdMessage.Style("font-family") = "Tahoma"
        tdMessage.Style("font-size") = "medium"

        divMsgBoxMessage.ID = ClientID & "_divMsgBoxMessage"
        If Text <> String.Empty Then
            divMsgBoxMessage.InnerText = Text
        Else
            divMsgBoxMessage.InnerText = "No Message"
        End If

        tdImage.Controls.Add(imgError)
        tdImage.Controls.Add(imgInfo)
        tdImage.Controls.Add(imgQuestion)
        tdImage.Controls.Add(imgStop)
        tdImage.Controls.Add(imgWarning)

        tdMessage.Controls.Add(divMsgBoxMessage)
        trMessage.Controls.Add(tdImage)
        trMessage.Controls.Add(tdMessage)
        tblMessage.Controls.Add(trMessage)

        divMsgBody.Controls.Add(tblMessage)

        divButtons.ID = ClientID & "_divButtons"
        divButtons.Style("text-align") = "center"
        divButtons.Style("width") = "100%"

        btnAbort.ID = ClientID & "_btnAbort"
        btnAbort.Attributes.Add("class", "dlgboxbutton")
        btnAbort.Attributes.Add("runat", "server")
        btnAbort.Attributes.Add("tabindex", "0")
        btnAbort.Attributes.Add("type", "button")
        btnAbort.InnerText = "Abort"
        btnAbort.CausesValidation = False
        btnAbort.Visible = False
        divButtons.Controls.Add(btnAbort)


        btnOK.ID = ClientID & "_btnOK"
        btnOK.Attributes.Add("class", "dlgboxbutton")
        btnOK.Attributes.Add("runat", "server")
        btnOK.Attributes.Add("tabindex", "0")
        btnOK.Attributes.Add("type", "button")
        'btnOK.Attributes.Add("autofocus", "true")
        btnOK.InnerText = "OK"
        btnOK.CausesValidation = False
        btnOK.Visible = False
        divButtons.Controls.Add(btnOK)

        btnRetry.ID = ClientID & "_btnRetry"
        btnRetry.Attributes.Add("class", "dlgboxbutton")
        btnRetry.Attributes.Add("runat", "server")
        btnRetry.Attributes.Add("tabindex", "0")
        btnRetry.Attributes.Add("type", "button")
        btnRetry.InnerText = "Retry"
        btnRetry.CausesValidation = False
        btnRetry.Visible = False
        divButtons.Controls.Add(btnRetry)

        btnIgnore.ID = ClientID & "_btnIgnore"
        btnIgnore.Attributes.Add("class", "dlgboxbutton")
        btnIgnore.Attributes.Add("runat", "server")
        btnIgnore.Attributes.Add("tabindex", "0")
        btnIgnore.Attributes.Add("type", "button")
        btnIgnore.InnerText = "Ignore"
        btnIgnore.CausesValidation = False
        btnIgnore.Visible = False
        divButtons.Controls.Add(btnIgnore)

        btnYes.ID = ClientID & "_btnYes"
        btnYes.Attributes.Add("class", "dlgboxbutton")
        btnYes.Attributes.Add("runat", "server")
        btnYes.Attributes.Add("tabindex", "0")
        btnYes.Attributes.Add("type", "button")
        btnYes.InnerText = "Yes"
        btnYes.CausesValidation = False
        btnYes.Visible = False
        divButtons.Controls.Add(btnYes)

        btnNo.ID = ClientID & "_btnNo"
        btnNo.Attributes.Add("class", "dlgboxbutton")
        btnNo.Attributes.Add("runat", "server")
        btnNo.Attributes.Add("tabindex", "0")
        btnNo.Attributes.Add("type", "button")
        btnNo.InnerText = "No"
        btnNo.CausesValidation = False
        btnNo.Visible = False
        divButtons.Controls.Add(btnNo)

        btnOther1.ID = ClientID & "_btnOther1"
        btnOther1.Attributes.Add("class", "dlgboxbutton")
        btnOther1.Attributes.Add("runat", "server")
        btnOther1.Attributes.Add("tabindex", "0")
        btnOther1.Attributes.Add("type", "button")
        If OtherButtonText1 <> String.Empty Then
            btnOther1.InnerText = OtherButtonText1
        Else
            btnOther1.InnerText = "Other 1"
        End If

        btnOther1.CausesValidation = False
        btnOther1.Visible = False
        divButtons.Controls.Add(btnOther1)

        btnOther2.ID = ClientID & "_btnOther2"
        btnOther2.Attributes.Add("class", "dlgboxbutton")
        btnOther2.Attributes.Add("runat", "server")
        btnOther2.Attributes.Add("tabindex", "0")
        btnOther2.Attributes.Add("type", "button")
        If OtherButtonText2 <> String.Empty Then
            btnOther2.InnerText = OtherButtonText2
        Else
            btnOther2.InnerText = "Other 2"
        End If
        btnOther2.CausesValidation = False
        btnOther2.Visible = False
        divButtons.Controls.Add(btnOther2)

        btnCancel.ID = ClientID & "_btnCancel"
        btnCancel.Attributes.Add("class", "dlgboxbutton")
        btnCancel.Attributes.Add("runat", "server")
        btnCancel.Attributes.Add("tabindex", "0")
        btnCancel.Attributes.Add("type", "button")
        btnCancel.InnerText = "Cancel"
        btnCancel.CausesValidation = False
        btnCancel.Visible = False
        divButtons.Controls.Add(btnCancel)

        divMsgBox.Controls.Add(divButtons)
        divMsgBackGround.Controls.Add(divMsgBox)
        MakeButtonsVisible(VisibleButtons)
        MakeIconVisible(VisibleIcon)
        SetDefaultButtonFocus(DefaultButton)

        MyBase.CreateChildControls()
    End Sub

    Protected Overrides Sub OnPreRender(e As EventArgs)
        If Not Me.DesignMode Then
            EmbedJavaScript(Me.GetType)
        End If
        MyBase.OnPreRender(e)
    End Sub
    Protected Overrides ReadOnly Property tagkey As System.Web.UI.HtmlTextWriterTag
        Get
            Return HtmlTextWriterTag.Div
        End Get
    End Property

    'Protected Overrides ReadOnly Property TagKey As HtmlTextWriterTag
    '    Get
    '        Return MyBase.TagKey
    '    End Get
    'End Property
    Protected Overrides Sub Render(writer As HtmlTextWriter)
        MyBase.Render(writer)
    End Sub

    'Public Overrides Property CssClass As String
    '    Get
    '        Return MyBase.CssClass
    '    End Get
    '    Set(value As String)
    '        MyBase.CssClass = value
    '    End Set
    'End Property
#End Region

#Region "Private Methods/Functions"
    Private Sub EmbedJavaScript(ThisType As Type)
        If Not Me.DesignMode Then
            Dim csm As ClientScriptManager = Page.ClientScript
            Dim smName As String = "MessageControlScript"
            If Not csm.IsClientScriptBlockRegistered(ThisType, smName) Then
                Dim names As String() = ThisType.Assembly.GetManifestResourceNames
                If names.Length > 0 Then
                    Dim ThisAssembly As Assembly = ThisType.Assembly
                    Dim AssemblyName As String = ThisAssembly.GetName.Name
                    Dim JavaResource As String = ".MessageControl.js"
                    Dim exAssembly As Assembly = Assembly.GetExecutingAssembly
                    Dim st As Stream = Assembly.GetExecutingAssembly.GetManifestResourceStream(ThisType.Namespace & JavaResource)
                    Dim sr As StreamReader = New StreamReader(st)
                    csm.RegisterClientScriptBlock(ThisType, smName, sr.ReadToEnd, True)
                End If
            End If
        End If
    End Sub
    Private Sub EmbedStyleSheet(ThisType As Type)
        If Not Me.DesignMode Then
            Dim cssURL As String = Page.ClientScript.GetWebResourceUrl(ThisType, "MyWebControls.MessageControl.css")
            Dim csm As ClientScriptManager = Page.ClientScript
            Dim cssLink As New HTC.HtmlLink
            cssLink.Href = cssURL
            cssLink.Attributes.Add("rel", "stylesheet")
            cssLink.Attributes.Add("type", "text/css")
            Dim ss As String = cssLink.ToString
            Page.Header.Controls.Add(cssLink)
            Dim smName = "cssMessageControl"
            If Not csm.IsClientScriptBlockRegistered(ThisType, smName) Then
                Dim css As String = "<link href=""" & cssURL & " type=""text/css"" rel=""stylesheet"" />"
                csm.RegisterClientScriptBlock(ThisType, smName, css, False)
            End If
        End If

    End Sub
    Private Function ColorToString(clr As Color) As String
        Dim sColor As String = clr.Name.ToLower
        If Not clr.IsNamedColor Then
            Dim r As String = Hex(clr.R)
            Dim g As String = Hex(clr.G)
            Dim b As String = Hex(clr.B)

            r = IIf(r.Length = 1, "0" & r, r).ToString
            g = IIf(g.Length = 1, "0" & g, g).ToString
            b = IIf(b.Length = 1, "0" & b, b).ToString

            sColor = "#" & r & g & b
        End If
        Return sColor
    End Function
    Private Sub MakeIconVisible(msgIcon As MessageIcon)
        imgError.Visible = False
        imgInfo.Visible = False
        imgQuestion.Visible = False
        imgStop.Visible = False
        imgWarning.Visible = False

        Select Case msgIcon
            Case MessageIcon.Error
                imgError.Visible = True
            Case MessageIcon.Information
                imgInfo.Visible = True
            Case MessageIcon.Question
                imgQuestion.Visible = True
            Case MessageIcon.Stop
                imgStop.Visible = True
            Case MessageIcon.Warning
                imgWarning.Visible = True
        End Select
    End Sub
    Private Sub MakeButtonsVisible(Button As Buttons)
        btnOK.Visible = False
        btnCancel.Visible = False
        btnYes.Visible = False
        btnNo.Visible = False
        btnRetry.Visible = False
        btnAbort.Visible = False
        btnIgnore.Visible = False
        btnOther1.Visible = False
        btnOther2.Visible = False

        Select Case Button
            Case Buttons.OK
                btnOK.Visible = True
            Case Buttons.OKCancel
                btnOK.Visible = True
                btnCancel.Visible = True
            Case Buttons.YesNo
                btnYes.Visible = True
                btnNo.Visible = True
            Case Buttons.YesNoCancel
                btnYes.Visible = True
                btnNo.Visible = True
                btnCancel.Visible = True
            Case Buttons.RetryCancel
                btnRetry.Visible = True
                btnCancel.Visible = True
            Case Buttons.AbortRetryIgnore
                btnAbort.Visible = True
                btnRetry.Visible = True
                btnIgnore.Visible = True
            Case Buttons.Other
                btnOther1.Visible = True
            Case Buttons.OtherCancel
                btnOther1.Visible = True
                btnCancel.Visible = True
            Case Buttons.OtherOther
                btnOther1.Visible = True
                btnOther2.Visible = True
            Case Buttons.OtherOtherCancel
                btnOther1.Visible = True
                btnOther2.Visible = True
                btnCancel.Visible = True
        End Select

    End Sub

    Private Sub SetDefaultButtonFocus(btnDefault As MessageDefaultButton)
        'btnOK.Attributes.Add("autofocus", "true")
        btnOK.Attributes.Remove("autofocus")
        btnCancel.Attributes.Remove("autofocus")
        btnYes.Attributes.Remove("autofocus")
        btnNo.Attributes.Remove("autofocus")
        btnRetry.Attributes.Remove("autofocus")
        btnAbort.Attributes.Remove("autofocus")
        btnIgnore.Attributes.Remove("autofocus")
        btnOther1.Attributes.Remove("autofocus")
        btnOther2.Attributes.Remove("autofocus")


        Select Case btnDefault
            Case MessageDefaultButton.OK
                If btnOK.Visible Then btnOK.Attributes.Add("autofocus", "true")
            Case MessageDefaultButton.Cancel
                If btnCancel.Visible Then btnCancel.Attributes.Add("autofocus", "true")
                'SetDefaultFocus("btnPostCancel")
            Case MessageDefaultButton.Yes
                If btnYes.Visible Then btnYes.Attributes.Add("autofocus", "true")
            Case MessageDefaultButton.No
                If btnNo.Visible Then btnNo.Attributes.Add("autofocus", "true")
            Case MessageDefaultButton.Retry
                If btnRetry.Visible Then btnRetry.Attributes.Add("autofocus", "true")
            Case MessageDefaultButton.Abort
                If btnAbort.Visible Then btnAbort.Attributes.Add("autofocus", "true")
            Case MessageDefaultButton.Ignore
                If btnIgnore.Visible Then btnIgnore.Attributes.Add("autofocus", "true")
            Case MessageDefaultButton.Other1
                If btnOther1.Visible Then btnOther1.Attributes.Add("autofocus", "true")
            Case MessageDefaultButton.Other2
                If btnOther2.Visible Then btnOther2.Attributes.Add("autofocus", "true")
        End Select

    End Sub
    Private Function ToMessageResult(result As String) As MessageResult
        Dim ret As MessageResult = MessageResult.None
        Select Case result.ToLower()
            Case "ok"
                ret = MessageResult.OK
            Case "cancel"
                ret = MessageResult.Cancel
            Case "yes"
                ret = MessageResult.Yes
            Case "no"
                ret = MessageResult.No
            Case "retry"
                ret = MessageResult.Retry
            Case "ignore"
                ret = MessageResult.Ignore
            Case "abort"
                ret = MessageResult.Abort
            Case "other1"
                ret = MessageResult.Other1
            Case "other2"
                ret = MessageResult.Other2
        End Select
        Return ret
    End Function
#End Region

#Region "Public Methods/Functions"

    Public Sub Show(msg As String)
        Text = msg
        ShowMessage = True
        DefaultButton = MessageDefaultButton.OK
    End Sub
    Public Sub Show(msg As String, caption As String)
        Text = msg
        HeadingText = caption
        ShowMessage = True
        DefaultButton = MessageDefaultButton.OK
    End Sub
    Public Sub Show(msg As String, caption As String, Tag As String, Buttons As Buttons)
        Text = msg
        HeadingText = caption
        MessageTag = Tag
        VisibleButtons = Buttons
        ShowMessage = True
    End Sub
    Public Sub Show(msg As String, caption As String, Tag As String, Buttons As Buttons, Icon As MessageIcon)
        Text = msg
        HeadingText = caption
        MessageTag = Tag
        VisibleButtons = Buttons
        VisibleIcon = Icon
        ShowMessage = True
    End Sub
    Public Sub Show(msg As String, caption As String, Tag As String, Buttons As Buttons, Icon As MessageIcon, DefaultBtn As MessageDefaultButton)
        Text = msg
        HeadingText = caption
        MessageTag = Tag
        VisibleButtons = Buttons
        VisibleIcon = Icon
        DefaultButton = DefaultBtn
        ShowMessage = True
    End Sub
#End Region

#Region "Event Definitions"
    Public Delegate Sub MsgBoxEventHandler(sender As Object, e As MessageEventArgs)
    Public Event MessageResulted As MsgBoxEventHandler
    Protected Overridable Sub OnMessageResulted(e As MessageEventArgs)
        RaiseEvent MessageResulted(Me, e)
    End Sub
#End Region

#Region "Event Handlers"
    Protected Sub btnOK_Click(sender As Object, e As EventArgs)
        OnMessageResulted(New MessageEventArgs(MessageResult.OK, MessageTag))
        MessageTag = ""
    End Sub
#End Region

End Class
