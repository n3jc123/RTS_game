using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildScript : MonoBehaviour
{

	FSM fsm;
	public Vector3 target;
	float speed = 20;
	Vector3[] path;
	int targetIndex;
	//public GameObject building;

	bool moving;
	bool arrived;
	// Start is called before the first frame update
	void Start()
    {
		fsm = gameObject.GetComponent<FSM>();
		moving = false;
		
	}

    // Update is called once per frame
    void Update()
    {
        if(target != Vector3.zero && !moving)
        {
			if(fsm.buildingA.tag == "Barracks")
            {
				PathRequestManager.RequestPath(transform.position, new Vector3(target.x + 6, target.y, target.z), OnPathFound);

			}
			PathRequestManager.RequestPath(transform.position, new Vector3(target.x, target.y, target.z + 6), OnPathFound);
			Debug.Log(target);
			moving = true;
		}

		if(arrived)
        {
			fsm.buildingA.GetComponent<BuildingScript>().villagerArrived = true;
			if(fsm.buildingA.GetComponent<BuildingScript>().isBuilt)
            {
				fsm.building = false;

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
					arrived = true;
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
