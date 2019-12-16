﻿using ProductivityTools.BackupFiles.Logic.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductivityTools.BackupFiles.Logic
{
    class ActionList
    {
        private Dictionary<string, BaseAction> CurrentWork = new Dictionary<string, BaseAction>();

        public void Add(string key, BackupMode a)
        {
            BaseAction baseAction = ReflectionTools.CreateInstanceOfActionFromEnum(a);
            if (baseAction != null)
            {
                this.CurrentWork.Add(key, baseAction);
            }
        }

        public bool Contains(string path)
        {
            foreach (var item in this.CurrentWork)
            {
                if (path.StartsWith(item.Key))
                {
                    return true;
                }
            }
            return false;
        }

        public void InvokeForPath(string masterSourcePath, string masterDestinationPath, string directory)
        {
            var workOrdered = this.CurrentWork.OrderByDescending(x => x.Key.Length);
            foreach (var item in workOrdered)
            {
                if (directory.StartsWith(item.Key))
                {
                    item.Value.Process(masterSourcePath, masterDestinationPath, directory);
                    return;
                }
            }
        }
    }
}

