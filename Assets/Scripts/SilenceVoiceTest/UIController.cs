using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private UIView currentView;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InitLoadingBar(float maxValue)
    {
        currentView.InitLoadingBar(maxValue);

    }

    public void UpdateSentenceText(string sentence)
    {
        currentView.UpdateSentenceText(sentence);
    }

    public void AnimateLoadingBar(float value)
    {
        currentView.AnimateLoadingBar(value);
    }

    public void ResetLoadingBar()
    {
        currentView.AnimateLoadingBar(0f);
    }
}