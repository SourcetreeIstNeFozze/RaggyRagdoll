using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
	[Header("Config")]
	public bool delay;
	public bool lerp;

	public Transform player1;
	public Transform player2;
	public float cameraHeight;
	public float cameraDistance;
	public float cameraMinDistance;
	public float cameraMaxDistance;
	public float distanceMultiplier = 1.5f;
    
	// Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		// get line between the players
		Vector3 player12Dposition = player1.transform.position;
		player12Dposition.y = cameraHeight;
		Vector3 player2DPosition = player2.transform.position;
		player2DPosition.y = cameraHeight;

		Vector3 lineBetweenPlayers = player12Dposition - player2DPosition;
		Vector3 midPointBetweenPlayers = player2DPosition + new Vector3(0f, cameraHeight, 0f) + lineBetweenPlayers / 2;

		

		// get vector perpendicultar to the players
		Vector3 perpendicularDir = Vector3.Cross(lineBetweenPlayers, Vector3.up).normalized;

		cameraDistance = Mathf.Clamp(lineBetweenPlayers.magnitude * distanceMultiplier, cameraMinDistance, cameraMaxDistance);

		this.transform.position = midPointBetweenPlayers + perpendicularDir * cameraDistance;
		this.transform.LookAt(midPointBetweenPlayers);

	}
}
