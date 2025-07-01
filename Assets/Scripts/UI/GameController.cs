using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    [SerializeField] private int defaultSceneIndex;
    
    private static IEnumerator DoAsyncSwitch(int sceneId)
    {
        yield return SceneManager.LoadSceneAsync(sceneId);
    }
    
    public void ContinueGame()
    {
        if (Serializer.ReadData<GameStateSerialized>() is not GameStateSerialized levelState) return;

        StartCoroutine(DoAsyncSwitch(levelState.LastLevelId));
    }

    public void SaveState()
    {
        var sceneId = SceneManager.GetActiveScene().buildIndex;

        GameStateSerialized data = new()
        {
            LastLevelId = sceneId
        };

        Serializer.WriteData(data);
    }

    public void RestartGameCompletely()
    {
        Serializer.DeleteAllData();
        
        StartCoroutine(DoAsyncSwitch(defaultSceneIndex));
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
    public void Start()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnDestroy()
    {
        if (Instance != this) return;
        Instance = null;
        SaveState();
    }
}
