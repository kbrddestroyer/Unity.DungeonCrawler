using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class UIController : MonoBehaviour
{
    [SerializeField] private ButtonController[] buttons;

    private void Start()
    {
        foreach (var controller in buttons)
            controller.Initialize();
    }
}
