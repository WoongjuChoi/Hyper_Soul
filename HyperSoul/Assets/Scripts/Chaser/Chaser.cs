using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Chaser : MonoBehaviour
{
    [SerializeField]
    private bool _active = false;

    private Transform _target = null;

    private NavMeshAgent _agent = null;

    public Transform IsTarget { get { return _target; } set { _target = value; } }
    public bool IsActive { get { return _active; } set { _active = value; } }

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (_active && null != _target)
        {
            _agent.SetDestination(_target.position);
        }
    }

    public void ResetPath()
    {
        _agent.ResetPath();
    }
}
