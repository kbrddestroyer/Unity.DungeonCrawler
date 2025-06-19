using System;
using System.IO;
using System.Runtime.Serialization.Json;
using UnityEngine;

public class Serializer
{
    public static void WriteData(SerializedType serializable, string fname = null)
    {
        fname ??= serializable.GetType().Name + ".json";
        
        var path = Application.persistentDataPath + "/" + fname;
        Debug.Log(path);
        var serializer = new DataContractJsonSerializer(serializable.GetType());

        using var stream = new FileStream(path, FileMode.Create);
        stream.Flush();
        serializer.WriteObject(stream, serializable);
    }

    public static SerializedType ReadData<T>(string fname = null)
    {
        fname ??= typeof(T).Name + ".json";
        
        if (typeof(T).BaseType != typeof(SerializedType))
            throw new NotImplementedException();
        
        var path = Application.persistentDataPath + "/" + fname;

        if (!File.Exists(path))
            return null;
        
        var serializer = new DataContractJsonSerializer(typeof(T));
        
        using var stream = new FileStream(path, FileMode.Open);
        return serializer.ReadObject(stream) as SerializedType;
    }
}
