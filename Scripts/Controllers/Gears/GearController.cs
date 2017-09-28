using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mob
{
	public class GearController : MonoBehaviour 
	{
		public ScrollableList list;
		public GearListBuyItem listBuyItemResource;
		public GearListUpgradeItem listUpgradeResource;

		public Image helm;
		public Image armor;
		public Image weapon;
		public Image cloth;
		public Image ring;
		public Image artifact;

		public GearBoughtItem[] availableItems;

		public Race _character;
		GearModule _gearModule;
//		Sprite _none;

		void Start(){
			list.ClearAll ();

//			_none = Resources.Load<Sprite> ("Sprites/none");

			helm.sprite = Resources.Load<Sprite> ("Sprites/buy-helm");
			armor.sprite = Resources.Load<Sprite> ("Sprites/buy-armor");
			weapon.sprite = Resources.Load<Sprite> ("Sprites/buy-weapon");
			cloth.sprite = Resources.Load<Sprite> ("Sprites/buy-cloth");
			ring.sprite = Resources.Load<Sprite> ("Sprites/buy-ring");
			artifact.sprite = Resources.Load<Sprite> ("Sprites/none");

			EventManager.StartListening (Constants.EVENT_BOUGHT_GEAR, new Action<GearType, uint>((gearType, ownNetId) => {
				if(!TryToConnect())
					return;
				if(!_character.netId.Value.Equals(ownNetId))
					return;
				FilterItemsByType(gearType);
				Wear(gearType);
			}));

			EventManager.StartListening (Constants.EVENT_UPGRADED_GEAR, new Action<GearType, uint>((gearType, ownNetId) => {
				if(!TryToConnect())
					return;
				if(!_character.netId.Value.Equals(ownNetId))
					return;
				Wear(gearType);
			}));
		}

		void Update(){
			if (!TryToConnect ())
				return;
		}

		bool TryToConnect(){
			return NetworkHelper.instance.TryToConnect (() => {
				if(!_character.IsNull() && !_gearModule.IsNull())
					return true;
				_character = Race.GetLocalCharacter();
				if(_character.IsNull())
					return false;
				_gearModule = _character.GetModule<GearModule>();
				return false;
			});
		}

		public void Wear(GearType gearType){
			switch(gearType){
			case GearType.Helm:
				WearHelm();
				break;
			case GearType.Armor:
				WearArmor();
				break;
			case GearType.Artifact:
				WearArtifact();
				break;
			case GearType.Cloth:
				WearCloth();
				break;
			case GearType.Ring:
				WearRing();
				break;
			case GearType.Weapon:
				WearWeapon();
				break;
			default:
				break;
			}	
		}

		public void WearHelm(){
			if (!_gearModule.syncGear.Any (x => x.gearType == GearType.Helm))
				return;
			var o = _gearModule.syncGear.FirstOrDefault (x => x.gearType == GearType.Helm);
			helm.sprite = IconHelper.instance.GetIcon (o.icon);
		}

		public void WearArmor(){
			if (!_gearModule.syncGear.Any (x => x.gearType == GearType.Armor))
				return;
			var o = _gearModule.syncGear.FirstOrDefault (x => x.gearType == GearType.Armor);
			armor.sprite = IconHelper.instance.GetIcon (o.icon);
		}

		public void WearWeapon(){
			if (!_gearModule.syncGear.Any (x => x.gearType == GearType.Weapon))
				return;
			var o = _gearModule.syncGear.FirstOrDefault (x => x.gearType == GearType.Weapon);
			weapon.sprite = IconHelper.instance.GetIcon (o.icon);
		}

		public void WearCloth(){
			if (!_gearModule.syncGear.Any (x => x.gearType == GearType.Cloth))
				return;
			var o = _gearModule.syncGear.FirstOrDefault (x => x.gearType == GearType.Cloth);
			cloth.sprite = IconHelper.instance.GetIcon (o.icon);
		}

		public void WearRing(){
			if (!_gearModule.syncGear.Any (x => x.gearType == GearType.Ring))
				return;
			var o = _gearModule.syncGear.FirstOrDefault (x => x.gearType == GearType.Ring);
			ring.sprite = IconHelper.instance.GetIcon (o.icon);
		}

		public void WearArtifact(){
			if (!_gearModule.syncGear.Any (x => x.gearType == GearType.Artifact))
				return;
			var o = _gearModule.syncGear.FirstOrDefault (x => x.gearType == GearType.Artifact);
			artifact.sprite = IconHelper.instance.GetIcon (o.icon);
		}

		public void FilterItemsByType(GearType gearType){
			list.ClearAll ();
			var syncAvailableItems = _gearModule.GetSyncAvailableGearsByType (gearType);
			var syncOwnItems = _gearModule.GetOwnSyncItemByType (gearType);
			foreach (var item in syncOwnItems) {
				var gearSLI = Instantiate<GearListUpgradeItem> (listUpgradeResource, list.transform);
				gearSLI.item = (SyncGearItem)item;
				gearSLI.gearType = gearType;
				gearSLI.gearController = this;
				gearSLI.Prepare ();
			}
			foreach (var item in syncAvailableItems) {
				var gearSLI = Instantiate<GearListBuyItem> (listBuyItemResource, list.transform);
				gearSLI.boughtItem = (SyncGearBoughtItem)item;
				gearSLI.gearType = gearType;
				gearSLI.gearController = this;
				gearSLI.Prepare ();
			}
			list.Refresh ();
		}

//		void Update(){
//			WearHelm ();
//			WearArmor ();
//			WearCloth ();
//			WearWeapon ();
//			WearRing ();
//			WearArtifact ();
//		}
	}	
}