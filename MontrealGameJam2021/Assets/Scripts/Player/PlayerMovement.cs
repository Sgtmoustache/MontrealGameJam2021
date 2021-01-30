using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviourPun
{
    public Transform Camera;
    [SerializeField] private float WalkingSpeed = 3.0f;
    [SerializeField] private float RunningSpeed = 5.0f;
    [SerializeField] private float TurnSmoothTime = 0.1f;
    [SerializeField] private float GravityForce = -0.4f;
    private Animator _anim;
    
    private CharacterController _controller;
    private float _turnSmoothVelocity;

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
            _controller.Move(moveDirection.normalized * (Input.GetKey(KeyCode.LeftShift)? RunningSpeed : WalkingSpeed ) * Time.deltaTime + new Vector3(0, GravityForce, 0));
             _anim.SetBool("isRunning", true);
        }
        else 
        {
            _controller.Move(new Vector3(0, GravityForce, 0));
            _anim.SetBool("isRunning", false);
        }
    }
}
