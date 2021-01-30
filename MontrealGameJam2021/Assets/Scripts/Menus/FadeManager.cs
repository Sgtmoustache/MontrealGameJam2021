using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animation))]
public class FadeManager : MonoBehaviour
{
    public static FadeManager _Instance;
    
    private Animation _animations;
    
    // Start is called before the first frame update
    void Start()
    {
        _Instance = this;
        _animations = GetComponent<Animation>();
        
        FadeIn();
    }

    public void FadeIn()
    {
        _animations.Play("FadeIn");
    }
    
    public void FadeOut()
    {
        _animations.Play("FadeOut");
    }

    public IEnumerator FadeInRoutine()
    {
        FadeIn();
        yield return new WaitForSeconds(_animations["FadeIn"].length);
    }
    
    public IEnumerator FadeOutRoutine()
    {
        FadeOut();
        yield return new WaitForSeconds(_animations["FadeOut"].length);
    }
}
