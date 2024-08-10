using System.Collections;
using System.Collections.Generic;
using SFramework.Game;
using UnityEngine;

public class NpcEntity : RootEntity
{
    [SerializeField]
    private float _startForce = 15f;

    [SerializeField]
    private Rigidbody2D _rb;
    [SerializeField]
    private SpriteRenderer _spriteRender;

    public override void DestroyEntity() { }

    public override void Recycle() { }

    public override void Show() 
    {
        if(_rb == null)
            _rb = GetComponent<Rigidbody2D>();
        _rb.AddForce(transform.up * _startForce, ForceMode2D.Impulse);
    }
}
