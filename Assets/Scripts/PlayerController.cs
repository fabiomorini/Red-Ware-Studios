using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public float speed;

	Animator anim;
	
	void Start(){
		anim = GetComponent<Animator>();
	}
	void Update () {

		Vector3 mov = new Vector3(
			Input.GetAxisRaw("Horizontal"),
			Input.GetAxisRaw("Vertical"), 
			0
			);

		transform.position = Vector3.MoveTowards(
			transform.position,
			transform.position + mov, 
			speed * Time.deltaTime
		);

		anim.SetFloat("Mov_X", mov.x);
		anim.SetFloat("Mov_Y", mov.y);
	}
}
