using Godot;

namespace Kiwijam2025.FmodServer;

public class FmodDspSettings : FmodGdAPI
{
    public FmodDspSettings(Variant ev) : base(ev)
    {
        
    }
    
    #region Properties
    public int DspBufferCount
    {
        get => (int)GetProperty("dsp_buffer_count");
        set => SetProperty("dsp_buffer_count", value);
    }
    public int DspBufferSize
    {
        get => (int)GetProperty("dsp_buffer_size");
        set => SetProperty("dsp_buffer_size", value);
    }
    #endregion
}