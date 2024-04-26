using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ColorStreamGun : MonoBehaviour
{
    [SerializeField] private InputActionReference _streamOnShoot;
    [SerializeField] public ParticleSystem _particleLauncher;
    [SerializeField] public ParticleSystem _splatterParticle;

    public Gradient _particleColorGradient;
    List<ParticleCollisionEvent> _collisionEvents;

    private void Start()
    {
        _collisionEvents = new List<ParticleCollisionEvent>();
    }

    private void OnEnable()
    {
        _streamOnShoot.action.Enable();
    }

    private void OnDisable()
    {
        _streamOnShoot.action.Disable();
    }

    private void OnParticleCollision(GameObject other)
    {
        ParticlePhysicsExtensions.GetCollisionEvents(_particleLauncher, other, _collisionEvents);
        for (int i = 0; i < _collisionEvents.Count; i++)
        {
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
        if (_streamOnShoot.action.IsPressed())
        {
            ParticleSystem.MainModule psMain = _particleLauncher.main;
            psMain.startColor = _particleColorGradient.Evaluate(Random.Range(0f, 1f));
            _particleLauncher.Emit(1);
        }
    }
}
