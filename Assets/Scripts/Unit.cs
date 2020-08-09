using UnityEngine;
using System.Collections;
//using System.Numerics;

public class Unit : MonoBehaviour
{


	public Vector3 target;
	float speed = 20;
	Vector3[] path;
	int targetIndex;

	public bool isSelected;
	private bool isMoving;


	void Start()
	{
		target = this.transform.position;
		//PathRequestManager.RequestPath(transform.position, target, OnPathFound);
	}

    void Update()
    {
		if(isMoving)
        {
			RaycastHit rayHit;
			Vector3 rayOrigin = this.transform.position;
			Vector3 ray = this.transform.forward * 5;


			if (Physics.Raycast(rayOrigin, ray, out rayHit, ray.magnitude))
			{
				
				if (rayHit.transform.tag == "Townhall" || rayHit.transform.tag == "Warehouse" || rayHit.transform.tag == "House" || rayHit.transform.tag == "Barracks" || rayHit.transform.tag == "Stables")
				{
					PathRequestManager.RequestPath(transform.position, target, OnPathFound);
				}

			}
		}
		



		if (isSelected)
		{
			this.transform.GetChild(0).gameObject.SetActive(true);
		}
		else
		{
			this.transform.GetChild(0).gameObject.SetActive(false);
		}
	}

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
	{
		
		if (pathSuccessful)
		{
			isMoving = true;
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
					
					yield break;
				}
				currentWaypoint = path[targetIndex];
			}
			this.transform.LookAt(currentWaypoint);

			transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
			yield return null;

		}
	}

	public void TakeAction()
	{

		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit))
		{
			if (hit.collider.transform.tag == "Forrest" || hit.collider.transform.tag == "Gold" || hit.collider.transform.tag == "Stone" || hit.collider.transform.tag == "Farm")
			{
				if(this.tag == "Villager")
                {
					target = hit.transform.position;
					
					GetComponent<Villager>().isGathering = true;

				}
                else
                {
					target = hit.transform.position;
				}
				
			}
			else
			{
				if(this.tag == "Villager")
                {
					GetComponent<Villager>().isGathering = false;
					target = Grid.GetMouseWorldPosition();
				}
                else
                {
					target = Grid.GetMouseWorldPosition();
				}
				
				

			}
			PathRequestManager.RequestPath(transform.position, target, OnPathFound);
		}
		
		
	}



    public void OnTriggerEnter(Collider collider)
    {
		
		if (collider.gameObject.layer == 11)
        {
			speed = 4;
        }
    }

	public void OnTriggerExit(Collider collider)
	{

		if (collider.gameObject.layer == 11)
		{
			speed = 20;
		}
	}

	
public void OnDrawGizmos()
	{
		if (path != null)
		{
			for (int i = targetIndex; i < path.Length; i++)
			{
				Gizmos.color = Color.black;
				Gizmos.DrawCube(path[i], Vector3.one);

				if (i == targetIndex)
				{
					Gizmos.DrawLine(transform.position, path[i]);
				}
				else
				{
					Gizmos.DrawLine(path[i - 1], path[i]);
				}
			}
		}
	}
}