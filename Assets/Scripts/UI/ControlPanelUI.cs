using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPanelUI : MonoBehaviour
{
    public GameEvent controlPanelInteracted;
    public GameObject controlPanelUI;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnEnable()
    {
        controlPanelInteracted.AddListener(OnControlPanelInteracted);
    }

    public void OnDisable()
    {
        controlPanelInteracted.RemoveListener(OnControlPanelInteracted);
    }

    public void OnControlPanelInteracted()
    {
        controlPanelUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseControlPanel()
    {
        controlPanelUI.SetActive(false);
    }
}
