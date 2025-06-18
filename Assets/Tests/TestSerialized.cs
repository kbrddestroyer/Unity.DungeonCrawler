using System.Runtime.Serialization;

[DataContract]
public class TestSerialized : SerializedType
{
    [DataMember] private string _name;
    [DataMember] private int _age;
    
    public string Name => _name;
    public int Age => _age;

    public TestSerialized()
    {
        
    }
    
    public TestSerialized(string name, int age)
    {
        _name = name;
        _age = age;
    }
}
