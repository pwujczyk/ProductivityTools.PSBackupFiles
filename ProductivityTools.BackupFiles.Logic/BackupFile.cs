﻿using ProductivityTools.BackupFiles.Logic.Modes;
using ProductivityTools.BackupFiles.Logic.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace ProductivityTools.BackupFiles.Logic
{
    public class BackupFile
    {
        private const string FileName = ".Backup";

        public object Direct { get; private set; }
        private string NodeNameBackup = "Backup";
        private string NodeNameMode = "Mode";
        private string NodeNameCopyStrategy = "CopyStrategy";

        public void CreateBackupFile(string directory)
        {
            Create(directory);
        }

        private void Create(string directory)
        {
            XComment comment;
            var document = new XDocument();
            var mainNode = new XElement(NodeNameBackup);
            document.Add(mainNode);


            var properties = typeof(BackupConfig).GetProperties();
            foreach (var property in properties)
            {
                var s1 = $"{property.Name} - {property.GetPropertyDescription()}";
                comment = new XComment(s1);
                mainNode.Add(comment);

                comment = new XComment("Options:");
                mainNode.Add(comment);

                var enums = property.PropertyType.GetEnumValues();
                foreach (var @enum in enums)
                {
                    var s = ReflectionTools.GetEnumDescription(@enum);
                    comment = new XComment($"{@enum} - {s}");
                    mainNode.Add(comment);
                }

                var modeElement2 = new XElement(property.Name, "notDefined");
                mainNode.Add(modeElement2);

            }

            string targetFile = Path.Combine(directory, FileName);
            document.Save(targetFile);

            IEnumerable<Attribute> attribs = ActionDescription.GetActionAttribute();
        }

        //public BackupMode GetBackupMode(string directory)
        //{
        //    var x = Directory.GetFiles(directory, FileName, SearchOption.TopDirectoryOnly).SingleOrDefault();
        //    if (x == null)
        //    {
        //        return BackupMode.NotDefined;
        //    }
        //    else
        //    {
        //        XDocument xdoc = XDocument.Load(x);
        //        string backupMode = (from mode in xdoc.Descendants(NodeNameMode)
        //                             select mode.Value).SingleOrDefault();
        //        if (string.IsNullOrEmpty(backupMode))
        //        {
        //            return BackupMode.NotDefined;
        //        }
        //        BackupMode modeEnum = (BackupMode)Enum.Parse(typeof(BackupMode), backupMode);
        //        return modeEnum;
        //    }
        //}

        private T ParseEnum<T>(XDocument xdoc, string nodeName) 
        {
            T result = default(T);
            string backupMode = (from mode in xdoc.Descendants(nodeName)
                                 select mode.Value).SingleOrDefault();
            if (!string.IsNullOrEmpty(backupMode))
            {
                result = (T)Enum.Parse(typeof(T), backupMode);
            }
            return result;
        }

        public BackupConfig GetBackupConfig(string directory)
        {
            var result = new BackupConfig();
            var x = Directory.GetFiles(directory, FileName, SearchOption.TopDirectoryOnly).SingleOrDefault();
            if (x == null)
            {
                return null;
            }
            else
            {
                XDocument xdoc = XDocument.Load(x);
                var mode=ParseEnum<BackupMode>(xdoc, NodeNameMode);
                var copyStrategy = ParseEnum<CopyStrategyMode>(xdoc, NodeNameCopyStrategy);
                result.Mode = mode;
                result.CopyStrategy = copyStrategy;

                //string backupMode = (from mode in xdoc.Descendants(NodeNameMode)
                //                     select mode.Value).SingleOrDefault();
                //if (!string.IsNullOrEmpty(backupMode))
                //{
                //    result.Mode = (BackupMode)Enum.Parse(typeof(BackupMode), backupMode);
                //}
            }
            return result;
        }
    }
}
