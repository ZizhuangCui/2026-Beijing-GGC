using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private string promptOpen = "开门";
    [SerializeField] private string promptClose = "关门";
    [SerializeField] private bool isOpen;

    public string Prompt => isOpen ? promptClose : promptOpen;

    public int Priority => 100; // 门更优先

    public void Interact(GameObject interactor)
    {
        isOpen = !isOpen;
        Debug.Log(isOpen ? "Door opened" : "Door closed");

        // TODO: 播动画、禁用Collider等
        // GetComponent<Collider2D>().enabled = !isOpen;
    }
}
