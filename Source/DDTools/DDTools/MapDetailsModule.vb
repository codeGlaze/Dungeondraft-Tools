Imports System.IO
Imports System.Text
Imports System.Environment
Imports Newtonsoft.Json

Module MapDetailsModule
    Private Class FileEntry
        Public path As String
        Public offset As Long
        Public size As Long

        Public Sub New(ByVal path As String, ByVal offset As Long, ByVal size As Long)
            Dim pos As Integer
            path = path.Substring(6).TrimEnd(vbNullChar)
            path = path.Replace("packs/", "")
            If path.Contains("/") Then
                pos = path.IndexOf("/")
            Else
                pos = path.IndexOf(".")
            End If
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

    Private Class TextureClass
        Public Property PackName As String
        Public Property PackVersion As String
        Public Property PackAuthor As String
        Public Property AssetName As String
        Public Property AssetType As String
        Public Property Color As String
        Public Property LevelName As String
        Public Property Instances As Integer
    End Class

    Private Declare Auto Function GetPrivateProfileString Lib "kernel32" (ByVal lpAppName As String,
                ByVal lpKeyName As String,
                ByVal lpDefault As String,
                ByVal lpReturnedString As StringBuilder,
                ByVal nSize As Integer,
                ByVal lpFileName As String) As Integer

    Private Function GetAssetFolder()
        Dim value As String
        Dim result As Integer
        Dim sb As StringBuilder
        Dim ConfigINIPath As String = GetFolderPath(SpecialFolder.ApplicationData) & "\Dungeondraft\config.ini"

        sb = New StringBuilder(300)
        result = GetPrivateProfileString("Assets", "custom_assets_directory", "", sb, sb.Capacity, ConfigINIPath)
        value = sb.ToString()
        value = value.Replace("\\", "\")
        Return value
    End Function

    Private Function GetTileList(AssetFolder, AssetManifest)
        Dim fileIndex = New List(Of FileEntry)()
        Dim Message As String

        For Each AssetPack In AssetManifest
            Dim inputfile As String = AssetFolder & "\" & AssetPack("name") & ".dungeondraft_pack"

            Dim MAGIC As Integer = &H43504447
            Dim foldername As String = Path.GetFileNameWithoutExtension(inputfile)
            Dim failed = 0

            If File.Exists(inputfile) Then
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
                        Message = ($"Godot Engine version: {inputStream.ReadInt32()}.{inputStream.ReadInt32()}.{inputStream.ReadInt32()}.{inputStream.ReadInt32()}")
                        inputStream.BaseStream.Seek(16 * 4, SeekOrigin.Current)
                        Dim fileCount = inputStream.ReadInt32()

                        For i = 0 To fileCount - 1
                            Dim pathLength = inputStream.ReadInt32()
                            Dim path = Encoding.UTF8.GetString(inputStream.ReadBytes(pathLength))
                            Dim fileEntry = New FileEntry(path.ToString(), inputStream.ReadInt64(), inputStream.ReadInt64())
                            If fileEntry.path.Contains("/textures/tilesets/") Then
                                fileIndex.Add(fileEntry)
                            End If
                            inputStream.BaseStream.Seek(16, SeekOrigin.Current)
                        Next
                    End If
                End Using
            End If
        Next
        Return fileIndex
    End Function

    Private Function ParseTexture(Texture, AssetManifest, AssetType, LevelName)
        Dim PackID As String
        Dim PackName As String = ""
        Dim PackVersion As String = "<no version number>"
        Dim PackAuthor As String = "<no author name>"
        Dim AssetName As String

        'Find the pack ID in the texture string.
        Dim SplitTexture = Texture.Split("/")
        PackID = SplitTexture(3)
        If AssetType = "roofs" Then
            AssetName = SplitTexture(SplitTexture.Length - 2)
        Else
            AssetName = SplitTexture(SplitTexture.Length - 1)
        End If

        'Iterate through the asset manifest until we find a matching pack ID,
        'then get the pack name of the maatching ID.
        For Each AssetPack In AssetManifest
            If PackID = AssetPack("id") Then
                PackName = AssetPack("name")
                If AssetPack("version") <> "" Then PackVersion = AssetPack("version")
                If AssetPack("author") <> "" Then PackAuthor = AssetPack("author")
                Exit For
            End If
        Next

        'If we didn't find the pack ID in the asset manifest, then put that info in the PackName field.
        If PackName = "" Then PackName = "Pack ID " & PackID & " not found in Asset Manifest"

        'Create a UsedAsset record with all the info we found.
        Dim UsedAsset As New TextureClass With {
            .PackName = PackName,
            .PackVersion = PackVersion,
            .PackAuthor = PackAuthor,
            .AssetName = AssetName,
            .AssetType = AssetType,
            .LevelName = LevelName
        }

        'Return the UsedAsset record as the result of this function.
        Return UsedAsset
    End Function

    Public Sub GetMapDetails(MapSource As String, MapFile As String, CreateLog As Boolean, LogFileName As String, Indent As String)
        'Initialize some variables
        Dim BuiltInTiles As Integer = 13
        Dim Message As String
        Dim AssetFolder = GetAssetFolder()
        Dim rawJson = File.ReadAllText(MapSource)
        Dim MapInfo = JsonConvert.DeserializeObject(rawJson)
        Dim Header = MapInfo("header")
        Dim AssetManifest = Header("asset_manifest")
        Dim World = MapInfo("world")
        Dim TraceObject
        Dim TileList = New List(Of FileEntry)()

        If AssetFolder <> "" And Not AssetManifest Is Nothing Then
            TileList = GetTileList(AssetFolder, AssetManifest)
        End If

        'Initialize some more variables.
        Dim AssetTypeArray As String() = {"lights", "materials", "objects", "paths", "patterns", "portals", "roofs", "terrain", "tiles", "walls"}
        Dim TerrainArray As String() = {"texture_1", "texture_2", "texture_3", "texture_4"}
        Dim levelnumber As String
        Dim levelname As String
        Dim texture As String
        Dim tiles
        Dim layernumber As String
        Dim UsedAssetList As New List(Of TextureClass)
        Dim ColorAssetList As New List(Of TextureClass)
        Dim ColorTileList As New List(Of TextureClass)
        Dim ColorCaveList As New List(Of TextureClass)
        Dim ColorEnvironmentList As New List(Of TextureClass)
        Dim ColorDeepWaterList As New List(Of TextureClass)
        Dim ColorShallowWaterList As New List(Of TextureClass)

        Dim SearchIndex As Integer

        Dim gridcolor As String = World("grid")("color")

        'Iterate through each level of the map.
        For Each Level In World("levels")
            'Get the number and name assigned to the current level.
            levelnumber = Level.Name.ToString
            levelname = World("levels")(levelnumber)("label")

            Dim Environment As New TextureClass
            Environment.Color = World("levels")(levelnumber)("environment")("ambient_light")
            Environment.LevelName = levelname
            ColorEnvironmentList.Add(Environment)

            'Iterate through each asset type in the AssetTypeArray, and find all assets of that type.
            For Each AssetType In AssetTypeArray
                Select Case AssetType
                    Case "materials"
                        'Iterate through all assets where the asset type is "materials".
                        If Not World("levels")(levelnumber)(AssetType) Is Nothing Then
                            For Each layer In World("levels")(levelnumber)(AssetType)
                                'Materials are separated by layers (instead of having the layer stored as a property of the material),
                                'so we have to iterate through each layer under "materials" to find all materials used.
                                layernumber = layer.name.ToString
                                For Each asset In World("levels")(levelnumber)("materials")(layernumber)
                                    'Get the "texture" value for the current asset.
                                    texture = asset("texture")
                                    If texture.StartsWith("res://packs/") Then
                                        'If this is from a custom asset pack, then create a new record for it.
                                        Dim UsedAsset As New TextureClass

                                        'Call the "ParseTexture" function to get the info for the custom asset and
                                        'for the custom asset pack it's from.
                                        UsedAsset = ParseTexture(texture, AssetManifest, AssetType, levelname)

                                        'If this asset has already been added to our UsedAssetList, then increment the Instances property
                                        'of the existing record instead of adding the new one as a duplicate.
                                        'Otherwise, add the new record to the list And set the Instances property to 1.
                                        SearchIndex = UsedAssetList.FindIndex(Function(p) p.AssetName = UsedAsset.AssetName)
                                        If SearchIndex >= 0 Then
                                            UsedAssetList(SearchIndex).Instances += 1
                                        Else
                                            UsedAsset.Instances = 1
                                            UsedAssetList.Add(UsedAsset)
                                        End If

                                    End If
                                Next
                            Next
                        End If
                    Case "roofs"
                        'Iterate through all assets of the current type where the type is not defined above.
                        'i.e. "lights", "objects", "paths", "patterns", and freestanding "portals".
                        If Not World("levels")(levelnumber)(AssetType) Is Nothing Then
                            If Not World("levels")(levelnumber)(AssetType)(AssetType) Is Nothing Then
                                For Each asset In World("levels")(levelnumber)(AssetType)(AssetType)
                                    'Get the "texture" value for the current asset.
                                    texture = asset("texture")
                                    If Not texture Is Nothing Then
                                        If texture.StartsWith("res://packs/") Then
                                            'If this is from a custom asset pack, then create a new record for it.
                                            Dim UsedAsset As New TextureClass

                                            'Call the "ParseTexture" function to get the info for the custom asset and
                                            'for the custom asset pack it's from.
                                            UsedAsset = ParseTexture(texture, AssetManifest, AssetType, levelname)

                                            'If this asset has already been added to our UsedAssetList, then increment the Instances property
                                            'of the existing record instead of adding the new one as a duplicate.
                                            'Otherwise, add the new record to the list And set the Instances property to 1.
                                            SearchIndex = UsedAssetList.FindIndex(Function(p) p.AssetName = UsedAsset.AssetName)
                                            If SearchIndex >= 0 Then
                                                UsedAssetList(SearchIndex).Instances += 1
                                            Else
                                                UsedAsset.Instances = 1
                                                UsedAssetList.Add(UsedAsset)
                                            End If
                                        End If
                                    End If
                                Next
                            End If
                        End If
                    Case "terrain"
                        'Iterate through all assets where the asset type is "terrain".
                        For Each asset In TerrainArray
                            If Not World("levels")(levelnumber)(AssetType)(asset) Is Nothing Then
                                'Get the "texture" value for the current asset.
                                texture = World("levels")(levelnumber)(AssetType)(asset)
                                If texture.StartsWith("res://packs/") Then
                                    'If this is from a custom asset pack, then create a new record for it.
                                    Dim UsedAsset As New TextureClass

                                    'Call the "ParseTexture" function to get the info for the custom asset and
                                    'for the custom asset pack it's from.
                                    UsedAsset = ParseTexture(texture, AssetManifest, AssetType, levelname)

                                    'If this asset has already been added to our UsedAssetList, then increment the Instances property
                                    'of the existing record instead of adding the new one as a duplicate.
                                    'Otherwise, add the new record to the list And set the Instances property to 1.
                                    SearchIndex = UsedAssetList.FindIndex(Function(p) p.AssetName = UsedAsset.AssetName)
                                    If SearchIndex >= 0 Then
                                        UsedAssetList(SearchIndex).Instances += 1
                                    Else
                                        UsedAsset.Instances = 1
                                        UsedAssetList.Add(UsedAsset)
                                    End If

                                End If
                            End If
                        Next
                    Case "tiles"
                        tiles = World("levels")(levelnumber)(AssetType)
                        Dim TileArray = Nothing
                        Try
                            Dim cells = tiles("cells")
                            TileArray = cells.value.replace("PoolIntArray(", "").replace(")", "").split(",")
                        Catch ex As Exception
                            TileArray = tiles.value.replace("PoolIntArray(", "").replace(")", "").split(",")
                        End Try
                        'Dim cells = tiles("cells")
                        'Dim TileArray = cells.value.replace("PoolIntArray(", "").replace(")", "").split(",")
                        Dim UsedTiles As New List(Of Integer)
                        For Each value In TileArray
                            If CInt(value) > BuiltInTiles And Not UsedTiles.Contains(CInt(value) - BuiltInTiles) Then UsedTiles.Add(CInt(value) - BuiltInTiles)
                        Next

                        Dim PackID As String
                        For Each value In UsedTiles
                            Dim UsedAsset As New TextureClass
                            If value >= 0 And value <= TileList.Count - 1 Then
                                Dim TileInfo() As String = TileList(value).path.Split("/")
                                PackID = TileInfo(0)
                                UsedAsset.AssetName = TileInfo(TileInfo.Count - 1)
                            Else
                                PackID = ""
                                UsedAsset.AssetName = "<asset name not found>"
                            End If

                            UsedAsset.AssetType = "tilesets"
                            UsedAsset.LevelName = levelname

                            For Each AssetPack In AssetManifest
                                If PackID = AssetPack("id") Then
                                    UsedAsset.PackName = AssetPack("name")
                                    UsedAsset.PackVersion = AssetPack("version")
                                    UsedAsset.PackAuthor = AssetPack("author")
                                    Exit For
                                End If
                                'If UsedAsset.PackName = "" Then UsedAsset.PackName = "Pack ID " & PackID & " not found in Asset Manifest"
                                If UsedAsset.PackName = "" And PackID <> "" Then
                                    UsedAsset.PackName = "Pack ID " & PackID & " not found in Asset Manifest"
                                ElseIf UsedAsset.PackName = "" And PackID = "" Then
                                    UsedAsset.PackName = "<missing asset pack>"
                                End If

                                If UsedAsset.PackVersion = "" Then UsedAsset.PackVersion = "<no version number>"
                                If UsedAsset.PackAuthor = "" Then UsedAsset.PackAuthor = "<no author name>"
                            Next

                            'If UsedAsset.PackName = "" And PackID = "" Then UsedAsset.PackName = "<missing asset pack>"
                            UsedAssetList.Add(UsedAsset)
                        Next
                    Case "walls"
                        'Iterate through all assets where the asset type is "walls".
                        If Not World("levels")(levelnumber)(AssetType) Is Nothing Then
                            For Each asset In World("levels")(levelnumber)(AssetType)
                                'Get the "texture" value for the current asset.
                                texture = asset("texture")
                                If texture.StartsWith("res://packs/") Then
                                    'If this is from a custom asset pack, then create a new record for it.
                                    Dim UsedAsset As New TextureClass

                                    'Call the "ParseTexture" function to get the info for the custom asset and
                                    'for the custom asset pack it's from.
                                    UsedAsset = ParseTexture(texture, AssetManifest, AssetType, levelname)

                                    'If this asset has already been added to our UsedAssetList, then increment the Instances property
                                    'of the existing record instead of adding the new one as a duplicate.
                                    'Otherwise, add the new record to the list And set the Instances property to 1.
                                    SearchIndex = UsedAssetList.FindIndex(Function(p) p.AssetName = UsedAsset.AssetName)
                                    If SearchIndex >= 0 Then
                                        UsedAssetList(SearchIndex).Instances += 1
                                    Else
                                        UsedAsset.Instances = 1
                                        UsedAssetList.Add(UsedAsset)
                                    End If

                                End If

                                'Iterate through all portal assets anchored to the current wall.
                                For Each portal In asset("portals")
                                    'Get the "texture" value for the current asset.
                                    texture = portal("texture")
                                    If texture.StartsWith("res://packs/") Then
                                        'If this is from a custom asset pack, then create a new record for it.
                                        Dim UsedAsset As New TextureClass

                                        'Call the "ParseTexture" function to get the info for the custom asset and
                                        'for the custom asset pack it's from.
                                        UsedAsset = ParseTexture(texture, AssetManifest, "portals", levelname)

                                        'If this asset has already been added to our UsedAssetList, then increment the Instances property
                                        'of the existing record instead of adding the new one as a duplicate.
                                        'Otherwise, add the new record to the list And set the Instances property to 1.
                                        SearchIndex = UsedAssetList.FindIndex(Function(p) p.AssetName = UsedAsset.AssetName)
                                        If SearchIndex >= 0 Then
                                            UsedAssetList(SearchIndex).Instances += 1
                                        Else
                                            UsedAsset.Instances = 1
                                            UsedAssetList.Add(UsedAsset)
                                        End If

                                    End If
                                Next
                            Next
                        End If
                    Case Else
                        'Iterate through all assets of the current type where the type is not defined above.
                        'i.e. "lights", "objects", "paths", "patterns", and freestanding "portals".
                        If Not World("levels")(levelnumber)(AssetType) Is Nothing Then
                            For Each asset In World("levels")(levelnumber)(AssetType)
                                'Get the "texture" value for the current asset.
                                texture = asset("texture")
                                If Not texture Is Nothing Then
                                    If texture.StartsWith("res://packs/") Then
                                        'If this is from a custom asset pack, then create a new record for it.
                                        Dim UsedAsset As New TextureClass

                                        'Call the "ParseTexture" function to get the info for the custom asset and
                                        'for the custom asset pack it's from.
                                        UsedAsset = ParseTexture(texture, AssetManifest, AssetType, levelname)

                                        'If this asset has already been added to our UsedAssetList, then increment the Instances property
                                        'of the existing record instead of adding the new one as a duplicate.
                                        'Otherwise, add the new record to the list And set the Instances property to 1.
                                        SearchIndex = UsedAssetList.FindIndex(Function(p) p.AssetName = UsedAsset.AssetName)
                                        If SearchIndex >= 0 Then
                                            UsedAssetList(SearchIndex).Instances += 1
                                        Else
                                            UsedAsset.Instances = 1
                                            UsedAssetList.Add(UsedAsset)
                                        End If
                                    End If
                                End If
                            Next
                        End If
                End Select

                Dim ColorTypeArray As String() = {"lights", "objects", "patterns", "walls", "water"}
                Dim assetcolor As String
                Select Case AssetType
                    Case "objects"
                        For Each asset In World("levels")(levelnumber)(AssetType)
                            Try
                                assetcolor = asset("custom_color")
                                If Not assetcolor Is Nothing Then
                                    Dim ColorAsset As New TextureClass
                                    texture = asset("texture")
                                    ColorAsset = ParseTexture(texture, AssetManifest, AssetType, levelname)
                                    ColorAsset.Color = assetcolor
                                    ColorAssetList.Add(ColorAsset)
                                End If
                            Catch ex As Exception
                                'Do Nothing
                            End Try
                        Next
                    Case Else
                        If Not World("levels")(levelnumber)(AssetType) Is Nothing Then
                            For Each asset In World("levels")(levelnumber)(AssetType)
                                Try
                                    assetcolor = asset("color")
                                    If Not assetcolor Is Nothing Then
                                        Dim ColorAsset As New TextureClass
                                        texture = asset("texture")
                                        ColorAsset = ParseTexture(texture, AssetManifest, AssetType, levelname)
                                        ColorAsset.Color = assetcolor
                                        ColorAssetList.Add(ColorAsset)
                                    End If
                                Catch ex As Exception
                                    'Do Nothing
                                End Try
                            Next
                        End If
                End Select
            Next

            Dim ColorTiles = World("levels")(levelnumber)("tiles")
            Try
                For Each color In ColorTiles("colors")
                    Dim ColorTileAsset As New TextureClass
                    ColorTileAsset.LevelName = levelname
                    ColorTileAsset.Color = color
                    SearchIndex = ColorTileList.FindIndex(Function(p) p.Color = ColorTileAsset.Color)
                    If SearchIndex < 0 Then
                        ColorTileList.Add(ColorTileAsset)
                    End If
                Next
            Catch ex As Exception

            End Try

            Dim CaveGround As New TextureClass
            Dim CaveWall As New TextureClass

            Dim Cave = World("levels")(levelnumber)("cave")
            CaveGround.LevelName = levelname
            CaveGround.AssetName = "cave_ground"
            CaveGround.Color = Cave("ground_color")
            CaveWall.LevelName = levelname
            CaveWall.AssetName = "cave_wall"
            CaveWall.Color = Cave("wall_color")

            ColorCaveList.Add(CaveGround)
            ColorCaveList.Add(CaveWall)

            If Not World("levels")(levelnumber)("water") Is Nothing Then
                If Not World("levels")(levelnumber)("water")("tree") Is Nothing Then
                    If Not World("levels")(levelnumber)("water")("tree")("children") Is Nothing Then
                        Dim Water = World("levels")(levelnumber)("water")("tree")("children")
                        For Each color In Water
                            Dim DeepWaterAsset As New TextureClass
                            Dim ShallowWaterAsset As New TextureClass

                            DeepWaterAsset.LevelName = levelname
                            DeepWaterAsset.AssetName = "deep_color"
                            DeepWaterAsset.Color = color("deep_color")
                            ShallowWaterAsset.LevelName = levelname
                            ShallowWaterAsset.AssetName = "shallow_color"
                            ShallowWaterAsset.Color = color("shallow_color")

                            ColorDeepWaterList.Add(DeepWaterAsset)
                            ColorShallowWaterList.Add(ShallowWaterAsset)
                        Next
                    End If
                End If
            End If
        Next

        'Get some basic map info.
        Message = Indent & "Created with version: " & Header("creation_build") & vbCrLf
        Message &= Indent & "Created on (yyyy/m/d): " & Header("creation_date")("year") & "/" & Header("creation_date")("month") & "/" & Header("creation_date")("day") & vbCrLf
        Message &= Indent & "Grid size (W x H): " & World("width") & " x " & World("height") & vbCrLf

        'If the map includes a trace image, get that info.
        TraceObject = Header("editor_state")("trace_image")
        If TraceObject Is Nothing Then
            Message &= Indent & "Trace Image: none" & vbCrLf & vbCrLf
        ElseIf TraceObject.HasValues Then
            Message &= Indent & "Trace Image: " & Header("editor_state")("trace_image")("image") & vbCrLf & vbCrLf
        Else
            Message &= Indent & "Trace Image: none" & vbCrLf & vbCrLf
        End If

        Message &= Indent & "Color used for grid:" & vbCrLf
        Message &= Indent & Indent & gridcolor & vbCrLf
        Message &= vbCrLf

        Message &= Indent & "Colors used for environment:" & vbCrLf
        For Each EnvironmentColor In ColorEnvironmentList
            Message &= Indent & Indent & EnvironmentColor.Color & " on level [" & EnvironmentColor.LevelName & "]" & vbCrLf
        Next
        Message &= vbCrLf

        Message &= Indent & "Colors used for tilesets:" & vbCrLf
        For Each ColorTile In ColorTileList
            Message &= Indent & Indent & ColorTile.Color & " on level [" & ColorTile.LevelName & "]" & vbCrLf
        Next
        Message &= vbCrLf

        Message &= Indent & "Colors used for caves:" & vbCrLf
        For Each CaveColor In ColorCaveList
            Message &= Indent & Indent & CaveColor.Color & " for [" & CaveColor.AssetName & "] on level [" & CaveColor.LevelName & "]" & vbCrLf
        Next
        Message &= vbCrLf

        Message &= Indent & "Colors used for water:" & vbCrLf
        Message &= Indent & Indent & "Deep colors:" & vbCrLf
        For Each WaterColor In ColorDeepWaterList
            Message &= Indent & Indent & Indent & WaterColor.Color & " on level [" & WaterColor.LevelName & "]" & vbCrLf
        Next
        Message &= vbCrLf

        Message &= Indent & Indent & "Shallow colors:" & vbCrLf
        For Each WaterColor In ColorShallowWaterList
            Message &= Indent & Indent & Indent & WaterColor.Color & " on level [" & WaterColor.LevelName & "]" & vbCrLf
        Next
        Message &= vbCrLf

        Message &= Indent & "Colors used for assets:" & vbCrLf
        For Each AssetColor In ColorAssetList
            Message &= Indent & Indent & AssetColor.Color & " for [" & AssetColor.AssetName & "] of type [" & AssetColor.AssetType & "] on level [" & AssetColor.LevelName & "]" & vbCrLf
        Next
        Message &= vbCrLf

        'Get the list of custom asset packs that were selected when this map was saved.
        Message &= Indent & "Asset Manifest (Pack Name, Version #, Author Name):" & vbCrLf
        If AssetManifest.Count > 0 Then
            For Each AssetPack In AssetManifest
                Message &= Indent & Indent & AssetPack("name") & ", Version " & AssetPack("version") & ", " & AssetPack("author") & vbCrLf
            Next
        Else
            Message &= Indent & Indent & "No custom asset packs found in manifest." & vbCrLf
        End If
        Message &= vbCrLf

        Message &= Indent & "Custom assets in use:" & vbCrLf
        If UsedAssetList.Count > 0 Then
            'Our UsedAssetList has at least one record, then do all the things.
            Dim PackName As String
            Dim PackVersion As String
            Dim PackAuthor As String
            Dim Instances As String

            'Sort the UsedAssetList by the PackName property.
            UsedAssetList.Sort(Function(x, y) x.PackName.CompareTo(y.PackName))

            'List all the custom assets that have been used, grouping them by their respective pack names.
            PackName = UsedAssetList(0).PackName
            PackVersion = UsedAssetList(0).PackVersion
            PackAuthor = UsedAssetList(0).PackAuthor
            Message &= Indent & Indent & "From " & PackName & ", Version " & PackVersion & ", by " & PackAuthor & ":" & vbCrLf
            For AssetIndex = 0 To UsedAssetList.Count - 1
                If PackName <> UsedAssetList(AssetIndex).PackName Then
                    PackName = UsedAssetList(AssetIndex).PackName
                    PackVersion = UsedAssetList(AssetIndex).PackVersion
                    PackAuthor = UsedAssetList(AssetIndex).PackAuthor
                    Message &= vbCrLf & Indent & Indent & "From " & PackName & ", Version " & PackVersion & ", by " & PackAuthor & ":" & vbCrLf
                End If
                If UsedAssetList(AssetIndex).Instances = 1 Then Instances = "instance" Else Instances = "instances"
                Message &= Indent & Indent & Indent & UsedAssetList(AssetIndex).AssetName & " of type [" & UsedAssetList(AssetIndex).AssetType & "]"
                If UsedAssetList(AssetIndex).AssetType <> "tilesets" Then Message &= ", " & UsedAssetList(AssetIndex).Instances & " " & Instances & " found."
                Message &= vbCrLf
            Next
            Message &= vbCrLf
        Else
            'If our UsedAssetList has no records, then indicate that no custom assets have been used.
            Message &= Indent & Indent & "No custom assets are in use." & vbCrLf & vbCrLf
        End If

        Message &= Indent & "Embedded assets:" & vbCrLf
        If Not World("embedded") Is Nothing Then
            If World("embedded").count <= 0 Then
                Message &= Indent & Indent & "No embedded assets were found." & vbCrLf
            Else
                For Each Asset In World("embedded")
                    Message &= Indent & Indent & Asset.Name & vbCrLf
                Next
            End If
        Else
            Message &= Indent & Indent & "No embedded assets were found." & vbCrLf
        End If


        OutputForm.OutputTextBox.AppendText(Message)
        If CreateLog Then My.Computer.FileSystem.WriteAllText(LogFileName, Message, True)

    End Sub
End Module
