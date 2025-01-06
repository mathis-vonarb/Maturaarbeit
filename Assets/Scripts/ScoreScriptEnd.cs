using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreScriptEnd : MonoBehaviour
{
    public FloatVariable score;
    public TMP_Text mText;

    void Update()
    {
        mText.SetText(score.value.ToString());
    }
}

