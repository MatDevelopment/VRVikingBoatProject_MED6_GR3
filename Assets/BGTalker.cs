using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGTalker : MonoBehaviour
{
    Animator animator;

    private bool isTalking;
    
    float waitTilNextTalk;
    float timeSinceLastTalk;
    float timeSpentTalking;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        waitTilNextTalk = Random.Range(5, 15);
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastTalk += Time.deltaTime;

        if (timeSinceLastTalk >= waitTilNextTalk && !isTalking)
        {
            timeSpentTalking = Random.Range(4, 20);

            StartCoroutine(TalkForSeconds(timeSpentTalking));
        }

        if (isTalking)
        {
            animator.SetBool("isTalking", true);
        }
        else
        {
            animator.SetBool("isTalking", false);
        }
    }

    IEnumerator TalkForSeconds(float talkTime)
    {
        isTalking = true;
        yield return new WaitForSeconds(talkTime);
        waitTilNextTalk = Random.Range(5, 15);
        timeSinceLastTalk = 0;
        isTalking = false;
    }
}
