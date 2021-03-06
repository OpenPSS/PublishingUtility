using System.Collections.Generic;
using PublishingUtility.Properties;

namespace PublishingUtility
{
	public static class DrmError
	{
		public static Dictionary<ScePsmDrmStatus, string> dicDRMErrorMessageJa = new Dictionary<ScePsmDrmStatus, string>
		{
			{
				ScePsmDrmStatus.SCE_OK,
				"OK"
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_KDBG_ERROR_INVAL,
				"鍵作成時に不正な引数が渡されました。"
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_KDBG_ERROR_NOT_INITIALIZED,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_KDBG_ERROR_IN_USE,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_KDBG_ERROR_GENERATE_KEY,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_KDBG_ERROR_INVALID_PKCS12,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_KDBG_ERROR_LOAD_DLL,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_KDBG_ERROR_PARSE_TICKET,
				"資格確認に失敗しました。\nパブリッシャライセンスを取得しているか、ご確認ください。"
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_KDBG_ERROR_PUBLISHER_ENTITLEMENT_NOT_FOUND,
				"資格確認に失敗しました。\nパブリッシャライセンスを取得しているか、ご確認ください。"
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_KDBG_ERROR_KPUB_ALREADY_REGISTERED,
				"パブリッシャ鍵がすでに登録されています。"
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_KDBG_ERROR_CONNECT_PROXYSERVER,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_KDBG_ERROR_CONNECT_AUTHGW,
				"SCEサーバーへの接続に失敗しました。"
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_KDBG_ERROR_AUTHGW_HTTPS,
				"SCEサーバーとの通信に失敗しました。\nPCが正しくネットワークに接続されているか、ご確認ください。\n\n社内LAN環境などで実行している場合、プロキシサーバーの設定が正しく行われているかご確認ください。"
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_KDBG_ERROR_AUTHGW_HTTP_STATUS,
				"SCEサーバーとの通信に失敗しました。"
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_KDBG_ERROR_AUTHGW_HTTP_HEADER,
				"SCEサーバーから返されたHTTPヘッダーに必要な情報がありません。"
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_KDBG_ERROR_AUTHGW_HTTP_BODY,
				"SCEサーバーから返されたHTTPボディに必要な情報がありません。"
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_KDBG_ERROR_CONNECT_AUTHSERVER,
				"SCEサーバーへの接続に失敗しました。"
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_KDBG_ERROR_AUTHSERVER_HTTPS,
				"SCEサーバーとの通信に失敗しました。"
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_KDBG_ERROR_AUTHSERVER_HTTP_STATUS,
				"SCEサーバーとの通信に失敗しました。"
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_KDBG_ERROR_AUTHSERVER_HTTP_HEADER,
				"SCEサーバーから返されたHTTPヘッダーに必要な情報がありません。"
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_KDBG_ERROR_AUTHSERVER_HTTP_BODY,
				"SCEサーバーから返されたHTTPボディに必要な情報がありません。"
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_KDBG_ERROR_V1_SAVE,
				Resources.keyMismatchError
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_KDBG_ERROR_V1_LOAD,
				Resources.keyMismatchError
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_KDBG_ERROR_GET_CONSOLEID,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_KDBG_ERROR_READ_TKDBGLIST,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_KDBG_ERROR_IRREGULAR_FORMAT_TKDBGLIST,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_KDBG_ERROR_SAVE_TARGETKDBG,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_KDBG_ERROR_NO_TARGETKDBG,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_KDBG_ERROR_CRYPTO,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_KDBG_ERROR_SAVE_PROTECTEDKCONSOLECACHE,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_KDBG_ERROR_CREATEC1_CLOCK_NOT_INITIALIZED,
				Resources.keyMismatchError
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_KDBG_ERROR_CREATEC1_DELETE_ACTDAT,
				Resources.keyMismatchError
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_KDBG_ERROR_CREATEC1,
				Resources.keyMismatchError
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_KDBG_ERROR_VERIFYR1_IO,
				Resources.keyMismatchError
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_KDBG_ERROR_VERIFYR1_PERMISSION,
				Resources.keyMismatchError
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_KDBG_ERROR_VERIFYR1_BROKEN_DATA,
				Resources.keyMismatchError
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_KDBG_ERROR_VERIFYR1_IRREGULAR_FORMAT,
				Resources.keyMismatchError
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_KDBG_ERROR_VERIFYR1_NOT_ACTIVATED,
				Resources.keyMismatchError
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_KDBG_ERROR_VERIFYR1_BAD_ACTIVATION,
				Resources.keyMismatchError
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_KDBG_ERROR_VERIFYR1_CONSOLE_MISMATCH,
				Resources.keyMismatchError
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_KDBG_ERROR_VERIFYR1_GAMECARD_MISMATCH,
				Resources.keyMismatchError
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_KDBG_ERROR_VERIFYR1_ACCOUNT_MISMATCH,
				Resources.keyMismatchError
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_KDBG_ERROR_VERIFYR1_INVALID_MAC,
				Resources.keyMismatchError
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_KDBG_ERROR_VERIFYR1,
				Resources.keyMismatchError
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_AUTHGW_ERROR_SERVICE_END,
				"サービスが終了しています。"
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_AUTHGW_ERROR_SERVICE_UNAVAILABLE,
				"このサービスは一時的に利用できません。"
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_AUTHGW_ERROR_SERVICE_BUSY,
				"サービスがビジー状態です。"
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_AUTHGW_ERROR_MAINTENANCE,
				"サービスはメンテナンスにより利用不可能です。"
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_AUTHGW_ERROR_INVALID_DATA_LENGTH,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_AUTHGW_ERROR_UNSUPPORTED_VERSION,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_AUTHGW_ERROR_INVALID_CONTENT_TYPE,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_AUTHGW_ERROR_INVALID_SERVICE_ID,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_AUTHGW_ERROR_INVALID_CREDENTIALS,
				"ユーザー認証に失敗しました。\nSENアカウント、パスワードに誤りがないかご確認ください。"
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_AUTHGW_ERROR_INVALID_ENTITLEMENT_ID,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_AUTHGW_ERROR_INVALID_COSUMED_COUNT,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_AUTHGW_ERROR_INVALID_CONSOLE_ID,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_AUTHGW_ERROR_INVALID_SESSION_ID,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_AUTHGW_ERROR_CONSOLE_ID_SUSPENDED,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_AUTHGW_ERROR_INVALID_SERVICE_ENTITY,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_AUTHGW_ERROR_ACCOUNT_CLOSED,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_AUTHGW_ERROR_ACCOUNT_SUSPENDED,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_AUTHGW_ERROR_ACCOUNT_DEPRECATED,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_AUTHGW_ERROR_ACCOUNT_NEEDS_REACCEPTANCE,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_AUTHGW_ERROR_ACCOUNT_NEEDS_UPGRADE,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_AUTHGW_ERROR_ACCOUNT_NEEDS_REACCEPTANCE_SUB,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_AUTHGW_ERROR_ACCOUNT_NEEDS_RESET_PASSWORD,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_AUTHGW_ERROR_UNKNOWN_SERVER,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_AUTHGW_ERROR_UNKNOWN,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_AUTHSERVER_ERROR_BAD_REQUEST,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_AUTHSERVER_ERROR_INVALID_TICKET,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_AUTHSERVER_ERROR_INVALID_SIGNATURE,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_AUTHSERVER_ERROR_EXPIRED_TICKET,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_AUTHSERVER_ERROR_INVALID_JID,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_AUTHSERVER_ERROR_INTERNAL_SERVER_ERROR,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_AUTHSERVER_ERROR_BANNED_TICKET,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_AUTHSERVER_ERROR_BANNED_ID,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_AUTHSERVER_ERROR_MAINTENANCE,
				"サーバーは現在メンテナンス中です。"
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_AUTHSERVER_ERROR_BLACKBOX_ERROR,
				Resources.keyMismatchError
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_AUTHSERVER_ERROR_TOO_LONG_DELAY_RTC,
				"日時の設定でエラーが発生しました。\n\nPCおよび実機の日時がずれていないか、ご確認ください。"
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_AUTHSERVER_ERROR_TOO_MANY_CONSOLE,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_AUTHSERVER_ERROR_PUBLISHER_ENTITLEMENT_NOT_FOUND,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_AUTHSERVER_ERROR_NOT_VALID_PUBLISHER,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_AUTHSERVER_ERROR_DEVELOPER_NOT_REGISTERED,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_AUTHSERVER_ERROR_UNMATCH_KPUB_DIGEST,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_AUTHSERVER_ERROR_KPUB_ALREADY_REGISTERED,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_AUTHSERVER_ERROR_KPUB_NOT_REGISTERED,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_AUTHSERVER_ERROR_UNMATCH_ARCHIVE_DIGEST,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_AUTHSERVER_ERROR_UNKNOWN,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_KDBG_ERROR_FATAL,
				""
			}
		};

