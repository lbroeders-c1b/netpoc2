using System;
using System.Data;

namespace CreditOne.Microservices.BuildingBlocks.OracleProvider.Core
{
    public class SmartDataRecord : IDataRecord
    {
        #region Internal DataRecord

        private readonly IDataRecord _record;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="record">Record data</param>
        internal SmartDataRecord(IDataRecord record)
        {
            _record = record ?? throw new ArgumentNullException("record");
        }

        #endregion

        #region Exception Handling

        private Exception CreateFieldException(int fieldIndex, Exception e)
        {
            return new Exception(string.Format("Error on field [{0}].", this.GetName(fieldIndex)), e);
        }

        #endregion

        #region IDataRecord Implementation

        public int FieldCount
        {
            get { return this._record.FieldCount; }
        }

        private static readonly string boolYN = "Y|N";
        private static readonly string boolYesNo = "YES|NO";
        private static readonly string bool01 = "0|1";
        private static readonly string boolTrueFalse = "TRUE|FALSE";

        public bool GetBoolean(int i)
        {
            try
            {
                if (this.GetFieldType(i) == typeof(decimal))
                {
                    return Convert.ToBoolean(this._record.GetDecimal(i));
                }
                else if (this.GetFieldType(i) == typeof(Int32))
                {
                    return Convert.ToBoolean(this._record.GetInt32(i));
                }
                else if (this.GetFieldType(i) == typeof(Int16))
                {
                    return Convert.ToBoolean(this._record.GetInt16(i));
                }
                else if (this.GetFieldType(i) == typeof(string))
                {
                    string value = this.GetString(i).ToUpper();

                    if (boolYN.Contains(value))
                    {
                        if (value.Equals("N"))
                            return false;
                        else
                            return true;
                    }
                    else if (boolYesNo.Contains(value))
                    {
                        if (value.Equals("NO"))
                            return false;
                        else
                            return true;
                    }
                    else if (bool01.Contains(value))
                    {
                        if (value.Equals("0"))
                            return false;
                        else
                            return true;
                    }
                    else if (boolTrueFalse.Contains(value))
                    {
                        if (value.Equals("FALSE"))
                            return false;
                        else
                            return true;
                    }
                    else
                    {
                        throw new NotSupportedException(
                            string.Format("Error getting boolean for field [{0}]. Converting string value of [{1}] to boolean is not supported.",
                                            this.GetName(i),
                                            value));
                    }

                }
                else
                {
                    throw new NotSupportedException(
                        string.Format("Error getting boolean for field [{0}]. Converting type [{1}] to boolean is not supported.",
                                        this.GetName(i),
                                        this.GetFieldType(i)));
                }
            }
            catch (Exception e)
            {
                throw this.CreateFieldException(i, e);
            }
        }

        public byte GetByte(int i)
        {
            try
            {
                return this._record.GetByte(i);
            }
            catch (Exception e)
            {
                throw this.CreateFieldException(i, e);
            }
        }

        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            try
            {
                return this._record.GetBytes(i, fieldOffset, buffer, bufferoffset, length);
            }
            catch (Exception e)
            {
                throw this.CreateFieldException(i, e);
            }
        }

