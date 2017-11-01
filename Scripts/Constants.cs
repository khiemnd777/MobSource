using System;

namespace Mob
{
	public class Constants
	{
		public const float PRICE_UP_TO = 1.05f;
		public const int HUMAN_MAX_LEVEL = 16;
		public const int ORC_MAX_LEVEL = 16;
		public const int MIN_LEVEL = 2;
		public const int INIT_LEVEL = 1;
		public const float INIT_GAIN_POINT = 0f;
		public const float LERP_TIME = .5f;
		public const float WAIT_FOR_DESTROY = 10f;
		public const float TIME_EFFECT_END_DEFAULT = 3f;
		public const string TARGET_HP_LABEL = "Canvas/EnemyPanel/BodyPanel/TargetHPValue";
		public const string ATTACKER_HP_LABEL = "Canvas/CharacterInfoPanel/HPValue";
		public const string ATTACKER_GOLD_LABEL = "Canvas/CharacterInfoPanel/GoldValue";
		public const string ATTACKER_ENERGY_LABEL = "Canvas/CharacterInfoPanel/EnergyValue";
		public const string ATTACKER_GAIN_POINT_LABEL = "Canvas/CharacterInfoPanel/GainPointValue";
		public const string DECREASE_LABEL = "Prefabs/DecreaseLabel";
		public const string INCREASE_LABEL = "Prefabs/IncreaseLabel";
		public const string EFFECT_SLASH_LINE = "Effects/SlashLine";
		public const string PLAYER1 = "Player1";
		public const string PLAYER2 = "Player2";
		public const string PLAYER3 = "Player3";
		public const string PLAYER4 = "Player4";
		public const string PLAYER5 = "Player5";
		public const string PLAYER6 = "Player6";
		public const string PLAYER7 = "Player7";
		public const string PLAYER8 = "Player8"; 

		// Battle player
		public const string LOCAL_PLAYER = "LocalPlayer";
		public const string LOCAL_CHARACTER = "LocalCharacter";
		public const string SERVER_PLAYER = "Player";
		public const string OPPONENT_PLAYER = "OpponentPlayer";
		public const string OPPONENT_CHARACTER = "OpponentCharacter";

