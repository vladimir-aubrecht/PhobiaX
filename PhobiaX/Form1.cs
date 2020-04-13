using System;
using System.Drawing;
using System.IO;
using System.Xml;


namespace PhobiaX
{

    public struct Enviroment
    {
        private static int ResolutionX = 1024;

        public static int RozliseniX
        {
            set
            {
                ResolutionX = value;
            }

            get
            {
                return ResolutionX;
            }
        }

        private static int ResolutionY = 768;
        public static int RozliseniY
        {
            set
            {
                ResolutionY = value;
            }

            get
            {
                return ResolutionY;
            }
        }
        public static Random rand = new Random();
    }
    public struct Hrdina
    {

        private int hx;
        private int hy;
        private int smer;
        private int krok;
        private int ckroku;
        private int zamerovacX;
        private int zamerovacY;

        public int x
        {
            set
            {
                if ((value > 0) && (value < Enviroment.RozliseniX - Phobia0.HHrdina.Width))
                    hx = value;
            }

            get
            {
                return hx;
            }

        }

        public int y
        {
            set
            {
                if ((value > 0) && (value < Enviroment.RozliseniY - Phobia0.HHrdina.Height))
                    hy = value;
            }

            get
            {
                return hy;
            }

        }

        public int Smer
        {
            set
            {
                smer = value;

                if (smer < 0) smer = (Phobia0.PocetSmeru - 1) + smer;
                if (smer > (Phobia0.PocetSmeru - 1)) smer = smer - Phobia0.PocetSmeru;
            }

            get
            {
                return smer;
            }

        }
        public int Krok
        {
            set
            {
                krok = value;
            }

            get
            {
                return krok;
            }
        }
        public int CKroku
        {
            set
            {
                ckroku = value;
                if (ckroku > 25)
                    ckroku = 0;

                if (ckroku < 0)
                    ckroku = 25;
            }

            get
            {
                return ckroku;
            }

        }
        public int ZamerovacX
        {
            set
            {
                zamerovacX = value;
            }

            get
            {
                return zamerovacX;
            }
        }
        public int ZamerovacY
        {
            set
            {
                zamerovacY = value;
            }

            get
            {
                return zamerovacY;
            }
        }

    }

    public struct Strela
    {
        private int x;
        private int y;
        private int delka;
        private int smerx;
        private int smery;

        public int X
        {

            set
            {
                x = value;
            }

            get
            {
                return x;
            }

        }

        public int Y
        {

            set
            {
                y = value;
            }

            get
            {
                return y;
            }

        }
        public int SmerX
        {

            set
            {
                smerx = value;
            }

            get
            {
                return smerx;
            }

        }

        public int SmerY
        {

            set
            {
                smery = value;
            }

            get
            {
                return smery;
            }

        }
        public int Delka
        {
            set
            {
                delka = value;
            }
            get
            {
                return delka;
            }
        }
    }

    /// <summary>
    /// Summary description for Phobia0.
    /// </summary>
    public class Phobia0 : System.Windows.Forms.Form
    {

        #region Promeny, objekty, atp.
        private System.ComponentModel.IContainer components;
        private Microsoft.DirectX.DirectDraw.Device Draw = null;
        private Microsoft.DirectX.DirectInput.Device InputDevice = null;
        private Microsoft.DirectX.DirectSound.Device SoundDevice = null;
        private System.Windows.Forms.Timer grabData;
        private string[] Stisknuto = new string[32];
        private BaseAlien[] Alieni = new BaseAlien[PocetAlienu];
        private static Hrdina hrdina = new Hrdina();
        private static Hrdina hrdina2 = new Hrdina();
        private Surface front = null;
        private Surface back = null;
        private System.Timers.Timer AlienTimer;
        private bool needRestore = false;
        private System.Timers.Timer Strileni;
        private Strela S = new Strela();
        private Strela S2 = new Strela();
        private int NowStrela = 0;
        private int NowStrela2 = 0;
        private int Mrtvola = (-1);
        private Random ran66 = new Random();
        private int Body = 0;
        private int Zivota = 100;
        private bool Prohra = false;
        public static Image HHrdina = new Bitmap("res\\Hrdina\\" + hrdina.Smer + "\\" + hrdina.CKroku + ".bmp");
        public static Image HHrdina2 = new Bitmap("res\\Hrdina\\" + hrdina2.Smer + "\\" + hrdina2.CKroku + ".bmp");
        public static Image Lekarnicka = new Bitmap("res\\Ostatni\\lekarna.bmp");
        private readonly static Image Skore = new Bitmap("res\\Ostatni\\score.bmp");
        private readonly static Image Energie = new Bitmap("res\\Ostatni\\energie.bmp");
        private readonly static Image Zamerovac = new Bitmap("res\\Ostatni\\Zamerovac.bmp");
        private static Image pozadi = new Bitmap("res\\enviroment\\menu.bmp");
        private Image Alien = new Bitmap("res\\Alieni\\0\\0.bmp");
        private int SirkaStrely = 6;
        private int OrigSmer = 0;
        private int zbran = 1;
        private int OrigSmer2 = 0;
        private int zbran2 = 1;
        private bool Prohra2 = false;
        private int Zivota2 = 100;
        private int Body2 = 0;

        #region ImageAttributes
        private System.Drawing.Imaging.ImageAttributes ImgAttrib = new System.Drawing.Imaging.ImageAttributes();
        private System.Drawing.Imaging.ImageAttributes ImgAttrib2 = new System.Drawing.Imaging.ImageAttributes();
        private System.Drawing.Imaging.ImageAttributes ImgAttrib3 = new System.Drawing.Imaging.ImageAttributes();
        private System.Drawing.Imaging.ImageAttributes ImgAttrib4 = new System.Drawing.Imaging.ImageAttributes();
        private System.Drawing.Imaging.ImageAttributes ImgAttrib5 = new System.Drawing.Imaging.ImageAttributes();
        #endregion ImageAttributes

        private Rectangle EnergieRect = new Rectangle(Enviroment.RozliseniX - Energie.Width, 0, Energie.Width, Energie.Height);
        private Rectangle Score = new Rectangle(0, 0, Skore.Width, Skore.Height);
        private Brush pismo = new SolidBrush(Color.White);
        private Rectangle[] OldAlienRect = new Rectangle[PocetAlienu];
        private int[] SmerTel = new int[PocetAlienu];
        private static Image Mrtvolka = new Bitmap("res\\umrti\\alieni\\0.bmp");
        private int frameofanimmrtvola = 0;
        private bool[] Mrtvoly = new bool[PocetAlienu];
        private Rectangle NullRec = new Rectangle((-1), (-1), 1, 1);
        private const string StrelbaSoundRaketomet = "res\\Zvuky\\ROCKET.wav";
        private const string StrelbaSoundSamopal = "res\\Zvuky\\Machine Gun.wav";
        private const string LekarnaZvuk = "res\\Zvuky\\Emergency Buzzer.wav";
        private const string Aplause = "res\\Zvuky\\applause.wav";
        private const string klep = "res\\Zvuky\\hammer.wav";
        private const string UmrtiZvuk = "res\\Zvuky\\laughing.wav";
        private const string UmrtiAlienu = "res\\Zvuky\\Screaming.wav";
        private Microsoft.DirectX.DirectSound.SecondaryBuffer StrelbaBuffer1 = null;
        private Microsoft.DirectX.DirectSound.SecondaryBuffer StrelbaBuffer2 = null;
        private Microsoft.DirectX.DirectSound.SecondaryBuffer UmrtiAlienuBuffer = null;
        private Microsoft.DirectX.DirectSound.SecondaryBuffer Zvuky = null;
        private System.Timers.Timer Strileni2;

        #region Tlacitka
        private Tlacitko Start = new Tlacitko("res\\enviroment\\startgame.bmp");
        private Tlacitko Option = new Tlacitko("res\\enviroment\\option.bmp");
        private Tlacitko Credits = new Tlacitko("res\\enviroment\\Credits.bmp");
        private Tlacitko Zvuk = new Tlacitko("res\\enviroment\\Zvuk.bmp");
        private Tlacitko Ok = new Tlacitko("res\\enviroment\\Ok.bmp");
        private Tlacitko Cancel = new Tlacitko("res\\enviroment\\Cancel.bmp");
        private Tlacitko Exit = new Tlacitko("res\\enviroment\\exit.bmp");
        private Tlacitko HighScore = new Tlacitko("res\\enviroment\\HighScore.bmp");
        private Tlacitko Jeden = new Tlacitko("res\\enviroment\\jeden.bmp");
        private Tlacitko Dva = new Tlacitko("res\\enviroment\\dva.bmp");
        private Tlacitko Zpet = new Tlacitko("res\\enviroment\\zpet.bmp");
        private Tlacitko UpArrow = new Tlacitko("res\\enviroment\\UpArrow.bmp");
        private Tlacitko DownArrow = new Tlacitko("res\\enviroment\\DownArrow.bmp");
        #endregion Tlacitka

        #region Textik
        private Textik Hudba = null;
        private Textik ZvukTextik = null;
        private Textik HudbaHlasitost = null;
        private Textik ZvukHlasitost = null;
        #endregion Textik

        private Microsoft.DirectX.AudioVideoPlayback.Audio hudba = null;
        private string mp3 = "\\res\\hudba\\";
        private int cyklus = 0;
        private int SirkaStrely2 = 6;
        private bool NaHrace1 = true;
        private int PosunStrely = 90;
        private int MinHrdinaX = 0;
        private int MinHrdinaY = 0;
        private int MinHrdina2X = 0;
        private int MinHrdina2Y = 0;
        private string[] pisnicky = new String[512];
        private bool obnova = true;
        private bool obnova2 = true;
        private bool hra = false;
        private Video Intro = new Video("res\\Movies\\Intro.avi", false);
        private bool zapsano = false;
        private bool zapsano2 = false;
        private bool podpis = false;
        private string jmena = String.Empty;
        private bool podpis2 = false;
        private string jmena2 = String.Empty;
        private bool UmrtiZvukBylo = false;
        private bool zapis = false;
        private bool zapis2 = false;
        private string PocetHracu = "Base";
        private int skladba = 0;
        private int Vybuchovac = 0;
        private int Vybuchovac2 = 0;
        private int Vybuchovac3 = 0;
        private int Vybuchovac4 = 0;
        private Rectangle ZalohaVybuchRec2 = new Rectangle(-50, -50, 1, 1);
        private bool ZmenaVybuchu = false;
        private Rectangle ZalohaVybuchRec = new Rectangle(-50, -50, 1, 1);
        private Rectangle ZalohaVybuchRec3 = new Rectangle(-50, -50, 1, 1);
        private Rectangle ZalohaVybuchRec4 = new Rectangle(-50, -50, 1, 1);
        private int PocetVybuchu = 0;
        private int PocetVybuchu2 = 0;
        private bool ZmenaVybuchu2 = false;
        private bool umrti2 = false;
        private int Vybuchy2 = (-5);
        private bool ZmenaVybuchu3 = false;
        private bool ZmenaVybuchu4 = false;
        private bool blokace = true;

        private int Cyklus
        {
            set
            {
                cyklus = value;
                if (cyklus >= 5)
                {
                    cyklus = 0;

                    int nahodnik = Enviroment.rand.Next(0, 2);

                    if (nahodnik == 1)
                        NaHrace1 = false;
                    else
                        NaHrace1 = true;
                }
            }

            get
            {
                return cyklus;
            }
        }
        private double zamerovacsmer = 0.0;
        private double ZamerovacSmer
        {

            set
            {
                zamerovacsmer = value;

                if (zamerovacsmer > 360)
                    zamerovacsmer = zamerovacsmer - 360;
                else if (zamerovacsmer < 0)
                    zamerovacsmer = 360 + zamerovacsmer;

                hrdina.Smer = (int)(zamerovacsmer / (360 / PocetSmeru));
            }

            get
            {
                return zamerovacsmer;
            }

        }

