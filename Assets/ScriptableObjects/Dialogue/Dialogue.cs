using System;
using UnityEngine;

[Serializable]
public class Dialogue
{
    public string Speaker;
    [TextArea]
    public string Body;

    public bool changeExpression;
    public GameEventInt changeExpressionEvent;
    public JarvisExpression expression;
}