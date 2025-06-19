using System.Collections.Generic;
using System.Runtime.Serialization;

[DataContract]
public class InventoryStorageData : SerializedType
{
    [DataMember] public List<uint> ListItems = new();
}
