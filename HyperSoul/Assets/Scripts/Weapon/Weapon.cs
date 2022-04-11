using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    protected PlayerInfo _playerInfo;
    protected Animator _playerAnimator;
    protected PlayerCam _playerCam;
    protected PlayerInputs _input;
    protected AudioSource _audioSource;
    protected EGunState _gunState;

    DataManager _dataManager;
    private void Awake()
    {
        _playerInfo = GetComponentInParent<PlayerInfo>();
        _playerAnimator = Player.GetComponentInChildren<Animator>();
        _playerCam = Player.GetComponent<PlayerCam>();
        _input = Player.GetComponent<PlayerInputs>();
        _audioSource = this.gameObject.GetComponent<AudioSource>();
        _dataManager = GameObject.Find("DataManager").GetComponent<DataManager>();

        _playerAnimator.SetFloat(PlayerAnimatorID.RELOAD_SPEED, _reloadSpeed);
        ZoomRotationSpeed = new Vector2(0.2f, 0.2f);
        ZoomCam.SetActive(false);
        //MaxBulletAmt = _dataManager.
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

        if(CurBulletCnt <= 0)
        {
            _gunState = EGunState.Empty;
        }
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
        Debug.DrawRay(ray.origin, ray.direction * 200, Color.red);
        RaycastHit[] raycastHits = Physics.RaycastAll(ray).OrderBy(h => h.distance).ToArray();

        for (int i = 0; i < raycastHits.Length; ++i)
        {
            RaycastHit hit = raycastHits[i];

            if (hit.distance > 4)
            {
                _mousePos = hit.point;
                break;
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
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
}
