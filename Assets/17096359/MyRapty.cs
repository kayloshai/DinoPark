using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Jason;

public class MyRapty : Agent
{
    public enum raptyState // Use if you want to create a single script FSM
    {
        IDLE,       // The default state on creation.
        EATING,     // This is for eating depending on location of a target object (killed prey)
        DRINKING,   // This is for Drinking, depending on y value of the object to denote water level
        ALERTED,      // This is for hightened awareness, such as looking around
        HUNTING,    // Moving with the intent to hunt
        ATTACKING,  // Causing damage to a specific target
        FLEEING,     // Running away from a specific target
        DEAD
    };
    bool ishunting = false;

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }
    IEnumerator delaySetState(float delay)
    {
         yield return new WaitForSeconds(delay);
         SetState(new Hunt(this, wander, flee, seek, face));
    }



    // Use this for initialization if you use the Animation Manager as your FSM
    protected override void Start()
    {
        

        m_fov = GetComponent<FieldOfView>();
        wander = GetComponent<Wander>();
        face = GetComponent<Face>();
        seek = GetComponent<Seek>();
        m_ainavigation = GetComponent<AINavigation>();
        
        base.Start();
    }
    
    protected override void Update() // Template for a single script FSM
    {

        if (!ishunting)
        {
            StartCoroutine("delaySetState", 1.0f);
            ishunting = true;
        }
        
        // Idle - should only be used at startup
        //StartCoroutine(state.Hunt());

        // Eating - requires a box collision with a dead dino

        // Drinking - requires y value to be below 32 (?)

        // Alerted - up to the student what you do here

        // Hunting - up to the student what you do here

        // Fleeing - up to the student what you do here

        // Dead - If the animal is being eaten, reduce its 'health' until it is consumed

        Debug.Log(m_fov.visibleTargets.Count);
        Debug.Log(m_fov.stereoVisibleTargets.Count);

        //a* trigger
        if (m_pathfound && pathFindingCD <= 0) {
            pathFindingCD = 4.0f;
            foreach (Transform t in m_fov.visibleTargets) {
                if (t.gameObject.GetComponent<MyAnky>()) {
                    m_ainavigation.StartFindPath(this.transform.position, t.position);
                }
            }
            counter = 0;
        }

        if (pathFindingCD > 0) pathFindingCD -= Time.deltaTime;

        if (m_ainavigation.path.Count > 0 && m_pathfound) {
            int maxCount = m_ainavigation.path.Count;
            if (Vector3.Distance(transform.position, m_ainavigation.path[maxCount - 1].worldPosition) > 1.0f) {
                seek.target.transform.position = m_ainavigation.path[counter].worldPosition;
                if (Vector3.Distance(transform.position, m_ainavigation.path[counter].worldPosition) < 5.0f)
                    counter++;
            }
        }


        base.Update();
    }

    protected override void LateUpdate()
    {

       

        base.LateUpdate();
    }
}
