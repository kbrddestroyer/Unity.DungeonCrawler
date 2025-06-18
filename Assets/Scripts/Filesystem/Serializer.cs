using System;
using System.IO;
using System.Runtime.Serialization.Json;
using UnityEngine;

public class Serializer
{
    public static void WriteData(SerializedType serializable)
    {
        var path = Application.persistentDataPath + "/" + serializable.GetType().Name + ".json";

        var serializer = new DataContractJsonSerializer(serializable.GetType());

        using var stream = new FileStream(path, FileMode.OpenOrCreate);
        serializer.WriteObject(stream, serializable);
    }

    public static SerializedType ReadData<T>()
    {
        if (typeof(T).BaseType != typeof(SerializedType))
            throw new NotImplementedException();
        
        var path = Application.persistentDataPath + "/" + typeof(T).Name + ".json";

        if (!File.Exists(path))
            return null;
        
        var serializer = new DataContractJsonSerializer(typeof(T));
        
        using var stream = new FileStream(path, FileMode.Open);
        return serializer.ReadObject(stream) as SerializedType;
    }
}
