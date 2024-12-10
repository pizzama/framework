using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleTargetMove : MonoBehaviour
{
    private ParticleSystem _par;
    private ParticleSystem.Particle[] _arrPar;
    private int _arrCount;
    [SerializeField] private float _speed = 0.1f;
    [SerializeField] private float _delayTime = 1f;
    [SerializeField] private float _originEmissionRate;
    [SerializeField] private bool _isActive;
    [SerializeField] private float _speedAdd;
    [SerializeField] private float _speedAddDelta = 2f;
    public bool OncePar = false;
    private Vector3 _wPos;
    private void Awake()
    {
        _par = this.GetComponent<ParticleSystem>();
        _arrPar = new ParticleSystem.Particle[_par.main.maxParticles];
        _speedAdd = 0f;
        if(OncePar)
        {
            _originEmissionRate = _par.emission.GetBurst(0).count.curveMultiplier;
        }
        else
        {
            _originEmissionRate = _par.emission.rateOverTimeMultiplier;
        }
    }

    private void setActive()
    {
        _isActive = true;
    }

    private void Update()
    {
        if(!_isActive || !_par)
        {
            return;
        }
        _arrCount = _par.GetParticles(_arrPar);
        if(_arrCount < 1)
        {
            _isActive = false;
            _par.Stop();
            _speedAdd = 0f;
        }
        else
        {
            _speedAdd += Time.unscaledDeltaTime * _speedAddDelta;
        }

        for(int i = 0; i < _arrCount; i++)
        {
            Vector3 vector = _wPos - _arrPar[i].position;
            if (vector.magnitude <= (_speed + _speedAdd) * Time.unscaledDeltaTime)
            {
                _arrPar[i].position = _wPos;
                _arrPar[i].remainingLifetime = 0f;
                var emis = _par.emission;
                emis.rateOverTimeMultiplier = 0;
            }
            else
            {
                _arrPar[i].position += vector.normalized * (_speed + _speedAdd) * Time.unscaledDeltaTime;
            }
        }
        _par.SetParticles(_arrPar, _arrCount);
    }

    public  void Play(Vector3 _pos, int emit_count)
    {
        _wPos = _pos;
        Invoke("setActive", _delayTime + 0.0f);
        if(_par == null)
        {
            _par = this.GetComponent<ParticleSystem>();
        }
        if(OncePar)
        {
            var b = _par.emission.GetBurst(0);
            b.count = new ParticleSystem.MinMaxCurve(_originEmissionRate);
            _par.emission.SetBurst(0, b);
            OncePar = false;
        }
        _speedAdd = 0;
        _isActive = false;
        _par.Stop();
        _par.Emit(emit_count);
    }
}
