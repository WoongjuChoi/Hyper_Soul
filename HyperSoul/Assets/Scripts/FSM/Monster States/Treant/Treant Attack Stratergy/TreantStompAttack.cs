using UnityEngine;

public class TreantStompAttack : MonoBehaviour, ITreantAttack
{
    private bool _isAttack = false;

    private float _elapsedTime = 0f;

    private const float ATTACK_DELAY_TIME = 3f;

    public void Attack()
    {
        _elapsedTime += Time.deltaTime;

        if (_elapsedTime <= ATTACK_DELAY_TIME)
        {
            _isAttack = false;
        }
        else
        {
            _isAttack = true;

            _elapsedTime = 0;
        }

        gameObject.GetComponentInChildren<Animator>().SetBool(MonsterAnimatorID.IS_TREANT_ROOT_ATTACK, false);

        gameObject.GetComponentInChildren<Animator>().SetBool(MonsterAnimatorID.IS_TREANT_STOMP_ATTACK, _isAttack);

        gameObject.GetComponent<TreantInformation>().StompAttackArea.SetActive(_isAttack);
    }
}
