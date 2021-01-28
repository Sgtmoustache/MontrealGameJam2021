using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Player
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(NavMeshAgent))]
    public class BotMovement : MonoBehaviourPun
    {
        private NavMeshAgent _agent;
        private Animator _animator;
        
        public GameObject target;

        public void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
        }

        public void Update()
        {
            _agent.SetDestination(target.transform.position);

            _animator.SetBool("isRunning", _agent.velocity.magnitude >= 1);
        }
    }
}