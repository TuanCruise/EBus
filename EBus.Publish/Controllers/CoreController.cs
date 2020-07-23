using Core.DataAccess;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using WebCore.Common;
using WebCore.Entities;
using WebCore.Utils;

namespace EBus.Core.Controllers
{
    public class CoreController : BaseController
    {
        public CoreController(IConfiguration configuration) : base(configuration)
        {
            Session = new Session();
        }

        public Session Session { get; set; }
        public void CreateUserSession(out Session session, string userName, string password, string clientIP, string clientMacAddress)
        {
            session = null;
            try
            {
                session = null;
                CreateUserSession(out session, userName, password, clientIP, null, clientMacAddress);
            }

            catch (Exception ex)
            {
                //throw ErrorUtils.CreateError(ex);
            }
        }
        public void CreateUserSession(out Session session, string userName, string password, string clientIP, string dnsName, string clientMacAddress)
        {
            session = null;
            try
            {

                session = SQLHelper.ExecuteStoreProcedure<Session>(ConnectionString, null, SYSTEM_STORE_PROCEDURES.CREATE_NEW_SESSION, userName, password, clientIP)[0];

                session.SessionKey = CommonUtils.MD5Standard(session.SessionID.ToString());
                session.ClientIP = clientIP;
                session.DNSName = dnsName;
                session.ClientMacAddress = clientMacAddress;
                //SQLHelper.ExecuteStoreProcedure(ConnectionString, new Session(), SYSTEM_STORE_PROCEDURES.UPDATE_SESSION_INFO,
                //    session.SessionID,
                //    session.SessionKey,
                //    session.ClientIP,
                //    session.DNSName,
                //    session.ClientMacAddress);
            }
            catch (Exception ex)
            {
                //throw ErrorUtils.CreateError(ex);
            }
        }


        public void InitializeSessionID(string sessionID)
        {
            if (sessionID != null)
            {
                var sessions = SQLHelper.ExecuteStoreProcedure<Session>(ConnectionString, null, SYSTEM_STORE_PROCEDURES.GET_SESSION_INFO, sessionID);

                if (sessions.Count == 1)
                {
                    Session = sessions[0];
                    if (Session.SessionStatus == WebCore.CODES.SESSIONS.SESSIONSTATUS.SESSION_TERMINATED)
                    {
                        if (Session.Username != Session.TerminatedUsername)
                        {
                            //throw ErrorUtils.CreateErrorWithSubMessage(ERR_SYSTEM.ERR_SYSTEM_SESSION_TERMINATED_BY_ADMIN,
                            //    Session.TerminatedUsername + ": " + Session.Description);
                        }
                        //throw ErrorUtils.CreateError(ERR_SYSTEM.ERR_SYSTEM_SESSION_TERMINATED_BY_SELF);
                    }

                    if (Session.SessionStatus == WebCore.CODES.SESSIONS.SESSIONSTATUS.SESSION_TIMEOUT)
                    {
                        //throw ErrorUtils.CreateError(ERR_SYSTEM.ERR_SYSTEM_SESSION_TIMEOUT);
                    }
                }
                else
                {
                    //throw ErrorUtils.CreateError(ERR_SYSTEM.ERR_SYSTEM_SESSION_NOT_EXISTS_OR_DUPLICATE);
                }
            }
        }
        public List<ModuleInfo> BuildModulesInfo()
        {
            try
            {
                var moduleInfos = new List<ModuleInfo>();
                //moduleInfos.AddRange(SQLHelper.ExecuteStoreProcedure<ModuleInfo>(ConnectionString, SYSTEM_STORE_PROCEDURES.LIST_STATIC_MODULE).ToArray());
                //moduleInfos.AddRange(SQLHelper.ExecuteStoreProcedure<ModuleInfo>(ConnectionString, SYSTEM_STORE_PROCEDURES.LIST_BATCH_MODULE).ToArray());
                //moduleInfos.AddRange(SQLHelper.ExecuteStoreProcedure<StatisticsModuleInfo>(ConnectionString, SYSTEM_STORE_PROCEDURES.LIST_STATISTICS_MODULE).ToArray());
                //moduleInfos.AddRange(SQLHelper.ExecuteStoreProcedure<MaintainModuleInfo>(ConnectionString, SYSTEM_STORE_PROCEDURES.LIST_MAINTAIN_MODULE).ToArray());
                //moduleInfos.AddRange(SQLHelper.ExecuteStoreProcedure<ChartModuleInfo>(ConnectionString, SYSTEM_STORE_PROCEDURES.LIST_CHART_MODULE).ToArray());
                moduleInfos.AddRange(SQLHelper.ExecuteStoreProcedure<SearchModuleInfo>(ConnectionString, SYSTEM_STORE_PROCEDURES.LIST_SEARCHMASTER_MODULE).ToArray());
                moduleInfos.AddRange(SQLHelper.ExecuteStoreProcedure<ModESBInfo>(ConnectionString, SYSTEM_STORE_PROCEDURES.LIST_MODESB_MODULE).ToArray());
                //moduleInfos.AddRange(SQLHelper.ExecuteStoreProcedure<SwitchModuleInfo>(ConnectionString, SYSTEM_STORE_PROCEDURES.LIST_SWITCH_MODULE).ToArray());
                //moduleInfos.AddRange(SQLHelper.ExecuteStoreProcedure<ImportModuleInfo>(ConnectionString, SYSTEM_STORE_PROCEDURES.LIST_IMPORT_MODULE).ToArray());
                //moduleInfos.AddRange(SQLHelper.ExecuteStoreProcedure<ExecProcModuleInfo>(ConnectionString, SYSTEM_STORE_PROCEDURES.LIST_EXECUTEPROC_MODULE).ToArray());
                //moduleInfos.AddRange(SQLHelper.ExecuteStoreProcedure<AlertModuleInfo>(ConnectionString, SYSTEM_STORE_PROCEDURES.LIST_ALERT_MODULE).ToArray());
                //moduleInfos.AddRange(SQLHelper.ExecuteStoreProcedure<ReportModuleInfo>(ConnectionString, SYSTEM_STORE_PROCEDURES.LIST_REPORT_MODULE).ToArray());
                //moduleInfos.AddRange(SQLHelper.ExecuteStoreProcedure<ModuleInfo>(ConnectionString, SYSTEM_STORE_PROCEDURES.LIST_TREE_MODULE).ToArray());
                //moduleInfos.AddRange(SQLHelper.ExecuteStoreProcedure<ModuleInfo>(ConnectionString, SYSTEM_STORE_PROCEDURES.LIST_EXP_MODULE).ToArray());
                //moduleInfos.AddRange(SQLHelper.ExecuteStoreProcedure<MaintainModuleInfo>(ConnectionString, SYSTEM_STORE_PROCEDURES.LIST_WORKFLOW_MODULE).ToArray());
                //moduleInfos.AddRange(SQLHelper.ExecuteStoreProcedure<DashboardInfo>(ConnectionString, SYSTEM_STORE_PROCEDURES.LIST_DASHBOARD_MODULE).ToArray());

                return moduleInfos;
            }

            catch (Exception ex)
            {
                throw ErrorUtils.CreateError(ex);
            }
        }

