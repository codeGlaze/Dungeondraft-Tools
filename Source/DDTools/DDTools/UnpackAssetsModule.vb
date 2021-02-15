Imports System.IO
Imports System.Text
Imports BioLib

Module UnpackAssetsModule
    Private Class FileEntry
        Public path As String
        Public offset As Long
        Public size As Long

        Public Sub New(ByVal foldername As String, ByVal path As String, ByVal offset As Long, ByVal size As Long)
            Dim pos As Integer
            path = path.Substring(6).TrimEnd(vbNullChar)
            path = path.Replace("packs/", "")
            If path.Contains("/") Then
                pos = path.IndexOf("/")
            Else
                pos = path.IndexOf(".")
            End If
            Dim PackID As String = path.Substring(0, pos)
            path = path.Replace(PackID, foldername)
            Me.path = path
            Me.offset = offset
            Me.size = size
        End Sub

        Public Sub Resize(ByVal by As Integer, ByVal Optional stripAtEnd As Integer = 0)
            offset += by
            size -= by + stripAtEnd
        End Sub

        Public Sub ChangeExtension(ByVal from As String, ByVal [to] As String)
            path = path.Replace(from, [to])
        End Sub

        Public Overrides Function ToString() As String
            Return $"{offset} {path}, {size}"
        End Function
    End Class

    Public Sub UnpackAssets(ByVal inputfile As String, ByVal outputdirectory As String, ByVal CreateLog As Boolean, ByVal LogFilename As String, ByVal Indent As String)
        Dim MAGIC As Integer = &H43504447
        Dim convertAssets As Boolean
        Dim foldername As String = Path.GetFileNameWithoutExtension(inputfile)
        Dim Message As String
        'Bio.Header("godotdec", "2.1.0", "2018-2020", "A simple unpacker for Godot Engine package files (.pck|.exe)", "[<options>] <input_file> [<output_directory>]" & vbLf & vbLf & "Options:" & vbLf & "-c" & vbTab & "--convert" & vbTab & "Convert textures and audio files")
        Dim failed = 0

        Using inputStream = New BinaryReader(File.Open(inputfile, FileMode.Open))
            Dim ValidPackage As Boolean = False

            If inputStream.ReadInt32() = MAGIC Then
                ValidPackage = True
            Else
                inputStream.BaseStream.Seek(-4, SeekOrigin.[End])
                If inputStream.ReadInt32() = MAGIC Then
                    ValidPackage = True
                Else
                    inputStream.BaseStream.Seek(-12, SeekOrigin.Current)
                    Dim offset = inputStream.ReadInt64()
                    inputStream.BaseStream.Seek(-offset - 8, SeekOrigin.Current)
                    If inputStream.ReadInt32() = MAGIC Then ValidPackage = True
                End If
            End If

            If ValidPackage Then
                Message = Indent & "The input file appears to be a valid Godot package file." & vbCrLf
                OutputForm.OutputTextBox.AppendText(Message)
                If CreateLog Then My.Computer.FileSystem.WriteAllText(LogFilename, Message, True)
            Else
                Message = Indent & "The input file appears to be an invalid Godot package file." & vbCrLf
                Message &= Indent & "Skipping file." & vbCrLf
                OutputForm.OutputTextBox.AppendText(Message)
                If CreateLog Then My.Computer.FileSystem.WriteAllText(LogFilename, Message, True)
                Exit Sub
            End If

            Message = Indent & ($"Godot Engine version: {inputStream.ReadInt32()}.{inputStream.ReadInt32()}.{inputStream.ReadInt32()}.{inputStream.ReadInt32()}") & vbCrLf
            OutputForm.OutputTextBox.AppendText(Message)
            If CreateLog Then My.Computer.FileSystem.WriteAllText(LogFilename, Message, True)

            inputStream.BaseStream.Seek(16 * 4, SeekOrigin.Current)
            Dim fileCount = inputStream.ReadInt32()
            Message = Indent & ($"Found {fileCount} files in package") & vbCrLf
            Message &= Indent & "Reading file index" & vbCrLf
            OutputForm.OutputTextBox.AppendText(Message)
            If CreateLog Then My.Computer.FileSystem.WriteAllText(LogFilename, Message, True)
            Dim fileIndex = New List(Of FileEntry)()

            For i = 0 To fileCount - 1
                Dim pathLength = inputStream.ReadInt32()
                Dim path = Encoding.UTF8.GetString(inputStream.ReadBytes(pathLength))
                Dim fileEntry = New FileEntry(foldername, path.ToString(), inputStream.ReadInt64(), inputStream.ReadInt64())
                If fileEntry.path <> foldername & ".json" Then
                    fileIndex.Add(fileEntry)
                End If
                inputStream.BaseStream.Seek(16, SeekOrigin.Current)
            Next

            If fileIndex.Count < 1 Then
                Message = Indent & "No files were found inside the archive." & vbCrLf
                Exit Sub
            End If
            fileIndex.Sort(Function(a, b) CInt((a.offset - b.offset)))
            Dim fileIndexEnd = inputStream.BaseStream.Position

            For i = 0 To fileIndex.Count - 1
                Dim fileEntry = fileIndex(i)

                If fileEntry.offset < fileIndexEnd Then
                    Message = Indent & "Invalid file offset: " & fileEntry.offset & vbCrLf
                    OutputForm.OutputTextBox.AppendText(Message)
                    If CreateLog Then My.Computer.FileSystem.WriteAllText(LogFilename, Message, True)
                    Continue For
                End If

                If convertAssets Then

                    If fileEntry.path.EndsWith(".stex") AndAlso fileEntry.path.Contains(".png") Then
                        fileEntry.Resize(32)
                        fileEntry.ChangeExtension(".stex", ".png")
                    ElseIf fileEntry.path.EndsWith(".oggstr") Then
                        fileEntry.Resize(279, 4)
                        fileEntry.ChangeExtension(".oggstr", ".ogg")
                    ElseIf fileEntry.path.EndsWith(".sample") Then
                        'Bio.Warn("The file type '.sample' is currently not supported")
                    End If
                End If

                Dim destination = Path.Combine(outputdirectory, fileEntry.path)
                'destination = Bio.EnsureFileDoesNotExist(destination, "godotdec_overwrite")
                inputStream.BaseStream.Seek(fileEntry.offset, SeekOrigin.Begin)
                If destination Is Nothing Then Continue For

                Try
                    Dim fMode = FileMode.CreateNew
                    Directory.CreateDirectory(Path.GetDirectoryName(destination))

                    If File.Exists(destination) Then
                        fMode = FileMode.Create
                    End If

                    Using outputStream = New FileStream(destination, fMode)
                        BioLib.Streams.StreamExtensions.Copy(inputStream.BaseStream, outputStream, CInt(fileEntry.size))
                    End Using

                Catch e As Exception
                    failed += 1
                End Try
            Next
        End Using

        If failed < 1 Then
            Message = Indent & "All files successfully unpacked." & vbCrLf
        Else
            Message = Indent & failed & " files failed to extract."
        End If
        OutputForm.OutputTextBox.AppendText(Message)
        If CreateLog Then My.Computer.FileSystem.WriteAllText(LogFilename, Message, True)
    End Sub
End Module
