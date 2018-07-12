
////////////////////////////////////////////////////////////////////////////////
//  
// @module  uTween for UGUI
// @author  Flamesky Dexive
// @support flamesky@live.com
//
////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System.Collections;

namespace uTools {
	public enum PlayDirection {
		Reverse = -1,
		Toggle = 0,
		Forward = 1
	}

	public enum Trigger {
		OnPointerEnter,
		OnPointerDown,
		OnPointerClick,
		OnPointerUp,
		OnPointerExit,
	}
}