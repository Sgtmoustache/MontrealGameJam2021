using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(Animation))]
public class FadeManager : MonoBehaviourPun
{
    public static FadeManager _Instance;
    
    private Animation _animations;
    
    // Start is called before the first frame update
    void Start()
    {
        _Instance = this;
        _animations = GetComponent<Animation>();
    }

    [PunRPC]
    public void FadeIn()
    {
        Debug.LogWarning("--FADE IN--");
        _animations.Play("FadeIn");
    }
    
    [PunRPC]
    public void FadeOut()
    {
        Debug.LogWarning("--FADE OUT--");
        _animations.Play("FadeOut");
    }

    public IEnumerator FadeInRoutine()
    {
        Debug.LogWarning("--FADE IN ROUTINE--");
        photonView.RPC("FadeIn", RpcTarget.All);
        yield return new WaitForSeconds(_animations["FadeIn"].length);
    }
    
    public IEnumerator FadeOutRoutine()
    {
        Debug.LogWarning("--FADE OUT ROUTINE--");
        photonView.RPC("FadeOut", RpcTarget.All);
        yield return new WaitForSeconds(_animations["FadeOut"].length);
    }
}
