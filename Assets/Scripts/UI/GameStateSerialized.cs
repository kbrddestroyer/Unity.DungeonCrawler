using System.Runtime.Serialization;


[DataContract]
public class GameStateSerialized : SerializedType
{
    [DataMember] public int LastLevelId;
}
