using System;
using System.IO ;

namespace FZYK.Com.Msg
{
	/// <summary>
	/// ClassSerializers 的摘要说明。
	/// </summary>
	public class YKClassSerializers
	{
        public YKClassSerializers()
		{
			//
			// TODO: 在此处添加构造函数逻辑
			//
		}

		#region Binary Serializers
        public System.IO.MemoryStream SerializeBinary(object request) //将对象流转换成二进制流
		{
			System.Runtime.Serialization.Formatters.Binary.BinaryFormatter serializer = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
			System.IO.MemoryStream memStream = new System.IO.MemoryStream();    //创建一个内存流存储区
			serializer.Serialize(memStream, request);//将对象序列化为内存流中
			return memStream;
		}

        public object DeSerializeBinary(System.IO.MemoryStream memStream) //将二进制流转换成对象
		{
　			memStream.Position=0;
　			System.Runtime.Serialization.Formatters.Binary.BinaryFormatter deserializer = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
　			object newobj = deserializer.Deserialize(memStream);//将内存流反序列化为对象
　			memStream.Close();  //关闭内存流，并释放
　			return newobj;
		}
		#endregion

	}
}
