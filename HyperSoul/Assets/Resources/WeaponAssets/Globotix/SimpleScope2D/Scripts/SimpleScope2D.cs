using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// SimpleScope2D v1.0

namespace Globotix
{
    public class SimpleScope2D : MonoBehaviour
    {

        public float zoomedInRadius = 1;        //Othographic size    
        public float zoomedOutRadius =5;        //Othographic size   
        public float initialZoom = 2;           //Othographic size   
        public float scale = 1;                 //scale scope size
        public float maxSpeed = 30f;
        public float movementDampening = .2f;
        public bool hideOnStart = true;
        public bool holdRMBToShow = true;
        public bool moveWithMouse = true;
        public bool zoomWithWheel = true;
        public bool invertWheel = false;
        public float zoomWheelIncrement = .25f;
        public float zoomSpeed = 2f;
        public float zoomDampening = .2f;
        public bool hideCursor = true;
        private Vector3 _scopeDestination;
        private Camera _scopeCamera;
        private GameObject _scopeUIObject;
        private float _currentZoom, _desiredZoom, _zoomVelocityRef;
        private Vector2 _zoomRenderSize, _scopeGraphicSize;
        private Vector3 _movementVelocityRef = Vector3.zero;
        private AudioSource _shootSoundAudioSource;
        private float _screenPPU;
        private static bool _scopeVisible = true;
        public static bool ScopeVisible { get { return _scopeVisible; } }
        private void Start()
        {
            InitScope();
            if (hideOnStart)
                HideScope();
            else
                ShowScope();
        }

        void InitScope()
        {

            _shootSoundAudioSource = GetComponent<AudioSource>();

            _screenPPU = Screen.height / (Camera.main.orthographicSize * 2);
            _scopeCamera = gameObject.transform.Find("ScopeCamera").GetComponent<Camera>();
            _scopeUIObject = gameObject.transform.Find("ScopeCanvas/ScopeObject").gameObject;

            GameObject mask = gameObject.transform.Find("ScopeCanvas/ScopeObject/MaskForScope").gameObject;
            GameObject zoomedRender = gameObject.transform.Find("ScopeCanvas/ScopeObject/MaskForScope/ZoomedRender").gameObject;
            GameObject scopeGraphic = gameObject.transform.Find("ScopeCanvas/ScopeObject/ScopeImage").gameObject;
            //size 
            _zoomRenderSize = zoomedRender.GetComponent<RectTransform>().sizeDelta;
            _scopeGraphicSize = scopeGraphic.GetComponent<RectTransform>().sizeDelta;
            zoomedRender.GetComponent<RectTransform>().sizeDelta = _zoomRenderSize * scale;
            mask.GetComponent<RectTransform>().sizeDelta = _zoomRenderSize * scale;
            scopeGraphic.GetComponent<RectTransform>().sizeDelta = _scopeGraphicSize * scale;
            
            Zoom = initialZoom;
            MoveScopeToWorldCoordinates(_scopeCamera.transform.position);
        }


