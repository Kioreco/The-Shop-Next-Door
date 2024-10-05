using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class Controler : NetworkBehaviour
{
    Vector2 _movement;
    Transform _playerTransform;
    private NetworkVariable<Quaternion> _rotation = new NetworkVariable<Quaternion>();
    public NavMeshAgent _agent;

    [Header("Camera Movement")]
    GameObject camController;
    float moveSpeed = 2f;
    Vector3 lastPosition;
    bool isDrag = false;
    Vector3 moveDir;

    [Header("Camera Movement Limits")]
    public float minX = -10f;  
    public float maxX = 10f;
    public float minZ = -10f;
    public float maxZ = 10f;

    [Header("Camera Zoom")]
    int fovSinZoom = 60;
    int fovZoom = 25;
    float amountZoom;
    float zoomSpeed = 5f;


    void Start()
    {
        _playerTransform = transform;

        if (IsOwner)
        {
            GetComponent<PlayerInput>().enabled = true;
            GetComponent<NavMeshAgent>().enabled = true;
            camController = GameObject.FindWithTag("CameraController");
            amountZoom = fovSinZoom;
        }

        _rotation.OnValueChanged += OnRotationChanged;
    }

    private void Update()
    {
        if (IsOwner && Input.GetMouseButtonDown(0))
        {
            Ray mouse = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(mouse, out var hit))
            {
                //Llamada a la funci�n rpc pero la nueva creada para el click del rat�n
                SetDestinationServerRpc(hit.point);
            }
        }

        if (IsOwner && Input.GetMouseButtonDown(1))
        {
            isDrag = true;
            lastPosition = Input.mousePosition;
        }

        if (IsOwner && Input.GetMouseButtonUp(1))
        {
            isDrag = false;
        }

        if(isDrag)
        {
            Vector3 movement = Input.mousePosition - lastPosition;
            print(movement);

            float speed = 2f;

            moveDir.x = movement.x * speed;
            moveDir.z = movement.y * speed;

            lastPosition = Input.mousePosition;

            //mov horizontal
            Vector3 auxH = camController.transform.right * -moveDir.x;
            camController.transform.position += auxH * moveSpeed * Time.deltaTime;

            //mov vertical
            Vector3 auxV = new Vector3(0, 0, -moveDir.z); 
            camController.transform.position += auxV * moveSpeed * Time.deltaTime; 

            Vector3 newPosition = camController.transform.position;

            //El clamp para limitar el mov camara
            newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
            newPosition.z = Mathf.Clamp(newPosition.z, minZ, maxZ);

            camController.transform.position = newPosition;
        }

        if (IsOwner && Input.mouseScrollDelta.y > 0)
        {
            //Camera.main.fieldOfView = fovZoom;
            amountZoom -= 5;
            amountZoom = Mathf.Clamp(amountZoom, fovZoom, fovSinZoom);
        }

        if (IsOwner && Input.mouseScrollDelta.y < 0)
        {
            //Camera.main.fieldOfView = fovSinZoom;
            amountZoom += 5;
            amountZoom = Mathf.Clamp(amountZoom, fovZoom, fovSinZoom);
        }

        if(IsOwner) Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, amountZoom, Time.deltaTime * zoomSpeed);

    }

    void FixedUpdate()
    {
        //if (IsServer && Input.GetMouseButtonDown(0))
        //{
        //    _playerTransform.Rotate(Vector3.up * (_movement.x * _rotSpeed * Time.fixedDeltaTime));
        //    _playerTransform.Translate(Vector3.forward * (_movement.y * _speed * Time.fixedDeltaTime));
        //    _rotation.Value = _playerTransform.rotation;
        //}
        //if (IsOwner && Input.GetMouseButtonDown(0))
        
    }

    private void OnRotationChanged(Quaternion previousValue, Quaternion newValue)
    {
        _playerTransform.rotation = newValue;
    }

    private void OnDestroy()
    {
        _rotation.OnValueChanged -= OnRotationChanged;
    }

    #region Input

    public void OnMove(InputAction.CallbackContext context)
    {
        OnMoveServerRpc(context.ReadValue<Vector2>());
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        print("�PIUM, PIUM , CHIUUUUUUUUM!");
    }
    [ServerRpc]
    public void OnMoveServerRpc(Vector2 input)
    {
        _movement = input;
    }
    #endregion

    //Rpc igual que para el mov que hac�amos con la teclas pero para el rat�n
    [ServerRpc]
    public void SetDestinationServerRpc(Vector3 destination)
    {
        _agent.SetDestination(destination);
        //float angle = Mathf.Atan2(destination.y - _playerTransform.position.y, destination.x - _playerTransform.position.x) * Mathf.Rad2Deg;
        //_playerTransform.rotation = Quaternion.Euler(0, 0, angle);
        _playerTransform.LookAt(destination);

        _rotation.Value = _playerTransform.rotation;

    }


}