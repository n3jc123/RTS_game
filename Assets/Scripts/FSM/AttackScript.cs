﻿using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class AttackScript : MonoBehaviour
{
    FSM fsm;
    GameObject target;

	float speed = 20;
	Vector3[] path;
	int targetIndex;

	bool fighting;
	bool moving;

	float timer = 0.3f;
	// Start is called before the first frame update
	void Start()
    {
        fsm = gameObject.GetComponent<FSM>();
        target = fsm.closestEnemy;
    }

    // Update is called once per frame
    void Update()
    {
        if(!moving && !fighting)
        {
			//PathRequestManager.RequestPath(this.transform.position, fsm.closestEnemy.transform.position, OnPathFound);

				float step = speed * Time.deltaTime; // calculate distance to move
				transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);
				Debug.Log("lalalaa");
				moving = true;
			
        }
		//Vector3.Distance(other.position, transform.position);
		if (Vector3.Distance(this.transform.position, target.transform.position) < 2f)
        {
			moving = false;
			fighting = true;

			timer -= Time.deltaTime;
			if (timer < 0)
			{
				timer = 0.03f;
				if (target != null)
					target.GetComponent<FSM>().health -= fsm.attackDamage;
				else
					return;
			}
        }
		else
		{
			fighting = false;
		}
		if (this.transform.position != target.transform.position)
        {
			moving = false;
        }
        
    }

	public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
	{

		if (pathSuccessful)
		{
			path = newPath;
			targetIndex = 0;
			StopCoroutine("FollowPath");
			StartCoroutine("FollowPath");
		}
	}

	IEnumerator FollowPath()
	{

		Vector3 currentWaypoint = path[0];

		while (true)
		{
			if (transform.position == currentWaypoint)
			{
				targetIndex++;
				if (targetIndex >= path.Length)
				{
					targetIndex = 0;
					path = new Vector3[0];
					fsm.moving = false;
					yield break;
				}
				currentWaypoint = path[targetIndex];
			}
			this.transform.LookAt(currentWaypoint);

			transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
			yield return null;

		}
	}

	public void OnTriggerEnter(Collider collider)
	{

		if (collider.gameObject.layer == 11)
		{
			speed = 4;
			fsm.gathering = true;
			fsm.resource = collider.tag;

		}
	}

	public void OnTriggerExit(Collider collider)
	{

		if (collider.gameObject.layer == 11)
		{
			speed = 20;
			fsm.gathering = false;

		}
	}
}