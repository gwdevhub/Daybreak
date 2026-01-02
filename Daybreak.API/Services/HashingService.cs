namespace Daybreak.API.Services;

public sealed class HashingService
{
    private const uint DefaultHash = 0x325D1EAE;
    private const uint WStringHashSeed1 = 0xE2C15C9D;
    private const uint WStringHashSeed2 = 0x2170A28A;

    private static readonly uint[] HashTable = [
            0x2EB3C21D, 0xA6F1D482, 0xBE24628F, 0x35699E30, 0xC87EE224, 0x04BA0097, 0x791F0EAF, 0xA162D466,
            0x685B376A, 0xF82133BA, 0x4682D46B, 0xCB25C0C5, 0xAC3F9D22, 0xABF17202, 0xEF987728, 0x3E400B83,
            0x63003108, 0xD83F1C93, 0x5058F507, 0x032E9991, 0x3518BC24, 0x1F90D44B, 0x92508DCE, 0x42DAA15C,
            0x5B4105A3, 0x4D5026AD, 0x07F838B0, 0xE8C4D2A2, 0xF9EF4F54, 0x1FC1A85F, 0xB9041A40, 0x446F6969,
            0xACF1EE39, 0x1828CC65, 0xC2F6C59D, 0x2456C3C0, 0x64F70142, 0x9C8ADB42, 0xC76781BF, 0x6AEA07E3,
            0xE98B14BF, 0x7DBCB35E, 0x6CE78C80, 0x1E3F93DE, 0x0954FEB7, 0xB3103AEF, 0x6DAAC1EE, 0x6535E301,
            0x4A221E68, 0x819CDD81, 0x5378CE3B, 0xE7348236, 0x2C3A7BCC, 0x31C50234, 0x745319DA, 0xF0897A0B,
            0x5C182968, 0x48E018F4, 0x593E3BB6, 0xAB588C8D, 0xF1101F1F, 0x1D36E8BC, 0xABC7C7C9, 0xF58AA6C8,
            0xF75B4B98, 0xC447B576, 0x6E5290F2, 0xD344EDF8, 0x5E1BCAF1, 0xEE892405, 0x798F7609, 0x8905CC29,
            0x8977DEDC, 0x007850BC, 0x0AA3114A, 0x2F61C8F2, 0x376DFAA4, 0x0BABFC12, 0x98E64E82, 0xE6743711,
            0xE26B26B6, 0xCBB10C72, 0x87446F8F, 0x96E26C0D, 0xCF140831, 0x81F69B0B, 0x5C9DF30B, 0xB276F929,
            0x2712C5B6, 0x80EA90D6, 0x8EB30322, 0xCF7A705A, 0x57A17E8C, 0x556A2425, 0x15104549, 0x5F3D549F,
            0x5D98FDEE, 0x9D8FA73E, 0x9206C9EF, 0xEDA0541D, 0xA3164F2D, 0x04C83543, 0x9BB27230, 0xEE97F0BA,
            0xB6AEB38C, 0x41FF4DA7, 0x4023EE54, 0xECBB78BC, 0xC2C54733, 0x8850B63F, 0x93BF56EF, 0xC80177D7,
            0xE68B0C13, 0x3C1E9D4C, 0x5CB3EACC, 0xF16F5795, 0xAC2B8A5C, 0x952E466E, 0x3095B0D5, 0xEC9D3A43,
            0x23D8FCF7, 0x10C7A93F, 0x1B479FB5, 0x3664CF8F, 0xA514A56B, 0xE16C4886, 0x63828188, 0xA47F14C1,
            0x391C237C, 0xFB15911F, 0x29825A01, 0xFC66DA40, 0xB2719F4E, 0x7B0D1611, 0x8F1EE7DE, 0xA6A1E77D,
            0xDFC25D30, 0x76377036, 0x8145DB82, 0x2300A4E9, 0xA2417F88, 0x1B8C6AFC, 0x68EBDC8E, 0x48FC0C41,
            0x040C3DCD, 0x17F02C44, 0xCC40B026, 0x5089EE1E, 0x2A594C28, 0x0CC198EC, 0x3F676F3D, 0xCD0AA239,
            0x274A31A4, 0xC6B4D4FA, 0x9480A62F, 0xAF8862EF, 0x627CA44D, 0x572E89D3, 0x4C05136C, 0x60F36E8C,
            0x7DE31FEF, 0x1B97FED3, 0xED99E2BF, 0xA034D418, 0xBBDB3E04, 0x1F198B71, 0xED45E889, 0xF7F1B0DC,
            0x1592B9A5, 0x96D60F96, 0x03D616EE, 0xA5EF679D, 0x831D791D, 0x579D5FD1, 0x4B44DC48, 0x38F7A2F0,
            0x2BF34A28, 0xAF14318C, 0xBD66D20A, 0x9AC72D2A, 0xC41ADB95, 0x860B2205, 0xE0E580E7, 0x3A06669C,
            0x2D1A40A2, 0x577EDF60, 0x1A37693A, 0x3E576C35, 0xE3857565, 0xE46E75EE, 0x94E3625D, 0x7D1B9280,
            0x2A0CCCE1, 0xBAB36163, 0x66E4E087, 0x940129BB, 0x59C197CD, 0xB0634139, 0xBB2964E1, 0x17207138,
            0xD8BD3728, 0xB324876C, 0x7C72FD04, 0xBB5D9D82, 0xD9B40236, 0xC0045BF6, 0xF85100ED, 0x36493582,
            0x8D0DF92A, 0xC3328368, 0xC202E412, 0x7ADE0064, 0xD01529C6, 0x4445B2AD, 0x0F6FC63B, 0xEF976FC5,
            0x709EC6BE, 0x69D31EA1, 0x794BFEB4, 0xB522DB95, 0x700DE6DF, 0x49098B71, 0x38B7FA82, 0x11EDDC04,
            0x5BB21D9F, 0xB14BC6D4, 0x3009D754, 0x451F5F3D, 0xFAA58575, 0xFB190B0D, 0x999DF79F, 0x6798A7AA,
            0x0BC434FF, 0x429E6C15, 0xA1EA2298, 0x55A57B81, 0xD7D74B8D, 0x8FECB38B, 0x658AD588, 0xF0190227,
            0xBE5D2B36, 0x62C59613, 0xC9FEDD64, 0xADD07A1E, 0x757A7679, 0xD78B6DCC, 0x56F88B2C, 0x4CC020BB,
            0xA88B51FE, 0xF47F7691, 0x48587EDD, 0x3FCEC5DB, 0xA7E127AC, 0x561F6161, 0x37AB9607, 0xFE23282B,
        ];

