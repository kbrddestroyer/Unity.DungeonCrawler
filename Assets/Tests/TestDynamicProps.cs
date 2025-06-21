using NUnit.Framework;
using UnityEngine;

public class TestDynamicProps
{
    [Test]
    public void TestFloatProperty()
    {
        var dynProp = new FloatDynamicProperty();
        
        Assert.That(dynProp.Value, Is.EqualTo(1));
        
        var addProp = new FloatDynamicProperty(0.5f);
        var addProp2 = new FloatDynamicProperty(2f);
        dynProp.Add(addProp);
        Assert.That(addProp.Value, Is.EqualTo(0.5f));
        dynProp.Add(addProp2);
        Assert.That(dynProp.Value, Is.EqualTo(1));
        dynProp.Remove(addProp);
        Assert.That(dynProp.Value, Is.EqualTo(2));
    }

    [Test]
    public void TestResists()
    {
        var dynProp = new FloatDynamicProperty(2.0f);
        dynProp.Value -= 1.0f;
        
        Assert.That(dynProp.Value, Is.EqualTo(1.0f));
        
        var addProp = new FloatDynamicProperty(2f);
        dynProp.Add(addProp);
        
        Assert.That(dynProp.Value, Is.EqualTo(2.0f));
        
        dynProp.Value += 1.0f;
        
        Assert.That(dynProp.Value, Is.EqualTo(3.0f));
    }

    [Test]
    public void TestPlayerHealth()
    {
        var go = new GameObject();
        go.AddComponent<BoxCollider2D>();
        var player = go.AddComponent<Player>();

        player.Start();
        
        Assert.That(player, Is.Not.Null);
        Assert.That(player.Health, Is.EqualTo(0));
        
        player.Health = 100;
        Assert.That(player.Health, Is.EqualTo(100));
        
        var dynProperty = new FloatDynamicProperty(2);
        player.HealthMul.Add(dynProperty);
        Assert.That(player.HealthMul.Value, Is.EqualTo(200));

        player.Health -= 10;
        Assert.That(player.Health, Is.EqualTo(190));
        
        player.HealthMul.Remove(dynProperty);
        Assert.That(player.HealthMul.Value, Is.EqualTo(190 / 2));
    }
}
