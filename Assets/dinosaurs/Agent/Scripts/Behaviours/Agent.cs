using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Jason;

public class Agent : AIController
{
    public bool blendWeight = false;
    public bool blendPriority = false;
    public float priorityThreshold = 0.2f;
    public bool blendPipeline = false;
    public float maxSpeed;
    public float maxAccel;
    public float maxRotation;
    public float maxAngularAccel;
    public float orientation;
    public float rotation;
    public Vector3 velocity;
    protected Steering steering;
    private Dictionary<int, List<Steering>> groups;


    //Added to Agent class
    [HideInInspector]
    public FieldOfView m_fov;
    [HideInInspector]
    public bool m_foundDeadAgent;
    [HideInInspector]
    public Agent m_foundAgent;
    [HideInInspector]
    public AINavigation m_ainavigation;//reference to AiNavigation class
    [HideInInspector]
    public bool m_pathfound = false;
    //CD will be used for Cooldown.
    [HideInInspector]
    protected float pathFindingCD = 0.0f;
    protected int counter = 0;
    protected float drinkCD = 0.0f;
    protected float eatCD = 0.0f;
    protected Wander wander = null;
    protected Flee flee = null;
    protected Seek seek = null;
    protected Pursue pursue = null;
    protected Face face = null;

    public float hitPoints;//Health for agent
    public float damageDone;//Damage done by agent

    protected virtual void Start() // Changed so we can inherit
    {
        velocity = Vector3.zero;
        steering = new Steering();
        groups = new Dictionary<int, List<Steering>>();
        m_foundDeadAgent = false;
        m_foundAgent = null;
    }
    protected virtual void Update() // Changed so we can inherit
    {
        Vector3 displacement = velocity * Time.deltaTime;
        orientation += rotation * Time.deltaTime;
        if (orientation < 0.0f)
            orientation += 360.0f;
        else if (orientation > 360.0f)
            orientation -= 360.0f;
        transform.Translate(displacement, Space.World);
        transform.rotation = new Quaternion();
        transform.Rotate(Vector3.up, orientation);
    }
    protected virtual void LateUpdate() // Changed so we can inherit
    {
        if (blendPriority)
        {
            steering = GetPrioritySteering();
            groups.Clear();
        }
        velocity += steering.linear * Time.deltaTime;
        rotation += steering.angular * Time.deltaTime;
        if (velocity.magnitude > maxSpeed)
        {
            velocity.Normalize();
            velocity = velocity * maxSpeed;
        }
        if (rotation > maxRotation)
        {
            rotation = maxRotation;
        }
        if (steering.angular == 0.0f)
        {
            rotation = 0.0f;
        }
        if (steering.linear.sqrMagnitude == 0.0f)
        {
            velocity = Vector3.zero;
        }
        steering = new Steering();
    }
    public void SetSteering(Steering steering)
    {
        this.steering = steering;
    }

    public void SetSteering(Steering steering, float weight)
    {
        this.steering.linear += (weight * steering.linear);
        this.steering.angular += (weight * steering.angular);
    }
    public void SetSteering(Steering steering, int priority)
    {
        if (!groups.ContainsKey(priority))
        {
            groups.Add(priority, new List<Steering>());
        }
        groups[priority].Add(steering);
    }
    public void SetSteering(Steering steering, bool pipeline)
    {
        if (!pipeline)
        {
            this.steering = steering;
            return;
        }
    }
    private Steering GetPrioritySteering()
    {
        Steering steering = new Steering();
        float sqrThreshold = priorityThreshold * priorityThreshold;
        List<int> gIdList = new List<int>(groups.Keys);
        gIdList.Sort();
        foreach (int gid in gIdList)
        {
            steering = new Steering();
            foreach (Steering singleSteering in groups[gid])
            {
                steering.linear += singleSteering.linear;
                steering.angular += singleSteering.angular;
            }
            if (steering.linear.magnitude > priorityThreshold ||
                    Mathf.Abs(steering.angular) > priorityThreshold)
            {
                return steering;
            }
        }
        return steering;
    }

    protected virtual bool checkFOVThreats(){
        //look for agent in the visible targets, if found return true
        foreach (Transform transform in m_fov.visibleTargets) {
            if (transform.gameObject.GetComponent < Agent>()) { return true; }
        }
        //look for agent in the stereo visible targetss, if found return true
        foreach (Transform transform in m_fov.stereoVisibleTargets) {
            if (transform.gameObject.GetComponent<Agent>()) { return true; }
        }
        //if not found return false
        return false;
    }

    //protected virtual void OnCollisionEnter(Collision col) {
    //    m_foundAgent = col.gameObject.GetComponent<Agent>();
    //    if (!m_foundAgent) return;
    //    if (m_foundAgent.GetState().GetType() == typeof(Dead)) { m_foundDeadAgent = true; }
    //}
}
