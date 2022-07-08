using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class CircuitAgent : Agent
{


    [SerializeField]
    private DetectorsBehaviour frontSensor;
    [SerializeField]
    private DetectorsBehaviour yieldSensorLeft;
    [SerializeField]
    private DetectorsBehaviour incorpSensorLeft;

    private VehicleBehaviour vehicleBehaviour;

    public float maxSpeed = 120f;

    public float dCarFront = 2.25f;
    public float maxFrontDist = 2.25f;
    public float sCarFront = 120f;
    public int frontTypeRoad = 4;

    public float dCarYield = 3f;
    public float maxYieldDist = 3f;
    public float sCarYield = 120f;
    public int yieldTypeRoad = 4;

    public float dCarIncorp = 5f;
    public float maxIncorpDist = 5f;
    public float sCarIncorp = 120f;
    public int incorpTypeRoad = 4;

    public int roadType = 0;

    private Vector3 iniPos;

    public void Awake()
    {
        vehicleBehaviour = GetComponent<VehicleBehaviour>();
        iniPos = transform.localPosition;
    }

    public override void OnEpisodeBegin()
    {
        StopAllCoroutines();
        transform.localPosition = iniPos;
        vehicleBehaviour.currentSpeed = Random.Range(35f, 45f);
        vehicleBehaviour.distance = 0;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        if (frontSensor.closestVehicle != null) {
            dCarFront = Vector3.Distance(frontSensor.closestVehicle.position, transform.position);
            sCarFront = frontSensor.closestVehicle.GetComponent<VehicleBehaviour>().currentSpeed;
            frontTypeRoad = frontSensor.closestVehicle.GetComponent<VehicleBehaviour>().roadType;
        }
        else {
            dCarFront = maxFrontDist;
            sCarFront = 120f;
            frontTypeRoad = 4;
        }

        if (yieldSensorLeft.closestVehicle != null)
        {
            dCarYield = Vector3.Distance(yieldSensorLeft.closestVehicle.position, transform.position);
            sCarYield = yieldSensorLeft.closestVehicle.GetComponent<VehicleBehaviour>().currentSpeed;
            yieldTypeRoad = yieldSensorLeft.closestVehicle.GetComponent<VehicleBehaviour>().roadType;
        }
        else
        {
            dCarYield = maxYieldDist;
            sCarYield = 120f;
            yieldTypeRoad = 4;
        }

        if (incorpSensorLeft.closestVehicle != null)
        {
            dCarIncorp = Vector3.Distance(incorpSensorLeft.closestVehicle.position, transform.position);
            sCarIncorp = incorpSensorLeft.closestVehicle.GetComponent<VehicleBehaviour>().currentSpeed;
            incorpTypeRoad = incorpSensorLeft.closestVehicle.GetComponent<VehicleBehaviour>().roadType;
        }
        else
        {
            dCarIncorp = maxIncorpDist;
            sCarIncorp = 120f;
            incorpTypeRoad = 4;
        }

        sensor.AddObservation(dCarFront / maxFrontDist);
        sensor.AddObservation(sCarFront / maxSpeed);
        //sensor.AddOneHotObservation(roadType, 5);

        //sensor.AddObservation(dCarYield / maxYieldDist);
        //sensor.AddObservation(sCarYield / maxSpeed);
        //sensor.AddOneHotObservation(roadType, 5);

        //sensor.AddObservation(dCarIncorp / maxIncorpDist);
        //sensor.AddObservation(sCarIncorp / maxSpeed);
        //sensor.AddOneHotObservation(roadType, 5);

        sensor.AddObservation(vehicleBehaviour.currentSpeed / maxSpeed);

        //sensor.AddOneHotObservation(roadType, 4);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        int brake = actions.DiscreteActions[0];

        if (brake > 0.5f) vehicleBehaviour.brake = true;
        else vehicleBehaviour.brake = false;

        AddReward(-1f / MaxStep);

    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
        if (Input.GetKey(KeyCode.P)) discreteActions[0] = 1;
        else discreteActions[0] = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PointBehaviour>())
        {
            if (collision.gameObject.GetComponent<PointBehaviour>().nextHandles.Count == 0)
            {
                Debug.Log("Success");
                //SetReward(1f - accumulatedTimePenalty);
                AddReward(1f);
                //EndEpisode();
            }
        }
        else if (collision.gameObject.GetComponent<VehicleBehaviour>())
        {
            Debug.Log("Failure");
            //SetReward(-1f);
            AddReward(-5f);
            EndEpisode();
        }
    }

}