		// Event names
		public const string EVENT_LEVEL_UP_CLIENT = "level-up-client-fired";
		public const string EVENT_BOUGHT_ITEM_FROM_SHOP = "shop-bought-fired";
		public const string EVENT_BOUGHT_GEAR = "gear-bought-fired";
		public const string EVENT_UPGRADED_GEAR = "gear-upgraded-fired";
		public const string EVENT_ITEM_BOUGHT_FIRED = "item_bought_fired";
		public const string EVENT_ITEM_OVER_IN_BAG = "item_over_in_bag";
		public const string EVENT_SKILL_PICKED = "skill-picked";
		public const string EVENT_SKILL_LEARNED = "skill-learned";
		public const string EVENT_TAB_CONTENT_FIRED = "fire-tab-content";
		public const string EVENT_TAB_VIEW_INIT = "init-tab-view";
		public const string EVENT_RETURN_STAT_VALUE = "return-stat-value";
		public const string EVENT_REFRESH_GEAR_STAT = "refresh-gear-stat";
		public const string EVENT_REFRESH_SYNC_SKILLS = "refresh-sync-skills";
		public const string EVENT_REFRESH_SYNC_AVAILABLE_SKILLS = "refresh-sync-available-skills";
		public const string EVENT_REFRESH_SYNC_LEVEL = "refresh-sync-level";
		public const string EVENT_REFRESH_SYNC_UP_LEVEL = "refresh-sync-up-level";
		public const string EVENT_REFRESH_SYNC_HP = "refresh-sync-hp";
		public const string EVENT_CLIENT_ADD_HP = "client-add-hp";
		public const string EVENT_CLIENT_SUBTRACT_HP = "client-subtract-hp";
		public const string EVENT_REFRESH_SYNC_ENERGY = "refresh-sync-energy";
		public const string EVENT_REFRESH_SYNC_GOLD = "refresh-sync-gold";
		public const string EVENT_GOLD_DICE_ROLLED = "gold-dice-rolled";
		public const string EVENT_ENERGY_DICE_ROLLED = "energy-dice-rolled";
		public const string EVENT_GOLD_CHANGED = "gold-changed";
		public const string EVENT_ENERGY_CHANGED = "energy-changed";
		public const string EVENT_REFRESH_SYNC_GEARS_BY_TYPE = "refresh-sync-gears-by-type";
		public const string EVENT_REFRESH_SYNC_AVAILABLE_GEARS_BY_TYPE = "refresh-sync-available-gears-by-type";
		public const string EVENT_REFRESH_SYNC_GEARS = "refresh-sync-gears";
		public const string EVENT_REFRESH_SYNC_AVAILABLE_GEARS = "refresh-sync-available-gears";
		public const string EVENT_REFRESH_SYNC_SHOP_ITEMS = "refresh-sync-shop-items";
		public const string EVENT_REFRESH_SYNC_BAG_ITEMS = "refres-sync-bag-items";
		public const string EVENT_HP_SUBTRACTING_EFFECT = "hp-subtracting-effect";
		public const string EVENT_HP_ADDING_EFFECT = "hp-adding-effect";
		public const string EVENT_ENERGY_SUBTRACTING_EFFECT = "energy-subtracting-effect";
		public const string EVENT_ENERGY_ADDING_EFFECT = "energy-adding-effect";
		public const string EVENT_GOLD_SUBTRACTING_EFFECT = "gold-subtracting-effect";
		public const string EVENT_GOLD_ADDING_EFFECT = "gold-adding-effect";
		public const string EVENT_STAT_POINT_CHANGED = "stat-point-changed";
		public const string EVENT_STAT_STRENGTH_CHANGED = "stat-strength-changed";
		public const string EVENT_STAT_PHYSICAL_ATTACK_CHANGED = "stat-physical-attack-changed";
		public const string EVENT_STAT_PHYSICAL_DEFEND_CHANGED = "stat-physical-defend-changed";
		public const string EVENT_STAT_DEXTERITY_CHANGED = "stat-dexterity-changed";
		public const string EVENT_STAT_ATTACK_RATING_CHANGED = "stat-attack-rating-changed";
		public const string EVENT_STAT_CRITICAL_RATING_CHANGED = "stat-critical-rating-changed";
		public const string EVENT_STAT_INTELLIGENT_CHANGED = "stat-intelligent-changed";
		public const string EVENT_STAT_MAGIC_ATTACK_CHANGED = "stat-magic-attack-changed";
		public const string EVENT_STAT_MAGIC_RESIST_CHANGED = "stat-magic-resist-changed";
		public const string EVENT_STAT_VITALITY_CHANGED = "stat-vitality-changed";
		public const string EVENT_STAT_MAX_HP_CHANGED = "stat-max-hp-changed";
		public const string EVENT_STAT_REGENERATE_HP_CHANGED = "stat-regenerate-hp-changed";
		public const string EVENT_STAT_LUCK_CHANGED = "stat-luck-changed";
		public const string EVENT_STAT_LUCK_DICE_CHANGED = "stat-luck-dice-changed";
		public const string EVENT_STAT_LUCK_REWARD_CHANGED = "stat-luck-reward-changed";
		public const string EVENT_STAT_AUTO_POINT_ADDED = "stat-auto-point-added";
		public const string EVENT_COUNTDOWN_CALLBACK = "countdown-callback-fired";
		public const string EVENT_COUNTDOWN_ENDED = "countdown-ended";
		public const string EVENT_COUNTDOWN_STOPPED = "countdown-stopped";
		public const string EVENT_TURN_NUMBER_GETTING = "turn-number-getting";
		public const string EVENT_REFEREE_SERVER_ENDTURNED = "referee-server-endturned";
		public const string EVENT_REFEREE_CLIENT_ENDTURNED = "referee-client-endturned";
		public const string EVENT_REFEREE_SERVER_PLAYED = "referee-server-played";
		public const string EVENT_REFEREE_CLIENT_PLAYED = "referee-client-played";
		public const string EVENT_REFEREE_SERVER_JOINT = "referee-server-joint";
		public const string EVENT_REFEREE_CLIENT_JOINT = "referee-client-joint";

		// Connection status
		public const string EVENT_CONNECTION_STATUS_STARTING_A_CONNECTION = "connection-status-starting-a-connection";
		public const string EVENT_CONNECTION_STATUS_ON_CLIENT_CONNECT = "connection-status-on-client-connect";
		public const string EVENT_CONNECTION_STATUS_ON_START_HOST = "connection-status-on-start-host";
		public const string EVENT_CONNECTION_STATUS_ON_LOBBY_SERVER_CREATE_LOBBY_PLAYER = "connection-status-on-lobby-server-create-lobby-player";
		public const string EVENT_CONNECTION_STATUS_ON_DROP_CONNECTION = "connection-status-on-drop-connection";
		public const string EVENT_CONNECTION_STATUS_ON_SERVER_DISCONNECT = "connection-status-on-server-disconnect";
		public const string EVENT_CONNECTION_STATUS_ON_CLIENT_DISCONNECT = "connection-status-on-client-disconnect";
		public const string EVENT_CONNECTION_STATUS_ON_CLIENT_ERROR = "connection-status-on-client-error";
		public const string EVENT_CONNECTION_STATUS_ON_LOBBY_STOP_CLIENT = "connection-status-on-lobby-stop-client";
		public const string EVENT_CONNECTION_STATUS_ON_LOBBY_STOP_HOST = "connection-status-on-lobby-stop-host";
		public const string EVENT_CONNECTION_STATUS_ON_PLAYER_HAS_ALREADY = "connection-status-on-player-has-already";
		public const string EVENT_CONNECTION_STATUS_ON_PLAYER_COUNTDOWN = "connection-status-on-player-countdown";
		public const string EVENT_CONNECTION_STATUS_ON_CLIENT_DISCONNECT_IN_BATTLE = "connection-status-on-client-disconnect-in-battle";
	}
}

