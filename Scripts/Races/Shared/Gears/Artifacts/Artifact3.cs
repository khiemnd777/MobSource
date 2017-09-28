using UnityEngine;

namespace Mob
{
	public class Artifact3 : Affect
	{
		public override void Execute ()
		{
			own.GetModule<HealthPowerModule> (x => {
				x.AddHp(100f);
			});

			own.GetModule<StatModule> (x => {
				x.extraRegenerateHp += 5f;
				x.Calculate2ndPoint(StatType.Vitality);
			});
		}

		public override void Disuse ()
		{
			own.GetModule<HealthPowerModule> (x => {
				x.SubtractHp(100f);
			});

			own.GetModule<StatModule> (x => {
				x.extraRegenerateHp -= 5f;
				x.Calculate2ndPoint(StatType.Vitality);
			});
		}

		bool isRegenHp;

		void Update(){
			if (isRegenHp)
				return;
			ExecuteInTurn (own, () => {
				own.GetModule<HealthPowerModule>(x => {
					if(Mathf.Clamp(x.hp, x.maxHp * .3f, x.maxHp) == x.maxHp * .3f){
						own.GetModule<StatModule> (s => {
							s.extraRegenerateHp *= 2f;
						});
						isRegenHp = true;
					}
				});
			});
		}
	}

	public class Artifact3Item: Item, ISelfUsable
	{
		public override bool Use (Race[] targets)
		{
			if (Affect.HasAffect<Artifact3> (own))
				return false;

			Affect.CreatePrimitiveAndUse<Artifact3> (own, targets);
			return true;
		}

		public override bool Disuse ()
		{
			Affect.GetAffects<Artifact3> (own, x => {
				x.Disuse();
				Destroy(x.gameObject);
			});
			return true;
		}
	}
}

