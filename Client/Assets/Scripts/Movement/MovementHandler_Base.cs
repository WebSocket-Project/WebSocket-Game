using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovementHandler_Base : MonoBehaviour
{
    protected float _speed;
    protected Vector3 _direction;

    /// <summary>
    /// ������ ����
    /// </summary>
    public virtual void SetMovement(Vector3 direction, float speed)
    {
        _direction = direction.normalized;
        _speed = speed;
    }

    /// <summary>
    /// ������ ����
    /// </summary>
    public abstract void Move();

    /// <summary>
    /// ������ ��� ����
    /// </summary>
    public abstract void StopImmediately();
}