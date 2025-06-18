using System.Runtime.Serialization;

[DataContract]
public class SettingsSerialized : SerializedType
{
    private static SettingsSerialized _instance;

    public static SettingsSerialized Instance
    {
        get
        {
            _instance ??= Serializer.ReadData<SettingsSerialized>() as SettingsSerialized;
            _instance ??= new SettingsSerialized();
            return _instance;
        }
    }

    ~SettingsSerialized()
    {
        Serializer.WriteData(_instance);    
    }
    
    [DataMember] public float MasterAudio;
    [DataMember] public float MusicAudio;
    [DataMember] public float SfxAudio;
}
