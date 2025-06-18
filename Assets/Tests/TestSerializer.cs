using NUnit.Framework;

public class TestSerializer
{
    [Test]
    public void TestSerializerSimplePasses()
    {
        var serialized = new TestSerialized("Keyboard Destroyer", 21);
        serialized.Save();

        var serializedLoad = Serializer.ReadData<TestSerialized>() as TestSerialized;
        
        Assert.NotNull(serializedLoad);
        Assert.AreEqual(serializedLoad.Name, serialized.Name);
        Assert.AreEqual(serializedLoad.Age, serialized.Age);
    }
}
