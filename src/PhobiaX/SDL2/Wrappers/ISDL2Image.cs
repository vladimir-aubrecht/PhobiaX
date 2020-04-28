using System;

namespace PhobiaX.SDL2.Wrappers
{
    public interface ISDL2Image
    {
        IntPtr LoadTexture(IntPtr rendererIntPtr, string filePath);
    }
}