using UnityEngine;

public class cameraToTarget : MonoBehaviour
{
	public GameObject cameraTarget;
	public float movementSpeed;
	public float rotationSpeed;
	

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		this.transform.position = Vector3.MoveTowards(this.transform.position, cameraTarget.transform.position, movementSpeed);
		this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, Mathf.Lerp(this.transform.eulerAngles.y, cameraTarget.transform.eulerAngles.y, rotationSpeed), this.transform.eulerAngles.z);
    }
}
