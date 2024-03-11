using UnityEngine;

public class MovementTutorialGuide : MonoBehaviour
{
    public CharacterMovement player;
    public Transform phone;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(player.transform.position, phone.position) < 5f)
        {
            Debug.Log(player.transform.position);
            Debug.Log(transform.position);
            player.TeleportTo(transform.position);
        }
    }
}
