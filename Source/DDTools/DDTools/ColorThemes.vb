Imports System.IO
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Module ColorThemes
    Public Function GetMapColors(MapFile As String)
        Dim rawJson = File.ReadAllText(MapFile)
        Dim MapObject = JsonConvert.DeserializeObject(rawJson)
        Dim ColorPalettes As JObject = MapObject("header")("editor_state")("color_palettes")

        If DDToolsForm.CreateColorThemeIncludeAllCustomColorsCheckBox.Checked Then
            ColorPalettes = GetCustomColors(MapObject, ColorPalettes)
        End If

        If DDToolsForm.CreateColorThemeIncludeAllCustomColorsCheckBox.Checked Then
            ColorPalettes = GetNonPaletteColors(MapObject, ColorPalettes)
        End If

        Return ColorPalettes
    End Function

    Public Function GetCustomColors(MapObject, ColorPalettes)
        Dim World = MapObject("world")
        Dim levelnumber As String
        Dim match As Boolean

        match = False
        For Each Color In ColorPalettes("grid_colors")
            If Color = World("grid")("color") Then
                match = True
                Exit For
            End If
        Next
        If match = False And Not World("grid")("color") Is Nothing Then ColorPalettes("grid_colors").Add(World("grid")("color"))

        For Each Level In World("levels")
            levelnumber = Level.Name.ToString
            If Not World("levels")(levelnumber)("water") Is Nothing Then
                Dim WorldWater = World("levels")(levelnumber)("water")
                For Each Water In WorldWater
                    Try
                        match = False
                        For Each Color In ColorPalettes("deep_water_colors")
                            If Color = Water("deep_color") Then
                                match = True
                                Exit For
                            End If
                        Next
                        If match = False And Not Water("deep_color") Is Nothing Then ColorPalettes("deep_water_colors").Add(Water("deep_color"))
                    Catch ex As Exception

                    End Try

                    Try
                        match = False
                        For Each Color In ColorPalettes("shallow_water_colors")
                            If Color = Water("shallow_color") Then
                                match = True
                                Exit For
                            End If
                        Next
                        If match = False And Not Water("shallow_color") Is Nothing Then ColorPalettes("shallow_water_colors").Add(Water("shallow_color"))
                    Catch ex As Exception

                    End Try
                Next

                If Not WorldWater("tree") Is Nothing Then
                    For Each Water In WorldWater("tree")
                        Try
                            match = False
                            For Each Color In ColorPalettes("deep_water_colors")
                                If Color = Water("deep_color") Then
                                    match = True
                                    Exit For
                                End If
                            Next
                            If match = False And Not Water("deep_color") Is Nothing Then ColorPalettes("deep_water_colors").Add(Water("deep_color"))
                        Catch ex As Exception

                        End Try

                        Try
                            match = False
                            For Each Color In ColorPalettes("shallow_water_colors")
                                If Color = Water("shallow_color") Then
                                    match = True
                                    Exit For
                                End If
                            Next
                            If match = False And Not Water("shallow_color") Is Nothing Then ColorPalettes("shallow_water_colors").Add(Water("shallow_color"))
                        Catch ex As Exception

                        End Try
                    Next

                    If Not WorldWater("tree")("children") Is Nothing Then
                        For Each Water In WorldWater("tree")("children")
                            Try
                                match = False
                                For Each Color In ColorPalettes("deep_water_colors")
                                    If Color = Water("deep_color") Then
                                        match = True
                                        Exit For
                                    End If
                                Next
                                If match = False And Not Water("deep_color") Is Nothing Then ColorPalettes("deep_water_colors").Add(Water("deep_color"))
                            Catch ex As Exception

                            End Try

                            Try
                                match = False
                                For Each Color In ColorPalettes("shallow_water_colors")
                                    If Color = Water("shallow_color") Then
                                        match = True
                                        Exit For
                                    End If
                                Next
                                If match = False And Not Water("shallow_color") Is Nothing Then ColorPalettes("shallow_water_colors").Add(Water("shallow_color"))
                            Catch ex As Exception

                            End Try
                        Next
                    End If
                End If
            End If 'Water

            For Each Asset In World("levels")(levelnumber)("objects")
                match = False
                For Each Color In ColorPalettes("object_custom_colors")
                    If Color = Asset("custom_color") Then
                        match = True
                        Exit For
                    End If
                Next
                If match = False And Not Asset("custom_color") Is Nothing Then ColorPalettes("object_custom_colors").Add(Asset("custom_color"))
            Next

            For Each Light In World("levels")(levelnumber)("lights")
                match = False
                For Each Color In ColorPalettes("light_colors")
                    If Color = Light("color") Then
                        match = True
                        Exit For
                    End If
                Next
                If match = False And Not Light("color") Is Nothing Then ColorPalettes("light_colors").Add(Light("color"))
            Next

            match = False
            For Each Color In ColorPalettes("cave_ground_colors")
                If Color = World("levels")(levelnumber)("cave")("ground_color") Then
                    match = True
                    Exit For
                End If
            Next
            If match = False And Not World("levels")(levelnumber)("cave")("ground_color") Is Nothing Then ColorPalettes("cave_ground_colors").Add(World("levels")(levelnumber)("cave")("ground_color"))

            match = False
            For Each Color In ColorPalettes("cave_wall_colors")
                If Color = World("levels")(levelnumber)("cave")("wall_color") Then
                    match = True
                    Exit For
                End If
            Next
            If match = False And Not World("levels")(levelnumber)("cave")("wall_color") Is Nothing Then ColorPalettes("cave_wall_colors").Add(World("levels")(levelnumber)("cave")("wall_color"))
        Next

        Return ColorPalettes
    End Function

    Public Function GetNonPaletteColors(MapObject, ColorPalettes)
        Dim World = MapObject("world")
        Dim levelnumber As String
        Dim match As Boolean

        For Each Level In World("levels")
            levelnumber = Level.Name.ToString
            match = False
            For Each Color In ColorPalettes("object_custom_colors")
                If Color = World("levels")(levelnumber)("environment")("ambient_light") Then
                    match = True
                    Exit For
                End If
            Next
            If match = False And Not World("levels")(levelnumber)("environment")("ambient_light") Is Nothing Then ColorPalettes("object_custom_colors").Add(World("levels")(levelnumber)("environment")("ambient_light"))

            For Each TileColor In World("levels")(levelnumber)("tiles")("colors")
                match = False
                For Each ObjectColor In ColorPalettes("object_custom_colors")
                    If TileColor = ObjectColor Then
                        match = True
                        Exit For
                    End If
                Next
                If match = False And Not TileColor Is Nothing Then ColorPalettes("object_custom_colors").Add(TileColor)
            Next

            For Each Pattern In World("levels")(levelnumber)("patterns")
                match = False
                For Each Color In ColorPalettes("object_custom_colors")
                    If Color = Pattern("color") Then
                        match = True
                        Exit For
                    End If
                Next
                If match = False And Not Pattern("color") Is Nothing Then ColorPalettes("object_custom_colors").Add(Pattern("color"))
            Next

            For Each Wall In World("levels")(levelnumber)("walls")
                match = False
                For Each Color In ColorPalettes("object_custom_colors")
                    If Color = Wall("color") Then
                        match = True
                        Exit For
                    End If
                Next
                If match = False And Not Wall("color") Is Nothing Then ColorPalettes("object_custom_colors").Add(Wall("color"))
            Next
        Next
        Return ColorPalettes
    End Function

    Public Sub ApplyColorTheme(ColorTheme As JObject, MapFile As String)
        Dim rawJson = File.ReadAllText(MapFile)
        Dim MapObject = JsonConvert.DeserializeObject(rawJson)
        MapObject("header")("editor_state")("color_palettes") = ColorTheme
        Dim SerializedMap As String = JsonConvert.SerializeObject(MapObject, Formatting.Indented)
        My.Computer.FileSystem.WriteAllText(MapFile, SerializedMap, False, System.Text.Encoding.ASCII)
    End Sub

    Public Sub SaveColorTheme(SaveFile As String, ColorTheme As JObject)
        Dim SerializedTheme As String = JsonConvert.SerializeObject(ColorTheme, Formatting.Indented)
        My.Computer.FileSystem.WriteAllText(SaveFile, SerializedTheme, False, System.Text.Encoding.ASCII)
    End Sub

    Public Function GetColorTheme(ColorFile As String)
        Dim rawJson = File.ReadAllText(ColorFile)
        Dim ColorTheme = JsonConvert.DeserializeObject(rawJson)
        Return ColorTheme
    End Function
End Module
