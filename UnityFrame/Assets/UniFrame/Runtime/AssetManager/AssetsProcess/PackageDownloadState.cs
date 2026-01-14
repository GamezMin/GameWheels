using Cysharp.Threading.Tasks;
using YooAsset;

namespace GameFrame.Runtime
{
    public class PackageDownloadState : FsmState
    {
        public override void OnEnter(FsmController fsmController)
        {
            base.OnEnter(fsmController);
            BeginDownload().Forget();
        }

        private async UniTask BeginDownload()
        {
            var downloader = (ResourceDownloaderOperation) GetData("downloader");
            downloader.DownloadErrorCallback = (errData) => { Debugger.Log($"{errData.FileName}下载失败 :error:{errData.ErrorInfo}"); };
            downloader.DownloadUpdateCallback = (downloadData) =>
            {
                
                Debugger.Log($"{downloadData.TotalDownloadCount}/{downloadData.CurrentDownloadCount}  " +
                             $"|  {downloadData.TotalDownloadBytes}/{downloadData.CurrentDownloadBytes}");
            };

            downloader.BeginDownload();
            await downloader.ToUniTask();
            // 检测下载结果
            if (downloader.Status != EOperationStatus.Succeed)
                return;
            var packageName = (string) GetData("packageName");
            var package = YooAssets.GetPackage(packageName);
            var operation = package.ClearCacheFilesAsync(EFileClearMode.ClearUnusedBundleFiles);
            operation.Completed += Operation_Completed;
        }

        private void Operation_Completed(YooAsset.AsyncOperationBase obj)
        {
            ChangeState<PackageDoneState>();
        }
    }
}