public interface IInteractable
{
    string Prompt { get; }
    int Priority { get; }
    void Interact(UnityEngine.GameObject interactor);
}
