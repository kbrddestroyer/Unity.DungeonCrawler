using System.Runtime.Serialization;

[DataContract]
public class LevelSerialized : SerializedType
{
    [DataMember] public uint LevelID;
    [DataMember] public float PlayerHp;
    [DataMember] public float PlayerXp;
}
