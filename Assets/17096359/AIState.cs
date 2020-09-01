using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jason {
    public abstract class AIState : MonoBehaviour
    {
        //All states that will be used at this time of implementation, any new states can be added here in future.
        protected Agent agent;
        protected Wander wander;
        protected Flee flee;
        protected Seek seek;
        protected Face face;

        public AIState(Agent _agent, Wander _wander, Flee _flee, Seek _seek, Face _face) {
            agent = _agent;
            wander = _wander;
            flee = _flee;
            seek = _seek;
            face = _face;
        }
        public virtual IEnumerator Start() { yield break; }
        public virtual IEnumerator Idle() { yield break; }
        public virtual IEnumerator Eat() { yield break; }
        public virtual IEnumerator Drink() { yield break; }
        public virtual IEnumerator Alert() { yield break; }
        public virtual IEnumerator Hunt() { yield break; }
        public virtual IEnumerator Attack() { yield break; }
        public virtual IEnumerator Flee() { yield break; }
        public virtual IEnumerator Dead() { yield break; }
    }
}
