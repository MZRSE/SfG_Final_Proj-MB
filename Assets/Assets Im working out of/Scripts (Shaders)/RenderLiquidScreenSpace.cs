using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering.Universal.Internal;

public class RenderLiquidScreenSpace : ScriptableRendererFeature
{
    class RenderLiquidDepthPass : ScriptableRenderPass
    {
        const string LiquidDepthRTId = "_LiquidDepthRT";
        int _liquidDepthRTId;
        public Material _writeDepthMaterial;

        RenderTargetIdentifier _LDRT;
        RenderStateBlock _RSB;
        RenderQueueType _RQT;
        FilteringSettings _FS;
        ProfilingSampler _PS;
        List<ShaderTagId> _STIdList = new List<ShaderTagId>();

        public RenderLiquidDepthPass(string _profilerTag, RenderPassEvent _renderPassEvent, string[] _shaderTags, RenderQueueType _renderQueueType, int _layerMask)
        {
            profilingSampler = new ProfilingSampler(nameof(RenderObjectsPass));

            _PS = new ProfilingSampler(_profilerTag);
            this.renderPassEvent = _renderPassEvent;
            this._RQT = _renderQueueType;
            RenderQueueRange renderQueueRange = (_renderQueueType == RenderQueueType.Transparent)
                ? RenderQueueRange.transparent
                : RenderQueueRange.opaque;
            _FS = new FilteringSettings(renderQueueRange, _layerMask);

            if (_shaderTags != null && _shaderTags.Length > 0)
            {
                foreach (var passName in _shaderTags)
                    _STIdList.Add(new ShaderTagId(passName));
            } else {
                _STIdList.Add(new ShaderTagId("SRPDefaultUnlit"));
                _STIdList.Add(new ShaderTagId("UniversalForward"));
                _STIdList.Add(new ShaderTagId("UniversalForwardOnly"));
                _STIdList.Add(new ShaderTagId("LightweightForward"));
            }

            _RSB = new RenderStateBlock(RenderStateMask.Nothing);
        }

        public override void OnCameraSetup(CommandBuffer _cmd, ref RenderingData _renderingData)
        {
            RenderTextureDescriptor _blitTargetDescriptor = _renderingData.cameraData.cameraTargetDescriptor;

            _liquidDepthRTId = Shader.PropertyToID(LiquidDepthRTId);
            _cmd.GetTemporaryRT(_liquidDepthRTId, _blitTargetDescriptor);
            _LDRT = new RenderTargetIdentifier(_liquidDepthRTId);
            ConfigureTarget(_LDRT);
            ConfigureClear(ClearFlag.All, Color.clear);
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData _renderingData)
        {
            SortingCriteria _sortCriteria = (_RQT == RenderQueueType.Transparent)
                ? SortingCriteria.CommonTransparent
                : _renderingData.cameraData.defaultOpaqueSortFlags;

            DrawingSettings _drawSettings = CreateDrawingSettings(_STIdList, ref _renderingData, _sortCriteria);

            CommandBuffer _cmd = CommandBufferPool.Get();
            using (new ProfilingScope(_cmd, _PS))
            {
                _drawSettings.overrideMaterial = _writeDepthMaterial;
                context.DrawRenderers(_renderingData.cullResults, ref _drawSettings, ref _FS, ref _RSB);
            }

            context.ExecuteCommandBuffer(_cmd);
            CommandBufferPool.Release(_cmd);
        }
    }

    class RenderLiquidScreenSpacePass : ScriptableRenderPass
    {
        const string _liquidRTId = "_LiquidRT";
        const string _liquidRT2Id = "_LiquidRT2";
        const string _liquidDepthRTId = "_LiquidDepthRT";

        int _LRTId;
        int _LRT2Id;
        int _LDepthRTId;

        public Material BlitMaterial;
        Material _blurMaterial;
        Material _blitCopyDepthMaterial;

        public int _blurPasses;
        public float _blurDistance;

        RenderTargetIdentifier _LRT;
        RenderTargetIdentifier _LRT2;
        RenderTargetIdentifier _LDRT;
        RenderTargetIdentifier _CTId;
        RenderTargetIdentifier _CDTId;

        RenderQueueType _RQT;
        RenderStateBlock _RSB;
        FilteringSettings _FS;
        ProfilingSampler _PS;
        List<ShaderTagId> _STIdList = new List<ShaderTagId>();

        public RenderLiquidScreenSpacePass(string _profilerTag, RenderPassEvent _renderPassEvent, string[] _shaderTags, RenderQueueType _renderQueueType, int _layerMask)
        {
            profilingSampler = new ProfilingSampler(nameof(RenderObjectsPass));

            _PS = new ProfilingSampler(_profilerTag);
            this.renderPassEvent = _renderPassEvent;
            this._RQT = _renderQueueType;
            RenderQueueRange renderQueueRange = (_renderQueueType == RenderQueueType.Transparent)
                ? RenderQueueRange.transparent
                : RenderQueueRange.opaque;
            _FS = new FilteringSettings(renderQueueRange, _layerMask);

            if (_shaderTags != null && _shaderTags.Length > 0)
            {
                foreach (var passName in _shaderTags)
                    _STIdList.Add(new ShaderTagId(passName));
            }
            else
            {
                _STIdList.Add(new ShaderTagId("SRPDefaultUnlit"));
                _STIdList.Add(new ShaderTagId("UniversalForward"));
                _STIdList.Add(new ShaderTagId("UniversalForwardOnly"));
                _STIdList.Add(new ShaderTagId("LightweightForward"));
            }

            _RSB = new RenderStateBlock(RenderStateMask.Nothing);

            _blitCopyDepthMaterial = new Material(Shader.Find("Hidden/BlitToDepth"));
            _blurMaterial = new Material(Shader.Find("Hidden/KawaseBlur"));
        }

