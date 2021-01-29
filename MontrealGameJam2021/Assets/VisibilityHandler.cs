using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VisibilityHandler : MonoBehaviour
{

    private GameObject Player;
    private GameObject _player => Player ? Player : PlayerSpawner.LocalPlayer;

    private const float RayHeight = 0.5f;
    private const float Range = 15;
    
    private List<SkinnedMeshRenderer> GFX = new List<SkinnedMeshRenderer>();

     void Start()
    {
        GFX = GetComponentsInChildren<SkinnedMeshRenderer>().ToList();
    }

    void Update()
    {
        RaycastHit hit;

        Vector3 origin = transform.position + new Vector3(0, RayHeight, 0);
        Vector3 direction = _player.transform.position - transform.position + new Vector3(0, RayHeight, 0);

        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(origin, direction, out hit, Range))
        {
            if (hit.transform.CompareTag("Player"))
            {
                Debug.DrawRay(origin, direction, Color.red);
                GFX.ForEach(b => b.enabled = true);
            }
            else
            {
                Debug.DrawRay(origin, direction, Color.yellow);
                GFX.ForEach(b => b.enabled = false);
            }
        }
        else
        {
            Debug.DrawRay(origin, direction, Color.green);
            GFX.ForEach(b => b.enabled = false);
        }
    }
}