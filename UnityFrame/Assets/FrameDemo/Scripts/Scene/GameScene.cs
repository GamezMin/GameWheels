using GameFrame.Runtime;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace FrameDemo
{
    public class GameScene : SceneBase
    {
        protected override string SingleSceneName => "World";

        protected override void OnReady()
        {
            SetCamera();
            //这里是走ecs路线
            // AddComponent<ECSGameWorld, int>(AllComponents.TotalComponents);
            //这里是走ecc路线
            //AddComponent<ECCGameWorld, int>(AllComponents.TotalComponents);
            QualitySettings.vSyncCount = 0;
            // EventSend.Instance
        }

        private void SetCamera()
        {
            var uiCamera = GameObject.Find("UICamera").GetComponent<Camera>();
            uiCamera.GetUniversalAdditionalCameraData().renderType = CameraRenderType.Overlay;
            
            var worldCamera =GameObject.Find("MainCamera").GetComponent<Camera>();
            var cameraData = worldCamera.GetUniversalAdditionalCameraData();
            cameraData.cameraStack.Add(uiCamera);
        }


        public override void Dispose()
        {
            base.Dispose();
        }
    }
}