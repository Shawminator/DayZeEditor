using DarkUI.Forms;
using DayZeLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Windows.Forms;
using BIS.PAA;
using System.Text.RegularExpressions;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Web.UI.Design.WebControls;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DayZeEditor
{
    public partial class ImageConvertor : DarkForm
    {
        public int originalwidth;
        public int originalheight;
        public string filepathname;
        private Bitmap bmp;
        public DDS dds { get; set; }
        public PAA paa { get; set; }

        public ImageConvertor()
        {
            InitializeComponent();
            glControl1.Enabled = false;
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            dds = null;
            paa = null;
            checkBox1.Checked = false;
            OpenFileDialog openfile = new OpenFileDialog();
            if (openfile.ShowDialog() == DialogResult.OK)
            {
                Cursor.Current = Cursors.WaitCursor;
                filepathname = openfile.FileName;
                if (Path.GetExtension(filepathname) == ".edds" || Path.GetExtension(filepathname) == ".dds")
                {
                    using (BinaryReader binaryReader = new BinaryReader(File.Open(filepathname, FileMode.Open, FileAccess.ReadWrite)))
                    {
                        dds = new DDS(binaryReader, Path.GetExtension(filepathname) == ".edds" ? true : false);
                        originalwidth = dds.Header.Width;
                        originalheight = dds.Header.Height;
                        trackBar2.Value = 100;
                        glControl1.Visible = true;
                        rentex = new RenderTexture(glControl1, null, false);
                        rentex.Setcale(100);
                        rentex.texture = rentex.LoadTexture(dds);
                        rentex.loadvertices();
                        rentex.CreateTexture();
                        label8.Text = "Texture Type: " + Path.GetExtension(filepathname);
                        label9.Text = "Pixel Format: " + getpixelformat(dds.Header.PixelFormat.FourCc);
                        label10.Text = "Width: " + dds.Header.Width.ToString();
                        label11.Text = "Hieght: " + dds.Header.Height.ToString();
                        label12.Text = "Depth: " + dds.Header.Depth.ToString();
                        label13.Text = "Num of MipMaps: " + dds.Header.MipMapCount.ToString();
                    }
                }
                else if(Path.GetExtension(filepathname) == ".paa")
                {
                    
                    using (FileStream fs = File.Open(filepathname, FileMode.Open))
                    {
                        paa = new PAA(fs, false);
                        originalwidth = paa.Width;
                        originalheight = paa.Height;
                        trackBar2.Value = 100;
                        byte[] argB32PixelData = PAA.GetARGB32PixelData(paa, fs);
                        rentex = new RenderTexture(glControl1, null, false);
                        rentex.Setcale(100);
                        rentex.texture = rentex.LoadTexture(paa, argB32PixelData);
                        rentex.loadvertices();
                        rentex.CreateTexture();
                        label8.Text = "Texture Type: " + Path.GetExtension(filepathname);
                        label9.Text = "Pixel Format: " + paa.Type.ToString();
                        label10.Text = "Width: " + paa.Width.ToString();
                        label11.Text = "Hieght: " + paa.Height.ToString();
                        label12.Text = "Depth: 0";
                        label13.Text = "Num of MipMaps: " + paa.mipmaps.Count.ToString();
                    }
                    
                }
            }
            Cursor.Current = Cursors.Default;
        }
        public static string getpixelformat(int a)
        {
            switch (a)
            {
                case 0:
                    return "A8R8G8B8";
                case 1:
                    return "Luminance";
                case 2:
                case 0x31545844:
                    return "DXT1";
                case 0x33545844:
                    return "DXT3";
                case 4:
                case 0x35545844:
                    return "DXT5";
                case 9:
                    return "BC5_UNORM";
                case 11:
                case 0x30315844:
                    return "BC7 UNORM";
                case 12:
                    return "A16R16G16B16_Float";
                default:
                    return "";
            }
        }
        private void SaveFileButton_Click(object sender, EventArgs e)
        {
            if(dds != null)
            {
                SaveFileDialog save = new SaveFileDialog
                {
                    Filter = "DDS Files (*.dds)| *.dds",
                    DefaultExt = "dds",
                    FileName = Path.GetFileNameWithoutExtension(filepathname)
                };
                if (save.ShowDialog() == DialogResult.OK)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    dds.write(filepathname + ".dds");
                }
                
            }
            else if (paa != null)
            {
                SaveFileDialog save = new SaveFileDialog
                {
                    Filter = "DDS Files (*.dds)| *.dds",
                    DefaultExt = "dds",
                    FileName = Path.GetFileNameWithoutExtension(filepathname)
                };
                if (save.ShowDialog() == DialogResult.OK)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    using (FileStream fs = File.Open(filepathname, FileMode.Open))
                    {
                        paa = new PAA(fs, false);
                        DDS paadds = new DDS(paa, fs);
                        paadds.write(filepathname + ".DDS");
                    }
                }
            }
            Cursor.Current = Cursors.Default;
        }


        public RenderTexture rentex;
        private Point _mouseLastPosition;
        private Point _newscrollPosition;

        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            if (glControl1.Enabled && rentex != null)
            {
                rentex.Paint(); 
                panel2.AutoScrollPosition = _newscrollPosition;
            }
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (rentex != null)
            {
                rentex.Alphaswitch_checkchanged(checkBox1.Checked);
            }
        }
        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            rentex.Setcale(trackBar2.Value);
            glControl1.Width = (int)((decimal)originalwidth / 100 * trackBar2.Value);
            glControl1.Height = (int)((decimal)originalheight / 100 * trackBar2.Value);
            rentex.loadvertices();
            rentex.CreateTexture();
        }
        private void glControl1_MouseHover(object sender, EventArgs e)
        {
            
        }
        private void glControl1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point changePoint = new Point(e.Location.X - _mouseLastPosition.X, e.Location.Y - _mouseLastPosition.Y);
                _newscrollPosition = new Point(-panel2.AutoScrollPosition.X - changePoint.X, -panel2.AutoScrollPosition.Y - changePoint.Y);
                if (_newscrollPosition.X <= 0)
                    _newscrollPosition.X = 0;
                if (_newscrollPosition.Y <= 0)
                    _newscrollPosition.Y = 0;
                panel2.AutoScrollPosition = _newscrollPosition;
                //glControl1.Invalidate();
            }
            //else
            //{
            //    Control control = sender as Control;
            //    Point pt = control.PointToClient(Control.MousePosition);
            //    float y = ((glControl1.Height - pt.Y) / rentex.Scale) * 100;// - (glControl1.Height - rentex.pic_Height) ;// / rentex.Scale); * 100 ;
            //    float X = (pt.X / rentex.Scale) * 100;
            //    label2.Text = "Mouse clicked at texture coords: (" + X + ", " + y + ")";
            //}
        }
        private void glControl1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Control control = sender as Control;
            Point pt = control.PointToClient(Control.MousePosition);
            float y = ((glControl1.Height - pt.Y) / rentex.Scale) * 100;// - (glControl1.Height - rentex.pic_Height) ;// / rentex.Scale); * 100 ;
            float X = (pt.X / rentex.Scale) * 100;
            MessageBox.Show("Mouse clicked at texture coords: (" + X + ", " + y + ")");
        }
        private void glControl1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Cursor.Current = Cursors.SizeAll;
                _mouseLastPosition = e.Location;
            }
        }
        private void glControl1_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta < 0)
            {
                pictureBox4_ZoomOut();
            }
            else
            {
                pictureBox4_ZoomIn();
            }
            rentex.Setcale(trackBar2.Value);
            glControl1.Width = (int)((decimal)originalwidth / 100 * trackBar2.Value);
            glControl1.Height = (int)((decimal)originalheight / 100 * trackBar2.Value);
            rentex.loadvertices();
            rentex.CreateTexture();
        }
        private void pictureBox4_ZoomIn()
        {
            int oldpictureboxhieght = glControl1.Height;
            int oldpitureboxwidht = glControl1.Width;
            Point oldscrollpos = panel2.AutoScrollPosition;
            int tbv = trackBar2.Value;
            int newval = tbv + 10;
            if (newval >= 400)
                newval = 400;
            trackBar2.Value = newval;
            if (glControl1.Height > panel2.Height)
            {
                decimal newy = ((decimal)oldscrollpos.Y / (decimal)oldpictureboxhieght);
                int y = (int)(glControl1.Height * newy);
                _newscrollPosition.Y = y * -1;
                panel2.AutoScrollPosition = _newscrollPosition;
            }
            if (glControl1.Width > panel2.Width)
            {
                decimal newy = ((decimal)oldscrollpos.X / (decimal)oldpitureboxwidht);
                int x = (int)(glControl1.Width * newy);
                _newscrollPosition.X = x * -1;
                panel2.AutoScrollPosition = _newscrollPosition;
            }
            glControl1.Invalidate();
        }
        private void pictureBox4_ZoomOut()
        {
            int oldpictureboxhieght = glControl1.Height;
            int oldpitureboxwidht = glControl1.Width;
            Point oldscrollpos = panel2.AutoScrollPosition;
            int tbv = trackBar2.Value;
            int newval = tbv - 10;
            if (newval <= 0)
                newval = 0;
            trackBar2.Value = newval;
            if (glControl1.Height > panel2.Height)
            {
                decimal newy = ((decimal)oldscrollpos.Y / (decimal)oldpictureboxhieght);
                int y = (int)(glControl1.Height * newy);
                _newscrollPosition.Y = y * -1;
                panel2.AutoScrollPosition = _newscrollPosition;
            }
            if (glControl1.Width > panel2.Width)
            {
                decimal newy = ((decimal)oldscrollpos.X / (decimal)oldpitureboxwidht);
                int x = (int)(glControl1.Width * newy);
                _newscrollPosition.X = x * -1;
                panel2.AutoScrollPosition = _newscrollPosition;
            }
            glControl1.Invalidate();
        }
    }
    public struct Texture2d
    {
        private int id;
        private Vector2 size;
        public int ID { get { return id; } }
        public Vector2 Size { get { return size; } }
        public int width { get { return (int)size.X; } }
        public int hieght { get { return (int)size.Y; } }

        public Texture2d(int id, Vector2 size)
        {
            this.id = id;
            this.size = size;
        }
    }
    public struct Texture3d
    {
        private int id;
        private Vector3 size;
        public int ID { get { return id; } }
        public Vector3 Size { get { return size; } }
        public int width { get { return (int)size.X; } }
        public int hieght { get { return (int)size.Y; } }
        public int depth { get { return (int)size.Z; } }

        public Texture3d(int id, Vector3 size)
        {
            this.id = id;
            this.size = size;
        }
    }
    struct Vertex
    {
        public Vector2 position;
        public Vector2 texCoord;
        public Vector4 color;
        public Color Color
        {
            get
            {
                return Color.FromArgb(
                    (int)(255 * color.W),
                    (int)(255 * color.X),
                    (int)(255 * color.Y),
                    (int)(255 * color.Z)
                    );
            }
            set
            {
                this.color = new Vector4(
                    value.R / 255f,
                    value.G / 255f,
                    value.B / 255f,
                    value.A / 255f
                    );
            }
        }
        public static int SizeInBytes { get { return Vector2.SizeInBytes * 2 + Vector4.SizeInBytes; } }
        public Vertex(Vector2 position, Vector2 texCoord)
        {
            this.position = position;
            this.texCoord = texCoord;
            this.color = new Vector4(1, 1, 1, 1);
        }
    }
    public class RenderTexture
    {
        public Texture2d texture;
        public Texture3d _3dtexture;
        private bool _customsize;
        private int _picheight;
        private int _picwidth;
        public int pic_Height;
        public int pic_Width;
        private Vertex[] texVertbuffer;
        private int texVBO;
        private GLControl _thisglcont;
        private TabPage _tab;
        public byte[] filearray;
        bool alpha = false;

        public float Scale { get; private set; }

        public RenderTexture(GLControl glcont, TabPage tab, bool customsize, int picheight = 0, int picwidth = 0)
        {
            filearray = null;
            _thisglcont = glcont;

            _thisglcont.Enabled = true;
            _thisglcont.Visible = true;
            _thisglcont.MakeCurrent();
            _thisglcont.Visible = false;
            _tab = tab;
            _customsize = customsize;
            _picheight = picheight;
            _picwidth = picwidth;
            _thisglcont.Enabled = false;
        }

        private void set_thisglcontrolsize(PAA paa)
        {
            if (_customsize)
            {
                this.pic_Height = _picheight;
                this.pic_Width = _picwidth;
            }
            else
            {
                this.pic_Height = (int)(paa.Height);
                this.pic_Width = (int)paa.Width;
            }
            _thisglcont.Size = new System.Drawing.Size(this.pic_Width, this.pic_Height);
        }
        private void set_thisglcontrolsize(DDS dds)
        {
            if (_customsize)
            {
                this.pic_Height = _picheight;
                this.pic_Width = _picwidth;
            }
            else
            {
                this.pic_Height = dds.Header.Height;
                this.pic_Width = dds.Header.Width;
            }
            _thisglcont.Size = new System.Drawing.Size(this.pic_Width, this.pic_Height);
        }
        public void Setcale(float _Scale)
        {
            Scale = _Scale;
        }
        public void loadvertices()
        {
            this.texVertbuffer = new Vertex[4]
            {
                new Vertex(new Vector2(0,0), new Vector2(0,0)),
                new Vertex(new Vector2(0,_thisglcont.Height), new Vector2(0,1)),
                new Vertex(new Vector2(_thisglcont.Width,_thisglcont.Height), new Vector2(1,1)),
                new Vertex(new Vector2(_thisglcont.Width,0), new Vector2(1,0))
            };
        }
        public void CreateTexture()
        {
            _thisglcont.Visible = true;
            //_thisglcont.MakeCurrent();

            //GL.Enable(EnableCap.Blend);
            //GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

            GL.Disable(EnableCap.Blend);

            GL.ClearColor(Color.FromArgb(60, 63, 65));
            GL.Enable(EnableCap.Texture2D);


            this.texVBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, this.texVBO);
            GL.BufferData<Vertex>(
                BufferTarget.ArrayBuffer,
                (IntPtr)(Vertex.SizeInBytes * this.texVertbuffer.Length),
                this.texVertbuffer,
                BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            this.SetupViewport();
            GL.Disable(EnableCap.ColorLogicOp);


            _thisglcont.Enabled = true;
            _thisglcont.Invalidate();
            _thisglcont.Update();
        }
        public void Alphaswitch_checkchanged(bool Alphaswitch)
        {
            alpha = Alphaswitch;
            if (Alphaswitch)
            {
                GL.Enable(EnableCap.Blend);
                GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
                this._thisglcont.Invalidate();
                this._thisglcont.Update();
            }
            else
            {
                GL.Disable(EnableCap.Blend);
                this._thisglcont.Invalidate();
                this._thisglcont.Update();
            }
        }
        private void deletetempfiles()
        {
            string[] files = System.IO.Directory.GetFiles(Application.StartupPath + "\\Work\\", "*.png");
            foreach (string file in files)
            {
                System.IO.File.Delete(file);
            }
            files = System.IO.Directory.GetFiles(Application.StartupPath + "\\Work\\", "*.dds");
            foreach (string file in files)
            {
                System.IO.File.Delete(file);
            }
        }
        private void converttex()
        {
            string name = Application.StartupPath + "\\Work\\Temp.dds";
            string newname = Application.StartupPath + "\\Work\\Temp.png";
            string exe = Application.StartupPath + "\\Work\\texconv.exe";
            string line = @"""" + name + @""" -ft PNG";
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo = new System.Diagnostics.ProcessStartInfo
            {
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                WorkingDirectory = Application.StartupPath + "\\Work",
                FileName = exe,
                Arguments = line
            };
            process.Start();
            process.WaitForExit();
            filearray = File.ReadAllBytes(newname);
            File.Delete(name);
            File.Delete(newname);
        }
        public Texture2d LoadTexture(PAA paa, byte[] mipmap, bool pixalatted = false)
        {
            int texture = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, texture);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, 9729);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, 9729);

            set_thisglcontrolsize(paa);
            switch (paa.Type)
            {
                case PAAType.DXT1:
                    GL.CompressedTexImage2D<byte>(
                                TextureTarget.Texture2D,
                                0,
                                PixelInternalFormat.CompressedRgbaS3tcDxt1Ext,
                                paa.Width,
                                paa.Height,
                                0,
                                mipmap.Length,
                                mipmap);
                    break;
                case PAAType.DXT5:
                    GL.CompressedTexImage2D<byte>(
                                TextureTarget.Texture2D,
                                0,
                                PixelInternalFormat.CompressedRgbaS3tcDxt5Ext,
                                paa.Width,
                                paa.Height,
                                0,
                                mipmap.Length,
                                mipmap);
                    break;
            }
            return new Texture2d(texture, new Vector2(paa.Width, paa.Height));
        }
        public Texture2d LoadTexture(DDS dds, bool pixalatted = false)
        {
            int texture = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, texture);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, 9729);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, 9729);

            set_thisglcontrolsize(dds);
            switch (dds.Header.PixelFormat.Flags)
            {
                case DdsPixelFormatFlag.FourCc:
                    switch (dds.Header.PixelFormat.FourCc)
                    {
                        case 0x31545844:
                            GL.CompressedTexImage2D<byte>(
                                TextureTarget.Texture2D,
                                0,
                                PixelInternalFormat.CompressedRgbaS3tcDxt1Ext,
                                dds.Header.Width,
                                dds.Header.Height,
                                0,
                                dds.bitmaps[0].Length,
                                dds.bitmaps[0]);
                            break;
                        case 0x32545844:
                            break;
                        case 0x33545844:
                            GL.CompressedTexImage2D<byte>(
                                TextureTarget.Texture2D,
                                0,
                                PixelInternalFormat.CompressedRgbaS3tcDxt3Ext,
                                dds.Header.Width,
                                dds.Header.Height,
                                0,
                                dds.bitmaps[0].Length,
                                dds.bitmaps[0]);
                            break;
                        case 0x34545844:
                            break;
                        case 0x35545844:
                            GL.CompressedTexImage2D<byte>(
                                TextureTarget.Texture2D,
                                0,
                                PixelInternalFormat.CompressedRgbaS3tcDxt5Ext,
                                dds.Header.Width,
                                dds.Header.Height,
                                0,
                                dds.bitmaps[0].Length,
                                dds.bitmaps[0]);
                            break;
                        case 0x30315844:
                            GL.CompressedTexImage2D<byte>(
                                TextureTarget.Texture2D,
                                0,
                                PixelInternalFormat.CompressedRgbaBptcUnorm,
                                dds.Header.Width,
                                dds.Header.Height,
                                0,
                                dds.bitmaps[0].Length,
                                dds.bitmaps[0]);
                            break;
                    }
                    break;
                case DdsPixelFormatFlag.Alpha:
                    MessageBox.Show(DdsPixelFormatFlag.Alpha.ToString() + " Not implemented yet!.....");
                    break;
                case DdsPixelFormatFlag.Luminance:
                    MessageBox.Show(DdsPixelFormatFlag.Luminance.ToString() + " Not implemented yet!.....");
                    break;
                case DdsPixelFormatFlag.Normal:
                    MessageBox.Show(DdsPixelFormatFlag.Normal.ToString() + " Not implemented yet!.....");
                    break;
                case DdsPixelFormatFlag.Rgb:
                    GL.TexImage2D<byte>(
                            TextureTarget.Texture2D,
                            0,
                            PixelInternalFormat.Rgba8,
                            dds.Header.Width,
                            dds.Header.Height,
                            0,
                            OpenTK.Graphics.OpenGL.PixelFormat.Bgra,
                            PixelType.UnsignedByte,
                            dds.Data);
                    break;
                case DdsPixelFormatFlag.Rgba:
                    GL.TexImage2D<byte>(
                            TextureTarget.Texture2D,
                            0,
                            PixelInternalFormat.Rgba8,
                            dds.Header.Width,
                            dds.Header.Height,
                            0,
                            OpenTK.Graphics.OpenGL.PixelFormat.Bgra,
                            PixelType.UnsignedByte,
                            dds.Data);
                    break;
            }
            return new Texture2d(texture, new Vector2(dds.Header.Width, dds.Header.Height));
        }
        

        private void SetupViewport()
        {
            int width = _thisglcont.Width;
            int height = _thisglcont.Height;
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Viewport(0, 0, width, height);
            GL.Ortho(0, (double)width, (double)height, 0, -1, 1);
            GL.Disable(EnableCap.DepthTest);
        }
        public void Paint()
        {
            if (this._thisglcont.Enabled)
            {
                //GL.ClearColor();
                GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit | ClearBufferMask.ColorBufferBit);
                GL.Enable(EnableCap.Texture2D);
                GL.BindTexture(TextureTarget.Texture2D, texture.ID);
                if(alpha)
                {
                    GL.Enable(EnableCap.Blend);
                    GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
                }
                GL.EnableClientState(ArrayCap.VertexArray);
                GL.EnableClientState(ArrayCap.TextureCoordArray);
                GL.EnableClientState(ArrayCap.ColorArray);

                GL.BindBuffer(BufferTarget.ArrayBuffer, this.texVBO);
                GL.VertexPointer(2, VertexPointerType.Float, Vertex.SizeInBytes, (IntPtr)0);
                GL.TexCoordPointer(2, TexCoordPointerType.Float, Vertex.SizeInBytes, (IntPtr)Vector2.SizeInBytes);
                GL.ColorPointer(4, ColorPointerType.Float, Vertex.SizeInBytes, (IntPtr)(Vector2.SizeInBytes * 2));
                GL.DrawArrays(PrimitiveType.Quads, 0, this.texVertbuffer.Length);
                GL.End();
                GL.Flush();
                this._thisglcont.SwapBuffers();

            }
        }
        public void clean()
        {
            GL.Flush();
        }


    }
}
