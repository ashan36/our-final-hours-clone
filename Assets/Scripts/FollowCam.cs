using UnityEngine;
using System.Collections;

public class FollowCam : MonoBehaviour {

    public static FollowCam S;

    public float easing = 0.03f;
    public GameObject poi;
    public float camY;
    public Vector3 camOffset;

    void Awake()
    {
        S = this;
        camY = this.transform.position.y;
        camOffset = new Vector3(-0.11f, 5.163f, -4.649f);
    }

	// Use this for initialization
	void Start () 
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (poi == null) return;

        Vector3 destination = poi.transform.position + camOffset;
        destination.y = camY;

        if (Mathf.Abs(destination.x - transform.position.x) > 0.64 || Mathf.Abs(destination.z - transform.position.z) > 0.6)
        {
            destination = Vector3.Lerp(transform.position, destination, easing);
            transform.position = destination;
        }
	}
}
