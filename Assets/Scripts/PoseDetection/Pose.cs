using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPose", menuName = "Pose")]
public class Pose : ScriptableObject
{
    public string name = "New Pose";
    public List<JointRotation> jointPositions = new List<JointRotation>() { new JointRotation("middle 3"), new JointRotation("middle 2"), new JointRotation("middle 1"), new JointRotation("index 3"), new JointRotation("index 2"), new JointRotation("index 1") };
}
