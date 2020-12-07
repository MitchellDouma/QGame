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
        int[] boxPosition = new int[2];
        int numberOfMoves;
        int columnNumber;
        int rowNumber;
        string[][] gameGrid;
        string boxColour;
        Direction direction;

        enum Direction
        {
            UP,
            DOWN,
            LEFT,
            RIGHT,
            NONE
        }


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
            lblSelectedBox.Text = "Selected: None";
        }

        //looks at the position of the selected box and predicts its
        //next location by either adding or subtracting 1 from either 
        //its row position or column position depending on whether its
        //going up,down, left, or right, but only if the next location
        //either empty or a door of the same colour
        //if there are no more boxes then it will display a message
        //and delete the grid
        public void Movement(int[] futurePosition, string boxColour)
        {
            foreach (Control control in Controls)
            {
                if (control.Name.StartsWith("grid" + futurePosition[0].ToString() + futurePosition[1].ToString()) && control.Name.Contains("Empty"))
                {
                    ((PictureBox)Controls[boxSelected]).Image = null;
                    ((PictureBox)Controls[boxSelected]).Name = boxPosition[0].ToString() + boxPosition[1].ToString() + "Empty";
                    boxPosition[0] = futurePosition[0];
                    boxPosition[1] = futurePosition[1];
                    (control as PictureBox).Name = boxSelected;
                    switch (boxColour)
                    {
                        case "Red":
                            (control as PictureBox).Image = Properties.Resources.redBox;
                            break;
                        case "Blue":
                            (control as PictureBox).Image = Properties.Resources.blueBox;
                            break;
                        case "Yellow":
                            (control as PictureBox).Image = Properties.Resources.yellowBox;
                            break;
                        case "Green":
                            (control as PictureBox).Image = Properties.Resources.greenBox;
                            break;
                    }
                    numberOfMoves++;
                    txtNumberOfMoves.Text = numberOfMoves.ToString() + boxPosition[0].ToString() + boxPosition[1].ToString();
                }
                //when a box goes to a door it checks if it is the same colour and if so,
                //removes the box
                //if all boxes are removed then display win message
                else if (control.Name.StartsWith("grid" + futurePosition[0].ToString() + futurePosition[1].ToString()) && control.Name.Contains("Door"))
                {
                    string doorColour = control.Name.Substring(6, 6);
                    doorColour.Replace("-", "");
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
            direction = Direction.NONE;
        }
  
        // sets the first two lines in the file as the grid size
        // then reads the picture code and assigns a picture to the 
        // tile that coresponds to the code
        private void loadFile(string filename)
        {
            StreamReader reader = new StreamReader(filename);
            rowNumber = int.Parse(reader.ReadLine());
            columnNumber = int.Parse(reader.ReadLine());
            gameGrid = new string[rowNumber][];
            const int ORIGINPOINT = 148;
            const int TILESIZE = 70;
            int originPointX = ORIGINPOINT;
            int originPointY = 55;
            int emptyNumber = 0;
            int wallNumber = 0;
            int doorNumber = 0;
            boxNumber = 0;
            boxColour = "";
            boxSelected = "";
            boxPosition[0] = -1;
            boxPosition[1] = -1;
            try
            {
                DeleteGrid();
                for (int i = 0; i < rowNumber; i++)
                {
                    gameGrid[i] = new string[columnNumber];
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
                                gameGrid[i][j] = "Empty";
                                break;
                            case "1":
                                grid.Image = Properties.Resources.wall;
                                wallNumber++;
                                grid.Name += "Wall" + wallNumber;
                                gameGrid[i][j] = "Wall";
                                break;
                            case "2":
                                grid.Image = Properties.Resources.redDoor;
                                doorNumber++;
                                grid.Name += "Red---Door" + doorNumber;
                                gameGrid[i][j] = "Red---Door";
                                break;
                            case "3":
                                grid.Image = Properties.Resources.blueDoor;
                                doorNumber++;
                                grid.Name += "Blue--Door" + doorNumber;
                                gameGrid[i][j] = "Blue--Door";
                                break;
                            case "4":
                                grid.Image = Properties.Resources.yellowDoor;
                                doorNumber++;
                                grid.Name += "YellowDoor" + doorNumber;
                                gameGrid[i][j] = "YellowDoor";
                                break;
                            case "5":
                                grid.Image = Properties.Resources.greenDoor;
                                doorNumber++;
                                grid.Name += "Green-Door" + doorNumber;
                                gameGrid[i][j] = "Green-Door";
                                break;
                            case "6":
                                grid.Image = Properties.Resources.redBox;
                                boxNumber++;
                                grid.Name += "Red---Box" + boxNumber;
                                gameGrid[i][j] = "Red---Box";
                                break;
                            case "7":
                                grid.Image = Properties.Resources.blueBox;
                                boxNumber++;
                                grid.Name += "Blue--Box" + boxNumber;
                                gameGrid[i][j] = "Blue--Box";
                                break;
                            case "8":
                                grid.Image = Properties.Resources.yellowBox;
                                boxNumber++;
                                grid.Name += "YellowBox" + boxNumber;
                                gameGrid[i][j] = "YellowBox";
                                break;
                            case "9":
                                grid.Image = Properties.Resources.greenBox;
                                boxNumber++;
                                grid.Name += "Green-Box" + boxNumber;
                                gameGrid[i][j] = "Green-Box";
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
                boxPosition[0] = int.Parse(boxSelected.Substring(4, 1));
                boxPosition[1] = int.Parse(boxSelected.Substring(5, 1));
                boxColour = boxSelected.Substring(6, 6);
                boxColour.Replace("-", "");

                lblSelectedBox.Text = "Selected: " + boxColour + " box";
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
                direction = Direction.UP;
                int[] futurePosition = FuturePosition();
                Movement(futurePosition, boxColour);
            }
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            if (boxSelected != null && boxSelected != "")
            {
                direction = Direction.DOWN;
                int[] futurePosition = FuturePosition();
                Movement(futurePosition, boxColour);
            }
        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            if (boxSelected != null && boxSelected != "")
            {
                direction = Direction.LEFT;
                int[] futurePosition = FuturePosition();
                Movement(futurePosition, boxColour);
            }
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            if (boxSelected != null && boxSelected != "")
            {
                direction = Direction.RIGHT;
                int[] futurePosition = FuturePosition();
                Movement(futurePosition, boxColour);
            }
        }

        //looks at selected box position and desired direction and 
        //returns grid position of the space beside the wall that it hits or the door
        private int[] FuturePosition()
        {
            int[] futurePosition = new int[2];
            int newPosition = 0;

            if(direction == Direction.DOWN || direction == Direction.UP)
            {
                
                for (int i = boxPosition[0]; i < rowNumber; i++)
                {
                    futurePosition[0] = newPosition;
                    futurePosition[1] = boxPosition[1];
                    //make sure we dont go out of bounds
                    if ((i + 1 > rowNumber - 1 && direction == Direction.DOWN) || (i - 1 < 0 && direction == Direction.UP))
                    {
                        break;
                    }
                    
                    if (direction == Direction.UP)
                    {
                        if (gameGrid[i - 1][boxPosition[1]] != "Empty")
                        {
                            break;
                        }
                        newPosition = i - 1;
                    }
                    else if (direction == Direction.DOWN)
                    {                       
                        if (gameGrid[i + 1][boxPosition[1]] != "Empty")
                        {
                            break;
                        }
                        newPosition = i + 1;
                    }
                
                }
            }
            else
            {
                
                for (int i = boxPosition[1]; i < columnNumber; i++)
                {
                    futurePosition[0] = boxPosition[0];
                    futurePosition[1] = newPosition;
                    
                    //make sure we dont go out of bounds
                    if ((i + 1 > columnNumber - 1 && direction == Direction.RIGHT) || (i - 1 < 0 && direction == Direction.LEFT))
                    {
                        break;
                    }                
                    if (direction == Direction.LEFT)
                    {                       
                        if (gameGrid[boxPosition[0]][i - 1] != "Empty")
                        {
                                break;
                        }
                        newPosition = i - 1;
                    }
                    else if (direction == Direction.RIGHT)
                    {                
                        if (gameGrid[boxPosition[0]][i + 1] != "Empty")
                        {
                                break;
                        }
                        newPosition = i + 1;
                    }
                    
                }
            }
                    
            return futurePosition;
        }
    }
}
