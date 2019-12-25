using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    private Animator slidingAnimator;

    // Start is called before the first frame update
    void Start()
    {
        InitAnimator();
    }


    /// <summary>
    /// Method for initializing animator
    /// </summary>
    private void InitAnimator()
    {
        this.slidingAnimator = GetComponentInChildren<Animator>();
    }

    /// <summary>
    /// Causes slider to slide out
    /// </summary>
    public void SlideOut()
    {
        this.slidingAnimator.SetBool("SlidingIn", false);
    }

    /// <summary>
    /// Causes slider to slide in
    /// </summary>
    public void SlideIn()
    {
        this.slidingAnimator.SetBool("SlidingIn", true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
