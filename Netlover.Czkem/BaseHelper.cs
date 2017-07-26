using System.Collections;
using zkemkeeper;

namespace Netlover.Czkem
{
	public class BaseHelper
	{
		public static string GetError(int errorid)
		{
			var value = "未知错误";
			var msglist = new Hashtable
			{
				{-100, "不支持或数据存在"},
				{-10, "传输的数据长度不对"},
				{-5, "数据已经存在"},
				{-4, "空间不足"},
				{-3, "错误的大小"},
				{-2, "文件读写错误"},
				{-1, "SDK未初始化，需要重新连接"},
				{0, "找不到数据或数据重复"},
				{1, "操作正确"},
				{4, "参数错误"},
				{101, "分配缓冲区错误"}
			};
			if (msglist.ContainsKey(errorid))
			{
				value = msglist[errorid] as string;
			}
			return value;
		}

		public bool IsFaceMachine(CZKEM czkem)
		{
			var faceMaxCount = 0;//1904507651
			czkem.GetDeviceStatus(czkem.MachineNumber, 22, ref faceMaxCount);
			if (faceMaxCount == 1904507651 || faceMaxCount == 0)
			{
				return false;
			}
			return true;
		}
	}
}