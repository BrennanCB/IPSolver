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

namespace IPSolver
{
    public partial class frmGraph : Form
    {
        double[,] coords;

        public frmGraph(double[,] coords)
        {
            InitializeComponent();

            this.coords = coords;
        }

        private void frmGraph_Load(object sender, EventArgs e)
        {
            chart1.Series.Clear();

            //Adds the points to the graph
            for (int i = 0; i < coords.GetLength(0); i++)
            {
                chart1.Series.Add("Constraint " + (i + 1));

                chart1.Series[i].Points.AddXY(coords[i, 0], 0);

                chart1.Series[i].Points.AddXY(0, coords[i, 1]);

                //Changes the type to a line graph
                chart1.Series[i].ChartType = SeriesChartType.Line;
            }
        }
    }
}
