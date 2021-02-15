﻿Imports System.IO
Module ConvertPackModule
    Public Sub ConvertPack(Source As String, Destination As String, Pack As String, CreateLog As Boolean, LogFileName As String, Indent As String, CleanUp As Boolean)
        'Initialize some variables.
        Dim UnpackFolder As String = Destination & "\Unpacked Assets"
        Dim ConvertFolder As String = Destination & "\Converted Folders"
        Dim RepackFolder As String = Destination & "\Converted Packs"
        Dim SourcePack As String
        Dim DestinationPack As String
        Dim PackBaseName As String
        Dim RepackSource As String
        Dim Message As String

        'Get the name of the pack file without the full path and without the .dungeondraft_pack extension.
        PackBaseName = Path.GetFileNameWithoutExtension(Pack)

        SourcePack = Source & "\" & Pack
        DestinationPack = Pack.Replace(".dungeondraft_pack", " (webp).dungeondraft_pack")
        DestinationPack = RepackFolder & "\" & DestinationPack
        If My.Computer.FileSystem.FileExists(DestinationPack) Then
            Message = Indent & "Pack file already exists in destination: " & DestinationPack & vbCrLf
            OutputForm.OutputTextBox.AppendText(Message)
            If CreateLog Then My.Computer.FileSystem.WriteAllText(LogFileName, Message, True)
        Else
            Message = Indent & "Unpacking from: " & SourcePack & vbCrLf
            OutputForm.OutputTextBox.AppendText(Message)
            If CreateLog Then My.Computer.FileSystem.WriteAllText(LogFileName, Message, True)
            Message = Indent & "            to: " & DestinationPack & vbCrLf
            OutputForm.OutputTextBox.AppendText(Message)
            If CreateLog Then My.Computer.FileSystem.WriteAllText(LogFileName, Message, True)

            UnpackAssets(SourcePack, UnpackFolder, CreateLog, LogFileName, Indent)

            RepackSource = ConvertFolder & "\" & PackBaseName & " (webp)"
            ConvertAssets(UnpackFolder & "\" & PackBaseName, RepackSource, CreateLog, LogFileName, Indent)

            'DestinationPack = RepackFolder

            Message = Indent & "Repacking from: " & RepackSource & vbCrLf
            OutputForm.OutputTextBox.AppendText(Message)
            If CreateLog Then My.Computer.FileSystem.WriteAllText(LogFileName, Message, True)
            Message = Indent & "            to: " & RepackFolder & vbCrLf
            OutputForm.OutputTextBox.AppendText(Message)
            If CreateLog Then My.Computer.FileSystem.WriteAllText(LogFileName, Message, True)

            CreatePack(RepackSource, RepackFolder, CreateLog, LogFileName, Indent)
        End If

        If CleanUp Then
            If My.Computer.FileSystem.DirectoryExists(UnpackFolder) Then
                Message = Indent & "Removing working folder: " & UnpackFolder & vbCrLf
                OutputForm.OutputTextBox.AppendText(Message)
                If CreateLog Then My.Computer.FileSystem.WriteAllText(LogFileName, Message, True)
                My.Computer.FileSystem.DeleteDirectory(UnpackFolder, FileIO.DeleteDirectoryOption.DeleteAllContents)
            End If

            If My.Computer.FileSystem.DirectoryExists(ConvertFolder) Then
                Message = Indent & "Removing working folder: " & ConvertFolder & vbCrLf
                OutputForm.OutputTextBox.AppendText(Message)
                If CreateLog Then My.Computer.FileSystem.WriteAllText(LogFileName, Message, True)
                My.Computer.FileSystem.DeleteDirectory(ConvertFolder, FileIO.DeleteDirectoryOption.DeleteAllContents)
            End If
        Else
            If My.Computer.FileSystem.DirectoryExists(UnpackFolder) Then
                Message = Indent & "Leaving working folder in place: " & UnpackFolder & vbCrLf
                OutputForm.OutputTextBox.AppendText(Message)
                If CreateLog Then My.Computer.FileSystem.WriteAllText(LogFileName, Message, True)
            End If

            If My.Computer.FileSystem.DirectoryExists(ConvertFolder) Then
                Message = Indent & "Leaving working folder in place: " & ConvertFolder & vbCrLf
                OutputForm.OutputTextBox.AppendText(Message)
                If CreateLog Then My.Computer.FileSystem.WriteAllText(LogFileName, Message, True)
            End If
        End If
    End Sub
End Module
