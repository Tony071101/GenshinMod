using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkerPlayer : MonoBehaviour
{
    [Header("Eyes Blinking Period")]
    [SerializeField] private int blinkSecconds;

    [Header("Reference to Character Animator")]
    [SerializeField] private Animator anim;

    private bool corrutineStarted;
    private bool isBlinking;
    private float animPlayTime = 0.015f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Blink();
    }

    private void Blink() {
        if(!isBlinking){
            if(!corrutineStarted){
                StartCoroutine(BlinkSeconds(blinkSecconds));
            }
        }
        else{
            StartCoroutine(MakeAnim());
        }
    }

    IEnumerator BlinkSeconds (int blinkSec){
        corrutineStarted = true;
        yield return new WaitForSeconds(blinkSec);

        isBlinking = true;
    }

    IEnumerator MakeAnim(){
        anim.SetBool("isBlink", true);

        yield return new WaitForSeconds(animPlayTime);

        isBlinking = false;
        corrutineStarted = false;

        anim.SetBool("isBlink", false);
    }
}