        public override void OnCameraSetup(CommandBuffer _cmd, ref RenderingData _renderingData)
        {
            RenderTextureDescriptor _blitTargetDescriptor = _renderingData.cameraData.cameraTargetDescriptor;
            _blitTargetDescriptor.colorFormat = RenderTextureFormat.ARGB32;

            var _renderer = _renderingData.cameraData.renderer;

            _LRTId = Shader.PropertyToID(_liquidRTId);
            _LRT2Id = Shader.PropertyToID(_liquidRT2Id);
            _LDepthRTId = Shader.PropertyToID(_liquidDepthRTId);

            _cmd.GetTemporaryRT(_LRTId, _blitTargetDescriptor, FilterMode.Bilinear);
            _cmd.GetTemporaryRT(_LRT2Id, _blitTargetDescriptor, FilterMode.Bilinear);

            _LRT = new RenderTargetIdentifier(_LRTId);
            _LRT2 = new RenderTargetIdentifier(_LRT2Id);
            _LDRT = new RenderTargetIdentifier(_LDepthRTId);

            ConfigureTarget(_LRT);

            _CTId = _renderer.cameraColorTarget;
            _CDTId = new RenderTargetIdentifier("_CameraDepthTexture");
            }

        public override void Execute(ScriptableRenderContext context, ref RenderingData _renderingData)
        {
            SortingCriteria _sortCriteria = (_RQT == RenderQueueType.Transparent)
                ? SortingCriteria.CommonTransparent
                : _renderingData.cameraData.defaultOpaqueSortFlags;

            DrawingSettings _drawSettings = CreateDrawingSettings(_STIdList, ref _renderingData, _sortCriteria);

            CommandBuffer _cmd = CommandBufferPool.Get();
            using (new ProfilingScope(_cmd, _PS))
            {
                _cmd.ClearRenderTarget(true, true, Color.clear);
                context.ExecuteCommandBuffer(_cmd);
                _cmd.Clear();

                Blit(_cmd, _CDTId, _LRT, _blitCopyDepthMaterial);
                context.ExecuteCommandBuffer(_cmd);
                _cmd.Clear();

                context.DrawRenderers(_renderingData.cullResults, ref _drawSettings, ref _FS, ref _RSB);
                context.ExecuteCommandBuffer(_cmd);
                _cmd.Clear();

                _cmd.SetGlobalTexture("_BlurDepthTex", _LDRT);
                _cmd.SetGlobalFloat("_BlurDistance", _blurDistance);
                float offset = 1.5f;
                _cmd.SetGlobalFloat("_Offset", offset);
                Blit(_cmd, _LRT, _LRT2, _blurMaterial);
                context.ExecuteCommandBuffer(_cmd);
                _cmd.Clear();

                var tmpRT = _LRT;
                _LRT = _LRT2;
                _LRT2 = tmpRT;

                for (int i = 1; i < _blurPasses; ++i)
                {
                    offset += 1.0f;
                    _cmd.SetGlobalFloat("_Offset", offset);
                    Blit(_cmd, _LRT, _LRT2, _blurMaterial);
                    context.ExecuteCommandBuffer(_cmd);
                    _cmd.Clear();

                    tmpRT = _LRT;
                    _LRT = _LRT2;
                    _LRT2 = tmpRT;
                }

                Blit(_cmd, _LRT, _CTId, BlitMaterial);
            }
            
            context.ExecuteCommandBuffer(_cmd);
            CommandBufferPool.Release(_cmd);
        }

        public override void OnCameraCleanup(CommandBuffer _cmd)
        {
            _cmd.ReleaseTemporaryRT(_LRTId);
            _cmd.ReleaseTemporaryRT(_LRT2Id);
            _cmd.ReleaseTemporaryRT(_LDepthRTId);
        }
    }

    public string _passTag = "RenderLiquidScreenSpace";
    public RenderPassEvent _event = RenderPassEvent.AfterRenderingOpaques;

    public RenderObjects.FilterSettings _FS = new RenderObjects.FilterSettings();

    public Material BlitMaterial;
    public Material _writeDepthMaterial;

    RenderLiquidDepthPass _renderLiquidDepthPass;
    RenderLiquidScreenSpacePass _renderLiquidScreenSpacePass;

    [Range(1, 15)]
    public int _blurPasses = 1;

    [Range(0f, 1f)]
    public float _blurDistance = 0.5f;

    public override void Create()
    {
        _renderLiquidDepthPass = new RenderLiquidDepthPass(_passTag, _event, _FS.PassNames, _FS.RenderQueueType, _FS.LayerMask)
        {
            _writeDepthMaterial = _writeDepthMaterial
        };

        _renderLiquidScreenSpacePass = new RenderLiquidScreenSpacePass(_passTag, _event, _FS.PassNames, _FS.RenderQueueType, _FS.LayerMask)
        {
            BlitMaterial = BlitMaterial,
            _blurPasses = _blurPasses,
            _blurDistance = _blurDistance
        };
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(_renderLiquidDepthPass);
        renderer.EnqueuePass(_renderLiquidScreenSpacePass);
    }
}