        private double zamerovacsmer2 = 0.0;
        private double ZamerovacSmer2
        {

            set
            {
                zamerovacsmer2 = value;

                if (zamerovacsmer2 > 360)
                    zamerovacsmer2 = zamerovacsmer2 - 360;
                else if (zamerovacsmer2 < 0)
                    zamerovacsmer2 = 360 + zamerovacsmer2;

                hrdina2.Smer = (int)(zamerovacsmer2 / (360 / PocetSmeru));
            }

            get
            {
                return zamerovacsmer2;
            }

        }

        private int FrameOfAnimMrtvola
        {

            set
            {
                if (value <= PocetFramuMrtvol)
                    frameofanimmrtvola = value;
                else
                    frameofanimmrtvola = 0;
            }

            get
            {
                return frameofanimmrtvola;
            }
        }

        private bool DvaHraci = false;
        private string Error = "";
        private Point Lekarna = new Point(-50, -50);
        private bool DisplayHighScore = false;
        private int Vybuchy = (-5);
        private bool umrti = false;


        public const int PocetSmeru = 32;
        public const int Rozptyl = 400;
        public const int ColorDepht = 16;
        public const int PocetAlienu = 26;
        public const int PocetFramuMrtvol = 6;
        public const int UhelOtoceni = 8;
        public const int MaxHeal = 100;

        #endregion Promeny, objekty, atp.

        public Phobia0()
        {

            InitializeDSound();
            Restart(true, false);

            ImgAttrib.SetColorKey(Color.FromArgb(2, 65, 17), Color.FromArgb(2, 66, 18));
            ImgAttrib2.SetColorKey(Color.FromArgb(0, 0, 183), Color.FromArgb(41, 53, 255));
            ImgAttrib3.SetColorKey(Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255));
            ImgAttrib4.SetColorKey(Color.FromArgb(0, 4, 0), Color.FromArgb(114, 217, 111));
            ImgAttrib5.SetColorKey(Color.FromArgb(0, 118, 0), Color.FromArgb(128, 255, 121));



            InitializeComponent();
            InitializeDDraw();
            InitializeDInput();

            pisnicky = Directory.GetFiles(Directory.GetCurrentDirectory() + mp3);

            hudba = new Audio(pisnicky[skladba], true);
            //hudba.Volume = 100;
            hudba.Ending += new EventHandler(hudba_Ending);

            this.AlienTimer.Stop();
            this.grabData.Stop();


            UmrtiAlienuBuffer = new Microsoft.DirectX.DirectSound.SecondaryBuffer(UmrtiAlienu, SoundDevice);


            //DisplayVideo();

            //Intro.Ending += new EventHandler(this.Movie_Ending);


