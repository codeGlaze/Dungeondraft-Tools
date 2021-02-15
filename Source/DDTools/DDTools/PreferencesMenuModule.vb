Imports Newtonsoft.Json
Imports System.IO
Module PreferencesMenuModule
    Public Function BuildConfigObject()
        Dim GlobalConfig As New System.Collections.Specialized.OrderedDictionary
        Dim TagAssetsConfig As New System.Collections.Specialized.OrderedDictionary
        Dim ApplyColorThemeConfig As New System.Collections.Specialized.OrderedDictionary
        Dim CreateColorThemeConfig As New System.Collections.Specialized.OrderedDictionary
        Dim ConvertAssetsConfig As New System.Collections.Specialized.OrderedDictionary
        Dim ConvertPacksConfig As New System.Collections.Specialized.OrderedDictionary
        Dim CopyAssetsConfig As New System.Collections.Specialized.OrderedDictionary
        Dim CopyTilesConfig As New System.Collections.Specialized.OrderedDictionary
        Dim DataFilesConfig As New System.Collections.Specialized.OrderedDictionary
        Dim MapDetailsConfig As New System.Collections.Specialized.OrderedDictionary
        Dim PackAssetsConfig As New System.Collections.Specialized.OrderedDictionary
        Dim UnpackAssetsConfig As New System.Collections.Specialized.OrderedDictionary

        GlobalConfig.Add("location_x", 0)
        GlobalConfig.Add("location_y", 0)

        TagAssetsConfig.Add("source", "")
        TagAssetsConfig.Add("default_tag", "")
        TagAssetsConfig.Add("create_log", False)
        TagAssetsConfig.Add("select_all", False)
        TagAssetsConfig.Add("size_width", 802)
        TagAssetsConfig.Add("size_height", 653)

        ApplyColorThemeConfig.Add("map_folder", "")
        ApplyColorThemeConfig.Add("theme_folder", "")
        ApplyColorThemeConfig.Add("create_backup", True)
        ApplyColorThemeConfig.Add("size_width", 1000)
        ApplyColorThemeConfig.Add("size_height", 653)

        CreateColorThemeConfig.Add("map_folder", "")
        CreateColorThemeConfig.Add("theme_folder", "")
        CreateColorThemeConfig.Add("include_custom", True)
        CreateColorThemeConfig.Add("include_nonpalette", False)
        CreateColorThemeConfig.Add("size_width", 802)
        CreateColorThemeConfig.Add("size_height", 205)

        ConvertAssetsConfig.Add("source", "")
        ConvertAssetsConfig.Add("destination", "")
        ConvertAssetsConfig.Add("create_log", False)
        ConvertAssetsConfig.Add("select_all", False)
        ConvertAssetsConfig.Add("size_width", 802)
        ConvertAssetsConfig.Add("size_height", 653)

        ConvertPacksConfig.Add("source", "")
        ConvertPacksConfig.Add("destination", "")
        ConvertPacksConfig.Add("cleanup", True)
        ConvertPacksConfig.Add("create_log", False)
        ConvertPacksConfig.Add("select_all", False)
        ConvertPacksConfig.Add("size_width", 802)
        ConvertPacksConfig.Add("size_height", 653)

        CopyAssetsConfig.Add("source", "")
        CopyAssetsConfig.Add("destination", "")
        CopyAssetsConfig.Add("create_tags", False)
        CopyAssetsConfig.Add("separate_portals", True)
        CopyAssetsConfig.Add("create_log", False)
        CopyAssetsConfig.Add("select_all", False)
        CopyAssetsConfig.Add("size_width", 802)
        CopyAssetsConfig.Add("size_height", 653)

        CopyTilesConfig.Add("source", "")
        CopyTilesConfig.Add("destination", "")
        CopyTilesConfig.Add("create_log", False)
        CopyTilesConfig.Add("select_all", False)
        CopyTilesConfig.Add("size_width", 802)
        CopyTilesConfig.Add("size_height", 653)

        DataFilesConfig.Add("source", False)
        DataFilesConfig.Add("create_log", False)
        DataFilesConfig.Add("size_width", 1000)
        DataFilesConfig.Add("size_height", 653)

        MapDetailsConfig.Add("source", "")
        MapDetailsConfig.Add("create_log", False)
        MapDetailsConfig.Add("select_all", False)
        MapDetailsConfig.Add("size_width", 802)
        MapDetailsConfig.Add("size_height", 653)

        PackAssetsConfig.Add("source", "")
        PackAssetsConfig.Add("destination", "")
        PackAssetsConfig.Add("overwrite", False)
        PackAssetsConfig.Add("create_log", False)
        PackAssetsConfig.Add("select_all", False)
        PackAssetsConfig.Add("size_width", 1000)
        PackAssetsConfig.Add("size_height", 653)

        UnpackAssetsConfig.Add("source", "")
        UnpackAssetsConfig.Add("destination", "")
        UnpackAssetsConfig.Add("create_log", False)
        UnpackAssetsConfig.Add("select_all", False)
        UnpackAssetsConfig.Add("size_width", 802)
        UnpackAssetsConfig.Add("size_height", 653)

        Dim ConfigObject As New System.Collections.Specialized.OrderedDictionary
        ConfigObject.Add("globabl_config", GlobalConfig)
        ConfigObject.Add("tag_assets", TagAssetsConfig)
        ConfigObject.Add("apply_color_theme", ApplyColorThemeConfig)
        ConfigObject.Add("create_color_theme", CreateColorThemeConfig)
        ConfigObject.Add("convert_assets", ConvertAssetsConfig)
        ConfigObject.Add("convert_packs", ConvertPacksConfig)
        ConfigObject.Add("copy_assets", CopyAssetsConfig)
        ConfigObject.Add("copy_tiles", CopyTilesConfig)
        ConfigObject.Add("map_details", MapDetailsConfig)
        ConfigObject.Add("pack_assets", PackAssetsConfig)
        ConfigObject.Add("unpack_assets", UnpackAssetsConfig)

        Dim SerialConfig As String = JsonConvert.SerializeObject(ConfigObject, Formatting.Indented)
        Dim JSONConfig = JsonConvert.DeserializeObject(SerialConfig)
        Return JSONConfig
    End Function

    Public Function GetSavedConfig(ConfigFile As String)
        Dim rawJson = File.ReadAllText(ConfigFile)
        Dim ConfigObject = JsonConvert.DeserializeObject(rawJson)
        Return ConfigObject
    End Function

    Public Sub SaveNewConfig(ConfigObject As Object, ConfigFolderName As String, ConfigFileName As String)
        Dim NewConfig As String = JsonConvert.SerializeObject(ConfigObject, Formatting.Indented)
        If Not Directory.Exists(ConfigFolderName) Then Directory.CreateDirectory(ConfigFolderName)
        My.Computer.FileSystem.WriteAllText(ConfigFileName, NewConfig, False, System.Text.Encoding.ASCII)
    End Sub
End Module
