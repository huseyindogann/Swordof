using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;
using System;

public class SaveSystem
{
    #region GameSave
    public static void Save(GameData data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream fs = new FileStream(getPath(), FileMode.Create);
        formatter.Serialize(fs, data);
        fs.Close();

    }
    public static GameData Load(GameData data)
    {
        if (!File.Exists(getPath()))
        {
            GameData emptyData = new GameData();
            Save(emptyData);
            return emptyData;
        }
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream fs = new FileStream(getPath(), FileMode.Open);
        data = formatter.Deserialize(fs) as GameData;
        fs.Close();
        return data;
    }


    private static string getPath()
    {
        return Application.persistentDataPath + Path.DirectorySeparatorChar + "data.qnd";
    }
    #endregion

    #region JSON save

    public static void SaveToJSON<T>(List<T> toSave,string filename)
    {
        //Debug.Log(GetPathJSON(filename));
        string content = JsonHelper.ToJson<T>(toSave.ToArray());
        WriteFile(GetPathJSON(filename), content);

    }
    public static void SaveToJSON<T>(T toSave, string filename)
    {
        Debug.Log(GetPathJSON(filename));
        string content = JsonUtility.ToJson(toSave);
        WriteFile(GetPathJSON(filename), content);

    }
    public static List<T> ReadListFromJSON<T>(string filename)
    {
        string content = ReadFile(GetPathJSON(filename));
        Debug.Log(content);

        if(string.IsNullOrEmpty(content) || content =="{}") { return null; }

        List<T> res = JsonHelper.FromJson<T>(content).ToList();
        return res;


    }
    public static T ReadFromJSON<T>(string filename)
    {
        string content = ReadFile(GetPathJSON(filename));

        if (string.IsNullOrEmpty(content) || content == "{}") { return default(T); }

        T res = JsonUtility.FromJson<T>(content);
        return res;


    }
    public static string GetPathJSON(string filename)
    {
        return Application.persistentDataPath + "/" +filename;
    }
    private static void WriteFile(string path,string content)
    {
        FileStream fileStream=new FileStream(path, FileMode.Create);
        using (StreamWriter writer = new StreamWriter(fileStream)) { 
            writer.Write(content); }
    }
   private static string ReadFile(string path)
    {
        if (File.Exists(path))
        {
            using (StreamReader reader = new StreamReader(path))
            {
                string content = reader.ReadToEnd();
                return content;
            }
        }
        else
        {
            Debug.Log("Kayýt Dosyasý Yok");
            return null;
        }

    }
    public static class JsonHelper
    {
        public static T[] FromJson<T>(string json)
        {
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
            return wrapper.Items;
        }

        public static string ToJson<T>(T[] array)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Items = array;
            return JsonUtility.ToJson(wrapper);
        }

        public static string ToJson<T>(T[] array, bool prettyPrint)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Items = array;
            return JsonUtility.ToJson(wrapper, prettyPrint);
        }

        [Serializable]
        private class Wrapper<T>
        {
            public T[] Items;
        }
    }

    #endregion

}
