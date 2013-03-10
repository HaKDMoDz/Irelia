using System.Collections.Generic;
using Irelia.Render;
using System.Diagnostics;

namespace RocketCommander
{
    public sealed class AsteroidManager
    {
        #region Fields
        private static readonly int numberOfSectors = 23;
        private static readonly int middleSector = numberOfSectors / 2;
        private static readonly int minSector = -middleSector;
        private static readonly int maxSector = +middleSector;

        private static readonly int numberOfSmallSectors = 7;
        private static readonly int smallMiddleSector = numberOfSmallSectors / 2;
        private static readonly int smallSectorAdd = middleSector - smallMiddleSector;
        private static readonly int minSmallSector = -smallMiddleSector;
        private static readonly int maxSmallSector = +smallMiddleSector;

        private List<Asteroid>[,] asteroids = new List<Asteroid>[numberOfSectors, numberOfSectors];
        private List<SmallAsteroid>[,] smallAsteroids = new List<SmallAsteroid>[numberOfSmallSectors, numberOfSmallSectors];
        private readonly bool[,] sectorVisibleInRange = new bool[numberOfSectors, numberOfSectors];
        private readonly bool[,] sectorIsVisible = new bool[numberOfSectors, numberOfSectors];
        private readonly Vector3[,] sectorDirection = new Vector3[numberOfSectors, numberOfSectors];
        private readonly Level level;
        private readonly RocketCommanderGame game;
        private int lastCameraSectorPosX;
        private int lastCameraSectorPosZ;
        #endregion

        #region Properties
        private Radian ViewableFieldOfView
        {
            get { return this.game.FieldOfView; }
        }
        #endregion

        public AsteroidManager(Level level, RocketCommanderGame game)
        {
            this.level = level;
            this.game = game;

            // Create all asteroids
            for (int z = minSector; z <= maxSector; ++z)
            {
                for (int x = minSector; x <= maxSector; ++x)
                {
                    int iz = z + middleSector;
                    int ix = x + middleSector;
                    this.asteroids[iz, ix] = new List<Asteroid>();
                    GenerateAsteroids(this.asteroids[iz, ix], x, z);
                }
            }

            // Create smaller asteroids
            for (int z = minSmallSector; z <= maxSmallSector; ++z)
            {
                for (int x = minSmallSector; x <= maxSmallSector; ++x)
                {
                    int iz = z + smallMiddleSector;
                    int ix = x + smallMiddleSector;
                    this.smallAsteroids[iz, ix] = new List<SmallAsteroid>();
                    GenerateSmallAsteroids(this.smallAsteroids[iz, ix], this.asteroids[iz + smallSectorAdd, ix + smallSectorAdd].Count, x, z);
                }
            }

            // Precalculate visible sector stuff
            for (int z = minSector; z <= maxSector; ++z)
            {
                for (int x = minSector; x <= maxSector; ++x)
                {
                    int iz = z + middleSector;
                    int ix = x + middleSector;

                    this.sectorVisibleInRange[iz, ix] = MathUtils.Sqrt(x * x + z * z) < middleSector + 0.25f;
                    this.sectorDirection[iz, ix] = new Vector3(x, 0, z).Normalize();
                }
            }

            UpdateSectors();
        }

        public void Update()
        {
            #region Initialize and calculate sectors
            // Get current sector we are in
            int cameraSectorPosX = (int)MathUtils.Round(this.game.Camera.EyePos.x / Level.SectorWidth);
            int cameraSectorPosZ = (int)MathUtils.Round(this.game.Camera.EyePos.z / Level.SectorDepth);

            UpdateSectors();
            #endregion
        }

        public string GetVisibilityText()
        {
            // Only for debugging.
            string str = "";
            for (int z = 0; z < numberOfSectors; ++z)
            {
                for (int x = 0; x < numberOfSectors; ++x)
                {
                    str += this.sectorIsVisible[z, x] ? "*" : "-";
                }
                str += "\n";
            }
            return str;
        }

        private void GenerateAsteroids(List<Asteroid> asteroids, int x, int z)
        {
            if (MathUtils.Abs(x) < 2 && MathUtils.Abs(z) < 2)
                return;

            float density = 0.1f;
            int levelX = x + level.Width / 2;
            int levelZ = z;
            if (levelX >= 0 && levelX < level.Width &&
                levelZ >= 0 && levelZ < level.Length)
            {
                density += this.level.GetDensity(levelX, levelZ);
            }

            int numOfAsteroids = RandomHelper.GetRandomInt(2 + (int)(density * 10));
            for (int i = 0; i < numOfAsteroids; ++i)
            {
                var asteroid = new Asteroid(this.game.framework, this.game.sceneManager, RandomHelper.GetRandomInt(Asteroid.NumTypes - 1));
                asteroid.MeshNode.Position = new Vector3(x * Level.SectorWidth,
                                                         this.game.Camera.EyePos.y + RandomHelper.GetRandomFloat(-Level.SectorWidth * 3.15f, +Level.SectorWidth * 3.15f),
                                                         z * Level.SectorDepth);
                asteroid.MeshNode.Position += RandomHelper.GetRandomVector3(-Level.SectorWidth * 0.42f, +Level.SectorWidth * 0.42f);
                asteroids.Add(asteroid);
            }
        }

