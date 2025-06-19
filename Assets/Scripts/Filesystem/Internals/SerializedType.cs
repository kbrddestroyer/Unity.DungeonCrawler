using System.Runtime.Serialization;

[DataContract]
public abstract class SerializedType
{
    public void Save(string filename = null) => Serializer.WriteData(this, filename);
}
