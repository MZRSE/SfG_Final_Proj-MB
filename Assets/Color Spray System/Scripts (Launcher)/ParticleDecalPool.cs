using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDecalPool : MonoBehaviour
{
    public int _maxDecals = 1000;
    public float _decalSizeMin = .5f;
    public float _decalSizeMax = 1.5f;

    private ParticleSystem _decalParticleSystem;
    private int _particleDataIndex;
    private ParticleDecalData[] _particleData;
    private ParticleSystem.Particle[] _particles;

    void Start()
    {
        _decalParticleSystem = GetComponent<ParticleSystem>();
        _particles = new ParticleSystem.Particle[_maxDecals];
        _particleData = new ParticleDecalData[_maxDecals];
        for (int i = 0; i < _maxDecals; i++)
        {
            _particleData[i] = new ParticleDecalData();
        }
    }

    public void ParticleHit(ParticleCollisionEvent _particleCollisionEvent, Gradient _colorGradient)
    {
        SetParticleData(_particleCollisionEvent, _colorGradient);
        DisplayParticles();
    }

    void SetParticleData(ParticleCollisionEvent _particleCollisionEvent, Gradient _colorGradient)
    {
        if (_particleDataIndex >= _maxDecals)
        {
            _particleDataIndex = 0;
        }
        _particleData[_particleDataIndex]._position = _particleCollisionEvent.intersection;
        Vector3 _particleRotEuler = Quaternion.LookRotation(_particleCollisionEvent.normal).eulerAngles;
        _particleRotEuler.z = Random.Range(0, 360);
        _particleData[_particleDataIndex]._rotation = _particleRotEuler;
        _particleData[_particleDataIndex]._size = Random.Range(_decalSizeMin, _decalSizeMax);
        _particleData[_particleDataIndex]._color = _colorGradient.Evaluate(Random.Range(0f, 1f));

        _particleDataIndex++;
    }

    void DisplayParticles()
    {
        for(int i = 0; i < _particleData.Length; i++)
        {
            _particles[i].position = _particleData[i]._position;
            _particles[i].rotation3D = _particleData[i]._rotation;
            _particles[i].startSize = _particleData[i]._size;
            _particles[i].startColor = _particleData[i]._color;
        }

        _decalParticleSystem.SetParticles(_particles, _particles.Length);
    }
}