        private void GenerateSmallAsteroids(List<SmallAsteroid> smallAsteroids, int numOfAsteroids, int x, int z)
        {
            // Always create at least 1 smaller asteroid instance per sector
            int numOfSmallAsteroids = 2 + RandomHelper.GetRandomInt(4 + numOfAsteroids);

            for (int i = 0; i < numOfSmallAsteroids; ++i)
            {
                int type = RandomHelper.GetRandomInt(SmallAsteroid.NumTypes - 1);
                var smallAsteroid = new SmallAsteroid(this.game.framework, this.game.sceneManager, type);
                smallAsteroid.MeshNode.Position = new Vector3(x * Level.SectorWidth, 0, z * Level.SectorDepth);
                smallAsteroid.MeshNode.Position += new Vector3(RandomHelper.GetRandomFloat(-Level.SectorWidth / 2, +Level.SectorWidth / 2),
                                                               RandomHelper.GetRandomFloat(-Level.SectorWidth * 2.1f, +Level.SectorWidth * 2.1f),
                                                               RandomHelper.GetRandomFloat(-Level.SectorWidth / 2, +Level.SectorWidth / 2));
                smallAsteroids.Add(smallAsteroid);
            }
        }

        private void UpdateSectors()
        {
            UpdateSectorMovement();
            UpdateSectorVisibility();
        }

        private void UpdateSectorMovement()
        {
            int cameraSectorPosX = (int)MathUtils.Round(this.game.Camera.EyePos.x / Level.SectorWidth);
            int cameraSectorPosZ = (int)MathUtils.Round(this.game.Camera.EyePos.z / Level.SectorDepth);

            if (this.lastCameraSectorPosX == cameraSectorPosX && this.lastCameraSectorPosZ == cameraSectorPosZ)
                return;

            // How much we are momving
            int movedXSectors = cameraSectorPosX - this.lastCameraSectorPosX;
            int movedZSectors = cameraSectorPosZ - this.lastCameraSectorPosZ;

            // Normal asteroids
            var helperCopyAsteroids = new List<Asteroid>[numberOfSectors, numberOfSectors];
            for (int z = 0; z < numberOfSectors; ++z)
            {
                for (int x = 0; x < numberOfSectors; ++x)
                {
                    helperCopyAsteroids[z, x] = new List<Asteroid>();

                    // Can we copy the sector over from sectorAsteroids
                    if (x >= -movedXSectors && z >= -movedZSectors &&
                        x < numberOfSectors - movedXSectors && z < numberOfSectors - movedZSectors)
                    {
                        helperCopyAsteroids[z, x] = this.asteroids[z + movedZSectors, x + movedXSectors];
                        this.asteroids[z + movedZSectors, x + movedXSectors] = null;
                    }
                    else
                    {
                        GenerateAsteroids(helperCopyAsteroids[z, x], x - middleSector + cameraSectorPosX, z - middleSector + cameraSectorPosZ);
                    }
                }
            }
            for (int z = 0; z < numberOfSectors; ++z)
            {
                for (int x = 0; x < numberOfSectors; ++x)
                {
                    if (this.asteroids[z, x] != null)
                    {
                        this.asteroids[z, x].ForEach(a => this.game.sceneManager.RemoveRenderable(a.MeshNode));
                    }
                }
            }
            this.asteroids = helperCopyAsteroids;

            // Small asteroids
            var helperCopySmallAsteroids = new List<SmallAsteroid>[numberOfSmallSectors, numberOfSmallSectors];
            for (int z = 0; z < numberOfSmallSectors; ++z)
            {
                for (int x = 0; x < numberOfSmallSectors; ++x)
                {
                    helperCopySmallAsteroids[z, x] = new List<SmallAsteroid>();

                    if (x >= -movedXSectors && z >= -movedZSectors &&
                        x < numberOfSmallSectors - movedXSectors && z < numberOfSmallSectors - movedZSectors)
                    {
                        helperCopySmallAsteroids[z, x] = this.smallAsteroids[z + movedZSectors, x + movedXSectors];
                        this.smallAsteroids[z + movedZSectors, x + movedXSectors] = null;
                    }
                    else
                    {
                        GenerateSmallAsteroids(helperCopySmallAsteroids[z, x],
                            this.asteroids[z + smallSectorAdd, x + smallSectorAdd].Count,
                            x - smallMiddleSector + cameraSectorPosX,
                            z - smallMiddleSector + cameraSectorPosZ);
                    }
                }
            }
            for (int z = 0; z < numberOfSmallSectors; ++z)
            {
                for (int x = 0; x < numberOfSmallSectors; ++x)
                {
                    if (this.smallAsteroids[z, x] != null)
                    {
                        this.smallAsteroids[z, x].ForEach(a => this.game.sceneManager.RemoveRenderable(a.MeshNode));
                    }
                }
            }
            this.smallAsteroids = helperCopySmallAsteroids;

            this.lastCameraSectorPosX = cameraSectorPosX;
            this.lastCameraSectorPosZ = cameraSectorPosZ;
        }

        private void UpdateSectorVisibility()
        {
            for (int z = minSector; z <= maxSector; ++z)
            {
                for (int x = minSector; x <= maxSector; ++x)
                {
                    int iz = z + middleSector;
                    int ix = x + middleSector;

                    bool isVisible = this.sectorVisibleInRange[iz, ix];

                    if (isVisible && MathUtils.Abs(this.game.Camera.Direction.y) < 0.75f)
                    {
                        var angle = Radian.AngleBetween(this.game.Camera.Direction, sectorDirection[iz, ix]);
                        isVisible = angle < ViewableFieldOfView;
                    }

                    if (MathUtils.Abs(x) + MathUtils.Abs(z) <= 2)
                        isVisible = true;

                    this.sectorIsVisible[iz, ix] = isVisible;
                }
            }
        }
    }
}