        public List<ModuleFieldInfo> BuildModuleFieldsInfo()
        {
            try
            {
                return SQLHelper.ExecuteStoreProcedure<ModuleFieldInfo>(ConnectionString, SYSTEM_STORE_PROCEDURES.LIST_FIELD_INFO);
            }
            catch (Exception ex)
            {
                throw ErrorUtils.CreateError(ex);
            }
        }

        public void ForceLoadModule(
           out List<ModuleInfo> modulesInfo,
           out List<ModuleFieldInfo> fieldsInfo,
           out List<ButtonInfo> buttonsInfo,
           out List<ButtonParamInfo> buttonParamsInfo,
           out List<LanguageInfo> languageInfo,
           out List<OracleParam> oracleParamsInfo,
           string moduleID)
        {
            modulesInfo = null;
            fieldsInfo = null;
            buttonsInfo = null;
            buttonParamsInfo = null;
            languageInfo = null;
            oracleParamsInfo = null;
            try
            {
                modulesInfo = new List<ModuleInfo>();
                modulesInfo.AddRange(SQLHelper.ExecuteStoreProcedure<ModuleInfo>(ConnectionString, SYSTEM_STORE_PROCEDURES.GET_STATIC_MODULE, moduleID).ToArray());
                modulesInfo.AddRange(SQLHelper.ExecuteStoreProcedure<MaintainModuleInfo>(ConnectionString, SYSTEM_STORE_PROCEDURES.GET_MAINTAIN_MODULE, moduleID).ToArray());
                fieldsInfo = SQLHelper.ExecuteStoreProcedure<ModuleFieldInfo>(ConnectionString, SYSTEM_STORE_PROCEDURES.LIST_FIELD_INFO_BY_MODID, moduleID);
                buttonsInfo = SQLHelper.ExecuteStoreProcedure<ButtonInfo>(ConnectionString, SYSTEM_STORE_PROCEDURES.LIST_BUTTON_BY_MODID, moduleID);
                buttonParamsInfo = SQLHelper.ExecuteStoreProcedure<ButtonParamInfo>(ConnectionString, SYSTEM_STORE_PROCEDURES.LIST_BUTTON_PARAM_BY_MODID, moduleID);
                languageInfo = SQLHelper.ExecuteStoreProcedure<LanguageInfo>(ConnectionString, SYSTEM_STORE_PROCEDURES.LIST_LANGUAGE_BY_MODID, moduleID);
                //SQLHelper SQLHelper = new SQLHelper();
                var stores = SQLHelper.ExecuteStoreProcedure<OracleStore>(ConnectionString, SYSTEM_STORE_PROCEDURES.LIST_STOREPROC_BY_MODID, moduleID);
            }
            catch (Exception ex)
            {
                throw ErrorUtils.CreateError(ex);
            }
        }

