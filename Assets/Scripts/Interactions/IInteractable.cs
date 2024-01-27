public interface IInteractable
{
    string DisplayText { get; }
    void Interact();
    bool CanInteract();
}