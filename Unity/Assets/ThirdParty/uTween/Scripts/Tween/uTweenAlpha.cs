
////////////////////////////////////////////////////////////////////////////////
//  
// @module  uTween for UGUI
// @author  Flamesky Dexive
// @support flamesky@live.com
//
////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace uTools {
	[AddComponentMenu("uTools/Tween/Tween Alpha(uTools)")]
	public class uTweenAlpha : uTweenValue {

		public bool includeChilds = false;

		private Text mText;
		private Light mLight;
		private Material mMat;
		private Image mImage;
		private SpriteRenderer mSpriteRender;
		private CanvasGroup mCanvasGroup;

		float mAlpha = 0f;

		public float alpha {
			get {
				return mAlpha;
			}
			set {
				SetAlpha(transform, value);
				mAlpha = value;
			}
		}

		protected override void ValueUpdate (float value, bool isFinished)
		{
			alpha = value;
		}

		void SetAlpha(Transform _transform, float _alpha) {
			Color c = Color.white;
			mText = _transform.GetComponent<Text> ();
			if (mText != null){
				c = mText.color;
				c.a = _alpha;
				mText.color = c;
			}
			mLight = _transform.GetComponent<Light>();
			if (mLight != null){ 
				c = mLight.color;
				c.a = _alpha;
				mLight.color = c;
			}
			mImage = _transform.GetComponent<Image> ();
			if (mImage != null) {
				c = mImage.color;
				c.a = _alpha;
				mImage.color = c;
			}
			mSpriteRender = _transform.GetComponent<SpriteRenderer> ();
			if (mSpriteRender != null) {
				c = mSpriteRender.color;
				c.a = _alpha;
				mSpriteRender.color = c;
			}
		    mCanvasGroup = _transform.GetComponent<CanvasGroup> ();
			if (mCanvasGroup != null) {
                //c = mSpriteRender.color;
			    mCanvasGroup.alpha = _alpha;
				//mSpriteRender.color = c;
			}
			if (_transform.GetComponent<Renderer>() != null) {
				mMat = _transform.GetComponent<Renderer>().material;
				if (mMat != null) {
					c = mMat.color;
					c.a = _alpha;
					mMat.color = c;
				}
			}
			if (includeChilds) {
				for (int i = 0; i < _transform.childCount; ++i) {
					Transform child = _transform.GetChild(i);
					SetAlpha(child, _alpha);
				}
			}

		}



	}

}