using UnityEngine;

namespace Jason {
    public abstract class AIController : MonoBehaviour
    {
        protected AIState state;

        //function to set the state
        public void SetState(AIState _newAIState) {
            state = _newAIState;
            StartCoroutine(state.Start());
        }
        //function to resturn the state
        public AIState getState() { return state; }
    }
}
    
