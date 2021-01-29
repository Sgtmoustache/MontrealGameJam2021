using UnityEngine;

namespace DefaultNamespace
{
    public static class PlacerHelper
    {
        public static void GroundObject(Transform gameObject, float thickness)
        {
            RaycastHit hit;

            Vector3 origin = gameObject.position;

            int layerMask = 1 << 8;
            layerMask = ~layerMask;
        
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(origin, Vector3.down, out hit, Mathf.Infinity, layerMask))
            {
                
            }
        }
    }
}