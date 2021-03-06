﻿using ProductivityTools.BackupFiles.Logic.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductivityTools.BackupFiles.Logic
{
    [Description("It will decide how copying operation should be performed")]
    public enum BackupMode : int
    {
        NotDefined,
        DoNothing,
        [Default]
        [Description("It will copy files and directories recursively, until it will find another file which will override action")]
        CopyRecursively,
        CopyJustFiles
    }
}
