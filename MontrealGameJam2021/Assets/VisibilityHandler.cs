using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VisibilityHandler : MonoBehaviour
{
    public GameObject player;

    [SerializeField] private float rayHeight = 0;
    [SerializeField] private float range = 10;
    private List<SkinnedMeshRenderer> GFX = new List<SkinnedMeshRenderer>();

     void Start()
    {
        GFX = GetComponentsInChildren<SkinnedMeshRenderer>().ToList();
        player = PlayerSpawner.LocalPlayer;
        Debug.LogError($"Found {GFX.Count} child");
    }

    void Update()
    {
        RaycastHit hit;

        Vector3 origin = transform.position + new Vector3(0, rayHeight, 0);
        Vector3 direction = player.transform.position - transform.position + new Vector3(0, rayHeight, 0);

        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(origin, direction, out hit, range))
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
