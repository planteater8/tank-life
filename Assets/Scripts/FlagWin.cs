﻿using UnityEngine;
using System.Collections;

namespace Stuff {

	public class FlagWin : Stuff.Pickup
	{

		public int minimumFlagsRequired;

		// Use this for initialization
		void Start ()
		{

		}

		// Update is called once per frame
		void Update ()
		{

		}

		internal override bool DoEffect (Player p) {
			Debug.Log ("DOING THINGS!");
			bool jobsDone = false;
			stayingAlive = true;

			if ((p.GetComponentInChildren<Flag> () is Flag
					&& p.flags >= minimumFlagsRequired)
					&& p.neededWins <= 1
					&& jobsDone) {
					Debug.Log ("You Win");
					Application.LoadLevel ("Lv" + (1 + levelCount));
			} else if (p.GetComponentInChildren<Flag> () is Flag
					&& p.flags >= minimumFlagsRequired) {
					stayingOut = true;
					p.neededWins -= 1;
					Debug.Log ("You need more wins");
			} else {
					stayingOut = true;
					Debug.Log ("else?");
			}

			return false;
		}
	}

}
