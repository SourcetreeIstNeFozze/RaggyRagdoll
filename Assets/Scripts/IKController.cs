using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class IKGoal
{
    public GameObject goal;
    //public bool left;


    public Vector3 startPosition;
    //[Range(0.0f, 2f)]
    //public float depthAxis;
}



public class IKController : MonoBehaviour
{
    bool useIK;
    public List<IKGoal> IKGoals;

    [SerializeField]
    public InputController inputC;

    void Awake()
    {
        for (int i = 0; i < IKGoals.Count; i++)
        {
            IKGoals[i].startPosition = IKGoals[i].goal.transform.position;
        }
    }

    void Start()
    {
        if (this.GetComponent<PlayerInputControllerRealisticHand>().IK_controls)
            useIK = true;
        else
            useIK = false;
    }

    public void OnIKControllINDEX(InputValue value)
    {
        if (useIK)
        {
            //Vector3 newPosition = new Vector3(IKGoals[0].startPosition.x + value.Get<Vector2>().x, IKGoals[0].startPosition.y + value.Get<Vector2>().y, IKGoals[1].startPosition.z);
            Vector3 newPosition = new Vector3(IKGoals[0].startPosition.x, IKGoals[0].startPosition.y + value.Get<Vector2>().y, IKGoals[1].startPosition.z + value.Get<Vector2>().x);

            IKGoals[0].goal.transform.position = newPosition;
        }
    }

    public void OnIKControllMIDDLE(InputValue value)
    {
        if (useIK)
        {
            //Vector3 newPosition = new Vector3 (IKGoals[1].startPosition.x + value.Get<Vector2>().x, IKGoals[1].startPosition.y + value.Get<Vector2>().y, IKGoals[1].startPosition.z);
            //Vector3 newPosition = new Vector3(IKGoals[1].startPosition.x, IKGoals[1].startPosition.y + value.Get<Vector2>().y, IKGoals[1].startPosition.z + value.Get<Vector2>().x);
            Vector3 newPosition = IKGoals[1].startPosition + IKGoals[1].goal.transform.up * value.Get<Vector2>().y + IKGoals[1].goal.transform.forward * value.Get<Vector2>().x;
            IKGoals[1].goal.transform.position = newPosition;
        }
    }

}