            while (Created)
            {

                if (hra)
                {
                    DisplayFrame();
                }
                else
                {
                    DisplayMenu();
                }


                Application.DoEvents();
            }
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Draw.RestoreDisplayMode();
                Draw.SetCooperativeLevel(this, Microsoft.DirectX.DirectDraw.CooperativeLevelFlags.Normal);

                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);

        }


        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.grabData = new System.Windows.Forms.Timer(this.components);
            this.AlienTimer = new System.Timers.Timer();
            this.Strileni = new System.Timers.Timer();
            this.Strileni2 = new System.Timers.Timer();

            ((System.ComponentModel.ISupportInitialize)(this.AlienTimer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Strileni)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Strileni2)).BeginInit();
            // 
            // grabData
            // 
            this.grabData.Interval = 10;
            this.grabData.Tick += new System.EventHandler(this.grabData_Tick);
            // 
            // AlienTimer
            // 
            this.AlienTimer.Enabled = true;
            this.AlienTimer.Interval = 40;
            this.AlienTimer.SynchronizingObject = this;
            this.AlienTimer.Elapsed += new System.Timers.ElapsedEventHandler(this.AlienTimer_Elapsed);
            // 
            // Strileni
            // 
            this.Strileni.Enabled = true;
            this.Strileni.Interval = 1;
            this.Strileni.SynchronizingObject = this;
            this.Strileni.Elapsed += new System.Timers.ElapsedEventHandler(this.Strileni_Elapsed);
            // 
            // Strileni2
            // 
            this.Strileni2.Enabled = true;
            this.Strileni2.Interval = 1;
            this.Strileni2.SynchronizingObject = this;
            this.Strileni2.Elapsed += new System.Timers.ElapsedEventHandler(this.Strileni2_Elapsed);
            // 
            // Phobia0
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Phobia0";
            this.Text = "PhobiaX";
            this.Click += new System.EventHandler(this.Phobia0_Click);
            this.MouseMove += new MouseEventHandler(Phobia0_MouseMove);
            ((System.ComponentModel.ISupportInitialize)(this.AlienTimer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Strileni)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Strileni2)).EndInit();

        }
        #endregion

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] Args)
        {
            if (Args.Length >= 2)
                if ((Convert.ToInt32(Args[0]) >= 640) && (Convert.ToInt32(Args[1]) >= 480) && (Convert.ToInt32(Args[0]) <= 1600) && (Convert.ToInt32(Args[1]) <= 1200))
                {
                    Enviroment.RozliseniX = Convert.ToInt32(Args[0]);
                    Enviroment.RozliseniY = Convert.ToInt32(Args[1]);
                }

            Phobia0 phob = new Phobia0();
            Application.Exit();
        }

        private void Restart(bool firststart, bool LoadUn)
        {
            hrdina.Krok = 10;
            S.Delka = 5;
            S2.Delka = 5;

            Random ran = new Random();

            for (int k = 0; k < Alieni.Length; k++)
            {
                Alieni[k] = new BaseAlien(ran.NextDouble() * 10000000);
                OldAlienRect[k] = NullRec;
                Mrtvoly[k] = false;
                SmerTel[k] = 0;
            }

            hrdina.x = Enviroment.RozliseniX / 2;
            hrdina.y = Enviroment.RozliseniY / 2;
            hrdina.CKroku = 0;
            hrdina.Smer = 0;
            podpis = false;
            jmena = String.Empty;
            podpis2 = false;
            jmena2 = String.Empty;
            StrelbaBuffer1 = new Microsoft.DirectX.DirectSound.SecondaryBuffer(StrelbaSoundRaketomet, SoundDevice);
            StrelbaBuffer2 = new Microsoft.DirectX.DirectSound.SecondaryBuffer(StrelbaSoundRaketomet, SoundDevice);
            UmrtiZvukBylo = false;
            zapis = false;
            zapis2 = false;
            ZmenaVybuchu = false;
            PocetVybuchu = 0;
            PocetVybuchu2 = 0;
            ZalohaVybuchRec = new Rectangle(-50, -50, 1, 1);
            ZalohaVybuchRec3 = new Rectangle(-50, -50, 1, 1);
            ZalohaVybuchRec4 = new Rectangle(-50, -50, 1, 1);
            ZmenaVybuchu = false;
            ZmenaVybuchu2 = false;
            Vybuchovac = 0;
            Vybuchovac2 = 0;
            Vybuchovac3 = 0;
            Vybuchovac4 = 0;
            ZalohaVybuchRec2 = new Rectangle(-50, -50, 1, 1);
            Vybuchovac2 = 0;
            zbran = 1;
            zbran2 = 1;
            umrti2 = false;
            Vybuchy2 = (-5);
            ZmenaVybuchu3 = false;
            ZmenaVybuchu3 = false;
            blokace = true;

            zbran = 1;
            zbran2 = 1;
            Prohra = false;
            Zivota = 100;
            Body = 0;
            Mrtvola = (-1);
            NowStrela = 0;
            ZamerovacSmer = 0.0;
            ZamerovacSmer2 = 0.0;
            Lekarna.X = -50;
            Lekarna.Y = -50;
            zapsano = false;
            zapsano2 = false;

            if (DvaHraci && !LoadUn)
                LoadUnloadTwoPlayer();

            if (!firststart)
                this.AlienTimer.Start();

        }


        #region InicializaceDX
        private void InitializeDDraw()
        {
            SurfaceDescription SurfDesc = new SurfaceDescription();

            Draw = new Microsoft.DirectX.DirectDraw.Device();
            Draw.SetCooperativeLevel(this, Microsoft.DirectX.DirectDraw.CooperativeLevelFlags.FullscreenExclusive | Microsoft.DirectX.DirectDraw.CooperativeLevelFlags.AllowReboot);
            Draw.SetDisplayMode(Enviroment.RozliseniX, Enviroment.RozliseniY, ColorDepht, 0, false);

            SurfDesc.SurfaceCaps.PrimarySurface = SurfDesc.SurfaceCaps.Flip = SurfDesc.SurfaceCaps.Complex = true;
            SurfDesc.BackBufferCount = 1;

            front = new Surface(SurfDesc, Draw);
            SurfaceCaps caps = new SurfaceCaps();
            caps.BackBuffer = true;
            back = front.GetAttachedSurface(caps);
            back.ForeColor = Color.White;

            SurfDesc.Clear();

        }

        private void InitializeDInput()
        {

            grabData.Enabled = false;

            Microsoft.DirectX.DirectInput.CooperativeLevelFlags coopFlags = Microsoft.DirectX.DirectInput.CooperativeLevelFlags.Exclusive;
            coopFlags |= Microsoft.DirectX.DirectInput.CooperativeLevelFlags.Foreground;
            coopFlags |= Microsoft.DirectX.DirectInput.CooperativeLevelFlags.NoWindowsKey;


            InputDevice = new Microsoft.DirectX.DirectInput.Device(SystemGuid.Keyboard);
            InputDevice.SetCooperativeLevel(this, coopFlags);
            InputDevice.Acquire();

            grabData.Enabled = true;

        }

        private void InitializeDSound()
        {
            SoundDevice = new Microsoft.DirectX.DirectSound.Device();
            SoundDevice.SetCooperativeLevel(this, Microsoft.DirectX.DirectSound.CooperativeLevel.Normal);
        }

        #endregion InicializaceDX

        private void DisplayFrame()
        {
            if (null == front)
                return;

            if (false == Draw.TestCooperativeLevel())
            {
                needRestore = true;
                return;
            }

            if (true == needRestore)
            {
                needRestore = false;
                Draw.RestoreAllSurfaces();
            }
            if ((!Prohra && !DvaHraci) || (!Prohra && !Prohra2))
                this.grabData.Interval = 10;
            else
                this.grabData.Interval = 120;

            hrdina.ZamerovacX = 100;
            hrdina.ZamerovacY = -100;
            hrdina2.ZamerovacX = 100;
            hrdina2.ZamerovacY = -100;

            double uhel = ((Math.PI / 180) * (ZamerovacSmer + 90));

            hrdina.ZamerovacX *= ((int)(Math.Cos(uhel) * 100000));
            hrdina.ZamerovacY *= ((int)(Math.Sin(uhel) * 100000));
            hrdina.ZamerovacX /= 100000;
            hrdina.ZamerovacY /= 100000;


            double uhel2 = ((Math.PI / 180) * (ZamerovacSmer2 + 90));

            hrdina2.ZamerovacX *= ((int)(Math.Cos(uhel2) * 100000));
            hrdina2.ZamerovacY *= ((int)(Math.Sin(uhel2) * 100000));
            hrdina2.ZamerovacX /= 100000;
            hrdina2.ZamerovacY /= 100000;

            System.IntPtr dc = back.GetDc();
            Graphics g = Graphics.FromHdc(dc);



            if (!Prohra)
            {
                if ((hrdina.x != MinHrdinaX) || (hrdina.y != MinHrdinaY))
                    HHrdina = new Bitmap("res\\Hrdina\\" + hrdina.Smer + "\\" + hrdina.CKroku + ".bmp");
                else if (obnova)
                {
                    HHrdina = new Bitmap("res\\Hrdina\\klid\\" + hrdina.Smer + ".bmp");
                    obnova = false;
                }
                else
                    obnova = true;
            }
            else
                HHrdina = new Bitmap("res\\Umrti\\Hrdina\\" + hrdina.Smer + ".bmp");

            if (DvaHraci && !Prohra2)
            {
                if ((hrdina2.x != MinHrdina2X) || (hrdina2.y != MinHrdina2Y))
                    HHrdina2 = new Bitmap("res\\Hrdina\\" + hrdina2.Smer + "\\" + hrdina2.CKroku + ".bmp");
                else if (obnova2)
                {
                    HHrdina2 = new Bitmap("res\\Hrdina\\klid\\" + hrdina2.Smer + ".bmp");
                    obnova2 = false;
                }
                else
                    obnova2 = true;
            }
            else if (DvaHraci)
                HHrdina2 = new Bitmap("res\\Umrti\\Hrdina\\" + hrdina2.Smer + ".bmp");


            MinHrdinaX = hrdina.x;
            MinHrdinaY = hrdina.y;
            MinHrdina2X = hrdina2.x;
            MinHrdina2Y = hrdina2.y;

            //Pozadi
            g.DrawImage(pozadi, 0, 0, Enviroment.RozliseniX, Enviroment.RozliseniY);

            //Lekarna

            if ((Lekarna.X >= 0) && (Lekarna.Y >= 0))
            {
                Rectangle LekarnaRec = new Rectangle(Lekarna.X, Lekarna.Y, Lekarnicka.Width, Lekarnicka.Height);
                g.DrawImage(Lekarnicka, LekarnaRec, 0, 0, Lekarnicka.Width, Lekarnicka.Height, GraphicsUnit.Pixel, ImgAttrib);
            }

            //Alieni

            for (int z = 0; z < Alieni.Length; z++)
            {

                if (Mrtvola == z)
                {
                    OldAlienRect[z] = new Rectangle(Alieni[z].X, Alieni[z].Y, Alien.Width, Alien.Height);
                    SmerTel[z] = Alieni[z].Smer;
                    Alieni[z].VygenerujSouradnice(ran66.NextDouble() * 100000);
                    Mrtvola = (-1);
                    Mrtvoly[z] = true;

                    if (!blokace)
                    {
                        Vybuchy = z;
                        Vybuchy2 = z;
                        blokace = true;
                    }

                }

                if ((OldAlienRect[z] != NullRec) && Mrtvoly[z])
                {
                    Mrtvolka = new Bitmap("res\\umrti\\alieni\\" + SmerTel[z] + ".bmp");
                    g.DrawImage(Mrtvolka, OldAlienRect[z], 0, 0, Mrtvolka.Width, Mrtvolka.Height, GraphicsUnit.Pixel, ImgAttrib2);

                }

                if ((((Alieni[z].X - Alien.Width) > 0) && (Alieni[z].X < Enviroment.RozliseniX)) && (((Alieni[z].Y - Alien.Height) > 0) && (Alieni[z].Y < Enviroment.RozliseniY)))
                {
                    Rectangle AlieniRec = new Rectangle(Alieni[z].X, Alieni[z].Y, Alien.Width, Alien.Height);
                    Alien = new Bitmap("res\\Alieni\\" + Alieni[z].Smer + "\\" + Alieni[z].CKroku + ".bmp");
                    g.DrawImage(Alien, AlieniRec, 0, 0, Alien.Width, Alien.Height, GraphicsUnit.Pixel, ImgAttrib2);
                }

            }

            //Hrdina

            Rectangle HRec = new Rectangle(hrdina.x, hrdina.y, HHrdina.Width, HHrdina.Height);
            g.DrawImage(HHrdina, HRec, 0, 0, HHrdina.Width, HHrdina.Height, GraphicsUnit.Pixel, ImgAttrib);


            if (DvaHraci)
            {
                Rectangle HRec2 = new Rectangle(hrdina2.x, hrdina2.y, HHrdina2.Width, HHrdina2.Height);
                g.DrawImage(HHrdina2, HRec2, 0, 0, HHrdina2.Width, HHrdina2.Height, GraphicsUnit.Pixel, ImgAttrib);
            }


            // Vybuchy Strel
            if ((zbran == 1) && umrti && (Vybuchy != (-5)))
            {

                Bitmap Explose = new Bitmap("res\\Ostatni\\Raketa\\Vybuch\\" + Vybuchovac + ".bmp");
                Rectangle ExploseRec = new Rectangle(-50, -50, 1, 1);
                if (ZmenaVybuchu)
                {
                    ExploseRec = new Rectangle(OldAlienRect[Vybuchy].X, OldAlienRect[Vybuchy].Y, Explose.Width, Explose.Height);
                    ZalohaVybuchRec = ExploseRec;
                    ZmenaVybuchu = false;
                }
                else
                {
                    ExploseRec = ZalohaVybuchRec;
                }

                g.DrawImage(Explose, ExploseRec, 0, 0, Explose.Width, Explose.Height, GraphicsUnit.Pixel, ImgAttrib4);

                if (Vybuchovac < 40)
                    Vybuchovac++;
                else
                {
                    umrti = false;
                    Vybuchovac = 0;

                    if (PocetVybuchu > 1)
                    {
                        ZalohaVybuchRec = ZalohaVybuchRec2;
                        ZalohaVybuchRec2 = new Rectangle(-50, -50, 1, 1);
                        Vybuchovac = Vybuchovac2;
                        umrti = true;
                    }
                    else
                    {
                        Vybuchy = (-5);
                        ZalohaVybuchRec = new Rectangle(-50, -50, 1, 1);
                    }

                    PocetVybuchu--;
                }

                if (PocetVybuchu > 1)
                {
                    Bitmap Explose2 = new Bitmap("res\\Ostatni\\Raketa\\Vybuch\\" + Vybuchovac2 + ".bmp");
                    Rectangle ExploseRec2 = new Rectangle(-50, -50, 1, 1);
                    if (ZmenaVybuchu2)
                    {
                        ExploseRec2 = new Rectangle(OldAlienRect[Vybuchy].X, OldAlienRect[Vybuchy].Y, Explose.Width, Explose.Height);
                        ZalohaVybuchRec2 = ExploseRec2;
                        ZmenaVybuchu2 = false;
                    }
                    else
                    {
                        ExploseRec2 = ZalohaVybuchRec2;
                    }

                    g.DrawImage(Explose2, ExploseRec2, 0, 0, Explose2.Width, Explose2.Height, GraphicsUnit.Pixel, ImgAttrib4);

                    if (Vybuchovac2 < 40)
                        Vybuchovac2++;
                    else
                    {
                        Vybuchovac2 = 0;
                        if (PocetVybuchu > 1)
                            PocetVybuchu--;
                    }
                }
            }


            if ((zbran2 == 1) && umrti2 && (Vybuchy2 != (-5)))
            {

                Bitmap Explose = new Bitmap("res\\Ostatni\\Raketa\\Vybuch\\" + Vybuchovac3 + ".bmp");
                Rectangle ExploseRec = new Rectangle(-50, -50, 1, 1);
                if (ZmenaVybuchu3)
                {
                    ExploseRec = new Rectangle(OldAlienRect[Vybuchy2].X, OldAlienRect[Vybuchy2].Y, Explose.Width, Explose.Height);
                    ZalohaVybuchRec3 = ExploseRec;
                    ZmenaVybuchu3 = false;
                }
                else
                {
                    ExploseRec = ZalohaVybuchRec3;
                }

                g.DrawImage(Explose, ExploseRec, 0, 0, Explose.Width, Explose.Height, GraphicsUnit.Pixel, ImgAttrib4);

                if (Vybuchovac3 < 40)
                    Vybuchovac3++;
                else
                {
                    umrti2 = false;
                    Vybuchovac3 = 0;

                    if (PocetVybuchu2 > 1)
                    {
                        ZalohaVybuchRec3 = ZalohaVybuchRec4;
                        ZalohaVybuchRec4 = new Rectangle(-50, -50, 1, 1);
                        Vybuchovac3 = Vybuchovac4;
                        umrti2 = true;
                    }
                    else
                    {
                        Vybuchy2 = (-5);
                        ZalohaVybuchRec3 = new Rectangle(-50, -50, 1, 1);
                    }

                    PocetVybuchu2--;
                }

                if (PocetVybuchu2 > 1)
                {
                    Bitmap Explose2 = new Bitmap("res\\Ostatni\\Raketa\\Vybuch\\" + Vybuchovac4 + ".bmp");
                    Rectangle ExploseRec2 = new Rectangle(-50, -50, 1, 1);
                    if (ZmenaVybuchu4)
                    {
                        ExploseRec2 = new Rectangle(OldAlienRect[Vybuchy2].X, OldAlienRect[Vybuchy2].Y, Explose.Width, Explose.Height);
                        ZalohaVybuchRec4 = ExploseRec2;
                        ZmenaVybuchu4 = false;
                    }
                    else
                    {
                        ExploseRec2 = ZalohaVybuchRec4;
                    }

                    g.DrawImage(Explose2, ExploseRec2, 0, 0, Explose2.Width, Explose2.Height, GraphicsUnit.Pixel, ImgAttrib4);

                    if (Vybuchovac4 < 40)
                        Vybuchovac4++;
                    else
                    {
                        Vybuchovac4 = 0;
                        if (PocetVybuchu2 > 1)
                            PocetVybuchu2--;
                    }
                }
            }


            //Skore atp.
            g.DrawImage(Skore, Score, 0, 0, Skore.Width, Skore.Height, GraphicsUnit.Pixel, ImgAttrib3);
            g.DrawImage(Energie, EnergieRect, 0, 0, Energie.Width, Energie.Height, GraphicsUnit.Pixel, ImgAttrib3);

            g.DrawString("Hraè 1: " + Body.ToString(), new Font(FontFamily.GenericSansSerif.Name, 15), pismo, Skore.Width - 100, 60);
            g.DrawString("Hraè 1: " + Zivota.ToString() + "%", new Font(FontFamily.GenericSansSerif.Name, 15), pismo, Enviroment.RozliseniX - Energie.Width - 15, 60);

            if (DvaHraci)
            {
                g.DrawString("Hraè 2: " + Body2.ToString(), new Font(FontFamily.GenericSansSerif.Name, 15), pismo, Skore.Width - 100, 85);
                g.DrawString("Hraè 2: " + Zivota2.ToString() + "%", new Font(FontFamily.GenericSansSerif.Name, 15), pismo, Enviroment.RozliseniX - Energie.Width - 15, 85);
            }

            if (!DvaHraci)
                g.DrawString("Esc Menu      F2 Restart", new Font(FontFamily.GenericSansSerif, 12), new SolidBrush(Color.Goldenrod), (Enviroment.RozliseniX / 2) - 80, Enviroment.RozliseniY - 30);
            else
                g.DrawString("Esc Menu      F2 Restart", new Font(FontFamily.GenericSansSerif, 12), new SolidBrush(Color.Goldenrod), (Enviroment.RozliseniX / 2) - 80, Enviroment.RozliseniY - 30);

            string Zbran = "";

            switch (zbran)
            {
                case 1:
                    Zbran = "Øízená støela";
                    break;

                case 2:
                    Zbran = "Samopal";
                    break;
            }

            g.DrawString(Zbran, new Font(FontFamily.GenericSansSerif.Name, 14), pismo, (Enviroment.RozliseniX / 2) - (Zbran.Length * 4), 20);

            if (DvaHraci)
            {
                switch (zbran2)
                {
                    case 1:
                        Zbran = "Øízená støela";
                        break;

                    case 2:
                        Zbran = "Samopal";
                        break;
                }

                g.DrawString(Zbran, new Font(FontFamily.GenericSansSerif.Name, 14), pismo, (Enviroment.RozliseniX / 2) - (Zbran.Length * 4), 40);
            }
            if (Prohra && !Prohra2 && DvaHraci)
            {
                g.DrawString("Hráè 1 prohrál !!! :)", new Font(FontFamily.GenericSansSerif.Name, 20), new SolidBrush(Color.Red), (Enviroment.RozliseniX / 4 * 3), Enviroment.RozliseniY - 100);
                this.Strileni.Stop();
            }


            if (Prohra2 && !Prohra && DvaHraci)
            {
                g.DrawString("Hráè 2 prohrál !!! :)", new Font(FontFamily.GenericSansSerif.Name, 20), new SolidBrush(Color.Red), (Enviroment.RozliseniX / 4) - 100, Enviroment.RozliseniY - 100);
                this.Strileni2.Stop();
            }

            if ((Prohra && Prohra2) || (Prohra && !DvaHraci))
            {
                if (!UmrtiZvukBylo)
                {
                    Zvuky = new Microsoft.DirectX.DirectSound.SecondaryBuffer(UmrtiZvuk, SoundDevice);
                    Zvuky.Play(0, Microsoft.DirectX.DirectSound.BufferPlayFlags.Default);
                    UmrtiZvukBylo = true;
                }

                g.DrawString("Všichni jsou mrtvý !!! :)", new Font(FontFamily.GenericSansSerif.Name, 26), new SolidBrush(Color.Red), (Enviroment.RozliseniX / 2) - 180, Enviroment.RozliseniY / 3);
                this.AlienTimer.Stop();

                if (!podpis)
                {
                    g.DrawString("Prosím podepište se: [Hráè 1]", new Font(FontFamily.GenericSansSerif.Name, 20), new SolidBrush(Color.Red), (Enviroment.RozliseniX / 2) - 180, Enviroment.RozliseniY / 2);
                    g.DrawString(jmena + "_", new Font(FontFamily.GenericSansSerif.Name, 20), new SolidBrush(Color.Red), (Enviroment.RozliseniX / 2) - 180, (Enviroment.RozliseniY / 2) + 30);
                }
                else
                {
                    if (!zapsano)
                    {

                        XmlTextReader tr = null;
                        string ZalohaJmena = String.Empty;
                        int ZalohaScore = 0;
                        bool NotNewWrite = false;
                        int pocitadlo = 0;
                        bool ukoncit = false;
                        bool BestScore = false;
                        bool bylzapis = false;

                        if (File.Exists("HighScore.xml"))
                            tr = new XmlTextReader("highscore.xml");

                        XmlTextWriter tw = new XmlTextWriter("highscore-new.xml", System.Text.Encoding.UTF8);
                        tw.WriteStartDocument();
                        tw.WriteStartElement("HighScore");

                        if (File.Exists("HighScore.xml"))
                        {
                            int prejeti = 0;


                            while (tr.Read())
                            {
                                if ((prejeti != 0) && (tr.Name.ToLower().Trim() != "HighScore".ToLower().Trim()))
                                {

                                    if (BestScore && !bylzapis && !zapis)
                                    {
                                        Zvuky = new Microsoft.DirectX.DirectSound.SecondaryBuffer(Aplause, SoundDevice);
                                        Zvuky.Play(0, Microsoft.DirectX.DirectSound.BufferPlayFlags.Default);

                                        tw.WriteStartElement("Jmeno");
                                        tw.WriteAttributeString("value", jmena);
                                        tw.WriteString(Body.ToString());
                                        tw.WriteEndElement();

                                        bylzapis = true;
                                        BestScore = false;
                                        zapis = true;
                                    }

                                    if (XmlNodeType.Element == tr.NodeType)
                                    {
                                        if (ZalohaJmena != String.Empty)
                                        {
                                            tw.WriteStartElement("Jmeno");
                                            tw.WriteAttributeString("value", ZalohaJmena);
                                        }
                                        ZalohaJmena = tr.GetAttribute("value");
                                    }

                                    if (XmlNodeType.Text == tr.NodeType)
                                    {
                                        if (ZalohaScore != 0)
                                        {
                                            tw.WriteString(ZalohaScore.ToString());
                                            ukoncit = true;
                                        }

                                        if ((ZalohaScore != 0) && (Body <= ZalohaScore) && (Body >= System.Convert.ToInt32(tr.Value)) && (!NotNewWrite))
                                        {
                                            NotNewWrite = true;
                                        }
                                        else if (Body > System.Convert.ToInt32(tr.Value))
                                            BestScore = true;

                                        ZalohaScore = System.Convert.ToInt32(tr.Value);
                                    }

                                    if ((XmlNodeType.EndElement == tr.NodeType) && ukoncit)
                                    {
                                        tw.WriteEndElement();
                                        ukoncit = false;
                                    }

                                    if ((pocitadlo == 1) && !zapis)
                                    {
                                        tw.WriteStartElement("Jmeno");



                                        tw.WriteAttributeString("value", jmena);
                                        tw.WriteString(Body.ToString());
                                        tw.WriteEndElement();
                                        pocitadlo = 0;
                                        NotNewWrite = false;
                                        zapis = true;
                                    }
                                    if (NotNewWrite)
                                    {
                                        pocitadlo++;
                                    }

                                }
                                prejeti++;
                            }

                            tr.Close();

                        }

                        if (!File.Exists("HighScore.xml"))
                        {
                            tw.WriteStartElement("Jmeno");
                            tw.WriteAttributeString("value", jmena);
                            tw.WriteString(Body.ToString());
                            tw.WriteEndElement();

                            Zvuky = new Microsoft.DirectX.DirectSound.SecondaryBuffer(Aplause, SoundDevice);
                            Zvuky.Play(0, Microsoft.DirectX.DirectSound.BufferPlayFlags.Default);
                        }
                        else
                        {
                            tw.WriteStartElement("Jmeno");
                            tw.WriteAttributeString("value", ZalohaJmena);
                            tw.WriteString(ZalohaScore.ToString());
                            tw.WriteEndElement();
                        }

                        tw.WriteEndElement();
                        tw.WriteEndDocument();
                        tw.Close();

                        if (File.Exists("HighScore-new.xml"))
                        {
                            if (File.Exists("HighScore.xml"))
                                File.Delete("HighScore.xml");
                            File.Move("HighScore-new.xml", "HighScore.xml");
                        }

                        zapsano = true;
                    }
                }

                if (DvaHraci)
                {

                    if (!podpis2 && zapsano)
                    {
                        g.DrawString("Prosím podepište se: [Hráè 2]", new Font(FontFamily.GenericSansSerif.Name, 20), new SolidBrush(Color.Red), (Enviroment.RozliseniX / 2) - 180, Enviroment.RozliseniY / 2);
                        g.DrawString(jmena2 + "_", new Font(FontFamily.GenericSansSerif.Name, 20), new SolidBrush(Color.Red), (Enviroment.RozliseniX / 2) - 180, (Enviroment.RozliseniY / 2) + 30);
                    }
                    else if (podpis2 && zapsano)
                    {
                        if (!zapsano2)
                        {

                            XmlTextReader tr = null;
                            string ZalohaJmena = String.Empty;
                            int ZalohaScore = 0;
                            bool NotNewWrite = false;
                            int pocitadlo = 0;
                            bool ukoncit = false;
                            bool BestScore = false;
                            bool bylzapis = false;

                            if (File.Exists("HighScore.xml"))
                                tr = new XmlTextReader("highscore.xml");

                            XmlTextWriter tw = new XmlTextWriter("highscore-new.xml", System.Text.Encoding.UTF8);
                            tw.WriteStartDocument();
                            tw.WriteStartElement("HighScore");

                            if (File.Exists("HighScore.xml"))
                            {
                                int prejeti = 0;


                                while (tr.Read())
                                {
                                    if ((prejeti != 0) && (tr.Name.ToLower().Trim() != "HighScore".ToLower().Trim()))
                                    {

                                        if (BestScore && !bylzapis && !zapis2)
                                        {
                                            Zvuky = new Microsoft.DirectX.DirectSound.SecondaryBuffer(Aplause, SoundDevice);
                                            Zvuky.Play(0, Microsoft.DirectX.DirectSound.BufferPlayFlags.Default);

                                            tw.WriteStartElement("Jmeno");
                                            tw.WriteAttributeString("value", jmena2);
                                            tw.WriteString(Body2.ToString());
                                            tw.WriteEndElement();

                                            bylzapis = true;
                                            BestScore = false;
                                            zapis2 = true;
                                        }

                                        if (XmlNodeType.Element == tr.NodeType)
                                        {
                                            if (ZalohaJmena != String.Empty)
                                            {
                                                tw.WriteStartElement("Jmeno");
                                                tw.WriteAttributeString("value", ZalohaJmena);
                                            }
                                            ZalohaJmena = tr.GetAttribute("value");
                                        }

                                        if (XmlNodeType.Text == tr.NodeType)
                                        {
                                            if (ZalohaScore != 0)
                                            {
                                                tw.WriteString(ZalohaScore.ToString());
                                                ukoncit = true;
                                            }

                                            if ((ZalohaScore != 0) && (Body2 <= ZalohaScore) && (Body2 >= System.Convert.ToInt32(tr.Value)) && (!NotNewWrite))
                                            {
                                                NotNewWrite = true;
                                            }
                                            else if ((Body2 > System.Convert.ToInt32(tr.Value)) && (Body2 > System.Convert.ToInt32(ZalohaScore)))
                                                BestScore = true;

                                            ZalohaScore = System.Convert.ToInt32(tr.Value);
                                        }

                                        if ((XmlNodeType.EndElement == tr.NodeType) && ukoncit)
                                        {
                                            tw.WriteEndElement();
                                            ukoncit = false;
                                        }

                                        if ((pocitadlo == 1) && !zapis2)
                                        {
                                            tw.WriteStartElement("Jmeno");
                                            tw.WriteAttributeString("value", jmena2);
                                            tw.WriteString(Body2.ToString());
                                            tw.WriteEndElement();
                                            pocitadlo = 0;
                                            NotNewWrite = false;
                                            zapis2 = true;
                                        }
                                        if (NotNewWrite)
                                        {
                                            pocitadlo++;
                                        }

                                    }
                                    prejeti++;
                                }

                                tr.Close();

                            }

                            if (!File.Exists("HighScore.xml"))
                            {
                                tw.WriteStartElement("Jmeno");
                                tw.WriteAttributeString("value", jmena2);
                                tw.WriteString(Body2.ToString());
                                tw.WriteEndElement();

                                Zvuky = new Microsoft.DirectX.DirectSound.SecondaryBuffer(Aplause, SoundDevice);
                                Zvuky.Play(0, Microsoft.DirectX.DirectSound.BufferPlayFlags.Default);
                            }
                            else
                            {
                                tw.WriteStartElement("Jmeno");
                                tw.WriteAttributeString("value", ZalohaJmena);
                                tw.WriteString(ZalohaScore.ToString());
                                tw.WriteEndElement();
                            }

                            tw.WriteEndElement();
                            tw.WriteEndDocument();
                            tw.Close();

                            if (File.Exists("HighScore-new.xml"))
                            {
                                if (File.Exists("HighScore.xml"))
                                    File.Delete("HighScore.xml");
                                File.Move("HighScore-new.xml", "HighScore.xml");
                            }

                            zapsano2 = true;
                        }

                    }
                }
            }


            //Zamerovac
            if (!Prohra)
            {
                Rectangle ZamerovacRect = new Rectangle(hrdina.x + (HHrdina.Width / 2) + hrdina.ZamerovacX, hrdina.y + (HHrdina.Height / 2) + hrdina.ZamerovacY, Zamerovac.Width, Zamerovac.Height);
                g.DrawImage(Zamerovac, ZamerovacRect, 0, 0, Zamerovac.Width, Zamerovac.Height, GraphicsUnit.Pixel, ImgAttrib3);
            }

            if (DvaHraci && !Prohra2)
            {
                Rectangle ZamerovacRect2 = new Rectangle(hrdina2.x + (HHrdina2.Width / 2) + hrdina2.ZamerovacX, hrdina2.y + (HHrdina2.Height / 2) + hrdina2.ZamerovacY, Zamerovac.Width, Zamerovac.Height);
                g.DrawImage(Zamerovac, ZamerovacRect2, 0, 0, Zamerovac.Width, Zamerovac.Height, GraphicsUnit.Pixel, ImgAttrib3);
            }

            //strela
            if (this.Strileni.Enabled)
            {
                if (zbran == 2)
                    g.DrawLine(new Pen(Color.Gold, SirkaStrely), S.X, S.Y, S.X + S.SmerX, S.Y + S.SmerY);
                else
                {
                    Image Strela = new Bitmap("res\\ostatni\\raketa\\" + hrdina.Smer + ".bmp");
                    Rectangle StrelaRec = new Rectangle(S.X, S.Y, Strela.Width, Strela.Height);
                    g.DrawImage(Strela, StrelaRec, 0, 0, Strela.Width, Strela.Height, GraphicsUnit.Pixel, ImgAttrib);
                }

            }

            if (this.Strileni2.Enabled)
            {
                if (zbran2 == 2)
                    g.DrawLine(new Pen(Color.Gold, SirkaStrely2), S2.X, S2.Y, S2.X + S2.SmerX, S2.Y + S2.SmerY);
                else
                {
                    Image Strela = new Bitmap("res\\ostatni\\raketa\\" + hrdina2.Smer + ".bmp");
                    Rectangle StrelaRec = new Rectangle(S2.X, S2.Y, Strela.Width, Strela.Height);
                    g.DrawImage(Strela, StrelaRec, 0, 0, Strela.Width, Strela.Height, GraphicsUnit.Pixel, ImgAttrib);
                }
            }


            back.ReleaseDc(dc);

            back.DrawText(100, 100, Error, true);

            front.Flip(back, FlipFlags.DoNotWait);

        }

        private void DisplayMenu()
        {

            this.AlienTimer.Stop();

            if (null == front)
                return;

            if (false == Draw.TestCooperativeLevel())
            {
                needRestore = true;
                return;
            }

            if (true == needRestore)
            {
                needRestore = false;
                Draw.RestoreAllSurfaces();
            }


            System.IntPtr dc = back.GetDc();
            Graphics g = Graphics.FromHdc(dc);

            g.DrawImage(pozadi, 0, 0, Enviroment.RozliseniX, Enviroment.RozliseniY);

            if (PocetHracu == "Base")
            {
                Start.Dc = dc;
                Start.Vykresli(Enviroment.RozliseniX, 50);
                Option.Dc = dc;
                Option.Vykresli(Enviroment.RozliseniX, 150);
                HighScore.Dc = dc;
                HighScore.Vykresli(Enviroment.RozliseniX, 250);
                Credits.Dc = dc;
                Credits.Vykresli(Enviroment.RozliseniX, 350);
                Exit.Dc = dc;
                Exit.Vykresli(Enviroment.RozliseniX, 450);
            }
            else if (PocetHracu == "Hraci")
            {
                Jeden.Dc = dc;
                Jeden.Vykresli(Enviroment.RozliseniX, 50);
                Dva.Dc = dc;
                Dva.Vykresli(Enviroment.RozliseniX, 150);
                Zpet.Dc = dc;
                Zpet.Vykresli(Enviroment.RozliseniX, 250);
            }

            else if (PocetHracu == "Option")
            {

                Zvuk.Dc = dc;
                Zvuk.Vykresli(Enviroment.RozliseniX, 50);

                Zpet.Dc = dc;
                Zpet.Vykresli(Enviroment.RozliseniX, 150);
            }

            else if (PocetHracu == "Zvuk")
            {

                Image OptBckg = new Bitmap("res\\enviroment\\optionbackground.bmp");
                Rectangle OptBckgRec = new Rectangle(Enviroment.RozliseniX - OptBckg.Width, 0, OptBckg.Width, OptBckg.Height);
                g.DrawImage(OptBckg, OptBckgRec, 0, 0, OptBckg.Width, OptBckg.Height, GraphicsUnit.Pixel, ImgAttrib5);

                Hudba = new Textik(dc, "Hudba", Enviroment.RozliseniX / 2 - 200, 100);
                ZvukTextik = new Textik(dc, "Zvuk", Enviroment.RozliseniX / 2 - 200, 200);

                HudbaHlasitost = new Textik(dc, hudba.Volume, Enviroment.RozliseniX / 2 + 100, 100);
                ZvukHlasitost = new Textik(dc, 100, Enviroment.RozliseniX / 2 + 100, 200);

                UpArrow.Dc = dc;
                UpArrow.Vykresli(Enviroment.RozliseniX / 3 * 2 + 100, 110);

                DownArrow.Dc = dc;
                DownArrow.Vykresli(Enviroment.RozliseniX / 3 * 2 + 100, 135);

                Ok.Dc = dc;
                Ok.Vykresli((Enviroment.RozliseniX / 6 * 5) + 50, 700);

                Cancel.Dc = dc;
                Cancel.Vykresli(Enviroment.RozliseniX + 50, 700);
            }

            if (DisplayHighScore)
            {
                if (File.Exists("highscore.xml"))
                {
                    XmlTextReader tr = new XmlTextReader("highscore.xml");
                    int radek = 0;

                    while (tr.Read())
                    {

                        if (radek != 0)
                        {
                            try
                            {
                                g.DrawString(tr.GetAttribute("value"), new Font(FontFamily.GenericSansSerif.Name, 20), new SolidBrush(Color.Red), 400, 100 + (radek * 10));
                                g.DrawString(tr.Value, new Font(FontFamily.GenericSansSerif.Name, 20), new SolidBrush(Color.Red), 600, 100 + ((radek - 1) * 10));
                            }
                            catch { }
                        }
                        radek++;
                    }
                }

            }

            back.ReleaseDc(dc);


            front.Flip(back, FlipFlags.DoNotWait);

        }

        private void DisplayVideo()
        {



            Intro.Size = new Size(Enviroment.RozliseniX, Enviroment.RozliseniY);
            Intro.Owner = this;
            Intro.Fullscreen = true;
            Intro.Play();




        }
        private void StartGame()
        {

            PocetHracu = "Hraci";

        }

        private void OnePlayer()
        {
            pozadi = new Bitmap("res\\enviroment\\pozadi.bmp");
            hra = true;
            this.AlienTimer.Start();
            this.grabData.Start();
            Restart(false, false);
            DvaHraci = false;
        }

        private void TwoPlayers()
        {

            pozadi = new Bitmap("res\\enviroment\\pozadi.bmp");
            hra = true;
            this.AlienTimer.Start();
            this.grabData.Start();

            DvaHraci = true;
            LoadUnloadTwoPlayer();

        }

        private void FreeDirectInput()
        {
            if (null != InputDevice)
            {
                InputDevice.Unacquire();
                InputDevice.Dispose();
                InputDevice = null;
            }
        }


        #region Pomocny funkce pro DI
        bool ReadImmediateData()
        {
            string[] textNew = new string[32];

            KeyboardState state = null;

            if (null == InputDevice)
                return true;

            InputException ie = null;
            try
            {
                state = InputDevice.GetCurrentKeyboardState();
            }
            catch (DirectXException)
            {

                bool loop = true;
                do
                {
                    try
                    {
                        InputDevice.Acquire();
                    }
                    catch (InputLostException)
                    {
                        loop = true;
                    }
                    catch (InputException inputException)
                    {
                        ie = inputException;
                        loop = false;
                    }
                } while (loop);

                return true;
            }

            int t = 0;
            for (Key k = Key.Escape; k <= Key.MediaSelect; k++)
            {
                if (state[k])
                {
                    textNew[t] = k.ToString();
                    if (k == Key.Escape)
                    {
                        if (hra)
                        {
                            hra = false;
                            pozadi = new Bitmap("res\\enviroment\\menu.bmp");
                        }
                        else
                        {
                            pozadi = new Bitmap("res\\enviroment\\pozadi.bmp");
                            hra = true;
                            this.AlienTimer.Start();
                        }
                    }

                    t++;
                }
            }

            Stisknuto = textNew;

            return true;
        }

        private void grabData_Tick(object sender, System.EventArgs e)
        {
            if (!ReadImmediateData())
            {
                grabData.Enabled = false;
                MessageBox.Show("Chyba ve ètení vstupního statusu, aplikace bude ukonèena.", "Keyboard");
                Close();
            }


            for (int i = 0; i < Stisknuto.Length; i++)
            {
                if (!Prohra)
                {
                    if (Stisknuto[i] != String.Empty)
                        System.Console.WriteLine(Stisknuto[i]);
                    if (((Stisknuto[i] == "Up") || (Stisknuto[i] == "DownArrow")) && (!Prohra))
                    {

                        if (Stisknuto[i] == "DownArrow") hrdina.Krok = Math.Abs(hrdina.Krok) * (-1);
                        else hrdina.Krok = Math.Abs(hrdina.Krok);

                        double uhel = (Math.PI / 180) * (((360 / PocetSmeru) * hrdina.Smer) + 90);

                        hrdina.x += (int)(hrdina.Krok * Math.Cos(uhel));
                        hrdina.y -= (int)(hrdina.Krok * Math.Sin(uhel));

                        if (!this.Strileni.Enabled)
                        {
                            S.X = hrdina.x + ((HHrdina.Width / 2) - (HHrdina.Width % 2));
                            S.Y = hrdina.y + ((HHrdina.Height / 2) - (HHrdina.Height % 2));
                        }

                        if (Stisknuto[i] == "Up")
                            hrdina.CKroku++;
                        else
                            hrdina.CKroku--;
                    }
                }
                if (!Prohra2)
                {
                    if (((Stisknuto[i] == "W") || (Stisknuto[i] == "S")) && (DvaHraci) && (!Prohra2))
                    {

                        if (Stisknuto[i] == "S") hrdina2.Krok = Math.Abs(hrdina2.Krok) * (-1);
                        else hrdina2.Krok = Math.Abs(hrdina2.Krok);

                        double uhel = (Math.PI / 180) * (((360 / PocetSmeru) * hrdina2.Smer) + 90);

                        hrdina2.x += (int)(hrdina2.Krok * Math.Cos(uhel));
                        hrdina2.y -= (int)(hrdina2.Krok * Math.Sin(uhel));
                        if (!this.Strileni2.Enabled)
                        {
                            S2.X = hrdina2.x + ((HHrdina2.Width / 2) - (HHrdina2.Width % 2));
                            S2.Y = hrdina2.y + ((HHrdina2.Height / 2) - (HHrdina2.Height % 2));
                        }

                        if (Stisknuto[i] == "W")
                            hrdina2.CKroku++;
                        else
                            hrdina2.CKroku--;
                    }
                }

                if (!Prohra)
                {
                    if ((Stisknuto[i] == "Right") && (!Prohra))
                        ZamerovacSmer -= UhelOtoceni;

                    else if ((Stisknuto[i] == "LeftArrow") && (!Prohra))
                        ZamerovacSmer += UhelOtoceni;
                }

                if (!Prohra2)
                {
                    if ((Stisknuto[i] == "D") && (DvaHraci) && (!Prohra2))
                        ZamerovacSmer2 -= UhelOtoceni;

                    else if ((Stisknuto[i] == "A") && (DvaHraci) && (!Prohra2))
                        ZamerovacSmer2 += UhelOtoceni;
                }

                if (!Prohra || !Prohra2)
                {
                    if (((Stisknuto[i] == "LeftControl") && (!this.Strileni2.Enabled) && (!Prohra2) && DvaHraci) || ((Stisknuto[i] == "RightControl") && (!this.Strileni.Enabled) && (!Prohra)))
                    {
                        if (Stisknuto[i] == "RightControl")
                        {
                            OrigSmer = hrdina.Smer;
                            StrelbaBuffer1.Play(0, Microsoft.DirectX.DirectSound.BufferPlayFlags.Default);
                            this.Strileni.Start();
                        }
                        else
                        {
                            OrigSmer = hrdina2.Smer;
                            StrelbaBuffer2.Play(0, Microsoft.DirectX.DirectSound.BufferPlayFlags.Default);
                            this.Strileni2.Start();
                        }
                    }
                }

                if (!Prohra)
                {
                    if (Stisknuto[i] == "NumPad1")
                    {
                        this.Strileni.Interval = 20;
                        StrelbaBuffer1 = new Microsoft.DirectX.DirectSound.SecondaryBuffer(StrelbaSoundRaketomet, SoundDevice);
                        zbran = 1;
                    }
                    else if (Stisknuto[i] == "NumPad2")
                    {
                        this.Strileni.Interval = 1;
                        StrelbaBuffer1 = new Microsoft.DirectX.DirectSound.SecondaryBuffer(StrelbaSoundSamopal, SoundDevice);
                        zbran = 2;
                    }
                }

                if (!Prohra2)
                {
                    if (Stisknuto[i] == "D1")
                    {
                        this.Strileni2.Interval = 20;
                        StrelbaBuffer2 = new Microsoft.DirectX.DirectSound.SecondaryBuffer(StrelbaSoundRaketomet, SoundDevice);
                        zbran2 = 1;
                    }
                    else if (Stisknuto[i] == "D2")
                    {
                        this.Strileni2.Interval = 1;
                        StrelbaBuffer2 = new Microsoft.DirectX.DirectSound.SecondaryBuffer(StrelbaSoundSamopal, SoundDevice);
                        zbran2 = 2;
                    }
                }

                if (Stisknuto[i] == "F2")
                    Restart(false, false);


                if ((Prohra && !DvaHraci) || (Prohra && Prohra2))
                {
                    if (!podpis)
                    {
                        if ((Stisknuto[i] == "Return") || (Stisknuto[i] == "NumPadEnter"))
                        {
                            podpis = true;
                            Zvuky = new Microsoft.DirectX.DirectSound.SecondaryBuffer(klep, SoundDevice);
                            Zvuky.Play(0, Microsoft.DirectX.DirectSound.BufferPlayFlags.Default);
                        }
                        else
                        {
                            if (Stisknuto[i] != null)
                            {
                                if ((Stisknuto[i].Length == 1) || ((Stisknuto[i].Substring(0, 1) == "D") && (Stisknuto[i].Length == 2)))
                                {
                                    bool shift = false;
                                    for (int g = 0; g < Stisknuto.Length; g++)
                                        if ((Stisknuto[g] == "LeftShift") || (Stisknuto[g] == "RightShift"))
                                            shift = true;

                                    if (Stisknuto[i].Length == 2)
                                        Stisknuto[i] = Stisknuto[i].Substring(1, 1);

                                    if (jmena.Length < 10)
                                    {
                                        Zvuky = new Microsoft.DirectX.DirectSound.SecondaryBuffer(klep, SoundDevice);
                                        Zvuky.Play(0, Microsoft.DirectX.DirectSound.BufferPlayFlags.Default);

                                        if (shift == false)
                                            jmena += Stisknuto[i].ToLower();
                                        else
                                            jmena += Stisknuto[i].ToUpper();
                                    }

                                }
                                else if (Stisknuto[i] == "Back")
                                {
                                    if (jmena.Length > 0)
                                    {
                                        jmena = jmena.Substring(0, (jmena.Length - 1));
                                        Zvuky = new Microsoft.DirectX.DirectSound.SecondaryBuffer(klep, SoundDevice);
                                        Zvuky.Play(0, Microsoft.DirectX.DirectSound.BufferPlayFlags.Default);
                                    }
                                }
                            }
                        }
                    }

                    else if (DvaHraci && !podpis2)
                    {

                        if ((Stisknuto[i] == "Return") || (Stisknuto[i] == "NumPadEnter"))
                        {
                            podpis2 = true;
                            Zvuky = new Microsoft.DirectX.DirectSound.SecondaryBuffer(klep, SoundDevice);
                            Zvuky.Play(0, Microsoft.DirectX.DirectSound.BufferPlayFlags.Default);
                        }
                        else
                        {
                            if (Stisknuto[i] != null)
                            {
                                if ((Stisknuto[i].Length == 1) || ((Stisknuto[i].Substring(0, 1) == "D") && (Stisknuto[i].Length == 2)))
                                {
                                    bool shift = false;
                                    for (int g = 0; g < Stisknuto.Length; g++)
                                        if ((Stisknuto[g] == "LeftShift") || (Stisknuto[g] == "RightShift"))
                                            shift = true;
                                    if (Stisknuto[i].Length == 2)
                                        Stisknuto[i] = Stisknuto[i].Substring(1, 1);

                                    if (jmena2.Length < 10)
                                    {
                                        Zvuky = new Microsoft.DirectX.DirectSound.SecondaryBuffer(klep, SoundDevice);
                                        Zvuky.Play(0, Microsoft.DirectX.DirectSound.BufferPlayFlags.Default);

                                        if (shift == false)
                                            jmena2 += Stisknuto[i].ToLower();
                                        else
                                            jmena2 += Stisknuto[i].ToUpper();
                                    }
                                }
                                else if (Stisknuto[i] == "Back")
                                {
                                    if (jmena2.Length > 0)
                                    {
                                        jmena2 = jmena2.Substring(0, (jmena2.Length - 1));
                                        Zvuky = new Microsoft.DirectX.DirectSound.SecondaryBuffer(klep, SoundDevice);
                                        Zvuky.Play(0, Microsoft.DirectX.DirectSound.BufferPlayFlags.Default);
                                    }
                                }
                            }
                        }

                    }
                }

                Stisknuto[i] = "";


            }




        }

        #endregion Pomocny funkce pro DI

        private void LoadUnloadTwoPlayer()
        {

            if (DvaHraci)
            {
                hrdina2.x = Enviroment.RozliseniX / 4;
                hrdina2.y = Enviroment.RozliseniY / 4;
                hrdina2.CKroku = 0;
                hrdina2.Smer = 0;
                hrdina2.Krok = 10;

                zbran2 = 1;
                Prohra2 = false;
                Zivota2 = 100;
                Body2 = 0;
                NowStrela2 = 0;

                S2.X = hrdina2.x + ((HHrdina2.Width / 2) - (HHrdina2.Width % 2));
                S2.Y = hrdina2.y + ((HHrdina2.Height / 2) - (HHrdina2.Height % 2));



                Restart(false, true);

            }

        }

        private void AlienTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {

            Random ran0 = Enviroment.rand;
            const int Posuvnik = 2;
            Rectangle HrdinaRec = new Rectangle(hrdina.x + (HHrdina.Width / Posuvnik), hrdina.y + (HHrdina.Height / Posuvnik), HHrdina.Width - (HHrdina.Width / Posuvnik), HHrdina.Height - (HHrdina.Height / Posuvnik));
            Rectangle HrdinaRec2 = new Rectangle(hrdina2.x + (HHrdina2.Width / Posuvnik), hrdina2.y + (HHrdina2.Height / Posuvnik), HHrdina2.Width - (HHrdina2.Width / Posuvnik), HHrdina2.Height - (HHrdina2.Height / Posuvnik));

            for (int h = 0; h < Alieni.Length; h++)
            {
                int pomocna = ran0.Next(-Rozptyl, Rozptyl);

                if (pomocna != 0)
                {

                    int kudy = ran0.Next(-200, 200);

                    if ((NaHrace1 && !Prohra) || !DvaHraci || Prohra2)
                    {
                        if (((Alieni[h].X + pomocna) < (hrdina.x + ((HHrdina.Width / 2) - (HHrdina.Width % 2)))) && (Alieni[h].Y < (hrdina.y + ((HHrdina.Height / 2) - (HHrdina.Height % 2)))))
                            Alieni[h].Pohyb(10);

                        else if (((Alieni[h].X + pomocna) > (hrdina.x + (HHrdina.Width / 2) - (HHrdina.Width % 2))) && (Alieni[h].Y < (hrdina.y + ((HHrdina.Height / 2) - (HHrdina.Height % 2)))))
                            Alieni[h].Pohyb(6);

                        else if ((Alieni[h].X < (hrdina.x + ((HHrdina.Width / 2) - (HHrdina.Width % 2)))) && ((Alieni[h].Y + pomocna) > (hrdina.y + ((HHrdina.Height / 2) - (HHrdina.Height % 2)))))
                            Alieni[h].Pohyb(14);

                        else if ((Alieni[h].X > (hrdina.x + ((HHrdina.Width / 2) - (HHrdina.Width % 2)))) && ((Alieni[h].Y + pomocna) > (hrdina.y + ((HHrdina.Height / 2) - (HHrdina.Height % 2)))))
                            Alieni[h].Pohyb(2);

                        else if (((Alieni[h].X + pomocna) < (hrdina.x + ((HHrdina.Width / 2) - (HHrdina.Width % 2)))) && (Alieni[h].Y == (hrdina.y + ((HHrdina.Height / 2) - (HHrdina.Height % 2)))))
                            Alieni[h].Pohyb(12);

                        else if ((Alieni[h].X == (hrdina.x + (HHrdina.Width / 2) - (HHrdina.Width % 2) + pomocna)) && (Alieni[h].Y < (hrdina.y + ((HHrdina.Height / 2) - (HHrdina.Height % 2)))))
                            Alieni[h].Pohyb(8);

                        else if (((Alieni[h].X + pomocna) > (hrdina.x + ((HHrdina.Width / 2) - (HHrdina.Width % 2)))) && (Alieni[h].Y == (hrdina.y + ((HHrdina.Height / 2) - (HHrdina.Height % 2)))))
                            Alieni[h].Pohyb(4);

                        else if ((Alieni[h].X == (hrdina.x + ((HHrdina.Width / 2) - (HHrdina.Width % 2)))) && (Alieni[h].Y > ((hrdina.y + ((HHrdina.Height / 2) - (HHrdina.Height % 2))) + pomocna)))
                            Alieni[h].Pohyb(0);

                    }
                    else if ((!NaHrace1 && !Prohra2) || Prohra)
                    {
                        if (((Alieni[h].X + pomocna) < (hrdina2.x + ((HHrdina.Width / 2) - (HHrdina.Width % 2)))) && (Alieni[h].Y < (hrdina2.y + ((HHrdina.Height / 2) - (HHrdina.Height % 2)))))
                            Alieni[h].Pohyb(10);

                        else if (((Alieni[h].X + pomocna) > (hrdina2.x + (HHrdina.Width / 2) - (HHrdina.Width % 2))) && (Alieni[h].Y < (hrdina2.y + ((HHrdina.Height / 2) - (HHrdina.Height % 2)))))
                            Alieni[h].Pohyb(6);

                        else if ((Alieni[h].X < (hrdina2.x + ((HHrdina.Width / 2) - (HHrdina.Width % 2)))) && ((Alieni[h].Y + pomocna) > (hrdina2.y + ((HHrdina.Height / 2) - (HHrdina.Height % 2)))))
                            Alieni[h].Pohyb(14);

                        else if ((Alieni[h].X > (hrdina2.x + ((HHrdina.Width / 2) - (HHrdina.Width % 2)))) && ((Alieni[h].Y + pomocna) > (hrdina2.y + ((HHrdina.Height / 2) - (HHrdina.Height % 2)))))
                            Alieni[h].Pohyb(2);

                        else if (((Alieni[h].X + pomocna) < (hrdina2.x + ((HHrdina.Width / 2) - (HHrdina.Width % 2)))) && (Alieni[h].Y == (hrdina2.y + ((HHrdina.Height / 2) - (HHrdina.Height % 2)))))
                            Alieni[h].Pohyb(12);

                        else if ((Alieni[h].X == (hrdina2.x + (HHrdina.Width / 2) - (HHrdina.Width % 2) + pomocna)) && (Alieni[h].Y < (hrdina2.y + ((HHrdina.Height / 2) - (HHrdina.Height % 2)))))
                            Alieni[h].Pohyb(8);

                        else if (((Alieni[h].X + pomocna) > (hrdina2.x + ((HHrdina.Width / 2) - (HHrdina.Width % 2)))) && (Alieni[h].Y == (hrdina2.y + ((HHrdina.Height / 2) - (HHrdina.Height % 2)))))
                            Alieni[h].Pohyb(4);

                        else if ((Alieni[h].X == (hrdina2.x + ((HHrdina.Width / 2) - (HHrdina.Width % 2)))) && (Alieni[h].Y > ((hrdina2.y + ((HHrdina.Height / 2) - (HHrdina.Height % 2))) + pomocna)))
                            Alieni[h].Pohyb(0);

                    }
                }

                Cyklus++;

                Rectangle AlienRec = new Rectangle(Alieni[h].X, Alieni[h].Y, Alien.Width, Alien.Height);

                if (HrdinaRec.IntersectsWith(AlienRec))
                {
                    if (Zivota > 1)
                        Zivota--;
                    else
                    {
                        Zivota = 0;
                        Prohra = true;
                    }
                }

                if (DvaHraci)
                    if (HrdinaRec2.IntersectsWith(AlienRec))
                    {
                        if (Zivota2 > 1)
                            Zivota2--;
                        else
                        {
                            Zivota2 = 0;
                            Prohra2 = true;
                        }
                    }

                Rectangle StrelaRec = new Rectangle(S.X, S.Y, S.Delka, SirkaStrely);
                Rectangle StrelaRec2 = new Rectangle(S2.X, S2.Y, S2.Delka, SirkaStrely2);

                if (ZalohaVybuchRec.Contains(AlienRec))
                {
                    blokace = true;
                    Mrtvola = h;
                    Body++;

                    UmrtiAlienuBuffer.Play(0, Microsoft.DirectX.DirectSound.BufferPlayFlags.Default);
                }

                if (ZalohaVybuchRec2.Contains(AlienRec))
                {
                    blokace = true;
                    Mrtvola = h;
                    Body++;

                    UmrtiAlienuBuffer.Play(0, Microsoft.DirectX.DirectSound.BufferPlayFlags.Default);
                }

                if (ZalohaVybuchRec3.Contains(AlienRec))
                {
                    blokace = true;
                    Mrtvola = h;
                    Body2++;

                    UmrtiAlienuBuffer.Play(0, Microsoft.DirectX.DirectSound.BufferPlayFlags.Default);
                }

                if (ZalohaVybuchRec4.Contains(AlienRec))
                {
                    blokace = true;
                    Mrtvola = h;
                    Body2++;

                    UmrtiAlienuBuffer.Play(0, Microsoft.DirectX.DirectSound.BufferPlayFlags.Default);
                }

                if (NowStrela != 0)
                    if (AlienRec.Contains(StrelaRec))
                    {

                        Mrtvola = h;
                        Body++;

                        UmrtiAlienuBuffer.Play(0, Microsoft.DirectX.DirectSound.BufferPlayFlags.Default);

                        blokace = false;
                        umrti = true;
                        if (PocetVybuchu == 0)
                            ZmenaVybuchu = true;

                        PocetVybuchu++;

                        if (PocetVybuchu > 1)
                            ZmenaVybuchu2 = true;

                        this.Strileni.Stop();
                        NowStrela = 0;
                    }

                if (DvaHraci)
                    if (NowStrela2 != 0)
                        if (AlienRec.Contains(StrelaRec2))
                        {

                            Mrtvola = h;
                            Body2++;

                            UmrtiAlienuBuffer.Play(0, Microsoft.DirectX.DirectSound.BufferPlayFlags.Default);

                            blokace = false;
                            umrti2 = true;
                            if (PocetVybuchu2 == 0)
                                ZmenaVybuchu3 = true;

                            PocetVybuchu2++;

                            if (PocetVybuchu2 > 1)
                                ZmenaVybuchu4 = true;

                            this.Strileni2.Stop();
                            NowStrela2 = 0;
                        }

                Alieni[h].CKroku++;



                if ((Lekarna.X == (-50)) && (Lekarna.Y == (-50)))
                {
                    Lekarna.X = ran0.Next(0, (Enviroment.RozliseniX - Lekarnicka.Width));
                    Lekarna.Y = ran0.Next(0, (Enviroment.RozliseniY - Lekarnicka.Height));
                }

                Rectangle LekarnaRec = new Rectangle(Lekarna.X, Lekarna.Y, Lekarnicka.Width, Lekarnicka.Height);

                if (HrdinaRec.IntersectsWith(LekarnaRec) && (Zivota < MaxHeal))
                {

                    Zvuky = new Microsoft.DirectX.DirectSound.SecondaryBuffer(LekarnaZvuk, SoundDevice);
                    Zvuky.Play(0, Microsoft.DirectX.DirectSound.BufferPlayFlags.Default);

                    Lekarna.X = (-50);
                    Lekarna.Y = (-50);

                    if (!DvaHraci)
                        Zivota += 20;
                    else
                        Zivota += 10;
                }

                if (DvaHraci)
                    if (HrdinaRec2.IntersectsWith(LekarnaRec) && (Zivota2 < MaxHeal))
                    {

                        Zvuky = new Microsoft.DirectX.DirectSound.SecondaryBuffer(LekarnaZvuk, SoundDevice);
                        Zvuky.Play(0, Microsoft.DirectX.DirectSound.BufferPlayFlags.Default);

                        Lekarna.X = (-50);
                        Lekarna.Y = (-50);

                        if (!DvaHraci)
                            Zivota2 += 20;
                        else
                            Zivota2 += 10;
                    }


            }
        }

        private void Strileni_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {

            if (NowStrela == 0)
            {
                S.X = hrdina.x + ((HHrdina.Width / 2) - (HHrdina.Width % 2));
                S.Y = hrdina.y + ((HHrdina.Height / 2) - (HHrdina.Height % 2));
            }

            if (zbran == 1)
            {
                OrigSmer = hrdina.Smer;
                S.Delka = 5;
                SirkaStrely = 6;
            }
            else if (zbran == 2)
            {
                S.Delka = 10;
                SirkaStrely = 2;
            }

            double uhel = (Math.PI / 180) * (ZamerovacSmer + PosunStrely);

            S.SmerY = -(int)(S.Delka * Math.Sin(uhel));
            S.Y -= (int)(S.Delka * Math.Sin(uhel));
            S.SmerX = (int)(S.Delka * Math.Cos(uhel));
            S.X += (int)(S.Delka * Math.Cos(uhel));

            NowStrela++;


            if ((S.X < 0) || (S.Y < 0) || (S.X > Enviroment.RozliseniX) || (S.Y > Enviroment.RozliseniY))
            {
                this.Strileni.Stop();
                NowStrela = 0;
            }

        }

        private void Strileni2_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (NowStrela2 == 0)
            {
                S2.X = hrdina2.x + ((HHrdina2.Width / 2) - (HHrdina2.Width % 2));
                S2.Y = hrdina2.y + ((HHrdina2.Height / 2) - (HHrdina2.Height % 2));
            }

            if (zbran2 == 1)
            {
                OrigSmer2 = hrdina2.Smer;
                S2.Delka = 5;
                SirkaStrely2 = 6;
            }
            else if (zbran2 == 2)
            {
                S2.Delka = 10;
                SirkaStrely2 = 2;
            }

            double uhel = (Math.PI / 180) * (ZamerovacSmer2 + PosunStrely);

            S2.SmerY = -(int)(S2.Delka * Math.Sin(uhel));
            S2.Y -= (int)(S2.Delka * Math.Sin(uhel));
            S2.SmerX = (int)(S2.Delka * Math.Cos(uhel));
            S2.X += (int)(S2.Delka * Math.Cos(uhel));

            NowStrela2++;


            if ((S2.X < 0) || (S2.Y < 0) || (S2.X > Enviroment.RozliseniX) || (S2.Y > Enviroment.RozliseniY))
            {
                this.Strileni2.Stop();
                NowStrela2 = 0;
            }
        }

        private void Hlasitost(bool nahlas)
        {

            switch (nahlas)
            {
                case true:
                    if (hudba.Volume == 0)
                        hudba.Play();

                    if (hudba.Volume <= 9)
                        hudba.Volume += 1;
                    break;

                case false:
                    if (hudba.Volume >= 1)
                        hudba.Volume -= 1;

                    if (hudba.Volume == 0)
                        hudba.Pause();

                    break;
            }

        }
        private void Phobia0_Click(object sender, EventArgs e)
        {

            Point souradnice = PointToClient(System.Windows.Forms.Cursor.Position);

            if (Start.VratRec().Contains(souradnice))
                StartGame();

            else if (Exit.VratRec().Contains(souradnice))
                Close();

            else if (Jeden.VratRec().Contains(souradnice))
                OnePlayer();

            else if (Dva.VratRec().Contains(souradnice))
                TwoPlayers();

            else if (Zpet.VratRec().Contains(souradnice))
                PocetHracu = "Base";

            else if (Option.VratRec().Contains(souradnice))
                PocetHracu = "Option";

            else if (Zvuk.VratRec().Contains(souradnice))
                PocetHracu = "Zvuk";

            else if (Cancel.VratRec().Contains(souradnice))
                PocetHracu = "Option";

            else if (Ok.VratRec().Contains(souradnice))
                Uloz();

            else if (UpArrow.VratRec().Contains(souradnice))
                Hlasitost(true);

            else if (DownArrow.VratRec().Contains(souradnice))
                Hlasitost(false);

            else if (HighScore.VratRec().Contains(souradnice))
            {
                if (!DisplayHighScore)
                    DisplayHighScore = true;
                else
                    DisplayHighScore = false;
            }
        }

        private void Uloz()
        {

            PocetHracu = "Option";
        }

        private void Movie_Ending(object sender, EventArgs e)
        {
            Intro.Stop();
        }

        private void Phobia0_MouseMove(object sender, MouseEventArgs e)
        {
            Point souradnice = System.Windows.Forms.Cursor.Position;

            if (Start.VratRec().Contains(souradnice))
                Start = new Tlacitko("res\\enviroment\\startgame-a.bmp");
            else
                Start = new Tlacitko("res\\enviroment\\startgame.bmp");

            if (Exit.VratRec().Contains(souradnice))
                Exit = new Tlacitko("res\\enviroment\\exit-a.bmp");
            else
                Exit = new Tlacitko("res\\enviroment\\exit.bmp");

            if (HighScore.VratRec().Contains(souradnice))
                HighScore = new Tlacitko("res\\enviroment\\HighScore-a.bmp");
            else
                HighScore = new Tlacitko("res\\enviroment\\HighScore.bmp");

            if (Jeden.VratRec().Contains(souradnice))
                Jeden = new Tlacitko("res\\enviroment\\jeden-a.bmp");
            else
                Jeden = new Tlacitko("res\\enviroment\\jeden.bmp");

            if (Dva.VratRec().Contains(souradnice))
                Dva = new Tlacitko("res\\enviroment\\dva-a.bmp");
            else
                Dva = new Tlacitko("res\\enviroment\\dva.bmp");

            if (Zpet.VratRec().Contains(souradnice))
                Zpet = new Tlacitko("res\\enviroment\\zpet-a.bmp");
            else
                Zpet = new Tlacitko("res\\enviroment\\zpet.bmp");

            if (Option.VratRec().Contains(souradnice))
                Option = new Tlacitko("res\\enviroment\\Option-a.bmp");
            else
                Option = new Tlacitko("res\\enviroment\\Option.bmp");

            if (Credits.VratRec().Contains(souradnice))
                Credits = new Tlacitko("res\\enviroment\\Credits-a.bmp");
            else
                Credits = new Tlacitko("res\\enviroment\\Credits.bmp");

            if (Zvuk.VratRec().Contains(souradnice))
                Zvuk = new Tlacitko("res\\enviroment\\Zvuk-a.bmp");
            else
                Zvuk = new Tlacitko("res\\enviroment\\Zvuk.bmp");

            if (Ok.VratRec().Contains(souradnice))
                Ok = new Tlacitko("res\\enviroment\\Ok-a.bmp");
            else
                Ok = new Tlacitko("res\\enviroment\\Ok.bmp");

            if (Cancel.VratRec().Contains(souradnice))
                Cancel = new Tlacitko("res\\enviroment\\Cancel-a.bmp");
            else
                Cancel = new Tlacitko("res\\enviroment\\Cancel.bmp");

            if (UpArrow.VratRec().Contains(souradnice))
                UpArrow = new Tlacitko("res\\enviroment\\UpArrow-a.bmp");
            else
                UpArrow = new Tlacitko("res\\enviroment\\UpArrow.bmp");

            if (DownArrow.VratRec().Contains(souradnice))
                DownArrow = new Tlacitko("res\\enviroment\\DownArrow-a.bmp");
            else
                DownArrow = new Tlacitko("res\\enviroment\\DownArrow.bmp");
        }

        private void hudba_Ending(object sender, EventArgs e)
        {
            if (skladba <= pisnicky.Length)
                skladba++;
            else if (skladba > 0)
                skladba--;

            hudba = new Audio(pisnicky[skladba], true);
            hudba.Ending += new EventHandler(hudba_Ending);

        }
    }

    #region Pomocny tridy
    public class BaseAlien
    {
        private int x = 0;
        public int X
        {
            get
            {
                return x;
            }
        }

        private int y = 0;
        public int Y
        {
            get
            {
                return y;
            }
        }

        private int smer = 0;
        public int Smer
        {
            set
            {
                smer = value;

                if (smer < 0) smer = 15;
                if (smer > 15) smer = 0;
            }

            get
            {
                return smer;
            }

        }
        private int krok = 10;
        public int Krok
        {
            set
            {
                krok = value;
            }

            get
            {
                return krok;
            }
        }
        private int ckroku = 0;
        public int CKroku
        {
            set
            {
                ckroku = value;
                if (ckroku > 10)
                    ckroku = 0;
            }

            get
            {
                return ckroku;
            }

        }

        private static string error = "";
        public static string Error
        {
            get
            {
                return error;
            }

        }


        public BaseAlien(double nahoda)
        {

            VygenerujSouradnice(nahoda);

        }

        public void VygenerujSouradnice(double nahoda)
        {

            Random ran = new Random((int)nahoda);

            int cislo;

            if (ran.NextDouble() >= 0.5)
                cislo = 1;
            else
                cislo = 0;

            if (cislo == 1)
            {

                x = ran.Next(-600, Enviroment.RozliseniX + 600);
                do
                {
                    y = ran.Next(-2000, Enviroment.RozliseniY + 2000);
                } while ((y > (-800)) && (y < (Enviroment.RozliseniY + 800)));

            }

            else if (cislo == 0)
            {

                y = ran.Next(-600, Enviroment.RozliseniY + 600);
                do
                {
                    x = ran.Next(-2000, Enviroment.RozliseniX + 2000);
                } while ((x > (-800)) && (x < (Enviroment.RozliseniX + 800)));

            }

            ckroku = ran.Next(0, 10);

        }

        public void Pohyb(int smerpohybu)
        {

            smer = smerpohybu;

            int pomoc = Enviroment.rand.Next(-251, 751);
            Random rand55 = new Random((int)pomoc);
            krok = rand55.Next(1, 9);

            switch (smerpohybu)
            {

                case 0:
                    y -= krok;
                    break;

                case 1:
                    x -= (krok / 2);
                    y -= krok;
                    break;

                case 2:
                    x -= krok;
                    y -= krok;
                    break;

                case 3:
                    x -= krok;
                    y -= (krok / 2);
                    break;

                case 4:
                    x -= krok;
                    break;

                case 5:
                    x -= krok;
                    y += (krok / 2);
                    break;

                case 6:
                    x -= krok;
                    y += krok;
                    break;

                case 7:
                    x -= (krok / 2);
                    y += krok;
                    break;

                case 8:
                    y += krok;
                    break;

                case 9:
                    x += (krok / 2);
                    y += krok;
                    break;

                case 10:
                    x += krok;
                    y += krok;
                    break;

                case 11:
                    x += krok;
                    y += (krok / 2);
                    break;

                case 12:
                    x += krok;
                    break;

                case 13:
                    x += krok;
                    y -= (krok / 2);
                    break;

                case 14:
                    x += krok;
                    y -= krok;
                    break;

                case 15:
                    x += (krok / 2);
                    y -= krok;
                    break;

            }


        }


    }

    public class Tlacitko
    {
        private int x;
        private int y;
        private System.IntPtr dc;
        public System.IntPtr Dc
        {
            set
            {
                dc = value;
            }
        }

        private Image Obrazek;
        private System.Drawing.Imaging.ImageAttributes ImgAttrib4 = new System.Drawing.Imaging.ImageAttributes();
        private string id;
        public string Id
        {
            set
            {
                id = value;
            }

            get
            {
                return id;
            }

        }

        private const int posuvnik = 40;

        public Tlacitko(string cesta)
        {
            ImgAttrib4.SetColorKey(Color.FromArgb(0, 163, 0), Color.FromArgb(83, 255, 76));
            Obrazek = new Bitmap(cesta);
        }

        public void Vykresli(int x, int y)
        {
            this.x = x;
            this.y = y;

            try
            {
                Graphics g = Graphics.FromHdc(dc);
                Rectangle ObrRec = new Rectangle(x - Obrazek.Width - posuvnik, y, Obrazek.Width / 3 * 2, Obrazek.Height / 3 * 2);
                g.DrawImage(Obrazek, ObrRec, 0, 0, Obrazek.Width, Obrazek.Height, GraphicsUnit.Pixel, ImgAttrib4);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        public Rectangle VratRec()
        {
            Rectangle Obdelnik = new Rectangle(x - Obrazek.Width - posuvnik, y, Obrazek.Width / 3 * 2, Obrazek.Height / 3 * 2);
            return Obdelnik;
        }
    }

    public class Textik
    {
        private System.IntPtr dc;
        private string text = String.Empty;
        private const string adresar = @"\res\enviroment\pismena\";
        private const string adresar2 = @"\res\enviroment\numbers\";
        private int x = 0;
        private int y = 0;
        private System.Drawing.Imaging.ImageAttributes ImgAttrib = new System.Drawing.Imaging.ImageAttributes();
        private Image[] Pismenka = null;

        public Textik(System.IntPtr Dc, string Text, int x, int y)
        {
            ImgAttrib.SetColorKey(Color.FromArgb(40, 125, 0), Color.FromArgb(174, 255, 120));

            text = Text;
            dc = Dc;

            this.x = x;
            this.y = y;

            if (text != String.Empty)
                Kresli();
        }

        public Textik(System.IntPtr Dc, int Cislo, int x, int y)
        {
            ImgAttrib.SetColorKey(Color.FromArgb(40, 125, 0), Color.FromArgb(174, 255, 120));

            text = Cislo.ToString();
            dc = Dc;

            this.x = x;
            this.y = y;

            if (text != String.Empty)
                Kresli2();
        }

        private void Kresli()
        {

            if (Pismenka == null)
            {
                string[] seznam = Directory.GetFiles(Directory.GetCurrentDirectory() + adresar);

                Pismenka = new Bitmap[seznam.Length];

                for (int i = 0; i < seznam.Length; i++)
                    Pismenka[i] = new Bitmap(seznam[i]);

            }
            Graphics g = Graphics.FromHdc(dc);

            for (int i = 0; i < text.Length; i++)
            {

                Rectangle FontRec = new Rectangle(x + (i * (Pismenka[0].Width / 3 * 2)), y, (Pismenka[0].Width / 3 * 2), (Pismenka[0].Height / 3 * 2));

                int pomocna = Preklad(text.ToUpper().Substring(i, 1));
                if (pomocna != 0)
                    g.DrawImage(Pismenka[pomocna - 1], FontRec, 0, 0, Pismenka[i].Width, Pismenka[i].Height, System.Drawing.GraphicsUnit.Pixel, ImgAttrib);
            }

        }

        private void Kresli2()
        {

            if (Pismenka == null)
            {
                string[] seznam = Directory.GetFiles(Directory.GetCurrentDirectory() + adresar2);

                Pismenka = new Bitmap[128];

                for (int i = 0; i < seznam.Length; i++)
                    Pismenka[i] = new Bitmap(seznam[i]);
            }
            Graphics g = Graphics.FromHdc(dc);


            for (int i = 0; i < text.Length; i++)
            {

                Rectangle FontRec = new Rectangle(x + (i * (Pismenka[0].Width / 3 * 2)), y, (Pismenka[0].Width / 3 * 2), (Pismenka[0].Height / 3 * 2));

                int pomocna = System.Convert.ToInt32(text.Substring(i, 1));
                g.DrawImage(Pismenka[pomocna], FontRec, 0, 0, Pismenka[i].Width, Pismenka[i].Height, System.Drawing.GraphicsUnit.Pixel, ImgAttrib);
            }


        }

        private int Preklad(string pismeno)
        {

            switch (pismeno)
            {

                case "A":
                    return 1;
                    break;

                case "B":
                    return 2;
                    break;

                case "C":
                    return 3;
                    break;

                case "D":
                    return 4;
                    break;

                case "E":
                    return 5;
                    break;

                case "F":
                    return 6;
                    break;

                case "G":
                    return 7;
                    break;

                case "H":
                    return 8;
                    break;

                case "I":
                    return 9;
                    break;

                case "J":
                    return 10;
                    break;

                case "K":
                    return 11;
                    break;

                case "L":
                    return 12;
                    break;

                case "M":
                    return 13;
                    break;

                case "N":
                    return 14;
                    break;

                case "O":
                    return 15;
                    break;

                case "P":
                    return 16;
                    break;

                case "Q":
                    return 17;
                    break;

                case "R":
                    return 18;
                    break;

                case "S":
                    return 19;
                    break;

                case "T":
                    return 20;
                    break;

                case "U":
                    return 21;
                    break;

                case "V":
                    return 22;
                    break;

                case "W":
                    return 23;
                    break;

                case "X":
                    return 24;
                    break;

                case "Y":
                    return 25;
                    break;

                case "Z":
                    return 26;
                    break;
            }

            return 0;

        }

    }
    #endregion Pomocny tridy
}
