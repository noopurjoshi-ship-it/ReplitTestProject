using System.Data;

namespace ReplitTestProject.Models
{
    public class UserInfo : ISBase.BaseClass
    {
        public string UserInfoID { get; private set; }
        public bool IsNew { get { return string.IsNullOrEmpty(UserInfoID); } }
        public string GUID { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsWarehouse { get; set; }
        public bool IsCustomer { get; set; }
        public bool IsDistributor { get; set; }
        public bool IsDesigner { get; set; }
        public bool IsAgent { get; set; }
        public bool IsSubmitOrder { get; set; }
        public bool IsViewDiscount { get; set; }
        public bool IsDifferentStyle { get; set; }
        public bool ShowInbound { get; set; }
        public bool ShowOutbound { get; set; }
        public bool ShowShipping { get; set; }
        public bool ShowInventory { get; set; }
        public bool ShowSettings { get; set; }
        public bool ShowReports { get; set; }
        public bool ShowCustomerOrder { get; set; }
        public bool ShowResource { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get { return FirstName + " " + LastName; } }
        public string EmployeeID { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PrinterID { get; set; }
        public string Printer2x4ID { get; set; }
        public string CustomerID { get; set; }
        public double DiscountPercentage { get; set; }
        public bool InActive { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public DateTime CreatedOn { get; set; }

        public string AccountType
        {
            get
            {
                string strAccountType = string.Empty;

                if (IsAdmin) strAccountType = "Admin";
                else if (IsDistributor) strAccountType = "Distributor";
                else if (IsCustomer) strAccountType = "Dealer";
                else if (IsDesigner) strAccountType = "Designer";
                else if (IsAgent) strAccountType = "Agent";
                else if (IsWarehouse) strAccountType = "Warehouse";

                else strAccountType = "Invalid";

                return strAccountType;
            }
        }


        public UserInfo()
        {
        }

        public UserInfo(string UserInfoID)
        {
            this.UserInfoID = UserInfoID;
            Load();
        }

        public UserInfo(DataRow objRow)
        {
            Load(objRow);
        }

        protected override void Load()
        {
            base.Load();

            DataSet objData = null;
            string strSQL = string.Empty;

            try
            {
                strSQL = "SELECT * " +
                         "FROM UserInfo (NOLOCK) " +
                         "WHERE UserInfoID=" + Database.HandleQuote(UserInfoID);
                objData = Database.GetDataSet(strSQL);
                if (objData != null && objData.Tables[0].Rows.Count > 0)
                {
                    Load(objData.Tables[0].Rows[0]);
                }
                else
                {
                    throw new Exception("UserInfoID=" + UserInfoID + " is not found");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objData = null;
            }
        }
        protected void Load(string GUID)
        {
            base.Load();

            DataSet objData = null;
            string strSQL = string.Empty;

            try
            {
                strSQL = "SELECT * " +
                         "FROM UserInfo (NOLOCK) " +
                         "WHERE GUID=" + Database.HandleQuote(GUID);
                objData = Database.GetDataSet(strSQL);
                if (objData != null && objData.Tables[0].Rows.Count > 0)
                {
                    Load(objData.Tables[0].Rows[0]);
                }
                else
                {
                    throw new Exception("GUID=" + GUID + " is not found");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objData = null;
            }
        }

        private void Load(DataRow objRow)
        {
            DataColumnCollection objColumns = null;

            try
            {
                objColumns = objRow.Table.Columns;

                if (objColumns.Contains("UserInfoID")) UserInfoID = Convert.ToString(objRow["UserInfoID"]);
                if (objColumns.Contains("GUID")) GUID = Convert.ToString(objRow["GUID"]);
                if (objColumns.Contains("IsAdmin") && objRow["IsAdmin"] != DBNull.Value) IsAdmin = Convert.ToBoolean(objRow["IsAdmin"]);
                if (objColumns.Contains("IsCustomer") && objRow["IsCustomer"] != DBNull.Value) IsCustomer = Convert.ToBoolean(objRow["IsCustomer"]);
                if (objColumns.Contains("IsDistributor") && objRow["IsDistributor"] != DBNull.Value) IsDistributor = Convert.ToBoolean(objRow["IsDistributor"]);
                if (objColumns.Contains("IsDesigner") && objRow["IsDesigner"] != DBNull.Value) IsDesigner = Convert.ToBoolean(objRow["IsDesigner"]);
                if (objColumns.Contains("IsAgent") && objRow["IsAgent"] != DBNull.Value) IsAgent = Convert.ToBoolean(objRow["IsAgent"]);
                if (objColumns.Contains("IsSubmitOrder") && objRow["IsSubmitOrder"] != DBNull.Value) IsSubmitOrder = Convert.ToBoolean(objRow["IsSubmitOrder"]);
                if (objColumns.Contains("IsViewDiscount") && objRow["IsViewDiscount"] != DBNull.Value) IsViewDiscount = Convert.ToBoolean(objRow["IsViewDiscount"]);
                if (objColumns.Contains("IsDifferentStyle") && objRow["IsDifferentStyle"] != DBNull.Value) IsDifferentStyle = Convert.ToBoolean(objRow["IsDifferentStyle"]);
                if (objColumns.Contains("FirstName")) FirstName = Convert.ToString(objRow["FirstName"]);
                if (objColumns.Contains("LastName")) LastName = Convert.ToString(objRow["LastName"]);
                if (objColumns.Contains("EmployeeID")) EmployeeID = Convert.ToString(objRow["EmployeeID"]);
                if (objColumns.Contains("Email")) Email = Convert.ToString(objRow["Email"]);
                if (objColumns.Contains("PrinterID")) PrinterID = Convert.ToString(objRow["PrinterID"]);
                if (objColumns.Contains("Printer2x4ID")) Printer2x4ID = Convert.ToString(objRow["Printer2x4ID"]);
                if (objColumns.Contains("CustomerID")) CustomerID = Convert.ToString(objRow["CustomerID"]);
                if (objColumns.Contains("DiscountPercentage") && objRow["DiscountPercentage"] != DBNull.Value) DiscountPercentage = Convert.ToDouble(objRow["DiscountPercentage"]);
                if (objColumns.Contains("Password")) Password = Convert.ToString(objRow["Password"]);
                if (objColumns.Contains("InActive") && objRow["InActive"] != DBNull.Value) InActive = Convert.ToBoolean(objRow["InActive"]);
                if (objColumns.Contains("UpdatedOn") && objRow["UpdatedOn"] != DBNull.Value) UpdatedOn = Convert.ToDateTime(objRow["UpdatedOn"]);
                if (objColumns.Contains("CreatedOn")) CreatedOn = Convert.ToDateTime(objRow["CreatedOn"]);

                if (objColumns.Contains("IsWarehouse") && objRow["IsWarehouse"] != DBNull.Value) IsWarehouse = Convert.ToBoolean(objRow["IsWarehouse"]);
                if (objColumns.Contains("ShowInbound") && objRow["ShowInbound"] != DBNull.Value) ShowInbound = Convert.ToBoolean(objRow["ShowInbound"]);
                if (objColumns.Contains("ShowOutbound") && objRow["ShowOutbound"] != DBNull.Value) ShowOutbound = Convert.ToBoolean(objRow["ShowOutbound"]);
                if (objColumns.Contains("ShowShipping") && objRow["ShowShipping"] != DBNull.Value) ShowShipping = Convert.ToBoolean(objRow["ShowShipping"]);
                if (objColumns.Contains("ShowInventory") && objRow["ShowInventory"] != DBNull.Value) ShowInventory = Convert.ToBoolean(objRow["ShowInventory"]);
                if (objColumns.Contains("ShowSettings") && objRow["ShowSettings"] != DBNull.Value) ShowSettings = Convert.ToBoolean(objRow["ShowSettings"]);
                if (objColumns.Contains("ShowReports") && objRow["ShowReports"] != DBNull.Value) ShowReports = Convert.ToBoolean(objRow["ShowReports"]);
                if (objColumns.Contains("ShowCustomerOrder") && objRow["ShowCustomerOrder"] != DBNull.Value) ShowCustomerOrder = Convert.ToBoolean(objRow["ShowCustomerOrder"]);
                if (objColumns.Contains("ShowResource") && objRow["ShowResource"] != DBNull.Value) ShowResource = Convert.ToBoolean(objRow["ShowResource"]);

                if (string.IsNullOrEmpty(UserInfoID)) throw new Exception("Missing UserInfoID in the datarow");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objColumns = null;
            }
        }

        protected virtual bool Login(string EmployeeID, string Password)
        {
            DataSet objData = null;
            string strSQL = string.Empty;
            string strPasswordHash = string.Empty;
            string strPasswordSaltKey = string.Empty;

            try
            {
                strSQL = "SELECT * " +
                         "FROM UserInfo (NOLOCK) " +
                         "WHERE EmployeeID=" + Database.HandleQuote(EmployeeID);
                objData = Database.GetDataSet(strSQL);
                if (objData != null && objData.Tables[0].Rows.Count > 0)
                {
                    //strPasswordHash = objData.Tables[0].Rows[0]["PasswordHash"].ToString();
                    //strPasswordSaltKey = objData.Tables[0].Rows[0]["PasswordSaltKey"].ToString();
                    //if (strPasswordHash != Utility.Security.CreatePasswordHash(Password, strPasswordSaltKey)) throw new Exception("Invalid password");
                    if (objData.Tables[0].Rows[0]["Password"].ToString() != Password) throw new Exception("Invalid email/password");

                    Load(objData.Tables[0].Rows[0]);
                }
                else
                {
                    throw new Exception("Invalid Employee ID/password");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objData = null;
            }
            return true;
        }

        protected string RandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHJKMNPQRSTUVWXYZabcdefghijkmnpqrstuvwxyz123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
