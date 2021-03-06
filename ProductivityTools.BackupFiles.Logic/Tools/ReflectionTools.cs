﻿using ProductivityTools.BackupFiles.Logic.Actions;
using ProductivityTools.BackupFiles.Logic.CopyStrategy;
using ProductivityTools.BackupFiles.Logic.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ProductivityTools.BackupFiles.Logic
{
    static class ReflectionTools
    {
        public static IEnumerable<Type> GetEnumerableOfType<T>() where T : class
        {
            List<Type> objects = new List<Type>();
            foreach (Type type in
                Assembly.GetAssembly(typeof(T)).GetTypes()
                .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(T))))
            {
                objects.Add(type);
            }
            return objects;
        }

        public static BaseAction CreateInstanceOfActionFromEnum(int backupConfig)
        {
            IEnumerable<Type> actions = GetEnumerableOfType<BaseAction>();
            foreach (Type type in actions)
            {
                var attribute = ActionDescription.GetActionAttribute(type);
                if (attribute != null)
                {
                    if (attribute.BackupMode == backupConfig)
                    {
                        var action = (BaseAction)Activator.CreateInstance(type);
                        return action;
                    }
                }
            }
            return null;
        }

        public static T CreateInstanceOfActionFromEnum<T>(int backupConfig) where T : class
        {
            IEnumerable<Type> actions = GetEnumerableOfType<T>();
            foreach (Type type in actions)
            {
                var attribute = ActionDescription.GetActionAttribute(type);
                if (attribute != null)
                {
                    if (attribute.BackupMode == backupConfig)
                    {
                        var action = (T)Activator.CreateInstance(type);
                        return action;
                    }
                }
            }
            return null;
        }

        public static string GetEnumDescription<T>(T value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes = fi.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

            if (attributes != null && attributes.Any())
            {
                return attributes.First().Description;
            }

            return value.ToString();
        }

        public static bool GetDefault<T>(T value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DefaultAttribute[] attributes = fi.GetCustomAttributes(typeof(DefaultAttribute), false) as DefaultAttribute[];

            if (attributes != null && attributes.Any())
            {
                return true;
            }

            return false;
        }

        public static string GetPropertyDescription(this PropertyInfo that)
        {
            var x = that.PropertyType.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];
            return x[0].Description;
        }
    }
}
