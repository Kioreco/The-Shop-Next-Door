using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class Controler : NetworkBehaviour
{
    Vector2 _movement;
    Transform _playerTransform;

    float _speed = 10f;
    float _rotSpeed = 270;
    public NavMeshAgent _agent;

    Vector3 cameraMovement;
    Vector3 origin;

    private NetworkVariable<Quaternion> _rotation = new NetworkVariable<Quaternion>();

    void Start()
    {
        _playerTransform = transform;

        if (IsOwner)
        {
            GetComponent<PlayerInput>().enabled = true;
            GetComponent<NavMeshAgent>().enabled = true;
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


        if (IsOwner && Input.GetMouseButton(1))
        {
            cameraMovement = (Camera.main.ScreenToWorldPoint(Input.mousePosition)) - Camera.main.transform.position;
            
            Camera.main.transform.position = origin - cameraMovement;
        }
        //print(Input.GetMouseButtonDown(1));
    }

    void FixedUpdate()
    {
        if (IsServer && Input.GetMouseButtonDown(0))
        {
            _playerTransform.Rotate(Vector3.up * (_movement.x * _rotSpeed * Time.fixedDeltaTime));
            _playerTransform.Translate(Vector3.forward * (_movement.y * _speed * Time.fixedDeltaTime));
            _rotation.Value = _playerTransform.rotation;
        }
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

    #endregion

    //Rpc igual que para el mov que hacíamos con la teclas pero para el ratón
    [ServerRpc]
    public void SetDestinationServerRpc(Vector3 destination)
    {
        _agent.SetDestination(destination);
    }

    [ServerRpc]
    public void OnMoveServerRpc(Vector2 input)
    {
        _movement = input;
    }
}