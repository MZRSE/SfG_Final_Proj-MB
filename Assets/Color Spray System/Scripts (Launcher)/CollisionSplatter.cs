using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSplatter : MonoBehaviour {

	public ParticleSystem _particleLauncher;
	public Gradient _particleColorGradient;
	public ParticleDecalPool _dropletDecalPool;

	List<ParticleCollisionEvent> _collisionEvents;


	void Start () 
	{
		_collisionEvents = new List<ParticleCollisionEvent> ();
	}

	void OnParticleCollision(GameObject other)
	{
		int numCollisionEvents = ParticlePhysicsExtensions.GetCollisionEvents (_particleLauncher, other, _collisionEvents);

		int i = 0;
		while (i < numCollisionEvents) 
		{
            _dropletDecalPool.ParticleHit(_collisionEvents[i], _particleColorGradient);
            i++;
		}

	}

}
