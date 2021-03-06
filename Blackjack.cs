using System;
using System.Drawing;
using System.Windows.Forms;

namespace BlackjackIDE {
    public class Blackjack {
        public static Bitmap Icon;
        public static void Main(string[] args) {
            BlackjackMainWindow window;
            Icon = ResizeBitmap(new Bitmap("icon.ico"), 24, 24);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(window = new BlackjackMainWindow());
        }
        public static Bitmap ResizeBitmap(Bitmap sourceBMP, int width, int height) {
            Bitmap result = new Bitmap(width, height);
            using(Graphics g = Graphics.FromImage(result)) {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                g.DrawImage(sourceBMP, 0, 0, width, height);
            }
            return result;
        }
    }
    public class BlackjackMainWindow : BlackjackWindowBase {
        public BlackjackMainWindow() : base(){
            Icon = new Icon("icon.ico");
            CenterToScreen();
        }
        protected override void Setup(){
            Name = "Blackjack";
        }
    }
    public abstract class BlackjackWindowBase : Form {
        public Color voidColor = Color.Black;
        public Color backgroundColor = Color.Maroon;
        public Color textColor = Color.WhiteSmoke;

        public BlackjackWindowBase() : base() {
            FormBorderStyle = FormBorderStyle.None;
            BackColor = voidColor;
            Setup();
            ResetLayout();
            Focus();
        }
        protected void ResetLayout() {
            SuspendLayout();
            NameBar nameBar = new NameBar();
            Controls.Add(nameBar);
            nameBar.Initialize(Name, Blackjack.Icon);
            ToolStrip toolbar = new ToolStrip(new ToolStripDropDownButton("File"));
            toolbar.Top = nameBar.Bottom;
            toolbar.Dock = DockStyle.Bottom;
            toolbar.BackColor = backgroundColor;
            toolbar.ForeColor = textColor;
            toolbar.GripStyle = ToolStripGripStyle.Hidden;
            toolbar.Renderer = new BlackjackToolStripRenderer();
            Controls.Add(toolbar);
            ResumeLayout(true);
        }
        protected virtual void Setup(){}
    }
    public class NameBar : Control {
        public Color backgroundColor = Color.Maroon;
        public Color textColor = Color.WhiteSmoke;
        TextBox xBox;
        TextBox maximize;
        TextBox minimize;
        public void Initialize(string name, Bitmap icon) {
            Margin = new Padding();
            Anchor = AnchorStyles.Top | AnchorStyles.Left;
            Name = name;
            BackColor = backgroundColor;
            Dock = DockStyle.Top;
            SuspendLayout();
            int leftPadding = 0;
            Controls.Clear();

            PictureBox iconBox = new PictureBox();
            iconBox.SizeMode = PictureBoxSizeMode.AutoSize;
            iconBox.Image = icon;
            Controls.Add(iconBox);
            leftPadding += iconBox.Width;

            TextBox nameBox = new TextBox();
            nameBox.Text = Name;
            nameBox.ReadOnly = true;
            nameBox.Font = new Font("Sylfaen", 11);
            nameBox.BackColor = backgroundColor;
            nameBox.ForeColor = textColor;
            nameBox.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            nameBox.GotFocus += (object sender, EventArgs e) => { Focus(); };
            nameBox.BorderStyle = BorderStyle.None;
            nameBox.Cursor = Cursor;
            nameBox.Size = TextRenderer.MeasureText(nameBox.Text, nameBox.Font);

            Height = icon.Height;
            nameBox.Location = new Point(leftPadding, Height-(int)(nameBox.Font.Height/2f+Height/2f));
            Controls.Add(nameBox);
            leftPadding += nameBox.Width;

            xBox = new TextBox();
            xBox.Text = "X";
            xBox.ReadOnly = true;
            xBox.Font = new Font("Sylfaen", 11);
            xBox.BackColor = backgroundColor;
            xBox.ForeColor = textColor;
            xBox.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            xBox.GotFocus += (object sender, EventArgs e) => { Focus(); };
            xBox.BorderStyle = BorderStyle.None;
            xBox.Cursor = Cursor;
            xBox.Size = TextRenderer.MeasureText(xBox.Text, xBox.Font);
            Controls.Add(xBox);

            maximize = new TextBox();
            maximize.Text = "□";
            maximize.ReadOnly = true;
            maximize.Font = new Font("Sylfaen", 11);
            maximize.BackColor = backgroundColor;
            maximize.ForeColor = textColor;
            maximize.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            maximize.GotFocus += (object sender, EventArgs e) => { Focus(); };
            maximize.BorderStyle = BorderStyle.None;
            maximize.Cursor = Cursor;
            maximize.Size = TextRenderer.MeasureText(maximize.Text, maximize.Font);
            Controls.Add(maximize);

            minimize = new TextBox();
            minimize.Text = "_";
            minimize.ReadOnly = true;
            minimize.Font = new Font("Sylfaen", 11);
            minimize.BackColor = backgroundColor;
            minimize.ForeColor = textColor;
            minimize.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            minimize.GotFocus += (object sender, EventArgs e) => { Focus(); };
            minimize.BorderStyle = BorderStyle.None;
            minimize.Cursor = Cursor;
            minimize.Size = TextRenderer.MeasureText(minimize.Text, minimize.Font);
            Controls.Add(minimize);

            MouseDown += StartDrag;
            nameBox.MouseDown += StartDrag;

            minimize.MouseDown += Minimize;
            maximize.MouseDown += Maximize;
            xBox.MouseDown += Close;
            Resize += (object sender, EventArgs e) => {MoveOnResize();};

            MoveOnResize();

            ResumeLayout(true);
        }
        void MoveOnResize() {
            int rightPadding = 0;
            rightPadding += xBox.Width;
            xBox.Location = new Point(Parent.Width-rightPadding, 0);
            rightPadding += maximize.Width;
            maximize.Location = new Point(Parent.Width-rightPadding, 0);
            rightPadding += minimize.Width;
            minimize.Location = new Point(Parent.Width-rightPadding, 0);
        }
        void Minimize(object sender, MouseEventArgs e){
            if (e.Button == MouseButtons.Left && Parent is Form form){
                form.WindowState = FormWindowState.Minimized;
            }
        }
        void Maximize(object sender, MouseEventArgs e){
            if (e.Button == MouseButtons.Left && Parent is Form form){
                form.WindowState^=FormWindowState.Maximized;
            }
        }
        void Close(object sender, MouseEventArgs e){
            if (e.Button == MouseButtons.Left && Parent is Form form){
                form.Close();
            }
        }
        void StartDrag(object sender, MouseEventArgs e){
            if (e.Button == MouseButtons.Left){
                External.ReleaseCapture();
                External.SendMessage(Parent.Handle, External.WM_NCLBUTTONDOWN, External.HT_CAPTION, 0);
            }
        }
    }
}
