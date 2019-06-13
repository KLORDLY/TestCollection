using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DeepClone
{
    [Serializable]
    public class WeeklyLog
    {
        //为了简化设计和实现，假设一份工作周报中只有一个附件对象，实际情况中可以包含多个附件，可以通过List等集合对象来实现

        private Attachment attachment;
        private String name;
        private String date;
        private String content;

        public void setAttachment(Attachment attachment)
        {
            this.attachment = attachment;
        }
        public void setName(String name)
        {
            this.name = name;
        }
        public void setDate(String date)
        {
            this.date = date;
        }
        public void setContent(String content)
        {
            this.content = content;
        }

        public Attachment getAttachment()
        {
            return (this.attachment);
        }
        public String getName()
        {
            return (this.name);
        }
        public String getDate()
        {
            return (this.date);
        }
        public String getContent()
        {
            return (this.content);
        }
        //使用序列化技术实现深克隆
        public WeeklyLog DeepClone()
        {
            //将对象写入流中
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            formatter.Serialize(stream, this);
            stream.Seek(0, SeekOrigin.Begin);
            //ByteArrayOutputStream bao = new ByteArrayOutputStream();
            //ObjectOutputStream oos = new ObjectOutputStream(bao);
            //oos.writeObject(this);

            //将对象从流中取出
            //ByteArrayInputStream bis = new ByteArrayInputStream(bao.toByteArray());
            //ObjectInputStream ois = new ObjectInputStream(bis);
            Object obj = formatter.Deserialize(stream);
            stream.Close();
            return (WeeklyLog)obj;
        }

        // 利用反射实现深拷贝
        public T DeepCopyWithReflection<T>(T obj)
        {
            Type type = obj.GetType();
            // 如果是字符串或值类型则直接返回
            if (obj is string || type.IsValueType)
                return obj;
            if (type.IsArray)
            {
                Type elementType = Type.GetType(type.FullName.Replace("[]", string.Empty));
                var array = obj as Array;
                Array copied = Array.CreateInstance(elementType, array.Length);
                for (int i = 0; i < array.Length; i++)
                {
                    copied.SetValue(DeepCopyWithReflection(array.GetValue(i)), i);
                }

                return (T)Convert.ChangeType(copied, obj.GetType());
            }

            object retval = Activator.CreateInstance(obj.GetType());

            PropertyInfo[] properties = obj.GetType().GetProperties(
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            foreach (var property in properties)
            {
                var propertyValue = property.GetValue(obj, null);
                if (propertyValue == null)
                    continue;
                property.SetValue(retval, DeepCopyWithReflection(propertyValue), null);
            }
            return (T)retval;
        }

        // 利用XML序列化和反序列化实现
        public WeeklyLog DeepCopyWithXmlSerializer()
        {
            object retval;
            using (MemoryStream ms = new MemoryStream())
            {
                Type t = this.GetType();
                XmlSerializer xml = new XmlSerializer(t);
                xml.Serialize(ms, this);
                ms.Seek(0, SeekOrigin.Begin);
                retval = xml.Deserialize(ms);
                ms.Close();
            }
            return (WeeklyLog)retval;
        }

        //需要silverlight支持
        //public static T DeepCopy<T>(T obj)
        //{
        //    object retval;
        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        DataContractSerializer ser = new DataContractSerializer(typeof(T));
        //        ser.WriteObject(ms, obj);
        //        ms.Seek(0, SeekOrigin.Begin);
        //        retval = ser.ReadObject(ms);
        //        ms.Close();
        //    }
        //    return (T)retval;
        //}
    }
}
