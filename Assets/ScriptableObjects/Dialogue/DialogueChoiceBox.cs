using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueChoiceBoxController : MonoBehaviour
{
    public GameObject optionPrefab;

    private List<DialogueChoice> _dialogueChoices = new List<DialogueChoice>();

    private int _currentIndex = 0;

    private int _currentNumChoices = 0;

    public void SetChoices(string[] choices)
    {
        if (_dialogueChoices.Count < choices.Length)
            ExpandObjectPool(choices.Length);
        
        _currentNumChoices = choices.Length;
        for (int i = 0; i < _currentNumChoices; i++)
        {
            _dialogueChoices[i].gameObject.SetActive(true);
            _dialogueChoices[i].SetChoice(choices[i]);
            _dialogueChoices[i].Deselect();
        }
        _currentIndex = 0;
        _dialogueChoices[_currentIndex].Select();
    }

    public void MoveDown()
    {
        _dialogueChoices[_currentIndex].Deselect();
        _currentIndex = mod(_currentIndex + 1, _currentNumChoices);
        _dialogueChoices[_currentIndex].Select();
    }

    public void MoveUp()
    {
        _dialogueChoices[_currentIndex].Deselect();
        _currentIndex = mod(_currentIndex - 1, _currentNumChoices);
        _dialogueChoices[_currentIndex].Select();
    }

    public string CurrentChoice()
    {
        return _dialogueChoices[_currentIndex].GetChoice();
    }

    private void ExpandObjectPool(int targetCount)
    {
        while (_dialogueChoices.Count < targetCount)
        {
            var go = Instantiate(optionPrefab, transform);
            _dialogueChoices.Add(go.GetComponent<DialogueChoice>());
            go.SetActive(false);
        }
    }
    
    // need a custom mod here because modulus operator in c# is not a real modulus operator
    // this mod works with negative numbers
    private int mod(int x, int m)
    {
        int r = x % m;
        return r < 0 ? r+m : r;
    }
}
