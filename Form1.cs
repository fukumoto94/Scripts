using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace WindowsBar
{
    public partial class Form1 : Form
    {
        //Mouse position
        private int xPos;
        private int yPos;

        private Rectangle[] buttonOriginalRect;
        private Size formOriginalSize;
        private Button[] bt;
        private Random rand;
        private string[] imgTypes = new string[5];
        private int qdtButton;

        public Form1()
        { 
            InitializeComponent();
        }

        //create Button and add properties
        private void CreateBt(int qtd)
        {
            for (int i = 0; i < qtd; i++)
            {
                bt[i] = new Button();
                bt[i].AllowDrop = true;
                bt[i].FlatStyle = FlatStyle.Flat;
                bt[i].BackgroundImageLayout = ImageLayout.Stretch;
                bt[i].Size = new Size(65,65);
                bt[i].Name = "";
                bt[i].Location = new Point(rand.Next(10, (this.Width - (bt[i].Size.Width +10))), rand.Next(10, (this.Height - (bt[i].Size.Height +10))));
                buttonOriginalRect[i] = new Rectangle(bt[i].Location.X, bt[i].Location.Y, bt[i].Width, bt[i].Height);
                AddEventBt(bt[i]);
                this.Controls.Add(bt[i]);
            }
        }
        //add event to button created
        private void AddEventBt(Button bt)
        {
            bt.Click     += btClick;
            bt.DragEnter += btDragEnter;
            bt.DragDrop  += btDragDrop;
            bt.MouseDown += btMouseDown;
            bt.MouseMove += btMouseMove;
            bt.MouseUp   += btMouseUp;
        }

        #region Drag Drop itens
        private void btDragEnter(object sender, DragEventArgs e)
        {
            if(e.Data.GetDataPresent(DataFormats.FileDrop, false) == true)
            {
                e.Effect = DragDropEffects.All;
            }
        }

        private void btDragDrop(object sender, DragEventArgs e)
        {
            Button bt = sender as Button;

            string[] droppedFiles = (string[])e.Data.GetData(DataFormats.FileDrop);

            foreach (string file in droppedFiles)
            {
                string fileName = getFileName(file);
                textBox1.Text = file;
                bt.FlatAppearance.BorderSize = 0;
                bt.Text = fileName;
                bt.Name = file;

                for (int i = 0; i < 5; i++)
                {
                    if(file.Contains(imgTypes[i]))
                    {
                       
                        bt.BackgroundImage = Image.FromFile(file);
                        break;
                    }
                    else if(i == 4)
                    {
                        if(file.Contains("."))
                        {
                            Icon IEIcon = Icon.ExtractAssociatedIcon(file);
                            Image im = IEIcon.ToBitmap();
                            bt.BackgroundImage = im;
                        }
                        else
                        {
                            bt.BackgroundImage = Image.FromFile(Directory.GetParent(@"..\..\..\").FullName + "\\folderIcon.png");

                        }

                    }
                }
               

            }
        }
        #endregion

        //Get filename
        private string getFileName(string path)
        {
            return Path.GetFileNameWithoutExtension(path);
        }

        #region Mouse events
        private void btClick(object sender, EventArgs e)
        {
            Button bt = sender as Button;
            if (bt.Name != "")
            {
                Process.Start(bt.Name);
            }
        }
    
        private void btMouseDown(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Right)
            {
                xPos = e.X;
                yPos = e.Y;
            }
        }

        private void btMouseMove(object sender, MouseEventArgs e)
        {
            Button b = sender as Button;
            if (b != null)
            {
                if (e.Button == MouseButtons.Right)
                {
                    b.Top += (e.Y - yPos);
                    b.Left += (e.X - xPos);
                }
            }
        }
        private void btMouseUp(object sender, MouseEventArgs e)
        {
            for(int i = 0; i < qdtButton; i++)
            {
                buttonOriginalRect[i] = new Rectangle(bt[i].Location.X, bt[i].Location.Y, buttonOriginalRect[i].Width, buttonOriginalRect[i].Height);
            }
        }
        #endregion

        //start initial properties
        private void Form1_Load(object sender, EventArgs e)
        {  
            //img types compatibles
            imgTypes[0] = ".jpg";
            imgTypes[1] = ".jpeg";
            imgTypes[2] = ".png";
            imgTypes[3] = ".bmp";
            imgTypes[4] = ".gif";

            formOriginalSize = this.Size;
        }

        #region Resize itens form 
        private void resizeChildrenControls()
        {
            for (int i = 0; i < qdtButton; i++)
            {
                resizeControl(buttonOriginalRect[i], bt[i]);

            }
        }
        private void resizeControl(Rectangle originalControlRect, Control control)
        {
            float xRatio = (float)(this.Width) / (float)(formOriginalSize.Width);
            float yRatio = (float)(this.Height) / (float)(formOriginalSize.Height);

            int newX = (int)(originalControlRect.X * xRatio);
            int newY = (int)(originalControlRect.Y * yRatio);
            int newWidth = (int)(originalControlRect.Width * xRatio);
            int newHeight = (int)(originalControlRect.Height * yRatio);

            control.Location = new Point(newX, newY);
            control.Size = new Size(newWidth, newHeight);
        }
      

        private void Form1_Resize(object sender, EventArgs e)
        {

            resizeChildrenControls();
        }
        #endregion

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                qdtButton = Int32.Parse(textBox1.Text);
            }

            bt = new Button[qdtButton];
            rand = new Random(qdtButton);
            buttonOriginalRect = new Rectangle[qdtButton];
            CreateBt(qdtButton);

        }
    }
}
