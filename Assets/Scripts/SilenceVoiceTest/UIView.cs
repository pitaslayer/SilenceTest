using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIView : MonoBehaviour
{
    [SerializeField]
    private Text sentenceText;

    [SerializeField]
    private Slider loadingBar;
   
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitLoadingBar(float loadingBarValue)
    {
        loadingBar.maxValue = loadingBarValue;
    }

    public void UpdateSentenceText(string sentence)
    {
        sentenceText.text = sentence;
    }

    public void AnimateLoadingBar(float value)
    {
        loadingBar.value = value;
    }
}
