using System;

[Flags]
public enum DialogueOptions
{
    NONE = 0,
    
    // Allow the player to move while the dialogue is being shown
    ALLOW_MOVEMENT = 1,
    
    // Do not allow the player to progress to the next dialogue, dialogue must be manually progressed by calling .ProgressDialogue()
    NO_INPUT = 1 << 1,
    
    // Current dialogue cannot be cancelled under any circumstances, any calls to StartDialogue will wait for current to finish
    // unimplemented
    STOP_TIME = 1 << 2,
    
    // Attempts to cancel dialogue that is currently playing
    INTERRUPTING = 1 << 3,
}
