using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using ProcessImage;

namespace MI_Test_UI
{
    public partial class Form1 : Form
    {       
        List<Int32> colorList = new List<Int32>();
        Dictionary<int, int> ignoreColors = new Dictionary<int, int>();
        List<Int32> imageColors = new List<Int32>();
        List<Int32> newimageColors;
        string[] imageFiles;
        Int32 hvid = Color.White.ToArgb();
        Color[] ignores = new Color[80000];
        Color[] pixels;
        Color[] newImage;
        int l = 0;
        int g = 0;
        int h = 0;
        int filesLoaded = 0;
        int imageCounter = 0;
        ProcessImage.ImageProcessor imageprocessor = new ImageProcessor();
        public Form1()
        {
            InitializeComponent();
            load_ignore_values();
            load_image_files();
                
        }
        public void load_ignore_values()
        {
            string file = "SavedList.txt";
            using (StreamReader r = new StreamReader(file)){
                string line;
                while ((line = r.ReadLine()) != null){
                    ignoreColors.Add(Int32.Parse(line), Int32.Parse(line));
                    progressBar1.Increment(1);
                }
            }
            
            label5.Text = (ignoreColors.Count + " Colors Loaded.");
        }
        
        public void load_image_files()
        {
            g = 0;
            imageFiles = Directory.GetFiles(".\\images", "*.png");
            progressBar5.Increment(100);
            label6.Text = ("All Files Loaded.");
            foreach (string fil in imageFiles)
            {
                g++;
            }
            filesLoaded = g;
            label9.Text = "0 / " + filesLoaded;
        }

        public void load_image_from_file(string filename)
        {
            pixels = null;
            imageColors.Clear();
            progressBar2.Value = 0;
            System.Drawing.Image image = System.Drawing.Image.FromFile(filename);
            pictureBoxOrig.Image = image;
            pixels = imageprocessor.convertToPixels(image);
            foreach (Color farve in pixels){
                imageColors.Add(farve.ToArgb());
            }
            progressBar2.Increment(100);
            label8.Text =("File Loaded");
        }

        public void compare_images()
        {
            progressBar3.Value = 0;
            newimageColors = null;
            newimageColors = new List<Int32>();
            progressBar3.Value = 0;
            h = -1;
            foreach (Int32 farv in imageColors){
                h++;
                if (ignoreColors.ContainsKey(farv)){
                    newimageColors.Add(hvid);
                }                
                else{
                    newimageColors.Add(farv);
                }
                progressBar3.Increment(1);
            }
        }

        public void create_new_image()
        {
            progressBar4.Value = 0;
            newImage = null;
            newImage = new Color[80000];
            l = 0;
            foreach (Int32 color in newimageColors){
                newImage[l] = Color.FromArgb(color);
                l++;
                progressBar4.Increment(1);
            }
            l = 0;
            pictureBoxProcessed.Image = imageprocessor.convertToImage(newImage);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            load_image_from_file(imageFiles[imageCounter]);
            compare_images();
            create_new_image();
            imageCounter++;
            label9.Text = (imageCounter + " / " + filesLoaded);
        }
    }
}
