using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubicCurvesGenerator : MonoBehaviour
{
    [SerializeField]
    [Range(2,64)]
    private int iterations = 32;

    [SerializeField]
    GameObject dirRendPref = null;

    private Vector3[] positions;

    // ======= OBJECT FUNCTIONS =======
    //Sets points of curve and draws it
    public float DrawCurve(Vector3[] elements, int nPoints, LineRenderer line)
    {
        List<Vector3> totalPositions = new List<Vector3> { };
        positions = new Vector3[iterations + 1];
        float distance = 0;

        for (int i = 0; i <= iterations; i++)
        {
            float t = (float)i / (float)iterations;
            positions[i] = CalculateQuadraticBezierPoint(t, elements);
            
            if (i != 0) distance += Vector3.Distance(positions[i-1], positions[i]);
            else distance += Vector3.Distance(positions[i], elements[0]);
        }
        distance += Vector3.Distance(elements[3], positions[iterations]);

        totalPositions.AddRange(positions);

        line.positionCount = totalPositions.Count;
        line.SetPositions(totalPositions.ToArray());

        if (line.transform.childCount == 0)
        {
            Instantiate(dirRendPref, CalculateQuadraticBezierPoint(0.5f, elements), Quaternion.Euler(0, 0, -90)).transform.parent = line.transform;
        }

        Vector2 dir = CalculateQuadraticBezierPoint(0.6f, elements) - CalculateQuadraticBezierPoint(0.4f, elements);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion rot = Quaternion.AngleAxis(angle, Vector3.forward);

        line.transform.GetChild(0).transform.position = CalculateQuadraticBezierPoint(0.5f, elements);
        line.transform.GetChild(0).transform.rotation = rot;

        return distance;
    }

    //Calculates position of curve in reference of points and time
    public Vector3 CalculateQuadraticBezierPoint(float t, Vector3[] ps)
    {
        float u = 1 - t;
        float uu = u * u;
        float uuu = uu * u;
        float tt = t * t;
        float ttt = tt * t;

        Vector3 p = (uuu * ps[0]) + (3 * uu * t * ps[1]) + (3 * u * tt * ps[2]) + (ttt * ps[3]);

        return p;
    }

}
