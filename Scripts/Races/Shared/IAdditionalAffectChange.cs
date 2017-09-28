
namespace Mob
{
	public interface IAdditionalAffectChange<T> where T:Affect
	{
		float AdditionalAffectChange(T affect, Race target);
		void DefineAffect(T affect);
		bool UseDefineAffect(T affect);
	}
}

