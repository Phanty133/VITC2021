using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering.Universal.PostProcessing;
using UnityEngine.Scripting;


// Mercilessly cannibalised from the tutorial on https://github.com/yahiaetman/URPCustomPostProcessingStack
// lowkey kinda bullshit how I have to use a package to do what's natively supported in HDRP for like 2 years now thanks unity
[System.Serializable, VolumeComponentMenu("CustomPostProcess/Damage")]
public class DamageEffect : VolumeComponent
{
	[Tooltip("Intensity of the effect.")]
	public ClampedFloatParameter intensity = new ClampedFloatParameter(0, 0, 1);
}

[CustomPostProcess("Damage", CustomPostProcessInjectionPoint.BeforePostProcess), Preserve]
public class DamageEffectRenderer : CustomPostProcessRenderer
{
	private DamageEffect m_VolumeComponent;

	private Material m_Material;

	static class ShaderIDs
	{
		internal readonly static int Input = Shader.PropertyToID("_MainTex");
		internal readonly static int Intensity = Shader.PropertyToID("_intensity");
	}

	public override bool visibleInSceneView => true;

	public override ScriptableRenderPassInput input => ScriptableRenderPassInput.Color;

	[Preserve]
	public override void Initialize()
	{
		m_Material = CoreUtils.CreateEngineMaterial("Custom/DamageShader");
	}

	[Preserve]
	public override bool Setup(ref RenderingData renderingData, CustomPostProcessInjectionPoint injectionPoint)
	{
		var stack = VolumeManager.instance.stack;
		m_VolumeComponent = stack.GetComponent<DamageEffect>();
		return m_VolumeComponent.intensity.value > 0;
	}

	[Preserve]
	public override void Render(CommandBuffer cmd, RenderTargetIdentifier source, RenderTargetIdentifier destination, ref RenderingData renderingData, CustomPostProcessInjectionPoint injectionPoint)
	{
		if (m_Material != null)
		{
			m_Material.SetFloat(ShaderIDs.Intensity, m_VolumeComponent.intensity.value);
		}
		cmd.SetGlobalTexture(ShaderIDs.Input, source);
		CoreUtils.DrawFullScreen(cmd, m_Material, destination);
	}
}