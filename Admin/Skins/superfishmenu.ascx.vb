Imports DotNetNuke.Entities.Tabs
Imports DotNetNuke.UI.Utilities
Imports DotNetNuke.UI.WebControls
Namespace DotNetNuke.UI.Skins.Controls
    Partial Class superfishmenu
        Inherits UI.Skins.NavObjectBase
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        End Sub
        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
            InitializeComponent()
        End Sub
        Private _menuJavaScript As String
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
        Public ReadOnly Property MenuJavaScript() As String
            Get
                Return _menuJavaScript
            End Get
        End Property

        Private Sub BuildTransMenu(ByVal objNode As DNNNode)
            Dim objNodes As DNNNodeCollection
            objNodes = DotNetNuke.UI.Navigation.GetNavigationNodes(ClientID)
            _menuJavaScript = ""
            Dim objPNode As DNNNode
            Dim tab As effority.Ealo.Specialized.TabInfo
            Dim index As Integer = 1
            For Each objPNode In objNodes
                If (Me.MLTabDic.TryGetValue(System.Convert.ToInt32(objPNode.ID), tab)) Then
                    If (Not (tab.EaloTitle Is Nothing)) Then
                        objPNode.Text = tab.EaloTitle.StringTextOrFallBack
                    Else
                        objPNode.Text = tab.Title
                    End If
                End If

                If objPNode.Text <> " " Then
                    If objPNode.Enabled Then
                        _menuJavaScript &= "<li><a href=""" & objPNode.NavigateURL & """>" & objPNode.Text & "</a>"
                    Else
                        _menuJavaScript &= "<li><a style='cursor:pointer;'>" & objPNode.Text & "</a>"
                    End If
                Else
                    index = index + 1
                    Continue For
                End If

                    ProcessChildNodes(objPNode)
                    _menuJavaScript &= "</li>"
                    index = index + 1
            Next
        End Sub

        Private Sub ProcessChildNodes(ByVal objParent As DNNNode)
            Dim objNode As DNNNode
            Dim index As Integer = 1
            Dim tab As effority.Ealo.Specialized.TabInfo
            If objParent.DNNNodes.Count > 0 Then
                _menuJavaScript &= "<ul class='" & objParent.Text.Replace(" ", "") & "'>"
            End If
            For Each objNode In objParent.DNNNodes
                If (Me.MLTabDic.TryGetValue(System.Convert.ToInt32(objNode.ID), tab)) Then
                    If (Not (tab.EaloTitle Is Nothing)) Then
                        objNode.Text = tab.EaloTitle.StringTextOrFallBack
                    Else
                        objNode.Text = tab.Title
                    End If
                End If

                If objNode.Text <> " " Then
                    If objNode.Enabled Then
                        _menuJavaScript &= "<li><a href=""" & objNode.NavigateURL & """>" & objNode.Text & "</a>"
                    Else
                        _menuJavaScript &= "<li><a style='cursor:pointer;'>" & objNode.Text & "</a>"
                    End If
                Else
                    index = index + 1
                    Continue For
                End If

                    ProcessChildNodes(objNode)
                    _menuJavaScript &= "</li>"
                    index = index + 1
            Next
            If objParent.DNNNodes.Count > 0 Then
                _menuJavaScript &= "</ul>"
            End If
        End Sub

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
    End Class
End Namespace