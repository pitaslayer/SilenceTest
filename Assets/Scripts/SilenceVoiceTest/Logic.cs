using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logic : MonoBehaviour
{
    [SerializeField]
    private float silenceWaitingTime;

    [SerializeField]
    private bool isTestRunning;

    [SerializeField]
    private string finishedTest;

    public static Logic instance;
  
    public Queue<string> ScriptQueue { get; set; }

    public UIController uiController;

    public delegate void NextSequenceSelected();
    public static event NextSequenceSelected OnNextSequenceSelected;

    public delegate void LogicInitialized();
    public static event LogicInitialized OnLogicInitialized;

    public delegate void StartDetection(bool flag);
    public static event StartDetection OnStartDetection;

    private float timer;
    private bool waitingForNextSentence;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else Destroy(gameObject);
    }

    private void Start()
    {
        VoiceSilenceDetection.OnSilenceDetected += WaitForNextSentence;
        ScriptLoader.OnScriptFileParsed += TestNextSentence;

        if (ScriptQueue == null)
        {
            ScriptQueue = new Queue<string>();
        }

        OnLogicInitialized?.Invoke();

        uiController.InitLoadingBar(silenceWaitingTime);

        isTestRunning = true;
    }

    private bool IsScriptFinished()
    {
        return ScriptQueue.Count <= 0;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!waitingForNextSentence && isTestRunning) return;

        timer += Time.deltaTime;
        if (timer < silenceWaitingTime)
        {
            uiController.AnimateLoadingBar(timer);
        }
    }

    private string NextSentence()
    {
         return ScriptQueue.Dequeue();
    }

    private void TestNextSentence()
    {
        if (IsScriptFinished())
        {
            Debug.Log("Script is empty");
            isTestRunning = false;
            uiController.UpdateSentenceText(finishedTest);
        }

        else
        {
            waitingForNextSentence = false;
            uiController.ResetLoadingBar();
            uiController.UpdateSentenceText(NextSentence());
            OnStartDetection?.Invoke(true);
        }
    }

    public void AddSentence(string sentence) => ScriptQueue.Enqueue(sentence);

    private void WaitForNextSentence()
    {
            Debug.Log("Waiting new sentence");
            timer = 0;
            waitingForNextSentence = true;
            Invoke("TestNextSentence", silenceWaitingTime);
    }
}