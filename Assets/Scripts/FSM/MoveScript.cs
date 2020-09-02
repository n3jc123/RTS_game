
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class MoveScript : MonoBehaviour
{
    FSM fsm;
	//public Vector3 target;
	
	float speed = 20;
	Vector3[] path;
	int targetIndex;

	bool resourceReturned;
	
	// Start is called before the first frame update
	void Start()
    {
        fsm = gameObject.GetComponent<FSM>();
		if(!fsm.building)
        {
			PathRequestManager.RequestPath(transform.position, fsm.target, OnPathFound);
			Debug.Log(fsm.target);
		}
			

		fsm.moving = true;
		if(gameObject.tag == "Knight")
        {
			speed = 25;
        }

    }

    // Update is called once per frame
    void Update()
    {
		if(fsm.targetBuilding != null && Vector3.Distance(this.transform.position, fsm.targetBuilding.transform.position) <= 8f)
        {
			fsm.goingToBuilding = false;
			fsm.attackingBuilding = true;
        }
        else
        {
			fsm.attackingBuilding = false;
		}
		if (Input.GetMouseButtonUp(1) && fsm.selected)
        {
			//fsm.target = Grid.GetMouseWorldPosition();
			PathRequestManager.RequestPath(transform.position, fsm.target, OnPathFound);
			//fsm.targetBuilding = null;
			fsm.returningResource = false;
		}
		else if(fsm.resourceAmount == 12)
        {
			fsm.targetWarehouse = GetClosestWarehouse();
			PathRequestManager.RequestPath(transform.position, new Vector3(fsm.targetWarehouse.x, fsm.targetWarehouse.y, fsm.targetWarehouse.z + 6), OnPathFound);
			fsm.returningResource = true;
			fsm.resourceAmount--;

		}
		else if(fsm.returningResource && Vector3.Distance(fsm.targetWarehouse, transform.position) < 9 && !resourceReturned)// fsm.targetWarehouse.x, fsm.targetWarehouse.y, fsm.targetWarehouse.z + 6) == transform.position)
        {
			Debug.Log("vrnu sem resource");
			fsm.resourceAmount = 0;
			fsm.player.AddResource(fsm.resource);
			resourceReturned = true;
			PathRequestManager.RequestPath(transform.position, new Vector3(fsm.target.x, fsm.target.y, fsm.target.z), OnPathFound);

		}

		RaycastHit rayHit;
		Vector3 rayOrigin = this.transform.position;
		Vector3 ray = this.transform.forward * 5;


		if (Physics.Raycast(rayOrigin, ray, out rayHit, ray.magnitude))
		{

			if ((rayHit.transform.tag == "Townhall" || rayHit.transform.tag == "Warehouse" || rayHit.transform.tag == "House" || rayHit.transform.tag == "Barracks"
				|| rayHit.transform.tag == "Stables") && fsm.targetBuilding != null && rayHit.collider.gameObject != fsm.targetBuilding)
			{
				PathRequestManager.RequestPath(transform.position, fsm.target, OnPathFound);
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
		Vector3 currentWaypoint;
		if (path.Length > 0)
			currentWaypoint = path[0];
		else
			currentWaypoint = Vector3.zero;

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
			this.GetComponentInChildren<Canvas>().transform.rotation = Quaternion.Euler(0, 0, 0);

			transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
			yield return null;

		}
	}

	public Vector3 GetClosestWarehouse()
    {
		float minDistance = 100000f;

		foreach (GameObject building in fsm.player.getBuildings())
		{
			float distance = Vector3.Distance(building.transform.position, transform.position);
			if (distance < minDistance && (building.tag == "Warehouse" || building.tag == "Townhall"))
			{
				minDistance = distance;
				fsm.closestBuilding = building;
			}
		}
		return fsm.closestBuilding.transform.position;
	}

	

	public void OnTriggerEnter(Collider collider)
	{

		if (collider.gameObject.layer == 11)
		{
			speed = 4;
			if(gameObject.tag == "Villager")
            {
				fsm.gathering = true;
				fsm.resource = collider.tag;
				fsm.returningResource = false;
			}
			

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