        public char GetChar(int i)
        {
            try
            {
                return Convert.ToChar(this._record.GetString(i));
            }
            catch (Exception e)
            {
                throw this.CreateFieldException(i, e);
            }
        }

        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            try
            {
                return this._record.GetChars(i, fieldoffset, buffer, bufferoffset, length);
            }
            catch (Exception e)
            {
                throw this.CreateFieldException(i, e);
            }
        }

        public IDataReader GetData(int i)
        {
            try
            {
                return this._record.GetData(i);
            }
            catch (Exception e)
            {
                throw this.CreateFieldException(i, e);
            }
        }

        public string GetDataTypeName(int i)
        {
            try
            {
                return this._record.GetDataTypeName(i);
            }
            catch (Exception e)
            {
                throw this.CreateFieldException(i, e);
            }
        }

        public DateTime GetDateTime(int i)
        {
            try
            {
                return this._record.GetDateTime(i);
            }
            catch (Exception e)
            {
                throw this.CreateFieldException(i, e);
            }
        }

        public decimal GetDecimal(int i)
        {
            try
            {
                return this._record.GetDecimal(i);
            }
            catch (Exception e)
            {
                throw this.CreateFieldException(i, e);
            }
        }

        public double GetDouble(int i)
        {
            try
            {
                return this._record.GetDouble(i);
            }
            catch (Exception e)
            {
                throw this.CreateFieldException(i, e);
            }
        }

        public Type GetFieldType(int i)
        {
            try
            {
                return this._record.GetFieldType(i);
            }
            catch (Exception e)
            {
                throw this.CreateFieldException(i, e);
            }
        }

        public float GetFloat(int i)
        {
            try
            {
                return this._record.GetFloat(i);
            }
            catch (Exception e)
            {
                throw this.CreateFieldException(i, e);
            }
        }

        public Guid GetGuid(int i)
        {
            try
            {
                return this._record.GetGuid(i);
            }
            catch (Exception e)
            {
                throw this.CreateFieldException(i, e);
            }
        }

        public short GetInt16(int i)
        {
            try
            {
                if (this.GetFieldType(i) == typeof(decimal))
                    return Convert.ToInt16(this._record.GetDecimal(i));
                else
                    return this._record.GetInt16(i);
            }
            catch (Exception e)
            {
                throw this.CreateFieldException(i, e);
            }
        }

        public int GetInt32(int i)
        {
            try
            {
                if (this.GetFieldType(i) == typeof(decimal))
                    return Convert.ToInt32(this._record.GetDecimal(i));
                else
                    return this._record.GetInt32(i);
            }
            catch (Exception e)
            {
                throw this.CreateFieldException(i, e);
            }
        }

        public long GetInt64(int i)
        {
            try
            {
                if (this.GetFieldType(i) == typeof(decimal))
                    return Convert.ToInt64(this._record.GetDecimal(i));
                else
                    return this._record.GetInt64(i);
            }
            catch (Exception e)
            {
                throw this.CreateFieldException(i, e);
            }
        }

        public string GetName(int i)
        {
            try
            {
                return this._record.GetName(i);
            }
            catch (IndexOutOfRangeException)
            {
                throw new Exception(string.Format("Index [{0}] does not exist in the data record.", i.ToString()));
            }
        }

        public int GetOrdinal(string name)
        {
            try
            {
                return this._record.GetOrdinal(name);
            }
            catch (IndexOutOfRangeException)
            {
                throw new Exception(string.Format("Field [{0}] does not exist in the data record.", name));
            }
        }

        public string GetString(int i)
        {
            try
            {
                return this._record.GetString(i);
            }
            catch (Exception e)
            {
                throw this.CreateFieldException(i, e);
            }
        }

        public object GetValue(int i)
        {
            try
            {
                return this._record.GetValue(i);
            }
            catch (Exception e)
            {
                throw this.CreateFieldException(i, e);
            }
        }

        public int GetValues(object[] values)
        {
            return this._record.GetValues(values);
        }

        public bool IsDBNull(int i)
        {
            try
            {
                return this._record.IsDBNull(i);
            }
            catch (Exception e)
            {
                throw this.CreateFieldException(i, e);
            }
        }

        public object this[string name]
        {
            get { return this.GetValue(this.GetOrdinal(name)); }
        }

        public object this[int i]
        {
            get { return this.GetValue(i); }
        }

        #endregion

        #region Get By Name Methods

        public bool GetBoolean(string name)
        {
            return this.GetBoolean(this.GetOrdinal(name));
        }

        public byte GetByte(string name)
        {
            return this.GetByte(this.GetOrdinal(name));
        }

        public long GetBytes(string name, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            return this._record.GetBytes(this.GetOrdinal(name), fieldOffset, buffer, bufferoffset, length);
        }

        public char GetChar(string name)
        {
            return this.GetChar(this.GetOrdinal(name));
        }

        public long GetChars(string name, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            return this.GetChars(this.GetOrdinal(name), fieldoffset, buffer, bufferoffset, length);
        }

        public IDataReader GetData(string name)
        {
            return this.GetData(this.GetOrdinal(name));
        }

        public string GetDataTypeName(string name)
        {
            return this.GetDataTypeName(this.GetOrdinal(name));
        }

        public DateTime GetDateTime(string name)
        {
            return this.GetDateTime(this.GetOrdinal(name));
        }

        public decimal GetDecimal(string name)
        {
            return this.GetDecimal(this.GetOrdinal(name));
        }

        public double GetDouble(string name)
        {
            return this.GetDouble(this.GetOrdinal(name));
        }

        public Type GetFieldType(string name)
        {
            return this.GetFieldType(this.GetOrdinal(name));
        }

        public float GetFloat(string name)
        {
            return this.GetFloat(this.GetOrdinal(name));
        }

        public Guid GetGuid(string name)
        {
            return this.GetGuid(this.GetOrdinal(name));
        }

        public short GetInt16(string name)
        {
            return this.GetInt16(this.GetOrdinal(name));
        }

        public int GetInt32(string name)
        {
            return this.GetInt32(this.GetOrdinal(name));
        }

        public long GetInt64(string name)
        {
            return this.GetInt64(this.GetOrdinal(name));
        }

        public string GetString(string name)
        {
            return this.GetString(this.GetOrdinal(name));
        }

        public object GetValue(string name)
        {
            return this.GetValue(this.GetOrdinal(name));
        }

        public bool IsDBNull(string name)
        {
            return this.IsDBNull(this.GetOrdinal(name));
        }

        #endregion

        #region Null safe methods

        public bool ContainsField(string name)
        {
            var exist = false;

            for (int i = 0; i < this.FieldCount; i++)
            {
                if (this.GetName(i).ToLower() == name.ToLower())
                    exist = true;
            }

            return exist;
        }

        public long TryGetInt64(string name)
        {
            if (this.IsDBNull(name))
            {
                return default(long);
            }

            return this.GetInt64(name);
        }

        public int TryGetInt32(string name)
        {
            if (this.IsDBNull(name))
            {
                return default(int);
            }

            return this.GetInt32(name);
        }

        public DateTime TryGetDateTime(string name)
        {
            if (this.IsDBNull(name))
            {
                return default(DateTime);
            }

            return this.GetDateTime(name);
        }

        public DateTime? TryGetNullableDateTime(string name)
        {
            if (this.IsDBNull(name))
            {
                return null;
            }

            return this.GetDateTime(name);
        }

        public string TryGetString(string name)
        {
            if (this.IsDBNull(name))
            {
                return default(string);
            }

            return this.GetString(name);
        }

        public decimal TryGetDecimal(string name)
        {
            if (this.IsDBNull(name))
            {
                return default(decimal);
            }

            return this.GetDecimal(name);
        }

        public bool TryGetBool(string name)
        {
            if (this.IsDBNull(name))
            {
                return default(bool);
            }

            return this.GetBoolean(name);
        }

        #endregion
    }
}
