using System;

namespace Enums
{
    [Serializable]
    public enum EBarEffect
    {
        None = 0,
        Highlight = 1,
        Sorted = 2,
    }
    
    internal static class EBarEffectExtensions
    {
        public static string AsString(this EBarEffect barEffect) => barEffect switch
        {
            EBarEffect.None => nameof(EBarEffect.None),
            EBarEffect.Highlight => nameof(EBarEffect.Highlight),
            EBarEffect.Sorted => nameof(EBarEffect.Sorted),
            _ => throw new ArgumentOutOfRangeException(nameof(barEffect), barEffect, null),
        };
    }
}