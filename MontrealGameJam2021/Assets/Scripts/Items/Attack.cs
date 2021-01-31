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
        GameManager._Instance.SetTeacherSpellColor(Color.gray);
        yield return new WaitForSeconds(12);
        canUsePower = true;
        GameManager._Instance.SetTeacherSpellColor(Color.white);
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
        if (Input.GetKey(KeyCode.R) && canUsePower){
            canUsePower = false;
            Collider[] collider = Physics.OverlapSphere(this.transform.position, 4f);
            string[] alreadyCheck = new string[0];
            bool hasHitPlayer = false;

            foreach (var hit in collider)
            {
                string find = alreadyCheck.FirstOrDefault(element => element == hit.gameObject.name);
                if(hit.gameObject.name.Length > 7){
                    if(hit.gameObject.name.Substring(0,7) == "Student" && hit.gameObject.name != find && hit.gameObject.name != "StudentBot(Clone)")
                    {
                        string[] temp = alreadyCheck;
                        alreadyCheck = new string[(temp.Length + 1)];
                        for(int i = 0; i < temp.Length; i++)
                            alreadyCheck[i] = temp[i];
                        alreadyCheck[(alreadyCheck.Length - 1)] = hit.gameObject.name;     
    
    
                        PlayerMovement player = hit.gameObject.GetComponent<PlayerMovement>();
                        Debug.LogWarning(hit.gameObject.name);
                        hasHitPlayer = true;
                        Debug.LogError(player.gameObject.name);
                        Debug.LogError(this.detentionSpawn.position);
                        Debug.LogError(this.detentionSpawnExit.position);

                        player.BroadcastMovementState(player.gameObject.name, this.detentionSpawn.position, this.detentionSpawnExit.position);
                    }
                }
            }
            if(!hasHitPlayer)
                StartCoroutine(MovingPlayer(8));

            StartCoroutine(bufferAttack());
        }
    }

}
