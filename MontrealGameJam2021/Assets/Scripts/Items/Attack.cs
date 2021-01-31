using System.Linq.Expressions;
using System.Collections;
using System.Linq;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
public class Attack : MonoBehaviour
{
    public bool canUsePower = true;

    [SerializeField] public Transform detentionSpawn;
    [SerializeField] public Transform detentionSpawnExit;

    public IEnumerator bufferAttack(){
        canUsePower = false;
        yield return new WaitForSeconds(10);
        canUsePower = true;
    }

    public IEnumerator MovingPlayer(int timer){
        PlayerMovement movement = PlayerSpawner.LocalPlayer.GetComponent<PlayerMovement>();

        movement.setMovement(false);
        FadeManager._Instance.FadeOut();
        yield return new WaitForSeconds(2);
        FadeManager._Instance.FadeIn();
        PlayerSpawner.LocalPlayer.transform.position = detentionSpawn.position;
        movement.setMovement(true);

        yield return new WaitForSeconds(timer);

        movement.setMovement(false);
        FadeManager._Instance.FadeOut();
        yield return new WaitForSeconds(2);
        FadeManager._Instance.FadeIn();
        PlayerSpawner.LocalPlayer.transform.position = detentionSpawnExit.position;
        movement.setMovement(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && canUsePower){
            Collider[] collider = Physics.OverlapSphere(this.transform.position, 4f);
            string[] alreadyCheck = new string[0];
            bool hasHitPlayer = false;
            foreach (var hit in collider)
            {
                if(hit.gameObject.name.Substring(0,7) == "Student" && !(alreadyCheck.Any(element => element == hit.gameObject.name)))
                {
                    string[] temp = alreadyCheck;
                    alreadyCheck = new string[temp.Length + 1];
                    for(int i = 0; i < temp.Length; i++)
                        alreadyCheck[i] = temp[i];
                    alreadyCheck[alreadyCheck.Length - 1] = hit.gameObject.name;     

                    PlayerMovement player = hit.gameObject.GetComponent<PlayerMovement>();
                    Debug.LogWarning(hit.gameObject.name);
                    hasHitPlayer = true;
                    player.BroadcastMovementState(player.gameObject.name, this.detentionSpawn.position, this.detentionSpawnExit.position);
                }
            }
            if(!hasHitPlayer)
                StartCoroutine(MovingPlayer(8));

            StartCoroutine(bufferAttack());
        }
    }

}
