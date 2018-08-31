using System;
using System.IO ;

namespace FZYK.Com.Msg
{
	/// <summary>
	/// ClassSerializers ��ժҪ˵����
	/// </summary>
	public class YKClassSerializers
	{
        public YKClassSerializers()
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
		}

		#region Binary Serializers
        public System.IO.MemoryStream SerializeBinary(object request) //��������ת���ɶ�������
		{
			System.Runtime.Serialization.Formatters.Binary.BinaryFormatter serializer = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
			System.IO.MemoryStream memStream = new System.IO.MemoryStream();    //����һ���ڴ����洢��
			serializer.Serialize(memStream, request);//���������л�Ϊ�ڴ�����
			return memStream;
		}

        public object DeSerializeBinary(System.IO.MemoryStream memStream) //����������ת���ɶ���
		{
��			memStream.Position=0;
��			System.Runtime.Serialization.Formatters.Binary.BinaryFormatter deserializer = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
��			object newobj = deserializer.Deserialize(memStream);//���ڴ��������л�Ϊ����
��			memStream.Close();  //�ر��ڴ��������ͷ�
��			return newobj;
		}
		#endregion

	}
}
