using System.Collections;
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
	bool attackingBuilding;

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
		if(target != null)
        {
			if (!moving && !fighting)
			{
				float step = speed * Time.deltaTime;
				transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);
				Debug.Log("lalalaa");
				moving = true;
			}
			if (Vector3.Distance(this.transform.position, target.transform.position) < 2f)
			{
				moving = false;
				fighting = true;

				timer -= Time.deltaTime;
				if (timer < 0)
				{
					timer = 0.1f;
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
		else if(fsm.targetBuilding != null)
        {
			
			if (Vector3.Distance(this.transform.position, fsm.targetBuilding.transform.position) <= 8f)
			{
				moving = false;
				fighting = true;

				timer -= Time.deltaTime;
				if (timer < 0)
				{
					timer = 0.3f;
					if (fsm.targetBuilding != null)
					{
						Debug.Log("building zgubla health");
						attackingBuilding = false;
						fsm.targetBuilding.GetComponent<BuildingScript>().health -= 1;// fsm.targetBuilding.GetComponent<BuildingScript>().health - (fsm.attackDamage / 10);
					}
					else
						return;
				}
			}
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
