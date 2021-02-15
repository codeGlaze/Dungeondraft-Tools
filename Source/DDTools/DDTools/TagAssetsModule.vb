Imports System.IO
Imports Newtonsoft.Json
Module TagAssetsModule
    Public Sub TagAssetsSub(Source As String, AssetFolder As String, DefaultTag As String, CreateLog As Boolean, LogFileName As String, Indent As String)
        Dim TagsFilePath As String = Source & "\data"
        Dim TagsFile As String = TagsFilePath & "\default.dungeondraft_tags"
        Dim TexturePath As String
        Dim ObjectPath As String
        Dim ColorablePath As String
        Dim Message As String

        'Get the exact, case-sensitive path name for the textures folder.
        Dim TexturePathArray = From subfolder In My.Computer.FileSystem.GetDirectories(Source)
                               Where (My.Computer.FileSystem.GetDirectoryInfo(subfolder)).Name.ToLower() = "textures"
        For Each path As String In TexturePathArray
            TexturePath = path
        Next

        'Get the exact, case-sensitive path name for the textures\objects folder.
        Dim ObjectPathArray = From subfolder In My.Computer.FileSystem.GetDirectories(TexturePath)
                              Where (My.Computer.FileSystem.GetDirectoryInfo(subfolder)).Name.ToLower() = "objects"
        For Each path As String In ObjectPathArray
            ObjectPath = path
        Next

        'Get the exact, case-sensitive path name for the textures\objects\colorable folder.
        Dim ColorablePathArray = From subfolder In My.Computer.FileSystem.GetDirectories(ObjectPath)
                                 Where (My.Computer.FileSystem.GetDirectoryInfo(subfolder)).Name.ToLower() = "colorable"
        For Each path As String In ColorablePathArray
            ColorablePath = path
        Next

        'Set the values for the main keys that will appear in the JSON file.
        Dim tags As String = "tags"
        Dim colorable As String = "Colorable"
        Dim sets As String = "sets"

        'Set up ordered dictionaries that we can later serialize for later covnersion to JSON.
        Dim TagObject As New System.Collections.Specialized.OrderedDictionary
        Dim FolderObject As New System.Collections.Specialized.OrderedDictionary
        Dim ColorableObject As New System.Collections.Specialized.OrderedDictionary
        Dim SetObject As New System.Collections.Specialized.OrderedDictionary

        'Get a recursive list of subfolders in textures\objects.
        Message = Indent & "Getting subfolder names to use as tags." & vbCrLf
        OutputForm.OutputTextBox.AppendText(Message)
        If CreateLog Then My.Computer.FileSystem.WriteAllText(LogFileName, Message, True)
        Dim SubFolders As String() = Directory.GetDirectories(ObjectPath, "*.*", SearchOption.AllDirectories)

        'If a default tag has been specified, add that value to the folder list.
        If DefaultTag <> "" Then
            ReDim Preserve SubFolders(SubFolders.Count)
            SubFolders(SubFolders.Count - 1) = DefaultTag
        End If

        'For each subfolder, split the path into substrings, and take the last value as the folder name to use as the tag.
        For i = 0 To SubFolders.Count - 1
            If SubFolders(i).Split("\").Count <> 0 Then SubFolders(i) = SubFolders(i).Split("\")(SubFolders(i).Split("\").Count - 1)
        Next

        'Eliminate duplicate folder names.
        Dim UniqueFolders = SubFolders.Distinct().ToArray

        'Sort the list alphabetically.
        Array.Sort(UniqueFolders)

        'Add the list of folders to the tag set.
        Message = Indent & "Adding the list of folders to the tag set." & vbCrLf
        OutputForm.OutputTextBox.AppendText(Message)
        If CreateLog Then My.Computer.FileSystem.WriteAllText(LogFileName, Message, True)
        SetObject.Add(AssetFolder, UniqueFolders)

        'Get the list files from the root of textures\objects and textures\objects\colorable.
        Message = Indent & "Getting root objects and root colorable objects." & vbCrLf
        OutputForm.OutputTextBox.AppendText(Message)
        If CreateLog Then My.Computer.FileSystem.WriteAllText(LogFileName, Message, True)
        Dim RootFiles As String() = Directory.GetFiles(ObjectPath)
        Dim RootColorableFiles As String() = Directory.GetFiles(ColorablePath)

        ReDim Preserve RootFiles(RootFiles.Count + RootColorableFiles.Count - 1)
        RootColorableFiles.CopyTo(RootFiles, RootFiles.Count - RootColorableFiles.Count)

        'For each file, truncate the path up to "textures" so we're left with "textures\objects\<filename>".
        'Then replace the backslashes with forward slashes so we're left with "textures/objects/<filename>".
        For i = 0 To RootFiles.Count - 1
            RootFiles(i) = RootFiles(i).Replace(Source & "\", "")
            RootFiles(i) = RootFiles(i).Replace("\", "/")
        Next

        'Sort the list alphabetically
        Array.Sort(RootFiles)

        'Get a recursive list of all files in all subfolders under textures\objects.
        Message = Indent & "Getting all objects from subfolders." & vbCrLf
        OutputForm.OutputTextBox.AppendText(Message)
        If CreateLog Then My.Computer.FileSystem.WriteAllText(LogFileName, Message, True)
        Dim AllObjects As String() = Directory.GetFiles(ObjectPath, "*.*", SearchOption.AllDirectories)

        'For each file, truncate the path up to "textures" so we're left with "textures\objects\<path>\<filename>".
        'Then replace the backslashes with forward slashes so we're left with "textures/objects/<path>/<filename>".
        For i = 0 To AllObjects.Count - 1
            AllObjects(i) = AllObjects(i).Replace(Source & "\", "")
            AllObjects(i) = AllObjects(i).Replace("\", "/")
        Next

        'For each subfolder, add the folder and its associated files to the FolderObject ordered dictionary.
        'Each subfolder name is what will be considered a Tag, and each file contained within that subfolder 
        'will be associated with each of its parent folder names as tags.
        Message = Indent & "Adding the tags for the subfolders." & vbCrLf
        OutputForm.OutputTextBox.AppendText(Message)
        If CreateLog Then My.Computer.FileSystem.WriteAllText(LogFileName, Message, True)
        For Each folder As String In UniqueFolders
            Dim folderset As String() = Array.FindAll(AllObjects, Function(s) s.Contains("/" & folder & "/"))
            If DefaultTag = folder Then
                'If the default tag value also exists as a folder name, merge the two file lists into one.

                ReDim Preserve folderset(folderset.Count + RootFiles.Count - 1)
                RootFiles.CopyTo(folderset, folderset.Count - RootFiles.Count)

            End If
            Array.Sort(folderset)
            FolderObject.Add(folder, folderset)
        Next

        Message = Indent & "Creating the tag object." & vbCrLf
        OutputForm.OutputTextBox.AppendText(Message)
        If CreateLog Then My.Computer.FileSystem.WriteAllText(LogFileName, Message, True)
        'Add the FolderObject ordered dictionary to the TagObject ordered dictionary.
        TagObject.Add(tags, FolderObject)

        'Add the SetObject ordered dictionary to the TagObject ordered dictionary.
        TagObject.Add(sets, SetObject)

        Message = Indent & "Converting the tag object to JSON." & vbCrLf
        OutputForm.OutputTextBox.AppendText(Message)
        If CreateLog Then My.Computer.FileSystem.WriteAllText(LogFileName, Message, True)
        'Convert the TagObject ordered dictionary to a JSON-formatted string.
        Dim JSONString As String = JsonConvert.SerializeObject(TagObject, Formatting.Indented)

        Message = Indent & "Looking for the data folder, and creating it if it doesn't exist." & vbCrLf
        OutputForm.OutputTextBox.AppendText(Message)
        If CreateLog Then My.Computer.FileSystem.WriteAllText(LogFileName, Message, True)
        'Check for a data folder, and if it doesn't exist, create it.
        If Not My.Computer.FileSystem.DirectoryExists(TagsFilePath) Then
            My.Computer.FileSystem.CreateDirectory(TagsFilePath)
        End If

        Message = Indent & "Writing the tag file to " & TagsFile & vbCrLf
        OutputForm.OutputTextBox.AppendText(Message)
        If CreateLog Then My.Computer.FileSystem.WriteAllText(LogFileName, Message, True)
        'Write the JSON string as a default.dungeondraft_tags file to the data folder.
        My.Computer.FileSystem.WriteAllText(TagsFile, JSONString, False, System.Text.Encoding.ASCII)
    End Sub
End Module
