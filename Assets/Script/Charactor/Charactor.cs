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
	public RectTransform RevivePoint;
    /// <summary>
    /// is on the ground
    /// </summary>
    public bool isGround;
    /// <summary>
    /// rigidbody
    /// </summary>
	private Rigidbody2D rig;

	// Use this for initialization
	void Start () {
        rig = GetComponent<Rigidbody2D>();
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
	void Update ()
	{
		switch (CurrentState) {
		case CharactorState.stop:
			rig.velocity = Vector2.Lerp (rig.velocity, new Vector2(0,rig.velocity.y),0.5f);
			break;
		case CharactorState.walk:
			rig.velocity = new Vector2 (10, rig.velocity.y);
			break;
		case CharactorState.Jump:
			if (isGround) {
				rig.AddForce (Vector2.up * 2600);
				rig.velocity = new Vector2 (10, rig.velocity.y);
				isGround = false;
			}
			break;

		}
    }
	void OnCollisionEnter2D(Collision2D co)
    {
		Debug.Log ("grounded");
		if (co.gameObject.layer == LayerMask.NameToLayer ("ground")) {
			isGround = true;
		}
		if (co.gameObject.layer == LayerMask.NameToLayer ("bounds")) {
			GetComponent<RectTransform> ().anchoredPosition = RevivePoint.anchoredPosition;
		}
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
