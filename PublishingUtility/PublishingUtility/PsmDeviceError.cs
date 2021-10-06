using System;
using System.Collections.Generic;
using System.Windows.Forms;
using PublishingUtility.Properties;

namespace PublishingUtility
{
	public static class PsmDeviceError
	{
		public static Dictionary<PsmDeviceStatus, string> dicPsmDeviceErrorMessageJa = new Dictionary<PsmDeviceStatus, string>
		{
			{
				PsmDeviceStatus.SCE_PSM_DEVICE_OK,
				"OK"
			},
			{
				PsmDeviceStatus.SCE_PSM_DEVICE_NO_CONNECTION,
				"デバイスとの接続が確立していません。\nPS Vitaの場合、DevAssistantを起動しておく必要があります。"
			},
			{
				PsmDeviceStatus.SCE_PSM_DEVICE_INVALID_PACKAGE,
				"マスターパッケージが不正です。"
			},
			{
				PsmDeviceStatus.SCE_PSM_DEVICE_INVALID_APPID,
				"アプリケーションIDが不正です。"
			},
			{
				PsmDeviceStatus.SCE_PSM_DEVICE_INVALID_FILEPATH,
				"ファイルパスが不正です。"
			},
			{
				PsmDeviceStatus.SCE_PSM_DEVICE_CANNOT_ACCESS_STORAGE,
				"デバイスのストレージにアクセスできません。"
			},
			{
				PsmDeviceStatus.SCE_PSM_DEVICE_STORAGE_FULL,
				"デバイスのストレージ容量が足りません。"
			},
			{
				PsmDeviceStatus.SCE_PSM_DEVICE_CONNECT_ERROR,
				"デバイスに接続できません。"
			},
			{
				PsmDeviceStatus.SCE_PSM_DEVICE_CREATE_PACKAGE,
				"パッケージを作成できません。"
			},
			{
				PsmDeviceStatus.SCE_PSM_DEVICE_CONNECTED_DEVICE,
				"デバイスに接続済みです。"
			},
			{
				PsmDeviceStatus.SCE_PSM_DEVICE_TIMEOUT,
				"接続中にタイムアウトしました。"
			},
			{
				PsmDeviceStatus.SCE_PSM_DEVICE_NO_LAUNCH_TARGET,
				"デバイスが起動していません。"
			},
			{
				PsmDeviceStatus.SCE_PSM_DEVICE_VERSION_HOST,
				"PlayStation Mobile SDK、または PSM Studio を更新して下さい。"
			},
			{
				PsmDeviceStatus.SCE_PSM_DEVICE_VERSION_TARGET,
				"PlayStation Mobile Development Assistant を更新して下さい。"
			},
			{
				PsmDeviceStatus.SCE_PSM_DEVICE_INVALID_PACKET,
				"パケット情報が不正です。"
			},
			{
				PsmDeviceStatus.SCE_PSM_DEVICE_TARGET_LAUNCHED,
				"ターゲットデバイス上で既にアプリケーションが起動中です。"
			},
			{
				PsmDeviceStatus.SCE_PSM_DEVICE_PSMDEVICE_ERROR,
				"PsmDeviceのプロセスエラーです。"
			},
			{
				PsmDeviceStatus.SCE_PSM_DEVICE_PSMDEVICE_OPTION,
				"PsmDeviceのオプションエラーです。"
			},
			{
				PsmDeviceStatus.SCE_PSM_DEVICE_INSTALL_ASSISTANT,
				"PlayStation Mobile Development Assistant のインストールに失敗しました。"
			},
			{
				PsmDeviceStatus.SCE_PSM_DEVICE_PACKAGE_NOTFOUND,
				"パッケージファイルが見つかりません。"
			},
			{
				PsmDeviceStatus.SCE_PSM_DRM_KDBG_ERROR_V1_LOAD,
				Resources.keyMismatchError
			},
			{
				PsmDeviceStatus.SCE_PSM_DRM_KDBG_ERROR_DELETE_INTERNAL_DATA,
				Resources.keyMismatchError
			},
			{
				PsmDeviceStatus.SCE_PSM_DRM_KDBG_ERROR_INTERNAL_DATA_REMOVE,
				Resources.keyMismatchError
			},
			{
				PsmDeviceStatus.SCE_PSM_DRM_KDBG_ERROR_VERIFYR1_INVALID_MAC,
				Resources.keyMismatchError
			}
		};

