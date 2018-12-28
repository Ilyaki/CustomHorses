using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Characters;
using System;
using System.Collections.Generic;
using System.IO;

namespace CustomHorses
{
	class ModEntry : Mod
	{
		//private const string dataKey = "7f3d7939-0276-44d6-b377-fd2d4560a6f0";//Random UUID 

		public static IModHelper _Helper;

		public static List<Horse> horses = new List<Horse>();

		private static readonly Dictionary<string, Texture2D> cachedTextures = new Dictionary<string, Texture2D>();

		//public static PerSaveHorseData data;
		public static LegacyDataHolder data;

		public override void Entry(IModHelper helper)
		{
			_Helper = helper;

			LoadAllTextures();

			data = new LegacyDataHolder();


			Helper.Events.GameLoop.DayStarted += (o, e) =>
			{
				//data = helper.Data.ReadSaveData<PerSaveHorseData>(dataKey) ?? new PerSaveHorseData();
				data.ReadData();

				Monitor.Log("Found horses:", LogLevel.Info);
				horses.Clear();
				foreach (NPC character in Game1.getFarm()?.characters)
					if (character is Horse horse)
					{
						horses.Add(horse);
						Monitor.Log($" - HorseName={horse.Name}, displayName={horse.displayName}, horseID={horse.HorseId}", LogLevel.Info);
					}

				
			};

			helper.ConsoleCommands.Add("horses_setHorseTexture", "Usage: horses_setHorseTexture <horse name> <file name of texture file>", this.Command_SetHorseTexture);
			helper.ConsoleCommands.Add("horses_save", "Saves the horse data for this save.", this.Command_Save);
			helper.ConsoleCommands.Add("horses_reloadTextures", "Reloads all the textures. Use this when you add a file while the game is running.", (o,e) => LoadAllTextures());

			Patch.PatchAll("me.ilyaki.testingmod");
		}

		private void Command_SetHorseTexture(string command, string[] args)
		{
			if (args.Length != 2)
			{
				Monitor.Log("Need 2 arguments.", LogLevel.Info);
				return;
			}

			string horseName = args[0];
			horseName = horseName.TrimStart('\"');
			horseName = horseName.TrimEnd('\"');

			string textureFileName = args[1];

			foreach (Horse horse in horses)
			{
				if (horse.Name.ToLower() == horseName.ToLower())
				{
					try
					{
						LoadTexture(textureFileName);
						data.GetIDs().Remove(horse.HorseId);
						data.GetIDs().Add(horse.HorseId, textureFileName);
					}
					catch (Exception e)
					{
						Console.WriteLine(e);
						Monitor.Log($"Unable to find file '{textureFileName}'. Make sure you include the file extension, e.g. 'Horse1.png'", LogLevel.Info);
					}

					return;
				}
			}

			Monitor.Log($"Unable to find {horseName}. Make sure you have loaded a world.", LogLevel.Info);
		}

		private void Command_Save(string command, string[] args)
		{
			if (Context.IsWorldReady)
				data.SaveData();
			//Helper.Data.WriteSaveData(dataKey, data);
			else
				Monitor.Log("You need to load a save first.", LogLevel.Info);
		}
		
		private void LoadAllTextures()
		{
			cachedTextures.Clear();

			foreach (string filepath in Directory.GetFiles(Helper.DirectoryPath, "*.png"))
			{
				string filename = Path.GetFileName(filepath);
				Monitor.Log($"Loading file '{filename}'");
				cachedTextures.Add(filename, Helper.Content.Load<Texture2D>(filename));
			}
		}

		internal static Texture2D LoadTexture(string fileName)
		{
			return cachedTextures[fileName];
		}
	}
}
