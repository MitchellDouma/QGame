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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MDoumaAssignment1
{
    public partial class DesignForm : Form
    {
        public DesignForm()
        {
            InitializeComponent();
        }

        // stores the name of the tool that is selected
        ToolBox tool;
        // the length and width of the generated grid
        Int32 rowNumber;
        Int32 columnNumber;
        enum ToolBox
        {
            NONE,
            WALL,
            REDDOOR,
            BLUEDOOR,
            YELLOWDOOR,
            GREENDOOR,
            REDBOX,
            BLUEBOX,
            YELLOWBOX,
            GREENBOX
        }
        //Deletes the previous grid by removing all of the pictureboxes
        public void DeleteGrid()
        {
            foreach (Control control in Controls)
            {
                foreach (Control control2 in Controls)
                {
                    if (control2.Name.StartsWith("grid"))
                    {

                        Controls.Remove(control2);
                        control2.Dispose();
                    }
                }
            }

        }

        // checks to see if the data entered in the row and column text boxes are positive integers
        // if they aren't, it won't allow a grid to be generated
        public string Validate(string row, string column)
        {
            string errors = "";
            try
            {
                rowNumber = Int32.Parse(row);
                columnNumber = Int32.Parse(column);

                if (rowNumber < 0 || columnNumber < 0)
                {
                    errors += "Please enter a positive integer.\n";
                }
                else if (rowNumber > 13 || columnNumber > 25)
                {
                    errors += "Row and/or column numbers are too large.\n";
                }
            }
            catch (OverflowException)
            {
                errors += "Row and/or column numbers are too large.\n";
            }
            catch
            {
                errors += "Please enter a positive integer.\n";
            }
            return errors;
        }
        //writes the picturebox number and tool inside to a text file
        private void SaveLevel(string filename)
        {
            StreamWriter writer = new StreamWriter(filename);
            try
            {
                writer.WriteLine(rowNumber);
                writer.WriteLine(columnNumber);
                for (int i = 0; i < rowNumber; i++)
                {
                    for (int j = 0; j < columnNumber; j++)
                    {
                        writer.WriteLine(i);
                        writer.WriteLine(j);
                        writer.WriteLine(((PictureBox)Controls["grid" + i.ToString() + j.ToString()]).Tag);
                    }
                }
                MessageBox.Show("File saved successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Problem saving the file \n{ex}");
            }
            finally
            {
                writer.Close();
            }

        }

        //closes the design form
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
        // if the row and column inputs are valid, this will remove the existing grid,
        // and create a new grid of picture boxes
        private void btnGenerate_Click(object sender, EventArgs e)
            {
            if (Validate(txtRows.Text, txtColumns.Text) == "")
            {
                rowNumber = Int32.Parse(txtRows.Text);
                columnNumber = Int32.Parse(txtColumns.Text);
                Int32 originPointX = 148;
                Int32 originPointY = 55;

                DeleteGrid();
                // drawing the the grid with pictureboxes by changing the location of each picture box after each iteration
                // and giving each picturebox a unique name so the code knows which one the user is refering to
                for (int i = 0; i < rowNumber; i++)
                {
                    for (int j = 0; j < columnNumber; j++)
                    {
                            PictureBox grid = new PictureBox();
                            grid.Name = "grid" + i.ToString() + j.ToString();
                            grid.Size = new Size(70, 70);
                            grid.Tag = 0;
                            grid.BackColor = Color.Gray;
                            grid.BorderStyle = BorderStyle.Fixed3D;
                            grid.Click += new EventHandler(gridTile_Click);
                            grid.Location = new Point(originPointX, originPointY);
                            Controls.Add(grid);
                            originPointX += 70;                      
                    }
                    originPointX = 148;
                    originPointY += 70;
                }
            }
            else
            {
                MessageBox.Show(Validate(txtRows.Text, txtColumns.Text));
            }

        }
        // when one of the tools are selected, the tool variable will be filled with
        // the name of the selected tool and disregard the privious tool
        private void tool_Click(object sender, EventArgs e)
        {
            switch ((sender as Button).Text)
            {
                case "None":
                    tool = ToolBox.NONE;
                    break;
                case "Wall":
                    tool = ToolBox.WALL;
                    break;
                case "Red Door":
                    tool = ToolBox.REDDOOR;
                    break;
                case "Blue Door":
                    tool = ToolBox.BLUEDOOR;
                    break;
                case "Yellow Door":
                    tool = ToolBox.YELLOWDOOR;
                    break;
                case "Green Door":
                    tool = ToolBox.GREENDOOR;
                    break;
                case "Red Box":
                    tool = ToolBox.REDBOX;
                    break;
                case "Blue Box":
                    tool = ToolBox.BLUEBOX;
                    break;
                case "Yellow Box":
                    tool = ToolBox.YELLOWBOX;
                    break;
                case "Green Box":
                    tool = ToolBox.GREENBOX;
                    break;

            }

        }
        // when a grid tile is clicked with a tool selected, the picture of the
        // tool will be inserted into the selected picturebox
        // the picturebox will also be given a tag that says which tool is in it
        // so the save feature can tell what tool is in the picturebox
        private void gridTile_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < rowNumber; i++)
            {
                for (int j = 0; j < columnNumber; j++)
                {

                    if ((sender as PictureBox).Name == "grid" + i.ToString() + j.ToString())
                    {
                        switch (tool)
                        {

                            case ToolBox.NONE:
                                ((PictureBox)Controls["grid" + i.ToString() + j.ToString()]).Image = null;
                                ((PictureBox)Controls["grid" + i.ToString() + j.ToString()]).Tag = 0;
                                break;
                            case ToolBox.WALL:
                                ((PictureBox)Controls["grid" + i.ToString() + j.ToString()]).Image = Properties.Resources.wall;
                                ((PictureBox)Controls["grid" + i.ToString() + j.ToString()]).Tag = 1;
                                break;
                            case ToolBox.REDDOOR:
                                ((PictureBox)Controls["grid" + i.ToString() + j.ToString()]).Image = Properties.Resources.redDoor;
                                ((PictureBox)Controls["grid" + i.ToString() + j.ToString()]).Tag = 2;
                                break;
                            case ToolBox.BLUEDOOR:
                                ((PictureBox)Controls["grid" + i.ToString() + j.ToString()]).Image = Properties.Resources.blueDoor;
                                ((PictureBox)Controls["grid" + i.ToString() + j.ToString()]).Tag = 3;
                                break;
                            case ToolBox.YELLOWDOOR:
                                ((PictureBox)Controls["grid" + i.ToString() + j.ToString()]).Image = Properties.Resources.yellowDoor;
                                ((PictureBox)Controls["grid" + i.ToString() + j.ToString()]).Tag = 4;
                                break;
                            case ToolBox.GREENDOOR:
                                ((PictureBox)Controls["grid" + i.ToString() + j.ToString()]).Image = Properties.Resources.greenDoor;
                                ((PictureBox)Controls["grid" + i.ToString() + j.ToString()]).Tag = 5;
                                break;
                            case ToolBox.REDBOX:
                                ((PictureBox)Controls["grid" + i.ToString() + j.ToString()]).Image = Properties.Resources.redBox;
                                ((PictureBox)Controls["grid" + i.ToString() + j.ToString()]).Tag = 6;
                                break;
                            case ToolBox.BLUEBOX:
                                ((PictureBox)Controls["grid" + i.ToString() + j.ToString()]).Image = Properties.Resources.blueBox;
                                ((PictureBox)Controls["grid" + i.ToString() + j.ToString()]).Tag = 7;
                                break;
                            case ToolBox.YELLOWBOX:
                                ((PictureBox)Controls["grid" + i.ToString() + j.ToString()]).Image = Properties.Resources.yellowBox;
                                ((PictureBox)Controls["grid" + i.ToString() + j.ToString()]).Tag = 8;
                                break;
                            case ToolBox.GREENBOX:
                                ((PictureBox)Controls["grid" + i.ToString() + j.ToString()]).Image = Properties.Resources.greenBox;
                                ((PictureBox)Controls["grid" + i.ToString() + j.ToString()]).Tag = 9;
                                break;
                        }
                    }
                }
            }
        }

        // opens the save dialog box
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {

            DialogResult result = saveDialog.ShowDialog();
           
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
                        string filename = saveDialog.FileName;
                        SaveLevel(filename);
                    }
                    catch (Exception)
                    {

                        MessageBox.Show("Error in file save");
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
    }
}

