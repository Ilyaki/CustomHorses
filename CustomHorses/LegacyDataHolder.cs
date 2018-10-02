using StardewValley;
using System;
using System.Collections.Generic;

namespace CustomHorses
{
	class LegacyDataHolder
	{
		public Dictionary<Guid, string> GetIDs()
		{
			long mpID = (Game1.MasterPlayer ?? Game1.player).UniqueMultiplayerID;

			if (data.IDsPerFarm.ContainsKey(mpID))
				return data.IDsPerFarm[mpID];
			else
			{
				var newDictionary = new Dictionary<Guid, string>();
				data.IDsPerFarm.Add(mpID, newDictionary);
				return newDictionary;
			}
		}

		private LegacyDataModel data;

		public void ReadData()
		{
			data = ModEntry._Helper.ReadJsonFile<LegacyDataModel>("horseData.json") ?? new LegacyDataModel();
		}

		public void SaveData()
		{
			ModEntry._Helper.WriteJsonFile("horseData.json", data);
		}
	}
}
