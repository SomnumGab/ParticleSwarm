using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace ParticleSwarm
{
    public partial class Form1 : Form
    {
        public int iter;
        public List<List<Double>> history;
        public Form1()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 3;
            chart1.Series.Clear();
            timer2.Enabled = false;
            timer2.Interval = 200;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var decimals = comboBox1.SelectedIndex + 2;
            var accuracy = Math.Pow(10, (-1 * decimals));
            var a = Int32.Parse(textBox1.Text);
            var b = Int32.Parse(textBox2.Text);
            var N = Int32.Parse(textBox3.Text);
            var T = Int32.Parse(textBox4.Text);
            var c1 = Double.Parse(textBox5.Text);
            var c2 = Double.Parse(textBox6.Text);
            var c3 = Double.Parse(textBox7.Text);
            var R = Int32.Parse(textBox8.Text);

            Swarm swarm = new Swarm();
           var output = swarm.SimulateSwarm(swarm.InitializeSwarm(a, b, N, decimals), N, R, c1, c2, c3, T, decimals);

            var best = output.OrderByDescending(m => m.xeval).First();
            label12.Text = "Najlepsza cząsteczka to x:\n "+best.x+",\n feval(x): \n" + best.xeval;

            chart1.Series.Clear();

            chart1.Series.Add("Fmax");
            chart1.Series["Fmax"].ChartType = SeriesChartType.Line;
            for (int i = 0; i < swarm.results.Count; i++) chart1.Series["Fmax"].Points.AddY(swarm.results[i].fmax);

            chart1.Series.Add("Average");
            chart1.Series["Average"].ChartType = SeriesChartType.Line;
            for (int i = 0; i < swarm.results.Count; i++) chart1.Series["Average"].Points.AddY(swarm.results[i].favg);

            chart1.Series.Add("Fmin");
            chart1.Series["Fmin"].ChartType = SeriesChartType.Line;
            for (int i = 0; i < swarm.results.Count; i++) chart1.Series["Fmin"].Points.AddY(swarm.results[i].fmin);

            iter = 0;
            history = swarm.history;
            timer2.Enabled = true;

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            this.Controls.Add(pictureBox1);
        
            Bitmap flag = new Bitmap(pictureBox1.Size.Width, pictureBox1.Size.Height);
            Graphics flagGraphics = Graphics.FromImage(flag);
            flagGraphics.FillRectangle(Brushes.Black, 10, 200, pictureBox1.Size.Width-28, 1);
            Pen bluePen = new Pen(Color.Black, 2);
            flagGraphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

            flagGraphics.FillRectangle(Brushes.Black, 18, 190, 1, 20);
            flagGraphics.FillRectangle(Brushes.Black, 758, 190, 1, 20);
            flagGraphics.DrawString("-4", DefaultFont, Brushes.Black, new Point(10, 210));
            flagGraphics.DrawString("12", DefaultFont, Brushes.Black, new Point(750, 210));

            flagGraphics.FillRectangle(Brushes.Red, ((int)(10.9999 + 4) * 46)+4, 190, 1, 20);

            flagGraphics.FillRectangle(Brushes.Red, ((int)(4.9999 + 4) * 46) + 4, 190, 1, 20);

            flagGraphics.FillRectangle(Brushes.Red, ((int)(-1.9999 + 4) * 46) + 4, 190, 1, 20);

            for (int i = 0; i < history.Count; i++)
            {
                int x = (int)(history[i][iter] + 4) * 46;
                Rectangle rect = new Rectangle(x, i*3+10, 10, 10);
                flagGraphics.FillEllipse(Brushes.Blue, rect);
                flagGraphics.DrawEllipse(bluePen, rect);
            }

            pictureBox1.Image = flag;
            if (iter == history[0].Count - 1) iter = 0;
            else iter++;
            label13.Text = "Frame:" + iter;
        }
    }
}
