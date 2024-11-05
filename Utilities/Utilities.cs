using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Newtonsoft.Json;

namespace Lecture_2024_2025_Notes.Utilities;

public static class Utilities
{
    private static string _filePath;
    public static string FilePath
    {
        get
        {
            if (string.IsNullOrEmpty(_filePath))
            {
#if WINDOWS
                _filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                    "AppData", "LocalLow", "NTUA_Notes.Data");
#elif ANDROID
                _filePath = Path.Combine(FileSystem.AppDataDirectory, "ntuanotesdata");
#endif
                if (!Directory.Exists(_filePath))
                {
                    Directory.CreateDirectory(_filePath);
                }
            }
#if DEBUG
            Console.WriteLine(_filePath);
#endif
            return _filePath;
        }

        private set { _filePath = value; }
    }

    public static string SaveDataPath
    {
        get
        {
            string path = Path.Combine(FilePath, "SavedData");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path;
        }
    }
    public static bool SaveDataToJson(string fileName, object data, bool overwriteIfExists = true)
    {
        bool result = false;

        if (string.IsNullOrWhiteSpace(fileName))
            return result;

        string filePath = Path.Combine(SaveDataPath, fileName);

        if (!overwriteIfExists && File.Exists(filePath))
            return false;

        string json = JsonConvert.SerializeObject(data
#if DEBUG
            , Formatting.Indented);
#else
            );
#endif


        using StreamWriter writer = new StreamWriter(filePath);
        writer.Write(json);

        return true;
    }
    public static bool LoadDataFromJson<T>(string fileName, out T data)
    {
        data = default;
        if (string.IsNullOrWhiteSpace(fileName))
            return false;

        string filePath = Path.Combine(SaveDataPath, fileName);
        if (!File.Exists(filePath))
            return false;

        using StreamReader reader = new StreamReader(filePath);
        string json = reader.ReadToEnd();
        data = JsonConvert.DeserializeObject<T>(json);

        return true;
    }

    public async static void ShowToast(string message, ToastDuration duration = ToastDuration.Short)
    {
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        var toast = Toast.Make(message, duration);

        await toast.Show(cancellationTokenSource.Token);
    }
}
