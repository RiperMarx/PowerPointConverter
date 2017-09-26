﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Form1.cs" company="Alkaworks">
//   Copyright Alkaworks
// </copyright>
// <summary>
//   Defines the Form1 type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace PowerPointConverter
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Windows.Forms;
    using Microsoft.Office.Core;
    using pptApplication = Microsoft.Office.Interop.PowerPoint;
    
    /// <summary>
    /// The form 1.
    /// </summary>
    public partial class Form1 : Form
    {
        /// <summary>
        /// The file list.
        /// </summary>
        private static readonly List<string> FileList = new List<string>();
        
        /// <summary>
        /// Initializes a new instance of the <see cref="Form1"/> class.
        /// </summary>
        public Form1()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// The form 1_ load.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void Form1Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.Fixed3D;
        }

        /// <summary>
        /// The button click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void ButtonSeleccionClick(object sender, EventArgs e)
        {
            FolderBrowserDialog fb =
                new FolderBrowserDialog
                    {
                        ShowNewFolderButton = false,
                        Description = @"Selecciona los archivos a importar"
                    };
            if (fb.ShowDialog() == DialogResult.OK)
            {
                VariablesGlobales.Directorio = fb.SelectedPath;

                var directory = new DirectoryInfo(VariablesGlobales.Directorio);
                var files = directory.GetFiles("*.pptx");

                foreach (var file in files)
                {
                    FileList.Add(file.FullName);
                }

                this.Button_Convertir.Enabled = true;
            }
            else
            {
                this.Button_Convertir.Enabled = false;
                MessageBox.Show(@"No Seleccionaste Ningun Folder");
            }
        }

        /// <summary>
        /// The button to start converting the files.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void ButtonConvertirClick(object sender, EventArgs e)
        {
            FolderBrowserDialog fbe =
                new FolderBrowserDialog
                    {
                        ShowNewFolderButton = true,
                        Description = @"Selecciona el folder para guardar las imagenes"
                    };
            if (fbe.ShowDialog() == DialogResult.OK)
            {
                VariablesGlobales.DirectorioExpo = fbe.SelectedPath;
            
           var pptp = new pptApplication.Application();

            try
            {
                foreach (var element in FileList)
            {
                pptApplication.Presentation ppps = pptp.Presentations.Open(
                element,
                MsoTriState.msoFalse,
                MsoTriState.msoFalse,
                MsoTriState.msoFalse);

                // Quitamos el dummy para que los archivos tubieran un nombre dinamico
                        // var diff = 0;
                        foreach (Microsoft.Office.Interop.PowerPoint.Slide pptSlides in ppps.Slides)
            {
                // Quitamos el dummy para que los archivos tubieran un nombre dinamico
                // diff++;
                pptSlides.Export(
                    VariablesGlobales.DirectorioExpo + "\\" + Path.GetFileNameWithoutExtension(element) + ".jpg",
                    "jpg",
                    3840,
                    2160);
                }
            }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());

               // throw;
            }

            pptp.Quit();

            MessageBox.Show(@"Convertido con Exito!");
            }
            else
            {
                MessageBox.Show(@"No seleccionaste ningun folder de Exportacion");
            }
        }

        /// <summary>
        /// Exits The Application
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void ButtonExitClick(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// The Public Variables.
        /// </summary>
        public class VariablesGlobales
        {
            /// <summary>
            /// Gets or sets the directory.
            /// </summary>
            public static string Directorio { get; set; }

            /// <summary>
            /// Gets or sets the directory expo.
            /// </summary>
            public static string DirectorioExpo { get; set; }
        }
    }
}
