﻿using ProductivityTools.BackupFiles.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace ProductivityTools.BackupFiles
{
    [Cmdlet(VerbsCommon.New, "BackupFile")]
    public class NewBackupFileCmdlet : ProductivityTools.PSCmdlet.PSCmdletPT
    {
        public NewBackupFileCmdlet()
        {
        }

        protected override void ProcessRecord()
        {
            string path = CurrentProviderLocation("FileSystem").ProviderPath;
            new BackupFile().CreateBackupFile(path);
            base.ProcessRecord();
        }
    }
}
