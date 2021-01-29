using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeeTroughtHandler : MonoBehaviour
{
    private Transform Camera;
    [SerializeField] private GameObject Bubble;
    [SerializeField] private float originHeight = 0.4f;
    public void Start()
    {
        if (PlayerSpawner.LocalPlayer == null)
            Destroy(gameObject);
            
        Camera = PlayerSpawner.LocalPlayer.GetComponent<PlayerMovement>().Camera;
    }

    public void Update()
    {
        RaycastHit hit;

        Vector3 origin = transform.position + new Vector3(0, originHeight, 0);
        Vector3 direction = Camera.position - transform.position;

        int layerMask = 1 << 8;
        layerMask = ~layerMask;
        
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(origin, direction, out hit, Mathf.Infinity, layerMask))
        {
            //If we see the 
            if(hit.transform.CompareTag($"Camera"))
            {
                Debug.DrawRay(origin, direction, Color.red);
                Bubble.SetActive(false);
            }
            else
            {
                Debug.DrawRay(origin, direction, Color.yellow);
                Bubble.SetActive(true);

            }
        }
        else
        {
            Debug.DrawRay(origin, direction, Color.green);
            Bubble.SetActive(true);
        }
    }
}
