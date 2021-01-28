using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviourPun
{
    public Transform Camera;
    [SerializeField] private float WalkingSpeed = 6f;
    [SerializeField] private float TurnSmoothTime = 0.1f;
    
    public Animator anim;

    private CharacterController _controller;
    private float _turnSmoothVelocity;

    public PlayerMovement(Transform camera)
    {
        Camera = camera;
    }

    void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (PhotonNetwork.IsConnected && photonView.IsMine == false)
        {
            return;
        }
        
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
            _controller.Move(moveDirection.normalized * WalkingSpeed * Time.deltaTime);
             anim.SetBool("isRunning", true);
        }
        else 
        {
            anim.SetBool("isRunning", false);
        }
    }
}
