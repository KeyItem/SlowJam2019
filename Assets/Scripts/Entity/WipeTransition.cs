using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WipeTransition : MonoBehaviour
{
    public Image transitionImage;   // Select which UI image you want to fade.
    public float speedOfTransition; // How fast this shit goes.
    public float transitionProgress;    // 0 = Start. 1 = Done.
    public bool levelFinished;  // level ends, start transition.

    private Image originalImageValues;
    
    public void Update()
    {
        if (levelFinished)
        {
            StartTransition();
        }
        else
        {
            TransitionRemoval();
        }
    }

    public void StartTransition()
    {
        transitionProgress += Time.deltaTime * (1f / speedOfTransition);
        transitionImage.fillAmount = transitionProgress;

        if (transitionProgress >= 1f)
        {
            transitionProgress = 1;
            levelFinished = true;
        }
    }

    public void TransitionRemoval()
    {
        transitionProgress -= Time.deltaTime * (1f / speedOfTransition);
        transitionImage.fillAmount = transitionProgress;

        if (transitionProgress <= 0)
        {
            transitionProgress = 0;
            levelFinished = false;
        }
    }
}