		public static Dictionary<ScePsmDrmStatus, string> dicDRMErrorMessageEn = new Dictionary<ScePsmDrmStatus, string>
		{
			{
				ScePsmDrmStatus.SCE_OK,
				"OK"
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_KDBG_ERROR_INVAL,
				"invalid arguments."
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_KDBG_ERROR_NOT_INITIALIZED,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_KDBG_ERROR_IN_USE,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_KDBG_ERROR_GENERATE_KEY,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_KDBG_ERROR_INVALID_PKCS12,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_KDBG_ERROR_LOAD_DLL,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_KDBG_ERROR_PARSE_TICKET,
				"Entitlement check failed. \nCheck whether you have obtained the publisher license."
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_KDBG_ERROR_PUBLISHER_ENTITLEMENT_NOT_FOUND,
				"Entitlement check failed. \nCheck whether you have obtained the publisher license."
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_KDBG_ERROR_KPUB_ALREADY_REGISTERED,
				"The Publisher Key is already registered."
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_KDBG_ERROR_CONNECT_PROXYSERVER,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_KDBG_ERROR_CONNECT_AUTHGW,
				"Connection to the SCE server failed."
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_KDBG_ERROR_AUTHGW_HTTPS,
				"HTTPS communication with the SCE server failed. \nCheck whether the PC is correctly connected to the network. \n\nWhen executing with a company intranet environment, for example, please check that proxy server settings are correctly made."
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_KDBG_ERROR_AUTHGW_HTTP_STATUS,
				"HTTPS communication with the SCE server failed (value other than 200)."
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_KDBG_ERROR_AUTHGW_HTTP_HEADER,
				"Required data is missing in the HTTP header returned from the SCE server."
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_KDBG_ERROR_AUTHGW_HTTP_BODY,
				"Required data is missing in the HTTP header returned from the SCE server."
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_KDBG_ERROR_CONNECT_AUTHSERVER,
				"Connection to the SCE server failed."
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_KDBG_ERROR_AUTHSERVER_HTTPS,
				"HTTPS communication with the SCE server failed."
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_KDBG_ERROR_AUTHSERVER_HTTP_STATUS,
				"HTTPS communication with the SCE server failed (value other than 200)."
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_KDBG_ERROR_AUTHSERVER_HTTP_HEADER,
				"Required data is missing in the HTTP header returned from the SCE server."
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_KDBG_ERROR_AUTHSERVER_HTTP_BODY,
				"Required data is missing in the HTTP body returned from the SCE server."
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_KDBG_ERROR_V1_SAVE,
				Resources.keyMismatchError
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_KDBG_ERROR_V1_LOAD,
				Resources.keyMismatchError
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_KDBG_ERROR_GET_CONSOLEID,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_KDBG_ERROR_READ_TKDBGLIST,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_KDBG_ERROR_IRREGULAR_FORMAT_TKDBGLIST,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_KDBG_ERROR_SAVE_TARGETKDBG,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_KDBG_ERROR_NO_TARGETKDBG,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_KDBG_ERROR_CRYPTO,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_KDBG_ERROR_SAVE_PROTECTEDKCONSOLECACHE,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_KDBG_ERROR_CREATEC1_CLOCK_NOT_INITIALIZED,
				Resources.keyMismatchError
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_KDBG_ERROR_CREATEC1_DELETE_ACTDAT,
				Resources.keyMismatchError
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_KDBG_ERROR_CREATEC1,
				Resources.keyMismatchError
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_KDBG_ERROR_VERIFYR1_IO,
				Resources.keyMismatchError
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_KDBG_ERROR_VERIFYR1_PERMISSION,
				Resources.keyMismatchError
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_KDBG_ERROR_VERIFYR1_BROKEN_DATA,
				Resources.keyMismatchError
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_KDBG_ERROR_VERIFYR1_IRREGULAR_FORMAT,
				Resources.keyMismatchError
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_KDBG_ERROR_VERIFYR1_NOT_ACTIVATED,
				Resources.keyMismatchError
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_KDBG_ERROR_VERIFYR1_BAD_ACTIVATION,
				Resources.keyMismatchError
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_KDBG_ERROR_VERIFYR1_CONSOLE_MISMATCH,
				Resources.keyMismatchError
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_KDBG_ERROR_VERIFYR1_GAMECARD_MISMATCH,
				Resources.keyMismatchError
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_KDBG_ERROR_VERIFYR1_ACCOUNT_MISMATCH,
				Resources.keyMismatchError
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_KDBG_ERROR_VERIFYR1_INVALID_MAC,
				Resources.keyMismatchError
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_KDBG_ERROR_VERIFYR1,
				Resources.keyMismatchError
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_AUTHGW_ERROR_SERVICE_END,
				"The service has ended."
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_AUTHGW_ERROR_SERVICE_UNAVAILABLE,
				"The service cannot be used temporarily."
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_AUTHGW_ERROR_SERVICE_BUSY,
				"The service is busy."
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_AUTHGW_ERROR_MAINTENANCE,
				"The service is under maintenance and cannot be used."
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_AUTHGW_ERROR_INVALID_DATA_LENGTH,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_AUTHGW_ERROR_UNSUPPORTED_VERSION,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_AUTHGW_ERROR_INVALID_CONTENT_TYPE,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_AUTHGW_ERROR_INVALID_SERVICE_ID,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_AUTHGW_ERROR_INVALID_CREDENTIALS,
				"Invalid user’s credentials.\nCheck Sony Entertainment Network account and password."
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_AUTHGW_ERROR_INVALID_ENTITLEMENT_ID,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_AUTHGW_ERROR_INVALID_COSUMED_COUNT,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_AUTHGW_ERROR_INVALID_CONSOLE_ID,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_AUTHGW_ERROR_INVALID_SESSION_ID,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_AUTHGW_ERROR_CONSOLE_ID_SUSPENDED,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_AUTHGW_ERROR_INVALID_SERVICE_ENTITY,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_AUTHGW_ERROR_ACCOUNT_CLOSED,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_AUTHGW_ERROR_ACCOUNT_SUSPENDED,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_AUTHGW_ERROR_ACCOUNT_DEPRECATED,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_AUTHGW_ERROR_ACCOUNT_NEEDS_REACCEPTANCE,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_AUTHGW_ERROR_ACCOUNT_NEEDS_UPGRADE,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_AUTHGW_ERROR_ACCOUNT_NEEDS_REACCEPTANCE_SUB,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_AUTHGW_ERROR_ACCOUNT_NEEDS_RESET_PASSWORD,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_AUTHGW_ERROR_UNKNOWN_SERVER,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_AUTHGW_ERROR_UNKNOWN,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_AUTHSERVER_ERROR_BAD_REQUEST,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_AUTHSERVER_ERROR_INVALID_TICKET,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_AUTHSERVER_ERROR_INVALID_SIGNATURE,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_AUTHSERVER_ERROR_EXPIRED_TICKET,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_AUTHSERVER_ERROR_INVALID_JID,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_AUTHSERVER_ERROR_INTERNAL_SERVER_ERROR,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_AUTHSERVER_ERROR_BANNED_TICKET,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_AUTHSERVER_ERROR_BANNED_ID,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_AUTHSERVER_ERROR_MAINTENANCE,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_AUTHSERVER_ERROR_BLACKBOX_ERROR,
				Resources.keyMismatchError
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_AUTHSERVER_ERROR_TOO_LONG_DELAY_RTC,
				"An error occurred in date settings. \n\nCheck that the date on the PC and on the actual device are not misaligned."
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_AUTHSERVER_ERROR_TOO_MANY_CONSOLE,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_AUTHSERVER_ERROR_PUBLISHER_ENTITLEMENT_NOT_FOUND,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_AUTHSERVER_ERROR_NOT_VALID_PUBLISHER,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_AUTHSERVER_ERROR_DEVELOPER_NOT_REGISTERED,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_AUTHSERVER_ERROR_UNMATCH_KPUB_DIGEST,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_AUTHSERVER_ERROR_KPUB_ALREADY_REGISTERED,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_AUTHSERVER_ERROR_KPUB_NOT_REGISTERED,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_AUTHSERVER_ERROR_UNMATCH_ARCHIVE_DIGEST,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_AUTHSERVER_ERROR_UNKNOWN,
				""
			},
			{
				ScePsmDrmStatus.SCE_PSM_DRM_KDBG_ERROR_FATAL,
				""
			}
		};

		public static string GetErrorMessage(ScePsmDrmStatus errCode)
		{
			string text = "";
			string text2;
			if (Program.appConfigData.PcLocale == "ja-JP")
			{
				text2 = $"システムエラー: ({(int)errCode}), 0x{errCode:X}";
				if (dicDRMErrorMessageJa.ContainsKey(errCode))
				{
					text = dicDRMErrorMessageJa[errCode];
				}
			}
			else
			{
				text2 = $"System error: ({(int)errCode}), 0x{errCode:X}";
				if (dicDRMErrorMessageEn.ContainsKey(errCode))
				{
					text = dicDRMErrorMessageEn[errCode];
				}
			}
			return text2 + "\n\n" + text;
		}

		public static string GetErrorCode(ScePsmDrmStatus errCode)
		{
			return $"({(int)errCode}), 0x{errCode:X}";
		}
	}
}
