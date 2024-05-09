using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.InputSystem;

public class ColorStreamGun : MonoBehaviour
{
    [SerializeField] public ParticleSystem _particleLauncher;
    [SerializeField] public ParticleSystem _nozzleParticle;
    [SerializeField] public ParticleSystem _splatterParticle;
    public Gradient _particleColorGradient;
    public ParticleDecalPool _splatDecalPool;
    List<ParticleCollisionEvent> _collisionEvents;
    
    private void Start()
    {
        _collisionEvents = new List<ParticleCollisionEvent>();
    }

    private void OnParticleCollision(GameObject other)
    {
        ParticlePhysicsExtensions.GetCollisionEvents(_particleLauncher, other, _collisionEvents);
        for (int i = 0; i < _collisionEvents.Count; i++)
        {
            _splatDecalPool.ParticleHit(_collisionEvents[i], _particleColorGradient); 
            EmitAtLocation(_collisionEvents[i]);
        }
    }

    void EmitAtLocation(ParticleCollisionEvent particleCollisionEvent)
    {
        _splatterParticle.transform.position = particleCollisionEvent.intersection;
        _splatterParticle.transform.rotation = Quaternion.LookRotation(particleCollisionEvent.normal); //splatter rotation, huh... keep this in mind
        ParticleSystem.MainModule psMain = _splatterParticle.main;
        psMain.startColor = _particleColorGradient.Evaluate(Random.Range(0f, 1f));
        _splatterParticle.Emit(1);
    }

    private void FixedUpdate()
    {
        if (Input.GetKey("mouse 0"))
        {
            ParticleSystem.MainModule psMain = _particleLauncher.main;
            //psMain.startColor = _particleColorGradient.Evaluate(Random.Range(0f, 1f));
            _particleLauncher.Emit(1);
            _nozzleParticle.Emit(1);
        }
    }
}
