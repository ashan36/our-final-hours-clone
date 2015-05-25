using UnityEngine;
using System.Collections;

public class WallTransparency : MonoBehaviour {

    Material wallMaterial;


    void Awake ()
    {
        wallMaterial = GetComponent<MeshRenderer>().material;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	    if (PlayerController.isInside)
            BecomeTransparent();
        if (!PlayerController.isInside)
            BecomeOpaque();
	}

    void BecomeTransparent ()
    {
        Color color = wallMaterial.color;
        color.a = 0.2f;
        wallMaterial.color = color;
    }

    void BecomeOpaque()
    {
        Color color = wallMaterial.color;
        color.a = 1f;
        wallMaterial.color = color;
    }

}
