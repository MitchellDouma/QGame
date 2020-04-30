/*
 * Program ID: QGame
 * 
 * Purpose: To design and save a QGame level and to play the level by
 * moving the boxes to the corresponding doors 
 * 
 * Revision History:
 * written by Mitchell Douma on November 2018
 * 
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MDoumaAssignment1
{
    public partial class PlayForm : Form
    {
        public PlayForm()
        {
            InitializeComponent();
        }
        int boxNumber;
        string boxSelected;
        int numberOfMoves;

        //Deletes the previous grid by removing all of the pictureboxes
        public void DeleteGrid()
        {
            foreach (Control control in Controls)
            {
                foreach (Control control2 in Controls)
                {
                    if (control2 is PictureBox)
                    {

                        Controls.Remove(control2);
                        control2.Dispose();
                    }
                }
            }
            numberOfMoves = 0;
        }

        //looks at the position of the selected box and predicts its
        //next location by either adding or subtracting 1 from either 
        //its row position or column position depending on whether its
        //going up,down, left, or right, but only if the next location
        //either empty or a door of the same colour
        //if there are no more boxes then it will display a message
        //and delete the grid
        public void Movement(string futurePosition, string boxPosition, string boxColour)
        {
            foreach (Control control in Controls)
            {
                if (control.Name.StartsWith(futurePosition) && control.Name.Contains("Empty"))
                {
                    ((PictureBox)Controls[boxSelected]).Image = null;
                    ((PictureBox)Controls[boxSelected]).Name = boxPosition + "Empty";
                    boxSelected = futurePosition + boxSelected.Remove(0, 6);
                    (control as PictureBox).Name = boxSelected;
                    switch (boxColour)
                    {
                        case "Red---":
                            (control as PictureBox).Image = Properties.Resources.redBox;
                            break;
                        case "Blue--":
                            (control as PictureBox).Image = Properties.Resources.blueBox;
                            break;
                        case "Yellow":
                            (control as PictureBox).Image = Properties.Resources.yellowBox;
                            break;
                        case "Green-":
                            (control as PictureBox).Image = Properties.Resources.greenBox;
                            break;
                    }
                    numberOfMoves++;
                    txtNumberOfMoves.Text = numberOfMoves.ToString();
                }
                //when a box goes to a door it checks if it is the same colour and if so,
                //removes the box
                //if all boxes are removed then display win message
                else if (control.Name.StartsWith(futurePosition) && control.Name.Contains("Door"))
                {
                    string doorColour = control.Name.Substring(6, 6);
                    if (boxColour == doorColour)
                    {

                        ((PictureBox)Controls[boxSelected]).Image = null;
                        ((PictureBox)Controls[boxSelected]).Name = boxPosition + "Empty";
                        boxSelected = "";
                        boxNumber -= 1;
                        if (boxNumber == 0)
                        {
                            MessageBox.Show("You're Winner!");
                            DeleteGrid();
                        }
                    }
                }

            }
        }
  
        // sets the first two lines in the file as the grid size
        // then reads the picture code and assigns a picture to the 
        // tile that coresponds to the code
        private void loadFile(string filename)
        {
            StreamReader reader = new StreamReader(filename);
            int rowNumber = int.Parse(reader.ReadLine());
            int columnNumber = int.Parse(reader.ReadLine());           
            const int ORIGINPOINT = 148;
            const int TILESIZE = 70;
            int originPointX = ORIGINPOINT;
            int originPointY = 55;
            int emptyNumber = 0;
            int wallNumber = 0;
            int doorNumber = 0;
            boxNumber = 0;
            try
            {
                DeleteGrid();
                for (int i = 0; i < rowNumber; i++)
                {
                    for (int j = 0; j < columnNumber; j++)
                    {
                        PictureBox grid = new PictureBox();
                        grid.Name = "grid" + reader.ReadLine() + reader.ReadLine();
                        grid.Size = new Size(TILESIZE, TILESIZE);
                        grid.BackColor = Color.Gray;
                        grid.BorderStyle = BorderStyle.Fixed3D;
                        grid.Location = new Point(originPointX, originPointY);
                        grid.Click += new EventHandler(selectBox_Click);
                        switch (reader.ReadLine())
                        {

                            case "0":
                                grid.Image = null;
                                emptyNumber++;
                                grid.Name += "Empty" + emptyNumber;
                                break;
                            case "1":
                                grid.Image = Properties.Resources.wall;
                                wallNumber++;
                                grid.Name += "Wall" + wallNumber;
                                break;
                            case "2":
                                grid.Image = Properties.Resources.redDoor;
                                doorNumber++;
                                grid.Name += "Red---Door" + doorNumber;
                                break;
                            case "3":
                                grid.Image = Properties.Resources.blueDoor;
                                doorNumber++;
                                grid.Name += "Blue--Door" + doorNumber;
                                break;
                            case "4":
                                grid.Image = Properties.Resources.yellowDoor;
                                doorNumber++;
                                grid.Name += "YellowDoor" + doorNumber;
                                break;
                            case "5":
                                grid.Image = Properties.Resources.greenDoor;
                                doorNumber++;
                                grid.Name += "Green-Door" + doorNumber;
                                break;
                            case "6":
                                grid.Image = Properties.Resources.redBox;
                                boxNumber++;
                                grid.Name += "Red---Box" + boxNumber;                              
                                break;
                            case "7":
                                grid.Image = Properties.Resources.blueBox;
                                boxNumber++;
                                grid.Name += "Blue--Box" + boxNumber;
                                break;
                            case "8":
                                grid.Image = Properties.Resources.yellowBox;
                                boxNumber++;
                                grid.Name += "YellowBox" + boxNumber;
                                break;
                            case "9":
                                grid.Image = Properties.Resources.greenBox;
                                boxNumber++;
                                grid.Name += "Green-Box" + boxNumber;
                                break;
                        }
                        Controls.Add(grid);
                        originPointX += TILESIZE;
                    }
                    originPointX = ORIGINPOINT;
                    originPointY += TILESIZE;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Problem creating grid: {ex}");
            }
            finally
            {              
                reader.Close();
            }

        }


        //Stores the name of the selected box, if the user actually
        //clicked on a box
        private void selectBox_Click(object sender, EventArgs e)
        {
            if ((sender as PictureBox).Name.Contains("Box"))
            {
                boxSelected = ((sender as PictureBox).Name);
            }          
        }

        //Closes the form
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
        //opens the open dialog box
        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = loadDialog.ShowDialog();
            switch (result)
            {
                case DialogResult.Abort:
                    break;
                case DialogResult.Cancel:
                    break;
                case DialogResult.Ignore:
                    break;
                case DialogResult.No:
                    break;
                case DialogResult.None:
                    break;
                case DialogResult.OK:
                    try
                    {
                        string filename = loadDialog.FileName;
                        loadFile(filename);
                    }
                    catch (Exception)
                    {

                        MessageBox.Show("Error in file load");
                    }
                    break;
                case DialogResult.Retry:
                    break;
                case DialogResult.Yes:
                    break;
                default:
                    break;
            }
        }
 
        private void btnUp_Click(object sender, EventArgs e)
        {
            if (boxSelected != null && boxSelected != "")
            {
                string boxPosition = boxSelected.Remove(6);
                grid
                int movement = int.Parse(boxPosition[4].ToString()) - 1;
                string futurePosition = "grid" + movement.ToString() + boxPosition[5];
                string boxColour = boxSelected.Substring(6, 6);
                Movement(futurePosition, boxPosition, boxColour);
            }
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            if (boxSelected != null && boxSelected != "")
            {
                string boxPosition = boxSelected.Remove(6);
                int movement = int.Parse(boxPosition[4].ToString()) + 1;
                string futurePosition = "grid" + movement.ToString() + boxPosition[5];
                string boxColour = boxSelected.Substring(6, 6);
                Movement(futurePosition, boxPosition, boxColour);
            }
        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            if (boxSelected != null && boxSelected != "")
            {
                string boxPosition = boxSelected.Remove(6);
                int movement = int.Parse(boxPosition[5].ToString()) - 1;
                string futurePosition = "grid" + boxPosition[4] + movement.ToString();
                string boxColour = boxSelected.Substring(6, 6);
                Movement(futurePosition, boxPosition, boxColour);
            }
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            if (boxSelected != null && boxSelected != "")
            {
                string boxPosition = boxSelected.Remove(6);
                int movement = int.Parse(boxPosition[5].ToString()) + 1;
                string futurePosition = "grid" + boxPosition[4] + movement.ToString();
                string boxColour = boxSelected.Substring(6, 6);
                Movement(futurePosition, boxPosition, boxColour);
            }
        }
    }
}
