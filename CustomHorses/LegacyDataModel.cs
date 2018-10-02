using System;
using System.Collections.Generic;

namespace CustomHorses
{
	class LegacyDataModel
	{
		//Farmer unique multplayer id : [Horse id : texture file name (with extension)]
		public Dictionary<long, Dictionary<Guid, string>> IDsPerFarm { get; set; } = new Dictionary<long, Dictionary<Guid, string>>();
	}
}
