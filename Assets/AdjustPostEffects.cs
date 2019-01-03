// Mit freundlicher Unterstützung von:
// http://answers.unity.com/answers/1483341/view.html

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class AdjustPostEffects : MonoBehaviour
{
	PostProcessVolume volume;
	DepthOfField dofLayer;
	Vignette vignetteLayer;

	void Start()
	{
		volume = gameObject.GetComponent<PostProcessVolume>();
		volume.profile.TryGetSettings(out dofLayer);
		volume.profile.TryGetSettings(out vignetteLayer);
	}

	void Update()
	{
		/*dofLayer.enabled.value = true;
		dofLayer.focusDistance.value = 4.45f;
		dofLayer.aperture.value = 0.1f;
		dofLayer.focalLength.value = 23f;*/
	}
}
