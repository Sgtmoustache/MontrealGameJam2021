using System.Collections;
using System.Linq;
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

        [SerializeField]
        private GameObject _debugtarget;

        private bool hasDestination = false;

        public Bounds _bounds;
        
        public void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
        }

        private IEnumerator Walk()
        {
            int time = Random.Range(2, 8);
            yield return new WaitForSeconds(time);
            hasDestination = false;
        }

        public void Update()
        {
            if (PhotonNetwork.IsConnected && !PhotonNetwork.IsMasterClient)
                return;
            
            if (!hasDestination)
            {
                _agent.SetDestination (new Vector3(
                    Random.Range(_bounds.min.x, _bounds.max.x),
                    Random.Range(_bounds.min.y, _bounds.max.y),
                    Random.Range(_bounds.min.z, _bounds.max.z)
                ));
                
                //Allows to see target position
                if(_debugtarget != null)
                    _debugtarget.transform.position = _agent.destination;
                
                StartCoroutine(Walk());
                hasDestination = true;
            }
            
            if(_agent.velocity.magnitude >= 1)
            _animator.SetInteger("Movement", 1);
            else
             _animator.SetInteger("Movement", 0);
        }
    }
}