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

namespace TBoI_SaveGameParser {
    public partial class Form1 : Form {
        string ContentOfFile;
        string driveLetter;
        string programFilesPath = @"Program Files\Steam";
        string programFilesX86Path = @"Program Files (x86)\Steam";
        string gamePath = @"SteamApps\common\The Binding Of Isaac";
        string fileName = "serial.txt";
        string completePath;
        int counter;
        bool found;
        private delegate void updateUIDelegate();
        DriveInfo[] allDrives = DriveInfo.GetDrives();

        FileSystemWatcher watcher;
        public Form1 () {
            InitializeComponent();
            counter = 0;
            found = false;
            ContentOfFile = string.Empty;
            watcher = new FileSystemWatcher();
            watcher.NotifyFilter = NotifyFilters.LastWrite;
            watcher.Filter = "serial.txt";
            watcher.Changed += watcher_Changed;
            

            this.Icon = global::TBoI_SaveGameParser.Properties.Resources.The_Binding_of_Isaac;


            foreach ( var item in allDrives ) {
                driveLetter = item.Name;
                programFilesPath = programFilesPath.Insert( 0, driveLetter );
                programFilesX86Path = programFilesX86Path.Insert( 0, driveLetter );

                if ( !Directory.Exists( programFilesX86Path ) && !Directory.Exists( programFilesPath ) ) {

                    MessageBox.Show( "Locate the file manually", "Steam Not Found" );
                    return;
                }


                if ( Directory.Exists( programFilesX86Path + "\\" + gamePath ) ) {
                    watcher.Path = programFilesX86Path + "\\" + gamePath;

                    if ( !File.Exists( programFilesX86Path + "\\" + gamePath + "\\" + fileName ) ) {
                        MessageBox.Show( "Locate the file manually", "File not found" );

                    }

                    completePath = programFilesX86Path + "\\" + gamePath + "\\" + fileName;
                    found = true;
                    break;

                } else if ( Directory.Exists( programFilesPath + "\\" + gamePath ) ) {
                    watcher.Path = programFilesPath + "\\" + gamePath;

                    if ( !File.Exists( programFilesPath + "\\" + gamePath + "\\" + fileName ) ) {
                        MessageBox.Show( "Locate the file manually", "File not found" );

                    }

                    completePath = programFilesPath + "\\" + gamePath + "\\" + fileName;
                    found = true;
                    break;

                } else {
                    MessageBox.Show( "Locate the file manually", "Game Not Installed via Steam" );
                    return;

                }
            }

            if ( !found ) {
                foreach ( var item in allDrives ) {



                }

            }

            using ( StreamReader sr = new StreamReader( completePath ) ) {
                ContentOfFile = sr.ReadToEnd();
                sr.Close();

            }
            watcher.EnableRaisingEvents = true;

        }

        void watcher_Changed ( object sender, FileSystemEventArgs e ) {

            while ( true ) {
                try {
                    using ( StreamReader sr = new StreamReader( completePath ) ) {
                        ContentOfFile = sr.ReadToEnd();
                        sr.Close();

                    }
                    break;
                } catch ( IOException ) {

                    continue;
                } catch {

                    MessageBox.Show( "Something went wrong", "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
                    Environment.Exit( 1 );
                }
            }
            
            ParseContent();
            updateCounter();

        }

        private void updateCounter () {

            if ( lblCounter.InvokeRequired ) {
                lblCounter.Invoke( new updateUIDelegate( updateCounter ) );

            } else {

                lblCounter.Text = ( ++counter / 2 ).ToString();


            }


        }

        private void btnExit_Click ( object sender, EventArgs e ) {
            Environment.Exit( Environment.ExitCode );
        }

        private void btnOpenFile_Click ( object sender, EventArgs e ) {
            OpenFileDialog ofd = new OpenFileDialog();

            switch ( ofd.ShowDialog() ) {
                case DialogResult.OK:
                    using ( StreamReader sr = new StreamReader( ofd.FileName ) ) {
                        ContentOfFile = sr.ReadToEnd();
                        completePath = ofd.FileName;
                        sr.Close();

                    }

                    ParseContent();

                    break;
                default:
                    break;
            }


        }

        private void Form1_Load ( object sender, EventArgs e ) {
            ParseContent();

        }


        private void ParseContent () {
            if ( ContentOfFile == string.Empty ) {
                return;
            }
            
            string[] parts = ContentOfFile.Split( new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries );

            //lbl1.Text = parts[1];
            //lbl2.Text = parts[2] == "1" ? "true" : "false";
            //lbl3.Text = parts[3];
            lbl4.Text = parts[4];
            lbl5.Text = parts[5];
            lbl6.Text = parts[6];
            lbl7.Text = parts[7];
            lbl8.Text = parts[8];
            lbl9.Text = parts[9];
            lbl10.Text = parts[10];
            lbl11.Text = parts[11];

        }
        

    }
}
