using System.Runtime.Serialization;

[DataContract]
public abstract class SerializedType
{
    public void Save() => Serializer.WriteData(this);
}
