﻿using System;
using System.Collections.Generic;
using System.Text;

namespace RenderWareFile.Enums
{
    public enum GXTexFmt : byte
    {
        GX_TF_I4 = 0x0,
        GX_TF_I8 = 0x1,
        GX_TF_IA4 = 0x2,
        GX_TF_IA8 = 0x3,
        GX_TF_RGB565 = 0x4,
        GX_TF_RGB5A3 = 0x5,
        GX_TF_RGBA8 = 0x6,
        GX_TF_C4 = 0x8,
        GX_TF_C8 = 0x9,
        GX_TF_C14X2 = 0xA,
        GX_TF_CMPR = 0xE,
        GX_CTF_R4 = 0x20,
        GX_CTF_RA4 = 0x22,
        GX_CTF_RA8 = 0x23,
        GX_CTF_YUVA8 = 0x26,
        GX_CTF_A8 = 0x27,
        GX_CTF_R8 = 0x28,
        GX_CTF_G8 = 0x29,
        GX_CTF_B8 = 0x2A,
        GX_CTF_RG8 = 0x2B,
        GX_CTF_GB8 = 0x2C,
        GX_TF_Z8 = 0x11,
        GX_TF_Z16 = 0x13,
        GX_TF_Z24X8 = 0x16,
        GX_CTF_Z4 = 0x30,
        GX_CTF_Z8M = 0x39,
        GX_CTF_Z8L = 0x3A,
        GX_CTF_Z16L = 0x3C,
        GX_TF_A8 = 0x27,
        None = 0xFF
    }
}
