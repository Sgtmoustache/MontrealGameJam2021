using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
public class Attack : MonoBehaviour
{
    public bool canUsePower = true;

    [SerializeField] Transform detentionSpawn;
    [SerializeField] Transform detentionSpawnExit;

    public IEnumerator bufferAttack(){
        canUsePower = false;
        yield return new WaitForSeconds(10);
        canUsePower = true;
    }

    public IEnumerator MovingPlayer(GameObject player, int timer){
        PlayerMovement movement = player.GetComponent<PlayerMovement>();

        movement.setMovement(false);
        FadeManager._Instance.FadeIn();
        yield return new WaitForSeconds(2);
        FadeManager._Instance.FadeOut();
        player.transform.position = detentionSpawn.position;
        movement.setMovement(true);

        yield return new WaitForSeconds(timer);

        movement.setMovement(false);
        FadeManager._Instance.FadeIn();
        yield return new WaitForSeconds(2);
        FadeManager._Instance.FadeOut();
        player.transform.position = detentionSpawnExit.position;
        movement.setMovement(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && canUsePower){
            Collider[] collider = Physics.OverlapSphere(this.transform.position, 4f, LayerMask.NameToLayer("OtherPlayer"));
            bool hasHitPlayer = false;
            
            foreach (var hit in collider)
            {
                PlayerMovement player = hit.gameObject.GetComponent<PlayerMovement>();
                if(player)
                {
                    hasHitPlayer = true;
                    player.BroadcastMovementState(player.gameObject.name);
                }
            }
            if(!hasHitPlayer)
                StartCoroutine(MovingPlayer(this.gameObject, 8));

            StartCoroutine(bufferAttack());
        }
    }

}