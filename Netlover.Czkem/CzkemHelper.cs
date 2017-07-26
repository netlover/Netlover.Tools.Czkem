using System;
using System.Collections.Generic;
using Netlover.Czkem.Models;
using zkemkeeper;

namespace Netlover.Czkem
{
	public class CzkemHelper: BaseHelper
	{
		private readonly CZKEM _czkem;
		public readonly int MachineId;
		public readonly string Title;
		public readonly bool Back;
		public readonly bool Auto;
		private int _errorId;

		public CzkemHelper()
		{
			_czkem = new CZKEM();
		}
		public CzkemHelper(int machineid)
		{
			_czkem = new CZKEM();
			MachineId = machineid;
		}
		public CzkemHelper(int machineid, string title, bool back, bool auto)
		{
			_czkem = new CZKEM();
			MachineId = machineid;
			Title = title;
			Back = back;
			Auto = auto;
		}

		#region Base

		#region Connect

		public bool Connect(string ip, int port, int? password)
		{
			if (password.HasValue)
			{
				_czkem.SetCommPassword(password.Value);
			}
			return _czkem.Connect_Net(ip, port);
		}

		public bool Connect(int port, int number, int rate)
		{
			//if (password.HasValue)
			//{
			//	_czkem.SetCommPassword(password.Value);
			//}
			return _czkem.Connect_Com(port, number, rate);
		}

		public bool Connect(int number)
		{
			//if (password.HasValue)
			//{
			//	_czkem.SetCommPassword(password.Value);
			//}
			return _czkem.Connect_USB(number);
		}

		#endregion

		public void Disconnect()
		{
			_czkem.Disconnect();
		}

		public bool IsColorMachine()
		{
			return _czkem.IsTFTMachine(_czkem.MachineNumber);
		}

		public bool IsFaceMachine()
		{
			var faceMaxCount = 0;
			var result = _czkem.GetDeviceStatus(_czkem.MachineNumber, 22, ref faceMaxCount);
			if (result)
			{
				if (faceMaxCount == 1904507651 || faceMaxCount == 0)
				{
					result = false;
				}
			}
			return result;
		}

		public string Error
		{
			get
			{
				_czkem.GetLastError(ref _errorId);
				return GetError(_errorId);
			}
		}

		#endregion

		#region UserInfo

		#region SetUserInfo

		public bool SetUserInfo(CzkemUserInfo user)
		{
			return _czkem.SetUserInfo(_czkem.MachineNumber, user.UserId, user.Fullname, user.Password, user.Privilege, user.Enabled);
		}

		public bool SetUserInfo(CzkemUserInfoEx user)
		{
			return _czkem.SSR_SetUserInfo(_czkem.MachineNumber, user.UserId, user.Fullname, user.Password, user.Privilege, user.Enabled);
		}

		#endregion

		#region EnableUser

		public bool EnableUser(int userid, bool flag)
		{
			return _czkem.EnableUser(_czkem.MachineNumber, userid, 0, 0, flag);
		}

		public bool EnableUser(string userid, bool flag)
		{
			return _czkem.SSR_EnableUser(_czkem.MachineNumber, userid, flag);
		}

		#endregion

		#region ModifyPrivilege

		public bool Privilege(int userid, int privilege)
		{
			return _czkem.ModifyPrivilege(_czkem.MachineNumber, userid, 0, 0, privilege);
		}

		#endregion

		#region GetUserInfo

		public CzkemUserInfo GetUserInfo(int userid)
		{
			var fullname = string.Empty;
			var password = string.Empty;
			var privilege = 0;
			var enable = false;
			var result = _czkem.GetUserInfo(_czkem.MachineNumber, userid, ref fullname, ref password, ref privilege, ref enable);
			if (result)
			{
				return new CzkemUserInfo
				{
					UserId = userid,
					Fullname = fullname,
					Password = password,
					Privilege = privilege,
					Enabled = enable
				};
			}
			return null;
		}

		public CzkemUserInfoEx GetUserInfo(string userid)
		{
			string fullname;
			string password;
			int privilege;
			bool enable;
			var result = _czkem.SSR_GetUserInfo(_czkem.MachineNumber, userid, out fullname, out password, out privilege, out enable);
			if (result)
			{
				return new CzkemUserInfoEx
				{
					UserId = userid,
					Fullname = fullname,
					Password = password,
					Privilege = privilege,
					Enabled = enable
				};
			}
			return null;
		}

