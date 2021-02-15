Imports System.IO
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Module PackAssetsModule
    Private Class FileObject
        Public Property SourcePath As String
        Public Property PathLength As Int32
        Public Property PackPath As String
        Public Property Offset As Int64
        Public Property Size As Int64
        Public Property MD5() As Byte() = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
    End Class

    Private Class VersionObject
        Public Property Version As Int32 = 1
        Public Property Major As Int32 = 3
        Public Property Minor As Int32 = 2
        Public Property Revision As Int32 = 1
    End Class

    Private Class PackJSONObject
        Public Property Name As String
        Public Property ID As String
        Public Property Version As String
        Public Property Author As String
    End Class

    Private Class PackObject
        Public Property MAGIC As Integer = &H43504447
        Public Property GodotVersion As New VersionObject
        Public Property Reserved() As Int32() = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        Public Property FileCount As Int32
        Public Property FileList As New List(Of FileObject)
        Public Property JSON As New PackJSONObject
        Public Property Path As String = "res://packs/"
        Public Property Overwrite As Boolean
    End Class

    Private Function GetPackJSON(PackJSONPath As String)
        Dim rawJson = File.ReadAllText(PackJSONPath)
        Dim PackJSONInfo = JsonConvert.DeserializeObject(rawJson)
        Return PackJSONInfo
    End Function

    Private Sub WritePackJSON(PackJSONObject, PackJSONPath)
        Dim NewPackJSON As String = JsonConvert.SerializeObject(PackJSONObject, Formatting.Indented)
        My.Computer.FileSystem.WriteAllText(PackJSONPath, NewPackJSON, False, System.Text.Encoding.ASCII)
    End Sub

    Public Sub LoadAssetFolders(Source As String, CreateLog As Boolean, LogFileName As String, SelectFolder As Boolean, Indent As String)

        'Initialize the columns for DDToolsForm.PackAssetsDataGridView
        Dim SelectColumn As New DataGridViewCheckBoxColumn With {
            .HeaderText = "Select Folder",
            .Name = "SelectFolder"
        }
        DDToolsForm.PackAssetsDataGridView.Columns.Add(SelectColumn)

        Dim FolderColumn As New DataGridViewTextBoxColumn With {
            .HeaderText = "Folder Name",
            .Name = "FolderName",
            .Width = 199,
            .ReadOnly = True
        }
        DDToolsForm.PackAssetsDataGridView.Columns.Add(FolderColumn)

        Dim NameColumn As New DataGridViewTextBoxColumn With {
            .HeaderText = "Pack Name",
            .Name = "PackName",
            .Width = 199
        }
        DDToolsForm.PackAssetsDataGridView.Columns.Add(NameColumn)

        Dim IDColumn As New DataGridViewTextBoxColumn With {
            .HeaderText = "Pack ID",
            .Name = "PackID",
            .ReadOnly = True
        }
        DDToolsForm.PackAssetsDataGridView.Columns.Add(IDColumn)
        DDToolsForm.PackAssetsDataGridView.Columns("PackID").DefaultCellStyle.Font = New Font("Courier New", 10, FontStyle.Regular)

        Dim VersionColumn As New DataGridViewTextBoxColumn With {
            .HeaderText = "Version",
            .Name = "PackVersion",
            .Width = 70
        }
        DDToolsForm.PackAssetsDataGridView.Columns.Add(VersionColumn)

        Dim AuthorColumn As New DataGridViewTextBoxColumn With {
            .HeaderText = "Author",
            .Name = "PackAuthor",
            .Width = 130
        }
        DDToolsForm.PackAssetsDataGridView.Columns.Add(AuthorColumn)

        'Get the list of folders in the Source folder, then iterate through them.
        Dim Folders = My.Computer.FileSystem.GetDirectories(Source)
        Dim PackInfo
        Dim RowIndex As Integer = 0
        For Each AssetFolder As String In Folders
            Dim FolderName As String = (My.Computer.FileSystem.GetDirectoryInfo(AssetFolder)).Name
            'If the current folder has a "textures" subfolder, then assume it's a pack folder.
            'Get the pack.json info if it's there, but whether it is or not, add the folder to the DataGridView.
            'If there is no pack.json file, then set the folder name as the pack name, and set the version as "1".
            If My.Computer.FileSystem.DirectoryExists(AssetFolder & "\textures") Then
                If My.Computer.FileSystem.FileExists(AssetFolder & "\pack.json") Then
                    Dim rawJson = File.ReadAllText(AssetFolder & "\pack.json")
                    PackInfo = JsonConvert.DeserializeObject(rawJson)
                    Dim NewRow As Object() = New Object() {SelectFolder, FolderName, PackInfo("name"), PackInfo("id"), PackInfo("version"), PackInfo("author")}
                    DDToolsForm.PackAssetsDataGridView.Rows.Add(NewRow)
                Else
                    Dim NewRow As Object() = New Object() {SelectFolder, FolderName, FolderName, "", "1", ""}
                    DDToolsForm.PackAssetsDataGridView.Rows.Add(NewRow)
                End If
                RowIndex += 1
            End If
        Next
        DDToolsForm.PackAssetsDataGridView.AllowUserToResizeColumns = False
    End Sub

    Private Function GenerateID() As String
        Dim CharSet As String = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"
        Dim ID As String = ""
        Dim Low As Integer = 0
        Dim High As Integer = CharSet.Length
        Dim pos As Integer
        Randomize()
        For IDLength = 1 To 8
            pos = CInt(Int((High * Rnd()) + Low))
            ID &= CharSet.Substring(pos, 1)
        Next
        Return ID
    End Function

    Public Sub PackAssets(Source As String, Destination As String, RowIndex As Integer, CreateLog As Boolean, LogFileName As String, Indent As String)
        'Initialize some variables.
        Dim PackJSON As String = Source & "\pack.json"
        Dim JSONExists As Boolean = False
        Dim PackInfo

        Dim OriginalPackName As String
        Dim OriginalPackID As String
        Dim OriginalPackVersion As String
        Dim OriginalPackAuthor As String

        Dim NewPackName As String
        Dim NewPackID As String
        Dim NewPackVersion As String
        Dim NewPackAuthor As String

        Dim Message As String

        'Get the current, user-editable values for creating the pack.
        NewPackName = DDToolsForm.PackAssetsDataGridView.Rows(RowIndex).Cells("PackName").Value
        NewPackID = DDToolsForm.PackAssetsDataGridView.Rows(RowIndex).Cells("PackID").Value
        NewPackVersion = DDToolsForm.PackAssetsDataGridView.Rows(RowIndex).Cells("PackVersion").Value
        NewPackAuthor = DDToolsForm.PackAssetsDataGridView.Rows(RowIndex).Cells("PackAuthor").Value

        If My.Computer.FileSystem.FileExists(PackJSON) Then
            'If there's an existing pack.json file, we want to proceed differently than if there isn't.
            JSONExists = True

            'Load the pack.json file into PackInfo
            PackInfo = GetPackJSON(PackJSON)

            'Store the values from the pack.json file so we can later determine if the user has edited values.
            OriginalPackName = PackInfo("name")
            OriginalPackID = PackInfo("id")
            OriginalPackVersion = PackInfo("version")
            OriginalPackAuthor = PackInfo("author")

            If NewPackName = OriginalPackName And NewPackVersion = OriginalPackVersion And NewPackAuthor = OriginalPackAuthor Then
                'If no values have been edited, then create the pack with the minimum arguments.
                If OriginalPackName = "" Or OriginalPackID = "" Or OriginalPackVersion = "" Then
                    Message = Indent & "Pack Name, Pack ID and Version must all have values." & vbCrLf
                    Message &= Indent & "Skipping folder." & vbCrLf
                    OutputForm.OutputTextBox.AppendText(Message)
                    If CreateLog Then My.Computer.FileSystem.WriteAllText(LogFileName, Message, True)
                    Exit Sub
                Else
                    Message = Indent & "Original metadata." & vbCrLf
                    Message &= Indent & Indent & "Pack Name: " & OriginalPackName & vbCrLf
                    Message &= Indent & Indent & "Pack ID: " & OriginalPackID & vbCrLf
                    Message &= Indent & Indent & "Pack Version: " & OriginalPackVersion & vbCrLf
                    Message &= Indent & Indent & "Pack Author: " & OriginalPackAuthor & vbCrLf & vbCrLf
                    Message &= Indent & "Pack will be created with original metadata." & vbCrLf & vbCrLf
                    OutputForm.OutputTextBox.AppendText(Message)
                    If CreateLog Then My.Computer.FileSystem.WriteAllText(LogFileName, Message, True)
                End If
            ElseIf NewPackName = OriginalPackName And (NewPackVersion <> OriginalPackVersion Or NewPackAuthor <> OriginalPackAuthor) Then
                'If other values besides the name have been edited, then we want to add arguments to update values in the existing pack.json file.
                'This will generate a new pack ID, but since the name hasn't changed, we want to preserve the original pack ID.
                'So we're not going to create the pack yet. We just want to update the pack.json file.

                If OriginalPackName = "" Or NewPackID = "" Or NewPackVersion = "" Then
                    Message = Indent & "Pack Name, Pack ID and Version must all have values." & vbCrLf
                    Message &= Indent & "Skipping folder." & vbCrLf
                    OutputForm.OutputTextBox.AppendText(Message)
                    If CreateLog Then My.Computer.FileSystem.WriteAllText(LogFileName, Message, True)
                    Exit Sub
                Else
                    Dim JSON As New JObject
                    JSON("name") = OriginalPackName
                    JSON("id") = NewPackID
                    JSON("version") = NewPackVersion
                    JSON("author") = NewPackAuthor
                    WritePackJSON(JSON, PackJSON)

                    Message = Indent & "Original metadata." & vbCrLf
                    Message &= Indent & Indent & "Pack Name: " & OriginalPackName & vbCrLf
                    Message &= Indent & Indent & "Pack ID: " & OriginalPackID & vbCrLf
                    Message &= Indent & Indent & "Pack Version: " & OriginalPackVersion & vbCrLf
                    Message &= Indent & Indent & "Pack Author: " & OriginalPackAuthor & vbCrLf & vbCrLf

                    Message &= Indent & "Pack will be created with new metadata." & vbCrLf
                    Message &= Indent & Indent & "Pack Name: " & OriginalPackName & vbCrLf
                    Message &= Indent & Indent & "Pack ID: " & NewPackID & vbCrLf
                    Message &= Indent & Indent & "Pack Version: " & NewPackVersion & vbCrLf
                    Message &= Indent & Indent & "Pack Author: " & NewPackAuthor & vbCrLf & vbCrLf
                    OutputForm.OutputTextBox.AppendText(Message)
                    If CreateLog Then My.Computer.FileSystem.WriteAllText(LogFileName, Message, True)
                End If
            ElseIf NewPackName <> OriginalPackName Then
                'If the name has been edited, regardless of whether or not other values have been edited, we will treat this as the creation of a new pack.
                NewPackID = GenerateID()
                If NewPackName = "" Or NewPackID = "" Or NewPackVersion = "" Then
                    Message = Indent & "Pack Name, Pack ID and Version must all have values." & vbCrLf
                    Message &= Indent & "Skipping folder." & vbCrLf
                    OutputForm.OutputTextBox.AppendText(Message)
                    If CreateLog Then My.Computer.FileSystem.WriteAllText(LogFileName, Message, True)
                    Exit Sub
                Else
                    Dim JSON As New JObject
                    JSON("name") = NewPackName
                    JSON("id") = NewPackID
                    JSON("version") = NewPackVersion
                    JSON("author") = NewPackAuthor
                    WritePackJSON(JSON, PackJSON)

                    Message = Indent & "Original metadata." & vbCrLf
                    Message &= Indent & Indent & "Pack Name: " & OriginalPackName & vbCrLf
                    Message &= Indent & Indent & "Pack ID: " & OriginalPackID & vbCrLf
                    Message &= Indent & Indent & "Pack Version: " & OriginalPackVersion & vbCrLf
                    Message &= Indent & Indent & "Pack Author: " & OriginalPackAuthor & vbCrLf & vbCrLf

                    Message &= Indent & "Pack will be created with new metadata." & vbCrLf
                    Message &= Indent & Indent & "Pack Name: " & NewPackName & vbCrLf
                    Message &= Indent & Indent & "Pack ID: " & NewPackID & vbCrLf
                    Message &= Indent & Indent & "Pack Version: " & NewPackVersion & vbCrLf
                    Message &= Indent & Indent & "Pack Author: " & NewPackAuthor & vbCrLf & vbCrLf
                    OutputForm.OutputTextBox.AppendText(Message)
                    If CreateLog Then My.Computer.FileSystem.WriteAllText(LogFileName, Message, True)
                End If
            End If
        Else
            'If there is no existing pack.json file, we will treat this as the creation of a new pack.
            NewPackID = GenerateID()
            If NewPackName = "" Or NewPackID = "" Or NewPackVersion = "" Then
                Message = Indent & "Pack Name, Pack ID and Version must all have values." & vbCrLf
                Message &= Indent & "Skipping folder." & vbCrLf
                OutputForm.OutputTextBox.AppendText(Message)
                If CreateLog Then My.Computer.FileSystem.WriteAllText(LogFileName, Message, True)
                Exit Sub
            Else
                Dim JSON As New JObject
                JSON("name") = NewPackName
                JSON("id") = NewPackID
                JSON("version") = NewPackVersion
                JSON("author") = NewPackAuthor
                WritePackJSON(JSON, PackJSON)

                Message = Indent & "Original metadata." & vbCrLf
                Message &= Indent & Indent & "none" & vbCrLf

                Message &= Indent & "Pack will be created with new metadata." & vbCrLf
                Message &= Indent & Indent & "Pack Name: " & NewPackName & vbCrLf
                Message &= Indent & Indent & "Pack ID: " & NewPackID & vbCrLf
                Message &= Indent & Indent & "Pack Version: " & NewPackVersion & vbCrLf
                Message &= Indent & Indent & "Pack Author: " & NewPackAuthor & vbCrLf & vbCrLf
                OutputForm.OutputTextBox.AppendText(Message)
                If CreateLog Then My.Computer.FileSystem.WriteAllText(LogFileName, Message, True)
            End If
        End If
        CreatePack(Source, Destination, CreateLog, LogFileName, Indent)
    End Sub

    Public Sub CreatePack(ByVal packfoldername As String, ByVal destination As String, CreateLog As Boolean, LogFileName As String, Indent As String)
        Dim FileTypes() As String = {".jpg", ".jpeg", ".png", ".webp", ".dungeondraft_wall", ".dungeondraft_tileset", ".dungeondraft_tags", ".json"}

        Dim PackJSONPath As String = packfoldername & "\pack.json"
        Dim PackMeta As New PackObject
        Dim Message As String

        If File.Exists(PackJSONPath) Then
            Dim PackJSONInfo = GetPackJSON(PackJSONPath)
            PackMeta.JSON.Name = PackJSONInfo("name")
            PackMeta.JSON.ID = PackJSONInfo("id")
            PackMeta.JSON.Version = PackJSONInfo("version")
            PackMeta.JSON.Author = PackJSONInfo("author")
        Else
            Message = Indent & "File not found: " & PackJSONPath
            OutputForm.OutputTextBox.AppendText(Message)
            If CreateLog Then My.Computer.FileSystem.WriteAllText(LogFileName, Message, True)
            Exit Sub
        End If

        Dim DataFileList As IEnumerable(Of String) = Nothing
        If Directory.Exists(packfoldername & "\data") Then
            DataFileList = From File In Directory.GetFiles(packfoldername & "\data", "*.*", SearchOption.AllDirectories)
                           Where FileTypes.Contains(My.Computer.FileSystem.GetFileInfo(File).Extension.ToLower())
        End If

        Dim FileList = From File In Directory.GetFiles(packfoldername, "*.*", SearchOption.AllDirectories)
                       Where FileTypes.Contains(My.Computer.FileSystem.GetFileInfo(File).Extension.ToLower()) _
                           And Not File.Contains("\data\")

        Dim UnsupportedFileList = From File In Directory.GetFiles(packfoldername, "*.*", SearchOption.AllDirectories)
                                  Where Not FileTypes.Contains(My.Computer.FileSystem.GetFileInfo(File).Extension.ToLower())

        If UnsupportedFileList.Count > 0 Then
            Message = Indent & "Skipping files of unsupported types:" & vbCrLf
            OutputForm.OutputTextBox.AppendText(Message)
            If CreateLog Then My.Computer.FileSystem.WriteAllText(LogFileName, Message, True)

            For Each unsupportedfile In UnsupportedFileList
                Message = Indent & Indent & unsupportedfile & vbCrLf
                OutputForm.OutputTextBox.AppendText(Message)
                If CreateLog Then My.Computer.FileSystem.WriteAllText(LogFileName, Message, True)
            Next
            OutputForm.OutputTextBox.AppendText(Message)
            If CreateLog Then My.Computer.FileSystem.WriteAllText(LogFileName, Message, True)
        End If

        Dim Offset As Int64 = 0
        Dim JSONFile As New FileObject
        JSONFile.SourcePath = packfoldername & "\pack.json"
        JSONFile.PackPath = PackMeta.Path & PackMeta.JSON.ID & ".json"
        JSONFile.PathLength = JSONFile.PackPath.Length
        JSONFile.Size = My.Computer.FileSystem.GetFileInfo(JSONFile.SourcePath).Length
        JSONFile.Offset = Offset
        PackMeta.FileList.Add(JSONFile)
        Offset += JSONFile.Size

        If Not DataFileList Is Nothing Then
            For Each File In DataFileList
                Dim NewFile As New FileObject
                NewFile.SourcePath = File
                NewFile.PackPath = PackMeta.Path & File.Replace(packfoldername, PackMeta.JSON.ID).Replace("\", "/")
                NewFile.PathLength = NewFile.PackPath.Length
                NewFile.Size = My.Computer.FileSystem.GetFileInfo(File).Length
                NewFile.Offset = Offset
                PackMeta.FileList.Add(NewFile)
                Offset += NewFile.Size
            Next
        End If

        For Each File In FileList
            Dim NewFile As New FileObject
            NewFile.SourcePath = File
            NewFile.PackPath = PackMeta.Path & File.Replace(packfoldername, PackMeta.JSON.ID).Replace("\", "/")
            NewFile.PathLength = NewFile.PackPath.Length
            NewFile.Size = My.Computer.FileSystem.GetFileInfo(File).Length
            NewFile.Offset = Offset
            PackMeta.FileList.Add(NewFile)
            Offset += NewFile.Size
        Next

        PackMeta.FileCount = PackMeta.FileList.Count

        Dim FirstOffset As Int64 = 4 'MAGIC as Integer
        FirstOffset += 16 'Version as 4 x Int32
        FirstOffset += 64 'Reserved as 16 x Int32
        FirstOffset += 4 'FileCount as Int32

        For Each File In PackMeta.FileList
            FirstOffset += 4 'String Length as Int32
            FirstOffset += File.PathLength
            FirstOffset += 8 'File offset as Int64
            FirstOffset += 8 'File size as Int64
            FirstOffset += 16 'MD5 as 16 bytes
        Next

        For Each File In PackMeta.FileList
            File.Offset += FirstOffset
        Next

        If Not Directory.Exists(destination) Then Directory.CreateDirectory(destination)
        Dim PackPath As String = destination & "\" & PackMeta.JSON.Name & ".dungeondraft_pack"
        Dim PackWriter As BinaryWriter
        PackWriter = New BinaryWriter(New FileStream(PackPath, FileMode.Create))

        PackWriter.Write(PackMeta.MAGIC)
        PackWriter.Write(PackMeta.GodotVersion.Version)
        PackWriter.Write(PackMeta.GodotVersion.Major)
        PackWriter.Write(PackMeta.GodotVersion.Minor)
        PackWriter.Write(PackMeta.GodotVersion.Revision)
        For Each ReservedByte In PackMeta.Reserved
            PackWriter.Write(ReservedByte)
        Next
        PackWriter.Write(PackMeta.FileCount)

        For Each File In PackMeta.FileList
            PackWriter.Write(File.PathLength)
            PackWriter.Write(File.PackPath.ToCharArray)
            PackWriter.Write(File.Offset)
            PackWriter.Write(File.Size)
            For Each MD5Byte In File.MD5
                PackWriter.Write(MD5Byte)
            Next
        Next

        For Each File In PackMeta.FileList
            Dim ByteReader() As Byte
            ByteReader = My.Computer.FileSystem.ReadAllBytes(File.SourcePath)
            PackWriter.Write(My.Computer.FileSystem.ReadAllBytes(File.SourcePath))
        Next

        PackWriter.Close()
        PackWriter.Dispose()
    End Sub
End Module
