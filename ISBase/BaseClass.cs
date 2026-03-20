using System.Collections;
using Microsoft.Data.SqlClient;

namespace ReplitTestProject.ISBase
{
    public class BaseClass
    {
        public bool IsLoaded { get; set; }
        private bool? mIsActive = null;
        public virtual bool IsActive { get; set; }

        private ArrayList mQuery { get; set; }
        public ArrayList Query
        {
            get
            {
                if (mQuery == null) mQuery = new ArrayList();
                return mQuery;
            }
            set
            {
                mQuery = value;
            }

        }

        public BaseClass()
        {
            IsActive = true;
        }

        protected virtual void Load()
        {
            IsLoaded = true;
        }

        protected virtual void Load(SqlConnection objConn, SqlTransaction objTran)
        {
            IsLoaded = true;
            //mConnection = objConn;
            //mTransaction = objTran;
        }

        public virtual bool Create()
        {
            if (IsLoaded) throw new Exception("Create() cannot be performed because object is loaded from constructors");
            return true;
        }

        public virtual bool Create(SqlConnection objConn, SqlTransaction objTran)
        {
            if (IsLoaded) throw new Exception("Create() cannot be performed because object is loaded from constructors");
            //mConnection = objConn;
            //mTransaction = objTran;
            return true;
        }

        public virtual bool Update()
        {
            if (!IsLoaded) throw new Exception("Update() cannot be performed because object is not loaded from constructors");
            return true;
        }

        public virtual bool Update(SqlConnection objConn, SqlTransaction objTran)
        {
            if (!IsLoaded) throw new Exception("Update() cannot be performed because object is not loaded from constructors");
            //mConnection = objConn;
            //mTransaction = objTran;
            return true;
        }

        public virtual bool Copy()
        {
            IsLoaded = false;
            //if (!IsLoaded) throw new Exception("Copy() cannot be performed because object is not loaded from constructors");
            return true;
        }

        public virtual bool Copy(SqlConnection objConn, SqlTransaction objTran)
        {
            IsLoaded = false;
            //if (!IsLoaded) throw new Exception("Copy() cannot be performed because object is not loaded from constructors");
            return true;
        }

        public virtual bool Delete()
        {
            if (!IsLoaded) throw new Exception("Delete() cannot be performed because object is not loaded from constructors");
            return true;
        }

        public virtual bool Delete(SqlConnection objConn, SqlTransaction objTran)
        {
            if (!IsLoaded) throw new Exception("Delete() cannot be performed because object is not loaded from constructors");
            //mConnection = objConn;
            //mTransaction = objTran;
            return true;
        }
    }
}