		#endregion

		#region RemoveUserInfo

		public bool RemoveUserInfo(int userid)
		{
			return _czkem.DeleteUserInfoEx(_czkem.MachineNumber, userid);
		}

		#endregion

		#region GetAllUserInfo

		public List<CzkemUserInfo> GetAllUserInfo
		{
			get
			{
				var list = new List<CzkemUserInfo>();
				var result = _czkem.ReadAllUserID(_czkem.MachineNumber);
				if (result)
				{
					var userid = 0;
					var fullname = string.Empty;
					var password = string.Empty;
					var privilege = 0;
					var enable = false;
					while (_czkem.GetAllUserInfo(_czkem.MachineNumber, ref userid, ref fullname, ref password, ref privilege, ref enable))
					{
						list.Add(new CzkemUserInfo
						{
							UserId = userid,
							Fullname = fullname,
							Password = password,
							Privilege = privilege,
							Enabled = enable
						});
					}
				}
				return list;
			}
		}

		public List<CzkemUserInfoEx> GetAllUserInfoEx
		{
			get
			{
				var list = new List<CzkemUserInfoEx>();
				var result = _czkem.ReadAllUserID(_czkem.MachineNumber);
				if (result)
				{
					string userid;
					string fullname;
					string password;
					int privilege;
					bool enable;
					while (_czkem.SSR_GetAllUserInfo(_czkem.MachineNumber, out userid, out fullname, out password, out privilege, out enable))
					{
						list.Add(new CzkemUserInfoEx
						{
							UserId = userid,
							Fullname = fullname,
							Password = password,
							Privilege = privilege,
							Enabled = enable
						});
					}
				}
				return list;
			}
		}

		#endregion

		#endregion

		#region UserTemp

		#region SetUserTemp

		public bool SetUserTemp(int userid, int index, string data)
		{
			return _czkem.SetUserTmpStr(_czkem.MachineNumber, userid, index, data);
		}

		public bool SetUserTemp(string userid, int index, int flag, string data)
		{
			return _czkem.SetUserTmpExStr(_czkem.MachineNumber, userid, index, flag, data);
		}

		#endregion

		#region GetUserTemp

		public CzkemUserTemp GetUserTemp(int userid, int index)
		{
			var data = string.Empty;
			var length = 0;
			var result = _czkem.GetUserTmpStr(_czkem.MachineNumber, userid, index, ref data, ref length);
			if (result)
			{
				return new CzkemUserTemp
				{
					Index = index,
					Data = data,
					Length = length,
				};
			}
			return null;
		}

		public CzkemUserTemp GetUserTemp(string userid, int index)
		{
			string data;
			int length;
			int flag;
			var result = _czkem.GetUserTmpExStr(_czkem.MachineNumber, userid, index, out flag, out data, out length);
			if (result)
			{
				return new CzkemUserTemp
				{
					Index = index,
					Data = data,
					Length = length,
					Flag = flag
				};
			}
			return null;
		}

		#endregion

		#region RemoveUserTemp

		public bool RemoveUserTemp(int userid, int index)
		{
			return _czkem.DelUserTmp(_czkem.MachineNumber, userid, index);
		}

		//public bool RemoveUserTemp(string userid, int index, ref string parameter)
		//{
		//	var result = _czkem.SSR_DelUserTmp(_czkem.MachineNumber, userid, index);
		//	if (!result)
		//	{
		//		parameter = Error;
		//	}
		//	return result;
		//}

		/// <summary>
		/// 删除指纹
		/// </summary>
		/// <param name="userid"></param>
		/// <param name="index">若为13则删除全部</param>
		/// <returns></returns>
		public bool RemoveUserTemp(string userid, int index)
		{
			return _czkem.SSR_DelUserTmpExt(_czkem.MachineNumber, userid, index);
		}

		#endregion

		#region ReadAllTemp

		public bool ReadAllTemp()
		{
			return _czkem.ReadAllTemplate(_czkem.MachineNumber);
		}

		public bool ReadAllTemp(string userid)
		{
			return _czkem.ReadUserAllTemplate(_czkem.MachineNumber, userid);
		}

		#endregion

		#endregion

		#region UserFace

