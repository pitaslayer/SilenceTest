using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class ScriptLoader : MonoBehaviour
{
    [SerializeField]
    private string scriptFile;

    [SerializeField]
    private string scriptFolderPath;

    private TextAsset sentenceData;

    public delegate void ScriptFileParsed();
    public static event ScriptFileParsed OnScriptFileParsed;

    private void Awake()
    {
        Logic.OnLogicInitialized += LoadFile;
    }

    void LoadFile()
    {
        string fileHander = $"{scriptFolderPath}{scriptFile}";
        sentenceData = Resources.Load(fileHander) as TextAsset;
        ParseTextAsset(sentenceData);
    }

    void ParseTextAsset(TextAsset asset)
    {
        string data = asset.text;

        string[] sentences = Regex.Split(data, "\n");

        foreach (string sentence in sentences)
        {
            Logic.instance.AddSentence(sentence);
        }

        OnScriptFileParsed?.Invoke();
    }
}