using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleTargetMove : MonoBehaviour
{
    private ParticleSystem par;
    private ParticleSystem.Particle[] arrPar;
    private int arrCount;
    public float speed = 0.1f;
    public float delayTime = 1f;
    public float originEmissionRate;
    public bool isActive;
    public float speedAdd;
    public float speedAddDelta = 2f;
    public bool oncePar = false;
    private Vector3 wPos;
    private void Awake()
    {
        par = this.GetComponent<ParticleSystem>();
        arrPar = new ParticleSystem.Particle[par.main.maxParticles];
        speedAdd = 0f;
        if(oncePar)
        {
            originEmissionRate = par.emission.GetBurst(0).count.curveMultiplier;
        }
        else
        {
            originEmissionRate = par.emission.rateOverTimeMultiplier;
        }
    }

    private void setActive()
    {
        isActive = true;
    }

    private void Update()
    {
        if(!isActive || !par)
        {
            return;
        }
        arrCount = par.GetParticles(arrPar);
        if(arrCount < 1)
        {
            isActive = false;
            par.Stop();
            speedAdd = 0f;
        }
        else
        {
            speedAdd += Time.unscaledDeltaTime * speedAddDelta;
        }

        for(int i = 0; i < arrCount; i++)
        {
            Vector3 vector = wPos - arrPar[i].position;
            if (vector.magnitude <= (speed + speedAdd) * Time.unscaledDeltaTime)
            {
                arrPar[i].position = wPos;
                arrPar[i].remainingLifetime = 0f;
                var emis = par.emission;
                emis.rateOverTimeMultiplier = 0;
            }
            else
            {
                arrPar[i].position += vector.normalized * (speed + speedAdd) * Time.unscaledDeltaTime;
            }
        }
        par.SetParticles(arrPar, arrCount);
    }

    public  void Play(Vector3 _pos, int emit_count)
    {
        wPos = _pos;
        Invoke("setActive", delayTime + 0.0f);
        if(par == null)
        {
            par = this.GetComponent<ParticleSystem>();
        }
        if(oncePar)
        {
            var b = par.emission.GetBurst(0);
            b.count = new ParticleSystem.MinMaxCurve(originEmissionRate);
            par.emission.SetBurst(0, b);
        }
        speedAdd = 0;
        isActive = false;
        par.Stop();
        par.Emit(emit_count);
    }
}