		#region SetUserFace

		public bool SetUserFace(string userid, string data, int length)
		{
			var index = 50;
			return _czkem.SetUserFaceStr(_czkem.MachineNumber, userid, index, data, length);
		}

		#endregion

		#region GetUserTemp

		public CzkemUserFace GetUserFace(string userid)
		{
			var data = string.Empty;
			var length = 0;
			var index = 50;
			var result = _czkem.GetUserFaceStr(_czkem.MachineNumber, userid, index, ref data, ref length);
			if (result)
			{
				return new CzkemUserFace
				{
					Data = data,
					Length = length
				};
			}
			return null;
		}

		#endregion

		#region RemoveUserTemp

		public bool RemoveUserFace(string userid)
		{
			var index = 50;//必须为50，代表删除该用户所有人脸模板
			return _czkem.DelUserFace(_czkem.MachineNumber, userid, index);
		}

		#endregion

		#endregion

		#region UserLog

		public bool ReadUserLog => _czkem.ReadAllGLogData(_czkem.MachineNumber);

		public List<CzkemUserLog> GetUserLog
		{
			get
			{
				var list = new List<CzkemUserLog>();
				if (ReadUserLog)
				{
					var userid = 0;
					var verify = 0;
					var inout = 0;
					var datetime = string.Empty;
					while (_czkem.GetGeneralLogDataStr(_czkem.MachineNumber, ref userid, ref verify, ref inout, ref datetime))
					{
						list.Add(new CzkemUserLog
						{
							UserId = userid,
							Verify = verify,
							InOut = inout,
							Datetime = Convert.ToDateTime(datetime)
						});
					}
				}
				return list;
			}
		}

		public List<CzkemUserLogEx> GetUserLogEx
		{
			get
			{
				var list = new List<CzkemUserLogEx>();
				if (ReadUserLog)
				{
					string userid;
					int verify;
					int inout;
					int year;
					int month;
					int day;
					int hour;
					int minute;
					int secound;
					var code = 0;
					while (_czkem.SSR_GetGeneralLogData(_czkem.MachineNumber, out userid, out verify, out inout, out year, out month,
						out day, out hour, out minute, out secound, ref code))
					{
						list.Add(new CzkemUserLogEx
						{
							UserId = userid,
							Verify = verify,
							InOut = inout,
							Datetime = new DateTime(year, month, day, hour, minute, secound)
						});
					}
				}
				return list;
			}
		}

		public bool ClearLog()
		{
			return _czkem.ClearGLog(_czkem.MachineNumber);
		}

		#endregion

		#region Machine

		public bool EnableDevice(bool flag)
		{
			return _czkem.EnableDevice(_czkem.MachineNumber, flag);
		}

		public bool RefreshData()
		{
			return _czkem.RefreshData(_czkem.MachineNumber);
		}

		/// <summary>
		/// 需获取的数据，范围为 1-22，含义如下
		/// </summary>
		/// <param name="status">需获取的数据，范围为 1-22，含义如下</param>
		/// 1 管理员数量
		/// 2 注册用户数量
		/// 3 机器内指纹模板数量
		/// 4 密码数量
		/// 5 操作记录数
		/// 6 考勤记录数
		/// 7 指纹模板容量
		/// 8 用户容量
		/// 9 考勤记录容量
		/// 10 剩余指纹模板容量
		/// 11 剩余用户数容量
		/// 12 剩余考勤记录容量
		/// 21 人脸总数
		/// 22 人脸容量
		/// 其他状况返回 其他状况返回 其他状况返回 0
		/// <returns></returns>
		public int? GetDeviceStatus(int status)
		{
			var value = 0;
			var result = _czkem.GetDeviceStatus(_czkem.MachineNumber, status, ref value);
			if (result)
			{
				return value;
			}
			return null;
		}

		public bool SetDeviceTime()
		{
			return _czkem.SetDeviceTime(_czkem.MachineNumber);
		}

		public bool SetDeviceTime(DateTime datetime)
		{
			return _czkem.SetDeviceTime2(_czkem.MachineNumber, datetime.Year, datetime.Month, datetime.Day, datetime.Hour, datetime.Minute, datetime.Second);
		}

