
namespace Mob
{
	public abstract class PluginHandler : MobBehaviour
	{
		public virtual void InitPlugin(){
			
		}
		public abstract void HandlePlugin(params object[] args);
	}
}

