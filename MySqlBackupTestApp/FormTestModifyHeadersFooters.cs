﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace MySqlBackupTestApp
{
    public partial class FormTestModifyHeadersFooters : Form
    {
        public FormTestModifyHeadersFooters()
        {
            InitializeComponent();
        }

        private void btGetHeadersFooters_Click(object sender, EventArgs e)
        {
            using (MySqlConnection conn = new MySqlConnection(Program.ConnectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    using (var mb = new MySqlBackup(cmd))
                    {
                        cmd.Connection = conn;
                        conn.Open();

                        txtHeaders.Lines = mb.ExportInfo.GetDocumentHeaders(cmd).ToArray();
                        txtFooters.Lines = mb.ExportInfo.GetDocumentFooters().ToArray();
                    }
                }
            }
        }

        private void btExportWithNewHeadersFooters_Click(object sender, EventArgs e)
        {
            if (!Program.TargetDirectoryIsValid())
                return;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(Program.ConnectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        using (var mb = new MySqlBackup(cmd))
                        {
                            cmd.Connection = conn;
                            conn.Open();

                            var lstHeaders = new List<string>(txtHeaders.Lines);
                            var lstFooters = new List<string>(txtFooters.Lines);

                            mb.ExportInfo.SetDocumentHeaders(lstHeaders);
                            mb.ExportInfo.SetDocumentFooters(lstFooters);

                            mb.ExportToFile(Program.TargetFile);
                        }
                    }
                }
                MessageBox.Show("Done.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}