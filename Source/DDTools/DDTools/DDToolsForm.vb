﻿Imports System.IO
Imports Newtonsoft.Json
Imports System.Environment
Public Class DDToolsForm
    Private Sub DDToolsForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        PreferencesToolStripMenu.Visible = False
        TitlePanel.Hide()
        TagAssetsGroupBox.Hide()
        ColorThemeApplyGroupBox.Hide()
        ColorThemeCreateGroupBox.Hide()
        ConvertAssetsGroupBox.Hide()
        ConvertPacksGroupBox.Hide()
        CopyAssetsGroupBox.Hide()
        CopyTilesGroupBox.Hide()
        DataFilesGroupBox.Hide()
        MapDetailsGroupBox.Hide()
        PackAssetsGroupBox.Hide()
        UnpackAssetsGroupBox.Hide()

        Dim ConfigFileName As String = GlobalVariables.ConfigFolderName & "\" & GlobalVariables.ConfigFileName

        If File.Exists(ConfigFileName) Then
            Dim ConfigObject = GetSavedConfig(ConfigFileName)
            GetSavedConfig(ConfigFileName)

            Dim ActiveTool = "global_config"
            If Not ConfigObject(ActiveTool) Is Nothing Then
                Dim LocationPref As New Point
                LocationPref.X = ConfigObject(ActiveTool)("location_x")
                LocationPref.Y = ConfigObject(ActiveTool)("location_y")
                Me.Location = LocationPref
            End If
        End If

        Me.Text = "EightBitz's DDTools - Version " + My.Application.Info.Version.ToString

        VersionLabel.Text = "Version " & My.Application.Info.Version.ToString
        GitHubLinkLabel.Text = "The latest version of this program can be found in its GitHub repository."
        GitHubLinkLabel.Links.Add(55, 17, "https://github.com/EightBitz/Dungeondraft-Tools")
        CreativeCommonsLinkLabel.Text = "This work is licensed under a Creative Commons Attribution-NonCommercial 4.0 International License."
        CreativeCommonsLinkLabel.Links.Add(30, 68, "https://creativecommons.org/licenses/by-nc/4.0/legalcode")
        EmailLinkLabel.Text = "Email: eightbitz73@outlook.com"
        EmailLinkLabel.Links.Add(7, 23, "mailto:eightbitz73@outlook.com")

        If Not Directory.Exists(GlobalVariables.LogsFolder) Then Directory.CreateDirectory(GlobalVariables.LogsFolder)
        Me.MinimumSize = New Size(1000, 441)
        Me.MaximumSize = New Size(1000, 441)
        TitlePanel.BringToFront()
        TitlePanel.Show()
        Me.Size = New Size(1000, 441)
    End Sub

    Private Sub AboutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AboutToolStripMenuItem.Click
        AboutBox1.Show()
    End Sub

    Private Sub GitHubLinkLabel_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles GitHubLinkLabel.LinkClicked
        System.Diagnostics.Process.Start(e.Link.LinkData)
    End Sub

    Private Sub CreativeCommonsLinkLabel_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles CreativeCommonsLinkLabel.LinkClicked
        System.Diagnostics.Process.Start(e.Link.LinkData)
    End Sub

    Private Sub EmailLinkLabel_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles EmailLinkLabel.LinkClicked
        System.Diagnostics.Process.Start(e.Link.LinkData)
    End Sub

    Private Sub DocumentationMenuItem_Click(sender As Object, e As EventArgs) Handles DocumentationMenuItem.Click
        Dim DocFile As String = My.Application.Info.DirectoryPath & "\Documentation\EightBitz's Dungeondraft Tools Documentation.pdf"
        System.Diagnostics.Process.Start(DocFile)
    End Sub

    Private Sub LicenseMenuItem_Click(sender As Object, e As EventArgs) Handles LicenseMenuItem.Click
        Dim LicenseFile As String = My.Application.Info.DirectoryPath & "\Documentation\LICENSE.html"
        System.Diagnostics.Process.Start(LicenseFile)
    End Sub

    Private Sub READMEMenuItem_Click(sender As Object, e As EventArgs) Handles READMEMenuItem.Click
        Dim READMEFile As String = My.Application.Info.DirectoryPath & "\Documentation\README.html"
        System.Diagnostics.Process.Start(READMEFile)
    End Sub

    '###### Main Menu Items ######
    Private Sub TagAssetsMenuItem_Click(sender As Object, e As EventArgs) Handles TagAssetsMenuItem.Click
        TitlePanel.Hide()
        TagAssetsGroupBox.Hide()
        ColorThemeApplyGroupBox.Hide()
        ColorThemeCreateGroupBox.Hide()
        ConvertAssetsGroupBox.Hide()
        ConvertPacksGroupBox.Hide()
        CopyAssetsGroupBox.Hide()
        CopyTilesGroupBox.Hide()
        DataFilesGroupBox.Hide()
        MapDetailsGroupBox.Hide()
        PackAssetsGroupBox.Hide()
        UnpackAssetsGroupBox.Hide()

        TagAssetsGroupBox.BringToFront()
        TagAssetsGroupBox.Show()

        Me.MinimumSize = New Size(802, 653)
        Me.MaximumSize = New Size(0, 0)
        PreferencesToolStripMenu.Visible = True

        Dim ConfigFileName As String = GlobalVariables.ConfigFolderName & "\" & GlobalVariables.ConfigFileName

        If File.Exists(ConfigFileName) Then
            Dim ConfigObject = GetSavedConfig(ConfigFileName)
            Dim ActiveTool As String = "tag_assets"
            GetSavedConfig(ConfigFileName)

            If Not ConfigObject(ActiveTool) Is Nothing Then
                TagAssetsSourceTextBox.Text = ConfigObject(ActiveTool)("source")
                TagAssetsDefaultTagTextBox.Text = ConfigObject(ActiveTool)("default_tag")
                TagAssetsLogCheckBox.Checked = ConfigObject(ActiveTool)("create_log")
                TagAssetsSelectAllCheckBox.Checked = ConfigObject(ActiveTool)("select_all")

                TagAssetsSourceTextBox_LostFocus(sender, e)
                Dim SizePref As New Size
                SizePref.Width = ConfigObject(ActiveTool)("size_width")
                SizePref.Height = ConfigObject(ActiveTool)("size_height")
                Me.Size = SizePref
            End If
        End If
    End Sub

    Private Sub ColorThemeApplyToMapMenuItem_Click(sender As Object, e As EventArgs) Handles ColorThemeApplyToMapMenuItem.Click
        TitlePanel.Hide()
        TagAssetsGroupBox.Hide()
        ColorThemeApplyGroupBox.Hide()
        ColorThemeCreateGroupBox.Hide()
        ConvertAssetsGroupBox.Hide()
        ConvertPacksGroupBox.Hide()
        CopyAssetsGroupBox.Hide()
        CopyTilesGroupBox.Hide()
        DataFilesGroupBox.Hide()
        MapDetailsGroupBox.Hide()
        PackAssetsGroupBox.Hide()
        UnpackAssetsGroupBox.Hide()

        ColorThemeApplyGroupBox.BringToFront()
        ColorThemeApplyGroupBox.Show()

        Me.MinimumSize = New Size(1000, 653)
        Me.MaximumSize = New Size(0, 0)

        PreferencesToolStripMenu.Visible = True

        Dim ConfigFileName As String = GlobalVariables.ConfigFolderName & "\" & GlobalVariables.ConfigFileName
        If File.Exists(ConfigFileName) Then
            Dim ConfigObject = GetSavedConfig(ConfigFileName)
            Dim ActiveTool As String = "apply_color_theme"
            GetSavedConfig(ConfigFileName)
            If Not ConfigObject(ActiveTool) Is Nothing Then
                ApplyColorThemeColorThemeFolderTextBox.Text = ConfigObject(ActiveTool)("theme_folder")
                ApplyColorThemeMapFolderTextBox.Text = ConfigObject(ActiveTool)("map_folder")
                ApplyColorThemeBackupPalettesCheckBox.Checked = ConfigObject(ActiveTool)("create_backup")
                ApplyColorThemeColorThemeFolderTextBox_LostFocus(sender, e)
                ApplyColorThemeMapFolderTextBox_LostFocus(sender, e)

                Dim SizePref As New Size
                SizePref.Width = ConfigObject(ActiveTool)("size_width")
                SizePref.Height = ConfigObject(ActiveTool)("size_height")
                Me.Size = SizePref
            End If
        End If
    End Sub

    Private Sub ColorThemeCreateFromMapMenuItem_Click(sender As Object, e As EventArgs) Handles ColorThemeCreateFromMapMenuItem.Click
        TitlePanel.Hide()
        TagAssetsGroupBox.Hide()
        ColorThemeApplyGroupBox.Hide()
        ColorThemeCreateGroupBox.Hide()
        ConvertAssetsGroupBox.Hide()
        ConvertPacksGroupBox.Hide()
        CopyAssetsGroupBox.Hide()
        CopyTilesGroupBox.Hide()
        DataFilesGroupBox.Hide()
        MapDetailsGroupBox.Hide()
        PackAssetsGroupBox.Hide()
        UnpackAssetsGroupBox.Hide()

        ColorThemeCreateGroupBox.BringToFront()
        ColorThemeCreateGroupBox.Show()

        Me.MinimumSize = New Size(802, 205)
        Me.MaximumSize = New Size(0, 0)

        PreferencesToolStripMenu.Visible = True

        Dim ConfigFileName As String = GlobalVariables.ConfigFolderName & "\" & GlobalVariables.ConfigFileName
        If File.Exists(ConfigFileName) Then
            Dim ConfigObject = GetSavedConfig(ConfigFileName)
            Dim ActiveTool As String = "create_color_theme"
            GetSavedConfig(ConfigFileName)
            If Not ConfigObject(ActiveTool) Is Nothing Then
                CreateColorThemeSourceMapTextBox.Text = ConfigObject(ActiveTool)("map_folder")
                CreateColorThemeColorThemeTextBox.Text = ConfigObject(ActiveTool)("theme_folder")
                CreateColorThemeIncludeAllCustomColorsCheckBox.Checked = ConfigObject(ActiveTool)("include_custom")
                CreateColorThemeIncludeNonPaletteColorsCheckBox.Checked = ConfigObject(ActiveTool)("include_nonpalette")

                Dim SizePref As New Size
                SizePref.Width = ConfigObject(ActiveTool)("size_width")
                SizePref.Height = ConfigObject(ActiveTool)("size_height")
                Me.Size = SizePref
            End If
        End If
    End Sub

    Private Sub ConvertAssetssMenuItem_Click(sender As Object, e As EventArgs) Handles ConvertAssetsMenuItem.Click
        TitlePanel.Hide()
        TagAssetsGroupBox.Hide()
        ColorThemeApplyGroupBox.Hide()
        ColorThemeCreateGroupBox.Hide()
        ConvertAssetsGroupBox.Hide()
        ConvertPacksGroupBox.Hide()
        CopyAssetsGroupBox.Hide()
        CopyTilesGroupBox.Hide()
        DataFilesGroupBox.Hide()
        MapDetailsGroupBox.Hide()
        PackAssetsGroupBox.Hide()
        UnpackAssetsGroupBox.Hide()

        ConvertAssetsGroupBox.BringToFront()
        ConvertAssetsGroupBox.Show()

        Me.MinimumSize = New Size(802, 653)
        Me.MaximumSize = New Size(0, 0)

        PreferencesToolStripMenu.Visible = True

        Dim ConfigFileName As String = GlobalVariables.ConfigFolderName & "\" & GlobalVariables.ConfigFileName
        If File.Exists(ConfigFileName) Then
            Dim ConfigObject = GetSavedConfig(ConfigFileName)
            Dim ActiveTool As String = "convert_assets"
            GetSavedConfig(ConfigFileName)

            If Not ConfigObject(ActiveTool) Is Nothing Then
                ConvertAssetsSourceTextBox.Text = ConfigObject(ActiveTool)("source")
                ConvertAssetsDestinationTextBox.Text = ConfigObject(ActiveTool)("destination")
                ConvertAssetsLogCheckBox.Checked = ConfigObject(ActiveTool)("create_log")
                ConvertAssetsSelectAllCheckBox.Checked = ConfigObject(ActiveTool)("select_all")

                ConvertAssetsSourceTextBox_LostFocus(sender, e)

                Dim SizePref As New Size
                SizePref.Width = ConfigObject(ActiveTool)("size_width")
                SizePref.Height = ConfigObject(ActiveTool)("size_height")
                Me.Size = SizePref
            End If
        End If
    End Sub

    Private Sub ConvertPacksMenuItem_Click(sender As Object, e As EventArgs) Handles ConvertPacksMenuItem.Click
        TitlePanel.Hide()
        TagAssetsGroupBox.Hide()
        ColorThemeApplyGroupBox.Hide()
        ColorThemeCreateGroupBox.Hide()
        ConvertAssetsGroupBox.Hide()
        ConvertPacksGroupBox.Hide()
        CopyAssetsGroupBox.Hide()
        CopyTilesGroupBox.Hide()
        DataFilesGroupBox.Hide()
        MapDetailsGroupBox.Hide()
        PackAssetsGroupBox.Hide()
        UnpackAssetsGroupBox.Hide()

        ConvertPacksGroupBox.BringToFront()
        ConvertPacksGroupBox.Show()

        Me.MinimumSize = New Size(802, 653)
        Me.MaximumSize = New Size(0, 0)

        PreferencesToolStripMenu.Visible = True

        Dim ConfigFileName As String = GlobalVariables.ConfigFolderName & "\" & GlobalVariables.ConfigFileName
        If File.Exists(ConfigFileName) Then
            Dim ConfigObject = GetSavedConfig(ConfigFileName)
            Dim ActiveTool As String = "convert_packs"
            GetSavedConfig(ConfigFileName)

            If Not ConfigObject(ActiveTool) Is Nothing Then
                ConvertPacksSourceTextBox.Text = ConfigObject(ActiveTool)("source")
                ConvertPacksDestinationTextBox.Text = ConfigObject(ActiveTool)("destination")
                ConvertPacksCleanUpCheckBox.Checked = ConfigObject(ActiveTool)("cleanup")
                ConvertPacksLogCheckBox.Checked = ConfigObject(ActiveTool)("create_log")
                ConvertPacksSelectAllCheckBox.Checked = ConfigObject(ActiveTool)("select_all")

                ConvertPacksSourceTextBox_LostFocus(sender, e)

                Dim SizePref As New Size
                SizePref.Width = ConfigObject(ActiveTool)("size_width")
                SizePref.Height = ConfigObject(ActiveTool)("size_height")
                Me.Size = SizePref
            End If
        End If
    End Sub

    Private Sub CopyAssetsMenuItem_Click(sender As Object, e As EventArgs) Handles CopyAssetsMenuItem.Click
        TitlePanel.Hide()
        TagAssetsGroupBox.Hide()
        ColorThemeApplyGroupBox.Hide()
        ColorThemeCreateGroupBox.Hide()
        ConvertAssetsGroupBox.Hide()
        ConvertPacksGroupBox.Hide()
        CopyAssetsGroupBox.Hide()
        CopyTilesGroupBox.Hide()
        DataFilesGroupBox.Hide()
        MapDetailsGroupBox.Hide()
        PackAssetsGroupBox.Hide()
        UnpackAssetsGroupBox.Hide()

        CopyAssetsGroupBox.BringToFront()
        CopyAssetsGroupBox.Show()

        Me.MinimumSize = New Size(802, 653)
        Me.MaximumSize = New Size(0, 0)

        PreferencesToolStripMenu.Visible = True

        Dim ConfigFileName As String = GlobalVariables.ConfigFolderName & "\" & GlobalVariables.ConfigFileName
        If File.Exists(ConfigFileName) Then
            Dim ConfigObject = GetSavedConfig(ConfigFileName)
            Dim ActiveTool As String = "copy_assets"
            GetSavedConfig(ConfigFileName)

            If Not ConfigObject(ActiveTool) Is Nothing Then
                CopyAssetsSourceTextBox.Text = ConfigObject(ActiveTool)("source")
                CopyAssetsDestinationTextBox.Text = ConfigObject(ActiveTool)("destination")
                CopyAssetsCreateTagsCheckBox.Checked = ConfigObject(ActiveTool)("create_tags")
                CopyAssetsPortalsCheckBox.Checked = ConfigObject(ActiveTool)("separate_portals")
                CopyAssetsLogCheckBox.Checked = ConfigObject(ActiveTool)("create_log")
                CopyAssetsSelectAllCheckBox.Checked = ConfigObject(ActiveTool)("select_all")

                CopyAssetsSourceTextBox_LostFocus(sender, e)

                Dim SizePref As New Size
                SizePref.Width = ConfigObject(ActiveTool)("size_width")
                SizePref.Height = ConfigObject(ActiveTool)("size_height")
                Me.Size = SizePref
            End If
        End If
    End Sub

    Private Sub CopyTilesMenuItem_Click(sender As Object, e As EventArgs) Handles CopyTilesMenuItem.Click
        TitlePanel.Hide()
        TagAssetsGroupBox.Hide()
        ColorThemeApplyGroupBox.Hide()
        ColorThemeCreateGroupBox.Hide()
        ConvertAssetsGroupBox.Hide()
        ConvertPacksGroupBox.Hide()
        CopyAssetsGroupBox.Hide()
        CopyTilesGroupBox.Hide()
        DataFilesGroupBox.Hide()
        MapDetailsGroupBox.Hide()
        PackAssetsGroupBox.Hide()
        UnpackAssetsGroupBox.Hide()

        CopyTilesGroupBox.BringToFront()
        CopyTilesGroupBox.Show()

        Me.MinimumSize = New Size(802, 653)
        Me.MaximumSize = New Size(0, 0)

        PreferencesToolStripMenu.Visible = True

        Dim ConfigFileName As String = GlobalVariables.ConfigFolderName & "\" & GlobalVariables.ConfigFileName
        If File.Exists(ConfigFileName) Then
            Dim ConfigObject = GetSavedConfig(ConfigFileName)
            Dim ActiveTool As String = "copy_tiles"
            GetSavedConfig(ConfigFileName)

            If Not ConfigObject(ActiveTool) Is Nothing Then
                CopyTilesSourceTextBox.Text = ConfigObject(ActiveTool)("source")
                CopyTilesDestinationTextBox.Text = ConfigObject(ActiveTool)("destination")
                CopyTilesLogCheckBox.Checked = ConfigObject(ActiveTool)("create_log")
                CopyTilesSelectAllCheckBox.Checked = ConfigObject(ActiveTool)("select_all")

                CopyTilesSourceTextBox_LostFocus(sender, e)

                Dim SizePref As New Size
                SizePref.Width = ConfigObject(ActiveTool)("size_width")
                SizePref.Height = ConfigObject(ActiveTool)("size_height")
                Me.Size = SizePref
            End If
        End If
    End Sub

    Private Sub DataFilesMenuItem_Click(sender As Object, e As EventArgs) Handles DataFilesMenuItem.Click
        TitlePanel.Hide()
        TagAssetsGroupBox.Hide()
        ColorThemeApplyGroupBox.Hide()
        ColorThemeCreateGroupBox.Hide()
        ConvertAssetsGroupBox.Hide()
        ConvertPacksGroupBox.Hide()
        CopyAssetsGroupBox.Hide()
        CopyTilesGroupBox.Hide()
        DataFilesGroupBox.Hide()
        MapDetailsGroupBox.Hide()
        PackAssetsGroupBox.Hide()
        UnpackAssetsGroupBox.Hide()

        DataFilesGroupBox.BringToFront()
        DataFilesGroupBox.Show()

        Me.MinimumSize = New Size(1000, 653)
        Me.MaximumSize = New Size(0, 0)

        PreferencesToolStripMenu.Visible = True

        Dim ConfigFileName As String = GlobalVariables.ConfigFolderName & "\" & GlobalVariables.ConfigFileName
        If File.Exists(ConfigFileName) Then
            Dim ConfigObject = GetSavedConfig(ConfigFileName)
            Dim ActiveTool As String = "data_files"
            GetSavedConfig(ConfigFileName)

            If Not ConfigObject(ActiveTool) Is Nothing Then
                DataFilesSourceTextBox.Text = ConfigObject(ActiveTool)("source")
                DataFilesLogCheckBox.Checked = ConfigObject(ActiveTool)("create_log")
                DataFilesSourceTextBox_LostFocus(sender, e)

                Dim SizePref As New Size
                SizePref.Width = ConfigObject(ActiveTool)("size_width")
                SizePref.Height = ConfigObject(ActiveTool)("size_height")
                Me.Size = SizePref
            End If
        End If
    End Sub

    Private Sub MapDetailsMenuItem_Click(sender As Object, e As EventArgs) Handles MapDetailsMenuItem.Click
        TitlePanel.Hide()
        TagAssetsGroupBox.Hide()
        ColorThemeApplyGroupBox.Hide()
        ColorThemeCreateGroupBox.Hide()
        ConvertAssetsGroupBox.Hide()
        ConvertPacksGroupBox.Hide()
        CopyAssetsGroupBox.Hide()
        CopyTilesGroupBox.Hide()
        DataFilesGroupBox.Hide()
        MapDetailsGroupBox.Hide()
        PackAssetsGroupBox.Hide()
        UnpackAssetsGroupBox.Hide()

        MapDetailsGroupBox.BringToFront()
        MapDetailsGroupBox.Show()

        Me.MinimumSize = New Size(802, 653)
        Me.MaximumSize = New Size(0, 0)

        PreferencesToolStripMenu.Visible = True

        Dim ConfigFileName As String = GlobalVariables.ConfigFolderName & "\" & GlobalVariables.ConfigFileName
        If File.Exists(ConfigFileName) Then
            Dim ConfigObject = GetSavedConfig(ConfigFileName)
            Dim ActiveTool As String = "map_details"
            GetSavedConfig(ConfigFileName)

            If Not ConfigObject(ActiveTool) Is Nothing Then
                MapDetailsSourceTextBox.Text = ConfigObject(ActiveTool)("source")
                MapDetailsLogCheckBox.Checked = ConfigObject(ActiveTool)("create_log")
                MapDetailsSelectAllCheckBox.Checked = ConfigObject(ActiveTool)("select_all")

                MapDetailsSourceTextBox_LostFocus(sender, e)

                Dim SizePref As New Size
                SizePref.Width = ConfigObject(ActiveTool)("size_width")
                SizePref.Height = ConfigObject(ActiveTool)("size_height")
                Me.Size = SizePref
            End If
        End If
    End Sub

    Private Sub PackAssetsMenuItem_Click(sender As Object, e As EventArgs) Handles PackAssetsMenuItem.Click
        TitlePanel.Hide()
        TagAssetsGroupBox.Hide()
        ColorThemeApplyGroupBox.Hide()
        ColorThemeCreateGroupBox.Hide()
        ConvertAssetsGroupBox.Hide()
        ConvertPacksGroupBox.Hide()
        CopyAssetsGroupBox.Hide()
        CopyTilesGroupBox.Hide()
        DataFilesGroupBox.Hide()
        MapDetailsGroupBox.Hide()
        PackAssetsGroupBox.Hide()
        UnpackAssetsGroupBox.Hide()

        PackAssetsGroupBox.BringToFront()
        PackAssetsGroupBox.Show()

        Me.MinimumSize = New Size(1000, 653)
        Me.MaximumSize = New Size(0, 0)

        PreferencesToolStripMenu.Visible = True

        Dim ConfigFileName As String = GlobalVariables.ConfigFolderName & "\" & GlobalVariables.ConfigFileName
        If File.Exists(ConfigFileName) Then
            Dim ConfigObject = GetSavedConfig(ConfigFileName)
            Dim ActiveTool As String = "pack_assets"
            GetSavedConfig(ConfigFileName)

            If Not ConfigObject(ActiveTool) Is Nothing Then
                PackAssetsSourceTextBox.Text = ConfigObject(ActiveTool)("source")
                PackAssetsDestinationTextBox.Text = ConfigObject(ActiveTool)("destination")
                PackAssetsOverwriteCheckBox.Checked = ConfigObject(ActiveTool)("overwrite")
                PackAssetsLogCheckBox.Checked = ConfigObject(ActiveTool)("create_log")
                PackAssetsSelectAllCheckBox.Checked = ConfigObject(ActiveTool)("select_all")

                PackAssetsSourceTextBox_LostFocus(sender, e)

                Dim SizePref As New Size
                SizePref.Width = ConfigObject(ActiveTool)("size_width")
                SizePref.Height = ConfigObject(ActiveTool)("size_height")
                Me.Size = SizePref
            End If
        End If
    End Sub

    Private Sub UnpackAssetsMenuItem_Click(sender As Object, e As EventArgs) Handles UnpackAssetsMenuItem.Click
        TitlePanel.Hide()
        TagAssetsGroupBox.Hide()
        ColorThemeApplyGroupBox.Hide()
        ColorThemeCreateGroupBox.Hide()
        ConvertAssetsGroupBox.Hide()
        ConvertPacksGroupBox.Hide()
        CopyAssetsGroupBox.Hide()
        CopyTilesGroupBox.Hide()
        DataFilesGroupBox.Hide()
        MapDetailsGroupBox.Hide()
        PackAssetsGroupBox.Hide()
        UnpackAssetsGroupBox.Hide()

        UnpackAssetsGroupBox.BringToFront()
        UnpackAssetsGroupBox.Show()

        Me.MinimumSize = New Size(802, 653)
        Me.MaximumSize = New Size(0, 0)

        PreferencesToolStripMenu.Visible = True

        Dim ConfigFileName As String = GlobalVariables.ConfigFolderName & "\" & GlobalVariables.ConfigFileName
        If File.Exists(ConfigFileName) Then
            Dim ConfigObject = GetSavedConfig(ConfigFileName)
            Dim ActiveTool As String = "unpack_assets"
            GetSavedConfig(ConfigFileName)

            If Not ConfigObject(ActiveTool) Is Nothing Then
                UnpackAssetsSourceTextBox.Text = ConfigObject(ActiveTool)("source")
                UnpackAssetsDestinationTextBox.Text = ConfigObject(ActiveTool)("destination")
                UnpackAssetsLogCheckBox.Checked = ConfigObject(ActiveTool)("create_log")
                PackAssetsSelectAllCheckBox.Checked = ConfigObject(ActiveTool)("select_all")

                UnpackAssetsSourceTextBox_LostFocus(sender, e)

                Dim SizePref As New Size
                SizePref.Width = ConfigObject(ActiveTool)("size_width")
                SizePref.Height = ConfigObject(ActiveTool)("size_height")
                Me.Size = SizePref
            End If
        End If
    End Sub

    Public Sub Me_SizeChanged() Handles Me.SizeChanged
        If TagAssetsGroupBox.Visible Then
            TagAssetsGroupBox.Size = New Size(Me.Size.Width - 44, Me.Size.Height - 88)
            TagAssetsBrowseButton.Location = New Point(Me.Size.Width - 144, 21)
            TagAssetsSelectAllButton.Location = New Point(Me.Size.Width - 144, 130)
            TagAssetsSelectNoneButton.Location = New Point(Me.Size.Width - 144, 162)
            TagAssetsStartButton.Location = New Point(Me.Size.Width - 144, 194)
            TagAssetsSourceTextBox.Size = New Size(Me.Size.Width - 255, 22)
            TagAssetsDefaultTagTextBox.Size = New Size(Me.Size.Width - 255, 22)
            TagAssetsCheckedListBox.Size = New Size(Me.Size.Width - 156, Me.Height - 224)
        ElseIf ColorThemeApplyGroupBox.Visible Then
            Dim SizeDiff As New Size
            ColorThemeApplyGroupBox.Size = New Size(Me.Size.Width - 44, Me.Size.Height - 88)
            SizeDiff = Me.Size - Me.MinimumSize
            ApplyColorThemeColorThemeFolderTextBox.Size = New Size(309 + Int(SizeDiff.Width / 2), ApplyColorThemeColorThemeFolderTextBox.Size.Height)
            ApplyColorThemeMapFolderBrowseButton.Location = New Point(481 + Int(SizeDiff.Width / 2), 21)
            ApplyColorThemeMapFolderTextBox.Location = New Point(581 + Int(SizeDiff.Width / 2), 23)
            ApplyColorThemeMapFolderTextBox.Size = New Size(369 + Int(SizeDiff.Width / 2), ApplyColorThemeMapFolderTextBox.Size.Height)
            ApplyColorThemeBackupPalettesCheckBox.Location = New Point(ApplyColorThemeMapFolderTextBox.Location.X, 53)
            ApplyColorThemeColorThemeListBox.Size = New Size(469 + Int(SizeDiff.Width / 2), Me.Size.Height - 233)
            ApplyColorThemeMapListBox.Location = New Point(481 + Int(SizeDiff.Width / 2), 81)
            ApplyColorThemeMapListBox.Size = New Size(469 + Int(SizeDiff.Width / 2), Me.Size.Height - 233)
            ApplyColorThemeStartButton.Location = New Point(6, Me.Height - 146)
            ApplyColorThemeStartButton.Size = New Size(Me.Width - 56, 52)
        ElseIf ColorThemeCreateGroupBox.Visible Then
            Dim MaxSize As New Size(0, 205)
            If Me.Size.Height > MaxSize.Height Then
                Me.Size = New Size(Me.Size.Width, MaxSize.Height)
            End If
            ColorThemeCreateGroupBox.Size = New Size(Me.Size.Width - 44, 117)
            CreateColorThemeSourceMapOpenButton.Location = New Point(Me.Size.Width - 144, 21)
            CreateColorThemeSaveButton.Location = New Point(Me.Size.Width - 144, 53)
            CreateColorThemeStartButton.Location = New Point(Me.Size.Width - 144, 85)
            CreateColorThemeSourceMapTextBox.Size = New Size(Me.Size.Width - 255, 22)
            CreateColorThemeColorThemeTextBox.Size = New Size(Me.Size.Width - 255, 22)
        ElseIf ConvertAssetsGroupBox.Visible Then
            ConvertAssetsGroupBox.Size = New Size(Me.Size.Width - 44, Me.Size.Height - 88)
            ConvertAssetsSourceBrowseButton.Location = New Point(Me.Size.Width - 144, 21)
            ConvertAssetsDestinationBrowseButton.Location = New Point(Me.Size.Width - 144, 53)
            ConvertAssetsSelectAllButton.Location = New Point(Me.Size.Width - 144, 130)
            ConvertAssetsSelectNoneButton.Location = New Point(Me.Size.Width - 144, 162)
            ConvertAssetsStartButton.Location = New Point(Me.Size.Width - 144, 194)
            ConvertAssetsSourceTextBox.Size = New Size(Me.Size.Width - 279, 22)
            ConvertAssetsDestinationTextBox.Size = New Size(Me.Size.Width - 279, 22)
            ConvertAssetsCheckedListBox.Size = New Size(Me.Size.Width - 156, Me.Height - 224)
        ElseIf ConvertPacksGroupBox.Visible Then
            ConvertPacksGroupBox.Size = New Size(Me.Size.Width - 44, Me.Size.Height - 88)
            ConvertPacksSourceBrowseButton.Location = New Point(Me.Size.Width - 144, 21)
            ConvertPacksDestinationBrowseButton.Location = New Point(Me.Size.Width - 144, 53)
            ConvertPacksSelectAllButton.Location = New Point(Me.Size.Width - 144, 130)
            ConvertPacksSelectNoneButton.Location = New Point(Me.Size.Width - 144, 162)
            ConvertPacksStartButton.Location = New Point(Me.Size.Width - 144, 194)
            ConvertPacksSourceTextBox.Size = New Size(Me.Size.Width - 279, 22)
            ConvertPacksDestinationTextBox.Size = New Size(Me.Size.Width - 279, 22)
            ConvertPacksCheckedListBox.Size = New Size(Me.Size.Width - 156, Me.Height - 224)
        ElseIf CopyAssetsGroupBox.Visible Then
            CopyAssetsGroupBox.Size = New Size(Me.Size.Width - 44, Me.Size.Height - 88)
            CopyAssetsSourceBrowseButton.Location = New Point(Me.Size.Width - 144, 21)
            CopyAssetsDestinationBrowseButton.Location = New Point(Me.Size.Width - 144, 53)
            CopyAssetsSelectAllButton.Location = New Point(Me.Size.Width - 144, 130)
            CopyAssetsSelectNoneButton.Location = New Point(Me.Size.Width - 144, 162)
            CopyAssetsStartButton.Location = New Point(Me.Size.Width - 144, 194)
            CopyAssetsSourceTextBox.Size = New Size(Me.Size.Width - 279, 22)
            CopyAssetsDestinationTextBox.Size = New Size(Me.Size.Width - 279, 22)
            CopyAssetsCheckedListBox.Size = New Size(Me.Size.Width - 156, Me.Height - 224)
        ElseIf CopyTilesGroupBox.Visible Then
            CopyTilesGroupBox.Size = New Size(Me.Size.Width - 44, Me.Size.Height - 88)
            CopyTilesSourceBrowseButton.Location = New Point(Me.Size.Width - 144, 21)
            CopyTilesDestinationBrowseButton.Location = New Point(Me.Size.Width - 144, 53)
            CopyTilesStartButton.Location = New Point(Me.Size.Width - 144, 85)
            CopyTilesSourceTextBox.Size = New Size(Me.Size.Width - 279, 22)
            CopyTilesDestinationTextBox.Size = New Size(Me.Size.Width - 279, 22)
            CopyTilesDataGridView.Size = New Size(Me.Size.Width - 59, Me.Height - 224)
            If CopyTilesDataGridView.Columns.Count > 0 Then
                CopyTilesDataGridView.Columns("TileName").Width = 182 + Me.Size.Width - Me.MinimumSize.Width
            End If
        ElseIf DataFilesGroupBox.Visible Then
            DataFilesGroupBox.Size = New Size(Me.Size.Width - 44, Me.Size.Height - 88)
            DataFilesSourceBrowseButton.Location = New Point(Me.Size.Width - 144, 21)
            DataFilesStartButton.Location = New Point(Me.Size.Width - 144, 53)
            DataFilesSourceTextBox.Size = New Size(Me.Size.Width - 252, 22)
            DataFilesDataGridView.Size = New Size(Me.Size.Width - 56, Me.Size.Height - 179)

            If DataFilesDataGridView.Columns.Count > 0 Then
                Dim TotalWidth = Me.Size.Width - Me.MinimumSize.Width
                Dim NameWidth = Int(TotalWidth * 0.48)
                Dim FileWidth = TotalWidth - NameWidth
                DataFilesDataGridView.Columns("Name").Width = 182 + NameWidth
                DataFilesDataGridView.Columns("DataFile").Width = 379 + FileWidth
            End If
        ElseIf MapDetailsGroupBox.Visible Then
            MapDetailsGroupBox.Size = New Size(Me.Size.Width - 44, Me.Size.Height - 88)
            MapDetailsBrowseButton.Location = New Point(Me.Size.Width - 144, 21)
            MapDetailsSelectAllButton.Location = New Point(Me.Size.Width - 144, 130)
            MapDetailsSelectNoneButton.Location = New Point(Me.Size.Width - 144, 162)
            MapDetailsStartButton.Location = New Point(Me.Size.Width - 144, 194)
            MapDetailsSourceTextBox.Size = New Size(Me.Size.Width - 255, 22)
            MapDetailsCheckedListBox.Size = New Size(Me.Size.Width - 156, Me.Height - 224)
        ElseIf PackAssetsGroupBox.Visible Then
            PackAssetsGroupBox.Size = New Size(Me.Size.Width - 44, Me.Size.Height - 88)
            PackAssetsSourceBrowseButton.Location = New Point(Me.Size.Width - 144, 21)
            PackAssetsDestinationBrowseButton.Location = New Point(Me.Size.Width - 144, 53)
            PackAssetsRefreshButton.Location = New Point(Me.Size.Width - 144, 130)
            PackAssetsSelectAllButton.Location = New Point(Me.Size.Width - 144, 162)
            PackAssetsSelectNoneButton.Location = New Point(Me.Size.Width - 144, 194)
            PackAssetsStartButton.Location = New Point(Me.Size.Width - 144, 226)
            PackAssetsSourceTextBox.Size = New Size(Me.Size.Width - 279, 22)
            PackAssetsDestinationTextBox.Size = New Size(Me.Size.Width - 279, 22)
            PackAssetsDataGridView.Size = New Size(Me.Size.Width - 158, Me.Size.Height - 224)

            If PackAssetsDataGridView.Columns.Count > 0 Then
                Dim TotalWidth = Me.Size.Width - Me.MinimumSize.Width
                Dim AuthorWidth = Int((TotalWidth / 3) * 0.65)
                Dim NewWidth = Int((TotalWidth - AuthorWidth) / 2)
                PackAssetsDataGridView.Columns("FolderName").Width = 199 + NewWidth
                PackAssetsDataGridView.Columns("PackName").Width = 199 + NewWidth
                PackAssetsDataGridView.Columns("PackAuthor").Width = 130 + AuthorWidth
            End If
        ElseIf UnpackAssetsGroupBox.Visible Then
            UnpackAssetsGroupBox.Size = New Size(Me.Size.Width - 44, Me.Size.Height - 88)
            UnpackAssetsSourceBrowseButton.Location = New Point(Me.Size.Width - 144, 21)
            UnpackAssetsDestinationBrowseButton.Location = New Point(Me.Size.Width - 144, 53)
            UnpackAssetsSelectAllButton.Location = New Point(Me.Size.Width - 144, 130)
            UnpackAssetsSelectNoneButton.Location = New Point(Me.Size.Width - 144, 162)
            UnpackAssetsStartButton.Location = New Point(Me.Size.Width - 144, 194)
            UnpackAssetsSourceTextBox.Size = New Size(Me.Size.Width - 279, 22)
            UnpackAssetsDestinationTextBox.Size = New Size(Me.Size.Width - 279, 22)
            UnpackAssetsCheckedListBox.Size = New Size(Me.Size.Width - 156, Me.Height - 224)
        End If
    End Sub

    '###### Preference Menu Items ######
    Private Sub SavePrefsMenuItem_Click(sender As Object, e As EventArgs) Handles SavePrefsMenuItem.Click
        Dim ConfigFolderName As String = GlobalVariables.ConfigFolderName
        Dim ConfigFileName As String = GlobalVariables.ConfigFolderName & "\" & GlobalVariables.ConfigFileName
        Dim ConfigObject
        Dim ActiveTool As String
        If My.Computer.FileSystem.FileExists(ConfigFileName) Then
            ConfigObject = GetSavedConfig(ConfigFileName)
        Else
            ConfigObject = BuildConfigObject()
        End If

        ActiveTool = "global_config"
        If ConfigObject(ActiveTool) Is Nothing Then
            Dim NewPrefs As New Newtonsoft.Json.Linq.JObject
            ConfigObject.Add(ActiveTool, NewPrefs)
        End If
        If ConfigObject(ActiveTool)("location_x") Is Nothing Then ConfigObject(ActiveTool).Add("location_x", 0)
        ConfigObject(ActiveTool)("location_x") = Me.Location.X
        If ConfigObject(ActiveTool)("location_y") Is Nothing Then ConfigObject(ActiveTool).Add("location_y", 0)
        ConfigObject(ActiveTool)("location_y") = Me.Location.Y

        If TagAssetsGroupBox.Visible = True Then
            ActiveTool = "tag_assets"
            If ConfigObject(ActiveTool) Is Nothing Then
                Dim NewPrefs As New Newtonsoft.Json.Linq.JObject
                ConfigObject.Add(ActiveTool, NewPrefs)
            End If
            If ConfigObject(ActiveTool)("source") Is Nothing Then ConfigObject(ActiveTool).Add("source", "")
            ConfigObject(ActiveTool)("source") = TagAssetsSourceTextBox.Text
            If ConfigObject(ActiveTool)("default_tag") Is Nothing Then ConfigObject(ActiveTool).Add("default_tag", "")
            ConfigObject(ActiveTool)("default_tag") = TagAssetsDefaultTagTextBox.Text
            If ConfigObject(ActiveTool)("create_log") Is Nothing Then ConfigObject(ActiveTool).Add("create_log", "")
            ConfigObject(ActiveTool)("create_log") = TagAssetsLogCheckBox.Checked
            If ConfigObject(ActiveTool)("select_all") Is Nothing Then ConfigObject(ActiveTool).Add("select_all", "")
            ConfigObject(ActiveTool)("select_all") = TagAssetsSelectAllCheckBox.Checked
            If ConfigObject(ActiveTool)("size_width") Is Nothing Then ConfigObject(ActiveTool).Add("size_width", 0)
            ConfigObject(ActiveTool)("size_width") = Me.Size.Width
            If ConfigObject(ActiveTool)("size_height") Is Nothing Then ConfigObject(ActiveTool).Add("size_height", 0)
            ConfigObject(ActiveTool)("size_height") = Me.Size.Height
        ElseIf ColorThemeApplyGroupBox.Visible = True Then
            ActiveTool = "apply_color_theme"
            If ConfigObject(ActiveTool) Is Nothing Then
                Dim NewPrefs As New Newtonsoft.Json.Linq.JObject
                ConfigObject.Add(ActiveTool, NewPrefs)
            End If
            If ConfigObject(ActiveTool)("theme_folder") Is Nothing Then ConfigObject(ActiveTool).Add("theme_folder", "")
            ConfigObject(ActiveTool)("theme_folder") = ApplyColorThemeColorThemeFolderTextBox.Text
            If ConfigObject(ActiveTool)("map_folder") Is Nothing Then ConfigObject(ActiveTool).Add("map_folder", "")
            ConfigObject(ActiveTool)("map_folder") = ApplyColorThemeMapFolderTextBox.Text
            If ConfigObject(ActiveTool)("create_backup") Is Nothing Then ConfigObject(ActiveTool).Add("create_backup", "")
            ConfigObject(ActiveTool)("create_backup") = ApplyColorThemeBackupPalettesCheckBox.Checked
            If ConfigObject(ActiveTool)("size_width") Is Nothing Then ConfigObject(ActiveTool).Add("size_width", 0)
            ConfigObject(ActiveTool)("size_width") = Me.Size.Width
            If ConfigObject(ActiveTool)("size_height") Is Nothing Then ConfigObject(ActiveTool).Add("size_height", 0)
            ConfigObject(ActiveTool)("size_height") = Me.Size.Height
        ElseIf ColorThemeCreateGroupBox.Visible = True Then
            ActiveTool = "create_color_theme"
            If ConfigObject(ActiveTool) Is Nothing Then
                Dim NewPrefs As New Newtonsoft.Json.Linq.JObject
                ConfigObject.Add(ActiveTool, NewPrefs)
            End If
            Dim themefolder As String = CreateColorThemeColorThemeTextBox.Text
            If File.Exists(themefolder) Then
                themefolder = Path.GetFullPath(Path.GetDirectoryName(themefolder))
            ElseIf Directory.Exists(themefolder) Then
                'nothing to do
            Else
                themefolder = ""
            End If

            Dim mapfolder As String = CreateColorThemeSourceMapTextBox.Text
            If File.Exists(mapfolder) Then
                mapfolder = Path.GetFullPath(Path.GetDirectoryName(mapfolder))
            ElseIf Directory.Exists(mapfolder) Then
                'nothing to do
            Else
                mapfolder = ""
            End If
            If ConfigObject(ActiveTool)("theme_folder") Is Nothing Then ConfigObject(ActiveTool).Add("theme_folder", "")
            ConfigObject(ActiveTool)("theme_folder") = themefolder
            If ConfigObject(ActiveTool)("map_folder") Is Nothing Then ConfigObject(ActiveTool).Add("map_folder", "")
            ConfigObject(ActiveTool)("map_folder") = mapfolder
            If ConfigObject(ActiveTool)("include_custom") Is Nothing Then ConfigObject(ActiveTool).Add("include_custom", "")
            ConfigObject(ActiveTool)("include_custom") = CreateColorThemeIncludeAllCustomColorsCheckBox.Checked
            If ConfigObject(ActiveTool)("include_nonpalette") Is Nothing Then ConfigObject(ActiveTool).Add("include_nonpalette", "")
            ConfigObject(ActiveTool)("include_nonpalette") = CreateColorThemeIncludeNonPaletteColorsCheckBox.Checked
            If ConfigObject(ActiveTool)("size_width") Is Nothing Then ConfigObject(ActiveTool).Add("size_width", 0)
            ConfigObject(ActiveTool)("size_width") = Me.Size.Width
            If ConfigObject(ActiveTool)("size_height") Is Nothing Then ConfigObject(ActiveTool).Add("size_height", 0)
            ConfigObject(ActiveTool)("size_height") = Me.Size.Height
        ElseIf ConvertAssetsGroupBox.Visible = True Then
            ActiveTool = "convert_assets"
            If ConfigObject(ActiveTool) Is Nothing Then
                Dim NewPrefs As New Newtonsoft.Json.Linq.JObject
                ConfigObject.Add(ActiveTool, NewPrefs)
            End If
            If ConfigObject(ActiveTool)("source") Is Nothing Then ConfigObject(ActiveTool).Add("source", "")
            ConfigObject(ActiveTool)("source") = ConvertAssetsSourceTextBox.Text
            If ConfigObject(ActiveTool)("destination") Is Nothing Then ConfigObject(ActiveTool).Add("destination", "")
            ConfigObject(ActiveTool)("destination") = ConvertAssetsDestinationTextBox.Text
            If ConfigObject(ActiveTool)("create_log") Is Nothing Then ConfigObject(ActiveTool).Add("create_log", "")
            ConfigObject(ActiveTool)("create_log") = ConvertAssetsLogCheckBox.Checked
            If ConfigObject(ActiveTool)("select_all") Is Nothing Then ConfigObject(ActiveTool).Add("select_all", "")
            ConfigObject(ActiveTool)("select_all") = ConvertAssetsSelectAllCheckBox.Checked
            If ConfigObject(ActiveTool)("size_width") Is Nothing Then ConfigObject(ActiveTool).Add("size_width", 0)
            ConfigObject(ActiveTool)("size_width") = Me.Size.Width
            If ConfigObject(ActiveTool)("size_height") Is Nothing Then ConfigObject(ActiveTool).Add("size_height", 0)
            ConfigObject(ActiveTool)("size_height") = Me.Size.Height
        ElseIf ConvertPacksGroupBox.Visible = True Then
            ActiveTool = "convert_packs"
            If ConfigObject(ActiveTool) Is Nothing Then
                Dim NewPrefs As New Newtonsoft.Json.Linq.JObject
                ConfigObject.Add(ActiveTool, NewPrefs)
            End If
            If ConfigObject(ActiveTool)("source") Is Nothing Then ConfigObject(ActiveTool).Add("source", "")
            ConfigObject(ActiveTool)("source") = ConvertPacksSourceTextBox.Text
            If ConfigObject(ActiveTool)("destination") Is Nothing Then ConfigObject(ActiveTool).Add("destination", "")
            ConfigObject(ActiveTool)("destination") = ConvertPacksDestinationTextBox.Text
            If ConfigObject(ActiveTool)("cleanup") Is Nothing Then ConfigObject(ActiveTool).Add("cleanup", "")
            ConfigObject(ActiveTool)("cleanup") = ConvertPacksCleanUpCheckBox.Checked
            If ConfigObject(ActiveTool)("create_log") Is Nothing Then ConfigObject(ActiveTool).Add("create_log", "")
            ConfigObject(ActiveTool)("create_log") = ConvertPacksLogCheckBox.Checked
            If ConfigObject(ActiveTool)("select_all") Is Nothing Then ConfigObject(ActiveTool).Add("select_all", "")
            ConfigObject(ActiveTool)("select_all") = ConvertPacksSelectAllCheckBox.Checked
            If ConfigObject(ActiveTool)("size_width") Is Nothing Then ConfigObject(ActiveTool).Add("size_width", 0)
            ConfigObject(ActiveTool)("size_width") = Me.Size.Width
            If ConfigObject(ActiveTool)("size_height") Is Nothing Then ConfigObject(ActiveTool).Add("size_height", 0)
            ConfigObject(ActiveTool)("size_height") = Me.Size.Height
        ElseIf CopyAssetsGroupBox.Visible = True Then
            ActiveTool = "copy_assets"
            If ConfigObject(ActiveTool) Is Nothing Then
                Dim NewPrefs As New Newtonsoft.Json.Linq.JObject
                ConfigObject.Add(ActiveTool, NewPrefs)
            End If
            If ConfigObject(ActiveTool)("source") Is Nothing Then ConfigObject(ActiveTool).Add("source", "")
            ConfigObject(ActiveTool)("source") = CopyAssetsSourceTextBox.Text
            If ConfigObject(ActiveTool)("destination") Is Nothing Then ConfigObject(ActiveTool).Add("destination", "")
            ConfigObject(ActiveTool)("destination") = CopyAssetsDestinationTextBox.Text
            If ConfigObject(ActiveTool)("create_tags") Is Nothing Then ConfigObject(ActiveTool).Add("create_tags", "")
            ConfigObject(ActiveTool)("create_tags") = CopyAssetsCreateTagsCheckBox.Checked
            If ConfigObject(ActiveTool)("separate_portals") Is Nothing Then ConfigObject(ActiveTool).Add("separate_portals", "")
            ConfigObject(ActiveTool)("separate_portals") = CopyAssetsPortalsCheckBox.Checked
            If ConfigObject(ActiveTool)("create_log") Is Nothing Then ConfigObject(ActiveTool).Add("create_log", "")
            ConfigObject(ActiveTool)("create_log") = CopyAssetsLogCheckBox.Checked
            If ConfigObject(ActiveTool)("select_all") Is Nothing Then ConfigObject(ActiveTool).Add("select_all", "")
            ConfigObject(ActiveTool)("select_all") = CopyAssetsSelectAllCheckBox.Checked
            If ConfigObject(ActiveTool)("size_width") Is Nothing Then ConfigObject(ActiveTool).Add("size_width", 0)
            ConfigObject(ActiveTool)("size_width") = Me.Size.Width
            If ConfigObject(ActiveTool)("size_height") Is Nothing Then ConfigObject(ActiveTool).Add("size_height", 0)
            ConfigObject(ActiveTool)("size_height") = Me.Size.Height
        ElseIf CopyTilesGroupBox.Visible = True Then
            ActiveTool = "copy_tiles"
            If ConfigObject(ActiveTool) Is Nothing Then
                Dim NewPrefs As New Newtonsoft.Json.Linq.JObject
                ConfigObject.Add(ActiveTool, NewPrefs)
            End If
            If ConfigObject(ActiveTool)("source") Is Nothing Then ConfigObject(ActiveTool).Add("source", "")
            ConfigObject(ActiveTool)("source") = CopyTilesSourceTextBox.Text
            If ConfigObject(ActiveTool)("destination") Is Nothing Then ConfigObject(ActiveTool).Add("destination", "")
            ConfigObject(ActiveTool)("destination") = CopyTilesDestinationTextBox.Text
            If ConfigObject(ActiveTool)("create_log") Is Nothing Then ConfigObject(ActiveTool).Add("create_log", "")
            ConfigObject(ActiveTool)("create_log") = CopyTilesLogCheckBox.Checked
            If ConfigObject(ActiveTool)("select_all") Is Nothing Then ConfigObject(ActiveTool).Add("select_all", "")
            ConfigObject(ActiveTool)("select_all") = CopyTilesSelectAllCheckBox.Checked
            If ConfigObject(ActiveTool)("size_width") Is Nothing Then ConfigObject(ActiveTool).Add("size_width", 0)
            ConfigObject(ActiveTool)("size_width") = Me.Size.Width
            If ConfigObject(ActiveTool)("size_height") Is Nothing Then ConfigObject(ActiveTool).Add("size_height", 0)
            ConfigObject(ActiveTool)("size_height") = Me.Size.Height
        ElseIf DataFilesGroupBox.Visible = True Then
            ActiveTool = "data_files"
            If ConfigObject(ActiveTool) Is Nothing Then
                Dim NewPrefs As New Newtonsoft.Json.Linq.JObject
                ConfigObject.Add(ActiveTool, NewPrefs)
            End If
            If ConfigObject(ActiveTool)("source") Is Nothing Then ConfigObject(ActiveTool).Add("source", "")
            ConfigObject(ActiveTool)("source") = DataFilesSourceTextBox.Text
            If ConfigObject(ActiveTool)("create_log") Is Nothing Then ConfigObject(ActiveTool).Add("create_log", "")
            ConfigObject(ActiveTool)("create_log") = DataFilesLogCheckBox.Checked
            If ConfigObject(ActiveTool)("size_width") Is Nothing Then ConfigObject(ActiveTool).Add("size_width", 0)
            ConfigObject(ActiveTool)("size_width") = Me.Size.Width
            If ConfigObject(ActiveTool)("size_height") Is Nothing Then ConfigObject(ActiveTool).Add("size_height", 0)
            ConfigObject(ActiveTool)("size_height") = Me.Size.Height
        ElseIf MapDetailsGroupBox.Visible = True Then
            ActiveTool = "map_details"
            If ConfigObject(ActiveTool) Is Nothing Then
                Dim NewPrefs As New Newtonsoft.Json.Linq.JObject
                ConfigObject.Add(ActiveTool, NewPrefs)
            End If
            If ConfigObject(ActiveTool)("source") Is Nothing Then ConfigObject(ActiveTool).Add("source", "")
            ConfigObject(ActiveTool)("source") = MapDetailsSourceTextBox.Text
            If ConfigObject(ActiveTool)("create_log") Is Nothing Then ConfigObject(ActiveTool).Add("create_log", "")
            ConfigObject(ActiveTool)("create_log") = MapDetailsLogCheckBox.Checked
            If ConfigObject(ActiveTool)("select_all") Is Nothing Then ConfigObject(ActiveTool).Add("select_all", "")
            ConfigObject(ActiveTool)("select_all") = MapDetailsSelectAllCheckBox.Checked
            If ConfigObject(ActiveTool)("size_width") Is Nothing Then ConfigObject(ActiveTool).Add("size_width", 0)
            ConfigObject(ActiveTool)("size_width") = Me.Size.Width
            If ConfigObject(ActiveTool)("size_height") Is Nothing Then ConfigObject(ActiveTool).Add("size_height", 0)
            ConfigObject(ActiveTool)("size_height") = Me.Size.Height
        ElseIf PackAssetsGroupBox.Visible = True Then
            ActiveTool = "pack_assets"
            If ConfigObject(ActiveTool) Is Nothing Then
                Dim NewPrefs As New Newtonsoft.Json.Linq.JObject
                ConfigObject.Add(ActiveTool, NewPrefs)
            End If
            If ConfigObject(ActiveTool)("source") Is Nothing Then ConfigObject(ActiveTool).Add("source", "")
            ConfigObject(ActiveTool)("source") = PackAssetsSourceTextBox.Text
            If ConfigObject(ActiveTool)("destination") Is Nothing Then ConfigObject(ActiveTool).Add("destination", "")
            ConfigObject(ActiveTool)("destination") = PackAssetsDestinationTextBox.Text
            If ConfigObject(ActiveTool)("overwrite") Is Nothing Then ConfigObject(ActiveTool).Add("overwrite", "")
            ConfigObject(ActiveTool)("overwrite") = PackAssetsOverwriteCheckBox.Checked
            If ConfigObject(ActiveTool)("create_log") Is Nothing Then ConfigObject(ActiveTool).Add("create_log", "")
            ConfigObject(ActiveTool)("create_log") = PackAssetsLogCheckBox.Checked
            If ConfigObject(ActiveTool)("select_all") Is Nothing Then ConfigObject(ActiveTool).Add("select_all", "")
            ConfigObject(ActiveTool)("select_all") = PackAssetsSelectAllCheckBox.Checked
            If ConfigObject(ActiveTool)("size_width") Is Nothing Then ConfigObject(ActiveTool).Add("size_width", 0)
            ConfigObject(ActiveTool)("size_width") = Me.Size.Width
            If ConfigObject(ActiveTool)("size_height") Is Nothing Then ConfigObject(ActiveTool).Add("size_height", 0)
            ConfigObject(ActiveTool)("size_height") = Me.Size.Height
        ElseIf UnpackAssetsGroupBox.Visible = True Then
            ActiveTool = "unpack_assets"
            If ConfigObject(ActiveTool) Is Nothing Then
                Dim NewPrefs As New Newtonsoft.Json.Linq.JObject
                ConfigObject.Add(ActiveTool, NewPrefs)
            End If
            If ConfigObject(ActiveTool)("source") Is Nothing Then ConfigObject(ActiveTool).Add("source", "")
            ConfigObject(ActiveTool)("source") = UnpackAssetsSourceTextBox.Text
            If ConfigObject(ActiveTool)("destination") Is Nothing Then ConfigObject(ActiveTool).Add("destination", "")
            ConfigObject(ActiveTool)("destination") = UnpackAssetsDestinationTextBox.Text
            If ConfigObject(ActiveTool)("create_log") Is Nothing Then ConfigObject(ActiveTool).Add("create_log", "")
            ConfigObject(ActiveTool)("create_log") = UnpackAssetsLogCheckBox.Checked
            If ConfigObject(ActiveTool)("select_all") Is Nothing Then ConfigObject(ActiveTool).Add("select_all", "")
            ConfigObject(ActiveTool)("select_all") = UnpackAssetsSelectAllCheckBox.Checked
            If ConfigObject(ActiveTool)("size_width") Is Nothing Then ConfigObject(ActiveTool).Add("size_width", 0)
            ConfigObject(ActiveTool)("size_width") = Me.Size.Width
            If ConfigObject(ActiveTool)("size_height") Is Nothing Then ConfigObject(ActiveTool).Add("size_height", 0)
            ConfigObject(ActiveTool)("size_height") = Me.Size.Height
        End If

        SaveNewConfig(ConfigObject, ConfigFolderName, ConfigFileName)
    End Sub

    Private Sub LoadPrefsMenuItem_Click(sender As Object, e As EventArgs) Handles LoadPrefsMenuItem.Click
        Dim ConfigFileName As String = GlobalVariables.ConfigFolderName & "\" & GlobalVariables.ConfigFileName
        Dim ActiveTool As String

        If My.Computer.FileSystem.FileExists(ConfigFileName) Then
            Dim ConfigObject = GetSavedConfig(ConfigFileName)
            Dim SizePref As New Size

            ActiveTool = "global_config"
            If Not ConfigObject(ActiveTool) Is Nothing Then
                Dim LocationPref As New Point
                LocationPref.X = ConfigObject(ActiveTool)("location_x")
                LocationPref.Y = ConfigObject(ActiveTool)("location_y")
                Me.Location = LocationPref
            End If

            If TagAssetsGroupBox.Visible = True Then
                ActiveTool = "tag_assets"
                TagAssetsSourceTextBox.Text = ConfigObject(ActiveTool)("source")
                TagAssetsDefaultTagTextBox.Text = ConfigObject(ActiveTool)("default_tag")
                TagAssetsLogCheckBox.Checked = ConfigObject(ActiveTool)("create_log")
                TagAssetsSelectAllCheckBox.Checked = ConfigObject(ActiveTool)("select_all")
                TagAssetsSourceTextBox_LostFocus(sender, e)
                SizePref.Width = ConfigObject(ActiveTool)("size_width")
                SizePref.Height = ConfigObject(ActiveTool)("size_height")
                Me.Size = SizePref
            ElseIf ColorThemeApplyGroupBox.Visible = True Then
                ActiveTool = "apply_color_theme"
                If Not ConfigObject(ActiveTool) Is Nothing Then
                    Dim one As String = ConfigObject(ActiveTool)("theme_folder")
                    ApplyColorThemeColorThemeFolderTextBox.Text = ConfigObject(ActiveTool)("theme_folder")
                    ApplyColorThemeMapFolderTextBox.Text = ConfigObject(ActiveTool)("map_folder")
                    ApplyColorThemeBackupPalettesCheckBox.Checked = ConfigObject(ActiveTool)("create_backup")
                    SizePref.Width = ConfigObject(ActiveTool)("size_width")
                    SizePref.Height = ConfigObject(ActiveTool)("size_height")
                    Me.Size = SizePref
                End If
            ElseIf ColorThemeCreateGroupBox.Visible = True Then
                ActiveTool = "create_color_theme"
                If Not ConfigObject(ActiveTool) Is Nothing Then
                    CreateColorThemeColorThemeTextBox.Text = ConfigObject(ActiveTool)("theme_folder")
                    CreateColorThemeSourceMapTextBox.Text = ConfigObject(ActiveTool)("map_folder")
                    CreateColorThemeIncludeAllCustomColorsCheckBox.Checked = ConfigObject(ActiveTool)("include_custom")
                    CreateColorThemeIncludeNonPaletteColorsCheckBox.Checked = ConfigObject(ActiveTool)("include_nonpalette")
                    SizePref.Width = ConfigObject(ActiveTool)("size_width")
                    SizePref.Height = ConfigObject(ActiveTool)("size_height")
                    Me.Size = SizePref
                End If
            ElseIf ConvertAssetsGroupBox.Visible = True Then
                ActiveTool = "convert_assets"
                ConvertAssetsSourceTextBox.Text = ConfigObject(ActiveTool)("source")
                ConvertAssetsDestinationTextBox.Text = ConfigObject(ActiveTool)("destination")
                ConvertAssetsLogCheckBox.Checked = ConfigObject(ActiveTool)("create_log")
                ConvertAssetsSelectAllCheckBox.Checked = ConfigObject(ActiveTool)("select_all")
                ConvertAssetsSourceTextBox_LostFocus(sender, e)
                SizePref.Width = ConfigObject(ActiveTool)("size_width")
                SizePref.Height = ConfigObject(ActiveTool)("size_height")
                Me.Size = SizePref
            ElseIf ConvertPacksGroupBox.Visible = True Then
                ActiveTool = "convert_packs"
                ConvertPacksSourceTextBox.Text = ConfigObject(ActiveTool)("source")
                ConvertPacksDestinationTextBox.Text = ConfigObject(ActiveTool)("destination")
                ConvertPacksCleanUpCheckBox.Checked = ConfigObject(ActiveTool)("cleanup")
                ConvertPacksLogCheckBox.Checked = ConfigObject(ActiveTool)("create_log")
                ConvertPacksSelectAllCheckBox.Checked = ConfigObject("convertpacks")("select_all")
                ConvertPacksSourceTextBox_LostFocus(sender, e)
                SizePref.Width = ConfigObject(ActiveTool)("size_width")
                SizePref.Height = ConfigObject(ActiveTool)("size_height")
                Me.Size = SizePref
            ElseIf CopyAssetsGroupBox.Visible = True Then
                ActiveTool = "copy_assets"
                CopyAssetsSourceTextBox.Text = ConfigObject(ActiveTool)("source")
                CopyAssetsDestinationTextBox.Text = ConfigObject(ActiveTool)("destination")
                CopyAssetsCreateTagsCheckBox.Checked = ConfigObject(ActiveTool)("create_tags")
                CopyAssetsPortalsCheckBox.Checked = ConfigObject(ActiveTool)("separate_portals")
                CopyAssetsLogCheckBox.Checked = ConfigObject(ActiveTool)("create_log")
                CopyAssetsSelectAllCheckBox.Checked = ConfigObject(ActiveTool)("select_all")
                CopyAssetsSourceTextBox_LostFocus(sender, e)
                SizePref.Width = ConfigObject(ActiveTool)("size_width")
                SizePref.Height = ConfigObject(ActiveTool)("size_height")
                Me.Size = SizePref
            ElseIf CopyTilesGroupBox.Visible = True Then
                ActiveTool = "copy_tiles"
                CopyTilesSourceTextBox.Text = ConfigObject(ActiveTool)("source")
                CopyTilesDestinationTextBox.Text = ConfigObject(ActiveTool)("destination")
                CopyTilesLogCheckBox.Checked = ConfigObject(ActiveTool)("create_log")
                CopyTilesSelectAllCheckBox.Checked = ConfigObject(ActiveTool)("select_all")
                CopyTilesSourceTextBox_LostFocus(sender, e)
                SizePref.Width = ConfigObject(ActiveTool)("size_width")
                SizePref.Height = ConfigObject(ActiveTool)("size_height")
                Me.Size = SizePref
            ElseIf DataFilesGroupBox.Visible = True Then
                ActiveTool = "data_files"
                DataFilesSourceTextBox.Text = ConfigObject(ActiveTool)("source")
                DataFilesLogCheckBox.Checked = ConfigObject(ActiveTool)("create_log")
                DataFilesSourceTextBox_LostFocus(sender, e)
                SizePref.Width = ConfigObject(ActiveTool)("size_width")
                SizePref.Height = ConfigObject(ActiveTool)("size_height")
                Me.Size = SizePref
            ElseIf MapDetailsGroupBox.Visible = True Then
                ActiveTool = "map_details"
                MapDetailsSourceTextBox.Text = ConfigObject(ActiveTool)("source")
                MapDetailsLogCheckBox.Checked = ConfigObject(ActiveTool)("create_log")
                MapDetailsSelectAllCheckBox.Checked = ConfigObject(ActiveTool)("select_all")
                MapDetailsSourceTextBox_LostFocus(sender, e)
                SizePref.Width = ConfigObject(ActiveTool)("size_width")
                SizePref.Height = ConfigObject(ActiveTool)("size_height")
                Me.Size = SizePref
            ElseIf PackAssetsGroupBox.Visible = True Then
                ActiveTool = "pack_assets"
                PackAssetsSourceTextBox.Text = ConfigObject(ActiveTool)("source")
                PackAssetsDestinationTextBox.Text = ConfigObject(ActiveTool)("destination")
                PackAssetsOverwriteCheckBox.Checked = ConfigObject(ActiveTool)("overwrite")
                PackAssetsLogCheckBox.Checked = ConfigObject(ActiveTool)("create_log")
                PackAssetsSelectAllCheckBox.Checked = ConfigObject(ActiveTool)("select_all")
                PackAssetsSourceTextBox_LostFocus(sender, e)
                SizePref.Width = ConfigObject(ActiveTool)("size_width")
                SizePref.Height = ConfigObject(ActiveTool)("size_height")
                Me.Size = SizePref
            ElseIf UnpackAssetsGroupBox.Visible = True Then
                ActiveTool = "unpack_assets"
                UnpackAssetsSourceTextBox.Text = ConfigObject(ActiveTool)("source")
                UnpackAssetsDestinationTextBox.Text = ConfigObject(ActiveTool)("destination")
                UnpackAssetsLogCheckBox.Checked = ConfigObject(ActiveTool)("create_log")
                UnpackAssetsSelectAllCheckBox.Checked = ConfigObject(ActiveTool)("select_all")
                UnpackAssetsSourceTextBox_LostFocus(sender, e)
                SizePref.Width = ConfigObject(ActiveTool)("size_width")
                SizePref.Height = ConfigObject(ActiveTool)("size_height")
                Me.Size = SizePref
            End If
        End If
    End Sub

    '###### Tag Assets Group Box ######
    Private Sub TagAssetsSourceTextBox_LostFocus(sender As Object, e As EventArgs) Handles TagAssetsSourceTextBox.LostFocus
        TagAssetsCheckedListBox.Items.Clear()
        Dim SourceFolderName As String
        Dim IsFolderNameValid As Boolean
        Dim DoesFolderExist As Boolean
        SourceFolderName = TagAssetsSourceTextBox.Text
        IsFolderNameValid = IsValidPathName(SourceFolderName)
        DoesFolderExist = System.IO.Directory.Exists(SourceFolderName)
        If IsFolderNameValid And DoesFolderExist Then
            If My.Computer.FileSystem.DirectoryExists(SourceFolderName & "\textures\objects") Then
                Dim SourceFolderInfo = My.Computer.FileSystem.GetDirectoryInfo(SourceFolderName)
                TagAssetsSourceTextBox.Text = SourceFolderInfo.Parent.FullName
                TagAssetsCheckedListBox.Items.Add(SourceFolderInfo.Name)
                TagAssetsCheckedListBox.SetItemChecked(TagAssetsCheckedListBox.Items.Count - 1, True)
            Else
                For Each TagFolder As String In My.Computer.FileSystem.GetDirectories(SourceFolderName)
                    If My.Computer.FileSystem.DirectoryExists(TagFolder & "\textures\objects") Then
                        Dim FolderName As New System.IO.DirectoryInfo(TagFolder)
                        TagAssetsCheckedListBox.Items.Add(FolderName.Name)
                        If TagAssetsSelectAllCheckBox.Checked Then TagAssetsCheckedListBox.SetItemChecked(TagAssetsCheckedListBox.Items.Count - 1, True)
                    End If
                Next
            End If
        End If
    End Sub

    Private Sub TagAssetsBrowseButton_Click(sender As Object, e As EventArgs) Handles TagAssetsBrowseButton.Click
        TagAssetsCheckedListBox.Items.Clear()
        TagAssetsSourceBrowserDialog.ShowDialog()
        TagAssetsSourceTextBox.Text = TagAssetsSourceBrowserDialog.SelectedPath

        Dim SourceFolderName As String
        Dim IsFolderNameValid As Boolean
        Dim DoesFolderExist As Boolean
        SourceFolderName = TagAssetsSourceTextBox.Text
        IsFolderNameValid = IsValidPathName(SourceFolderName)
        DoesFolderExist = System.IO.Directory.Exists(SourceFolderName)
        If IsFolderNameValid And DoesFolderExist Then
            If My.Computer.FileSystem.DirectoryExists(SourceFolderName & "\textures\objects") Then
                Dim SourceFolderInfo = My.Computer.FileSystem.GetDirectoryInfo(SourceFolderName)
                TagAssetsSourceTextBox.Text = SourceFolderInfo.Parent.FullName
                TagAssetsCheckedListBox.Items.Add(SourceFolderInfo.Name)
                TagAssetsCheckedListBox.SetItemChecked(TagAssetsCheckedListBox.Items.Count - 1, True)
            Else
                For Each TagFolder As String In My.Computer.FileSystem.GetDirectories(SourceFolderName)
                    If My.Computer.FileSystem.DirectoryExists(TagFolder & "\textures\objects") Then
                        Dim FolderName As New System.IO.DirectoryInfo(TagFolder)
                        TagAssetsCheckedListBox.Items.Add(FolderName.Name)
                        If TagAssetsSelectAllCheckBox.Checked Then TagAssetsCheckedListBox.SetItemChecked(TagAssetsCheckedListBox.Items.Count - 1, True)
                    End If
                Next
            End If
        End If
    End Sub

    Private Sub TagAssetsSelectAllCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles TagAssetsSelectAllCheckBox.CheckedChanged
        If TagAssetsSelectAllCheckBox.Checked Then
            TagAssetsSelectAllButton_Click(sender, e)
        Else
            TagAssetsSelectNoneButton_Click(sender, e)
        End If
    End Sub

    Private Sub TagAssetsSelectAllButton_Click(sender As Object, e As EventArgs) Handles TagAssetsSelectAllButton.Click
        Dim Count As Integer
        For Count = 0 To TagAssetsCheckedListBox.Items.Count - 1
            TagAssetsCheckedListBox.SetItemChecked(Count, True)
        Next
    End Sub

    Private Sub TagAssetsSelectNoneButton_Click(sender As Object, e As EventArgs) Handles TagAssetsSelectNoneButton.Click
        Dim Count As Integer
        For Count = 0 To TagAssetsCheckedListBox.Items.Count - 1
            TagAssetsCheckedListBox.SetItemChecked(Count, False)
        Next
    End Sub

    Private Sub TagAssetsStartButton_Click(sender As Object, e As EventArgs) Handles TagAssetsStartButton.Click
        Dim SourceFolderName As String = TagAssetsSourceTextBox.Text
        Dim IsSourceFolderNameValid As String
        Dim DoesSourceFolderExist As Boolean
        Dim DefaultTag As String = TagAssetsDefaultTagTextBox.Text
        Dim CreateLog As Boolean = TagAssetsLogCheckBox.Checked
        Dim LogFileName As String = GlobalVariables.LogsFolder & "\TagAssets.log"
        Dim TagSource As String
        Dim Indent As String = "    " '4 spaces
        Dim SubIndent As String = "        " '8 spaces
        Dim Message As String

        IsSourceFolderNameValid = IsValidPathName(SourceFolderName)
        DoesSourceFolderExist = System.IO.Directory.Exists(SourceFolderName)
        If SourceFolderName <> "" And IsSourceFolderNameValid And DoesSourceFolderExist Then
            If TagAssetsCheckedListBox.CheckedItems.Count >= 1 Then
                OutputForm.OutputTextBox.Text = ""
                OutputForm.Show()
                OutputForm.BringToFront()
                Message = "### Starting selected folders at " & DateTime.Now & "." & vbCrLf & vbCrLf
                OutputForm.OutputTextBox.AppendText(Message)
                If CreateLog Then My.Computer.FileSystem.WriteAllText(LogFileName, Message, False)
                Dim SelectedFolders = TagAssetsCheckedListBox.CheckedItems
                For Each AssetFolder In SelectedFolders
                    If SourceFolderName.EndsWith("\") Then
                        TagSource = SourceFolderName & AssetFolder
                    Else
                        TagSource = SourceFolderName & "\" & AssetFolder
                    End If
                    Message = Indent & "Starting " & AssetFolder & " at " & DateTime.Now & vbCrLf
                    OutputForm.OutputTextBox.AppendText(Message)
                    If CreateLog Then My.Computer.FileSystem.WriteAllText(LogFileName, Message, True)
                    TagAssetsSub(TagSource, AssetFolder, DefaultTag, CreateLog, LogFileName, SubIndent)
                    Message = Indent & "Finished " & AssetFolder & " at " & DateTime.Now & vbCrLf & vbCrLf
                    OutputForm.OutputTextBox.AppendText(Message)
                    If CreateLog Then My.Computer.FileSystem.WriteAllText(LogFileName, Message, True)
                Next
                Message = "### Finished selected folders at " & DateTime.Now & "." & vbCrLf
                OutputForm.OutputTextBox.AppendText(Message)
                If CreateLog Then My.Computer.FileSystem.WriteAllText(LogFileName, Message, True)
            Else
                MsgBox("Nothing was selected.")
            End If
        Else
            MsgBox("Source folder name is either invalid or does not exist.")
        End If

    End Sub

    '###### Color Theme: Apply to Map
    Private Sub ApplyColorThemeColorThemeFolderBrowseButton_Click(sender As Object, e As EventArgs) Handles ApplyColorThemeColorThemeFolderBrowseButton.Click
        ApplyColorThemeColorThemeListBox.Items.Clear()
        ApplyColorThemeThemeFolderBrowserDialog.ShowDialog()
        ApplyColorThemeColorThemeFolderTextBox.Text = ApplyColorThemeThemeFolderBrowserDialog.SelectedPath

        Dim IsFolderNameValid As Boolean
        Dim DoesFolderExist As Boolean
        Dim ThemeFolderName As String

        If ApplyColorThemeColorThemeFolderTextBox.Text.EndsWith("\") Then ApplyColorThemeColorThemeFolderTextBox.Text.TrimEnd("\")
        ThemeFolderName = ApplyColorThemeColorThemeFolderTextBox.Text
        IsFolderNameValid = IsValidPathName(ThemeFolderName)
        DoesFolderExist = Directory.Exists(ThemeFolderName)
        If IsFolderNameValid And DoesFolderExist Then
            For Each ThemeFile As String In My.Computer.FileSystem.GetFiles(ThemeFolderName)
                Dim ThemeName As New DirectoryInfo(ThemeFile)
                If ThemeName.Extension = ".ddtools_colortheme" Then
                    ApplyColorThemeColorThemeListBox.Items.Add(ThemeName.Name)
                End If
            Next
        Else
            MsgBox("Color Theme Folder name is either invalid or does not exist.")
        End If
    End Sub

    Private Sub ApplyColorThemeColorThemeFolderTextBox_LostFocus(sender As Object, e As EventArgs) Handles ApplyColorThemeColorThemeFolderTextBox.LostFocus
        ApplyColorThemeColorThemeListBox.Items.Clear()

        If ApplyColorThemeColorThemeFolderTextBox.Text <> "" Then
            Dim IsFolderNameValid As Boolean
            Dim DoesFolderExist As Boolean
            Dim ThemeFolderName As String

            If ApplyColorThemeColorThemeFolderTextBox.Text.EndsWith("\") Then ApplyColorThemeColorThemeFolderTextBox.Text.TrimEnd("\")
            ThemeFolderName = ApplyColorThemeColorThemeFolderTextBox.Text
            IsFolderNameValid = IsValidPathName(ThemeFolderName)
            DoesFolderExist = Directory.Exists(ThemeFolderName)
            If IsFolderNameValid And DoesFolderExist Then
                For Each ThemeFile As String In My.Computer.FileSystem.GetFiles(ThemeFolderName)
                    Dim ThemeName As New DirectoryInfo(ThemeFile)
                    If ThemeName.Extension = ".ddtools_colortheme" Then
                        ApplyColorThemeColorThemeListBox.Items.Add(ThemeName.Name)
                    End If
                Next
            Else
                MsgBox("Color Theme Folder name is either invalid or does not exist.")
            End If
        End If
    End Sub

    Private Sub ApplyColorThemeMapFolderBrowseButton_Click(sender As Object, e As EventArgs) Handles ApplyColorThemeMapFolderBrowseButton.Click
        ApplyColorThemeMapListBox.Items.Clear()
        ApplyColorThemeMapFolderBrowserDialog.ShowDialog()
        ApplyColorThemeMapFolderTextBox.Text = ApplyColorThemeMapFolderBrowserDialog.SelectedPath

        Dim IsFolderNameValid As Boolean
        Dim DoesFolderExist As Boolean
        Dim MapFolderName As String

        If ApplyColorThemeMapFolderTextBox.Text.EndsWith("\") Then ApplyColorThemeMapFolderTextBox.Text.TrimEnd("\")
        MapFolderName = ApplyColorThemeMapFolderTextBox.Text
        IsFolderNameValid = IsValidPathName(MapFolderName)
        DoesFolderExist = Directory.Exists(MapFolderName)
        If IsFolderNameValid And DoesFolderExist Then
            For Each MapFile As String In My.Computer.FileSystem.GetFiles(MapFolderName)
                Dim MapName As New DirectoryInfo(MapFile)
                If MapName.Extension = ".dungeondraft_map" Then
                    ApplyColorThemeMapListBox.Items.Add(MapName.Name)
                End If
            Next
        Else
            MsgBox("Map Folder name is either invalid or does not exist.")
        End If
    End Sub

    Private Sub ApplyColorThemeMapFolderTextBox_LostFocus(sender As Object, e As EventArgs) Handles ApplyColorThemeMapFolderTextBox.LostFocus
        ApplyColorThemeMapListBox.Items.Clear()

        If ApplyColorThemeMapFolderTextBox.Text <> "" Then
            Dim IsFolderNameValid As Boolean
            Dim DoesFolderExist As Boolean
            Dim MapFolderName As String

            If ApplyColorThemeMapFolderTextBox.Text.EndsWith("\") Then ApplyColorThemeMapFolderTextBox.Text.TrimEnd("\")
            MapFolderName = ApplyColorThemeMapFolderTextBox.Text
            IsFolderNameValid = IsValidPathName(MapFolderName)
            DoesFolderExist = Directory.Exists(MapFolderName)
            If IsFolderNameValid And DoesFolderExist Then
                For Each MapFile As String In My.Computer.FileSystem.GetFiles(MapFolderName)
                    Dim MapName As New DirectoryInfo(MapFile)
                    If MapName.Extension = ".dungeondraft_map" Then
                        ApplyColorThemeMapListBox.Items.Add(MapName.Name)
                    End If
                Next
            Else
                MsgBox("Map Folder name is either invalid or does not exist.")
            End If
        End If
    End Sub

    Private Sub ApplyColorThemeStartButton_Click(sender As Object, e As EventArgs) Handles ApplyColorThemeStartButton.Click
        Dim ThemeFolderName As String = ApplyColorThemeColorThemeFolderTextBox.Text
        Dim MapFolderName As String = ApplyColorThemeMapFolderTextBox.Text

        If ThemeFolderName <> "" And MapFolderName <> "" Then
            'empty folder names
            If IsValidPathName(ThemeFolderName) And IsValidPathName(MapFolderName) Then
                If Directory.Exists(ThemeFolderName) And Directory.Exists(MapFolderName) Then
                    If ApplyColorThemeColorThemeListBox.SelectedIndex >= 0 And ApplyColorThemeMapListBox.SelectedIndex >= 0 Then
                        Dim ThemeFileName As String = ApplyColorThemeColorThemeListBox.SelectedItem
                        Dim ThemeFile As String = ThemeFolderName & "\" & ThemeFileName
                        Dim MapFileName As String = ApplyColorThemeMapListBox.SelectedItem
                        Dim MapFile As String = MapFolderName & "\" & MapFileName

                        If File.Exists(ThemeFile) And File.Exists(MapFile) Then
                            Dim MessageBoxResult As DialogResult = MessageBox.Show("You are about to override the color palettes of " & MapFile & " with the color palettes of " & ThemeFile & "." & vbCrLf & vbCrLf &
                                                               "Are you sure you wish to continue?", "Override Color Palette?", MessageBoxButtons.YesNo)
                            If MessageBoxResult = DialogResult.Yes Then
                                Dim MapBaseFileName As String = Path.GetFileNameWithoutExtension(MapFileName)
                                Dim BackupFolderName As String = GlobalVariables.MyDocuments & "\DDTools\ColorThemes\Backups"
                                If Not Directory.Exists(BackupFolderName) Then Directory.CreateDirectory(BackupFolderName)
                                Dim Year As String = Today.Year
                                Dim Month As String = Today.Month
                                Dim Day As String = Today.Day
                                If Month.Length = 1 Then Month = "0" & Month
                                If Day.Length = 1 Then Month = "0" & Day
                                Dim BackupFileName As String = MapBaseFileName & "_" & Year & "-" & Month & "-" & Day & ".ddtools_colortheme"
                                Dim BackupFile As String = BackupFolderName & "\" & BackupFileName
                                If ApplyColorThemeBackupPalettesCheckBox.Checked Then
                                    Dim BackupTheme As Linq.JObject = GetMapColors(MapFile)
                                    SaveColorTheme(BackupFile, BackupTheme)
                                End If
                                Dim ColorTheme As Linq.JObject = GetColorTheme(ThemeFile)
                                ApplyColorTheme(ColorTheme, MapFile)
                                Dim ShowMessage As String = ThemeFile & " has been applied to " & MapFile
                                If ApplyColorThemeBackupPalettesCheckBox.Checked Then
                                    ShowMessage &= vbCrLf & vbCrLf & "The previous palettes for " & MapFile & " were backed up to " & BackupFile
                                End If
                                MsgBox(ShowMessage)
                            End If
                        Else
                            MsgBox("One or more specified files do not exist.")
                        End If
                    Else
                        MsgBox("You must select both a color theme file and a map file.")
                    End If
                Else
                    MsgBox("One or more specified folders do not exist.")
                End If
            Else
                MsgBox("One or more folder names are invalid.")
            End If
        Else
            MsgBox("One or more folder names have been left blank.")
        End If
    End Sub

    '###### Color Theme: Create from Map
    Private Sub CreateColorThemeSourceMapOpenButton_Click(sender As Object, e As EventArgs) Handles CreateColorThemeSourceMapOpenButton.Click
        Dim MapFile As String = CreateColorThemeSourceMapTextBox.Text
        Dim MapFolder As String = ""
        If MapFile <> "" Then
            If IsValidPathName(MapFile) Then
                If File.Exists(MapFile) Then
                    MapFolder = Path.GetDirectoryName(MapFile)
                ElseIf Directory.Exists(MapFile) Then
                    MapFolder = MapFile
                End If
            End If
        End If

        If MapFolder <> "" Then CreateColorThemeOpenFileDialog.InitialDirectory = MapFolder

        CreateColorThemeOpenFileDialog.FileName = ""
        CreateColorThemeOpenFileDialog.Filter = "Dungeondraft Map Files|*.dungeondraft_map"
        If CreateColorThemeOpenFileDialog.ShowDialog() = DialogResult.OK Then
            CreateColorThemeSourceMapTextBox.Text = CreateColorThemeOpenFileDialog.FileName
        End If
    End Sub

    Private Sub CreateColorThemeSaveButton_Click(sender As Object, e As EventArgs) Handles CreateColorThemeSaveButton.Click
        Dim ThemeFile As String = CreateColorThemeColorThemeTextBox.Text
        Dim ThemeFolder As String = ""
        If ThemeFile <> "" Then
            If IsValidPathName(ThemeFile) Then
                If File.Exists(ThemeFile) Then
                    ThemeFolder = Path.GetDirectoryName(ThemeFile)
                ElseIf Directory.Exists(ThemeFile) Then
                    ThemeFile = ThemeFile
                End If
            End If
        End If

        If ThemeFolder <> "" Then CreateColorThemeOpenFileDialog.InitialDirectory = ThemeFolder

        CreateColorThemeSaveFileDialog.OverwritePrompt = False
        CreateColorThemeSaveFileDialog.Filter = "DDTools Color Theme|*.ddtools_colortheme"
        If CreateColorThemeSaveFileDialog.ShowDialog() = DialogResult.OK Then
            CreateColorThemeColorThemeTextBox.Text = CreateColorThemeSaveFileDialog.FileName
            CreateColorThemeStartButton.PerformClick()
        End If
    End Sub

    Private Sub CreateColorThemeStartButton_Click(sender As Object, e As EventArgs) Handles CreateColorThemeStartButton.Click
        Dim MapFileName As String = CreateColorThemeSourceMapTextBox.Text
        Dim ThemeFileName As String = CreateColorThemeColorThemeTextBox.Text
        Dim ValidMapPath As Boolean = IsValidPathName(MapFileName)
        Dim ValidThemePath As Boolean = IsValidPathName(ThemeFileName)
        Dim ShowMessage As String

        If MapFileName <> "" And ThemeFileName <> "" Then
            If ValidMapPath And ValidThemePath Then
                Dim MapFileExists As Boolean = File.Exists(MapFileName)
                Dim ThemeFolder As String = Path.GetDirectoryName(ThemeFileName)
                Dim ThemeFolderExists As Boolean = Directory.Exists(ThemeFolder)

                If MapFileExists And ThemeFolderExists Then
                    If Directory.Exists(ThemeFileName) Then
                        ShowMessage = "No filename specified for Color Theme."
                        MsgBox(ShowMessage)
                    Else
                        Dim ThemeFileExists As Boolean = False
                        Dim MessageBoxResult As DialogResult
                        If Not ThemeFileName.EndsWith(".ddtools_colortheme") Then ThemeFileName &= ".ddtools_colortheme"
                        If File.Exists(ThemeFileName) Then
                            ThemeFileExists = True
                            MessageBoxResult = MessageBox.Show("File Exists: " & ThemeFileName & vbCrLf & vbCrLf & "Do you want to overwrite it?", "Overwrite?", MessageBoxButtons.YesNo)
                        End If

                        If Not ThemeFileExists Or MessageBoxResult = DialogResult.Yes Then
                            Dim ColorTheme As Linq.JObject = GetMapColors(MapFileName)
                            SaveColorTheme(ThemeFileName, ColorTheme)
                            MsgBox("The color palettes from " & MapFileName & " have been saved to " & ThemeFileName & ".")
                        End If
                    End If
                Else
                    ShowMessage = ""
                    If Not MapFileExists Then ShowMessage = "File does not exist: " & MapFileName
                    If Not ThemeFolderExists Then
                        If ShowMessage <> "" Then ShowMessage &= vbCrLf
                        ShowMessage &= "Folder does not exist: " & ThemeFolder
                    End If
                    MsgBox(ShowMessage)
                End If
            Else
                ShowMessage = ""
                If Not ValidMapPath Then ShowMessage &= "Invalid map path: " & MapFileName
                If Not ValidMapPath Then
                    If ShowMessage <> "" Then ShowMessage &= vbCrLf
                    ShowMessage &= "Invalid theme path: " & ThemeFileName
                End If
                MsgBox(ShowMessage)
            End If
        End If
    End Sub

    '###### Convert Assets Group Box ######
    Private Sub ConvertAssetsSourceTextBox_LostFocus(sender As Object, e As EventArgs) Handles ConvertAssetsSourceTextBox.LostFocus
        ConvertAssetsCheckedListBox.Items.Clear()
        Dim UserFolder As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        Dim SourceFolderName As String
        Dim IsFolderNameValid As Boolean
        Dim DoesFolderExist As Boolean
        Dim SourceFolder As System.IO.DirectoryInfo
        Dim DestinationFolderName As String

        SourceFolderName = ConvertAssetsSourceTextBox.Text
        IsFolderNameValid = IsValidPathName(SourceFolderName)
        DoesFolderExist = System.IO.Directory.Exists(SourceFolderName)
        If IsFolderNameValid And DoesFolderExist Then
            SourceFolder = New System.IO.DirectoryInfo(SourceFolderName)
            DestinationFolderName = UserFolder & "\Dungeondraft\Converted Assets\" & SourceFolder.Name
            If ConvertAssetsDestinationTextBox.Text = "" Then ConvertAssetsDestinationTextBox.Text = DestinationFolderName
            For Each AssetFolder As String In My.Computer.FileSystem.GetDirectories(SourceFolderName)
                Dim FolderName As New System.IO.DirectoryInfo(AssetFolder)
                ConvertAssetsCheckedListBox.Items.Add(FolderName.Name)
                If ConvertAssetsSelectAllCheckBox.Checked Then ConvertAssetsCheckedListBox.SetItemChecked(ConvertAssetsCheckedListBox.Items.Count - 1, True)
            Next
        End If
    End Sub

    Private Sub ConvertAssetsSourceBrowseButton_Click(sender As Object, e As EventArgs) Handles ConvertAssetsSourceBrowseButton.Click
        ConvertAssetsCheckedListBox.Items.Clear()
        ConvertAssetsSourceBrowserDialog.ShowDialog()
        ConvertAssetsSourceTextBox.Text = ConvertAssetsSourceBrowserDialog.SelectedPath

        Dim UserFolder As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        Dim SourceFolderName As String
        Dim IsFolderNameValid As Boolean
        Dim DoesFolderExist As Boolean
        Dim SourceFolder As System.IO.DirectoryInfo
        Dim DestinationFolderName As String

        SourceFolderName = ConvertAssetsSourceTextBox.Text
        IsFolderNameValid = IsValidPathName(SourceFolderName)
        DoesFolderExist = System.IO.Directory.Exists(SourceFolderName)
        If IsFolderNameValid And DoesFolderExist Then
            SourceFolder = New System.IO.DirectoryInfo(SourceFolderName)
            DestinationFolderName = UserFolder & "\Dungeondraft\Converted Assets\" & SourceFolder.Name
            If ConvertAssetsDestinationTextBox.Text = "" Then ConvertAssetsDestinationTextBox.Text = DestinationFolderName
            For Each AssetFolder As String In My.Computer.FileSystem.GetDirectories(SourceFolderName)
                Dim FolderName As New System.IO.DirectoryInfo(AssetFolder)
                ConvertAssetsCheckedListBox.Items.Add(FolderName.Name)
                If ConvertAssetsSelectAllCheckBox.Checked Then ConvertAssetsCheckedListBox.SetItemChecked(ConvertAssetsCheckedListBox.Items.Count - 1, True)
            Next
        End If
    End Sub

    Private Sub ConvertAssetsDestinationBrowseButton_Click(sender As Object, e As EventArgs) Handles ConvertAssetsDestinationBrowseButton.Click
        ConvertAssetsDestinationBrowserDialog.ShowDialog()
        ConvertAssetsDestinationTextBox.Text = ConvertAssetsDestinationBrowserDialog.SelectedPath
    End Sub

    Private Sub ConvertAssetsSelectAllCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles ConvertAssetsSelectAllCheckBox.CheckedChanged
        If ConvertAssetsSelectAllCheckBox.Checked Then
            ConvertAssetsSelectAllButton_Click(sender, e)
        Else
            ConvertAssetsSelectNoneButton_Click(sender, e)
        End If
    End Sub

    Private Sub ConvertAssetsSelectAllButton_Click(sender As Object, e As EventArgs) Handles ConvertAssetsSelectAllButton.Click
        Dim Count As Integer
        For Count = 0 To ConvertAssetsCheckedListBox.Items.Count - 1
            ConvertAssetsCheckedListBox.SetItemChecked(Count, True)
        Next
    End Sub
    Private Sub ConvertAssetsSelectNoneButton_Click(sender As Object, e As EventArgs) Handles ConvertAssetsSelectNoneButton.Click
        Dim Count As Integer
        For Count = 0 To ConvertAssetsCheckedListBox.Items.Count - 1
            ConvertAssetsCheckedListBox.SetItemChecked(Count, False)
        Next
    End Sub
    Private Sub ConvertAssetsStartButton_Click(sender As Object, e As EventArgs) Handles ConvertAssetsStartButton.Click

        Dim SourceFolderName As String = ConvertAssetsSourceTextBox.Text
        Dim DestinationFolderName As String = ConvertAssetsDestinationTextBox.Text
        Dim IsSourceFolderNameValid As String
        Dim DoesSourceFolderExist As Boolean
        Dim IsDestinationFolderNameValid As String
        Dim LogFileName As String = GlobalVariables.LogsFolder & "\ConvertAssets.log"
        Dim CreateLog = ConvertAssetsLogCheckBox.Checked
        Dim CopySource As String
        Dim CopyDestination As String
        Dim Indent As String = "    " '4 spaces
        Dim SubIndent As String = "        " '8 spaces
        Dim Message As String

        IsSourceFolderNameValid = IsValidPathName(SourceFolderName)
        DoesSourceFolderExist = System.IO.Directory.Exists(SourceFolderName)

        IsDestinationFolderNameValid = IsValidPathName(DestinationFolderName)

        If SourceFolderName <> "" And IsSourceFolderNameValid And DoesSourceFolderExist Then
            If DestinationFolderName <> "" And IsDestinationFolderNameValid Then
                If ConvertAssetsCheckedListBox.CheckedItems.Count >= 1 Then
                    OutputForm.OutputTextBox.Text = ""
                    OutputForm.Show()
                    OutputForm.BringToFront()
                    Message = "### Starting selected folders at " & DateTime.Now & "." & vbCrLf & vbCrLf
                    OutputForm.OutputTextBox.AppendText(Message)
                    If CreateLog Then My.Computer.FileSystem.WriteAllText(LogFileName, Message, False)
                    Dim SelectedFolders = ConvertAssetsCheckedListBox.CheckedItems
                    For Each AssetFolder In SelectedFolders
                        If SourceFolderName.EndsWith("\") Then
                            CopySource = SourceFolderName & AssetFolder
                        Else
                            CopySource = SourceFolderName & "\" & AssetFolder
                        End If

                        If DestinationFolderName.EndsWith("\") Then
                            CopyDestination = DestinationFolderName & AssetFolder & " (webp)"
                        Else
                            CopyDestination = DestinationFolderName & "\" & AssetFolder & " (webp)"
                        End If

                        Message = Indent & "Starting " & AssetFolder & " at " & DateTime.Now & vbCrLf
                        OutputForm.OutputTextBox.AppendText(Message)
                        If CreateLog Then My.Computer.FileSystem.WriteAllText(LogFileName, Message, True)
                        ConvertAssets(CopySource, CopyDestination, CreateLog, LogFileName, SubIndent)
                        Message = Indent & "Finished " & AssetFolder & " at " & DateTime.Now & vbCrLf & vbCrLf
                        OutputForm.OutputTextBox.AppendText(Message)
                        If CreateLog Then My.Computer.FileSystem.WriteAllText(LogFileName, Message, True)
                    Next
                    Message = "### Finished selected folders at " & DateTime.Now & "." & vbCrLf
                    OutputForm.OutputTextBox.AppendText(Message)
                    If CreateLog Then My.Computer.FileSystem.WriteAllText(LogFileName, Message, True)
                End If
            Else
                MsgBox("Destination folder name is invalid.")
            End If
        Else
            MsgBox("Source folder name is either invalid or does not exist.")
        End If
    End Sub

    '###### Convert Packs Group Box ######
    Private Sub ConvertPacksSourceTextBox_LostFocus(sender As Object, e As EventArgs) Handles ConvertPacksSourceTextBox.LostFocus
        ConvertPacksCheckedListBox.Items.Clear()

        Dim UserFolder As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        Dim SourceFolderName As String
        Dim IsFolderNameValid As Boolean
        Dim DoesFolderExist As Boolean
        Dim SourceFolder As System.IO.DirectoryInfo
        Dim DestinationFolderName As String

        SourceFolderName = ConvertPacksSourceTextBox.Text
        IsFolderNameValid = IsValidPathName(SourceFolderName)
        DoesFolderExist = System.IO.Directory.Exists(SourceFolderName)
        If IsFolderNameValid And DoesFolderExist Then
            SourceFolder = New System.IO.DirectoryInfo(SourceFolderName)
            DestinationFolderName = UserFolder & "\Dungeondraft\" & SourceFolder.Name
            If ConvertPacksDestinationTextBox.Text = "" Then ConvertPacksDestinationTextBox.Text = DestinationFolderName
            For Each PackFile As String In My.Computer.FileSystem.GetFiles(SourceFolderName)
                Dim PackName As New System.IO.DirectoryInfo(PackFile)
                If PackName.Extension = ".dungeondraft_pack" Then
                    ConvertPacksCheckedListBox.Items.Add(PackName.Name)
                    If ConvertPacksSelectAllCheckBox.Checked Then ConvertPacksCheckedListBox.SetItemChecked(ConvertPacksCheckedListBox.Items.Count - 1, True)
                End If
            Next
        End If
    End Sub

    Private Sub ConvertPacksSourceBrowseButton_Click(sender As Object, e As EventArgs) Handles ConvertPacksSourceBrowseButton.Click
        ConvertPacksCheckedListBox.Items.Clear()
        ConvertPacksSourceBrowserDialog.ShowDialog()
        ConvertPacksSourceTextBox.Text = ConvertPacksSourceBrowserDialog.SelectedPath

        Dim UserFolder As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        Dim SourceFolderName As String
        Dim IsFolderNameValid As Boolean
        Dim DoesFolderExist As Boolean
        Dim SourceFolder As System.IO.DirectoryInfo
        Dim DestinationFolderName As String

        SourceFolderName = ConvertPacksSourceTextBox.Text
        IsFolderNameValid = IsValidPathName(SourceFolderName)
        DoesFolderExist = System.IO.Directory.Exists(SourceFolderName)
        If IsFolderNameValid And DoesFolderExist Then
            SourceFolder = New System.IO.DirectoryInfo(SourceFolderName)
            DestinationFolderName = UserFolder & "\Dungeondraft\" & SourceFolder.Name
            If ConvertPacksDestinationTextBox.Text = "" Then ConvertPacksDestinationTextBox.Text = DestinationFolderName
            For Each PackFile As String In My.Computer.FileSystem.GetFiles(SourceFolderName)
                Dim PackName As New System.IO.DirectoryInfo(PackFile)
                If PackName.Extension = ".dungeondraft_pack" Then
                    ConvertPacksCheckedListBox.Items.Add(PackName.Name)
                    If ConvertPacksSelectAllCheckBox.Checked Then ConvertPacksCheckedListBox.SetItemChecked(ConvertPacksCheckedListBox.Items.Count - 1, True)
                End If
            Next
        End If
    End Sub

    Private Sub ConvertPacksDestinationBrowseButton_Click(sender As Object, e As EventArgs) Handles ConvertPacksDestinationBrowseButton.Click
        ConvertPacksDestinationBrowserDialog.ShowDialog()
        ConvertPacksDestinationTextBox.Text = ConvertPacksDestinationBrowserDialog.SelectedPath
    End Sub

    Private Sub ConvertPacksSelectAllCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles ConvertPacksSelectAllCheckBox.CheckedChanged
        If ConvertPacksSelectAllCheckBox.Checked Then
            ConvertPacksSelectAllButton_Click(sender, e)
        Else
            ConvertPacksSelectNoneButton_Click(sender, e)
        End If
    End Sub

    Private Sub ConvertPacksSelectAllButton_Click(sender As Object, e As EventArgs) Handles ConvertPacksSelectAllButton.Click
        Dim Count As Integer
        For Count = 0 To ConvertPacksCheckedListBox.Items.Count - 1
            ConvertPacksCheckedListBox.SetItemChecked(Count, True)
        Next
    End Sub

    Private Sub ConvertPacksSelectNoneButton_Click(sender As Object, e As EventArgs) Handles ConvertPacksSelectNoneButton.Click
        Dim Count As Integer
        For Count = 0 To ConvertPacksCheckedListBox.Items.Count - 1
            ConvertPacksCheckedListBox.SetItemChecked(Count, False)
        Next
    End Sub

    Private Sub ConvertPacksStartButton_Click(sender As Object, e As EventArgs) Handles ConvertPacksStartButton.Click
        Dim SourceFolderName As String = ConvertPacksSourceTextBox.Text
        Dim DestinationFolderName As String = ConvertPacksDestinationTextBox.Text
        Dim IsSourceFolderNameValid As String
        Dim DoesSourceFolderExist As Boolean
        Dim IsDestinationFolderNameValid As String
        Dim CleanUp As Boolean = ConvertPacksCleanUpCheckBox.Checked
        Dim LogFileName As String = GlobalVariables.LogsFolder & "\ConvertPacks.log"
        Dim Message As String
        Dim CreateLog = ConvertPacksLogCheckBox.Checked
        Dim Indent As String = "    " '4 spaces
        Dim SubIndent As String = "        " '8 spaces

        IsSourceFolderNameValid = IsValidPathName(SourceFolderName)
        DoesSourceFolderExist = System.IO.Directory.Exists(SourceFolderName)
        IsDestinationFolderNameValid = IsValidPathName(DestinationFolderName)

        If SourceFolderName <> "" And IsSourceFolderNameValid And DoesSourceFolderExist Then
            If DestinationFolderName <> "" And IsDestinationFolderNameValid Then
                If ConvertPacksCheckedListBox.CheckedItems.Count >= 1 Then
                    OutputForm.OutputTextBox.Text = ""
                    OutputForm.Show()
                    OutputForm.BringToFront()
                    Message = "### Starting selected packs at " & DateTime.Now & "." & vbCrLf & vbCrLf
                    OutputForm.OutputTextBox.AppendText(Message)
                    If CreateLog Then My.Computer.FileSystem.WriteAllText(LogFileName, Message, False)
                    Dim SelectedPacks = ConvertPacksCheckedListBox.CheckedItems
                    For Each PackFile In SelectedPacks
                        If SourceFolderName.EndsWith("\") Then SourceFolderName = SourceFolderName.TrimEnd("\")

                        If DestinationFolderName.EndsWith("\") Then DestinationFolderName = DestinationFolderName.TrimEnd("\")

                        Message = Indent & "Starting " & PackFile & " at " & DateTime.Now & vbCrLf
                        OutputForm.OutputTextBox.AppendText(Message)
                        If CreateLog Then My.Computer.FileSystem.WriteAllText(LogFileName, Message, True)
                        ConvertPack(SourceFolderName, DestinationFolderName, PackFile, CreateLog, LogFileName, SubIndent, CleanUp)
                        Message = Indent & "Finished " & PackFile & " at " & DateTime.Now & vbCrLf & vbCrLf
                        OutputForm.OutputTextBox.AppendText(Message)
                        If CreateLog Then My.Computer.FileSystem.WriteAllText(LogFileName, Message, True)
                    Next
                    Message = "### Finished selected packs at " & DateTime.Now & "." & vbCrLf
                    OutputForm.OutputTextBox.AppendText(Message)
                    If CreateLog Then My.Computer.FileSystem.WriteAllText(LogFileName, Message, True)
                Else
                    MsgBox("Nothing was selected.")
                End If

            Else
                MsgBox("Destination folder name Is invalid.")
            End If
        Else
            MsgBox("Source folder name Is either invalid Or does Not exist.")
        End If
    End Sub

    '###### Copy Assets Group Box ######
    Private Sub CopyAssetsSourceTextBox_LostFocus(sender As Object, e As EventArgs) Handles CopyAssetsSourceTextBox.LostFocus
        CopyAssetsCheckedListBox.Items.Clear()
        Dim UserFolder As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        Dim SourceFolderName As String
        Dim IsFolderNameValid As Boolean
        Dim DoesFolderExist As Boolean
        Dim SourceFolder As System.IO.DirectoryInfo
        Dim DestinationFolderName As String

        SourceFolderName = CopyAssetsSourceTextBox.Text
        IsFolderNameValid = IsValidPathName(SourceFolderName)
        DoesFolderExist = System.IO.Directory.Exists(SourceFolderName)
        If IsFolderNameValid And DoesFolderExist Then
            SourceFolder = New System.IO.DirectoryInfo(SourceFolderName)
            DestinationFolderName = UserFolder & "\Dungeondraft\Copied Assets\" & SourceFolder.Name
            If CopyAssetsDestinationTextBox.Text = "" Then CopyAssetsDestinationTextBox.Text = DestinationFolderName
            For Each AssetFolder As String In My.Computer.FileSystem.GetDirectories(SourceFolderName)
                Dim FolderName As New System.IO.DirectoryInfo(AssetFolder)
                CopyAssetsCheckedListBox.Items.Add(FolderName.Name)
                If CopyAssetsSelectAllCheckBox.Checked Then CopyAssetsCheckedListBox.SetItemChecked(CopyAssetsCheckedListBox.Items.Count - 1, True)
            Next
        End If
    End Sub

    Private Sub CopyAssetsSourceBrowseButton_Click(sender As Object, e As EventArgs) Handles CopyAssetsSourceBrowseButton.Click
        CopyAssetsCheckedListBox.Items.Clear()
        CopyAssetsSourceBrowserDialog.ShowDialog()
        CopyAssetsSourceTextBox.Text = CopyAssetsSourceBrowserDialog.SelectedPath

        Dim UserFolder As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        Dim SourceFolderName As String
        Dim IsFolderNameValid As Boolean
        Dim DoesFolderExist As Boolean
        Dim SourceFolder As System.IO.DirectoryInfo
        Dim DestinationFolderName As String

        SourceFolderName = CopyAssetsSourceTextBox.Text
        IsFolderNameValid = IsValidPathName(SourceFolderName)
        DoesFolderExist = System.IO.Directory.Exists(SourceFolderName)
        If IsFolderNameValid And DoesFolderExist Then
            SourceFolder = New System.IO.DirectoryInfo(SourceFolderName)
            DestinationFolderName = UserFolder & "\Dungeondraft\Copied Assets\" & SourceFolder.Name
            If CopyAssetsDestinationTextBox.Text = "" Then CopyAssetsDestinationTextBox.Text = DestinationFolderName
            For Each AssetFolder As String In My.Computer.FileSystem.GetDirectories(SourceFolderName)
                Dim FolderName As New System.IO.DirectoryInfo(AssetFolder)
                CopyAssetsCheckedListBox.Items.Add(FolderName.Name)
                If CopyAssetsSelectAllCheckBox.Checked Then CopyAssetsCheckedListBox.SetItemChecked(CopyAssetsCheckedListBox.Items.Count - 1, True)
            Next
        End If
    End Sub

    Private Sub CopyAssetsDestinationBrowseButton_Click(sender As Object, e As EventArgs) Handles CopyAssetsDestinationBrowseButton.Click
        CopyAssetsDestinationBrowserDialog.ShowDialog()
        CopyAssetsDestinationTextBox.Text = CopyAssetsDestinationBrowserDialog.SelectedPath
    End Sub

    Private Sub CopyAssetsSelectAllCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles CopyAssetsSelectAllCheckBox.CheckedChanged
        If CopyAssetsSelectAllCheckBox.Checked Then
            CopyAssetsSelectAllButton_Click(sender, e)
        Else
            CopyAssetsSelectNoneButton_Click(sender, e)
        End If
    End Sub

    Private Sub CopyAssetsSelectAllButton_Click(sender As Object, e As EventArgs) Handles CopyAssetsSelectAllButton.Click
        Dim Count As Integer
        For Count = 0 To CopyAssetsCheckedListBox.Items.Count - 1
            CopyAssetsCheckedListBox.SetItemChecked(Count, True)
        Next
    End Sub
    Private Sub CopyAssetsSelectNoneButton_Click(sender As Object, e As EventArgs) Handles CopyAssetsSelectNoneButton.Click
        Dim Count As Integer
        For Count = 0 To CopyAssetsCheckedListBox.Items.Count - 1
            CopyAssetsCheckedListBox.SetItemChecked(Count, False)
        Next
    End Sub
    Private Sub CopyAssetsStartButton_Click(sender As Object, e As EventArgs) Handles CopyAssetsStartButton.Click
        Dim PowerShellPath As String = Environment.SystemDirectory & "\WindowsPowerShell\v1.0\powershell.exe"
        Dim ScriptName As String = "'.\DDCopyAssets.ps1'"
        Dim SourceFolderName As String = CopyAssetsSourceTextBox.Text
        Dim DestinationFolderName As String = CopyAssetsDestinationTextBox.Text
        Dim CreateTagFile As Boolean = CopyAssetsCreateTagsCheckBox.Checked
        Dim Portals As Boolean = CopyAssetsPortalsCheckBox.Checked
        Dim IsSourceFolderNameValid As String
        Dim DoesSourceFolderExist As Boolean
        Dim IsDestinationFolderNameValid As String
        Dim CreateLog = CopyAssetsLogCheckBox.Checked
        Dim LogFileName As String = GlobalVariables.LogsFolder & "\CopyAssets.log"
        Dim CopySource As String
        Dim CopyDestination As String
        Dim Indent As String = "    " '4 spaces
        Dim SubIndent As String = "        " '8 spaces
        Dim Message As String
        Dim DefaultTag As String = ""

        If CopyAssetsCreateTagsCheckBox.Checked Then
            Dim ConfigFileName = GlobalVariables.ConfigFolderName & "\" & GlobalVariables.ConfigFileName
            Dim ConfigObject = GetSavedConfig(ConfigFileName)
            DefaultTag = ConfigObject("tag_assets")("default_tag")
        End If

        IsSourceFolderNameValid = IsValidPathName(SourceFolderName)
        DoesSourceFolderExist = System.IO.Directory.Exists(SourceFolderName)

        IsDestinationFolderNameValid = IsValidPathName(DestinationFolderName)

        If SourceFolderName <> "" And IsSourceFolderNameValid And DoesSourceFolderExist Then
            If DestinationFolderName <> "" And IsDestinationFolderNameValid Then
                If CopyAssetsCheckedListBox.CheckedItems.Count >= 1 Then
                    OutputForm.OutputTextBox.Text = ""
                    OutputForm.Show()
                    OutputForm.BringToFront()
                    Message = "### Starting selected folders at " & DateTime.Now & "." & vbCrLf & vbCrLf
                    OutputForm.OutputTextBox.AppendText(Message)
                    If CreateLog Then My.Computer.FileSystem.WriteAllText(LogFileName, Message, False)
                    Dim SelectedFolders = CopyAssetsCheckedListBox.CheckedItems
                    For Each AssetFolder In SelectedFolders
                        If SourceFolderName.EndsWith("\") Then
                            CopySource = SourceFolderName & AssetFolder
                        Else
                            CopySource = SourceFolderName & "\" & AssetFolder
                        End If

                        If DestinationFolderName.EndsWith("\") Then
                            CopyDestination = DestinationFolderName & AssetFolder
                        Else
                            CopyDestination = DestinationFolderName & "\" & AssetFolder
                        End If

                        Message = Indent & "Starting " & AssetFolder & " at " & DateTime.Now & vbCrLf
                        OutputForm.OutputTextBox.AppendText(Message)
                        If CreateLog Then My.Computer.FileSystem.WriteAllText(LogFileName, Message, True)
                        CopyAssetsSub(CopySource, CopyDestination, CreateTagFile, DefaultTag, Portals, CreateLog, LogFileName, SubIndent)
                        Message = Indent & "Finished " & AssetFolder & " at " & DateTime.Now & vbCrLf & vbCrLf
                        OutputForm.OutputTextBox.AppendText(Message)
                        If CreateLog Then My.Computer.FileSystem.WriteAllText(LogFileName, Message, True)
                    Next
                    Message = "### Finished selected folders at " & DateTime.Now & "." & vbCrLf
                    OutputForm.OutputTextBox.AppendText(Message)
                    If CreateLog Then My.Computer.FileSystem.WriteAllText(LogFileName, Message, True)
                Else
                    MsgBox("Nothing was selected.")
                End If
            Else
                MsgBox("Destination folder name is invalid.")
            End If
        Else
            MsgBox("Source folder name is either invalid or does not exist.")
        End If
    End Sub

    '###### Copy Tiles Group Box ######
    Private Sub CopyTilesSourceTextBox_LostFocus(sender As Object, e As EventArgs) Handles CopyTilesSourceTextBox.LostFocus
        CopyTilesDataGridView.Rows.Clear()
        CopyTilesDataGridView.Columns.Clear()
        Dim UserFolder As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        Dim SourceFolderName As String
        Dim IsFolderNameValid As Boolean
        Dim DoesFolderExist As Boolean
        Dim SourceFolder As System.IO.DirectoryInfo
        Dim DestinationFolderName As String
        Dim CreateLog As Boolean = TagAssetsLogCheckBox.Checked
        Dim LogFileName As String = GlobalVariables.LogsFolder & "\CopyTiles.log"
        Dim SubIndent As String = "        "
        Dim SelectAll As Boolean = CopyTilesSelectAllCheckBox.Checked

        SourceFolderName = CopyTilesSourceTextBox.Text
        IsFolderNameValid = IsValidPathName(SourceFolderName)
        DoesFolderExist = System.IO.Directory.Exists(SourceFolderName)
        If IsFolderNameValid And DoesFolderExist Then
            SourceFolder = New System.IO.DirectoryInfo(SourceFolderName)
            DestinationFolderName = UserFolder & "\Dungeondraft\Copied Assets\" & SourceFolder.Name
            If CopyTilesDestinationTextBox.Text = "" Then CopyTilesDestinationTextBox.Text = DestinationFolderName
            LoadTilesSub(SourceFolderName, CreateLog, LogFileName, SelectAll, SubIndent)
        End If
    End Sub

    Private Sub CopyTilesSourceBrowseButton_Click(sender As Object, e As EventArgs) Handles CopyTilesSourceBrowseButton.Click
        CopyTilesDataGridView.Rows.Clear()
        CopyTilesDataGridView.Columns.Clear()
        CopyTilesSourceBrowserDialog.ShowDialog()
        CopyTilesSourceTextBox.Text = CopyTilesSourceBrowserDialog.SelectedPath

        Dim UserFolder As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        Dim SourceFolderName As String
        Dim IsFolderNameValid As Boolean
        Dim DoesFolderExist As Boolean
        Dim SourceFolder As System.IO.DirectoryInfo
        Dim DestinationFolderName As String
        Dim CreateLog As Boolean = TagAssetsLogCheckBox.Checked
        Dim LogFileName As String = GlobalVariables.LogsFolder & "\CopyTiles.log"
        Dim SubIndent As String = "        "
        Dim SelectAll As Boolean = CopyTilesSelectAllCheckBox.Checked

        SourceFolderName = CopyTilesSourceTextBox.Text
        IsFolderNameValid = IsValidPathName(SourceFolderName)
        DoesFolderExist = System.IO.Directory.Exists(SourceFolderName)
        If IsFolderNameValid And DoesFolderExist Then
            SourceFolder = New System.IO.DirectoryInfo(SourceFolderName)
            DestinationFolderName = UserFolder & "\Dungeondraft\Copied Assets\" & SourceFolder.Name
            If CopyTilesDestinationTextBox.Text = "" Then CopyTilesDestinationTextBox.Text = DestinationFolderName
            LoadTilesSub(SourceFolderName, CreateLog, LogFileName, SelectAll, SubIndent)
        End If
    End Sub

    Private Sub CopyTilesDestinationBrowseButton_Click(sender As Object, e As EventArgs) Handles CopyTilesDestinationBrowseButton.Click
        CopyTilesSourceBrowserDialog.ShowDialog()
        CopyTilesDestinationTextBox.Text = CopyTilesSourceBrowserDialog.SelectedPath

        Dim UserFolder As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        Dim IsFolderNameValid As Boolean
        Dim DoesFolderExist As Boolean
        Dim DestinationFolderName As String
        Dim DestinationFolder As System.IO.DirectoryInfo
        Dim LogFileName As String = GlobalVariables.LogsFolder & "\CopyTiles.log"
        Dim SubIndent As String = "        "
        Dim SelectAll As Boolean = CopyTilesSelectAllCheckBox.Checked

        DestinationFolderName = CopyTilesDestinationTextBox.Text
        IsFolderNameValid = IsValidPathName(DestinationFolderName)
        DoesFolderExist = System.IO.Directory.Exists(DestinationFolderName)
        If IsFolderNameValid And DoesFolderExist Then
            DestinationFolder = New System.IO.DirectoryInfo(DestinationFolderName)
            DestinationFolderName = UserFolder & "\Dungeondraft\Copied Assets\" & DestinationFolder.Name
            If CopyTilesDestinationTextBox.Text = "" Then CopyTilesDestinationTextBox.Text = DestinationFolderName
        End If
    End Sub

    Private Sub CopyTileSelectAllCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles CopyTilesSelectAllCheckBox.CheckedChanged
        For RowIndex As Integer = 0 To CopyTilesDataGridView.Rows.Count - 1
            CopyTilesDataGridView.Rows(RowIndex).Cells(0).Value = CopyTilesSelectAllCheckBox.Checked
            CopyTilesDataGridView.Rows(RowIndex).Cells(3).Value = CopyTilesSelectAllCheckBox.Checked
            CopyTilesDataGridView.Rows(RowIndex).Cells(4).Value = CopyTilesSelectAllCheckBox.Checked
            CopyTilesDataGridView.Rows(RowIndex).Cells(5).Value = CopyTilesSelectAllCheckBox.Checked
        Next
    End Sub

    Private Sub CopyTilesStartButton_Click(sender As Object, e As EventArgs) Handles CopyTilesStartButton.Click
        Dim SourceFolderName As String = CopyTilesSourceTextBox.Text
        Dim DestinationFolderName As String = CopyTilesDestinationTextBox.Text
        Dim CopySource As String
        Dim CopyDestination As String
        Dim FileName As String
        Dim BaseName As String
        Dim Extension As String
        Dim TileName As String
        Dim Message As String
        Dim CreateLog As Boolean = CopyTilesLogCheckBox.Checked
        Dim LogFileName As String = GlobalVariables.LogsFolder & "\CopyTiles.log"
        Dim Indent As String = "    "

        If Not SourceFolderName.EndsWith("\") Then SourceFolderName &= "\"
        If Not DestinationFolderName.EndsWith("\") Then DestinationFolderName &= "\"

        Dim PatternsPath As String = DestinationFolderName & "textures\patterns\normal\"
        Dim TerrainPath As String = DestinationFolderName & "textures\terrain\"
        Dim SimpleTilePath As String = DestinationFolderName & "textures\tilesets\simple\"
        Dim SimpleTileDataPath As String = DestinationFolderName & "data\tilesets\"
        Dim SimpleTileBaseName As String
        Dim SimpleTileFileName As String
        Dim SimpleTileSetDataFile As String


        Dim Row As DataGridViewRow

        OutputForm.OutputTextBox.Text = ""
        OutputForm.Show()
        OutputForm.BringToFront()
        Message = "### Starting selected tiles at " & DateTime.Now & "." & vbCrLf
        OutputForm.OutputTextBox.AppendText(Message)
        If CreateLog Then My.Computer.FileSystem.WriteAllText(LogFileName, Message, False)

        For RowIndex = 1 To CopyTilesDataGridView.Rows.Count - 1
            Row = CopyTilesDataGridView.Rows(RowIndex)
            TileName = Row.Cells(2).Value
            If TileName <> "" Then
                CopySource = SourceFolderName & TileName

                For ColumnIndex = 3 To 5
                    Row.Cells(ColumnIndex).Value = Convert.ToBoolean(Row.Cells(ColumnIndex).EditedFormattedValue)
                Next

                FileName = Path.GetFileName(CopySource)
                BaseName = Path.GetFileNameWithoutExtension(CopySource)
                Extension = Path.GetExtension(CopySource)

                If Row.Cells("Patterns").Value Then
                    CopyDestination = PatternsPath & FileName
                    If My.Computer.FileSystem.FileExists(CopyDestination) Then
                        Message = Indent & "Destination file already exists: " & CopyDestination & vbCrLf
                        OutputForm.OutputTextBox.AppendText(Message)
                        If CreateLog Then My.Computer.FileSystem.WriteAllText(LogFileName, Message, True)
                    Else
                        If Not My.Computer.FileSystem.DirectoryExists(PatternsPath) Then My.Computer.FileSystem.CreateDirectory(PatternsPath)
                        Message = Indent & "Copying from: " & CopySource & vbCrLf
                        Message &= Indent & "          to: " & CopyDestination & vbCrLf
                        OutputForm.OutputTextBox.AppendText(Message)
                        If CreateLog Then My.Computer.FileSystem.WriteAllText(LogFileName, Message, True)
                        My.Computer.FileSystem.CopyFile(CopySource, CopyDestination)
                    End If
                End If

                If Row.Cells("Terrain").Value Then
                    CopyDestination = TerrainPath & BaseName & "_terrain" & Extension
                    If My.Computer.FileSystem.FileExists(CopyDestination) Then
                        Message = Indent & "Destination file already exists: " & CopyDestination & vbCrLf
                        OutputForm.OutputTextBox.AppendText(Message)
                        If CreateLog Then My.Computer.FileSystem.WriteAllText(LogFileName, Message, True)
                    Else
                        If Not My.Computer.FileSystem.DirectoryExists(TerrainPath) Then My.Computer.FileSystem.CreateDirectory(TerrainPath)
                        Message = Indent & "Copying from: " & CopySource & vbCrLf
                        Message &= Indent & "          to: " & CopyDestination & vbCrLf
                        OutputForm.OutputTextBox.AppendText(Message)
                        If CreateLog Then My.Computer.FileSystem.WriteAllText(LogFileName, Message, True)
                        My.Computer.FileSystem.CopyFile(CopySource, CopyDestination)
                    End If
                End If

                If Row.Cells("SimpleTile").Value Then
                    SimpleTileBaseName = BaseName & "_simple"
                    SimpleTileFileName = BaseName & Extension
                    CopyDestination = SimpleTilePath & BaseName & "_simple" & Extension

                    If My.Computer.FileSystem.FileExists(CopyDestination) Then
                        Message = Indent & "Destination file already exists: " & CopyDestination & vbCrLf
                        OutputForm.OutputTextBox.AppendText(Message)
                        If CreateLog Then My.Computer.FileSystem.WriteAllText(LogFileName, Message, True)
                    Else
                        If Not My.Computer.FileSystem.DirectoryExists(SimpleTilePath) Then My.Computer.FileSystem.CreateDirectory(SimpleTilePath)
                        Message = Indent & "Copying from: " & CopySource & vbCrLf
                        Message &= Indent & "          to: " & CopyDestination & vbCrLf
                        OutputForm.OutputTextBox.AppendText(Message)
                        If CreateLog Then My.Computer.FileSystem.WriteAllText(LogFileName, Message, True)
                        My.Computer.FileSystem.CopyFile(CopySource, CopyDestination)

                        Dim SimpleTileSetData As New System.Collections.Specialized.OrderedDictionary
                        SimpleTileSetData.Add("path", "textures/tilesets/simple/" & BaseName & "_simple" & Extension)
                        SimpleTileSetData.Add("name", BaseName)
                        SimpleTileSetData.Add("type", "normal")
                        SimpleTileSetData.Add("color", "ffffff")

                        If Not My.Computer.FileSystem.DirectoryExists(SimpleTileDataPath) Then My.Computer.FileSystem.CreateDirectory(SimpleTileDataPath)
                        SimpleTileSetDataFile = SimpleTileDataPath & SimpleTileBaseName & ".dungeondraft_tileset"
                        Dim JSONString As String = JsonConvert.SerializeObject(SimpleTileSetData, Formatting.Indented)
                        My.Computer.FileSystem.WriteAllText(SimpleTileSetDataFile, JSONString, False, System.Text.Encoding.ASCII)
                    End If
                End If
            End If
        Next
        Message = "### Finished selected tiles at " & DateTime.Now & "." & vbCrLf & vbCrLf
        OutputForm.OutputTextBox.AppendText(Message)
        If CreateLog Then My.Computer.FileSystem.WriteAllText(LogFileName, Message, True)
    End Sub

    Public Sub CopyTilesDataGridView_CellClick(ByVal sender As Object, ByVal e As DataGridViewCellEventArgs)
        'Dim opposite As Boolean
        'Check to ensure that the row CheckBox is clicked.
        'If e.RowIndex >= 0 AndAlso e.ColumnIndex >= 2 Then
        If e.ColumnIndex <> 1 And e.ColumnIndex <> 2 Then

            'Reference the GridView Row.
            Dim row As DataGridViewRow = CopyTilesDataGridView.Rows(e.RowIndex)
            Dim toprow As DataGridViewRow = CopyTilesDataGridView.Rows(0)
            Dim checkrow As DataGridViewRow
            Dim checkcolumn As DataGridViewColumn

            'Set the CheckBox selection.
            'For column As Integer = 1 To 6
            row.Cells(e.ColumnIndex).Value = Convert.ToBoolean(row.Cells(e.ColumnIndex).EditedFormattedValue)
            If e.RowIndex = 0 And e.ColumnIndex = 0 Then
                For eachrow As Integer = 0 To CopyTilesDataGridView.Rows.Count - 1
                    checkrow = CopyTilesDataGridView.Rows(eachrow)
                    If checkrow.Cells(2).Value <> "" Then
                        checkrow.Cells(e.ColumnIndex).Value = row.Cells(e.ColumnIndex).Value
                        For eachcolumn As Integer = 3 To 5
                            checkcolumn = CopyTilesDataGridView.Columns(eachcolumn)
                            If row.Cells(2).Value <> "" Then checkrow.Cells(eachcolumn).Value = row.Cells(e.ColumnIndex).Value
                        Next

                    End If
                Next
            ElseIf e.RowIndex = 0 Then
                For eachrow As Integer = 1 To CopyTilesDataGridView.Rows.Count - 1
                    checkrow = CopyTilesDataGridView.Rows(eachrow)
                    If checkrow.Cells(2).Value <> "" Then checkrow.Cells(e.ColumnIndex).Value = row.Cells(e.ColumnIndex).Value
                Next
            ElseIf e.ColumnIndex = 0 Then
                For eachcolumn As Integer = 3 To 5
                    checkcolumn = CopyTilesDataGridView.Columns(eachcolumn)
                    If row.Cells(2).Value <> "" Then row.Cells(eachcolumn).Value = row.Cells(e.ColumnIndex).Value
                Next
            Else
                If Not row.Cells(e.ColumnIndex).Value Then
                    toprow.Cells(e.ColumnIndex).Value = False
                    row.Cells(0).Value = False
                End If
            End If

        End If
    End Sub

    '###### Data Files Group Box ######
    Private Sub DataFilesSourceTextBox_LostFocus(sender As Object, e As EventArgs) Handles DataFilesSourceTextBox.LostFocus
        DataFilesDataGridView.Rows.Clear()
        DataFilesDataGridView.Columns.Clear()

        Dim SourceFolderName As String
        Dim IsFolderNameValid As Boolean
        Dim DoesFolderExist As Boolean
        Dim SourceFolder As System.IO.DirectoryInfo

        SourceFolderName = DataFilesSourceTextBox.Text
        IsFolderNameValid = IsValidPathName(SourceFolderName)
        DoesFolderExist = System.IO.Directory.Exists(SourceFolderName)
        If IsFolderNameValid And DoesFolderExist Then
            SourceFolder = New System.IO.DirectoryInfo(SourceFolderName)
            SourceFolderName.TrimEnd("\")
            GetTilesAndWalls(SourceFolderName)
        Else
            MsgBox("Source folder name is either invalid or does not exist.")
        End If
    End Sub

    Private Sub DataFilesSourceBrowseButton_Click(sender As Object, e As EventArgs) Handles DataFilesSourceBrowseButton.Click
        DataFilesDataGridView.Rows.Clear()
        DataFilesDataGridView.Columns.Clear()
        DataFilesSourceBrowserDialog.ShowDialog()
        DataFilesSourceTextBox.Text = DataFilesSourceBrowserDialog.SelectedPath

        Dim SourceFolderName As String
        Dim IsFolderNameValid As Boolean
        Dim DoesFolderExist As Boolean
        Dim SourceFolder As System.IO.DirectoryInfo

        SourceFolderName = DataFilesSourceTextBox.Text
        IsFolderNameValid = IsValidPathName(SourceFolderName)
        DoesFolderExist = System.IO.Directory.Exists(SourceFolderName)
        If IsFolderNameValid And DoesFolderExist Then
            SourceFolder = New System.IO.DirectoryInfo(SourceFolderName)
            SourceFolderName.TrimEnd("\")
            GetTilesAndWalls(SourceFolderName)
        Else
            MsgBox("Source folder name is either invalid or does not exist.")
        End If
    End Sub

    Public Sub DataFilesDataGridView_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles DataFilesDataGridView.CellValueChanged
        If e.ColumnIndex = 2 Then
            Dim row As DataGridViewRow
            row = DataFilesDataGridView.Rows(e.RowIndex)
            If row.Cells(2).Value = "" Then row.Cells(2).Value = "normal"
        End If
    End Sub

    Public Sub DataFilesDataGridView_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataFilesDataGridView.CellDoubleClick
        If e.ColumnIndex = 3 Then
            If DataFilesColorDialog.ShowDialog() = DialogResult.OK Then
                Dim NewColor = DataFilesColorDialog.Color
                Dim R = Hex(NewColor.R)
                Dim G = Hex(NewColor.G)
                Dim B = Hex(NewColor.B)
                If R.Length = 1 Then R = "0" & R
                If G.Length = 1 Then G = "0" & G
                If B.Length = 1 Then B = "0" & B
                Dim HexColor = R & G & B
                Dim row As DataGridViewRow
                row = DataFilesDataGridView.Rows(e.RowIndex)
                row.Cells(3).Value = HexColor.ToLower
            End If
        End If
    End Sub

    Private Sub DataFilesStartButton_Click(sender As Object, e As EventArgs) Handles DataFilesStartButton.Click
        Dim DataFolder As String
        Dim DataFile As String
        Dim Indent As String = "    "
        Dim OutputMessage As String
        Dim LogFileName As String = GlobalVariables.LogsFolder & "\DataFiles.log"
        Dim CreateLog As Boolean = DataFilesLogCheckBox.Checked

        OutputForm.OutputTextBox.Text = ""
        OutputForm.Show()
        OutputMessage = "### Starting at " & DateTime.Now & vbCrLf
        OutputForm.OutputTextBox.AppendText(OutputMessage)
        If CreateLog Then My.Computer.FileSystem.WriteAllText(LogFileName, OutputMessage, False)

        OutputMessage = Indent & "Deleting existing tileset and wall data files to eliminate strays." & vbCrLf
        OutputForm.OutputTextBox.AppendText(OutputMessage)
        If CreateLog Then My.Computer.FileSystem.WriteAllText(LogFileName, OutputMessage, True)

        Dim DataFolderList() As String = {DataFilesSourceTextBox.Text & "\data\tilesets", DataFilesSourceTextBox.Text & "\data\walls"}
        For Each SubFolder In DataFolderList
            If Directory.Exists(SubFolder) Then
                For Each DataFile In Directory.GetFiles(SubFolder)
                    File.Delete(DataFile)
                Next
            End If
        Next

        For Each SubFolder In DataFolderList
            If Directory.Exists(SubFolder) Then
                OutputMessage = Indent & "Directory exists: " & SubFolder & vbCrLf
                OutputForm.OutputTextBox.AppendText(OutputMessage)
                If CreateLog Then My.Computer.FileSystem.WriteAllText(LogFileName, OutputMessage, True)
            Else
                OutputMessage = Indent & "Creating directory: " & SubFolder & vbCrLf
                OutputForm.OutputTextBox.AppendText(OutputMessage)
                If CreateLog Then My.Computer.FileSystem.WriteAllText(LogFileName, OutputMessage, True)
                Directory.CreateDirectory(SubFolder)
            End If
        Next

        For Each row As DataGridViewRow In DataFilesDataGridView.Rows
            Dim AssetInfo As New System.Collections.Specialized.OrderedDictionary
            If row.Cells(0).Value = "Wall" Then
                AssetInfo.Add("path", row.Cells(6).Value)
                AssetInfo.Add("color", row.Cells(3).Value)
            Else
                AssetInfo.Add("path", row.Cells(6).Value)
                AssetInfo.Add("name", row.Cells(1).Value)
                AssetInfo.Add("type", row.Cells(2).Value)
                AssetInfo.Add("color", row.Cells(3).Value)
            End If
            DataFolder = row.Cells(5).Value

            DataFile = DataFolder & "\" & row.Cells(4).Value

            OutputMessage = Indent & "Writing " & DataFile & vbCrLf
            OutputForm.OutputTextBox.AppendText(OutputMessage)
            If CreateLog Then My.Computer.FileSystem.WriteAllText(LogFileName, OutputMessage, True)
            Dim DataContent As String = JsonConvert.SerializeObject(AssetInfo, Formatting.Indented)
            My.Computer.FileSystem.WriteAllText(DataFile, DataContent, False, System.Text.Encoding.ASCII)
        Next
        OutputMessage = "### Finished at " & DateTime.Now
        OutputForm.OutputTextBox.AppendText(OutputMessage)
        If CreateLog Then My.Computer.FileSystem.WriteAllText(LogFileName, OutputMessage, True)
    End Sub

    '###### Map Details Group Box ######
    Private Sub MapDetailsSourceTextBox_LostFocus(sender As Object, e As EventArgs) Handles MapDetailsSourceTextBox.LostFocus
        MapDetailsCheckedListBox.Items.Clear()
        Dim SourceFolderName As String
        Dim IsFolderNameValid As Boolean
        Dim DoesFolderExist As Boolean
        SourceFolderName = MapDetailsSourceTextBox.Text
        IsFolderNameValid = IsValidPathName(SourceFolderName)
        DoesFolderExist = System.IO.Directory.Exists(SourceFolderName)
        If IsFolderNameValid And DoesFolderExist Then
            Dim SourceFiles = Directory.GetFiles(SourceFolderName, "*.dungeondraft_map")
            'Where Path.GetExtension(File).ToLower() = ".dungeondraft_map"

            For Each File In SourceFiles
                Dim FileName As New System.IO.FileInfo(File)
                MapDetailsCheckedListBox.Items.Add(FileName.Name)
                If MapDetailsSelectAllCheckBox.Checked Then MapDetailsCheckedListBox.SetItemChecked(MapDetailsCheckedListBox.Items.Count - 1, True)
            Next
        End If
    End Sub

    Private Sub MapDetailsBrowseButton_Click(sender As Object, e As EventArgs) Handles MapDetailsBrowseButton.Click
        MapDetailsCheckedListBox.Items.Clear()
        MapDetailsSourceBrowserDialog.ShowDialog()
        MapDetailsSourceTextBox.Text = MapDetailsSourceBrowserDialog.SelectedPath

        Dim SourceFolderName As String
        Dim IsFolderNameValid As Boolean
        Dim DoesFolderExist As Boolean
        SourceFolderName = MapDetailsSourceTextBox.Text
        IsFolderNameValid = IsValidPathName(SourceFolderName)
        DoesFolderExist = System.IO.Directory.Exists(SourceFolderName)
        If IsFolderNameValid And DoesFolderExist Then
            Dim SourceFiles = Directory.GetFiles(SourceFolderName, "*.dungeondraft_map")
            For Each File In SourceFiles
                Dim FileName As New System.IO.FileInfo(File)
                MapDetailsCheckedListBox.Items.Add(FileName.Name)
                If MapDetailsSelectAllCheckBox.Checked Then MapDetailsCheckedListBox.SetItemChecked(MapDetailsCheckedListBox.Items.Count - 1, True)
            Next
        End If
    End Sub

    Private Sub MapDetailsSelectAllCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles MapDetailsSelectAllCheckBox.CheckedChanged
        If MapDetailsSelectAllCheckBox.Checked Then
            MapDetailsSelectAllButton_Click(sender, e)
        Else
            MapDetailsSelectNoneButton_Click(sender, e)
        End If
    End Sub

    Private Sub MapDetailsSelectAllButton_Click(sender As Object, e As EventArgs) Handles MapDetailsSelectAllButton.Click
        Dim Count As Integer
        For Count = 0 To MapDetailsCheckedListBox.Items.Count - 1
            MapDetailsCheckedListBox.SetItemChecked(Count, True)
        Next
    End Sub

    Private Sub MapDetailsSelectNoneButton_Click(sender As Object, e As EventArgs) Handles MapDetailsSelectNoneButton.Click
        Dim Count As Integer
        For Count = 0 To MapDetailsCheckedListBox.Items.Count - 1
            MapDetailsCheckedListBox.SetItemChecked(Count, False)
        Next
    End Sub

    Private Sub MapDetailsStartButton_Click(sender As Object, e As EventArgs) Handles MapDetailsStartButton.Click
        Dim SourceFolderName As String = MapDetailsSourceTextBox.Text
        Dim IsSourceFolderNameValid As String
        Dim DoesSourceFolderExist As Boolean
        Dim CreateLog As Boolean = MapDetailsLogCheckBox.Checked
        Dim LogFileName As String = GlobalVariables.LogsFolder & "\MapDetails.log"
        Dim MapSource As String
        Dim Indent As String = "    " '4 spaces
        Dim SubIndent As String = "        " '8 spaces
        Dim Message As String

        IsSourceFolderNameValid = IsValidPathName(SourceFolderName)
        DoesSourceFolderExist = System.IO.Directory.Exists(SourceFolderName)
        If SourceFolderName <> "" And IsSourceFolderNameValid And DoesSourceFolderExist Then
            If MapDetailsCheckedListBox.CheckedItems.Count >= 1 Then
                OutputForm.OutputTextBox.Text = ""
                OutputForm.Show()
                OutputForm.BringToFront()
                Message = "### Starting selected map files at " & DateTime.Now & "." & vbCrLf & vbCrLf
                OutputForm.OutputTextBox.AppendText(Message)
                If CreateLog Then My.Computer.FileSystem.WriteAllText(LogFileName, Message, False)
                Dim SelectedFiles = MapDetailsCheckedListBox.CheckedItems
                For Each MapFile In SelectedFiles
                    If SourceFolderName.EndsWith("\") Then
                        MapSource = SourceFolderName & MapFile
                    Else
                        MapSource = SourceFolderName & "\" & MapFile
                    End If
                    Message = Indent & "Starting " & MapFile & " at " & DateTime.Now & vbCrLf
                    OutputForm.OutputTextBox.AppendText(Message)
                    If CreateLog Then My.Computer.FileSystem.WriteAllText(LogFileName, Message, True)
                    GetMapDetails(MapSource, MapFile, CreateLog, LogFileName, SubIndent)
                    Message = Indent & "Finished " & MapFile & " at " & DateTime.Now & vbCrLf & vbCrLf
                    OutputForm.OutputTextBox.AppendText(Message)
                    If CreateLog Then My.Computer.FileSystem.WriteAllText(LogFileName, Message, True)
                Next
                Message = "### Finished selected folders at " & DateTime.Now & "." & vbCrLf
                OutputForm.OutputTextBox.AppendText(Message)
                If CreateLog Then My.Computer.FileSystem.WriteAllText(LogFileName, Message, True)
            Else
                MsgBox("Nothing was selected.")
            End If
        Else
            MsgBox("Source folder name is either invalid or does not exist.")
        End If
    End Sub

    '###### Pack Assets Group Box ######
    Private Sub PackAssetsSourceTextBox_LostFocus(sender As Object, e As EventArgs) Handles PackAssetsSourceTextBox.LostFocus
        PackAssetsDataGridView.Rows.Clear()
        PackAssetsDataGridView.Columns.Clear()
        Dim UserFolder As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        Dim SourceFolderName As String
        Dim IsFolderNameValid As Boolean
        Dim DoesFolderExist As Boolean
        Dim SourceFolder As System.IO.DirectoryInfo
        Dim DestinationFolderName As String
        Dim CreateLog As Boolean = PackAssetsLogCheckBox.Checked
        Dim LogFileName As String = GlobalVariables.LogsFolder & "\PackAssets.log"
        Dim SubIndent As String = "        "
        Dim SelectAll As Boolean = PackAssetsSelectAllCheckBox.Checked

        If PackAssetsSourceTextBox.Text.EndsWith("\") Then PackAssetsSourceTextBox.Text = PackAssetsSourceTextBox.Text.TrimEnd("\")
        SourceFolderName = PackAssetsSourceTextBox.Text
        IsFolderNameValid = IsValidPathName(SourceFolderName)
        DoesFolderExist = System.IO.Directory.Exists(SourceFolderName)
        If IsFolderNameValid And DoesFolderExist Then
            SourceFolder = New System.IO.DirectoryInfo(SourceFolderName)
            DestinationFolderName = UserFolder & "\Dungeondraft"
            If PackAssetsDestinationTextBox.Text = "" Then PackAssetsDestinationTextBox.Text = DestinationFolderName
            LoadAssetFolders(SourceFolderName, CreateLog, LogFileName, SelectAll, SubIndent)
        End If
    End Sub

    Private Sub PackAssetsSourceBrowseButton_Click(sender As Object, e As EventArgs) Handles PackAssetsSourceBrowseButton.Click
        PackAssetsDataGridView.Rows.Clear()
        PackAssetsDataGridView.Columns.Clear()
        PackAssetsSourceBrowserDialog.ShowDialog()
        PackAssetsSourceTextBox.Text = PackAssetsSourceBrowserDialog.SelectedPath
        Dim UserFolder As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        Dim SourceFolderName As String
        Dim IsFolderNameValid As Boolean
        Dim DoesFolderExist As Boolean
        Dim SourceFolder As System.IO.DirectoryInfo
        Dim DestinationFolderName As String
        Dim CreateLog As Boolean = PackAssetsLogCheckBox.Checked
        Dim LogFileName As String = GlobalVariables.LogsFolder & "\PackAssets.log"
        Dim SubIndent As String = "        "
        Dim SelectAll As Boolean = PackAssetsSelectAllCheckBox.Checked

        If PackAssetsSourceTextBox.Text.EndsWith("\") Then PackAssetsSourceTextBox.Text = PackAssetsSourceTextBox.Text.TrimEnd("\")
        SourceFolderName = PackAssetsSourceTextBox.Text
        IsFolderNameValid = IsValidPathName(SourceFolderName)
        DoesFolderExist = System.IO.Directory.Exists(SourceFolderName)
        If IsFolderNameValid And DoesFolderExist Then
            SourceFolder = New System.IO.DirectoryInfo(SourceFolderName)
            DestinationFolderName = UserFolder & "\Dungeondraft\Repacked\" & SourceFolder.Name
            If PackAssetsDestinationTextBox.Text = "" Then PackAssetsDestinationTextBox.Text = DestinationFolderName
            LoadAssetFolders(SourceFolderName, CreateLog, LogFileName, SelectAll, SubIndent)
        End If
    End Sub

    Private Sub PackAssetsDestinationTextBox_LostFocus(sender As Object, e As EventArgs) Handles PackAssetsDestinationTextBox.LostFocus
        If PackAssetsDestinationTextBox.Text.EndsWith("\") Then PackAssetsDestinationTextBox.Text = PackAssetsDestinationTextBox.Text.TrimEnd("\")
    End Sub

    Private Sub PackAssetsDestinationBrowseButton_Click(sender As Object, e As EventArgs) Handles PackAssetsDestinationBrowseButton.Click
        PackAssetsDestinationBrowserDialog.ShowDialog()
        PackAssetsDestinationTextBox.Text = PackAssetsDestinationBrowserDialog.SelectedPath
        If PackAssetsDestinationTextBox.Text.EndsWith("\") Then PackAssetsDestinationTextBox.Text = PackAssetsDestinationTextBox.Text.TrimEnd("\")
    End Sub

    Private Sub PackAssetsSelectAllCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles PackAssetsSelectAllCheckBox.CheckedChanged
        If PackAssetsSelectAllCheckBox.Checked Then
            PackAssetsSelectAllButton_Click(sender, e)
        Else
            PackAssetsSelectNoneButton_Click(sender, e)
        End If
    End Sub

    Private Sub PackAssetsRefreshButton_Click(sender As Object, e As EventArgs) Handles PackAssetsRefreshButton.Click
        PackAssetsDataGridView.Rows.Clear()
        PackAssetsDataGridView.Columns.Clear()
        Dim SourceFolderName As String
        Dim IsFolderNameValid As Boolean
        Dim DoesFolderExist As Boolean
        Dim CreateLog As Boolean = PackAssetsLogCheckBox.Checked
        Dim LogFileName As String = GlobalVariables.LogsFolder & "\PackAssets.log"
        Dim SubIndent As String = "        "
        Dim SelectAll As Boolean = PackAssetsSelectAllCheckBox.Checked

        SourceFolderName = PackAssetsSourceTextBox.Text
        IsFolderNameValid = IsValidPathName(SourceFolderName)
        DoesFolderExist = System.IO.Directory.Exists(SourceFolderName)
        If IsFolderNameValid And DoesFolderExist Then
            LoadAssetFolders(SourceFolderName, CreateLog, LogFileName, SelectAll, SubIndent)
        End If
    End Sub
    Private Sub PackAssetsSelectAllButton_Click(sender As Object, e As EventArgs) Handles PackAssetsSelectAllButton.Click
        Dim RowIndex As Integer
        For RowIndex = 0 To PackAssetsDataGridView.Rows.Count - 1
            If PackAssetsDataGridView.Rows(RowIndex).Cells(1).Value <> "" Then
                PackAssetsDataGridView.Rows(RowIndex).Cells(0).Value = True
            End If
        Next
    End Sub

    Private Sub PackAssetsSelectNoneButton_Click(sender As Object, e As EventArgs) Handles PackAssetsSelectNoneButton.Click
        Dim RowIndex As Integer
        For RowIndex = 0 To PackAssetsDataGridView.Rows.Count - 1
            If PackAssetsDataGridView.Rows(RowIndex).Cells(1).Value <> "" Then
                PackAssetsDataGridView.Rows(RowIndex).Cells(0).Value = False
            End If
        Next
    End Sub

    Private Sub PackAssetsStartButton_Click(sender As Object, e As EventArgs) Handles PackAssetsStartButton.Click
        Dim PackEXE As String = "dungeondraft-pack.exe"
        Dim SourceFolderName As String = PackAssetsSourceTextBox.Text
        Dim DestinationFolderName As String = PackAssetsDestinationTextBox.Text
        Dim IsSourceFolderNameValid As String
        Dim DoesSourceFolderExist As Boolean
        Dim IsDestinationFolderNameValid As String
        Dim PackEXEexists As Boolean
        Dim Message As String
        Dim Indent As String = "    " '4 spaces
        Dim SubIndent As String = "        " '8 spaces
        Dim RowIndex As Integer
        Dim CreateLog As Boolean = PackAssetsLogCheckBox.Checked
        Dim LogFileName As String = GlobalVariables.LogsFolder & "\PackAssets.log"
        Dim Overwrite As Boolean = PackAssetsOverwriteCheckBox.Checked
        Dim SelectAll As Boolean = PackAssetsSelectAllCheckBox.Checked

        Dim PackSource As String
        Dim PackDestination As String

        Dim FolderName As String
        Dim PackName As String
        Dim Version As String
        Dim Author As String


        IsSourceFolderNameValid = IsValidPathName(SourceFolderName)
        DoesSourceFolderExist = System.IO.Directory.Exists(SourceFolderName)
        IsDestinationFolderNameValid = IsValidPathName(DestinationFolderName)

        PackEXEexists = My.Computer.FileSystem.FileExists(PackEXE)

        If SourceFolderName <> "" And IsSourceFolderNameValid And DoesSourceFolderExist Then
            If DestinationFolderName <> "" And IsDestinationFolderNameValid Then

                If PackAssetsDataGridView.Rows.Count >= 1 Then
                    If SourceFolderName.EndsWith("\") Then SourceFolderName.TrimEnd("\")
                    If DestinationFolderName.EndsWith("\") Then DestinationFolderName.TrimEnd("\")
                    'If Not DestinationFolderName.EndsWith("\") Then DestinationFolderName &= "\"

                    OutputForm.OutputTextBox.Text = ""
                    OutputForm.Show()
                    OutputForm.BringToFront()
                    Message = "### Starting selected asset folders at " & DateTime.Now & "." & vbCrLf
                    If PackAssetsOverwriteCheckBox.Checked Then
                        Message &= Indent & "(Overwrite is enabled.)" & vbCrLf & vbCrLf
                    Else
                        Message &= Indent & "(Overwrite is disabled.)" & vbCrLf & vbCrLf
                    End If
                    OutputForm.OutputTextBox.AppendText(Message)
                    If CreateLog Then My.Computer.FileSystem.WriteAllText(LogFileName, Message, False)

                    If Not My.Computer.FileSystem.DirectoryExists(DestinationFolderName) Then My.Computer.FileSystem.CreateDirectory(DestinationFolderName)
                    For RowIndex = 0 To PackAssetsDataGridView.Rows.Count - 1
                        If PackAssetsDataGridView.Rows(RowIndex).Cells("SelectFolder").Value And PackAssetsDataGridView.Rows(RowIndex).Cells("FolderName").Value <> "" Then

                            FolderName = PackAssetsDataGridView.Rows(RowIndex).Cells("FolderName").Value
                            PackName = PackAssetsDataGridView.Rows(RowIndex).Cells("PackName").Value
                            Version = PackAssetsDataGridView.Rows(RowIndex).Cells("PackVersion").Value
                            Author = PackAssetsDataGridView.Rows(RowIndex).Cells("PackAuthor").Value

                            PackSource = SourceFolderName & "\" & FolderName
                            PackDestination = DestinationFolderName & "\" & PackName & ".dungeondraft_pack"

                            Message = Indent & "Starting " & PackName & " at " & DateTime.Now & "." & vbCrLf
                            Message &= SubIndent & "Source: " & PackSource & vbCrLf
                            Message &= SubIndent & "Destination: " & PackDestination & vbCrLf
                            OutputForm.OutputTextBox.AppendText(Message)
                            If CreateLog Then My.Computer.FileSystem.WriteAllText(LogFileName, Message, True)

                            If My.Computer.FileSystem.FileExists(PackDestination) And Not Overwrite Then
                                Message = SubIndent & "Destination file already exists." & vbCrLf
                                Message &= SubIndent & "Skipping Folder." & vbCrLf
                                OutputForm.OutputTextBox.AppendText(Message)
                                If CreateLog Then My.Computer.FileSystem.WriteAllText(LogFileName, Message, True)
                            Else
                                PackAssets(PackSource, DestinationFolderName, RowIndex, CreateLog, LogFileName, SubIndent)
                            End If
                            Message = Indent & "Finishing " & PackName & " at " & DateTime.Now & "." & vbCrLf & vbCrLf
                            OutputForm.OutputTextBox.AppendText(Message)
                            If CreateLog Then My.Computer.FileSystem.WriteAllText(LogFileName, Message, True)
                        End If
                    Next

                    Message = "### Finished selected asset folders at " & DateTime.Now & "." & vbCrLf
                    OutputForm.OutputTextBox.AppendText(Message)
                    If CreateLog Then My.Computer.FileSystem.WriteAllText(LogFileName, Message, True)
                    PackAssetsDataGridView.Rows.Clear()
                    PackAssetsDataGridView.Columns.Clear()
                    SourceFolderName = PackAssetsSourceTextBox.Text
                    LoadAssetFolders(SourceFolderName, CreateLog, LogFileName, SelectAll, SubIndent)
                Else
                    MsgBox("Asset folder list is empty.")
                End If

            Else
                MsgBox("Destination folder name is invalid.")
            End If
        Else
            MsgBox("Source folder name is either invalid or does not exist.")
        End If
    End Sub

    '###### Unpack Assets Group Box ######
    Private Sub UnpackAssetsSourceTextBox_LostFocus(sender As Object, e As EventArgs) Handles UnpackAssetsSourceTextBox.LostFocus
        UnpackAssetsCheckedListBox.Items.Clear()

        Dim UserFolder As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        Dim SourceFolderName As String
        Dim IsFolderNameValid As Boolean
        Dim DoesFolderExist As Boolean
        Dim SourceFolder As System.IO.DirectoryInfo
        Dim DestinationFolderName As String

        SourceFolderName = UnpackAssetsSourceTextBox.Text
        IsFolderNameValid = IsValidPathName(SourceFolderName)
        DoesFolderExist = System.IO.Directory.Exists(SourceFolderName)
        If IsFolderNameValid And DoesFolderExist Then
            SourceFolder = New System.IO.DirectoryInfo(SourceFolderName)
            DestinationFolderName = UserFolder & "\Dungeondraft"
            If UnpackAssetsDestinationTextBox.Text = "" Then UnpackAssetsDestinationTextBox.Text = DestinationFolderName
            For Each PackFile As String In My.Computer.FileSystem.GetFiles(SourceFolderName)
                Dim PackName As New System.IO.DirectoryInfo(PackFile)
                If PackName.Extension = ".dungeondraft_pack" Then
                    UnpackAssetsCheckedListBox.Items.Add(PackName.Name)
                    If UnpackAssetsSelectAllCheckBox.Checked Then UnpackAssetsCheckedListBox.SetItemChecked(UnpackAssetsCheckedListBox.Items.Count - 1, True)
                End If
            Next
        End If
    End Sub

    Private Sub UnpackAssetsSourceBrowseButton_Click(sender As Object, e As EventArgs) Handles UnpackAssetsSourceBrowseButton.Click
        UnpackAssetsCheckedListBox.Items.Clear()
        UnpackAssetsSourceBrowserDialog.ShowDialog()
        UnpackAssetsSourceTextBox.Text = UnpackAssetsSourceBrowserDialog.SelectedPath

        Dim UserFolder As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        Dim SourceFolderName As String
        Dim IsFolderNameValid As Boolean
        Dim DoesFolderExist As Boolean
        Dim SourceFolder As System.IO.DirectoryInfo
        Dim DestinationFolderName As String

        SourceFolderName = UnpackAssetsSourceTextBox.Text
        IsFolderNameValid = IsValidPathName(SourceFolderName)
        DoesFolderExist = System.IO.Directory.Exists(SourceFolderName)
        If IsFolderNameValid And DoesFolderExist Then
            SourceFolder = New System.IO.DirectoryInfo(SourceFolderName)
            DestinationFolderName = UserFolder & "\Dungeondraft\Unpacked\" & SourceFolder.Name
            If UnpackAssetsDestinationTextBox.Text = "" Then UnpackAssetsDestinationTextBox.Text = DestinationFolderName
            For Each PackFile As String In My.Computer.FileSystem.GetFiles(SourceFolderName)
                Dim PackName As New System.IO.DirectoryInfo(PackFile)
                If PackName.Extension = ".dungeondraft_pack" Then
                    UnpackAssetsCheckedListBox.Items.Add(PackName.Name)
                    If UnpackAssetsSelectAllCheckBox.Checked Then UnpackAssetsCheckedListBox.SetItemChecked(UnpackAssetsCheckedListBox.Items.Count - 1, True)
                End If
            Next
        Else
            MsgBox("Source folder name is either invalid or does not exist.")
        End If
    End Sub

    Private Sub UnpackAssetsDestinationBrowseButton_Click(sender As Object, e As EventArgs) Handles UnpackAssetsDestinationBrowseButton.Click
        UnpackAssetsDestinationBrowserDialog.ShowDialog()
        UnpackAssetsDestinationTextBox.Text = UnpackAssetsDestinationBrowserDialog.SelectedPath
    End Sub

    Private Sub UnpackAssetsSelectAllCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles UnpackAssetsSelectAllCheckBox.CheckedChanged
        If UnpackAssetsSelectAllCheckBox.Checked Then
            UnpackAssetsSelectAllButton_Click(sender, e)
        Else
            UnpackAssetsSelectNoneButton_Click(sender, e)
        End If
    End Sub

    Private Sub UnpackAssetsSelectAllButton_Click(sender As Object, e As EventArgs) Handles UnpackAssetsSelectAllButton.Click
        Dim Count As Integer
        For Count = 0 To UnpackAssetsCheckedListBox.Items.Count - 1
            UnpackAssetsCheckedListBox.SetItemChecked(Count, True)
        Next
    End Sub

    Private Sub UnpackAssetsSelectNoneButton_Click(sender As Object, e As EventArgs) Handles UnpackAssetsSelectNoneButton.Click
        Dim Count As Integer
        For Count = 0 To UnpackAssetsCheckedListBox.Items.Count - 1
            UnpackAssetsCheckedListBox.SetItemChecked(Count, False)
        Next
    End Sub

    Private Sub UnpackAssetsStartButton_Click(sender As Object, e As EventArgs) Handles UnpackAssetsStartButton.Click
        Dim SourceFolderName As String = UnpackAssetsSourceTextBox.Text
        Dim DestinationFolderName As String = UnpackAssetsDestinationTextBox.Text
        Dim IsSourceFolderNameValid As String
        Dim DoesSourceFolderExist As Boolean
        Dim IsDestinationFolderNameValid As String
        Dim Message As String
        Dim Indent As String = "    " '4 spaces
        Dim SubIndent As String = "        " '8 spaces
        Dim PackBaseName As String
        Dim LogFileName As String = GlobalVariables.LogsFolder & "\UnpackAssets.log"
        Dim CreateLog As Boolean = UnpackAssetsLogCheckBox.Checked

        IsSourceFolderNameValid = IsValidPathName(SourceFolderName)
        DoesSourceFolderExist = System.IO.Directory.Exists(SourceFolderName)
        IsDestinationFolderNameValid = IsValidPathName(DestinationFolderName)

        If SourceFolderName <> "" And IsSourceFolderNameValid And DoesSourceFolderExist Then
            If DestinationFolderName <> "" And IsDestinationFolderNameValid Then
                If UnpackAssetsCheckedListBox.CheckedItems.Count >= 1 Then
                    OutputForm.OutputTextBox.Text = ""
                    OutputForm.Show()
                    OutputForm.BringToFront()
                    Message = "### Starting selected pack files at " & DateTime.Now & "." & vbCrLf & vbCrLf
                    OutputForm.OutputTextBox.AppendText(Message)
                    If CreateLog Then My.Computer.FileSystem.WriteAllText(LogFileName, Message, False)
                    For Each PackFile As String In UnpackAssetsCheckedListBox.CheckedItems
                        PackBaseName = Path.GetFileNameWithoutExtension(SourceFolderName & "\" & PackFile)
                        Dim PackSource As String = SourceFolderName & "\" & PackFile
                        Dim PackDestination As String = DestinationFolderName & "\" & PackBaseName

                        Message = Indent & "Starting " & PackFile & " at " & DateTime.Now & "." & vbCrLf
                        Message &= SubIndent & "Source: " & PackSource & vbCrLf
                        Message &= SubIndent & "Destination: " & PackDestination & vbCrLf
                        OutputForm.OutputTextBox.AppendText(Message)
                        If CreateLog Then My.Computer.FileSystem.WriteAllText(LogFileName, Message, True)
                        If Directory.Exists(PackDestination) Then
                            Message = SubIndent & "Destination folder exists." & vbCrLf
                            Message &= SubIndent & "Skipping file." & vbCrLf
                            OutputForm.OutputTextBox.AppendText(Message)
                            If CreateLog Then My.Computer.FileSystem.WriteAllText(LogFileName, Message, True)
                        Else
                            UnpackAssets(PackSource, DestinationFolderName, CreateLog, LogFileName, SubIndent)
                        End If
                        Message = Indent & "Finished " & PackFile & " at " & DateTime.Now & "." & vbCrLf & vbCrLf
                        OutputForm.OutputTextBox.AppendText(Message)
                        If CreateLog Then My.Computer.FileSystem.WriteAllText(LogFileName, Message, True)
                    Next
                    Message = "### Finished selected pack files at " & DateTime.Now & "." & vbCrLf
                    OutputForm.OutputTextBox.AppendText(Message)
                    If CreateLog Then My.Computer.FileSystem.WriteAllText(LogFileName, Message, True)
                Else
                    MsgBox("Nothing was selected.")
                End If

            Else
                MsgBox("Destination folder name is invalid.")
            End If
        Else
            MsgBox("Source folder name is either invalid or does not exist.")
        End If
    End Sub

    Public Class GlobalVariables
        Public Shared Property ConfigFileName As String = "DDTools.config"
        Public Shared Property ConfigFolderName As String = GetFolderPath(SpecialFolder.ApplicationData) & "\EightBitz\DDTools"
        Public Shared Property MyDocuments As String = GetFolderPath(SpecialFolder.MyDocuments)
        Public Shared Property LogsFolder As String = GetFolderPath(SpecialFolder.MyDocuments) & "\DDTools\Logs"
    End Class
End Class
