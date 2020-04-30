/*
 * Program ID: QGame
 * 
 * Purpose: To design and save a QGame level and to play the level by
 * moving the boxes to the corresponding doors 
 * 
 * Revision History:
 * written by Mitchell Douma on September 2018
 * 
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MDoumaAssignment1
{
    public partial class QGameControlPanel : Form
    {
        public QGameControlPanel()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnDesign_Click(object sender, EventArgs e)
        {
            DesignForm designForm = new DesignForm();
            designForm.Show();
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            PlayForm playForm = new PlayForm();
            playForm.Show();
        }
    }
}
