using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SwitchSceneOnClick : MonoBehaviour
{
    public string sceneName;
    private Button _button;
    // Start is called before the first frame update
    void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(SwitchScene);
    }
    
    private void SwitchScene()
    {
        SceneManager.LoadScene(sceneName);
    }

}
