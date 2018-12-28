using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Characters;
using System;

namespace CustomHorses
{
	class LoadTexturePatcher : Patch
	{
		protected override PatchDescriptor GetPatchDescriptor() => new PatchDescriptor(typeof(AnimatedSprite), "loadTexture");

		public static bool Prefix(AnimatedSprite __instance)
		{
			foreach (Horse horse in ModEntry.horses)
			{
				if (horse.Sprite == __instance)
				{
					if (ModEntry.data?.GetIDs()?.ContainsKey(horse.HorseId) == true)
					{
						try
						{
							var texture = ModEntry.LoadTexture(ModEntry.data.GetIDs()[horse.HorseId]);
							var field = ModEntry._Helper.Reflection.GetField<Texture2D>(__instance, "spriteTexture");
							field.SetValue(texture);

							return false;
						}catch(Exception)
						{
							return true;
						}
					}
				}
			}

			return true;
		}
	}
}