        void Update()
        {
            if (!_scopeCamera) return;

            if (moveWithMouse)
            {
                _scopeDestination = Input.mousePosition;
            }
            //move scope;

            Vector3 scopePosition = Vector3.SmoothDamp(_scopeUIObject.transform.position, _scopeDestination, ref _movementVelocityRef, movementDampening, maxSpeed * _screenPPU);
            PositionScope(scopePosition);

            if (zoomWithWheel)
            {
                float scrollChange = Input.GetAxis("Mouse ScrollWheel");
                if (!invertWheel) scrollChange = -scrollChange;
                scrollChange = normalizeFloat(scrollChange);
                _desiredZoom += scrollChange * zoomWheelIncrement;
                ZoomTo(_desiredZoom);//clamps it
            }

            //adjust zoom
            if (_desiredZoom != _currentZoom)
            {
                _currentZoom = Mathf.SmoothDamp(_currentZoom, _desiredZoom, ref _zoomVelocityRef, zoomDampening);
                showCurrentZoom();
            }
            if (holdRMBToShow)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    MoveScopeTo(Input.mousePosition);
                    ShowScope();
                }
                if (Input.GetMouseButtonUp(1))
                {
                    HideScope();
                }
            }
            //play audio on click if audio source attached
            if (Input.GetMouseButtonDown(0))
            {
                if (_shootSoundAudioSource != null)
                {
                    if (_scopeVisible)
                    {
                        _shootSoundAudioSource.Play();
                    }
                }
            }
        }

        public void ShowScope()
        {
            _scopeUIObject.SetActive(true);
            _scopeVisible = true;
            if (hideCursor) Cursor.visible = false;
        }
        public void HideScope()
        {
            _scopeUIObject.SetActive(false);
            _scopeVisible = false;
            if (hideCursor) Cursor.visible = true;
        }
        public void SetScopeDestination(Vector2 screenCoordinates)
        {
            //scope will move to scope desitination based on its speed and dampening
            _scopeDestination = screenCoordinates;
        }
        public void SetScopeDestinationWorldUnits(Vector2 worldCoordinates)
        {
            //scope will move to scope desitination based on its speed and dampening
            _scopeDestination = Camera.main.WorldToScreenPoint(worldCoordinates);
        }
        public void MoveScopeTo(Vector2 screenCoordinates)
        {
            //will imediately move scope to position
            _scopeDestination = screenCoordinates;
            PositionScope(screenCoordinates);

        }
        public void MoveScopeToWorldCoordinates(Vector2 worldCoordinates)
        {
            //will imediately move scope to position
            _scopeDestination = Camera.main.WorldToScreenPoint(worldCoordinates);
            PositionScope(_scopeDestination);
        }
        public void ZoomTo(float desiredZoom)
        {
            //scope will zoom to this based on zoom speed and dampening
            _desiredZoom = Mathf.Clamp(desiredZoom, zoomedInRadius, zoomedOutRadius);
    

        }
        public float Zoom
        {
            //wil immediately set zoom level
            get{
                return _currentZoom; }
            set {
                _currentZoom = Mathf.Clamp(value, zoomedInRadius, zoomedOutRadius);
                _desiredZoom = _currentZoom;
                showCurrentZoom();
            }
        }
        public void SetScale(float myScale)
        {
            scale = myScale;
            GameObject mask = gameObject.transform.Find("ScopeCanvas/ScopeObject/MaskForScope").gameObject;
            GameObject zoomedRender = gameObject.transform.Find("ScopeCanvas/ScopeObject/MaskForScope/ZoomedRender").gameObject;
            GameObject scopeGraphic = gameObject.transform.Find("ScopeCanvas/ScopeObject/ScopeImage").gameObject;

            zoomedRender.GetComponent<RectTransform>().sizeDelta = _zoomRenderSize * scale;
            mask.GetComponent<RectTransform>().sizeDelta = _zoomRenderSize * scale;
            scopeGraphic.GetComponent<RectTransform>().sizeDelta = _scopeGraphicSize * scale;
        }
        private void showCurrentZoom()
        {
            //for internal Use Only
            _currentZoom = Mathf.Clamp(_currentZoom, zoomedInRadius, zoomedOutRadius);

            _scopeCamera.orthographicSize = _currentZoom;
        }
        private void OnValidate()
        {
            if (Application.isPlaying && Application.isEditor)
            {
               // SetScale(scale);
            }
        }
        private void PositionScope(Vector2 screenCoordinates)
        {
            Vector3 newScopePosition = screenCoordinates;
            //needs depth to convert to world point
            newScopePosition.z = 10;
            Vector3 cameraDest = Camera.main.ScreenToWorldPoint(newScopePosition);
            //keep camera's z
            cameraDest.z = _scopeCamera.transform.position.z;
            _scopeCamera.transform.position = cameraDest;
            _scopeUIObject.transform.position = newScopePosition;
        }
        private void PositionScopeWorldCoordinates(Vector3 worldCoordinates) { 

            Vector3 cameraDest = worldCoordinates;
            //keep camera's z
            cameraDest.z = _scopeCamera.transform.position.z;
            _scopeCamera.transform.position = cameraDest;
            Vector3 newScopePosition = Camera.main.WorldToScreenPoint(cameraDest);
            _scopeUIObject.transform.position = newScopePosition;

        }
     
         float normalizeFloat(float num)
        {
            if (num > 0) num = 1;
            if (num < 0) num = -1;
            return num;
        }



   
    }
}
