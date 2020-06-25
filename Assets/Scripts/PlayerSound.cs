using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
	PlayerInputController thisPlayer;
	SoundManager soundManager { get { return SoundManager.instance; } }

	public GameObject contactParticlePrefab;
	public List<EffectTuple> kickEffects;
	public Transform poolingPlace;
	// Start is called before the first frame update

	private void Awake()
	{	// get reference
		thisPlayer = GetComponent<PlayerInputController>();
	}

	void Start()
    {
		// wire events
		//NOTE: At this point the PlayerInputController should already have its references in place

		// stomping
		this.thisPlayer.activeAvatar.indexFinger.fingerBottom.OnTouchedGround += () =>
		{
			// sound 
			soundManager.Play(ExtensionMethods.GetRandomElement<string>(new List<string>() { "guitar1", "guitar2", "guitar3", "guitar4", "guitar5", "guitar6", "guitar7", "guitar8" }));

			////partickle effects
			//EffectTuple effectToSpawn = GetUnusedEffect();

			//// if no free effect found, make a new one
			//if (effectToSpawn == null)
			//{
			//	//spawn new Kick effect
			//	EffectTuple newEffect = new EffectTuple(GameObject.Instantiate(contactParticlePrefab);
			//	kickEffects.Add(newEffect);
			//	effectToSpawn = newEffect;
			//}

			//effectToSpawn.gameobject.transform.position = 

		};

	}

	public  class EffectTuple
	{
		public GameObject gameobject;
		public bool isUsed = false;

		//may need to make it more complex later
		public ParticleSystem particleSystem;

		public EffectTuple(GameObject gameObject)
		{
			gameobject = gameObject;
			particleSystem = gameObject.GetComponentInChildren<ParticleSystem>();
		}
	}

	public EffectTuple GetUnusedEffect(List<EffectTuple> effects)
	{
		for (int i = 0; i < effects.Count; i++)
		{
			if (effects[i].isUsed == false)
			{
				return effects[i];
			}
		}

		return null;
	} 

	public void FreeEffect(EffectTuple effect)
	{ }
}
