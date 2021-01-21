using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public float speed;
	private Rigidbody2D rb;
	private Vector2 force;
	Animator anim;
	
	private void Start(){
		anim = GetComponent<Animator>();
		rb = GetComponent<Rigidbody2D>();
	}

	private void FixedUpdate ()
	{
		force = new Vector2(Input.GetAxisRaw("Horizontal")*speed, Input.GetAxisRaw("Vertical")*speed).normalized;
		rb.MovePosition(rb.position + force * speed * Time.fixedDeltaTime);
	}

	private void Update() 
	{
		//SoundManager.PlaySound("walking");
		anim.SetFloat("Mov_X", force.x);
		anim.SetFloat("Mov_Y", force.y);
	}

}