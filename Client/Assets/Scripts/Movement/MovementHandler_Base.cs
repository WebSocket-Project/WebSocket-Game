using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovementHandler_Base : MonoBehaviour
{
    protected float _speed;
    protected Vector3 _direction;

    /// <summary>
    /// 움직임 지정
    /// </summary>
    public virtual void SetMovement(Vector3 direction, float speed)
    {
        _direction = direction.normalized;
        _speed = speed;
    }

    /// <summary>
    /// 움직임 동작
    /// </summary>
    public abstract void Move();

    /// <summary>
    /// 움직임 즉시 멈춤
    /// </summary>
    public abstract void StopImmediately();
}