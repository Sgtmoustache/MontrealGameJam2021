using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VisibilityHandler : MonoBehaviour
{
    private GameObject Player;
    private GameObject _player => Player ? Player : PlayerSpawner.LocalPlayer;

    private const float RayHeight = 1f;
    private const float Range = 15;
    
    private List<Renderer> GFX = new List<Renderer>();

    [SerializeField] private LayerMask IgnoreAllExcept;
    
     void Start()
    {
        GFX = GetComponentsInChildren<Renderer>().Where(b => !b.gameObject.CompareTag("VisibilityIndependant")).ToList();
    }

    void Update()
    {
        if (!_player)
            return;
        
        RaycastHit hit;

        Vector3 origin = transform.position + new Vector3(0, RayHeight, 0);
        Vector3 direction = _player.transform.position - transform.position + new Vector3(0, RayHeight, 0);

        if (Physics.Raycast(origin, direction, out hit, Range, ~IgnoreAllExcept))
        {
            if (hit.transform.root.CompareTag("Player"))
            {
                Debug.DrawRay(origin, direction, Color.green);
                GFX.ForEach(b => b.enabled = true);
            }
            else
            {
                Debug.DrawRay(origin, direction, Color.red);
                GFX.ForEach(b => b.enabled = false);
            }
        }
    }
}
