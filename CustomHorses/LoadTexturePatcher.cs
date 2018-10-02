using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Characters;

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
					if (ModEntry.data?.IDs?.ContainsKey(horse.HorseId) == true)
					{
						var texture = ModEntry.LoadTexture(ModEntry.data.IDs[horse.HorseId]);
						var field = ModEntry._Helper.Reflection.GetField<Texture2D>(__instance, "spriteTexture");
						field.SetValue(texture);

						return false;
					}
				}
			}

			return true;
		}
	}
}
