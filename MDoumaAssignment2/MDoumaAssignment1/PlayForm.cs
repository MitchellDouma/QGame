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

        private enum Direction
        {
            NONE,
            UP,
            DOWN,
            LEFT,
            RIGHT
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
            RemoveBox();
        }

        //removes selected box
        private void RemoveBox()
        {
            lblSelectedBox.Text = "Selected: None";
            boxPosition[0] = -1;
            boxPosition[1] = -1;
            boxSelected = "";
            boxColour = "";
            ToggleMovementButtons(false);
        }

        //based on the future position of the box
        //the sprite of the box of the current position will be removed 
        //and the sprite will be placed in the future position
        private void Movement(int futurePosition, Direction direction)
        {
            //find the spot where the box is currently 
            PictureBox currentGridPosition = (PictureBox)FindGridPosition(boxPosition[0], boxPosition[1]);
            //remove old sprite
            if (currentGridPosition != null)
            {
                //if box is on a door of opposing colour
                //bring the door sprite back
                if (gameGrid[boxPosition[0]][boxPosition[1]].Contains("Door"))
                {
                    BringBackDoor(currentGridPosition, boxPosition[0], boxPosition[1]);
                }
                else
                {
                    currentGridPosition.Image = null;
                    gameGrid[boxPosition[0]][boxPosition[1]] = "Empty";
                }
            }
            //assign position for the box to go to
            PictureBox futureGridPosition;
            int placementX;
            int placementY;
            if (direction == Direction.UP || direction == Direction.DOWN)
            {
                placementX = futurePosition;
                placementY = boxPosition[1];
            }
            else
            {
                placementX = boxPosition[0];
                placementY = futurePosition;

            }
            futureGridPosition = (PictureBox)FindGridPosition(placementX, placementY);
            //enter new sprite
            if (futureGridPosition != null)
            {
                //set new box position
                boxPosition[0] = placementX;
                boxPosition[1] = placementY;               
                //add the sprite
                MoveBox(futureGridPosition);               
            }
            WaitMovement(500);
            //run the functions again until a wall or box is hit
            switch (direction)
            {
                case Direction.UP:
                    FuturePositionUp();
                    break;
                case Direction.DOWN:
                    FuturePositionDown();
                    break;
                case Direction.LEFT:
                    FuturePositionLeft();
                    break;
                case Direction.RIGHT:
                    FuturePositionRight();
                    break;

            }
        }

        //check where the box lands
        private void StopMovement()
        {         
            //if the box stops on the door and they're the same colour
            //remove the box
            if (gameGrid[boxPosition[0]][boxPosition[1]].Contains("Door"))
            {
                string doorColour = gameGrid[boxPosition[0]][boxPosition[1]].Substring(0, 6).Replace("-", "");
                if (boxColour == doorColour)
                {                  
                    //find the spot where the box is currently 
                    PictureBox currentGridPosition = (PictureBox)FindGridPosition(boxPosition[0], boxPosition[1]);                   
                    BringBackDoor(currentGridPosition, boxPosition[0], boxPosition[1]);
                    boxNumber--;
                    RemoveBox();
                    //end game if all boxes are gone
                    if (boxNumber == 0)
                    {
                        MessageBox.Show("You Won!");
                        DeleteGrid();
                    }
                }
                else
                {
                    //if the box lands on a door that isn't that colour
                    //don't remove the door, but place it on top of the door
                    //so when it moves again the door will stay there
                    gameGrid[boxPosition[0]][boxPosition[1]] += boxSelected;
                }              
            }
            else
            {
                //and change game grid to box selected
                gameGrid[boxPosition[0]][boxPosition[1]] = boxSelected;
            }
            //add the moves          
            numberOfMoves++;
            txtNumberOfMoves.Text = numberOfMoves.ToString();
            ToggleMovementButtons(true);           
        }

        //grabs the picture box 
        private Control FindGridPosition(int positionX, int positionY)
        {
            foreach (Control control in Controls)
            {
                if (control.Name == "grid" + positionX.ToString() + positionY.ToString())
                {
                    return (control as PictureBox);
                }
            }
            return null;
        }
        //places the correct colour of box sprite in the new location
        private void MoveBox(PictureBox gridPosition)
        {
            switch (boxColour)
            {
                case "Red":
                    gridPosition.Image = Properties.Resources.redBox;
                    break;
                case "Blue":
                    gridPosition.Image = Properties.Resources.blueBox;
                    break;
                case "Yellow":
                    gridPosition.Image = Properties.Resources.yellowBox;
                    break;
                case "Green":
                    gridPosition.Image = Properties.Resources.greenBox;
                    break;
            }
        }
        //runs when a box leaves a door position
        private void BringBackDoor(PictureBox gridPosition, int row, int column)
        {
            string doorColour = gameGrid[row][column].Substring(0, 6).Replace("-", "");
            switch (doorColour)
            {
                case "Red":
                    gridPosition.Image = Properties.Resources.redDoor;
                    gameGrid[row][column] = "Red---Door";
                    break;
                case "Blue":
                    gridPosition.Image = Properties.Resources.blueDoor;
                    gameGrid[row][column] = "Blue--Door";
                    break;
                case "Yellow":
                    gridPosition.Image = Properties.Resources.yellowDoor;
                    gameGrid[row][column] = "YellowDoor";
                    break;
                case "Green":
                    gridPosition.Image = Properties.Resources.greenDoor;
                    gameGrid[row][column] = "Green-Door";
                    break;
            }
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
                                gameGrid[i][j] = "Empty";
                                break;
                            case "1":
                                grid.Image = Properties.Resources.wall;
                                wallNumber++;
                                gameGrid[i][j] = "Wall";
                                break;
                            case "2":
                                grid.Image = Properties.Resources.redDoor;
                                doorNumber++;
                                gameGrid[i][j] = "Red---Door";
                                break;
                            case "3":
                                grid.Image = Properties.Resources.blueDoor;
                                doorNumber++;
                                gameGrid[i][j] = "Blue--Door";
                                break;
                            case "4":
                                grid.Image = Properties.Resources.yellowDoor;
                                doorNumber++;
                                gameGrid[i][j] = "YellowDoor";
                                break;
                            case "5":
                                grid.Image = Properties.Resources.greenDoor;
                                doorNumber++;
                                gameGrid[i][j] = "Green-Door";
                                break;
                            case "6":
                                grid.Image = Properties.Resources.redBox;
                                boxNumber++;
                                gameGrid[i][j] = "Red---Box" + boxNumber;
                                break;
                            case "7":
                                grid.Image = Properties.Resources.blueBox;
                                boxNumber++;
                                gameGrid[i][j] = "Blue--Box" + boxNumber;
                                break;
                            case "8":
                                grid.Image = Properties.Resources.yellowBox;
                                boxNumber++;
                                gameGrid[i][j] = "YellowBox" + boxNumber;
                                break;
                            case "9":
                                grid.Image = Properties.Resources.greenBox;
                                boxNumber++;
                                gameGrid[i][j] = "Green-Box" + boxNumber;
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
            int[] tileSelected = new int[2];
            tileSelected[0] = int.Parse((sender as PictureBox).Name.Substring(4, 1));
            tileSelected[1] = int.Parse((sender as PictureBox).Name.Substring(5, 1));
            if (gameGrid[tileSelected[0]][tileSelected[1]].Contains("Box"))
            {
                boxSelected = gameGrid[tileSelected[0]][tileSelected[1]];
                boxPosition[0] = tileSelected[0];
                boxPosition[1] = tileSelected[1];
                boxColour = boxSelected.Substring(0, 6).Replace("-", "");

                lblSelectedBox.Text = "Selected: " + boxColour + " box";
                ToggleMovementButtons(true);
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
        //on movement, loop through movement method
        //until box hits something, then call stop movement,
        //buttons should be disabled while box is in motion
        private void btnUp_Click(object sender, EventArgs e)
        {
            if (boxSelected != "")
            {
                ToggleMovementButtons(false);
                FuturePositionUp();
            }
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            if (boxSelected != "")
            {
                ToggleMovementButtons(false);
                FuturePositionDown();
            }
        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            if (boxSelected != "")
            {
                ToggleMovementButtons(false);
                FuturePositionLeft();
            }
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            if (boxSelected != "")
            {
                ToggleMovementButtons(false);
                FuturePositionRight();
            }
        }

        //looks at selected box position and desired direction and 
        //returns grid position of the space beside the wall that it hits or the door
        private void FuturePositionDown()
        {
            int futurePosition;
            int i = boxPosition[0];

            //make sure we dont go out of bounds
            //stop if we hit a wall or other box
            if ((i + 1 < rowNumber) && (gameGrid[i + 1][boxPosition[1]] != "Wall" && !gameGrid[i + 1][boxPosition[1]].Contains("Box")))
            {
                futurePosition = i + 1;
                Movement(futurePosition, Direction.DOWN);
            }
            else
            {
                StopMovement();
            }
        }
        private void FuturePositionUp()
        {
            int futurePosition;
            int i = boxPosition[0];
            //make sure we dont go out of bounds
            if ((i - 1 >= 0) && (gameGrid[i - 1][boxPosition[1]] != "Wall" && !gameGrid[i - 1][boxPosition[1]].Contains("Box")))
            {
                futurePosition = i - 1;
                Movement(futurePosition, Direction.UP);
            }
            else
            {
                StopMovement();
            }
        }
        private void FuturePositionRight()
        {
            int futurePosition;
            int i = boxPosition[1];
            //make sure we dont go out of bounds
            if ((i + 1 < columnNumber) && (gameGrid[boxPosition[0]][i + 1] != "Wall" && !gameGrid[boxPosition[0]][i + 1].Contains("Box")))
            {
                futurePosition = i + 1;
                Movement(futurePosition, Direction.RIGHT);
            }
            else
            {
                StopMovement();
            }
        }
        private void FuturePositionLeft()
        {
            int futurePosition;
            int i = boxPosition[1];
            
            //make sure we dont go out of bounds or through walls
            if ((i - 1 >= 0) && (gameGrid[boxPosition[0]][i - 1] != "Wall" && !gameGrid[boxPosition[0]][i - 1].Contains("Box")))
            {
                futurePosition = i - 1;
                Movement(futurePosition, Direction.LEFT);
            }
            else
            {
                StopMovement();
            }
        }

        //disables or enables movement buttons
        private void ToggleMovementButtons(bool isEnabled)
        {
            btnDown.Enabled = isEnabled;
            btnLeft.Enabled = isEnabled;
            btnRight.Enabled = isEnabled;
            btnUp.Enabled = isEnabled;
        }

        //pauses to let animation play
        private void WaitMovement(int milliseconds)
        {
            //dont bother if there is no time given
            if (milliseconds == 0 || milliseconds < 0) return;

            //start wait timer"
            movementTimer.Interval = milliseconds;
            movementTimer.Enabled = true;
            movementTimer.Start();

            movementTimer.Tick += (s, e) =>
            {
                movementTimer.Enabled = false;
                movementTimer.Stop();
                // stop wait timer
            };

            while (movementTimer.Enabled)
            {
                Application.DoEvents();
            }
        }
    }
}
