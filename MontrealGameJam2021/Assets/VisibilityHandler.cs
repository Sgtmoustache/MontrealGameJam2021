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
    private const float VFX_TTL = 1;
    
    private List<SkinnedMeshRenderer> GFX = new List<SkinnedMeshRenderer>();

    private GameObject vfx_disapeer;
    

    private bool _CurrentVisibility;

    private bool currentVisibility
    {
        set
        {
            if (value != _CurrentVisibility || !value)
            {
                //ADD EFFECT
            }

            _CurrentVisibility = value;
        }
    }

    void Start()
    {
        GFX = GetComponentsInChildren<SkinnedMeshRenderer>().ToList();
    }

    void Update()
    {
        RaycastHit hit;

        Vector3 origin = transform.position + new Vector3(0, RayHeight, 0);
        Vector3 direction = _player.transform.position - transform.position + new Vector3(0, RayHeight, 0);

        int layerMask = 1 << 8;
        layerMask = ~layerMask;
        
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(origin, direction, out hit, Range, layerMask))
        {
            if (hit.transform.CompareTag("Player"))
            {
                Debug.DrawRay(origin, direction, Color.red);
                currentVisibility = true;
                GFX.ForEach(b => b.enabled = true);
            }
            else
            {
                Debug.DrawRay(origin, direction, Color.yellow);
                currentVisibility = false;
                GFX.ForEach(b => b.enabled = false);
            }
        }
        else
        {
            Debug.DrawRay(origin, direction, Color.green);
            currentVisibility = false;
            GFX.ForEach(b => b.enabled = false);
        }
    }
}