		public DateTime? GetDeviceTime
		{
			get
			{
				var year = 0;
				var month = 0;
				var day = 0;
				var hour = 0;
				var minute = 0;
				var secound = 0;
				var result = _czkem.GetDeviceTime(_czkem.MachineNumber, ref year, ref month, ref day, ref hour, ref minute, ref secound);
				if (result)
				{
					return new DateTime(year, month, day, hour, minute, secound);
				}
				return null;
			}
		}

		public string GetSerialNumber
		{
			get
			{
				string value;
				if (_czkem.GetSerialNumber(_czkem.MachineNumber, out value))
				{
					return value;
				}
				return string.Empty;
			}
		}

		public string GetProductCode
		{
			get
			{
				string value;
				if (_czkem.GetProductCode(_czkem.MachineNumber, out value))
				{
					return value;
				}
				return string.Empty;
			}
		}

		public string GetFirmwareVersion
		{
			get
			{
				var value = string.Empty;
				if (_czkem.GetFirmwareVersion(_czkem.MachineNumber, ref value))
				{
					return value;
				}
				return string.Empty;
			}
		}

		public string GetSdkVersion
		{
			get
			{
				var value = string.Empty;
				if (_czkem.GetFirmwareVersion(_czkem.MachineNumber, ref value))
				{
					return value;
				}
				return string.Empty;
			}
		}

		public string GetDeviceIp
		{
			get
			{
				var value = string.Empty;
				if (_czkem.GetDeviceIP(_czkem.MachineNumber, ref value))
				{
					return value;
				}
				return string.Empty;
			}
		}

		public bool SetDeviceIp(string ip)
		{
			return _czkem.SetDeviceIP(_czkem.MachineNumber, ip);
		}

		public string GetCardNumber
		{
			get
			{
				string value;
				if (_czkem.GetStrCardNumber(out value))
				{
					return value;
				}
				return string.Empty;
			}
		}

		public bool SetCardNumber(string cardnumber, ref string parameter)
		{
			var result = _czkem.SetStrCardNumber(cardnumber);
			if (!result)
			{
				parameter = Error;
			}
			return result;
		}

		/// <summary>
		/// 获取机器是否支持射频卡功能,当返回值为 1 时，仅支持射频卡；为 2 时，即支持射频卡也支持指纹；为 0 时，不支持射频卡
		/// </summary>
		public int? GetCardFun
		{
			get
			{
				var value = 0;
				if (_czkem.GetCardFun(_czkem.MachineNumber, ref value))
				{
					return value;
				}
				return null;
			}
		}

		/// <summary>
		/// 设置机器通讯密码，0为不设置
		/// </summary>
		/// <param name="password"></param>
		/// <returns></returns>
		public bool SetCommPassword(int password)
		{
			return _czkem.SetDeviceCommPwd(_czkem.MachineNumber, password);
		}

		public bool PowerOffDevice()
		{
			return _czkem.PowerOffDevice(_czkem.MachineNumber);
		}

		public bool RestartDevice()
		{
			return _czkem.RestartDevice(_czkem.MachineNumber);
		}

		public bool BatchUpdate()
		{
			return _czkem.BatchUpdate(_czkem.MachineNumber);
		}

		/// <summary>
		/// 批量上传
		/// </summary>
		/// <param name="flag">0：不覆盖；1：强制覆盖</param>
		/// <returns></returns>
		public bool BatchUpdateBegin(int flag)
		{
			return _czkem.BeginBatchUpdate(_czkem.MachineNumber, flag);
		}

		public bool BatchUpdateCancel()
		{
			return _czkem.CancelBatchUpdate(_czkem.MachineNumber);
		}

		public bool CancelOperation()
		{
			return _czkem.CancelOperation();
		}

