using UnityEngine;
using System.Collections;

public class Charactor : MonoBehaviour {

    public enum CharactorState
    {
        walk,
        Jump,
        stop
    }
    public CharactorState CurrentState = CharactorState.stop;
    /// <summary>
    /// is on the ground
    /// </summary>
    bool isGround;
    /// <summary>
    /// rigidbody
    /// </summary>
    private Rigidbody rig;

	// Use this for initialization
	void Start () {
        rig = GetComponent<Rigidbody>();
	}
    void OnEnable()
    {
        AddEvents();
    }
    void OnDisable()
    {
        RemoveEvents();
    }
    void AddEvents()
    {
        VolumToMoveManager.Instance.jump += Jump;
        VolumToMoveManager.Instance.move += Move;
        VolumToMoveManager.Instance.stop += Stop;

    }
    void RemoveEvents()
    {
        VolumToMoveManager.Instance.jump -= Jump;
        VolumToMoveManager.Instance.move -= Move;
        VolumToMoveManager.Instance.stop -= Stop;
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

    private void Move()
    {
        Debug.Log("walk");
        ChangeToState(CharactorState.walk);

    }
    private void Jump()
    {
        Debug.Log("jump");
        ChangeToState(CharactorState.Jump);

    }
    private void Stop()
    {
        Debug.Log("stop");
        ChangeToState(CharactorState.stop);
    }
    private void ChangeToState(CharactorState state)
    {
        if (CurrentState != state) CurrentState = state;
    }
}
