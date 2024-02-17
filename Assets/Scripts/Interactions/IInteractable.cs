public interface IInteractable
{
    // This function will be called with the player interacts with the object
    void Interact();

    // Can have the interactable object enable or disable interaction to play an animation
    bool CanInteract();
}