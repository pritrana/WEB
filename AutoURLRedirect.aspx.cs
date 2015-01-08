using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data.SqlClient;

public partial class AutoURLRedirect : System.Web.UI.Page
{

    string Loc_des, sector, Mdate, AutoURL, logindt, clkcnt, validdt, username, IPAddress, MsgBody, RedirectTo, SectorID, BankName, SessionID;
    int userID;

    private VcmUserNamespace.SessionInfo _session;
    public VcmUserNamespace.SessionInfo CurrentSession
    {
        get
        {
            if (_session == null)
            {
                _session = new VcmUserNamespace.SessionInfo(HttpContext.Current.Session);
            }
            return _session;
        }
        set
        {
            _session = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        string strBrows = Request.Browser.Version.ToString();
        if ((strBrows == "6.0" || strBrows == "7.0" || strBrows == "8.0") && Request.Browser.Browser.Contains("IE")) 
        { }
        else
        {
            Response.Redirect("~/Browser.aspx?RefNo=" + Convert.ToString(Request.QueryString["RefNo"]) + "");
        }

        DataSet dst = new DataSet();
        Session.Clear();
        AutoURL = Request.Url.ToString().Replace(" ", "%20");
        IPAddress = Request.UserHostAddress;
        string mailto = System.Configuration.ConfigurationManager.AppSettings["SUMMARYEMAIL"].ToString();
        string EnableEmail = System.Configuration.ConfigurationManager.AppSettings["EnableEmail"].ToString();
        string FromEmail = System.Configuration.ConfigurationManager.AppSettings["FromEmail"].ToString();      


        VcmUserNamespace.SessionInfo CurrentSession = new VcmUserNamespace.SessionInfo(HttpContext.Current.Session);

        if (Request.QueryString["RefNo"] != null)
        {
            vcmProductNamespace.cDbHandler.UpdatingAutoURL(Request.QueryString["RefNo"]);
            dst = vcmProductNamespace.cDbHandler.GET_SESSIONDTL_FRM_USERID(Request.QueryString["RefNo"], IPAddress);

            try
            {
                if (dst.Tables.Count > 0 && dst.Tables[0].Rows.Count > 0)
                {
                    int MsgID = Convert.ToInt32(dst.Tables[0].Rows[0]["MsgId"]);
                    if (MsgID == -2 || MsgID == 0 || MsgID == -5)
                    {
                        Loc_des = dst.Tables[0].Rows[0]["LocationCode"].ToString();
                        Mdate = dst.Tables[0].Rows[0]["MDate"].ToString();
                        sector = dst.Tables[0].Rows[0]["SessionName"].ToString(); //Sector
                        logindt = dst.Tables[0].Rows[0]["UpdatedDT"].ToString();
                        clkcnt = dst.Tables[0].Rows[0]["clkcnt"].ToString();
                        validdt = dst.Tables[0].Rows[0]["validdt"].ToString();
                        username = dst.Tables[0].Rows[0]["Username"].ToString();
                        RedirectTo = dst.Tables[0].Rows[0]["MovetoPage"].ToString();
                        //SectorID = dst.Tables[0].Rows[0]["SectorID"].ToString();
                        SessionID = dst.Tables[0].Rows[0]["SessionID"].ToString();
                        BankName = dst.Tables[0].Rows[0]["BankName"].ToString();
                        userID = Convert.ToInt32(dst.Tables[0].Rows[0]["UserID"]);                       
                    }

                    if (MsgID != 0)
                    {
                        if (MsgID == -2)
                        {
                            MsgBody = "<div style=\"font-size:8pt;font-family:Verdana\">Dear Admin,<br/><br/> User named " + username + ", was not able to login for the Auto URL and the session mentioned below:&nbsp;" +
                                                       " <table><tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Auto URL:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;<a href =" + AutoURL + ">" + AutoURL + "</a></td></tr> " +
                                                       "<tr><td width=\"20%\" style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana; \">Match Session Location :</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + Loc_des + "</td></tr>" +
                                                       "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Match Session Date:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + Mdate + "</td></tr> " +
                                                       "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Match Session Sector:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + sector + "</td></tr> " +
                                                       "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">User:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + username + "</td></tr> " +
                                                       "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Bank:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + BankName + "</td></tr> " +
                                                       "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">IP Address:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + IPAddress + "</td>" +
                                                       "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Login Date:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + Convert.ToDateTime(logindt).ToString("dd-MMM-yyyy") + "</td>" +
                                                       "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Valid Till:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + Convert.ToDateTime(validdt).AddDays(1).ToString("dd-MMM-yyyy") + "</td>" +
                                                       "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Number of Time:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + clkcnt + "</td></tr></table>" +
                                                       "<br/><br/>Sincerely,<br/>Swaption Team<br/>Vyapar Capital Market Partners<br/>volmax@vcmpartners.com<br/>NY:&nbsp;+1-646-688-7545<br/>London:&nbsp;+44 (0)20-7398-2800</div>";
                            //Session["EnableEmail"] = System.Configuration.ConfigurationManager.AppSettings["EnableEmail"].ToString();
                            if (EnableEmail == "1")
                            {
                                SendMail(FromEmail,mailto, "Swaption - AutoURL", MsgBody, false);
                                MsgBody = "";
                            }
                            Session["AutoURL"] = "lnkExpired";
                            Session["SessionDetails"] = Mdate + "#" + sector + "#" + Loc_des;
                            Response.Redirect("Index.aspx", false);
                            return;
                        }
                        else if (MsgID == -5)
                        {
                            MsgBody = "<div style=\"font-size:8pt;font-family:Verdana\">Dear Admin,<br/><br/> User named " + username + ", was not able to login because of Unauthorized IP Address for the session mentioned below:&nbsp;" +
                                                       " <table><tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Auto URL:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;<a href =" + AutoURL + ">" + AutoURL + "</a></td></tr> " +
                                                       "<tr><td width=\"20%\" style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana; \">Match Session Location :</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + Loc_des + "</td></tr>" +
                                                       "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Match Session Date:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + Mdate + "</td></tr> " +
                                                       "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Match Session Sector:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + sector + "</td></tr> " +
                                                       "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">User:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + username + "</td></tr> " +
                                                       "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Bank:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + BankName + "</td></tr> " +
                                                       "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">IP Address:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + IPAddress + "</td>" +
                                                       "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Login Date:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + Convert.ToDateTime(logindt).ToString("dd-MMM-yyyy") + "</td>" +
                                                       "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Valid Till:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + Convert.ToDateTime(validdt).AddDays(1).ToString("dd-MMM-yyyy") + "</td>" +
                                                       "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Number of Time:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + clkcnt + "</td></tr></table>" +
                                                       "<br/><br/>Sincerely,<br/>Swaption Team<br/>Vyapar Capital Market Partners<br/>volmax@vcmpartners.com<br/>NY:&nbsp;+1-646-688-7545<br/>London:&nbsp;+44 (0)20-7398-2800</div>";
                            //Session["EnableEmail"] = System.Configuration.ConfigurationManager.AppSettings["EnableEmail"].ToString();
                            if (EnableEmail == "1")
                            {
                                SendMail(FromEmail,mailto, "Swaption - AutoURL", MsgBody, false);
                                MsgBody = "";
                            }
                            Session["AutoURL"] = "UnauthorizedIP";
                            Session["SessionDetails"] = Mdate + "#" + sector + "#" + Loc_des;
                            Response.Redirect("Index.aspx", false);
                            return;
                        }
                        else if (MsgID == -3)
                        {
                            MsgBody = "<div style=\"font-size:8pt;font-family:Verdana\">Dear Admin,<br/><br/> User Failed to login with the AutoURL mentioned below:&nbsp;" +
                                                                   " <table><tr><td width=\"20% \" style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Auto URL:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;<a href =" + AutoURL + ">" + AutoURL + "</a></td></tr>" +
                                                                   "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">IP Address:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + IPAddress + "</td></tr></table>" +
                                                                   "<br/><br/>Sincerely,<br/>Swaption Team<br/>Vyapar Capital Market Partners<br/>volmax@vcmpartners.com<br/>NY:&nbsp;+1-646-688-7545<br/>London:&nbsp;+44 (0)20-7398-2800</div>";
                            //Session["EnableEmail"] = System.Configuration.ConfigurationManager.AppSettings["EnableEmail"].ToString();
                            if (EnableEmail == "1")
                            {
                                SendMail(FromEmail,mailto, "Swaption - AutoURL", MsgBody, false);
                                MsgBody = "";
                            }
                            Session["AutoURL"] = "lnkInvalid";
                            Session["SessionDetails"] = Mdate + "#" + sector + "#" + Loc_des;
                            Response.Redirect("Index.aspx", false);
                            return;
                        }
                        else if (MsgID == -1)
                        {
                            MsgBody = "<div style=\"font-size:8pt;font-family:Verdana\">Dear Admin,<br/><br/> User Failed to login with the AutoURL mentioned below:&nbsp;" +
                                                                   " <table><tr><td width=\"20% \" style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Auto URL:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;<a href =" + AutoURL + ">" + AutoURL + "</a></td></tr>" +
                                                                   "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">IP Address:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + IPAddress + "</td></tr></table>" +
                                                                   "<br/><br/>Sincerely,<br/>Swaption Team<br/>Vyapar Capital Market Partners<br/>volmax@vcmpartners.com<br/>NY:&nbsp;+1-646-688-7545<br/>London:&nbsp;+44 (0)20-7398-2800</div>";
                            //Session["EnableEmail"] = System.Configuration.ConfigurationManager.AppSettings["EnableEmail"].ToString();
                            if (EnableEmail == "1")
                            {
                                SendMail(FromEmail,mailto, "Swaption - AutoURL", MsgBody, false);
                                MsgBody = "";
                            }
                            Session["AutoURL"] = "InvalidUser";
                            Session["SessionDetails"] = Mdate + "#" + sector + "#" + Loc_des;
                            Response.Redirect("Index.aspx", false);
                            return;
                        }
                    }
                    else
                    {
                        MsgBody = "<div style=\"font-size:8pt;font-family:Verdana\">Dear Admin,<br/><br/> User named " + username + ", has successfully logged in for the Auto URL and the session mentioned below:&nbsp;<br/>" +
                                                   " <table><tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Auto URL:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;<a href =" + AutoURL + ">" + AutoURL + "</a></td></tr> " +
                                                   "<tr><td width=\"20%\" style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana; \">Match Session Location :</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + Loc_des + "</td></tr>" +
                                                   "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Match Session Date:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + Mdate + "</td></tr> " +
                                                   "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Match Session Sector:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + sector + "</td></tr> " +
                                                   "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">User:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + username + "</td></tr> " +
                                                   "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Bank:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + BankName + "</td></tr> " +
                                                   "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Page:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + RedirectTo + "</td></tr> " +
                                                   "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">IP Address:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + IPAddress + "</td>" +
                                                   "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Login Date:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + Convert.ToDateTime(logindt).ToString("dd-MMM-yyyy") + "</td>" +
                                                   "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Valid Till:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + Convert.ToDateTime(validdt).AddDays(1).ToString("dd-MMM-yyyy") + "</td>" +
                                                   "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Number of Time:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + clkcnt + "</td></tr></table>" +
                                                   "<br/><br/>Sincerely,<br/>Swaption Team<br/>Vyapar Capital Market Partners<br/>volmax@vcmpartners.com<br/>NY:&nbsp;+1-646-688-7545<br/>London:&nbsp;+44 (0)20-7398-2800</div>";
                        //Session["EnableEmail"] = System.Configuration.ConfigurationManager.AppSettings["EnableEmail"].ToString();
                        if (EnableEmail == "1")
                        {
                            SendMail(FromEmail, mailto, "Swaption - AutoURL", MsgBody, false);
                            MsgBody = "";
                        }

                        //Session["SetId"] = dst.Tables[0].Rows[0]["setID"].ToString();
                        //Session["SectorId"] = SectorID;

                        /**/

                        CurrentSession.User = new VcmUserNamespace.SiteUserInfo(Convert.ToInt64(dst.Tables[0].Rows[0]["UserID"].ToString()), dst.Tables[0].Rows[0]["LoginID"].ToString(), dst.Tables[0].Rows[0]["LoginID"].ToString(), 1, -100, DateTime.Now.ToString(), Convert.ToString(Guid.NewGuid()), Convert.ToString(dst.Tables[0].Rows[0]["ProductType"]),"");

                        //LoginTypeID remain - right now set -100

                        // For Currency 
                        CurrentSession.User.CurrencyId = dst.Tables[0].Rows[0]["CurrencyId"].ToString();
                        CurrentSession.User.CurrencyDesc = dst.Tables[0].Rows[0]["Currency_Desc"].ToString();
                        CurrentSession.User.CurrencySymbol = dst.Tables[0].Rows[0]["Currency_Symbol"].ToString();

                        Session["Sector"] = sector;
                        Session["MOrgDate"] = Mdate;
                        //Session["selectedDetailControl"] = RedirectTo;
                        //CurrentSession.User.Producttype = Convert.ToString(dst.Tables[0].Rows[0]["ProductType"]);
                        CurrentSession.User.EmailID = dst.Tables[0].Rows[0]["LoginID"].ToString();
                        CurrentSession.User.LastActivityDate = DateTime.Now.ToString();
                        CurrentSession.User.UserName = dst.Tables[0].Rows[0]["LoginID"].ToString();
                        CurrentSession.User.ASPSessionID = System.Convert.ToString(Guid.NewGuid());
                        
                        CurrentSession.User.SessionID = SessionID;
                        CurrentSession.User.LocationCode = dst.Tables[0].Rows[0]["Location_Code"].ToString();
                        CurrentSession.User.MDate = dst.Tables[0].Rows[0]["MDate"].ToString();
                        CurrentSession.User.MOrgDate = dst.Tables[0].Rows[0]["MDate"].ToString();

                        CurrentSession.User.LocationDesc = dst.Tables[0].Rows[0]["LocationCode"].ToString();
                        CurrentSession.User.Sector = sector;
                        CurrentSession.User.CustomerId = Convert.ToInt64(VcmUserNamespace.cUserDbHandler.GetCustomerId(CurrentSession.User.UserID.ToString(),Convert.ToString(CurrentSession.User.SessionID)));
                        CurrentSession.User.IsTrader = VcmUserNamespace.cUserDbHandler.CheckIsTrader(dst.Tables[0].Rows[0]["UserID"].ToString());

                        CurrentSession.User.ImpersonateUserID = Convert.ToInt64(dst.Tables[0].Rows[0]["UserID"].ToString());
                        CurrentSession.User.UserID = CurrentSession.User.ImpersonateUserID;
                        CurrentSession.User.ImpersonateCustomerId = CurrentSession.User.CustomerId;
                        CurrentSession.User.ImpersonateEmailID = dst.Tables[0].Rows[0]["LoginID"].ToString();
                        /**/

                        if (CurrentSession.User.SessionLogincount == null)
                        {
                            CurrentSession.User.SessionLogincount = Convert.ToString(vcmProductNamespace.cDbHandler.GetSessionCount(Convert.ToInt32(SessionID), Convert.ToInt32(CurrentSession.User.ImpersonateUserID)));
                        }

                        if (RedirectTo == "TradePage")
                        {
                            Response.Redirect("IRS_Trading.aspx", false);
                        }
                        else if (RedirectTo == "Home")
                        {
                            Response.Redirect("Session.aspx", false);
                        }
                              
                        else
                        {
                            Response.Redirect("Session.aspx", false);
                        }
                        VcmLogManager.Log.SendUserLoginNotification(Convert.ToString(CurrentSession.User.UserID), Convert.ToString(CurrentSession.User.UserName), 1, -1, "Link for Auto URL was Clicked", RedirectTo, HttpContext.Current.Request.UserHostAddress.ToString(), Convert.ToString(CurrentSession.User.SessionID), Convert.ToString(CurrentSession.User.ASPSessionID), Convert.ToString(CurrentSession.User.SessionLogincount));

                    }
                }
            }
            catch (Exception ex)
            {
                VcmLogManager.Log.writeLog(Convert.ToString(CurrentSession.User.UserName), Convert.ToString(CurrentSession.User.MOrgDate), Convert.ToString(CurrentSession.User.LocationDesc), "AutoURLPage", "Page_Load()", ex.StackTrace.ToString() + "<br/><br/>" + ex.Message, Convert.ToString(Request.UserHostAddress));
            }
            finally
            {
                Loc_des = null;
                sector = null;
                Mdate = null;
                AutoURL = null;
                logindt = null;
                clkcnt = null;
                validdt = null;
                username = null;
                IPAddress = null;
                RedirectTo = null;
            }
        }
        else
        {
            MsgBody = "<div style=\"font-size:8pt;font-family:Verdana\">Dear Admin,<br/><br/> User Failed to login with the AutoURL mentioned below:&nbsp;" +
                                                                              " <table><tr><td width=\"20% \" style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">Auto URL:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;<a href =" + AutoURL + ">" + AutoURL + "</a></td></tr>" +
                                                                              "<tr><td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">IP Address:</td> <td style=\"FONT-SIZE: 8pt; FONT-FAMILY: Verdana;\">&nbsp;" + IPAddress + "</td></tr></table>" +
                                                                              "<br/><br/>Sincerely,<br/>Swaption Team<br/>Vyapar Capital Market Partners<br/>volmax@vcmpartners.com<br/>NY:&nbsp;+1-646-688-7545<br/>London:&nbsp;+44 (0)20-7398-2800</div>";
            //Session["EnableEmail"] = System.Configuration.ConfigurationManager.AppSettings["EnableEmail"].ToString();
            if (EnableEmail == "1")
            {
                SendMail(FromEmail, mailto, "Swaption - AutoURL", MsgBody, false);

                MsgBody = "";
            }
            Session["AutoURL"] = "lnkInvalid";
            Response.Redirect("Index.aspx", false);
            return;
        }
    }

    private void SendMail(string strFrom, string strTo, string strSubject, string strBodyMsg, bool bFlag)
    {
        VcmMailNamespace.vcmMail _vcmMail = new VcmMailNamespace.vcmMail();
        _vcmMail.To = strTo;
        _vcmMail.From = strFrom;

        /*if (bFlag)
        {
          
            _vcmMail.BCC =  System.Configuration.ConfigurationManager.AppSettings["SUMMARYEMAIL"].ToString();
        }*/

        _vcmMail.SendAsync = true;
        _vcmMail.Subject = strSubject;
        _vcmMail.Body = strBodyMsg;
        _vcmMail.IsBodyHtml = true;
        _vcmMail.SendMail();
        _vcmMail = null;
    }
}