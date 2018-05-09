
Imports DotNetNuke.Entities.Tabs
Imports DotNetNuke.UI.Utilities
Imports DotNetNuke.UI.WebControls

Namespace DotNetNuke.UI.Skins.Controls

	''' -----------------------------------------------------------------------------
	''' Project	 : DotNetNuke
    ''' Class	 : TransMenu
	''' -----------------------------------------------------------------------------
	''' <summary>
    ''' TransMenu implements the transmenu.
	''' </summary>
	''' <remarks></remarks>
	''' <history>
    ''' </history>
	''' -----------------------------------------------------------------------------
    Partial Class TransMenu

        Inherits UI.Skins.NavObjectBase

#Region " Web Form Designer Generated Code "


        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

        End Sub

        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
            'CODEGEN: This method call is required by the Web Form Designer
            'Do not modify it using the code editor.
            InitializeComponent()
        End Sub

#End Region

#Region "Private Members"
        Private _menuJavaScript As String
        Private _menuRootItems As String
        Private _separator As String
        Private _itemID As Integer
        Public AppPath As String

        Private _MLTabDic As System.Collections.Generic.Dictionary(Of Int32, effority.Ealo.Specialized.TabInfo)
        Public ReadOnly Property MLTabDic() As System.Collections.Generic.Dictionary(Of Int32, effority.Ealo.Specialized.TabInfo)
            Get
                If (_MLTabDic Is Nothing) Then
                    _MLTabDic = effority.Ealo.Specialized.Tabs.GetTabsAsDictionary(PortalSettings.PortalId, System.Threading.Thread.CurrentThread.CurrentCulture.ToString(), False)
                End If
                Return _MLTabDic
            End Get
        End Property
#End Region

#Region "Public Properties"
        Public ReadOnly Property MenuJavaScript() As String
            Get
                Return _menuJavaScript
            End Get
        End Property

        Public ReadOnly Property MenuRootItems() As String
            Get
                Return _menuRootItems
            End Get
        End Property

        Public Property Separator() As String
            Get
                Return _separator
            End Get
            Set(ByVal value As String)
                _separator = value
            End Set
        End Property
#End Region

#Region "Private Methods"
        Private Sub BuildTransMenu(ByVal objNode As DNNNode)

            Dim objNodes As DNNNodeCollection
            objNodes = DotNetNuke.UI.Navigation.GetNavigationNodes(ClientID)
            'objNodes = GetNavigationNodes(objNode)
            _menuRootItems = ""
            _menuJavaScript = ""
            _itemID = 10

            Dim objPNode As DNNNode
            Dim index As Integer = 0
            Dim tab As effority.Ealo.Specialized.TabInfo
            For Each objPNode In objNodes
                If (Me.MLTabDic.TryGetValue(System.Convert.ToInt32(objPNode.ID), tab)) Then
                    If (Not (tab.EaloTitle Is Nothing)) Then
                        objPNode.Text = tab.EaloTitle.StringTextOrFallBack
                    Else
                        objPNode.Text = tab.Title
                    End If
                End If

                _menuRootItems &= "<td><A class=""mainlevel-trans"" id=""transmenu" & _itemID & """"
                _menuRootItems &= " href=""" & objPNode.NavigateURL & """"
                _menuRootItems &= ">" & objPNode.Text & "</A></td>" & vbCrLf

                If index <> objNodes.Count - 1 Then
                    If Separator.Contains(".") Then
                        _menuRootItems &= "<td><img border='0' src='" + Common.Globals.GetPortalSettings().ActiveTab.SkinPath + "images/" + Separator + "'/></td>" & vbCrLf
                    Else
                        _menuRootItems &= "<td class='mainlevel-trans'>" + Separator + "</td>" & vbCrLf
                    End If
                End If

                If objPNode.HasNodes Then
                    _menuJavaScript &= "var subtransmenu" & _itemID & "=ms.addMenu(document.getElementById('transmenu" & _itemID & "'));" & vbCrLf
                Else
                    _menuJavaScript &= "document.getElementById(""transmenu" & _itemID & """).onmouseover = function() { ms.hideCurrent(); }" & vbCrLf
                End If
                ProcessChildNodes(objPNode, _itemID)
                index += 1
                _itemID += 1
            Next
        End Sub

        Private Sub ProcessChildNodes(ByVal objParent As DNNNode, ByVal parentID As Integer)
            _menuJavaScript &= ""

            Dim objNode As DNNNode
            Dim index As Integer = 0
            Dim tab As effority.Ealo.Specialized.TabInfo
            For Each objNode In objParent.DNNNodes
                If (Me.MLTabDic.TryGetValue(System.Convert.ToInt32(objNode.ID), tab)) Then
                    If (Not (tab.EaloTitle Is Nothing)) Then
                        objNode.Text = tab.EaloTitle.StringTextOrFallBack
                    Else
                        objNode.Text = tab.Title
                    End If
                End If
                _menuJavaScript &= "subtransmenu" & parentID & ".addItem(""" & objNode.Text & """,""" & objNode.NavigateURL & """,""0"");" & vbCrLf
                If objNode.HasNodes Then
                    _itemID += +1
                    _menuJavaScript &= "var subtransmenu" & _itemID & " = subtransmenu" & parentID & ".addMenu(subtransmenu" & parentID & ".items[" & index & "]);" & vbCrLf
                    ProcessChildNodes(objNode, _itemID)
                End If
                index += 1
            Next
        End Sub

#End Region

#Region "Event Handlers"

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try
                If Common.Globals.ApplicationPath = "/" Then
                    AppPath = ""
                Else
                    AppPath = Common.Globals.ApplicationPath
                End If

                BuildTransMenu(Nothing)
            Catch exc As Exception           'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        

#End Region

    End Class

End Namespace
