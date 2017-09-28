//using System;
//
//namespace Mob
//{
//	public class FeatureExperiment : MobBehaviour
//	{
//		void Start(){
//			var s1 = Race.Create<Swordman> ("Races/Human/Swordman", (p1) => {
//				p1.tag = Constants.PLAYER1;
//				p1.name = Constants.PLAYER1;
//				p1.DefaultValue ();
//			});
//
//			var s2 = Race.Create<Swordman> ("Races/Human/Swordman", (p2) => {
//				p2.tag = Constants.PLAYER2;
//				p2.name = Constants.PLAYER2;
//				p2.DefaultValue ();
//			});
//
//			Affect.CreatePrimitive<Artifact1> (s1, new Race[]{ s1 });
//			s1.GetModule<SkillModule> (x => {
//				x.Use<SwordmanA1Skill> (new Race[]{ s2 });
//			});
//		}
//	}
//}
//
