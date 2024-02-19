using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierArrows : MonoBehaviour
{
    public GameObject arrowHeadPrefab;
    public GameObject arrowNodePrefab;
    public int arrowNodeNum;
    public float scaleFactor=1f;

    private RectTransform origin;
    private List<RectTransform> arrowNodes=new();
    private List<Vector2> controlPoints=new();
    private readonly List<Vector2> controlPointFactors=new(){new Vector2(-0.3f,0.8f),new Vector2(0.1f,1.4f)};

    private void Start()
    {
        origin=GetComponent<RectTransform>();
        for(int i=0;i<arrowNodeNum;i++)
        {
            arrowNodes.Add(Instantiate(arrowNodePrefab,transform).GetComponent<RectTransform>());
        }

        arrowNodes.Add(Instantiate(arrowHeadPrefab,transform).GetComponent<RectTransform>());
        arrowNodes.ForEach(a=>a.GetComponent<RectTransform>().position=new Vector3(-1000,-1000));

        for(int i=0;i<4;i++)
        {
            controlPoints.Add(Vector2.zero);
        }
    }

    private void Update()
    {
        controlPoints[0]=new Vector2(origin.position.x,origin.position.y);
        controlPoints[3]=new Vector2(Input.mousePosition.x,Input.mousePosition.y);

        controlPoints[1]=controlPoints[0]+(controlPoints[3]-controlPoints[0])*controlPointFactors[0];
        controlPoints[2]=controlPoints[0]+(controlPoints[3]-controlPoints[0])*controlPointFactors[1];

        for(int i=0;i<arrowNodes.Count;i++)
        {
            var t=Mathf.Log(1f*i/(arrowNodes.Count-1)+1f,2f);

            arrowNodes[i].position=
                Mathf.Pow(1-t,3)*controlPoints[0]+
                3*Mathf.Pow(1-t,2)*t*controlPoints[1]+
                3*(1-t)*Mathf.Pow(t,2)*controlPoints[2]+
                Mathf.Pow(t,3)*controlPoints[3];

            if(i>0)
            {
                var euler=new Vector3(0,0,Vector2.SignedAngle(Vector2.up,arrowNodes[i].position-arrowNodes[i-1].position));
                arrowNodes[i].rotation=Quaternion.Euler(euler);
            }

            var scale=scaleFactor*(1f-0.03f*(arrowNodes.Count-1-i));
            arrowNodes[i].localScale=new Vector3(scale,scale,1f);
        }

        arrowNodes[0].transform.rotation=arrowNodes[1].transform.rotation;
    }
}
