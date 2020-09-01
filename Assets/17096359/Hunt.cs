using System.Collections;
using UnityEngine;

namespace Jason
{
    public class Hunt : AIState{
        public Hunt(Agent _agent, Wander _wander, Flee _flee, Seek _seek, Face _face) : base(_agent, _wander, _flee, _seek, _face) { }
        public override IEnumerator Start()
        {
            //seek.enabled = true;
            //face.enabled = true;
            Debug.Log(agent.m_fov.visibleTargets.Count);
            Debug.Log(agent.m_fov.stereoVisibleTargets.Count);

            //Face the target, based on the visual from the FOV.
            if (agent is MyAnky) {
                if (face) {
                    foreach (Transform t in agent.m_fov.stereoVisibleTargets) {
                        face.target = t.gameObject;
                    }
                }
                if (agent.m_fov.stereoVisibleTargets.Count > 0) agent.m_pathfound = true;
            }

            //Face the target, based on the visual from the FOV.
            if (agent is MyRapty)
            {
                if (face)
                {
                    foreach (Transform t in agent.m_fov.visibleTargets)
                    {
                        if (t.gameObject.GetComponent<MyRapty>())
                            face.target = t.gameObject;
                    }
                }
                
                if (agent.m_fov.stereoVisibleTargets.Count > 0) {
                    agent.m_pathfound = true;
                }
            }

            yield return new WaitForSecondsRealtime(Random.Range(1.0f, 10.0f));
        }

        //if i do attack set new state on ienumerator attack()
    }
}
