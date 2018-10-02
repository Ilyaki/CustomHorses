using System;
using System.Collections.Generic;

namespace CustomHorses
{
	class PerSaveHorseData
	{
		//Horse id : texture file name (with extension)
		public Dictionary<Guid, string> IDs { get; set; } = new Dictionary<Guid, string>();
	}
}
