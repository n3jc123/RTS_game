using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveScript : MonoBehaviour
{
    FSM fsm;
	public Vector3 target;
	float speed = 20;
	Vector3[] path;
	int targetIndex;

	
	// Start is called before the first frame update
	void Start()
    {
        fsm = gameObject.GetComponent<FSM>();
		target = Grid.GetMouseWorldPosition();
		PathRequestManager.RequestPath(transform.position, target, OnPathFound);
		fsm.moving = true;
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
