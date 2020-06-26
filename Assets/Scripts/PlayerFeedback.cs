using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFeedback : MonoBehaviour
{
	PlayerInputController thisPlayer;
	SoundManager soundManager { get { return SoundManager.instance; } }

	[Header("Effects")]
	public GameObject kickEffect_prefab;
	//public bool kickEffectinCameraSpace;
	//public GameObject stompEffect;
	//public GameObject fallEffect;
	private List<EffectTuple> kickEffects = new List<EffectTuple>();

	public Transform particlePoolingPlace;

	// finger references
	private ActiveFinger indexFinger;
	private ActiveFinger middleFinger;
	private CollisionHandler[] wristTriggers;

	private void Awake()
	{	// get reference
		thisPlayer = GetComponent<PlayerInputController>();
	}

	void Start()
    {
		indexFinger = thisPlayer.activeAvatar.indexFinger;
		middleFinger = thisPlayer.activeAvatar.middleFinger;
		wristTriggers = thisPlayer.activeAvatar.wristTriggers;

		// wire events
		//NOTE: At this point the PlayerInputController should already have its references in place
		WireFingerEvents(indexFinger);
		WireFingerEvents(middleFinger);

		for (int i = 0; i < wristTriggers.Length; i++)
		{
			wristTriggers[i].OnTouchedGround += () =>
			{
				soundManager.Play(ExtensionMethods.GetRandomElement<string>(new List<string>() { "gong" }));
			};

		}

	}

	private void WireFingerEvents(ActiveFinger finger)
	{
		// stomping
		finger.fingerBottom.OnTouchedGround += () =>
		{
			// sound 
			soundManager.Play(ExtensionMethods.GetRandomElement<string>(new List<string>() { "guitar1", "guitar2", "guitar3", "guitar4", "guitar5", "guitar6", "guitar7", "guitar8" }));

		};

		//kicking
		finger.fingerBottom.OnContactWithOtherPlayer += (collision) =>
		{
			Debug.Log("Cimbals");
			// sound 
			soundManager.PlayOrRestart(ExtensionMethods.GetRandomElement<string>(new List<string>() { "cimbals1", "cimbals1", "cimbals2", "cimbals3", "cimbals4", "cimbals5" }));

			//particles
			EffectTuple effectToSpawn = GetUnusedEffect(kickEffects);
			if (effectToSpawn == null)
			{
				effectToSpawn = new EffectTuple(GameObject.Instantiate(kickEffect_prefab));
				kickEffects.Add(effectToSpawn);
			}
			SpawnParticleEffect(effectToSpawn, collision.contacts[0].point);


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

	public void SpawnParticleEffect(EffectTuple effect, Vector3 wordspace, bool toCameraSpace = false, float depth = 5f)
	{
		effect.gameobject.transform.position = wordspace;
		effect.particleSystem.Clear();
		effect.particleSystem.Play();
		effect.isUsed = true;
	}

	public void PoolParticleEffect(EffectTuple effect) 
	{
		effect.gameobject.transform.position = particlePoolingPlace.position;
		effect.particleSystem.Stop();
		effect.isUsed = false;
	}

	public void FreeEffect(EffectTuple effect)
	{ }
}
