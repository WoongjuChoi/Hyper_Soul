using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviourPun, IPunObservable
{
    public Vector2 ZoomRotationSpeed;
    public GameObject ZoomCam;
    public GameObject Player;
    public GameObject MuzzleFlashEffect;
    public AudioClip ShotSound;
    public AudioClip ReloadSound;

    public int CurBulletCnt = 0;
    public int MaxBulletAmt = 0;

    protected float _reloadTime = 0;
    protected float _reloadSpeed = 1;
    protected bool _canFire = true;

    protected Vector3 _mousePos;
    protected Animator _playerAnimator;
    protected PlayerCam _playerCam;
    protected PlayerInputs _input;
    protected AudioSource _audioSource;
    protected EGunState _gunState;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(CurBulletCnt);
            stream.SendNext(_gunState);
        }
        else
        {
            CurBulletCnt = (int)stream.ReceiveNext();
            _gunState = (EGunState)stream.ReceiveNext();
        }
    }


    [PunRPC]
    private void Awake()
    {
        _playerAnimator = Player.GetComponentInChildren<Animator>();
        _playerCam = Player.GetComponent<PlayerCam>();
        _input = Player.GetComponent<PlayerInputs>();
        _audioSource = this.gameObject.GetComponent<AudioSource>();

        _playerAnimator.SetFloat(PlayerAnimatorID.RELOAD_SPEED, _reloadSpeed);
        ZoomRotationSpeed = new Vector2(0.2f, 0.2f);
        ZoomCam.SetActive(false);
    }
    protected void Update()
    {
        if (_input.IsZoom)
        {
            Zoom();
        }
        else
        {
            ZoomCam.SetActive(false);
        }

        SetMousePos();
    }
    public virtual void Fire() { }
    public virtual void Zoom() { }

    public bool HasReloaded()
    {
        if (_gunState == EGunState.Reloading || CurBulletCnt >= MaxBulletAmt)
        {
            return false;
        }
        Debug.Log(_reloadTime);
        StartCoroutine(Reload());
        return true;
    }
    private IEnumerator Reload()
    {
        _gunState = EGunState.Reloading;
        _audioSource.clip = ReloadSound;
        _audioSource.Play();

        yield return new WaitForSeconds(_reloadTime);

        CurBulletCnt = MaxBulletAmt;

        _gunState = EGunState.Ready;
    }

    public void SetMousePos()
    {
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f))
        {
            _mousePos = raycastHit.point;
        }
    }
}
