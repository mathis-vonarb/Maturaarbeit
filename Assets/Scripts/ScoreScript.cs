using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreScript : MonoBehaviour
{
    public FloatVariable score;
    public TMP_Text mText;
    
    void Start()
    {
        score.value = 0;
    }

    void Update()
    {
        mText.SetText(score.value.ToString());
    }
}
