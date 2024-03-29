using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScreenSpaceToCanvas : MonoBehaviour
{
    public float thresholdDistance = 100.0f;
    public RectTransform canvas;

    public GameObject promptCirclePrefab;
    public GameControls gameControls;

    [Header("Triggers")]
    public GameEventVector3 lookAtEvent;
    
    private HintBubble[] _hintBubbles;
    private Camera _cam;

    private RectTransform[] _promptCircles;

    private PromptScript _currentPromptCircle;
    
    [Header("Listening To")]
    public GameEvent HUDDisableEvent;
    public GameEvent HUDEnableEvent;
    public GameEvent timeStopStartEvent;
    public GameEvent timeStopEndEvent;

    private bool _timeStopped = false;
    private bool _hudDisabled = false;
    private DialogueController _dialogueController;

    // Start is called before the first frame update
    void Start()
    {
        _cam = Camera.main;
        _dialogueController = FindObjectOfType<DialogueController>();
        _hintBubbles = FindObjectsOfType<HintBubble>();
        StartCoroutine(ShowNewlyUnlockedHints());
        _promptCircles = new RectTransform[_hintBubbles.Length];
        for (int i = 0; i < _hintBubbles.Length; i++)
        {
            var go = Instantiate(promptCirclePrefab, transform);
            _promptCircles[i] = go.GetComponent<RectTransform>();
            go.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_timeStopped && !_hudDisabled)
        {
            AlignPromptCircles();
            
            var previousPromptCircle = _currentPromptCircle;
            float minDist = Mathf.Infinity;
            int candidate = -1;
            for (int i = 0; i < _promptCircles.Length; i++)
            {
                var rect = _promptCircles[i];
                if (rect.gameObject.activeInHierarchy)
                {
                    if (minDist > rect.anchoredPosition.SqrMagnitude())
                    {
                        candidate = i;
                        minDist = rect.anchoredPosition.SqrMagnitude();
                    }
                }
            }

            if (candidate >= 0 && minDist < thresholdDistance*thresholdDistance)
            {
                _currentPromptCircle = _promptCircles[candidate].GetComponent<PromptScript>();
                _currentPromptCircle.ShowHint();
                if (gameControls.Wrapper.Player.Interact.WasPerformedThisFrame() &&
                    _hintBubbles[candidate].hintData.IsUnlocked() &&
                    _hintBubbles[candidate].hintData.WasShown() &&
                    _hintBubbles[candidate].hintData.hintDialogue != null)
                {
                    lookAtEvent.TriggerEvent(_hintBubbles[candidate].transform.position);
                    _currentPromptCircle.MarkAsRead();
                    _dialogueController.StartDialogue(_hintBubbles[candidate].hintData.hintDialogue);
                }
            }
            else
            {
                _currentPromptCircle = null;
            }
            
            if (previousPromptCircle != _currentPromptCircle && previousPromptCircle != null)
            {
                previousPromptCircle.HideHint();
            }
        }
    }

    private void OnEnable()
    {
        HUDDisableEvent.AddListener(HandleDisableHUD);
        timeStopEndEvent.AddListener(HandleTimeStopEnd);
        timeStopStartEvent.AddListener(HandleTimeStopStart);
        HUDEnableEvent.AddListener(HandleEnableHUD);
    }

    private void OnDisable()
    {
        HUDDisableEvent.RemoveListener(HandleDisableHUD);
        timeStopEndEvent.RemoveListener(HandleTimeStopEnd);
        timeStopStartEvent.RemoveListener(HandleTimeStopStart);
        HUDEnableEvent.RemoveListener(HandleEnableHUD);
    }

    private IEnumerator ShowNewlyUnlockedHints()
    {
        yield return null;
        gameControls.Wrapper.Player.Disable();
        for (int i = 0; i < _hintBubbles.Length; i++)
        {
            var hintData = _hintBubbles[i].hintData;
            var promptScript = _promptCircles[i].GetComponent<PromptScript>();
            if (hintData.IsUnlocked())
            {
                if (!hintData.WasShown())
                {
                    hintData.SetShown();
                    lookAtEvent.TriggerEvent(_hintBubbles[i].transform.position);
                    yield return new WaitForSeconds(1f);
                    promptScript.SetHintData(hintData);
                    promptScript.PlayNotification();
                    yield return new WaitForSeconds(0.5f);
                }
                else
                {
                    promptScript.SetHintData(hintData);
                }
            }
        }
        gameControls.Wrapper.Player.Enable();
    }
    
    private void HandleTimeStopStart()
    {
        _timeStopped = true;
    }
    
    private void HandleTimeStopEnd()
    {
        _timeStopped = false;
        SetActivePrompts(false);
    }
    
    private void HandleDisableHUD()
    {
        _hudDisabled = true;
        SetActivePrompts(false);
    }
    
    private void HandleEnableHUD()
    {
        _hudDisabled = false;
    }
    
    private void SetActivePrompts(bool active)
    {
        foreach (var t in _promptCircles)
        {
            t.gameObject.SetActive(active);
        }
    }
    private void AlignPromptCircles()
    {
        for (int i = 0; i < _hintBubbles.Length; i++)
        {
            var bubble = _hintBubbles[i];
            var rect = _promptCircles[i];
            
            Vector2 localpoint;
            var screenPoint = RectTransformUtility.WorldToScreenPoint(_cam, bubble.transform.position);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, screenPoint, null, out localpoint);
            if (!PointIsVisible(bubble.transform.position))
            {
                rect.gameObject.SetActive(false);
            }
            else
            {
                rect.gameObject.SetActive(true);
                rect.anchoredPosition = localpoint;
            }
        }
    }

    private bool PointIsVisible(Vector3 worldPoint)
    {
        var viewport = _cam.WorldToViewportPoint(worldPoint);
        return 0 < viewport.x && viewport.x < 1 && 0 < viewport.y && viewport.y < 1 && viewport.z > 0;
    }
}
