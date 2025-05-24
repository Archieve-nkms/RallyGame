using TMPro;
using UnityEngine;

public class TextUI : BaseUI, ITextUI
{
    TextMeshProUGUI _text;
    void Awake()
    {
        _text = GetComponentInChildren<TextMeshProUGUI>();
    }
    public virtual void SetText(params string[] texts)
    {
        _text.text = texts[0];
    }
}