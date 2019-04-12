using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour {

    public Animator animator;
    public float NextAnimation = .5f;
    private float t_NextAnimation = 0;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update ()
    {
        if (!animator.GetBool("PlaySwing"))
        {
            if (t_NextAnimation > NextAnimation)
            {

                animator.SetBool("PlaySwing", true);

                NextAnimation = Random.Range(.5f, 5);
                t_NextAnimation = 0;
            }

            t_NextAnimation += Time.deltaTime;
        }
	}

    public void SwitchState()
    {
        animator.SetBool("PlaySwing", false);
    }
}
