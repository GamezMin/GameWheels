using GameFrame.Runtime;

namespace FrameDemo
{
    public class GameStartState : FsmState
    {
        public override void OnEnter(FsmController fsmController)
        {
            base.OnEnter(fsmController);
            SceneFactory.ChangePlayerScene<GameScene>();
            GameFrame.Runtime.GameFrame.Instance.RemoveFsmComponents(fsmController);
        }
    }
}