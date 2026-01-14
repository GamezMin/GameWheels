using GameFrame.Runtime;

namespace FrameDemo
{
    public class GameInitState : FsmState
    {
        public override void OnEnter(FsmController fsmController)
        {
            base.OnEnter(fsmController);
            // Config.Instance.LoadTable();
            fsmController.ChangeState<GameStartState>();
        }
    }
}