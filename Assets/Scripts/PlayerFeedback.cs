using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFeedback : MonoBehaviour
{
	PlayerInputController thisPlayer;
	SoundManager soundManager { get { return SoundManager.instance; } }

	public GameObject contactParticlePrefab;
	public List<EffectTuple> kickEffects;
	public Transform poolingPlace;
	// Start is called before the first frame update

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
		finger.fingerBottom.OnTouchedGround += () =>
		{
			// sound 
			soundManager.Play(ExtensionMethods.GetRandomElement<string>(new List<string>() { "guitar1", "guitar2", "guitar3", "guitar4", "guitar5", "guitar6", "guitar7", "guitar8" }));

		};

		// stomping
		finger.fingerBottom.OnContactWithOtherPlayer += (collision) =>
		{
			Debug.Log("Cimbals");
			// sound 
			soundManager.PlayOrRestart(ExtensionMethods.GetRandomElement<string>(new List<string>() { "cimbals1", "cimbals1", "cimbals2", "cimbals3", "cimbals4", "cimbals5" }));

		};

	}

	public  class EffectTuple
	{
		public string effectType;
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
