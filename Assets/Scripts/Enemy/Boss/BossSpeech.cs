using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BossSpeech : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textDisplay = null;
    [SerializeField]
    private string[] speeches = null;
    private int index;
    [SerializeField]
    private float typingSpeed = 0.05f;
    private bool canStartTalk = true;
    [SerializeField]
    private float cinematicPause = 5f;
    private const int beginningSize = 3;
    private const int middleSize = 2;
    private const int endSize = 1;
    enum State
    {
        Idle,
        Beginning,
        Middle,
        End
    }
    private State state;

    // Start is called before the first frame update
    void Start()
    {
        state = State.Idle;
    }

    private IEnumerator Type()
    {
        foreach(char letter in speeches[index].ToCharArray())
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    private int GetNewIndex()
    {
        switch (state)
        {
            case State.Idle: return 0;
            case State.Beginning: return beginningSize;
            case State.Middle: return middleSize;
            case State.End: return endSize;
            default: return 0;
        }
    }

    private void HandleSpeechOrder()
    {
        switch (state)
        {
            case State.Idle:
            {
                break;
            }
            case State.Beginning:
            {
                StartCoroutine(SpeechPhase(3));
                break;
            }
            case State.Middle:
            {
                StartCoroutine(SpeechPhase(2));
                break;
            }
            case State.End:
            {
                StartCoroutine(SpeechPhase(1));
                break;
            }
        }
    }

    private IEnumerator SpeechPhase(int size)
    {
        for(int i = 0; i < size; i++)
        {   
            textDisplay.text = "";
            NextSpeech();
            yield return new WaitForSeconds(cinematicPause);
        }
    }

    private void NextSpeech()
    {
        if (index < speeches.Length - 1) {
            StartCoroutine(Type());
            index++;
        }
    }

    public void ActivateDialogManager()
    {
        if (canStartTalk) 
        {
            canStartTalk = false;
            state++;
            HandleSpeechOrder();
        }
    }

    public void DeactivateDialogManager()
    {
        textDisplay.text = "";
        canStartTalk = true;
        StopAllCoroutines();
        index = GetNewIndex();
    }
}
