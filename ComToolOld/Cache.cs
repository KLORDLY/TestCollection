using System;
using System.Collections.Generic;
using System.Text;

namespace ComToolOld
{
    public static class Cache
    {
        public static string FilePath 
        {
            get 
            {
                return string.Format(@"{0}\Config\",System.Environment.CurrentDirectory);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static void Save(object obj,string fileName)
        {
            if (obj == null)
                return;

            string path = FilePath + fileName;
            try
            {                
                string dir = System.IO.Path.GetDirectoryName(path);
                if (!System.IO.Directory.Exists(dir))
                    System.IO.Directory.CreateDirectory(dir);

                using (System.IO.FileStream fs = System.IO.File.Open(path, System.IO.FileMode.OpenOrCreate))
                {
                    System.Runtime.Serialization.Formatters.Binary.BinaryFormatter s = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    s.Serialize(fs, obj);
                }
            }
            catch 
            {
                //MeterERP.ErrorLog.Error.Save(path + "\n" + err.Message, err.StackTrace);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static void SaveByAllName(object obj, string fileAllName)
        {
            if (obj == null)
                return;

            string path = fileAllName;
            try
            {
                string dir = System.IO.Path.GetDirectoryName(path);
                if (!System.IO.Directory.Exists(dir))
                    System.IO.Directory.CreateDirectory(dir);

                using (System.IO.FileStream fs = System.IO.File.Open(path, System.IO.FileMode.OpenOrCreate))
                {
                    System.Runtime.Serialization.Formatters.Binary.BinaryFormatter s = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    s.Serialize(fs, obj);
                }
            }
            catch
            {
               //MeterERP.ErrorLog.Error.Save(path + "\n" + err.Message, err.StackTrace);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static object Load(string fileName)
        {
            string path = FilePath + fileName;
            try
            {
                if (!System.IO.File.Exists(path))
                    return null;

                using (System.IO.FileStream fs = System.IO.File.Open(path, System.IO.FileMode.OpenOrCreate))
                {
                    System.Runtime.Serialization.Formatters.Binary.BinaryFormatter s = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    object obj = s.Deserialize(fs);
                    return obj;
                }
            }
            catch 
            {
                //MeterERP.ErrorLog.Error.Save(path + "\n" + err.Message, err.StackTrace);
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static object LoadByAllName(string fileName)
        {
            string path = fileName;
            try
            {
                if (!System.IO.File.Exists(path))
                    return null;

                using (System.IO.FileStream fs = System.IO.File.Open(path, System.IO.FileMode.OpenOrCreate))
                {
                    System.Runtime.Serialization.Formatters.Binary.BinaryFormatter s = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    object obj = s.Deserialize(fs);
                    return obj;
                }
            }
            catch
            {
                //MeterERP.ErrorLog.Error.Save(path + "\n" + err.Message, err.StackTrace);
                return null;
            }
        }
    }
}
