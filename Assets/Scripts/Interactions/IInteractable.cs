public interface IInteractable
{
    // The Text that should be displayed at the cursor when the player hovers over the Object
    string DisplayText { get; }
    
    // This function will be called with the player interacts with the object
    void Interact();

    // Can have the interactable object enable or disable interaction to play an animation
    bool CanInteract();
}