using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using UnityEngine;

public class Singleton<T> where T:class,new()
{
    static T m_instance = null;
    public static T Instance
    {
        get
        {
            if(m_instance==null)
            {
                m_instance = new T();
            }
            return m_instance;
        }
    }
}
/*public object DeepClone()
{
    using (Stream objectStream = new MemoryStream())
    {
        IFormatter formatter = new BinaryFormatter();
        formatter.Serialize(objectStream, this);
        objectStream.Seek(0, SeekOrigin.Begin);
        return formatter.Deserialize(objectStream) as PlayerData;
    }
}*/
