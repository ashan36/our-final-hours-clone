using UnityEngine;
using System.Collections;

public class ShirtSwitch : MonoBehaviour {

	public int chestID = 0;
	public GameObject chest;
	public Mesh chestMesh;
	public Mesh newChest; //for armor dropping later
	public Mesh oldChest; //for armor dropping later

	// Reference meshes added to object manually before runtime
	public Mesh chest0;
	public Mesh chest1;
	public Mesh chest2;

	
	void Awake () 
	{
		chest = GameObject.Find ("PlayerChest");
		chestMesh = chest.GetComponent <SkinnedMeshRenderer>().sharedMesh;
	}

	void FixedUpdate () 
	{

		if (chestID == 0)
		{
			chest.GetComponent <MeshFilter>().mesh = chest0;
			newChest = chest0;
			ArmorChange ();
		}

		if (chestID == 1)
		{
			chest.GetComponent <MeshFilter>().mesh = chest1;
			newChest = chest1;
			ArmorChange ();
		}

		if (chestID == 2)
		{
			chest.GetComponent <MeshFilter>().mesh = chest2;
			newChest = chest2;
			ArmorChange ();
		}
	}
	
	void ArmorChange ()
	{
		//oldChest = chestMesh;
		chestMesh = newChest;
	
		chest.GetComponent <SkinnedMeshRenderer>().sharedMesh = chestMesh;
	}
}