    private static readonly uint[] StringHashTable = [
            0x92B9A528, 0x25D4FC88, 0xEDCBEFB8, 0x51063A80,
            0x91341C61, 0x0261229D, 0x726F48ED, 0xCE1C088C,
            0x76253EB5, 0x31E3A0DE, 0xA2AAD215, 0xCA7D6D27,
            0xA5F98970, 0x0541C365, 0x3C14FF04, 0x5056AF4F
        ];

    private static uint HashHelper(uint result, byte b)
    {
        return result + (HashTable[result >> 24] ^ (result >> 6) ^ HashTable[b]);
    }

    public uint Hash(byte[] data)
    {
        var hash = DefaultHash;
        foreach(var b in data)
        {
            hash = HashHelper(hash, b);
        }

        return hash;
    }

    public uint Hash(byte data)
    {
        return HashHelper(DefaultHash, data);
    }

    public uint Hash(short data)
    {
        uint hash = DefaultHash;
        hash = HashHelper(hash, (byte)(data >> 0));
        hash = HashHelper(hash, (byte)(data >> 8));
        return hash;
    }

    public uint Hash(uint data)
    {
        uint hash = DefaultHash;
        hash = HashHelper(hash, (byte)(data >> 0));
        hash = HashHelper(hash, (byte)(data >> 8));
        hash = HashHelper(hash, (byte)(data >> 16));
        hash = HashHelper(hash, (byte)(data >> 24));
        return hash;
    }

    public uint Hash(string data, int maxLength = -1)
    {
        if (maxLength is -1)
        {
            maxLength = data.Length;
        }

        uint seed1 = WStringHashSeed1;
        uint seed2 = WStringHashSeed2;
        uint hash = DefaultHash;

        for(var i = 0; i < maxLength && i < data.Length; i++)
        {
            var c = data[i];
            var uc = (uint)c;
            if (c >= 'a' && c <= 'z')
            {
                uc -= 0x20;
            }

            seed1 = (seed1 << 3) ^ uc;
            seed2 += StringHashTable[seed1 & 0xf];
            hash ^= seed2 + seed1;
        }

        return hash;
    }
}
