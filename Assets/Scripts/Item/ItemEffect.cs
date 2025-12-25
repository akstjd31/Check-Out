public abstract class ItemEffect
{
    public string EffectName { get; set; }
    public int Value1 { get; set; }
    public int Value2 { get; set; }
    public string ControlKey { get; set; }

    public virtual bool Use() { return true; }
    public virtual bool Use(out int value) { value = default; return true; }
    public virtual bool Use(out int value1, out int value2) { value1 = default; value2 = default; return true; }
}
