using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class SceneSwitcher : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private UnityEvent onSceneSwitch;
    [FormerlySerializedAs("MonoBehaviour")]
    [SerializeField] private IValidator validator;
    
    private IEnumerator AsyncSwitchScene()
    {
        onSceneSwitch.Invoke();
        
        yield return SceneManager.LoadSceneAsync(sceneName);
    }

    protected void SwitchScene() => StartCoroutine(AsyncSwitchScene());

    protected void TrySwitchScene()
    {
        if (!validator || validator.Validate())
            SwitchScene();
    }
    
#if UNITY_EDITOR
    private void OnValidate()
    {
        if (onSceneSwitch.GetPersistentEventCount() == 0)
        {
            var inventoryObjects = FindObjectsByType<Inventory>(FindObjectsInactive.Include, FindObjectsSortMode.None);

            foreach (var inventoryObject in inventoryObjects)
            {
                onSceneSwitch.AddListener(inventoryObject.OnLevelLoads);
            }
        }
    }
#endif
}
