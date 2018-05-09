Imports Microsoft.VisualBasic
Imports System.IO
Imports System.Data.SqlClient
Imports System.Xml
Imports System.Web
Imports System.web.UI.WebControls
Imports System.Text.RegularExpressions
Imports DotNetNuke
Imports DotNetNuke.Entities.Users
Imports DotNetNuke.Security
Imports DotNetNuke.Entities.Portals
Imports DotNetNuke.Entities.Modules
Imports ICSharpCode.SharpZipLib.Zip
Imports ICSharpCode.SharpZipLib.Checksums
Imports ICSharpCode.SharpZipLib.GZip

Namespace DotNetNuke.Modules.DownloadSupport
    Public Class DownloadFunctions
        Public Function DownloadFile(ByVal FileID As Integer) As Integer
            Dim i, iExtension As Integer
            Dim iStart As Integer = 0
            Dim iEnd As Integer = 0
            Dim strDownloadURL As String = ""
            Dim strURLTarget As String = ""

            ' File System
            Dim _portalSettings As PortalSettings = CType(HttpContext.Current.Items("PortalSettings"), PortalSettings)
            Dim file As DotNetNuke.Services.FileSystem.FileInfo = Me.ConvertFileIDtoFile(_portalSettings.PortalId, FileID)

            If Not (file Is Nothing) Then
                Return StreamFile(file.PhysicalPath, file.FileName)
            Else
                Return 0
            End If
        End Function


        Public Function DownloadMultiFiles(ByRef FileIDList As ArrayList, ByVal ZipFileName As String) As Integer
            Dim i, iExtension As Integer
            Dim iStart As Integer = 0
            Dim iEnd As Integer = 0
            Dim strDownloadURL As String = ""
            Dim strURLTarget As String = ""
            Dim _portalSettings As PortalSettings = CType(HttpContext.Current.Items("PortalSettings"), PortalSettings)
            Dim now As String = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss")

            Dim objCrc32 As New Crc32()
            Dim zos As ZipOutputStream
            zos = New ZipOutputStream(System.IO.File.Create(_portalSettings.HomeDirectoryMapPath & ZipFileName & now & ".zip"))
            For Each FileID As Integer In FileIDList
                Dim file As DotNetNuke.Services.FileSystem.FileInfo = Me.ConvertFileIDtoFile(_portalSettings.PortalId, FileID)
                If Not (file Is Nothing) Then
                    Dim strmFile As FileStream = System.IO.File.OpenRead(file.PhysicalPath)
                    Dim abyBuffer(CInt(strmFile.Length - 1)) As Byte
                    strmFile.Read(abyBuffer, 0, abyBuffer.Length)
                    Dim objZipEntry As ZipEntry = New ZipEntry(file.FileName)
                    objZipEntry.DateTime = DateTime.Now
                    objZipEntry.Size = strmFile.Length
                    strmFile.Close()
                    objCrc32.Reset()
                    objCrc32.Update(abyBuffer)
                    objZipEntry.Crc = objCrc32.Value
                    zos.PutNextEntry(objZipEntry)
                    zos.Write(abyBuffer, 0, abyBuffer.Length)
                End If
            Next
            zos.Finish()
            zos.Close()

            Dim res As Integer = StreamFile(_portalSettings.HomeDirectoryMapPath & ZipFileName & now & ".zip", ZipFileName & now & ".zip")
            System.IO.File.Delete(_portalSettings.HomeDirectoryMapPath & ZipFileName & now & ".zip")
            Return res
        End Function

        Private Function StreamFile(ByVal FilePath As String, ByVal DownloadAs As String) As Integer

            DownloadAs = DownloadAs.Replace(" ", "_")

            Dim objFile As New System.IO.FileInfo(FilePath)
            If Not objFile.Exists Then
                Return 0
            End If
            Dim objResponse As System.Web.HttpResponse = System.Web.HttpContext.Current.Response
            objResponse.ClearContent()

            objResponse.ClearHeaders()
            objResponse.AppendHeader("Content-Disposition", "attachment; filename=" & DownloadAs)
            objResponse.AppendHeader("Content-Length", objFile.Length.ToString())

            Dim strContentType As String
            Select Case objFile.Extension
                Case ".txt" : strContentType = "text/plain"
                Case ".htm", ".html" : strContentType = "text/html"
                Case ".rtf" : strContentType = "text/richtext"
                Case ".jpg", ".jpeg" : strContentType = "image/jpeg"
                Case ".gif" : strContentType = "image/gif"
                Case ".bmp" : strContentType = "image/bmp"
                Case ".mpg", ".mpeg" : strContentType = "video/mpeg"
                Case ".avi" : strContentType = "video/avi"
                Case ".pdf" : strContentType = "application/pdf"
                Case ".doc", ".dot" : strContentType = "application/msword"
                Case ".csv", ".xls", ".xlt" : strContentType = "application/vnd.msexcel"
                Case Else : strContentType = "application/octet-stream"
            End Select
            objResponse.ContentType = strContentType
            WriteFile(objFile.FullName)

            objResponse.Flush()
            objResponse.Close()
            Return 1
        End Function

        Public Shared Sub WriteFile(ByVal strFileName As String)
            Dim objResponse As System.Web.HttpResponse = System.Web.HttpContext.Current.Response
            Dim objStream As System.IO.Stream = Nothing

            ' Buffer to read 10K bytes in chunk:
            Dim bytBuffer(10000) As Byte

            ' Length of the file:
            Dim intLength As Integer

            ' Total bytes to read:
            Dim lngDataToRead As Long

            Try
                ' Open the file.
                objStream = New System.IO.FileStream(strFileName, System.IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.Read)

                ' Total bytes to read:
                lngDataToRead = objStream.Length

                objResponse.ContentType = "application/octet-stream"

                ' Read the bytes.
                While lngDataToRead > 0
                    ' Verify that the client is connected.
                    If objResponse.IsClientConnected Then
                        ' Read the data in buffer
                        intLength = objStream.Read(bytBuffer, 0, 10000)

                        ' Write the data to the current output stream.
                        objResponse.OutputStream.Write(bytBuffer, 0, intLength)

                        ' Flush the data to the HTML output.
                        objResponse.Flush()

                        ReDim bytBuffer(10000)       ' Clear the buffer
                        lngDataToRead = lngDataToRead - intLength
                    Else
                        'prevent infinite loop if user disconnects
                        lngDataToRead = -1
                    End If
                End While

            Catch ex As Exception
                ' Trap the error, if any.
                objResponse.Write("Error : " & ex.Message)
            Finally
                If IsNothing(objStream) = False Then
                    ' Close the file.
                    objStream.Close()
                End If
            End Try
        End Sub

        Public Function ConvertFileIDtoPath(ByVal pid As Integer, ByVal fid As Integer) As String
            Dim fc As New DotNetNuke.Services.FileSystem.FileController
            Dim file As DotNetNuke.Services.FileSystem.FileInfo = fc.GetFileById(fid, pid)
            Return file.PhysicalPath
        End Function

        Public Function ConvertFileIDtoFileName(ByVal pid As Integer, ByVal fid As Integer) As String
            Dim fc As New DotNetNuke.Services.FileSystem.FileController
            Dim file As DotNetNuke.Services.FileSystem.FileInfo = fc.GetFileById(fid, pid)
            Return file.FileName
        End Function

        Public Function ConvertFileIDtoExtension(ByVal pid As Integer, ByVal fid As Integer) As String
            Dim fc As New DotNetNuke.Services.FileSystem.FileController
            Dim file As DotNetNuke.Services.FileSystem.FileInfo = fc.GetFileById(fid, pid)
            Return file.Extension
        End Function

        Public Function ConvertFileIDtoFile(ByVal pid As Integer, ByVal fid As Integer) As DotNetNuke.Services.FileSystem.FileInfo
            Dim fc As New DotNetNuke.Services.FileSystem.FileController
            Dim file As DotNetNuke.Services.FileSystem.FileInfo = fc.GetFileById(fid, pid)
            Return file
        End Function

        'Public Function ExtractFileName(ByVal filename As String) As String
        '    Dim i, iExtension, iEnd, iStart As Integer
        '    Dim s As String = filename
        '    Dim firstDot As Integer = -1
        '    Dim secondDot As Integer = -1

        '    ' if the item has a GIUD, then there will be exactly 37 characters between the last two periods
        '    iExtension = s.LastIndexOf(".")
        '    iEnd = iExtension - 1
        '    i = iEnd
        '    While i > 0 And iStart = 0
        '        If s.Substring(i, 1) = "." Then
        '            iStart = i
        '        End If
        '        i -= 1
        '    End While

        '    If iExtension - iStart = 37 Then
        '        s = s.Substring(0, iStart) & s.Substring(iExtension)
        '    End If
        '    Return s
        'End Function
    End Class
End Namespace