using System.IO;
using System;

namespace Irelia.Render
{
    public class Font
    {
        private readonly AnimateSprite sprite;
        private readonly int[] widths = new int[256];

        public string Name { get; set; }
        public Texture Texture { get; private set; }
        public Stream WidthStream { get; private set; }
        public int CharWidth { get { return this.sprite.FrameWidth; } }
        public int CharHeight { get { return this.sprite.FrameHeight; } }

        #region Constructors
        public Font(string name, Texture texture, string dataFile)
            : this(name, texture)
        {
            using (var stream = File.OpenRead(dataFile))
            {
                ReadWidthData(stream);
            }
        }

        public Font(string name, Texture texture, Stream widthStream)
            : this(name, texture)
        {
            ReadWidthData(widthStream);
        }
        #endregion

        private Font(string name, Texture texture)
        {
            Name = name;
            Texture = texture;

            int numCharPerLine = 16;
            int charWidth = texture.Width / numCharPerLine;
            int charHeight = texture.Height / numCharPerLine;

            this.sprite = new AnimateSprite(texture, new Range<int>(0, 0), 0, numCharPerLine, charWidth, charHeight);
        }

        public bool Print(string text, Vector2 position, Color color, SpriteRenderer renderer, float scale = 1.0f)
        {
            var oldColor = sprite.Color;
            var oldScale = sprite.Scale;
            sprite.Color = color;
            sprite.Scale = new Vector2(scale, scale);

            // Draw
            Vector2 pos = position;
            foreach (char ch in text.ToCharArray())
            {
                int frame = (int)ch;

                sprite.CurrentFrame = frame;
                sprite.Position = pos;

                if (ch == '\n')
                {
                    pos.x = 0.0f;
                    pos.y += this.CharHeight;
                }
                else
                {
                    if ((this.sprite as ISprite).Render(renderer) == false)
                        return false;

                    pos.x += this.widths[frame] * this.sprite.Scale.x;
                }
            }

            sprite.Color = oldColor;
            sprite.Scale = oldScale;

            return true;
        }

        private void ReadWidthData(Stream stream)
        {
            WidthStream = new MemoryStream((int)(stream.Length - stream.Position));
            stream.CopyTo(WidthStream);
            WidthStream.Position = 0;

            var reader = new BinaryReader(WidthStream);
            var bytes = reader.ReadBytes(512);
            for (int i = 0; i < this.widths.Length; ++i)
            {
                this.widths[i] = bytes[i * 2];

                if (this.widths[i] == 0)
                    this.widths[i] = CharWidth;
            }
        }

        #region Save & Load
        public void Save(string fileName)
        {
            using (var zip = new ZipArchive(fileName))
            {
                zip.Save(Path.ChangeExtension(fileName, ".tga"), Texture.ToStream());
                zip.Save(Path.ChangeExtension(fileName, ".dat"), WidthStream);
            }
        }

        public static Font Load(Device device, string fileName, string name, AssetManager assetManager)
        {
            using (var zip = new ZipArchive(fileName))
            {
                string textureFile = Path.ChangeExtension(Path.GetFileName(fileName), ".tga");
                Texture texture = new Texture(device, zip.Load(textureFile), name);

                string widthDataFile = Path.ChangeExtension(Path.GetFileName(fileName), ".dat");
                return new Font(name, texture, zip.Load(widthDataFile));
            }
        }
        #endregion
    }

    public sealed class FontFactory : IAssetFactory
    {
        public string[] FileExtensions
        {
            get { return new string[] { ".font" }; }
        }

        public Type AssetType { get { return typeof(Font); } }

        public object Load(Device device, string filePath, AssetLoadArguments args, string name, AssetManager assetManager)
        {
            return Font.Load(device, filePath, name, assetManager);
        }
    }
}
