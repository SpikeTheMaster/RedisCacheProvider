using System;
using System.Runtime.Serialization;

namespace RedisCacheProviderTests
{
    [Serializable]
    public class Person: ISerializable
    {
        public string Name { get; set; }
        public int Age { get; set; }

        public Person()
        {
        }

        public Person(string name, int age)
        {
            Name = name;
            Age = age;
        }

        public Person(SerializationInfo info, StreamingContext context)
        {
            Name = (string) info.GetValue("Name", typeof (string));
            Age = (int) info.GetValue("Age", typeof (int));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Name", Name );
            info.AddValue("Age", Age );
        }
    }
}