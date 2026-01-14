using GameFrame.Runtime;

namespace FrameDemo
{
    public class GameProcess : FsmController
    {
        public override void OnInitialize()
        {
            base.OnInitialize();
            AddState<GameInitState>();
            AddState<GameStartState>();
            ChangeState<GameInitState>();
        }
    }
}