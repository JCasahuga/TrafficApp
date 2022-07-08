using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointBehaviour : MonoBehaviour
{
    //Prefabs
    [SerializeField]
    private GameObject handlePref = null;
    [SerializeField]
    private GameObject curveRendPref = null;
    [SerializeField]
    private GameObject handleRendPref = null;
    [SerializeReference]
    private int roadType = 0; // 0 == Lane, 1 == Roundabout, 2 == Yield  

    //Generator
    private CubicCurvesGenerator cubCurGen;

    //Renderers
    public List<LineRenderer> curveRenderers = new List<LineRenderer> { };
    public List<LineRenderer> handleRenderers = new List<LineRenderer> { };

    //Storage
    public List<Transform> prevPoints;
    public List<Transform> nextPoints;

    public List<Transform> prevHandles;
    public List<Transform> nextHandles;

    private List<Transform> handles = new List<Transform> { };

    private float cLength = 0;

    //Activation bool of Rendering
    public bool isClicked;

    // ======= UNITY FUNCTIONS =======
    //Runned at the Start
    private void Awake() => cubCurGen = FindObjectOfType<CubicCurvesGenerator>();

    //Runned Every Frame
    private void Update()
    {
        if (isClicked)
        {
            DrawHandlesLines();
        }
    }


    // ======= OBJECT FUNCTIONS =======
    //Adding Points
    public void AddPoint(Transform p, bool prevPoint)
    {
        //Points
        if (prevPoint)
        {
            prevPoints.Add(p);
        }
        else
        {
            nextPoints.Add(p);
            curveRenderers.Add(Instantiate(curveRendPref, this.transform).GetComponent<LineRenderer>());
        }

        //Handles
        GameObject h = Instantiate(handlePref, this.transform);
        h.transform.position = transform.position + ((p.position - this.transform.position) * 0.25f);

        if (prevPoint)
        {
            prevHandles.Add(h.transform);
            h.GetComponent<ElementReference>().prevPoint = p;
        }
        else
        {
            nextHandles.Add(h.transform);
            h.GetComponent<ElementReference>().nextPoint = p;
        }
        handles.Add(h.transform);

        handleRenderers.Add(Instantiate(handleRendPref, this.transform).GetComponent<LineRenderer>());

        handleRenderers[handleRenderers.Count - 1].transform.parent = h.transform;

        DrawHandlesLines();
    }

    //Drawing Functions
    public void DrawCurves(int e)
    {
        if (curveRenderers.Count > 0)
        {
            for (int i = 0; i < curveRenderers.Count; i++)
            {
                cLength = cubCurGen.DrawCurve(PositionPoints(i), 0, curveRenderers[i]);
            }
        }

        if (prevPoints.Count > 0 && e != 1)
        {
            for (int i = 0; i < prevPoints.Count; i++)
            {
                prevPoints[i].GetComponent<PointBehaviour>().DrawCurves(1);
            }
        }
    }

    private void DrawHandlesLines()
    {
        for (int i = 0; i < handleRenderers.Count; i++)
        {
            handleRenderers[i].SetPosition(0, handles[i].transform.position);
            handleRenderers[i].SetPosition(1, this.transform.position);
        }
    }

    //Transforms to 3D Points
    private Vector3[] PositionPoints(int i)
    {
        List<Vector3> es = new List<Vector3> { };

        es.Add(this.transform.position);
        es.Add(nextHandles[i].position);
        es.Add(nextPoints[i].GetComponent<PointBehaviour>().prevHandles[nextPoints[i].GetComponent<PointBehaviour>().prevPoints.IndexOf(this.transform)].position);
        es.Add(nextPoints[i].position);

        return es.ToArray();
    }

    //Deletes the Point
    public void DeletePoint()
    {
        if (prevPoints.Count > 0)
        {
            for (int i = 0; i < prevPoints.Count; i++)
            {
                for (int e = 0; e < prevPoints[i].GetComponent<PointBehaviour>().handles.Count; e++)
                {
                    if (prevPoints[i].GetComponent<PointBehaviour>().handles[e].GetComponent<ElementReference>().nextPoint == this.transform)
                    {
                        Transform h = prevPoints[i].GetComponent<PointBehaviour>().handles[e];
                        prevPoints[i].GetComponent<PointBehaviour>().nextHandles.Remove(h);
                        prevPoints[i].GetComponent<PointBehaviour>().handleRenderers.Remove(prevPoints[i].GetComponent<PointBehaviour>().handles[e].GetChild(1).GetComponent<LineRenderer>());
                        Destroy(h.gameObject);
                        prevPoints[i].GetComponent<PointBehaviour>().handles.RemoveAt(e);


                        Destroy(prevPoints[i].GetComponent<PointBehaviour>().curveRenderers[prevPoints[i].GetComponent<PointBehaviour>().curveRenderers.Count - 1].gameObject);
                        prevPoints[i].GetComponent<PointBehaviour>().curveRenderers.RemoveAt(prevPoints[i].GetComponent<PointBehaviour>().curveRenderers.Count - 1);

                        prevPoints[i].GetComponent<PointBehaviour>().nextPoints.Remove(this.transform);

                        prevPoints[i].GetComponent<PointBehaviour>().DrawCurves(0);
                    }
                }
            }
        }

        if (nextPoints.Count > 0)
        {
            for (int i = 0; i < nextPoints.Count; i++)
            {
                for (int e = 0; e < nextPoints[i].GetComponent<PointBehaviour>().handles.Count; e++)
                {
                    if (nextPoints[i].GetComponent<PointBehaviour>().handles[e].GetComponent<ElementReference>().prevPoint == this.transform)
                    {
                        Transform h = nextPoints[i].GetComponent<PointBehaviour>().handles[e];
                        nextPoints[i].GetComponent<PointBehaviour>().prevHandles.Remove(h);
                        nextPoints[i].GetComponent<PointBehaviour>().handleRenderers.Remove(nextPoints[i].GetComponent<PointBehaviour>().handles[e].GetChild(1).GetComponent<LineRenderer>());
                        Destroy(h.gameObject);
                        nextPoints[i].GetComponent<PointBehaviour>().handles.RemoveAt(e);

                        nextPoints[i].GetComponent<PointBehaviour>().prevPoints.Remove(this.transform);

                        nextPoints[i].GetComponent<PointBehaviour>().DrawCurves(0);
                    }
                }
            }
        }

        Destroy(this.gameObject);
    }


    //Updates Visuals
    public void UpdateSize(float size)
    {
        transform.GetChild(0).localScale = new Vector2(size, size);
        transform.GetComponent<BoxCollider2D>().size = new Vector2(size * 0.3f, size * 0.3f);
        for (int i = 0; i < handles.Count; i++)
        {
            handles[i].transform.GetComponent<BoxCollider2D>().size = new Vector2(size * 0.6f, size * 0.6f);
            handles[i].transform.GetChild(0).localScale = new Vector2(size, size);
        }
        for (int i = 0; i < curveRenderers.Count; i++)
        {
            curveRenderers[i].transform.GetComponent<LineRenderer>().startWidth = size * 0.05f;
            curveRenderers[i].transform.GetComponent<LineRenderer>().endWidth = size * 0.05f;
            curveRenderers[i].transform.GetChild(0).transform.localScale = new Vector2(size, size);
        }
        for (int i = 0; i < handleRenderers.Count; i++)
        {
            handleRenderers[i].transform.GetComponent<LineRenderer>().startWidth = size * 0.05f;
            handleRenderers[i].transform.GetComponent<LineRenderer>().endWidth = size * 0.05f;
        }
    }


    //Sets Waypoint
    public Vector3[] GetNextWayPoints()
    {
        if (nextPoints.Count == 0) return null;
        return PositionPoints(Random.Range(0, nextPoints.Count));
    }

    public int GetRoadType()
    {
        return roadType;
    }

    public float GetLength() {
        return cLength;
    }

}
