using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.UIElements;

public class Controler : NetworkBehaviour
{
    Vector2 _movement;
    Transform _playerTransform;
    private NetworkVariable<Quaternion> _rotation = new NetworkVariable<Quaternion>();


    public NavMeshAgent _agent;

    [Header("CameraMovement")]
    GameObject camController;
    float moveSpeed;
    float moveTime;
    Vector3 lastPosition;
    bool isDrag = false;
    Vector3 moveDir;

    [Header("Camera Movement Limits")]
    public float minX = -10f;  
    public float maxX = 10f;
    public float minZ = -10f;
    public float maxZ = 10f;


    void Start()
    {
        _playerTransform = transform;

        if (IsOwner)
        {
            GetComponent<PlayerInput>().enabled = true;
            GetComponent<NavMeshAgent>().enabled = true;
            camController = GameObject.FindWithTag("CameraController");
            //newPosition = camController.transform.position;
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
                //Llamada a la función rpc pero la nueva creada para el click del ratón
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

        //print(isDrag);

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
            //Vector3 aux = camController.transform.forward*moveDir.z + camController.transform.right *-1f * moveDir.y + camController.transform.right * -moveDir.x;
            camController.transform.position += auxH * 2f * Time.deltaTime;

            //mov vertical
            Vector3 auxV = new Vector3(0, 0, -moveDir.z); 
            camController.transform.position += auxV * 2f * Time.deltaTime; 

            Vector3 newPosition = camController.transform.position;

            //El clamp para limitar el mov camara
            newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
            newPosition.z = Mathf.Clamp(newPosition.z, minZ, maxZ);

            camController.transform.position = newPosition;
        }

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
        print("¡PIUM, PIUM , CHIUUUUUUUUM!");
    }
    [ServerRpc]
    public void OnMoveServerRpc(Vector2 input)
    {
        _movement = input;
    }
    #endregion

    //Rpc igual que para el mov que hacíamos con la teclas pero para el ratón
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