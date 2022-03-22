using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamplePlayerAttack : MonoBehaviour
{
    private bool _isAttack = false;

    public bool IsAttack { get { return _isAttack; } }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            _isAttack = true;
        }
        else
        {
            _isAttack = false;
        }
    }
}
