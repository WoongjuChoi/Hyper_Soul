using UnityEngine;
using System.Collections;

public class Human : MonoBehaviour {


	public float moveForce = 365f;
	public float maxSpeed = 1f;
    public GameObject explosionPrefab;
    public float minStateTime = 1;
    private float lastStateChangeTime;
	private int _direction = 1;
	private SpriteRenderer _spriteRenderer;


	private Animator _anim;
	private Rigidbody2D _rb2d;
    enum State { Walking, Idle, Dead };
    State state = State.Walking;

    private void OnEnable()
    {
        //reset all values
        state = State.Walking;
        _rb2d.velocity = Vector3.zero;
        lastStateChangeTime = Time.time;
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        Color color = _spriteRenderer.color;
        color.r = Random.value * .4f + .6f;
        color.g = Random.value * .4f + .6f;
        color.b = Random.value * .4f + .6f;
        _spriteRenderer.color = color;



    }

    void Awake () 
	{
        _anim = GetComponent<Animator>();
        
        _rb2d = GetComponent<Rigidbody2D>();
 
    }
    void OnMouseDown()
    {
     if (!Globotix.SimpleScope2D.ScopeVisible)
        {
            //don't kill if scope is hidden
          return;
        }
        if (state == State.Dead) return;
        StartCoroutine(FadeCoroutine());
        Vector3 impactPoint = Input.mousePosition;
        //if your scene has more than one camera, you may need change this to be the appropriate camera for mouseInput
        impactPoint.z = Mathf.Abs(Camera.main.transform.position.z);
        impactPoint = Camera.main.ScreenToWorldPoint(impactPoint);
        impactPoint.z = 0;
      
       GameObject blood =  Instantiate(explosionPrefab, impactPoint, Quaternion.identity) as GameObject;
        Destroy(blood, 3);

    }
    void Update()
    {
        if (state == State.Walking)
        {
            //keep adding force if not at max speed
            if (_direction * _rb2d.velocity.x < maxSpeed)
                _rb2d.AddForce(Vector2.right * _direction * moveForce);

            if (Mathf.Abs(_rb2d.velocity.x) > maxSpeed)
                _rb2d.velocity = new Vector2(Mathf.Sign(_rb2d.velocity.x) * maxSpeed, _rb2d.velocity.y);
        }

        if (gameObject.transform.position.x < -10 && _direction == -1)
        {
            //off screen left
            FlipRight();
        }
        if (gameObject.transform.position.x > 10 && _direction == 1)
        {
            //off screen right
            FlipLeft();
        }
        AI();
    }
    void AI()
    {
        if (Time.time - lastStateChangeTime > minStateTime)
        {
            if (Random.value* 300 < 1)
            {
                if (state == State.Walking)
                {
                    //stop
                    state = State.Idle;
                    _anim.SetBool("Standing", true);
                    _rb2d.velocity = Vector3.zero;
                }
                else if (state == State.Idle)
                {
                    //walk
                    state = State.Walking;
                    _anim.SetBool("Standing", false);
                    if (Random.value < .5f)
                    {
                        FlipLeft();
                    }
                    else
                    {
                        FlipRight();
                    }
                }
                lastStateChangeTime = Time.time;
            }

        }

    }

 IEnumerator FadeCoroutine (){
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        float delay = 2f;
        float duration = 1f;
        //
        _spriteRenderer.sortingOrder = 50;
        yield return new WaitForSeconds(.1f);
        state = State.Dead;
        _anim.SetTrigger("Shot");
     
        yield return new WaitForSeconds(delay - .9f);
        float start = Time.time;
        while (Time.time <= start + duration)
        {
            Color color = renderer.color;
            color.a = 1f - Mathf.Clamp01((Time.time - start) / duration);
            renderer.color = color;
            yield return new WaitForEndOfFrame();
        }
        Destroy(gameObject);
    }




	void FlipLeft()
	{
        _direction = -1;
		_spriteRenderer.flipX = true;

	}

	void FlipRight()
	{
		_direction = 1;
        _spriteRenderer.flipX = false;

	}
		
}