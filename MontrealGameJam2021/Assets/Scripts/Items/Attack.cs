using System.Collections.ObjectModel;
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
        yield return new WaitForSeconds(20);
        canUsePower = true;
        GameManager._Instance.SetTeacherSpellColor(Color.white);
    }

    public IEnumerator MovingPlayer(int timer){
        PlayerMovement movement = PlayerSpawner.LocalPlayer.GetComponent<PlayerMovement>();
        Inventory inventory = this.gameObject.GetComponent<Inventory>();
        var current = GameManager._Instance.CurrentRound;
        if(inventory){
            GameObject obj = inventory.GetItemGameObject();
            if(obj)
            {
                inventory.ClearItem();
                Vector3 vec = this.gameObject.transform.localPosition;
                Collecting collect = obj.GetComponent<Collecting>();
                collect.beInteractable();
                obj.transform.SetParent(null);
                obj.transform.localPosition = new Vector3(vec.x, (vec.y + 4f), vec.z);
            }
        }
        

        movement.setMovement(false);
        FadeManager._Instance.FadeOut();
        yield return new WaitForSeconds(2);
        FadeManager._Instance.FadeIn();
        PlayerSpawner.LocalPlayer.transform.position = detentionSpawn.position;
        movement.setMovement(true);

        yield return new WaitForSeconds(timer);
        if(GameManager._Instance.CurrentRound == current)
        {
            movement.setMovement(false);
            FadeManager._Instance.FadeOut();
            yield return new WaitForSeconds(2);
            FadeManager._Instance.FadeIn();
            PlayerSpawner.LocalPlayer.transform.position = detentionSpawnExit.position;
            movement.setMovement(true);
        }
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

                        player.BroadcastMovementState(player.gameObject.name, this.detentionSpawn.position, this.detentionSpawnExit.position);
                    }
                }
            }
            if(!hasHitPlayer)
                StartCoroutine(MovingPlayer(4));

            StartCoroutine(bufferAttack());
        }
    }

}