		public bool StartEnroll(int userid, int index)
		{
			return _czkem.StartEnroll(userid, index);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="userid"></param>
		/// <param name="index"></param>
		/// <param name="flag">0:无效 1：有效 3：胁迫</param>
		/// <returns></returns>
		public bool StartEnroll(string userid, int index, int flag)
		{
			return _czkem.StartEnrollEx(userid, index, flag);
		}

		/// <summary>
		/// 该参数指定需清除的记录类型
		/// </summary>
		/// <param name="flag">该参数指定需清除的记录类型，范围为 1—5，具体含义如下</param>
		/// 1 考勤记录
		/// 2 指纹模板数据
		/// 3 无
		/// 4 操作记录
		/// 5 用户信息，即删除机器内所有用户，注：此操作同会删除指纹机内所有指纹模板
		/// <returns></returns>
		public bool ClearData(int flag)
		{
			return _czkem.ClearData(_czkem.MachineNumber, flag);
		}

		public bool ClearGLog()
		{
			return _czkem.ClearGLog(_czkem.MachineNumber);
		}

		/// <summary>
		/// 发送文件到机器
		/// </summary>
		/// <param name="filename">发送文件到机器，一般发送文件到/mnt/mtdblock下，彩屏机如传的是用户照片或宣图下， 需命名为以下格式：图片会自动被转移到相应目录下</param>
		/// 宣传图片命名方式： “ad_” 为前缀 ,后加数字，范围为 1－20 ，后缀为 .jpg，如 ad_4.jpg
		/// 用户照片命名方式： “用户ID”+“.jpg”，如 1.jpg
		/// <returns></returns>
		public bool SendFile(string filename)
		{
			return _czkem.SendFile(_czkem.MachineNumber, filename);
		}

		public CzkemStatusInfo GetStatusInfo
		{
			get
			{
				var now = DateTime.Now;

				var adminCount = -1;
				var actionCount = -1;
				var passWordCount = -1;
				var fingerMaxCount = -1;
				var fingerResidueCount = -1;
				var fingerUsedCount = -1;
				var faceMaxCount = -1;
				int faceResidueCount;
				var faceUsedCount = -1;
				var peopleMaxCount = -1;
				var peopleResidueCount = -1;
				var peopleUsedCount = -1;
				var logMaxCount = -1;
				var logResidueCount = -1;
				var logUsedCount = -1;
				var dwYear = now.Year;
				var dwMonth = now.Month;
				var dwDay = now.Day;
				var dwHour = now.Hour;
				var dwMin = now.Minute;
				var dwSecond = now.Second;

				string productcode;
				var productversion = string.Empty;
				string productnumber;
				string sValue;
				string zkfaceversion;

				const string sOption = "~ZKFPVersion";
				_czkem.GetSysOption(_czkem.MachineNumber, sOption, out sValue);

				const string fOption = "ZKFaceVersion";
				_czkem.GetSysOption(_czkem.MachineNumber, fOption, out zkfaceversion);

				var iscolor = _czkem.IsTFTMachine(_czkem.MachineNumber);
				var isface = IsFaceMachine(_czkem);

				//获取机器相关数据
				_czkem.GetDeviceStatus(_czkem.MachineNumber, 1, ref adminCount);
				_czkem.GetDeviceStatus(_czkem.MachineNumber, 2, ref peopleUsedCount);
				_czkem.GetDeviceStatus(_czkem.MachineNumber, 3, ref fingerUsedCount);
				_czkem.GetDeviceStatus(_czkem.MachineNumber, 4, ref passWordCount);
				_czkem.GetDeviceStatus(_czkem.MachineNumber, 5, ref actionCount);
				_czkem.GetDeviceStatus(_czkem.MachineNumber, 6, ref logUsedCount);
				_czkem.GetDeviceStatus(_czkem.MachineNumber, 7, ref fingerMaxCount);
				_czkem.GetDeviceStatus(_czkem.MachineNumber, 8, ref peopleMaxCount);
				_czkem.GetDeviceStatus(_czkem.MachineNumber, 9, ref logMaxCount);
				_czkem.GetDeviceStatus(_czkem.MachineNumber, 10, ref fingerResidueCount);
				_czkem.GetDeviceStatus(_czkem.MachineNumber, 11, ref peopleResidueCount);
				_czkem.GetDeviceStatus(_czkem.MachineNumber, 12, ref logResidueCount);
				_czkem.GetDeviceTime(_czkem.MachineNumber, ref dwYear, ref dwMonth, ref dwDay, ref dwHour, ref dwMin, ref dwSecond);
				_czkem.GetProductCode(_czkem.MachineNumber, out productcode);
				_czkem.GetFirmwareVersion(_czkem.MachineNumber, ref productversion);
				_czkem.GetSerialNumber(_czkem.MachineNumber, out productnumber);

				if (isface)
				{
					_czkem.GetDeviceStatus(_czkem.MachineNumber, 21, ref faceUsedCount);
					_czkem.GetDeviceStatus(_czkem.MachineNumber, 22, ref faceMaxCount);

					faceResidueCount = faceMaxCount - faceUsedCount;
				}
				else
				{
					faceMaxCount = 0;
					faceResidueCount = 0;
					faceUsedCount = 0;
				}

				var model = new CzkemStatusInfo
				{
					MachineId = MachineId,
					AdminCount = adminCount,
					ActionCount = actionCount,
					FingerMaxCount = fingerMaxCount,
					FingerResidueCount = fingerResidueCount,
					FingerUsedCount = fingerUsedCount,
					LogMaxCount = logMaxCount,
					LogResidueCount = logResidueCount,
					LogUsedCount = logUsedCount,
					PassWordCount = passWordCount,
					PeopleMaxCount = peopleMaxCount,
					PeopleResidueCount = peopleResidueCount,
					PeopleUsedCount = peopleUsedCount,
					Model = productcode,//string.Format("{0} {1}", productcode, productversion);
					Version = productversion,
					SerialNumber = productnumber,
					ZkfpVersion = sValue,
					ZkFaceVersion = zkfaceversion,
					IsColor = iscolor,
					IsFace = isface,
					FaceMaxCount = faceMaxCount,
					FaceResidueCount = faceResidueCount,
					FaceUsedCount = faceUsedCount,
					MachineDatetime = new DateTime(dwYear, dwMonth, dwDay, dwHour, dwMin, dwSecond).ToString("yyyy-MM-dd HH:mm:ss"),
					LocalDatetime = now.ToString("yyyy-MM-dd HH:mm:ss")
				};

				return model;
			}
		}

		public List<CzkemRecord> GetRecord
		{
			get
			{
				var list = new List<CzkemRecord>();

				_czkem.ReadGeneralLogData(_czkem.MachineNumber);

				var idwVerifyMode = 1; //验证方式：0为密码验证，1为指纹验证，2为卡验证
				var idwInOutMode = 0; //考勤状态：0—Check-In   1—Check-Out  2—Break-Out  3—Break-In   4—OT-In   5—OT-Out
				var idwYear = 0; //考勤记录的日期和时间
				var idwMonth = 0; //考勤记录的日期和时间
				var idwDay = 0; //考勤记录的日期和时间
				var idwHour = 0; //考勤记录的日期和时间
				var idwMinute = 0; //考勤记录的日期和时间
				var idwSecond = 0; //考勤记录的日期和时间
				var idwWorkCode = 0; //记录的Workcode值
				var idwReserved = 0; //保留参数，无意义

				if (IsColorMachine() || IsFaceMachine())
				{
					string idwEnrollNo; //用户ID 号
					while (_czkem.SSR_GetGeneralLogData(_czkem.MachineNumber, out idwEnrollNo, out idwVerifyMode, out idwInOutMode, out idwYear, out idwMonth, out idwDay, out idwHour, out idwMinute, out idwSecond, ref idwWorkCode))
					{
						var datetime = new DateTime(idwYear, idwMonth, idwDay, idwHour, idwMinute, idwSecond);
						var model = new CzkemRecord
						{
							MachineId = MachineId,
							UserId = 0,
							UserNo = idwEnrollNo,
							VerifyMode = idwVerifyMode,
							InOutMode = idwInOutMode,
							WorkCode = idwWorkCode,
							Datetime = datetime
						};
						list.Add(model);
					}
				}
				else
				{
					var idwEnrollNumber = 0; //用户ID 号
					while (_czkem.GetGeneralExtLogData(_czkem.MachineNumber, ref idwEnrollNumber, ref idwVerifyMode, ref idwInOutMode, ref idwYear, ref idwMonth, ref idwDay, ref idwHour, ref idwMinute, ref idwSecond, ref idwWorkCode, ref idwReserved))
					{
						var datetime = new DateTime(idwYear, idwMonth, idwDay, idwHour, idwMinute, idwSecond);
						var model = new CzkemRecord
						{
							MachineId = MachineId,
							UserId = idwEnrollNumber,
							UserNo = string.Empty,
							VerifyMode = idwVerifyMode,
							InOutMode = idwInOutMode,
							WorkCode = idwWorkCode,
							Datetime = datetime
						};
						list.Add(model);
					}
				}

				return list;
			}
		}

		#endregion
	}
}