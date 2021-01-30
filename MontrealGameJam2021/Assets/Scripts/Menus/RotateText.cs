using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateText : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        this.gameObject.transform.rotation = Quaternion.Euler(45, 45, 0);
    }
}
