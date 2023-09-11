using ZLC.Application;
using ZLC.Common;
namespace ZLC.ResourceSystem
{
    /// <summary>
    /// 基于Addressables的默认游戏启动器
    /// 默认包含加载器
    /// </summary>
    public class DefaultAddressablesGameLauncher : AAppLauncher
    {
        /// <inheritdoc cref="AAppLauncher"/>>
        protected override void RegisterManagers()
        {
            
        }
        
        /// <inheritdoc cref="AAppLauncher"/>>
        protected override IEnumerator<ILoader> InitPreloaders()
        {
            return null;
        }
    }
}