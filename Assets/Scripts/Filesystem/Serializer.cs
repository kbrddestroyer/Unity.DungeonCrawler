using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using UnityEngine;

public class Serializer
{
    public static void WriteData(SerializedType serializable, string fname = null)
    {
        fname ??= serializable.GetType().Name + ".json";
        fname += ".gamedata";
        
        var path = Application.persistentDataPath + "/" + fname;
        Debug.Log(path);
        var serializer = new DataContractJsonSerializer(serializable.GetType());
        
        using var stream = new FileStream(path, FileMode.Create);
        stream.Flush();
        serializer.WriteObject(stream, serializable);
        stream.Close();
    }

    public static SerializedType ReadData<T>(string fname = null)
    {
        fname ??= typeof(T).Name + ".json";
        fname += ".gamedata";
        
        if (typeof(T).BaseType != typeof(SerializedType))
            throw new NotImplementedException();
        
        var path = Application.persistentDataPath + "/" + fname;

        if (!File.Exists(path))
            return null;
        
        var serializer = new DataContractJsonSerializer(typeof(T));
        
        using var stream = new FileStream(path, FileMode.Open);
        return serializer.ReadObject(stream) as SerializedType;
    }

    public static void DeleteAllData()
    {
        foreach (var file in Directory.GetFiles(Application.persistentDataPath, "*.gamedata"))
        {
            File.Delete(file);
        }
    }
}
