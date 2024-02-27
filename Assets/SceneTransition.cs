using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    public Image sceneTransition;
    public GameEvent resetLoopEvent;
    [Range(0f, 1f)]
    public float fadeSpeed = 0.05f;

    private void Start()
    {
        EnterTransition();
    }

    private void OnEnable()
    {
        resetLoopEvent.AddListener(ExitTransition);
    }

    private void OnDisable()
    {
        resetLoopEvent.RemoveListener(ExitTransition);
    }

    private void EnterTransition()
    {
        sceneTransition.gameObject.SetActive(true);
        StartCoroutine(FadeFromBlack());
    }
    private void ExitTransition()
    {
        sceneTransition.gameObject.SetActive(true);
        StartCoroutine(FadeToBlack());
    }

    private IEnumerator FadeFromBlack()
    {
        Color target = Color.black;
        target.a = 0.0f;

        float t = 0;
        while (t <= 1f)
        {
            sceneTransition.color = Color.Lerp(Color.black, target, t);
            t += fadeSpeed;
            yield return new WaitForSeconds(1f / 60f);
        }
        sceneTransition.gameObject.SetActive(false);
    }

    private IEnumerator FadeToBlack()
    {
        Color src = Color.black;
        src.a = 0.0f;
        
        float t = 0;
        while (t <= 1.0f)
        {
            sceneTransition.color = Color.Lerp(src, Color.black, t);
            t += fadeSpeed;
            yield return new WaitForSeconds(1 / 60f);
        }

        sceneTransition.color = Color.black;
        yield return new WaitForSeconds(1.0f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
