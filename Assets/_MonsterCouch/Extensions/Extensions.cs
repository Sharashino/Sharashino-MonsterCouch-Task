using UnityEngine;

namespace _MonsterCouch.Extensions
{
	public static class Extensions
	{
		public static void Enable(this CanvasGroup cg)
		{
			cg.alpha = 1;
			cg.interactable = true;
			cg.blocksRaycasts = true;
		}

		public static void Disable(this CanvasGroup cg)
		{
			if (cg == null)
				return;

			cg.alpha = 0;
			cg.interactable = false;
			cg.blocksRaycasts = false;
		}
	}
}