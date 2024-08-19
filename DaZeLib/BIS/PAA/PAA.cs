using BIS.Core.Streams;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace BIS.PAA
{
    public class PAA
    {
        public List<Mipmap> mipmaps;
        private int[] mipmapOffsets = new int[16];

        public PAAType Type { get; private set; } = PAAType.UNDEFINED;

        public Palette Palette { get; private set; }

        public int Width => (int)this.mipmaps[0].Width;

        public int Height => (int)this.mipmaps[0].Height;

        public PAA(string file)
          : this((Stream)File.OpenRead(file), !file.EndsWith(".pac"))
        {
        }

        public PAA(Stream stream, bool isPac = false)
          : this(new BinaryReaderEx(stream), isPac)
        {
        }

        public PAA(BinaryReaderEx stream, bool isPac = false) => this.Read(stream, isPac);

        public Mipmap this[int i] => this.mipmaps[i];

        private static PAAType MagicNumberToType(ushort magic)
        {
            switch (magic)
            {
                case 5461:
                    return PAAType.RGBA_5551;
                case 17476:
                    return PAAType.RGBA_4444;
                case 32896:
                    return PAAType.AI88;
                case 65281:
                    return PAAType.DXT1;
                case 65282:
                    return PAAType.DXT2;
                case 65283:
                    return PAAType.DXT3;
                case 65284:
                    return PAAType.DXT4;
                case 65285:
                    return PAAType.DXT5;
                default:
                    return PAAType.UNDEFINED;
            }
        }

        private void Read(BinaryReaderEx input, bool isPac = false)
        {
            PAAType format = BIS.PAA.PAA.MagicNumberToType(input.ReadUInt16());
            if (format == PAAType.UNDEFINED)
            {
                format = !isPac ? PAAType.RGBA_4444 : PAAType.P8;
                input.Position -= 2L;
            }
            this.Type = format;
            this.Palette = new Palette(format);
            this.Palette.Read(input, this.mipmapOffsets);
            this.mipmaps = new List<Mipmap>(16);
            int index = 0;
            while (input.ReadUInt32() > 0U)
            {
                input.Position -= 4L;
                Debug.Assert(input.Position == (long)this.mipmapOffsets[index]);
                this.mipmaps.Add(new Mipmap(input, this.mipmapOffsets[index++]));
            }
            if (input.ReadUInt16() > (ushort)0)
                throw new FormatException("Expected two more zero's at end of file.");
        }

        public static byte[] GetARGB32PixelData(Stream paaStream, bool isPac = false, int mipmapIndex = 0) => BIS.PAA.PAA.GetARGB32PixelData(new BIS.PAA.PAA(paaStream, isPac), paaStream, mipmapIndex);

        public static byte[] GetARGB32PixelData(BIS.PAA.PAA paa, Stream paaStream, int mipmapIndex = 0)
        {
            BinaryReaderEx input = new BinaryReaderEx(paaStream);
            Mipmap mipmap = paa[mipmapIndex];
            return mipmap.GetRawPixelData(input, paa.Type);
            //byte[] rawPixelData = mipmap.GetRawPixelData(input, paa.Type);
            //switch (paa.Type)
            //{
            //    case PAAType.DXT1:
            //    case PAAType.DXT2:
            //    case PAAType.DXT3:
            //    case PAAType.DXT4:
            //    case PAAType.DXT5:
            //        return PixelFormatConversion.DXTToARGB32(rawPixelData, (int)mipmap.Width, (int)mipmap.Height, (int)paa.Type);
            //    case PAAType.RGBA_5551:
            //        return PixelFormatConversion.ARGB1555ToARGB32(rawPixelData);
            //    case PAAType.RGBA_4444:
            //        return PixelFormatConversion.ARGB16ToARGB32(rawPixelData);
            //    case PAAType.AI88:
            //        return PixelFormatConversion.AI88ToARGB32(rawPixelData);
            //    case PAAType.RGBA_8888:
            //    case PAAType.P8:
            //        return PixelFormatConversion.P8ToARGB32(rawPixelData, paa.Palette);
            //    default:
            //        throw new Exception(string.Format("Cannot retrieve pixel data from this PaaType: {0}", (object)paa.Type));
        }
    }
}
