using System;

namespace Netlover.Czkem.Models
{
	public class CzkemUserTemp
	{
		public int Index { get; set; }
		public string Data { get; set; }
		public int Length { get; set; }
		public int? Flag { get; set; }
	}

	public class CzkemUserFace
	{
		public string Data { get; set; }
		public int Length { get; set; }
	}

	public class CzkemUserInfo
	{
		public int UserId { get; set; }
		public string Fullname { get; set; }
		public string Password { get; set; }
		public int Privilege { get; set; }
		public bool Enabled { get; set; }
	}

	public class CzkemUserInfoEx
	{
		public string UserId { get; set; }
		public string Fullname { get; set; }
		public string Password { get; set; }
		public int Privilege { get; set; }
		public bool Enabled { get; set; }
	}

	public class CzkemUserLog
	{
		public int UserId { get; set; }
		public int Verify { get; set; }
		public int InOut { get; set; }
		public DateTime Datetime { get; set; }
	}

	public class CzkemUserLogEx
	{
		public string UserId { get; set; }
		public int Verify { get; set; }
		public int InOut { get; set; }
		public DateTime Datetime { get; set; }
	}

	public class CzkemStatusInfo
	{
		public int MachineId { get; set; }
		public int AdminCount { get; set; }
		public int ActionCount { get; set; }
		public int PassWordCount { get; set; }
		public int FingerMaxCount { get; set; }
		public int FingerResidueCount { get; set; }
		public int FingerUsedCount { get; set; }
		public int PeopleMaxCount { get; set; }
		public int PeopleResidueCount { get; set; }
		public int PeopleUsedCount { get; set; }
		public int LogMaxCount { get; set; }
		public int LogResidueCount { get; set; }
		public int LogUsedCount { get; set; }
		public bool MachineStatus { get; set; }
		public string MachineStatusName { get; set; }
		public string MachineDatetime { get; set; }
		public string LocalDatetime { get; set; }
		public string Model { get; set; }
		public string Version { get; set; }
		public string ZkfpVersion { get; set; }
		public string ZkFaceVersion { get; set; }
		public string SerialNumber { get; set; }
		public bool IsColor { get; set; }
		public bool IsFace { get; set; }
		public int FaceMaxCount { get; set; }
		public int FaceResidueCount { get; set; }
		public int FaceUsedCount { get; set; }
	}

	public class CzkemRecord
	{
		public int MachineId { get; set; }
		public int UserId { get; set; }
		public string UserNo { get; set; }
		public int VerifyMode { get; set; }//0:password，1:finger，2:card
		public int InOutMode { get; set; }//0—Check-In   1—Check-Out  2—Break-Out  3—Break-In   4—OT-In   5—OT-Out
		public int WorkCode { get; set; }
		public DateTime Datetime { get; set; }
	}
}
