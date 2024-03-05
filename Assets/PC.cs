using UnityEngine;
using UnityEngine.Serialization;

public class PC : MonoBehaviour, IInteractable, ILabel
{
    public GameEvent PowerOn;
    public GameEvent PowerOff;
    public GameObject LoadingScreen;
    public GameObject OnScreen;
    public GameObject CrashScreen;

    public GameEventString hoverTextChangeEvent;
    public SharedBool timeStopped;
    private bool _canInteract = true;
    public enum Status
    {
        Off,
        Loading,
        On,
        Crash
    }
    public Status PCStatus;
    private const float LoadingDuration = 12.0f;
    private float LoadingCountDown;

    private void Start()
    {
        LoadingCountDown = LoadingDuration;
        LoadingScreen.SetActive(false);
        OnScreen.SetActive(false);
        CrashScreen.SetActive(false);
    }
    
    void OnEnable()
    {
        PowerOn.AddListener(SetInteractOn);
        PowerOff.AddListener(SetInteractOff);
    }
    
    void SetInteractOn()
    {
        _canInteract = true;
    }
    
    void SetInteractOff()
    {
        _canInteract = false;
        Crash();
    }

    public string GetLabel()
    {
        return _canInteract && PCStatus == Status.Off ? "Boot PC (E)" : "";
    }
    
    public bool CanInteract()
    {
        return _canInteract;
    }
    
    public void Interact()
    {
        if (PCStatus == Status.Off)
        {
            PCStatus = Status.Loading;
            hoverTextChangeEvent.TriggerEvent("");
        }
    }
    public void Crash()
    {
        PCStatus = Status.Crash;
        LoadingScreen.SetActive(false);
        OnScreen.SetActive(false);
        
        CrashScreen.SetActive(true);
        LoadingCountDown = LoadingDuration;
    }
    private void Update()
    {
        if (!timeStopped.GetValue())
        {
            if (PCStatus == Status.Loading)
            {
                LoadingScreen.SetActive(true);
                LoadingCountDown -= Time.deltaTime;
                if (LoadingCountDown <= 0f)
                {
                    PCStatus = Status.On;
                }
            }
            if (PCStatus == Status.On)
            {
                LoadingScreen.SetActive(false);
                LoadingCountDown = LoadingDuration;
                OnScreen.SetActive(true);
            }
        }
    }
    public float GetLoadingCountDown()
    {
        return LoadingCountDown;
    }
}
