using UnityEngine;
using System.Collections;

//Purpose: This class is a work around to make sure than the particle objects
//	will disapear after they have played their animation.
public class AutoParticleDeath : MonoBehaviour {
	ParticleSystem ps;
	// Use this for initialization
	void Start () {
		ps = this.GetComponent<ParticleSystem> ();
	}
	
	// Update is called once per frame
	void Update () {
	if (ps) {
			if(!ps.IsAlive())
			{
				Destroy (gameObject);
			}
		}
	}
}
