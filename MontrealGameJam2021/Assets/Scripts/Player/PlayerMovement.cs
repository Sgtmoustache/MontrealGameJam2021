using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviourPun
{
    public Transform Camera;

    [SerializeField] Transform detentionSpawn;
    [SerializeField] Transform detentionSpawnExit;
    [SerializeField] private float WalkingSpeed = 3.0f;
    [SerializeField] private float RunningSpeed = 5.0f;
    [SerializeField] private float TurnSmoothTime = 0.1f;
    [SerializeField] private float GravityForce = -0.4f;
    private Animator _anim;
    
    private CharacterController _controller;
    private float _turnSmoothVelocity;
    private float timer = 0.0f;
    private bool CanMove = true;

    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (PhotonNetwork.IsConnected && !photonView.IsMine)
        {
            return;
        }

        Inventory inventory = this.gameObject.GetComponent<Inventory>();
        if(inventory.HasItem()){
            if (Input.GetKeyDown(KeyCode.Q)){   
                _anim.SetInteger("Movement", 3);
                _anim.SetBool("isDropping", true);
                timer = 0.5f;
                CanMove = false;
            }
        }
        
        if (inventory){
            if (!(inventory.HasItem())){
                if (Input.GetKeyDown(KeyCode.E)){
                    _anim.SetInteger("Movement", 3);
                    _anim.SetBool("isGrabbing", true);
                    timer = 0.7f;
                    CanMove = false;
                }
            }
        }

        ///controles pour le teacher
        /*if (Input.GetKeyDown(KeyCode.Space)){
            _anim.SetInteger("Movement", 3);
            _anim.SetBool("IsAttacking", true);
            timer = 0.5f;
            CanMove = false;
        }*/

        if(timer > 0.0f){
            timer -= Time.deltaTime;
        }
        else{
            _anim.SetBool("isGrabbing", false);
            _anim.SetBool("isDropping", false);
            _anim.SetInteger("Movement", 0);
            CanMove = true;
        }


        if (GameManager.PlayersCanMove && CanMove)
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

            //If were getting some input to move
            if (direction.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + Camera.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity,
                    TurnSmoothTime);


                transform.rotation = Quaternion.Euler(0f, angle, 0f);


                Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                _controller.Move(
                    moveDirection.normalized * (Input.GetKey(KeyCode.LeftShift) ? RunningSpeed : WalkingSpeed) *
                    Time.deltaTime + new Vector3(0, GravityForce, 0));
                if(Input.GetKey(KeyCode.LeftShift))  
                    _anim.SetInteger("Movement", 2);
                else 
                    _anim.SetInteger("Movement", 1);     
            }
            else
            {
                _controller.Move(new Vector3(0, GravityForce, 0));
                _anim.SetInteger("Movement", 0);
            }
        }
        else
        {
            _anim.SetInteger("Movement", 0);
            _controller.Move(new Vector3(0, GravityForce, 0));
        }
    }

    public void setMovement(bool mov){
        CanMove = mov;
    }

    public void BroadcastMovementState(string name)
    {
        photonView.RPC("MovePlayer", RpcTarget.Others, name);
    }

[PunRPC]
    public void MovePlayer(string name){
        MovingPlayer(GameObject.Find(name), 30);
    }

    public IEnumerator MovingPlayer(GameObject player, int timer){
        PlayerMovement movement = player.GetComponent<PlayerMovement>();

        movement.setMovement(false);
        FadeManager._Instance.FadeOut();
        yield return new WaitForSeconds(2);
        FadeManager._Instance.FadeIn();
        player.transform.position = detentionSpawn.position;
        movement.setMovement(true);

        yield return new WaitForSeconds(timer);

        movement.setMovement(false);
        FadeManager._Instance.FadeOut();
        yield return new WaitForSeconds(2);
        FadeManager._Instance.FadeIn();
        player.transform.position = detentionSpawnExit.position;
        movement.setMovement(true);
    }


}
