using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Jarvis : MonoBehaviour
{
    public GameEventInt changeExpressionEvent;
    public Image panel;
    public HashMap<JarvisExpression, Sprite> sprites;

    private void OnEnable()
    {
        changeExpressionEvent.AddListener(ChangeExpression);
    }

    private void OnDisable()
    {
        changeExpressionEvent.RemoveListener(ChangeExpression);
    }

    private void ChangeExpression(int expression)
    {
        if (expression == (int)JarvisExpression.Empty)
        {
            panel.sprite = null;
            return;
        }
        panel.sprite = sprites[(JarvisExpression)expression];
    }
}

public enum JarvisExpression
{
    Empty = 0,
    Neutral,
    Happy,
    Sad,
    Suprised,
    Angry,
    Mocking,
}
