using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSplatter : MonoBehaviour {

	public ParticleSystem particleLauncher;
	public Gradient particleColorGradient;
	//public ParticleDecalPool dropletDecalPool;

	List<ParticleCollisionEvent> _collisionEvents;


	void Start () 
	{
		_collisionEvents = new List<ParticleCollisionEvent> ();
	}

	void OnParticleCollision(GameObject other)
	{
		int numCollisionEvents = ParticlePhysicsExtensions.GetCollisionEvents (particleLauncher, other, _collisionEvents);

		int i = 0;
		while (i < numCollisionEvents) 
		{
            //	dropletDecalPool.ParticleHit(collisionEvents[i], particleColorGradient);
            i++;
		}

	}

}
