namespace PublishingUtility
{
	public enum ScePsmDrmStatus
	{
		SCE_OK = 0,
		SCE_PSM_DRM_KDBG_ERROR_INVAL = -2138107647,
		SCE_PSM_DRM_KDBG_ERROR_NOT_INITIALIZED = -2138107646,
		SCE_PSM_DRM_KDBG_ERROR_IN_USE = -2138107645,
		SCE_PSM_DRM_KDBG_ERROR_GENERATE_KEY = -2138107644,
		SCE_PSM_DRM_KDBG_ERROR_INVALID_PKCS12 = -2138107643,
		SCE_PSM_DRM_KDBG_ERROR_LOAD_DLL = -2138107642,
		SCE_PSM_DRM_KDBG_ERROR_PARSE_TICKET = -2138107641,
		SCE_PSM_DRM_KDBG_ERROR_PUBLISHER_ENTITLEMENT_NOT_FOUND = -2138107640,
		SCE_PSM_DRM_KDBG_ERROR_KPUB_ALREADY_REGISTERED = -2138107639,
		SCE_PSM_DRM_KDBG_ERROR_CONNECT_PROXYSERVER = -2138107616,
		SCE_PSM_DRM_KDBG_ERROR_CONNECT_AUTHGW = -2138107615,
		SCE_PSM_DRM_KDBG_ERROR_AUTHGW_HTTPS = -2138107614,
		SCE_PSM_DRM_KDBG_ERROR_AUTHGW_HTTP_STATUS = -2138107613,
		SCE_PSM_DRM_KDBG_ERROR_AUTHGW_HTTP_HEADER = -2138107612,
		SCE_PSM_DRM_KDBG_ERROR_AUTHGW_HTTP_BODY = -2138107611,
		SCE_PSM_DRM_KDBG_ERROR_CONNECT_AUTHSERVER = -2138107610,
		SCE_PSM_DRM_KDBG_ERROR_AUTHSERVER_HTTPS = -2138107609,
		SCE_PSM_DRM_KDBG_ERROR_AUTHSERVER_HTTP_STATUS = -2138107608,
		SCE_PSM_DRM_KDBG_ERROR_AUTHSERVER_HTTP_HEADER = -2138107607,
		SCE_PSM_DRM_KDBG_ERROR_AUTHSERVER_HTTP_BODY = -2138107606,
		SCE_PSM_DRM_KDBG_ERROR_V1_SAVE = -2138107600,
		SCE_PSM_DRM_KDBG_ERROR_V1_LOAD = -2138107599,
		SCE_PSM_DRM_KDBG_ERROR_GET_CONSOLEID = -2138107598,
		SCE_PSM_DRM_KDBG_ERROR_READ_TKDBGLIST = -2138107597,
		SCE_PSM_DRM_KDBG_ERROR_IRREGULAR_FORMAT_TKDBGLIST = -2138107596,
		SCE_PSM_DRM_KDBG_ERROR_SAVE_TARGETKDBG = -2138107595,
		SCE_PSM_DRM_KDBG_ERROR_NO_TARGETKDBG = -2138107594,
		SCE_PSM_DRM_KDBG_ERROR_CRYPTO = -2138107593,
		SCE_PSM_DRM_KDBG_ERROR_SAVE_PROTECTEDKCONSOLECACHE = -2138107592,
		SCE_PSM_DRM_KDBG_ERROR_CREATEC1_CLOCK_NOT_INITIALIZED = -2138107584,
		SCE_PSM_DRM_KDBG_ERROR_CREATEC1_DELETE_ACTDAT = -2138107583,
		SCE_PSM_DRM_KDBG_ERROR_CREATEC1 = -2138107582,
		SCE_PSM_DRM_KDBG_ERROR_VERIFYR1_IO = -2138107568,
		SCE_PSM_DRM_KDBG_ERROR_VERIFYR1_PERMISSION = -2138107567,
		SCE_PSM_DRM_KDBG_ERROR_VERIFYR1_BROKEN_DATA = -2138107566,
		SCE_PSM_DRM_KDBG_ERROR_VERIFYR1_IRREGULAR_FORMAT = -2138107565,
		SCE_PSM_DRM_KDBG_ERROR_VERIFYR1_NOT_ACTIVATED = -2138107564,
		SCE_PSM_DRM_KDBG_ERROR_VERIFYR1_BAD_ACTIVATION = -2138107563,
		SCE_PSM_DRM_KDBG_ERROR_VERIFYR1_CONSOLE_MISMATCH = -2138107562,
		SCE_PSM_DRM_KDBG_ERROR_VERIFYR1_GAMECARD_MISMATCH = -2138107561,
		SCE_PSM_DRM_KDBG_ERROR_VERIFYR1_ACCOUNT_MISMATCH = -2138107560,
		SCE_PSM_DRM_KDBG_ERROR_VERIFYR1_INVALID_MAC = -2138107559,
		SCE_PSM_DRM_KDBG_ERROR_VERIFYR1 = -2138107558,
		SCE_PSM_DRM_AUTHGW_ERROR_SERVICE_END = -2138107552,
		SCE_PSM_DRM_AUTHGW_ERROR_SERVICE_UNAVAILABLE = -2138107551,
		SCE_PSM_DRM_AUTHGW_ERROR_SERVICE_BUSY = -2138107550,
		SCE_PSM_DRM_AUTHGW_ERROR_MAINTENANCE = -2138107549,
		SCE_PSM_DRM_AUTHGW_ERROR_INVALID_DATA_LENGTH = -2138107548,
		SCE_PSM_DRM_AUTHGW_ERROR_UNSUPPORTED_VERSION = -2138107547,
		SCE_PSM_DRM_AUTHGW_ERROR_INVALID_CONTENT_TYPE = -2138107546,
		SCE_PSM_DRM_AUTHGW_ERROR_INVALID_SERVICE_ID = -2138107545,
		SCE_PSM_DRM_AUTHGW_ERROR_INVALID_CREDENTIALS = -2138107544,
		SCE_PSM_DRM_AUTHGW_ERROR_INVALID_ENTITLEMENT_ID = -2138107543,
		SCE_PSM_DRM_AUTHGW_ERROR_INVALID_COSUMED_COUNT = -2138107542,
		SCE_PSM_DRM_AUTHGW_ERROR_INVALID_CONSOLE_ID = -2138107541,
		SCE_PSM_DRM_AUTHGW_ERROR_INVALID_SESSION_ID = -2138107540,
		SCE_PSM_DRM_AUTHGW_ERROR_CONSOLE_ID_SUSPENDED = -2138107539,
		SCE_PSM_DRM_AUTHGW_ERROR_INVALID_SERVICE_ENTITY = -2138107538,
		SCE_PSM_DRM_AUTHGW_ERROR_ACCOUNT_CLOSED = -2138107537,
		SCE_PSM_DRM_AUTHGW_ERROR_ACCOUNT_SUSPENDED = -2138107536,
		SCE_PSM_DRM_AUTHGW_ERROR_ACCOUNT_DEPRECATED = -2138107535,
		SCE_PSM_DRM_AUTHGW_ERROR_ACCOUNT_NEEDS_REACCEPTANCE = -2138107534,
		SCE_PSM_DRM_AUTHGW_ERROR_ACCOUNT_NEEDS_UPGRADE = -2138107533,
		SCE_PSM_DRM_AUTHGW_ERROR_ACCOUNT_NEEDS_REACCEPTANCE_SUB = -2138107532,
		SCE_PSM_DRM_AUTHGW_ERROR_ACCOUNT_NEEDS_RESET_PASSWORD = -2138107531,
		SCE_PSM_DRM_AUTHGW_ERROR_UNKNOWN_SERVER = -2138107530,
		SCE_PSM_DRM_AUTHGW_ERROR_UNKNOWN = -2138107521,
		SCE_PSM_DRM_AUTHSERVER_ERROR_BAD_REQUEST = -2138107520,
		SCE_PSM_DRM_AUTHSERVER_ERROR_INVALID_TICKET = -2138107519,
		SCE_PSM_DRM_AUTHSERVER_ERROR_INVALID_SIGNATURE = -2138107518,
		SCE_PSM_DRM_AUTHSERVER_ERROR_EXPIRED_TICKET = -2138107517,
		SCE_PSM_DRM_AUTHSERVER_ERROR_INVALID_JID = -2138107516,
		SCE_PSM_DRM_AUTHSERVER_ERROR_INTERNAL_SERVER_ERROR = -2138107515,
		SCE_PSM_DRM_AUTHSERVER_ERROR_BANNED_TICKET = -2138107514,
		SCE_PSM_DRM_AUTHSERVER_ERROR_BANNED_ID = -2138107513,
		SCE_PSM_DRM_AUTHSERVER_ERROR_MAINTENANCE = -2138107512,
		SCE_PSM_DRM_AUTHSERVER_ERROR_BLACKBOX_ERROR = -2138107511,
		SCE_PSM_DRM_AUTHSERVER_ERROR_TOO_LONG_DELAY_RTC = -2138107510,
		SCE_PSM_DRM_AUTHSERVER_ERROR_TOO_MANY_CONSOLE = -2138107509,
		SCE_PSM_DRM_AUTHSERVER_ERROR_PUBLISHER_ENTITLEMENT_NOT_FOUND = -2138107508,
		SCE_PSM_DRM_AUTHSERVER_ERROR_NOT_VALID_PUBLISHER = -2138107507,
		SCE_PSM_DRM_AUTHSERVER_ERROR_DEVELOPER_NOT_REGISTERED = -2138107506,
		SCE_PSM_DRM_AUTHSERVER_ERROR_UNMATCH_KPUB_DIGEST = -2138107505,
		SCE_PSM_DRM_AUTHSERVER_ERROR_KPUB_ALREADY_REGISTERED = -2138107504,
		SCE_PSM_DRM_AUTHSERVER_ERROR_KPUB_NOT_REGISTERED = -2138107503,
		SCE_PSM_DRM_AUTHSERVER_ERROR_UNMATCH_ARCHIVE_DIGEST = -2138107502,
		SCE_PSM_DRM_AUTHSERVER_ERROR_UNKNOWN = -2138107489,
		SCE_PSM_DRM_KDBG_ERROR_FATAL = -2138107393
	}
}