        public List<MaintainModuleInfo> LoadMainTainModule(string modId)
        {
            var stores = SQLHelper.ExecuteStoreProcedure<MaintainModuleInfo>(ConnectionString, SYSTEM_STORE_PROCEDURES.DEV_SP_MODMAINTAIN_SELBY_MODID, modId);
            return stores;
        }


        public List<CodeInfo> LoadDefModByTypeValue(string defModValue)
        {
            var stores = SQLHelper.ExecuteStoreProcedure<CodeInfo>(ConnectionString, null, SYSTEM_STORE_PROCEDURES.DEFCODE_SelectByTypeValue, defModValue);
            return stores;
        }
        public List<SearchModuleInfo> LoadModSearchByModId(string defModValue)
        {
            var stores = SQLHelper.ExecuteStoreProcedure<SearchModuleInfo>(ConnectionString, SYSTEM_STORE_PROCEDURES.DEV_SP_MODSEARCH_SEL_BY_MODID, defModValue);
            return stores;
        }
        public object RunStoreToDataTable(string storeName, List<SqlParameter> parrams)
        {
            try
            {
                var SQLHelper = new SQLHelper();
                DataTable data = new DataTable();
                SQLHelper.FillDataTable(ConnectionString, storeName, out data, parrams);
                return data;
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public List<ExecProcModuleInfo> LoadModExecProcByModId(string modId)
        {
            try
            {
                var stores = SQLHelper.ExecuteStoreProcedure<ExecProcModuleInfo>(ConnectionString, SYSTEM_STORE_PROCEDURES.DEV_MODEXECPROC_SEL_BY_MODID, modId);
                return stores;
            }
            catch (Exception ex)
            {

            }
            return null;
        }


        public List<User> GetUserByUserNamePassword(string username, string password)
        {
            var lst = new List<string>();
            lst.Add(username);
            lst.Add(password);
            return SQLHelper.ExecuteStoreProcedure<User>(ConnectionString, SYSTEM_STORE_PROCEDURES.GET_USER_BY_USERNAME_PASSWORD, lst.ToArray());
        }
        public List<ErrorInfo> LoadAllErrorInfo()
        {
            var stores = SQLHelper.ExecuteStoreProcedure<ErrorInfo>(ConnectionString, SYSTEM_STORE_PROCEDURES.DEFERROR_SELECT_ALL, "");
            return stores;
        }

        public List<LanguageInfo> GetAllLanguageText()
        {
            var stores = SQLHelper.ExecuteStoreProcedure<LanguageInfo>(ConnectionString, SYSTEM_STORE_PROCEDURES.DEFLANG_SELECT_TEXT_LANG, "");
            return stores;
        }
        public List<LanguageInfo> GetAllBtnLanguageText()
        {
            var stores = SQLHelper.ExecuteStoreProcedure<LanguageInfo>(ConnectionString, SYSTEM_STORE_PROCEDURES.DEFLANG_SELECT_BTN_LANG, "");
            return stores;
        }
        public List<CodeInfo> GetAllCodeInfo()
        {
            var stores = SQLHelper.ExecuteStoreProcedure<CodeInfo>(ConnectionString, SYSTEM_STORE_PROCEDURES.DEFCODE_SelectAll, "");
            return stores;
        }

        public List<SysVar> GetAllSysVar()
        {
            var stores = SQLHelper.ExecuteStoreProcedure<SysVar>(ConnectionString, SYSTEM_STORE_PROCEDURES.SYSVAR_SelectAll, "");
            return stores;
        }
        public List<MenuItemInfo> GetAllMenu(int userId)
        {
            var stores = SQLHelper.ExecuteStoreProcedure<MenuItemInfo>(ConnectionString, SYSTEM_STORE_PROCEDURES.Menu_SelectAll, userId);
            return stores;
        }
        public List<LanguageInfo> GetAllLanguageIcon()
        {
            var stores = SQLHelper.ExecuteStoreProcedure<LanguageInfo>(ConnectionString, SYSTEM_STORE_PROCEDURES.DEFLANG_SelectAllIcon_MenuText, "");
            return stores;
        }

        public List<GroupMod> GetGroupModByUserId(string userId)
        {
            var stores = SQLHelper.ExecuteStoreProcedure<GroupMod>(ConnectionString, SYSTEM_STORE_PROCEDURES.DEFLANG_SelectRoleByUserId, userId);
            return stores;
        }

    }
}
