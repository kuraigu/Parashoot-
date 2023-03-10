using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private static DataManager _instance;

    // Path to the save file
    [SerializeField]
    private string _saveFilePath;

    [SerializeField]
    private Data _data;

    public static DataManager instance
    {
        get { return _instance; }
    }

    public Data data
    { get { return _data; } }

    public void Awake()
    {
        _instance = this;
    }

    [System.Serializable]
    public class Data
    {
        public uint highScore;
        public uint totalScore;
        public Item[] items;

        public Settings settings;

        public Data()
        {
            highScore = 0;
            totalScore = 0;

            settings = new Settings();
        }

        [System.Serializable]
        public class Item
        {
            public string itemName;
            public int count;

            public Item()
            {
                itemName = "";
                count = 0;
            }

            public Item(string itemName, int count)
            {
                this.itemName = itemName;
                this.count = count;
            }
        }

        [System.Serializable]
        public class Settings
        {
            public float musicVolume;

            public Settings()
            {
                musicVolume = 1.0f;
            }


            public void ApplySettings()
            {
                if (AudioManager.instance != null)
                {
                    AudioManager.instance.SetVolume(musicVolume);
                }
            }
        }
    }

    // Save the data to a file
    public void SaveData()
    {
        string fullPath = Path.Combine(Application.persistentDataPath, _saveFilePath);
        // Convert the data to a JSON string
        string jsonData = JsonUtility.ToJson(_data);

        // Write the JSON string to a file
        File.WriteAllText(fullPath, jsonData);
    }

    // Load the data from a file
    public void LoadData()
    {
        string fullPath = Path.Combine(Application.persistentDataPath, _saveFilePath);
        if (File.Exists(fullPath))
        {
            // Read the JSON string from the file
            string jsonData = File.ReadAllText(fullPath);

            // Deserialize the JSON string into a Data object
            _data = JsonUtility.FromJson<Data>(jsonData);


            _data.settings?.ApplySettings();
        }

        else
        {
            // If the save file doesn't exist, create a new Data object
            _data = new Data();
            // Create the save file and write the initial data to it
            using (FileStream fs = File.Create(fullPath))
            {
                string jsonData = JsonUtility.ToJson(_data);
                byte[] data = System.Text.Encoding.UTF8.GetBytes(jsonData);
                fs.Write(data, 0, data.Length);
            }
        }
    }
}
