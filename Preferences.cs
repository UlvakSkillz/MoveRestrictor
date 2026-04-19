using MelonLoader;

namespace MoveRestrictor
{
    public class Preferences
	{
		private const string CONFIG_FILE = "config.cfg";
		private const string USER_DATA = "UserData/MoveRestrictor/";
        internal static Dictionary<MelonPreferences_Entry, object> LastSavedValues = new();

        internal static MelonPreferences_Category SettingsCategory;
        internal static MelonPreferences_Entry<bool> PrefEnabled;

        internal static MelonPreferences_Category StructuresCategory;
        internal static MelonPreferences_Entry<bool> PrefDiscBlocked;
        internal static MelonPreferences_Entry<bool> PrefPillarBlocked;
        internal static MelonPreferences_Entry<bool> PrefBallBlocked;
        internal static MelonPreferences_Entry<bool> PrefWallBlocked;
        internal static MelonPreferences_Entry<bool> PrefCubeBlocked;

        internal static MelonPreferences_Category ModifiersCategory;
        internal static MelonPreferences_Entry<bool> PrefStraightBlocked;
        internal static MelonPreferences_Entry<bool> PrefKickBlocked;
        internal static MelonPreferences_Entry<bool> PrefStompBlocked;
        internal static MelonPreferences_Entry<bool> PrefUppercutBlocked;
        internal static MelonPreferences_Entry<bool> PrefParryBlocked;
        internal static MelonPreferences_Entry<bool> PrefHoldLeftBlocked;
        internal static MelonPreferences_Entry<bool> PrefHoldRightBlocked;
        internal static MelonPreferences_Entry<bool> PrefExplodeBlocked;
        internal static MelonPreferences_Entry<bool> PrefFlickBlocked;

        internal static MelonPreferences_Category MovementCategory;
        internal static MelonPreferences_Entry<bool> PrefSprintBlocked;
        internal static MelonPreferences_Entry<bool> PrefJumpBlocked;
        internal static MelonPreferences_Entry<bool> PrefDashBlocked;


        internal static void InitPrefs()
		{
			if (!Directory.Exists(USER_DATA)) { Directory.CreateDirectory(USER_DATA); }

            SettingsCategory = MelonPreferences.CreateCategory("MoveRestrictor", "Settings");
            SettingsCategory.SetFilePath(Path.Combine(USER_DATA, CONFIG_FILE));

            PrefEnabled = SettingsCategory.CreateEntry("Enabled", true, "Enabled", "Toggles the Mod On/Off");

            StructuresCategory = MelonPreferences.CreateCategory("Structures", "Structures");
            StructuresCategory.SetFilePath(Path.Combine(USER_DATA, CONFIG_FILE));

            PrefDiscBlocked = StructuresCategory.CreateEntry("DiscBlocked", false, "Disc Blocked", "Turns Off Disc");
            PrefPillarBlocked = StructuresCategory.CreateEntry("PillarBlocked", false, "Pillar Blocked", "Turns Off Pillar");
            PrefBallBlocked = StructuresCategory.CreateEntry("BallBlocked", false, "Ball Blocked", "Turns Off Ball");
            PrefWallBlocked = StructuresCategory.CreateEntry("WallBlocked", false, "Wall Blocked", "Turns Off Wall");
            PrefCubeBlocked = StructuresCategory.CreateEntry("CubeBlocked", false, "Cube Blocked", "Turns Off Cube");

            ModifiersCategory = MelonPreferences.CreateCategory("Modifiers", "Modifiers");
            ModifiersCategory.SetFilePath(Path.Combine(USER_DATA, CONFIG_FILE));

            PrefStraightBlocked = ModifiersCategory.CreateEntry("StraightBlocked", false, "Straight Blocked", "Turns Off Straight");
            PrefKickBlocked = ModifiersCategory.CreateEntry("KickBlocked", false, "Kick Blocked", "Turns Off Kick");
            PrefStompBlocked = ModifiersCategory.CreateEntry("StompBlocked", false, "Stomp Blocked", "Turns Off Stomp");
            PrefUppercutBlocked = ModifiersCategory.CreateEntry("UppercutBlocked", false, "Uppercut Blocked", "Turns Off Uppercut");
            PrefParryBlocked = ModifiersCategory.CreateEntry("ParryBlocked", false, "Parry Blocked", "Turns Off Parry");
            PrefHoldLeftBlocked = ModifiersCategory.CreateEntry("HoldLeftBlocked", false, "Hold Left Blocked", "Turns Off Left Hand Hold");
            PrefHoldRightBlocked = ModifiersCategory.CreateEntry("HoldRightBlocked", false, "Hold Right Blocked", "Turns Off Right Hand Hold");
            PrefExplodeBlocked = ModifiersCategory.CreateEntry("ExplodeBlocked", false, "Explode Blocked", "Turns Off Explode");
            PrefFlickBlocked = ModifiersCategory.CreateEntry("FlickBlocked", false, "Flick Blocked", "Turns Off Flick");

            MovementCategory = MelonPreferences.CreateCategory("Movement", "Movement");
            MovementCategory.SetFilePath(Path.Combine(USER_DATA, CONFIG_FILE));

            PrefSprintBlocked = MovementCategory.CreateEntry("SprintBlocked", false, "Sprint Blocked", "Turns Off Sprint");
            PrefJumpBlocked = MovementCategory.CreateEntry("JumpBlocked", false, "Jump Blocked", "Turns Off Jump");
            PrefDashBlocked = MovementCategory.CreateEntry("DashBlocked", false, "Dash Blocked", "Turns Off Dash");


            StoreLastSavedPrefs();
		}

		internal static void StoreLastSavedPrefs()
		{
			List<MelonPreferences_Entry> prefs = new();
			prefs.AddRange(SettingsCategory.Entries);
			prefs.AddRange(StructuresCategory.Entries);
			prefs.AddRange(ModifiersCategory.Entries);
			prefs.AddRange(MovementCategory.Entries);

			foreach (MelonPreferences_Entry entry in  prefs) { LastSavedValues[entry] = entry.BoxedValue; }
		}

		public static bool AnyPrefsChanged()
		{
			foreach (KeyValuePair<MelonPreferences_Entry, object> pair in LastSavedValues)
			{
				if (!pair.Key.BoxedValue.Equals(pair.Value)) { return true; }
			}
			return false;
		}
	}
}