		public static Dictionary<PsmDeviceStatus, string> dicPsmDeviceErrorMessageEn = new Dictionary<PsmDeviceStatus, string>
		{
			{
				PsmDeviceStatus.SCE_PSM_DEVICE_OK,
				"OK"
			},
			{
				PsmDeviceStatus.SCE_PSM_DEVICE_NO_CONNECTION,
				"Connection with a device has not been established. \nOn Vita, DevAssistant must be running."
			},
			{
				PsmDeviceStatus.SCE_PSM_DEVICE_INVALID_PACKAGE,
				"Invalid Master Package."
			},
			{
				PsmDeviceStatus.SCE_PSM_DEVICE_INVALID_APPID,
				"Invalid Application ID."
			},
			{
				PsmDeviceStatus.SCE_PSM_DEVICE_INVALID_FILEPATH,
				"Invalid file path."
			},
			{
				PsmDeviceStatus.SCE_PSM_DEVICE_CANNOT_ACCESS_STORAGE,
				"Cannot access device storage."
			},
			{
				PsmDeviceStatus.SCE_PSM_DEVICE_STORAGE_FULL,
				"Device storage is full."
			},
			{
				PsmDeviceStatus.SCE_PSM_DEVICE_CONNECT_ERROR,
				"Cannot connect to device."
			},
			{
				PsmDeviceStatus.SCE_PSM_DEVICE_CREATE_PACKAGE,
				"Cannot create package."
			},
			{
				PsmDeviceStatus.SCE_PSM_DEVICE_CONNECTED_DEVICE,
				"Connected a device."
			},
			{
				PsmDeviceStatus.SCE_PSM_DEVICE_TIMEOUT,
				"Timed out during connection."
			},
			{
				PsmDeviceStatus.SCE_PSM_DEVICE_NO_LAUNCH_TARGET,
				"Device has not started."
			},
			{
				PsmDeviceStatus.SCE_PSM_DEVICE_VERSION_HOST,
				"Please update the PlayStation Mobile SDK or the PSM Studio."
			},
			{
				PsmDeviceStatus.SCE_PSM_DEVICE_VERSION_TARGET,
				"Please update the PlayStation Mobile Development Assistant."
			},
			{
				PsmDeviceStatus.SCE_PSM_DEVICE_INVALID_PACKET,
				"Invalid packet data."
			},
			{
				PsmDeviceStatus.SCE_PSM_DEVICE_TARGET_LAUNCHED,
				"Application is starting on a device."
			},
			{
				PsmDeviceStatus.SCE_PSM_DEVICE_PSMDEVICE_ERROR,
				"Process error of PsmDevice."
			},
			{
				PsmDeviceStatus.SCE_PSM_DEVICE_PSMDEVICE_OPTION,
				"Option error of PsmDevice."
			},
			{
				PsmDeviceStatus.SCE_PSM_DEVICE_INSTALL_ASSISTANT,
				"Cannot install the PlayStation Mobile Development Assistant."
			},
			{
				PsmDeviceStatus.SCE_PSM_DEVICE_PACKAGE_NOTFOUND,
				"Package file is not found."
			},
			{
				PsmDeviceStatus.SCE_PSM_DRM_KDBG_ERROR_V1_LOAD,
				Resources.keyMismatchError
			},
			{
				PsmDeviceStatus.SCE_PSM_DRM_KDBG_ERROR_DELETE_INTERNAL_DATA,
				Resources.keyMismatchError
			},
			{
				PsmDeviceStatus.SCE_PSM_DRM_KDBG_ERROR_INTERNAL_DATA_REMOVE,
				Resources.keyMismatchError
			},
			{
				PsmDeviceStatus.SCE_PSM_DRM_KDBG_ERROR_VERIFYR1_INVALID_MAC,
				Resources.keyMismatchError
			}
		};

		public static void PsmDeviceMessage(PsmDeviceStatus errCode, string txt)
		{
			string text = "";
			MessageBox.Show(string.Format(arg2: (Program.appConfigData.PcLocale == "ja-JP") ? ((!dicPsmDeviceErrorMessageJa.ContainsKey(errCode)) ? "不明なエラーコードです。" : dicPsmDeviceErrorMessageJa[errCode]) : ((!dicPsmDeviceErrorMessageEn.ContainsKey(errCode)) ? "Unknown Error Message." : dicPsmDeviceErrorMessageEn[errCode]), format: "{0}\n\n({1}), 0x{1:X}\n\n{2}", arg0: txt, arg1: (int)errCode), "Publishing Utility", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
		}

		public static string OptionList(int res, Guid guid, string op1 = "x", string op2 = "x", string op3 = "x", string op4 = "x", string op5 = "x")
		{
			string text = guid.ToString();
			int num = ((text.Length == 0) ? 1 : 0) | ((op1.Length == 0) ? 2 : 0) | ((op2.Length == 0) ? 4 : 0) | ((op3.Length == 0) ? 8 : 0) | ((op4.Length == 0) ? 16 : 0) | ((op5.Length == 0) ? 32 : 0);
			if (res != -1)
			{
				num = 0;
			}
			if (num != 0)
			{
				return $" (0x{num:X})";
			}
			return "";
		}
	}
}
