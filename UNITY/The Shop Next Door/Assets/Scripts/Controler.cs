using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class Controler : NetworkBehaviour
{
    Vector2 _movement;
    Transform _playerTransform;

    float _speed = 10f;
    float _rotSpeed = 270;
    void Start()
    {
        _playerTransform = transform;

        if (IsOwner)
        {
            GetComponent<PlayerInput>().enabled = true;
        }
    }

    void FixedUpdate()
    {
        if (IsServer)
        {
            _playerTransform.Rotate(Vector3.up * (_movement.x * _rotSpeed * Time.fixedDeltaTime));
            _playerTransform.Translate(Vector3.forward * (_movement.y * _speed * Time.fixedDeltaTime));
        }
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

    [ServerRpc]
    public void OnMoveServerRpc(Vector2 input)
    {
        _movement = input;
    }
}