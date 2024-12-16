using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiTwo : MonoBehaviour
{
    private RaycastHit  hit;
    private NavMeshAgent agent;
    public Vector3 EndPos;
    public LineRenderer line;



    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();


    }


    private void Start()
    {

    }


    private void Update()
    { 
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit);
            if (hit.transform != null)
            {
                EndPos = hit.point;
                //划线
            }
        }

        if (EndPos!=null)
        {
            agent.SetDestination(EndPos);
            //划线
            line.positionCount = agent.path.corners.Length;
            line.SetPositions (agent.path.corners);

        }

    }

}
