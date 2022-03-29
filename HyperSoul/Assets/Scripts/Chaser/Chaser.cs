using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Chaser : MonoBehaviour
{
    NavMeshAgent Agent;

    [SerializeField]
    private Transform _target;
    [SerializeField]
    private bool _active = false;

    public Transform IsTarget { get { return _target; } set { _target = value; } }
    public bool IsActive { get { return _active; } set { _active = value; } }

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (_active == true && _target != null)
        {
            Agent.SetDestination(_target.position);
        }
    }

}
