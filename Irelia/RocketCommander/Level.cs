using System;
using System.IO;
using Irelia.Render;
using System.Collections.Generic;
using System.Linq;

namespace RocketCommander
{
    public sealed class Level
    {
        public enum ItemType
        {
            Fuel,
            Health,
            ExtraLife,
            Speed,
            Bomb
        }

        #region Properties
        public static float SectorWidth
        {
            get { return 200.0f; }
        }

        public static float SectorHeight
        {
            get { return 200.0f; }
        }

        public static float SectorDepth
        {
            get { return 200.0f; }
        }

        public string Name { get; private set; }
        public int Width { get; private set; }
        public int Length { get; private set; }
        public IDictionary<ItemType, List<Vector3>> Items { get; private set; }
        public Color[] SunColors { get; private set; }
        #endregion

        #region Fields
        private static readonly int defaultLevelWidth = 20;
        private static readonly Color FuelItemColor = new Color(255, 255, 255, 0),
                                      HealthItemColor = new Color(255, 0, 255, 0),
                                      ExtraLifecolor = new Color(255, 255, 0, 255),
                                      SpeedItemColor = new Color(255, 0, 0, 255),
                                      BombItemColor = new Color(255, 255, 0, 0);

        private readonly float[,] densities;
        #endregion

        #region Constructors
        public Level(string missionFile)
        {
            Name = Path.GetFileNameWithoutExtension(missionFile);

            int height;
            Color[,] levelColors;
            using (var reader = new BinaryReader(File.Open(missionFile, FileMode.Open)))
            {
                int width = reader.ReadInt32();
                height = reader.ReadInt32();
                
                levelColors = new Color[width, height];
                for (int x = 0; x < width; ++x)
                {
                    for (int y = 0; y < height; ++y)
                    {
                        byte r = reader.ReadByte();
                        byte g = reader.ReadByte();
                        byte b = reader.ReadByte();
                        levelColors[x, y] = new Color((int)255, (int)r, (int)g, (int)b);
                    }
                }
            }

            Width = defaultLevelWidth;
            Length = height;

            // Load everything in as 0-1 density values
            this.densities = new float[Width, Length];
            Items = new Dictionary<ItemType, List<Vector3>>();
            foreach (ItemType itemType in Enum.GetValues(typeof(ItemType)))
            {
                Items.Add(itemType, new List<Vector3>());
            }
            SunColors = new Color[Length];

            float lastDensity = 0.0f;
            for (int y = 0; y < Length; ++y)
            {
                int yPos = Length - (y + 1);
                for (int x = 0; x < Width; ++x)
                {
                    var color = levelColors[x * 2, y];
                    var color2 = levelColors[x * 2 + 1, y];

                    float density = lastDensity;

                    if (color == FuelItemColor || color2 == FuelItemColor)
                        Items[ItemType.Fuel].Add(GenerateItemPosition(x, yPos));
                    else if (color == HealthItemColor || color2 == HealthItemColor)
                        Items[ItemType.Health].Add(GenerateItemPosition(x, yPos));
                    else if (color == ExtraLifecolor || color2 == ExtraLifecolor)
                        Items[ItemType.ExtraLife].Add(GenerateItemPosition(x, yPos));
                    else if (color == SpeedItemColor || color2 == SpeedItemColor)
                        Items[ItemType.Speed].Add(GenerateItemPosition(x, yPos));
                    else if (color == BombItemColor || color2 == BombItemColor)
                        Items[ItemType.Bomb].Add(GenerateItemPosition(x, yPos));
                    else
                        density = color.r;

                    this.densities[x, yPos] = density;
                }

                SunColors[yPos] = levelColors[50, y];
            }
        }
        #endregion

        #region Public Methods
        public float GetDensity(int x, int z)
        {
            return this.densities[x, z];
        }
        #endregion

        #region Private Methods
        private Vector3 GenerateItemPosition(int xPos, int zPos)
        {
            return new Vector3()
            {
                x = (xPos - Width / 2) * SectorWidth,
                y = RandomHelper.GetRandomFloat(-SectorHeight * 1.8f, +SectorHeight * 1.9f),
                z = SectorDepth * zPos + RandomHelper.GetRandomFloat(-SectorWidth / 2, +SectorWidth / 2)
            };
        }
        #endregion
    }
}
