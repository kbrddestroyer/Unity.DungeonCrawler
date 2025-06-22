using System.Collections.Generic;
using System.Runtime.Serialization;

[DataContract]
public class QuestControllerData : SerializedType
{
    [DataMember] public List<uint> QuestIDs;
}
