using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Security;
using System.Runtime.InteropServices;
using System.Threading;
using System.Diagnostics;

namespace Irelia.Render
{
    // Provides basic graphics device initialization, game logic, rendering code, and a game loop.
    public class Game
    {
        #region Events
        public event EventHandler<EventArgs> Exiting = delegate { };
        #endregion

        #region Properties
        private static bool IsApplicationIdle
        {
            get
            {
                Message msg;
                return !PeekMessage(out msg, IntPtr.Zero, 0, 0, 0);
            }
        }

        public int Width { get; private set; }
        public int Height { get; private set; }
        public int TargetElapsedTime
        {
            get { return this.targetElapsedTime; }
            set
            {
                if (value <= 0.0f)
                    throw new ArgumentOutOfRangeException("TargetElapsedTime", "Specify a nonzero positive value");
                this.targetElapsedTime = value;
            }
        }

        public int TotalGameTime { get; private set; }
        #endregion

        #region Fields
        protected readonly Form form;
        public readonly Framework framework;
        public readonly SceneManager sceneManager;
        private int targetElapsedTime = 1000 / 60;
        private readonly Timer timer = new Timer();
        private int lastUpdateTime;
        private int lastDrawTime;
        private bool needExit;
        #endregion

        #region Constructor
        public Game(string title, int width, int height)
        {
            Width = width;
            Height = height;

            this.form = new Form()
            {
                Text = title,
                ClientSize = new System.Drawing.Size(Width, Height)
            };

            // Initialize render system
            string mediaPath = Path.Combine(Directory.GetCurrentDirectory(), "../../../Media");
            RenderSettings.MediaPath = Path.GetFullPath(mediaPath);

            this.framework = new Framework(form.Handle, Width, Height, RenderSettings.MediaPath);
            this.sceneManager = new SceneManager(this.framework.Device, this.framework.AssetManager);
            this.sceneManager.Camera.AspectRatio = (float)Width / (float)Height;
        }
        #endregion

        #region Public Methods
        public void Run()
        {
            Initialize();

            Application.Idle += (o, e) =>
            {
                while (IsApplicationIdle)
                {
                    if (this.needExit)
                        this.form.Close();
                    else
                        RunOneFrame();
                }
            };
            Application.Run(form);
        }

        public void Exit()
        {
            OnExiting(this, EventArgs.Empty);
        }

        public void RunOneFrame()
        {
            int nextUpdateTime = this.timer.Elapsed + TargetElapsedTime;

            // Update
            Update(this.timer.Elapsed - this.lastUpdateTime);
            this.lastUpdateTime = this.timer.Elapsed;

            // After update is called, if it is not time to call Update again, calls Draw.
            if (this.timer.Elapsed < nextUpdateTime)
            {
                Draw(this.timer.Elapsed - this.lastDrawTime);
                this.lastDrawTime = this.timer.Elapsed;
            }

            // If it is not time to call update again, idles until it is time to call Update.
            if (this.timer.Elapsed < nextUpdateTime)
            {
                Thread.Sleep(nextUpdateTime - this.timer.Elapsed);
            }
        }
        #endregion

        #region Protected Methods
        // Called after the Game and graphics are created
        protected virtual void Initialize()
        {
            this.timer.Start();
        }

        // Called when the game has determined that game logic needs to be processed
        //  elapsed: Time passed since the last call to Update
        protected virtual void Update(int elapsed)
        {
            TotalGameTime += elapsed;
        }

        // Called when the game determines it is time to draw a frame
        //  elapsed: Time passed since the last call to draw
        protected virtual void Draw(float elapsed)
        {
            this.sceneManager.Render(this.framework.Renderer, this.framework.Renderer.PrimaryRenderTarget, true);
        }

        // Raises an Exiting event. Override this method to add code to handle when the game is exiting
        protected virtual void OnExiting(object sender, EventArgs args)
        {
            Exiting(sender, args);
            this.needExit = true;
        }
        #endregion

        #region PInvoke
        [StructLayout(LayoutKind.Sequential)]
        public struct Message
        {
            public IntPtr hWnd;
            public uint msg;
            public IntPtr wParam;
            public IntPtr lParam;
            public uint time;
            public System.Drawing.Point p;
        }

        [SuppressUnmanagedCodeSecurity]
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool PeekMessage(out Message msg, IntPtr hWnd, uint messageFilterMin, uint messageFilterMax, uint flags);
        #endregion
    }
}
