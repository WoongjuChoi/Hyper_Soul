using UnityEngine;

public class TreantStompAttack : ITreantAttack
{
    private bool _isAttack = false;

    private float _elapsedTime = 0f;

    private const float ATTACK_TIME = 1f;
    private const float ATTACK_DELAY_TIME = 5f;

    public void Attack(GameObject obj)
    {
        _elapsedTime += Time.deltaTime;

        obj.GetComponentInChildren<Animator>().SetBool(MonsterAnimatorID.IS_TREANT_ROOT_ATTACK, false);

        if (_elapsedTime <= ATTACK_DELAY_TIME)
        {
            _isAttack = false;
        }
        else
        {
            _isAttack = true;

            _elapsedTime = 0;
        }

        obj.GetComponentInChildren<Animator>().SetBool(MonsterAnimatorID.IS_TREANT_STOMP_ATTACK, _isAttack);

        obj.GetComponent<TreantInformation>().StompAttackArea.SetActive(_isAttack);
    }
}
