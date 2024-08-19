using BIS.Core.Streams;
using BIS.PAA;
using K4os.Compression.LZ4.Encoders;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DayZeLib
{
    public class DDS
    {
        public List<int> copyBlocks = new List<int>();
        public List<int> lz4Blocks = new List<int>();
        public List<byte> list = new List<byte>();
        public string m_Signature { get; set; }
        public DDSHeader Header { get; set; }
        public DX10Header dx10head { get; set; }
        public byte[] Data { get; set; }
        public List<byte[]> bitmaps { get; set; }

        public DDS()
        {

        }
        public DDS(BinaryReader br, bool isEDDS)
        {
            Header = null;
            dx10head = null;
            m_Signature = new string(br.ReadChars(4));
            if (m_Signature != "DDS ")
            {
                MessageBox.Show("Not a DDS FILE.....");
                return;
            }
            Header = new DDSHeader(br);
            if (Header.IsDx10())
            {
                dx10head = new DX10Header(br);
                Data = br.ReadBytes((int)br.BaseStream.Length - 148);
            }
            else
            {
                Data = br.ReadBytes((int)br.BaseStream.Length - 128);
            }
            if (isEDDS)
            {
                using (MemoryStream ms = new MemoryStream(Data))
                using (BinaryReader binaryReader = new BinaryReader(ms))
                {
                    FindBlocks(binaryReader);
                    long position = binaryReader.BaseStream.Position;
                    foreach (int blocks in copyBlocks)
                    {
                        byte[] collection = binaryReader.ReadBytes(blocks);
                        list.InsertRange(0, collection);
                    }
                    foreach (int blocks in lz4Blocks)
                    {
                        LZ4ChainDecoder lz4ChainDecoder = new LZ4ChainDecoder(65536, 0);
                        byte[] blockarray = new byte[binaryReader.ReadUInt32()];

                        int num1 = 0;
                        int num2 = 0;
                        for (int i = 0; i < blocks - 4; i += num1 + 4)
                        {
                            num1 = binaryReader.ReadInt32() & 0x7FFFFFFF;
                            byte[] numArray = binaryReader.ReadBytes(num1);
                            byte[] buffer = new byte[65536];
                            LZ4EncoderExtensions.DecodeAndDrain(lz4ChainDecoder, numArray, 0, num1, buffer, 0, 65536, out int num3);
                            Array.Copy(buffer, 0, blockarray, num2, num3);
                            num2 += num3;
                        }
                        list.InsertRange(0, blockarray);
                    }
                }
                Data = list.ToArray();
            }
            bitmaps = new List<byte[]>();
            bitmaps = GetMipMapData();
        }
        public DDS(PAA paa, Stream fs)
        {
            BinaryReaderEx input = new BinaryReaderEx(fs);
            Header = new DDSHeader
            {
                Size = 124,
                Flags = DdsFileHeaderFlags.Texture,
                Height = (int)paa.Height,
                Width = (int)paa.Width,
                Depth = (int)1,
                MipMapCount = (int)paa.mipmaps.Count,
                Caps = (DdsSurfaceFlags.Texture)
            };
            if (Header.MipMapCount > 1)
            {
                Header.Flags |= DdsFileHeaderFlags.MipMap;
                Header.Caps |= DdsSurfaceFlags.MipMap;
            }
            switch (paa.Type)
            {
                //case PAAType.DXT1;
                //    Header.PixelFormat = DdsPixelFormat.DdsPfA8R8G8B8();
                //    Header.PitchOrLinearSize = (Header.Width * 32 + 7) / 8;
                //    break;
                //case 1:
                //    Header.PixelFormat = DdsPixelFormat.DdsLuminance();
                //    Header.PitchOrLinearSize = (Header.Width * 32 + 7) / 8; ;
                //    break;
                case PAAType.DXT1:
                    Header.PixelFormat = DdsPixelFormat.DdsPfDxt1();
                    Header.PitchOrLinearSize = (int)paa.mipmaps[0].GetRawPixelData(input, paa.Type).Length;
                    break;
                //case 3:
                //    Header.PixelFormat = DdsPixelFormat.DdsPfDxt3();
                //    Header.PitchOrLinearSize = (int)ftexfile.mipmapinfo[0].DecompressedFileSize;
                //    break;
                case PAAType.DXT5:
                    Header.PixelFormat = DdsPixelFormat.DdsPfDxt5();
                    Header.PitchOrLinearSize = (int)paa.mipmaps[0].GetRawPixelData(input, paa.Type).Length;
                    break;
                //case 9:
                //    Header.PixelFormat = DdsPixelFormat.DdsPfDxt7();
                //    Header.PitchOrLinearSize = (int)ftexfile.mipmapinfo[0].DecompressedFileSize;
                //    break;
                //case 11:
                //    Header.PixelFormat = DdsPixelFormat.DdsPfDx10();
                //    Header.PitchOrLinearSize = (int)ftexfile.mipmapinfo[0].DecompressedFileSize;
                //    dx10head = new DX10Header(ftexfile);
                //    break;
                //case 12:
                //    Header.Flags |= DdsFileHeaderFlags.LinearSize;
                //    Header.PixelFormat = DdsPixelFormat.DdsPfA16R16G16B16f();
                //    Header.PitchOrLinearSize = (int)ftexfile.mipmapinfo[0].DecompressedFileSize;
                //    break;
                default:
                    throw new ArgumentException($"Unknown PixelFormatType {paa.Type}");
            }
            Data = new byte[0];
            foreach (Mipmap fmp in paa.mipmaps)
            {
                byte[] mipmap = fmp.GetRawPixelData(input, paa.Type);
                int datal = Data.Length;
                byte[] newarray = new byte[Data.Length + mipmap.Length];
                if (datal > 0)
                    Array.Copy(Data, 0, newarray, 0, datal);
                Array.Copy(mipmap, 0, newarray, datal, mipmap.Length);
                Data = newarray;
            }
        }
        public bool write(string path)
        {
            using (BinaryWriter bw = new BinaryWriter(new FileStream(path, FileMode.Create, FileAccess.Write)))
            {

                bw.Write(0x20534444);
                Header.writeheader(bw);
                if (Header.IsDx10())
                {
                    dx10head.writeheader(bw);
                }
                bw.Write(Data);

            }
            return true;
        }
        public List<byte[]> GetMipMapData()
        {
            const int minimumWidth = 1;
            const int minimumHeight = 1;
            List<byte[]> mipMapDatas = new List<byte[]>();
            int dataOffset = 0;
            int width = Header.Width;
            int height = Header.Height;
            int depth = Header.Depth == 0 ? 1 : Header.Depth;
            int mipMapsCount = Header.Flags.HasFlag(DdsFileHeaderFlags.MipMap) ? Header.MipMapCount : 1;
            for (int i = 0; i < mipMapsCount; i++)
            {
                int size = DdsPixelFormat.CalculateImageSize(Header.PixelFormat, width, height, depth);
                var buffer = new byte[size];
                Array.Copy(Data, dataOffset, buffer, 0, size);
                mipMapDatas.Add(buffer);
                dataOffset += size;
                width = Math.Max(width / 2, minimumWidth);
                height = Math.Max(height / 2, minimumHeight);
            }
            return mipMapDatas;
        }
        public Bitmap GetBitmap()
        {
            if ((bitmaps != null) && (bitmaps.Count >= 1))
            {
                EImageType type = EImageType.DXT1;
                switch (Header.PixelFormat.FourCc)
                {
                    case 0x31545844:
                        type = EImageType.DXT1;
                        break;
                    case 0x33545844:
                        type = EImageType.DXT3;
                        break;
                    case 0x35545844:
                        type = EImageType.DXT5;
                        break;
                    case 0x00000000:
                        type = EImageType.A8R8G8B8;
                        break;
                }
                RawImage ri = new RawImage(Header.Width, Header.Height, type, bitmaps[0].Length);
                ri.Load(new BinaryReader(new MemoryStream(bitmaps[0])));
                return (ri.Bitmap);
            }
            else
            {
                return (null);
            }
        }
        public void FindBlocks(BinaryReader reader)
        {
            while (true)
            {
                byte[] bytes = reader.ReadBytes(4);
                string text = new string(Encoding.UTF8.GetChars(bytes));
                int item = reader.ReadInt32();
                if (!(text == "COPY"))
                {
                    if (!(text == "LZ4 "))
                    {
                        break;
                    }
                    lz4Blocks.Add(item);
                }
                else
                {
                    copyBlocks.Add(item);
                }
            }
            reader.BaseStream.Seek(-8L, SeekOrigin.Current);
        }
    }
    public class DX10Header
    {
        public DxgiFormat Format { get; set; }
        public D3D10ResourceDimension ResourceDimension { get; set; }
        public uint MiscFlag { get; set; }
        public uint ArraySize { get; set; }
        public uint MiscFlag2 { get; set; }

        public DX10Header(BinaryReader br)
        {
            Format = (DxgiFormat)br.ReadUInt32();
            ResourceDimension = (D3D10ResourceDimension)br.ReadInt32();
            MiscFlag = br.ReadUInt32();
            ArraySize = br.ReadUInt32();
            MiscFlag2 = br.ReadUInt32();
        }
        public void writeheader(BinaryWriter bw)
        {
            bw.Write(Convert.ToUInt32(Format));
            bw.Write(Convert.ToInt32(ResourceDimension));
            bw.Write(MiscFlag);
            bw.Write(ArraySize);
            bw.Write(MiscFlag2);
        }
    }
    public class DDSHeader
    {
        
        public const int DefaultHeaderSize = 124;
        public int Size { get; set; }
        public DdsFileHeaderFlags Flags { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public int PitchOrLinearSize { get; set; }
        public int Depth { get; set; }
        public int MipMapCount { get; set; }
        public DdsPixelFormat PixelFormat { get; set; }
        public DdsSurfaceFlags Caps { get; set; }
        public DdsCubemap Caps2 { get; set; }
        public int Caps3 { get; set; }
        public int Caps4 { get; set; }

        public DDSHeader() { }
        public DDSHeader(BinaryReader br)
        {
            Size = br.ReadInt32();
            Flags = (DdsFileHeaderFlags)br.ReadInt32();
            Height = br.ReadInt32();
            Width = br.ReadInt32();
            PitchOrLinearSize = br.ReadInt32();
            Depth = br.ReadInt32();
            MipMapCount = br.ReadInt32();
            br.ReadBytes(44);
            PixelFormat = new DdsPixelFormat(br);
            Caps = (DdsSurfaceFlags)br.ReadInt32();
            Caps2 = (DdsCubemap)br.ReadInt32();
            Caps3 = br.ReadInt32();
            Caps4 = br.ReadInt32();
            br.ReadBytes(4);
        }
        public void writeheader(BinaryWriter bw)
        {
            bw.Write(Size);
            bw.Write(Convert.ToInt32(Flags));
            bw.Write(Height);
            bw.Write(Width);
            bw.Write(PitchOrLinearSize);
            bw.Write(Depth);
            bw.Write(MipMapCount);
            // int Reserved1[11];
            WriteZeros(bw, 44);
            PixelFormat.Write(bw);
            bw.Write(Convert.ToInt32(Caps));
            bw.Write(Convert.ToInt32(Caps2));
            bw.Write(Caps3);
            bw.Write(Caps4);
            // int Reserved2
            WriteZeros(bw, 4);
        }
        public void WriteZeros(BinaryWriter writer, int count)
        {
            byte[] zeros = new byte[count];
            writer.Write(zeros);
        }
        public bool IsDx10()
        {
            return PixelFormat != null &&
                   PixelFormat.Flags.HasFlag(DdsPixelFormatFlag.FourCc) &&
                   PixelFormat.FourCc == DdsPixelFormat.Dx10FourCc;
        }
    }
    public class DdsPixelFormat
    {
        private const int DefaultSize = 32;
        internal const int Dxt1FourCc = 0x31545844;
        internal const int Dxt2FourCc = 0x32545844;
        internal const int Dxt3FourCc = 0x33545844;
        internal const int Dtx4FourCc = 0x34545844;
        internal const int Dtx5FourCc = 0x35545844;
        internal const int Dtx7FourCc = 0x55354342;
        internal const int Dx10FourCc = 0x30315844;
        internal const int dgxirgba16f = 0x00000071;

        public int Size { get; set; }
        public DdsPixelFormatFlag Flags { get; set; }
        public int FourCc { get; set; }
        public int RgbBitCount { get; set; }
        public uint RBitMask { get; set; }
        public uint GBitMask { get; set; }
        public uint BBitMask { get; set; }
        public uint ABitMask { get; set; }

        public DdsPixelFormat() { }
        public DdsPixelFormat(BinaryReader br)
        {
            Size = br.ReadInt32();
            Flags = (DdsPixelFormatFlag)br.ReadUInt32();
            FourCc = br.ReadInt32();
            RgbBitCount = br.ReadInt32();
            RBitMask = br.ReadUInt32();
            GBitMask = br.ReadUInt32();
            BBitMask = br.ReadUInt32();
            ABitMask = br.ReadUInt32();
        }
        public void Write(BinaryWriter bw)
        {
            bw.Write(Size);
            bw.Write(Convert.ToUInt32(Flags));
            bw.Write(FourCc);
            bw.Write(RgbBitCount);
            bw.Write(RBitMask);
            bw.Write(GBitMask);
            bw.Write(BBitMask);
            bw.Write(ABitMask);
        }
        public static DdsPixelFormat DdsPfDxt1()
        {
            return DdsPfDx(Dxt1FourCc); // DXT1
        }

        public static DdsPixelFormat DdsPfDxt2()
        {
            return DdsPfDx(Dxt2FourCc); // DXT2
        }

        public static DdsPixelFormat DdsPfDxt3()
        {
            return DdsPfDx(Dxt3FourCc); // DXT3
        }

        public static DdsPixelFormat DdsPfDxt4()
        {
            return DdsPfDx(Dtx4FourCc); // DXT4
        }

        public static DdsPixelFormat DdsPfDxt5()
        {
            return DdsPfDx(Dtx5FourCc); // DXT5
        }

        public static DdsPixelFormat DdsPfA8R8G8B8()
        {
            DdsPixelFormat pixelFormat = new DdsPixelFormat
            {
                Size = DefaultSize,
                Flags = DdsPixelFormatFlag.Rgba,
                FourCc = 0,
                RgbBitCount = 32,
                RBitMask = 0x00ff0000,
                GBitMask = 0x0000ff00,
                BBitMask = 0x000000ff,
                ABitMask = 0xff000000
            };
            return pixelFormat;
        }
        public static DdsPixelFormat DdsPfA16R16G16B16f()
        {
            DdsPixelFormat pixelFormat = new DdsPixelFormat
            {
                Size = DefaultSize,
                Flags = DdsPixelFormatFlag.FourCc,
                FourCc = 113
            };
            return pixelFormat;
        }
        public static DdsPixelFormat DdsPfA1R5G5B5()
        {
            DdsPixelFormat pixelFormat = new DdsPixelFormat
            {
                Size = DefaultSize,
                Flags = DdsPixelFormatFlag.Rgba,
                FourCc = 0,
                RgbBitCount = 16,
                RBitMask = 0x00007c00,
                GBitMask = 0x000003e0,
                BBitMask = 0x0000001f,
                ABitMask = 0x00008000
            };
            return pixelFormat;
        }

        public static DdsPixelFormat DdsPfA4R4G4B4()
        {
            DdsPixelFormat pixelFormat = new DdsPixelFormat
            {
                Size = DefaultSize,
                Flags = DdsPixelFormatFlag.Rgba,
                FourCc = 0,
                RgbBitCount = 16,
                RBitMask = 0x00000f00,
                GBitMask = 0x000000f0,
                BBitMask = 0x0000000f,
                ABitMask = 0x0000f000
            };
            return pixelFormat;
        }

        public static DdsPixelFormat DdsPfR8G8B8()
        {
            DdsPixelFormat pixelFormat = new DdsPixelFormat
            {
                Size = DefaultSize,
                Flags = DdsPixelFormatFlag.Rgb,
                FourCc = 0,
                RgbBitCount = 24,
                RBitMask = 0x00ff0000,
                GBitMask = 0x0000ff00,
                BBitMask = 0x000000ff,
                ABitMask = 0x00000000
            };
            return pixelFormat;
        }

        public static DdsPixelFormat DdsPfR5G6B5()
        {
            DdsPixelFormat pixelFormat = new DdsPixelFormat
            {
                Size = DefaultSize,
                Flags = DdsPixelFormatFlag.Rgb,
                FourCc = 0,
                RgbBitCount = 16,
                RBitMask = 0x0000f800,
                GBitMask = 0x000007e0,
                BBitMask = 0x0000001f,
                ABitMask = 0x00000000
            };
            return pixelFormat;
        }

        public static DdsPixelFormat DdsPfDxt7()
        {
            return DdsPfDx(Dtx7FourCc);
        }
        public static DdsPixelFormat DdsPfDx10()
        {
            return DdsPfDx(Dx10FourCc); // DX10
        }

        private static DdsPixelFormat DdsPfDx(int fourCc)
        {
            DdsPixelFormat pixelFormat = new DdsPixelFormat
            {
                Size = DefaultSize,
                Flags = DdsPixelFormatFlag.FourCc,
                FourCc = fourCc
            };
            return pixelFormat;
        }

        public static DdsPixelFormat DdsLuminance()
        {
            DdsPixelFormat pixelFormat = new DdsPixelFormat
            {
                Size = DefaultSize,
                Flags = DdsPixelFormatFlag.Luminance,
                RgbBitCount = 8,
                RBitMask = 0x000000ff
            };
            return pixelFormat;
        }
        public static int CalculateImageSize(DdsPixelFormat pixelFormat, int width, int height, int depth)
        {
            if (pixelFormat.FourCc == 0x31545844
                    && pixelFormat.Flags == DdsPixelFormatFlag.FourCc
                    && pixelFormat.Size == 32)
                return (int)((long)width * height * depth) / 2; // ((width*height*32)/8)/8;
            else if (pixelFormat.FourCc == 0x33545844
                    && pixelFormat.Flags == DdsPixelFormatFlag.FourCc
                    && pixelFormat.Size == 32)
                return (width * height * depth); // ((width*height*32)/4)/8;
            else if (pixelFormat.FourCc == 0x35545844
                    && pixelFormat.Flags == DdsPixelFormatFlag.FourCc
                    && pixelFormat.Size == 32)
                return (width * height * depth); // ((width*height*32)/4)/8;
            else if (pixelFormat.FourCc == 0x55354342
                    && pixelFormat.Flags == DdsPixelFormatFlag.FourCc
                    && pixelFormat.Size == 32)
                return (width * height * depth); // ((width*height*32)/4)/8;
            else if (pixelFormat.FourCc == 0x30315844
                   && pixelFormat.Flags == DdsPixelFormatFlag.FourCc
                   && pixelFormat.Size == 32)
                return (width * height * depth); // ((width*height*32)/4)/8;
            else if (pixelFormat.FourCc == 0x00000071
                   && pixelFormat.Flags == DdsPixelFormatFlag.FourCc
                   && pixelFormat.Size == 32)
                return (width * height * depth * 8); // ((width*height*32)/4)/8;
            else if (pixelFormat.RgbBitCount > 0)
                return (int)((long)width * height * depth * pixelFormat.RgbBitCount / 8);
            throw new ArgumentException("Could not calculate the image size of the current pixel format.");
        }
    }
    public enum DdsFileHeaderFlags
    {
        /// <summary>
        ///     DDSD_CAPS | DDSD_HEIGHT | DDSD_WIDTH | DDSD_PIXELFORMAT
        /// </summary>
        Texture = 0x00001007,

        /// <summary>
        ///     DDSD_MIPMAPCOUNT
        /// </summary>
        MipMap = 0x00020000,

        /// <summary>
        ///     DDSD_DEPTH
        /// </summary>
        Volume = 0x00800000,

        /// <summary>
        ///     DDSD_PITCH
        /// </summary>
        Pitch = 0x00000008,

        /// <summary>
        ///     DDSD_LINEARSIZE
        /// </summary>
        LinearSize = 0x00080000
    }
    public enum DdsPixelFormatFlag : uint
    {
        /// <summary>
        ///     DDPF_ALPHA
        /// </summary>
        Alpha = 0x00000002,

        /// <summary>
        ///     DDPF_FOURCC
        /// </summary>
        FourCc = 0x00000004,

        /// <summary>
        ///     DDPF_RGB
        /// </summary>
        Rgb = 0x00000040,

        /// <summary>
        ///     DDPF_RGB | DDPF_ALPHAPIXELS
        /// </summary>
        Rgba = 0x00000041,

        /// <summary>
        ///     DDPF_LUMINANCE
        /// </summary>
        Luminance = 0x00020000,

        /// <summary>
        ///     Nvidia custom DDPF_NORMA
        /// </summary>
        Normal = 0x80000000
    }
    public enum DdsSurfaceFlags
    {
        /// <summary>
        ///     DDSCAPS_TEXTURE
        /// </summary>
        Texture = 0x00001000,

        /// <summary>
        ///     DDSCAPS_COMPLEX | DDSCAPS_MIPMAP
        /// </summary>
        MipMap = 0x00400008,

        /// <summary>
        ///     DDSCAPS_COMPLEX
        /// </summary>
        CubeMap = 0x00000008
    }
    public enum DdsCubemap
    {
        /// <summary>
        ///     DDSCAPS2_CUBEMAP | DDSCAPS2_CUBEMAP_POSITIVEX
        /// </summary>
        PositiveX = 0x00000600,

        /// <summary>
        ///     DDSCAPS2_CUBEMAP | DDSCAPS2_CUBEMAP_NEGATIVEX
        /// </summary>
        NegativeX = 0x00000a00,

        /// <summary>
        ///     DDSCAPS2_CUBEMAP | DDSCAPS2_CUBEMAP_POSITIVEY
        /// </summary>
        PositiveY = 0x00001200,

        /// <summary>
        ///     DDSCAPS2_CUBEMAP | DDSCAPS2_CUBEMAP_NEGATIVEY
        /// </summary>
        NegativeY = 0x00002200,

        /// <summary>
        ///     DDSCAPS2_CUBEMAP | DDSCAPS2_CUBEMAP_POSITIVEZ
        /// </summary>
        PositiveZ = 0x00004200,

        /// <summary>
        ///     DDSCAPS2_CUBEMAP | DDSCAPS2_CUBEMAP_NEGATIVEZ
        /// </summary>
        NegativeZ = 0x00008200,

        /// <summary>
        ///     DDSCAPS2_CUBEMAP |
        ///     DDSCAPS2_CUBEMAP_POSITIVEX | DDSCAPS2_CUBEMAP_NEGATIVEX |
        ///     DDSCAPS2_CUBEMAP_POSITIVEY | DDSCAPS2_CUBEMAP_POSITIVEY |
        ///     DDSCAPS2_CUBEMAP_POSITIVEZ | DDSCAPS2_CUBEMAP_NEGATIVEZ
        /// </summary>
        AllFaces = PositiveX | NegativeX | PositiveY | NegativeY | PositiveZ | NegativeZ
    }
    public enum DxgiFormat : uint
    {
        Unknown = 0,
        R32G32B32A32Typeless = 1,
        R32G32B32A32Float = 2,
        R32G32B32A32Uint = 3,
        R32G32B32A32Sint = 4,
        R32G32B32Typeless = 5,
        R32G32B32Float = 6,
        R32G32B32Uint = 7,
        R32G32B32Sint = 8,
        R16G16B16A16Typeless = 9,
        R16G16B16A16Float = 10,
        R16G16B16A16Unorm = 11,
        R16G16B16A16Uint = 12,
        R16G16B16A16Snorm = 13,
        R16G16B16A16Sint = 14,
        R32G32Typeless = 15,
        R32G32Float = 16,
        R32G32Uint = 17,
        R32G32Sint = 18,
        R32G8X24Typeless = 19,
        D32FloatS8X24Uint = 20,
        R32FloatX8X24Typeless = 21,
        X32TypelessG8X24Uint = 22,
        R10G10B10A2Typeless = 23,
        R10G10B10A2Unorm = 24,
        R10G10B10A2Uint = 25,
        R11G11B10Float = 26,
        R8G8B8A8Typeless = 27,
        R8G8B8A8Unorm = 28,
        R8G8B8A8UnormSrgb = 29,
        R8G8B8A8Uint = 30,
        R8G8B8A8Snorm = 31,
        R8G8B8A8Sint = 32,
        R16G16Typeless = 33,
        R16G16Float = 34,
        R16G16Unorm = 35,
        R16G16Uint = 36,
        R16G16Snorm = 37,
        R16G16Sint = 38,
        R32Typeless = 39,
        D32Float = 40,
        R32Float = 41,
        R32Uint = 42,
        R32Sint = 43,
        R24G8Typeless = 44,
        D24UnormS8Uint = 45,
        R24UnormX8Typeless = 46,
        X24TypelessG8Uint = 47,
        R8G8Typeless = 48,
        R8G8Unorm = 49,
        R8G8Uint = 50,
        R8G8Snorm = 51,
        R8G8Sint = 52,
        R16Typeless = 53,
        R16Float = 54,
        D16Unorm = 55,
        R16Unorm = 56,
        R16Uint = 57,
        R16Snorm = 58,
        R16Sint = 59,
        R8Typeless = 60,
        R8Unorm = 61,
        R8Uint = 62,
        R8Snorm = 63,
        R8Sint = 64,
        A8Unorm = 65,
        R1Unorm = 66,
        R9G9B9E5Sharedexp = 67,
        R8G8B8G8Unorm = 68,
        G8R8G8B8Unorm = 69,
        Bc1Typeless = 70,
        Bc1Unorm = 71,
        Bc1UnormSrgb = 72,
        Bc2Typeless = 73,
        Bc2Unorm = 74,
        Bc2UnormSrgb = 75,
        Bc3Typeless = 76,
        Bc3Unorm = 77,
        Bc3UnormSrgb = 78,
        Bc4Typeless = 79,
        Bc4Unorm = 80,
        Bc4Snorm = 81,
        Bc5Typeless = 82,
        Bc5Unorm = 83,
        Bc5Snorm = 84,
        B5G6R5Unorm = 85,
        B5G5R5A1Unorm = 86,
        B8G8R8A8Unorm = 87,
        B8G8R8X8Unorm = 88,
        R10G10B10XrBiasA2Unorm = 89,
        B8G8R8A8Typeless = 90,
        B8G8R8A8UnormSrgb = 91,
        B8G8R8X8Typeless = 92,
        B8G8R8X8UnormSrgb = 93,
        Bc6HTypeless = 94,
        Bc6HUf16 = 95,
        Bc6HSf16 = 96,
        Bc7Typeless = 97,
        Bc7Unorm = 98,
        Bc7UnormSrgb = 99,
        Ayuv = 100,
        Y410 = 101,
        Y416 = 102,
        Nv12 = 103,
        P010 = 104,
        P016 = 105,
        Opaque420 = 106, // 420_OPAQUE
        Yuy2 = 107,
        Y210 = 108,
        Y216 = 109,
        Nv11 = 110,
        Ai44 = 111,
        Ia44 = 112,
        P8 = 113,
        A8P8 = 114,
        B4G4R4A4Unorm = 115
    }
    public enum D3D10ResourceDimension
    {
        Unknown = 0,
        Buffer = 1,
        Texture1D = 2,
        Texture2D = 3,
        Texture3D = 4
    }
    public enum EImageType
    {
        DXT1 = 0, // NFS6 DXT1 packed bitmap
        DXT3 = 1, // NFS6 DXT5 packed bitmap
        DXT5 = 2, // NFS6 DXT5 packed bitmap
        A8R8G8B8 = 3, // 32-bit bitmap 8A 8R 8G 8B
        GREY8 = 4, // 32-bit bitmap 8A 8R 8G 8B
        GREY8ALFA8 = 5, // 8-bit bitmap 
        DC_XY_NORMAL_MAP = 7,

        A4R4G4B4 = 0x6d, // a4 r4 g4 b4
        R5G6B5 = 0x78, // 15-bit bitmap R5 G6 B5
        X1R5G5B5 = 0x7e, // X1 R5 G5 B5
        BIT8 = 0x7b, // 8-bit bitmap 
        R8G8B8 = 0x7f, // 24-bit bitmap 8R 8G 8B
    }
    public class RawImage
    {
        private int m_Width;
        private int m_Height;
        private EImageType m_ImageType;

        protected bool m_SwapEndian;

        private Bitmap m_Bitmap;
        public Bitmap Bitmap
        {
            get
            {
                if (m_Bitmap == null)
                {
                    // Create the bitmap
                    CreateBitmap();
                }
                return m_Bitmap;
            }
            set
            {
                m_Bitmap = value;
                m_NeedToSaveRawData = true;
            }
        }

        private bool m_NeedToSaveRawData;

        private byte[] m_RawData;

        protected int m_Size;

        public RawImage(int width, int height, EImageType dxtType, int size)
        {
            m_Width = width;
            m_Height = height;
            m_ImageType = dxtType;
            m_Size = size;
            m_Bitmap = null;
        }

        public bool Load(BinaryReader r)
        {
            int size = m_Size;
            if (m_ImageType == EImageType.A8R8G8B8)
            {
                size = m_Width * m_Height * 4;
            }
            //if (m_ImageType == EImageType.DC_XY_NORMAL_MAP)
            //{

            //} 
            m_RawData = r.ReadBytes(size);
            m_NeedToSaveRawData = false;
            return (true);
        }

        public bool Save(BinaryWriter w)
        {
            if (m_NeedToSaveRawData)
            {
                CreateRawData();
                m_NeedToSaveRawData = false;
            }

            w.Write(m_RawData);
            return (true);
        }

        private void CreateBitmap()
        {
            if (m_Width < 1) m_Width = 1;
            if (m_Height < 1) m_Height = 1;

            switch (m_ImageType)
            {
                case EImageType.DXT1:
                case EImageType.DXT3:
                case EImageType.DXT5:

                    m_Bitmap = new Bitmap(m_Width, m_Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                    ReadDxtToBitmap();
                    break;
                case EImageType.A8R8G8B8:
                    m_Bitmap = new Bitmap(m_Width, m_Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                    Rectangle rect1 = new Rectangle(0, 0, m_Bitmap.Width, m_Bitmap.Height);
                    System.Drawing.Imaging.BitmapData bmpData1 = m_Bitmap.LockBits(rect1,
                        System.Drawing.Imaging.ImageLockMode.WriteOnly,
                        m_Bitmap.PixelFormat);
                    // Get the address of the first line.
                    IntPtr ptr1 = bmpData1.Scan0;

                    // This code is specific to a bitmap with 32 bits per pixels.
                    int nPixels1 = m_Bitmap.Width * m_Bitmap.Height;
                    System.Runtime.InteropServices.Marshal.Copy(m_RawData, 0, ptr1, nPixels1 * 4);
                    m_Bitmap.UnlockBits(bmpData1);
                    break;
                case EImageType.GREY8:
                    m_Bitmap = new Bitmap(m_Width, m_Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                    int index = 0;
                    for (int k = 0; k < m_Bitmap.Height; k++)
                    {
                        for (int j = 0; j < m_Bitmap.Width; j++)
                        {
                            byte color = m_RawData[index++];

                            int a = 255;
                            int r = color;
                            int g = color;
                            int b = color;
                            m_Bitmap.SetPixel(j, k, Color.FromArgb(a, r, g, b));
                        }
                    }
                    break;

                case EImageType.GREY8ALFA8:
                    m_Bitmap = new Bitmap(m_Width, m_Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                    index = 0;
                    for (int k = 0; k < m_Bitmap.Height; k++)
                    {
                        for (int j = 0; j < m_Bitmap.Width; j++)
                        {
                            byte color = m_RawData[index++];
                            byte alfa = m_RawData[index++];

                            int a = alfa;
                            int r = color;
                            int g = color;
                            int b = color;
                            m_Bitmap.SetPixel(j, k, Color.FromArgb(a, r, g, b));
                        }
                    }
                    break;
                case EImageType.DC_XY_NORMAL_MAP:
                    m_Bitmap = new Bitmap(m_Width, m_Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                    ReadDxtToBitmap();
                    break;
                default:
                    break;
            }
        }

        private void CreateRawData()
        {
            if (m_Width < 1) m_Width = 1;
            if (m_Height < 1) m_Height = 1;
            switch (m_ImageType)
            {
                case EImageType.DXT1:
                case EImageType.DXT3:
                case EImageType.DXT5:
                case EImageType.DC_XY_NORMAL_MAP:

                    WriteBitmapToDxt();
                    break;
                case EImageType.A8R8G8B8:
                    WriteBitmapToA8R8G8B8();
                    break;

                case EImageType.GREY8:
                    WriteBitmapToGrey8();
                    break;

                case EImageType.GREY8ALFA8:
                    WriteBitmapToGrey8Alfa8();
                    break;

                default:
                    break;
            }
        }

        // Read dxt data from the raw data and fill the bitmap
        private void ReadDxtToBitmap()
        {
            DxtBlock dxt = new DxtBlock((int)m_ImageType);

            MemoryStream ms = new MemoryStream(m_RawData);
            BinaryReader mr = new BinaryReader(ms);

            // Lock the bitmap data
            Rectangle rect = new Rectangle(0, 0, m_Width, m_Height);
            System.Drawing.Imaging.BitmapData bmpData = m_Bitmap.LockBits(rect,
                System.Drawing.Imaging.ImageLockMode.WriteOnly,
                m_Bitmap.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            int nPixels = m_Bitmap.Width * m_Bitmap.Height;
            int[] bitmapRawData = new int[nPixels];


            for (int k = 0; k < m_Bitmap.Height / 4; k++)
            {
                for (int j = 0; j < m_Bitmap.Width / 4; j++)
                {
                    dxt.Load(mr);

                    int address = (k * 4) * m_Bitmap.Width;
                    address += j * 4;
                    bitmapRawData[address] = dxt.Colors[0, 0].ToArgb();
                    bitmapRawData[address + 1] = dxt.Colors[1, 0].ToArgb();
                    bitmapRawData[address + 2] = dxt.Colors[2, 0].ToArgb();
                    bitmapRawData[address + 3] = dxt.Colors[3, 0].ToArgb();

                    address += m_Bitmap.Width;
                    bitmapRawData[address] = dxt.Colors[0, 1].ToArgb();
                    bitmapRawData[address + 1] = dxt.Colors[1, 1].ToArgb();
                    bitmapRawData[address + 2] = dxt.Colors[2, 1].ToArgb();
                    bitmapRawData[address + 3] = dxt.Colors[3, 1].ToArgb();

                    address += m_Bitmap.Width;
                    bitmapRawData[address] = dxt.Colors[0, 2].ToArgb();
                    bitmapRawData[address + 1] = dxt.Colors[1, 2].ToArgb();
                    bitmapRawData[address + 2] = dxt.Colors[2, 2].ToArgb();
                    bitmapRawData[address + 3] = dxt.Colors[3, 2].ToArgb();

                    address += m_Bitmap.Width;
                    bitmapRawData[address] = dxt.Colors[0, 3].ToArgb();
                    bitmapRawData[address + 1] = dxt.Colors[1, 3].ToArgb();
                    bitmapRawData[address + 2] = dxt.Colors[2, 3].ToArgb();
                    bitmapRawData[address + 3] = dxt.Colors[3, 3].ToArgb();
                }
            }
            System.Runtime.InteropServices.Marshal.Copy(bitmapRawData, 0, ptr, nPixels);
            m_Bitmap.UnlockBits(bmpData);
            bitmapRawData = null;

            mr.Close();
            ms.Close();

        }

        private void WriteBitmapToA8R8G8B8()
        {
            if ((m_Bitmap.Height * m_Bitmap.Width * 4) > m_RawData.Length)
            {
                return;
            }
            // trasforma Bitmap A8R8G8B8 in raw data
            int index = 0;
            for (int k = 0; k < m_Bitmap.Height; k++)
            {
                for (int j = 0; j < m_Bitmap.Width; j++)
                {
                    Color color = m_Bitmap.GetPixel(j, k);
                    byte b = color.B;
                    byte g = color.G;
                    byte r = color.R;
                    byte a = color.A;
                    m_RawData[index++] = b;
                    m_RawData[index++] = g;
                    m_RawData[index++] = r;
                    m_RawData[index++] = a;
                }
            }
        }

        private void WriteBitmapToDxt()
        {

            MemoryStream ms = new MemoryStream(m_RawData);
            BinaryWriter bw = new BinaryWriter(ms);

            DxtBlock dxt = new DxtBlock((int)m_ImageType);
            int kmax = (m_Bitmap.Height + 3) / 4;
            int jmax = (m_Bitmap.Width + 3) / 4;

            if ((m_Bitmap.Height < 4) || (m_Bitmap.Width < 4))
            {
                dxt.Colors[0, 0] = Color.FromArgb(0, 128, 128, 128);
                dxt.Colors[0, 1] = Color.FromArgb(0, 128, 128, 128);
                dxt.Colors[0, 2] = Color.FromArgb(0, 128, 128, 128);
                dxt.Colors[0, 3] = Color.FromArgb(0, 128, 128, 128);
                dxt.Colors[1, 0] = Color.FromArgb(0, 128, 128, 128);
                dxt.Colors[1, 1] = Color.FromArgb(0, 128, 128, 128);
                dxt.Colors[1, 2] = Color.FromArgb(0, 128, 128, 128);
                dxt.Colors[1, 3] = Color.FromArgb(0, 128, 128, 128);
                dxt.Colors[2, 0] = Color.FromArgb(0, 128, 128, 128);
                dxt.Colors[2, 1] = Color.FromArgb(0, 128, 128, 128);
                dxt.Colors[2, 2] = Color.FromArgb(0, 128, 128, 128);
                dxt.Colors[2, 3] = Color.FromArgb(0, 128, 128, 128);
                dxt.Colors[3, 0] = Color.FromArgb(0, 128, 128, 128);
                dxt.Colors[3, 1] = Color.FromArgb(0, 128, 128, 128);
                dxt.Colors[3, 2] = Color.FromArgb(0, 128, 128, 128);
                dxt.Colors[3, 3] = Color.FromArgb(0, 128, 128, 128);

                for (int i = 0; i < m_Bitmap.Width; i++)
                {
                    for (int j = 0; j < m_Bitmap.Height; j++)
                    {
                        if ((i >= 0) && (j >= 0) && (i < 4) && (j < 4))
                        {
                            dxt.Colors[i, j] = m_Bitmap.GetPixel(i, j);
                        }
                    }
                }

                // Sembra indifferente salvare o nn s
                dxt.Save(bw);

            }
            else
            {
                // gestire caso minore di 4 x 4
                for (int k = 0; k < kmax; k++)
                {
                    int ki = k * 4;
                    for (int j = 0; j < jmax; j++)
                    {
                        int ji = j * 4;
                        dxt.Colors[0, 0] = m_Bitmap.GetPixel(ji + 0, ki + 0);
                        dxt.Colors[0, 1] = m_Bitmap.GetPixel(ji + 0, ki + 1);
                        dxt.Colors[0, 2] = m_Bitmap.GetPixel(ji + 0, ki + 2);
                        dxt.Colors[0, 3] = m_Bitmap.GetPixel(ji + 0, ki + 3);
                        dxt.Colors[1, 0] = m_Bitmap.GetPixel(ji + 1, ki + 0);
                        dxt.Colors[1, 1] = m_Bitmap.GetPixel(ji + 1, ki + 1);
                        dxt.Colors[1, 2] = m_Bitmap.GetPixel(ji + 1, ki + 2);
                        dxt.Colors[1, 3] = m_Bitmap.GetPixel(ji + 1, ki + 3);
                        dxt.Colors[2, 0] = m_Bitmap.GetPixel(ji + 2, ki + 0);
                        dxt.Colors[2, 1] = m_Bitmap.GetPixel(ji + 2, ki + 1);
                        dxt.Colors[2, 2] = m_Bitmap.GetPixel(ji + 2, ki + 2);
                        dxt.Colors[2, 3] = m_Bitmap.GetPixel(ji + 2, ki + 3);
                        dxt.Colors[3, 0] = m_Bitmap.GetPixel(ji + 3, ki + 0);
                        dxt.Colors[3, 1] = m_Bitmap.GetPixel(ji + 3, ki + 1);
                        dxt.Colors[3, 2] = m_Bitmap.GetPixel(ji + 3, ki + 2);
                        dxt.Colors[3, 3] = m_Bitmap.GetPixel(ji + 3, ki + 3);

                        dxt.Save(bw);
                    }
                }
            }
            bw.Close();
            ms.Close();
            return;
        }

        private void WriteBitmapToGrey8()
        {
            if ((m_Bitmap.Height * m_Bitmap.Width) > m_RawData.Length)
            {
                return;
            }
            // trasforma Bitmap A8R8G8B8 in raw data
            int index = 0;
            for (int k = 0; k < m_Bitmap.Height; k++)
            {
                for (int j = 0; j < m_Bitmap.Width; j++)
                {
                    Color color = m_Bitmap.GetPixel(j, k);
                    byte b = color.B;
                    m_RawData[index++] = b;
                }
            }
        }

        private void WriteBitmapToGrey8Alfa8()
        {
            if ((m_Bitmap.Height * m_Bitmap.Width * 4) > m_RawData.Length)
            {
                return;
            }
            // trasforma Bitmap A8R8G8B8 in raw data
            int index = 0;
            for (int k = 0; k < m_Bitmap.Height; k++)
            {
                for (int j = 0; j < m_Bitmap.Width; j++)
                {
                    Color color = m_Bitmap.GetPixel(j, k);
                    byte b = color.B;
                    m_RawData[index++] = b;
                }
            }
        }

    }
    ///////////////////////////////////////////////////////////////////////////
    /// @class MipMap
    /// @brief A RawImage preceded by a MipMap header
    ///////////////////////////////////////////////////////////////////////////
    public class MipMap : RawImage
    {
        private int m_Unknown0;
        private int m_Unknown4;
        private int m_UnknownC;

        public MipMap(int width, int height, EImageType dxtType, bool swapEndian)
            : base(width, height, dxtType, 0)
        {
            m_SwapEndian = swapEndian;
        }

        public new bool Load(BinaryReader r)
        {
            if (m_SwapEndian)
            {
                m_Unknown0 = Helper.SwapEndian(r.ReadInt32());
                m_Unknown4 = Helper.SwapEndian(r.ReadInt32());
                m_Size = Helper.SwapEndian(r.ReadInt32());
                m_UnknownC = Helper.SwapEndian(r.ReadInt32());

                base.Load(r);
            }
            else
            {
                m_Unknown0 = (r.ReadInt32()); ;
                m_Unknown4 = (r.ReadInt32()); ;
                m_Size = (r.ReadInt32()); ;
                m_UnknownC = (r.ReadInt32()); ;

                base.Load(r);
            }
            return (true);
        }

        public new bool Save(BinaryWriter w)
        {
            if (m_SwapEndian)
            {
                w.Write(Helper.SwapEndian(m_Unknown0));
                w.Write(Helper.SwapEndian(m_Unknown4));
                w.Write(Helper.SwapEndian(m_Size));
                w.Write(Helper.SwapEndian(m_UnknownC));

                base.Save(w);
            }
            else
            {
                w.Write((m_Unknown0));
                w.Write((m_Unknown4));
                w.Write((m_Size));
                w.Write((m_UnknownC));

                base.Save(w);
            }

            return (true);
        }
    }

    ///////////////////////////////////////////////////////////////////////////
    /// @class DxtBlock
    /// @brief A single dxt block of 4x4 pixels
    ///////////////////////////////////////////////////////////////////////////
    public class DxtBlock
    {
        private EImageType m_DxtType;   // dxt type
        private ushort m_Col0;  // color 0
        private ushort m_Col1;  // color 1
        private byte m_Alfa0;   // alfa 0
        private byte m_Alfa1;   // alfa 1
        private int[,] m_ColorLut;  // Color look up table 4x4
        private int[,] m_AlfaLut;   // alfa look up table 4x4
        private int[,] m_Alfa;      // alfa values
        private int[,] m_TempLut;   // temporary look up table 4x4

        private Color[,] m_Colors;  // pixel's color values 

        // set or get the color of a pixel in a 4x4 array
        public Color[,] Colors
        {
            get { return m_Colors; }
            set { m_Colors = value; }
        }

        // Create an empty dxt 
        public DxtBlock(int dxtType)
        {
            m_ColorLut = new int[4, 4];
            m_AlfaLut = new int[4, 4];
            m_Colors = new Color[4, 4];
            m_Alfa = new int[4, 4];
            Init(dxtType);
        }

        // Read a dxt block from a stream
        public DxtBlock(int dxtType, BinaryReader br)
        {
            m_DxtType = (EImageType)dxtType;
            m_ColorLut = new int[4, 4];
            m_Colors = new Color[4, 4];

            Load(br);
        }

        //Read dxt block data from a stream, the dxt type must be preset
        public void Load(BinaryReader br)
        {
            switch (m_DxtType)
            {
                case EImageType.DXT1:
                    // Read the 4 x 4 colors
                    ReadColorLut(br);
                    ConvertTo4x4();

                    break;
                case EImageType.DXT3:
                    // Read the 4 x 4 alpha channel
                    ReadAlfaChannel(br);
                    // Read the 4 x 4 colors
                    ReadColorLut(br);
                    ConvertTo4x4();

                    break;
                case EImageType.DXT5:
                    // Read the 4 x 4 alpha channel
                    ReadAlfaLut(br);
                    // Read the 4 x 4 colors
                    ReadColorLut(br);
                    ConvertTo4x4();

                    break;
                case EImageType.DC_XY_NORMAL_MAP:
                    ReadAlfaLut(br);
                    ConvertTo3DC(0);
                    ReadAlfaLut(br);
                    ConvertTo3DC(1);
                    break;
                default:
                    return;
            }

        }

        //Write dxt data in a stream, the dxt type must be preset
        public void Save(BinaryWriter bw)
        {

            switch (m_DxtType)
            {
                case EImageType.DXT1:
                    ConvertFrom4x4();
                    // Read the 4 x 4 colors
                    WriteColorLut(bw);
                    break;
                case EImageType.DXT3:
                    ConvertFrom4x4();
                    // Read the 4 x 4 alpha channel
                    WriteAlfaChannel(bw);
                    // Read the 4 x 4 colors
                    WriteColorLut(bw);
                    break;
                case EImageType.DXT5:
                    ConvertFrom4x4();
                    // Read the 4 x 4 alpha channel
                    WriteAlfaLut(bw);
                    // Read the 4 x 4 colors
                    WriteColorLut(bw);
                    break;
                case EImageType.DC_XY_NORMAL_MAP:
                    ConvertFrom3DC(0);
                    WriteAlfaLut(bw);
                    ConvertFrom3DC(1);
                    WriteAlfaLut(bw);
                    break;
                default:
                    return;
            }


        }

        private void Init(int dxtType)
        {
            m_DxtType = (EImageType)dxtType;
            m_Col0 = 0;
            m_Col1 = 0;

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    m_ColorLut[i, j] = 0;
                    m_Alfa[i, j] = 255;
                    m_Colors[i, j] = Color.FromArgb(0, 0, 0);
                }
            }
        }

        private void ReadAlfaChannel(BinaryReader br)
        {
            for (int i = 0; i < 4; i++)
            {
                int alfa = br.ReadInt16();

                m_Alfa[0, i] = (int)((alfa & 0x0f) << 4);
                m_Alfa[1, i] = (int)(((alfa >> 4) & 0x0f) << 4);
                m_Alfa[2, i] = (int)(((alfa >> 8) & 0x0f) << 4);
                m_Alfa[3, i] = (int)(((alfa >> 12) & 0x0f) << 4);
            }
        }
        private void ReadColorLut(BinaryReader br)
        {
            m_Col0 = br.ReadUInt16();
            m_Col1 = br.ReadUInt16();

            for (int i = 0; i < 4; i++)
            {
                byte mask = br.ReadByte();

                m_ColorLut[0, i] = (mask & 0x3);
                m_ColorLut[1, i] = ((mask >> 2) & 0x3);
                m_ColorLut[2, i] = ((mask >> 4) & 0x3);
                m_ColorLut[3, i] = ((mask >> 6) & 0x3);
            }
        }

        private void ReadNormalLut(BinaryReader br)
        {
            m_Col0 = br.ReadByte();
            m_Col1 = br.ReadByte();

            byte[] mask = br.ReadBytes(6);
            m_ColorLut[0, 0] = (mask[0] & 0x7);
            m_ColorLut[1, 0] = ((mask[0] >> 3) & 0x7);
            m_ColorLut[2, 0] = ((mask[0] >> 6) & 0x3) + ((mask[1] & 0x1) << 2);
            m_ColorLut[3, 0] = ((mask[1] >> 1) & 0x7);
            m_ColorLut[0, 1] = ((mask[1] >> 4) & 0x7);
            m_ColorLut[1, 1] = ((mask[1] >> 7) & 0x1) + ((mask[2] & 0x3) << 1); ;
            m_ColorLut[2, 1] = ((mask[2] >> 2) & 0x7);
            m_ColorLut[3, 1] = ((mask[2] >> 5) & 0x7);
            m_ColorLut[0, 2] = (mask[0] & 0x7);
            m_ColorLut[1, 2] = ((mask[3] >> 3) & 0x7);
            m_ColorLut[2, 2] = ((mask[3] >> 6) & 0x3) + ((mask[4] & 0x1) << 2);
            m_ColorLut[3, 2] = ((mask[3] >> 1) & 0x7);
            m_ColorLut[0, 3] = ((mask[4] >> 4) & 0x7);
            m_ColorLut[1, 3] = ((mask[4] >> 7) & 0x1) + ((mask[5] & 0x3) << 1); ;
            m_ColorLut[2, 3] = ((mask[5] >> 2) & 0x7);
            m_ColorLut[3, 3] = ((mask[5] >> 5) & 0x7);
        }
        private void ReadAlfaLut(BinaryReader br)
        {
            m_Alfa0 = br.ReadByte();
            m_Alfa1 = br.ReadByte();

            byte[] lut = br.ReadBytes(6);
            ulong lutbit = 0;

            for (int i = 5; i >= 0; i--)
            {
                lutbit = lutbit * 256 | lut[i];
            }
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    m_AlfaLut[j, i] = (int)(lutbit & 7);
                    lutbit = lutbit >> 3;
                }
            }

            //int layer1 = FifaUtil.SwapEndian(br.ReadInt16());
            //int layer2 = FifaUtil.SwapEndian(br.ReadInt16());
            //int layer3 = FifaUtil.SwapEndian(br.ReadInt16());
            //for (int i = 3; i >= 0; i--)
            //{
            //    for (int j = 3; j >= 0; j--)
            //    {
            //        m_AlfaLut[i, j] = (layer1 & 1) + (layer2 & 1) * 2 + (layer3 & 1) * 4;
            //        layer1 = layer1 >> 1;
            //        layer2 = layer2 >> 1;
            //        layer3 = layer3 >> 1;
            //    }
            //}

        }

        private void WriteAlfaChannel(BinaryWriter bw)
        {
            for (int i = 0; i < 4; i++)
            {
                ushort alfa = 0;

                alfa = (ushort)((m_Alfa[0, i] & 0x00f0) >> 4);
                alfa |= (ushort)((m_Alfa[1, i] & 0x00f0));
                alfa |= (ushort)((m_Alfa[2, i] & 0x00f0) << 4);
                alfa |= (ushort)((m_Alfa[3, i] & 0x00f0) << 8);

                bw.Write(alfa);
            }
        }
        private void WriteColorLut(BinaryWriter bw)
        {
            bw.Write((ushort)m_Col0);
            bw.Write((ushort)m_Col1);

            for (int i = 0; i < 4; i++)
            {
                byte mask = 0;

                mask = (byte)(m_ColorLut[0, i] & 0x3);
                mask |= (byte)((m_ColorLut[1, i] & 0x3) << 2);
                mask |= (byte)((m_ColorLut[2, i] & 0x3) << 4);
                mask |= (byte)((m_ColorLut[3, i] & 0x3) << 6);

                bw.Write(mask);
            }
        }
        private void WriteAlfaLut(BinaryWriter bw)
        {
            bw.Write((byte)m_Alfa0);
            bw.Write((byte)m_Alfa1);
            ulong lutbit = 0;
            for (int i = 3; i >= 0; i--)
            {
                for (int j = 3; j >= 0; j--)
                {
                    lutbit = lutbit << 3;
                    lutbit |= (ulong)((uint)m_AlfaLut[j, i] & 7U);
                }
            }

            byte[] lutBytes = new byte[6];
            for (int i = 0; i < 6; i++)
            {
                lutBytes[i] = (byte)(lutbit & 255);
                // bw.Write((byte)(lutbit & 255));
                lutbit = lutbit >> 8;
            }
            bw.Write(lutBytes);
        }

        private void CleanLuts()
        {
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                {
                    m_AlfaLut[i, j] = 0;
                    m_ColorLut[i, j] = 0;
                }
        }

        // Convert 4x4 pixels into packed version
        private void ConvertFrom4x4()
        {
            Color[] differentRgb = new Color[16];
            int[] differentAlfa = new int[16];
            bool hasTransparency = false;

            int nDifferentRgb = 0;
            int alfaMin = 255;
            int alfaMax = 0;

            bool colorFound;
            // bool alfaFound;

            int nColors = 4; // Number of colors to use

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    uint argb = (uint)m_Colors[i, j].ToArgb();
                    argb = argb & (uint)0xfff8fcf8;
                    m_Colors[i, j] = Color.FromArgb((int)argb);
                }
            }

            // Find all different rgb colors
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    colorFound = false;

                    // If this is a transparent dxt1 color ignore it but use no more then 3 colors
                    if (m_DxtType == EImageType.DXT1)
                    {
                        if (m_Colors[i, j].A == 0)
                        {
                            hasTransparency = true;
                            nColors = 3;
                            continue;
                        }
                    }
                    else
                    {
                        if (m_Colors[i, j].A < alfaMin)
                        {
                            alfaMin = m_Colors[i, j].A;
                        }
                        if (m_Colors[i, j].A > alfaMax)
                        {
                            alfaMax = m_Colors[i, j].A;
                        }
                    }

                    for (int k = 0; k < nDifferentRgb; k++)
                    {
                        if ((m_Colors[i, j].R == differentRgb[k].R) &&
                            (m_Colors[i, j].G == differentRgb[k].G) &&
                            (m_Colors[i, j].B == differentRgb[k].B))
                        {
                            colorFound = true;
                            break;
                        }
                    }
                    if (colorFound == false)
                    {
                        differentRgb[nDifferentRgb] = m_Colors[i, j];
                        nDifferentRgb++;
                    }
                }
            }

            int minErr = 0x00ffffff;
            int err;
            m_TempLut = new int[4, 4];

            // Here we know how many different colors are present in nDifferentRgb
            // and also if the dxt1 contains a transparency ni hasTransparency
            if (m_DxtType == EImageType.DXT1)
            {

                if (hasTransparency)
                {
                    // Here we have transparent pixels
                    if (nDifferentRgb == 0)
                    {
                        // All transparent dxt1
                        for (int p = 0; p < 4; p++)
                            for (int q = 0; q < 4; q++)
                            {
                                m_ColorLut[p, q] = 3;
                            }
                        // Choose conventional colors
                        m_Col0 = 0;
                        m_Col1 = 0;
                        return;
                    }
                    else if (nDifferentRgb == 1)
                    {
                        // Uniform and transparent
                        for (int p = 0; p < 4; p++)
                            for (int q = 0; q < 4; q++)
                            {
                                if (m_Colors[p, q].A == 0)
                                    m_ColorLut[p, q] = 3;
                                else
                                    m_ColorLut[p, q] = 0;
                            }
                        m_Col0 = m_Col1 = (ushort)(((differentRgb[0].R & 0xf8) << 8) | ((differentRgb[0].G & 0xfc) << 3) | ((differentRgb[0].B & 0xf8) >> 3));
                        // m_Col1++;
                        return;
                    }
                    else if (nDifferentRgb == 2)
                    {
                        // Get the two colors and eventually exchange them
                        m_Col0 = (ushort)(((differentRgb[0].R & 0xf8) << 8) | ((differentRgb[0].G & 0xfc) << 3) | ((differentRgb[0].B & 0xf8) >> 3));
                        m_Col1 = (ushort)(((differentRgb[1].R & 0xf8) << 8) | ((differentRgb[1].G & 0xfc) << 3) | ((differentRgb[1].B & 0xf8) >> 3));
                        if (m_Col0 >= m_Col1)
                        {
                            ushort temp = m_Col0;
                            m_Col0 = m_Col1;
                            m_Col1 = temp;
                            Color tempColor = differentRgb[0];
                            differentRgb[0] = differentRgb[1];
                            differentRgb[1] = tempColor;
                        }
                        // Two colors and transparent
                        for (int p = 0; p < 4; p++)
                            for (int q = 0; q < 4; q++)
                            {
                                if (m_Colors[p, q].A == 0)
                                    m_ColorLut[p, q] = 3;
                                else if (m_Colors[p, q] == differentRgb[0])
                                    m_ColorLut[p, q] = 0;
                                else
                                    m_ColorLut[p, q] = 1;
                            }
                        return;

                    }
                }
            }

            CleanLuts();
            if ((alfaMax == 0) && (alfaMin == 0))
            {
                m_Alfa0 = 0;
                m_Col0 = 0;
                m_Alfa1 = 1;
                m_Col1 = 1;
                nDifferentRgb = 0;
            }
            if (nDifferentRgb == 1)
            {
                // Uniform and transparent
                for (int p = 0; p < 4; p++)
                    for (int q = 0; q < 4; q++)
                        m_ColorLut[p, q] = 0;
                m_Col0 = m_Col1 = (ushort)(((differentRgb[0].R & 0xf8) << 8) | ((differentRgb[0].G & 0xfc) << 3) | ((differentRgb[0].B & 0xf8) >> 3));
            }


            for (int i = 0; i < nDifferentRgb; i++)
            {
                for (int j = i + 1; j < nDifferentRgb; j++)
                {
                    ushort col0 = (ushort)(((differentRgb[i].R & 0xf8) << 8) | ((differentRgb[i].G & 0xfc) << 3) | ((differentRgb[i].B & 0xf8) >> 3));
                    ushort col1 = (ushort)(((differentRgb[j].R & 0xf8) << 8) | ((differentRgb[j].G & 0xfc) << 3) | ((differentRgb[j].B & 0xf8) >> 3));

                    ushort cSmall;
                    ushort cBig;
                    int ixSmall, ixBig;

                    if (col0 < col1)
                    {
                        cSmall = col0;
                        cBig = col1;
                        ixSmall = i;
                        ixBig = j;
                    }
                    else
                    {
                        cSmall = col1;
                        cBig = col0;
                        ixSmall = j;
                        ixBig = i;
                    }
                    if (nColors == 4)
                    {
                        err = ScoreColors(differentRgb[ixBig], differentRgb[ixSmall], nColors);
                        if (err < minErr)
                        {
                            minErr = err;
                            for (int p = 0; p < 4; p++)
                                for (int q = 0; q < 4; q++)
                                    m_ColorLut[p, q] = m_TempLut[p, q];
                            m_Col0 = cBig;
                            m_Col1 = cSmall;
                            if (err == 0)
                                break;
                        }
                    }


                    if (m_DxtType == EImageType.DXT1)
                    {
                        err = ScoreColors(differentRgb[ixSmall], differentRgb[ixBig], 3);
                        if (err < minErr)
                        {
                            minErr = err;
                            for (int p = 0; p < 4; p++)
                                for (int q = 0; q < 4; q++)
                                    m_ColorLut[p, q] = m_TempLut[p, q];
                            m_Col0 = cSmall;
                            m_Col1 = cBig;
                            if (err == 0)
                                break;
                        }
                    }
                }
            }

            // Now adjust the alfa channel if dxtType 2 or 3
            if ((m_DxtType == EImageType.DXT3) || (m_DxtType == EImageType.DXT3))
            {
                for (int p = 0; p < 4; p++)
                    for (int q = 0; q < 4; q++)
                        m_Alfa[p, q] = (m_Colors[p, q].A & 0x00f0);
            }

            // Now adjust the alfa lut if dxtType 4 or 5
            if ((m_DxtType == EImageType.DXT5) || (m_DxtType == EImageType.DXT5))
            {
                if ((alfaMin == 0) && (alfaMax == 0))
                {
                    return;
                }
                m_Alfa1 = (byte)alfaMin;
                m_Alfa0 = (byte)alfaMax;
                int delta = (alfaMax - alfaMin);
                int[] alfaValues = new int[8];
                if (delta != 0)
                {
                    for (int i = 2; i < 8; i++)
                    {
                        alfaValues[i] = (m_Alfa0 * (8 - i) + m_Alfa1 * (i - 1)) / 7;
                    }
                }
                int halfStep = (delta / 7) / 2;
                // Find the min and the max alfa values
                for (int p = 0; p < 4; p++)
                    for (int q = 0; q < 4; q++)
                    {
                        int alfa = m_Colors[p, q].A;
                        if (alfa <= (alfaMin + halfStep)) m_AlfaLut[p, q] = 1;
                        else if (alfa >= (alfaMax - halfStep)) m_AlfaLut[p, q] = 0;
                        else if (delta != 0)
                        {
                            m_AlfaLut[p, q] = 0;
                            for (int i = 2; i < 8; i++)
                            {
                                if (alfa > alfaValues[i] - halfStep)
                                {
                                    m_AlfaLut[p, q] = i;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            m_AlfaLut[p, q] = 0;
                        }
                    }
            }

            return;
        }

        private void ConvertFrom3DC(int rgb)
        {

            int alfaMin = 255;
            int alfaMax = 0;

            if (rgb == 0)
            {
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        m_Alfa[i, j] = m_Colors[i, j].R;
                        if (m_Alfa[i, j] < alfaMin) { alfaMin = m_Alfa[i, j]; }
                        if (m_Alfa[i, j] > alfaMax) { alfaMax = m_Alfa[i, j]; }
                    }
                }
            }
            else if (rgb == 1)
            {
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        m_Alfa[i, j] = m_Colors[i, j].G;
                        if (m_Alfa[i, j] < alfaMin) { alfaMin = m_Alfa[i, j]; }
                        if (m_Alfa[i, j] > alfaMax) { alfaMax = m_Alfa[i, j]; }
                    }
                }
            }

            CleanLuts();
            if ((alfaMax == 0) && (alfaMin == 0))
            {
                m_Alfa0 = 0;
                m_Alfa1 = 1;
            }

            // Now adjust the alfa lut 
            if (m_DxtType == EImageType.DC_XY_NORMAL_MAP)
            {
                if ((alfaMin == 0) && (alfaMax == 0))
                {
                    return;
                }
                m_Alfa1 = (byte)alfaMin;
                m_Alfa0 = (byte)alfaMax;
                int delta = (alfaMax - alfaMin);
                int[] alfaValues = new int[8];
                if (delta != 0)
                {
                    for (int i = 2; i < 8; i++)
                    {
                        alfaValues[i] = (m_Alfa0 * (8 - i) + m_Alfa1 * (i - 1)) / 7;
                    }
                }
                int halfStep = (delta / 7) / 2;
                // Find the min and the max alfa values
                for (int p = 0; p < 4; p++)
                    for (int q = 0; q < 4; q++)
                    {
                        int alfa = m_Alfa[p, q];
                        if (alfa <= (alfaMin + halfStep)) m_AlfaLut[p, q] = 1;
                        else if (alfa >= (alfaMax - halfStep)) m_AlfaLut[p, q] = 0;
                        else if (delta != 0)
                        {
                            m_AlfaLut[p, q] = 0;
                            for (int i = 2; i < 8; i++)
                            {
                                if (alfa > alfaValues[i] - halfStep)
                                {
                                    m_AlfaLut[p, q] = i;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            m_AlfaLut[p, q] = 0;
                        }
                    }
            }

            return;
        }

        private void ConvertTo3DC(int rgb)
        {

            int[] comp = new int[8];

            comp[0] = m_Alfa0;
            comp[1] = m_Alfa1;

            if (m_Alfa0 > m_Alfa1)
            {
                for (int c = 2; c < 8; c++)
                {
                    comp[c] = (m_Alfa0 * (8 - c) + m_Alfa1 * (c - 1)) / 7;
                }
            }
            else
            {
                for (int c = 2; c < 6; c++)
                {
                    comp[c] = (m_Alfa0 * (6 - c) + m_Alfa1 * (c - 1)) / 5;
                }
                comp[6] = 0;
                comp[7] = 255;
            }

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    int c = m_AlfaLut[i, j];
                    m_Alfa[i, j] = comp[c];
                }
            }

            if (rgb == 0)
            {
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        m_Colors[i, j] = Color.FromArgb(comp[m_AlfaLut[i, j]], 0, 0);
                    }
                }
            }
            if (rgb == 1)
            {
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        m_Colors[i, j] = Color.FromArgb(m_Colors[i, j].R, comp[m_ColorLut[i, j]], 255);
                    }
                }
            }

        }

        // Convert packed version into 4x4 pixels
        private void ConvertTo4x4()
        {
            int[] alfaValues = new int[8];

            alfaValues[0] = m_Alfa0;
            alfaValues[1] = m_Alfa1;

            if ((m_DxtType == EImageType.DXT5))
            {
                if (m_Alfa0 > m_Alfa1)
                {
                    for (int c = 2; c < 8; c++)
                    {
                        alfaValues[c] = (m_Alfa0 * (8 - c) + m_Alfa1 * (c - 1)) / 7;
                    }
                }
                else
                {
                    for (int c = 2; c < 6; c++)
                    {
                        alfaValues[c] = (m_Alfa0 * (6 - c) + m_Alfa1 * (c - 1)) / 5;
                    }
                    alfaValues[6] = 0;
                    alfaValues[7] = 255;
                }
            }

            // If DXT4 or 5 build the alfa channel from the alfa lut
            if ((m_DxtType == EImageType.DXT5))
            {
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        int c = m_AlfaLut[i, j];
                        m_Alfa[i, j] = alfaValues[c];
                    }
                }
            }

            // If DXT1 build the alfa channel from the color lut
            if (m_DxtType == EImageType.DXT1)
            {
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        int c = m_ColorLut[i, j];
                        if ((m_Col0 <= m_Col1) && (c == 3))
                            m_Alfa[i, j] = 0;
                        else
                            m_Alfa[i, j] = 255;
                    }
                }
            }


            // Build the color using the color lut and the alfa channel
            int[] r = new int[4];
            int[] g = new int[4];
            int[] b = new int[4];
            b[0] = 8 * (m_Col0 & 31);
            g[0] = 4 * ((m_Col0 >> 5) & 63);
            r[0] = 8 * (m_Col0 >> 11);
            b[1] = 8 * (m_Col1 & 31);
            g[1] = 4 * ((m_Col1 >> 5) & 63);
            r[1] = 8 * (m_Col1 >> 11);


            if ((m_Col0 > m_Col1) || (m_DxtType != EImageType.DXT1))
            {
                r[2] = (2 * r[0] + r[1]) / 3;
                g[2] = (2 * g[0] + g[1]) / 3;
                b[2] = (2 * b[0] + b[1]) / 3;
                r[3] = (r[0] + 2 * r[1]) / 3;
                g[3] = (g[0] + 2 * g[1]) / 3;
                b[3] = (b[0] + 2 * b[1]) / 3;
            }
            else
            {
                r[2] = (r[0] + r[1]) / 2;
                g[2] = (g[0] + g[1]) / 2;
                b[2] = (b[0] + b[1]) / 2;
                r[3] = 0;
                g[3] = 0;
                b[3] = 0;
            }

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    int c = m_ColorLut[i, j];
                    m_Colors[i, j] = Color.FromArgb(m_Alfa[i, j], r[c], g[c], b[c]);
                }
            }
        }

        private int[] ComputeInterpolatedAlfa(int alfa0, int alfa1)
        {
            int[] alfaValues = new int[8];
            alfaValues[0] = alfa0;
            alfaValues[1] = alfa1;
            for (int i = 2; i < 8; i++)
            {
                int c = 8 - i;
                int d = i - 1;
                alfaValues[i] = (c * alfa0 + d * alfa1) / 7;
            }
            return (alfaValues);
        }

        private int[,] ComputeInterpolatedRgb(Color col0, Color col1, int nColors)
        {
            int[,] rgbValues = new int[4, 3];
            rgbValues[0, 0] = col0.R;
            rgbValues[0, 1] = col0.G;
            rgbValues[0, 2] = col0.B;

            rgbValues[1, 0] = col1.R;
            rgbValues[1, 1] = col1.G;
            rgbValues[1, 2] = col1.B;

            if (nColors <= 3)
            {
                nColors = 3;
                for (int i = 2; i < nColors; i++)
                {
                    int c = nColors - i;
                    int d = i - 1;
                    rgbValues[i, 0] = (c * col0.R + d * col1.R) / 2;
                    rgbValues[i, 1] = (c * col0.G + d * col1.G) / 2;
                    rgbValues[i, 2] = (c * col0.B + d * col1.B) / 2;
                }
                rgbValues[3, 0] = 0;
                rgbValues[3, 1] = 0;
                rgbValues[3, 2] = 0;

            }
            else
            {
                nColors = 4;
                for (int i = 2; i < nColors; i++)
                {
                    int c = nColors - i;
                    int d = i - 1;
                    rgbValues[i, 0] = (c * col0.R + d * col1.R) / 3;
                    rgbValues[i, 1] = (c * col0.G + d * col1.G) / 3;
                    rgbValues[i, 2] = (c * col0.B + d * col1.B) / 3;
                }

            }

            return (rgbValues);
        }

        private int ScoreColors(Color col0, Color col1, int nColors)
        {
            int[,] rgb = ComputeInterpolatedRgb(col0, col1, nColors);
            int minErr;
            int totalErr = 0;

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    minErr = 0x00ffffff;
                    int r = m_Colors[i, j].R;
                    int g = m_Colors[i, j].G;
                    int b = m_Colors[i, j].B;
                    int a = m_Colors[i, j].A;
                    if ((a == 0) && (nColors == 3))
                    {
                        m_TempLut[i, j] = 3;
                        continue; // Ignore transparent pixel in dxt1
                    }
                    for (int k = 0; k < nColors; k++)
                    {
                        int eR = rgb[k, 0] - r;
                        int eG = rgb[k, 1] - g;
                        int eB = rgb[k, 2] - b;
                        int err = eR * eR + eG * eG + eB * eB;
                        if (err < minErr)
                        {
                            minErr = err;
                            m_TempLut[i, j] = k;
                            if (err == 0)
                                break;

                        }
                    }
                    totalErr += minErr;
                }
            }
            return (totalErr);
        }

    }
}
