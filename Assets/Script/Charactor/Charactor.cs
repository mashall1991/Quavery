using UnityEngine;
using System.Collections;

public class Charactor : MonoBehaviour {

    public enum CharactorState
    {
        walk,
        run,
        stop
    }

    bool isGround;

    private Rigidbody rig;

	// Use this for initialization
	void Start () {
        rig = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.A))
        {
            rig.velocity = new Vector3(-10,rig.velocity.y,0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            rig.velocity = new Vector3(10, rig.velocity.y, 0);

        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rig.AddForce(Vector3.up * 1000);
        }
    }
    void OnColliderEnter(Collider co)
    {

    }
}
