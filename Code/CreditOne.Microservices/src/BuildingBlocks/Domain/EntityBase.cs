using System;

namespace CreditOne.Microservices.BuildingBlocks.Common.Domain
{
    /// <summary>
    /// Represents the status for an object (IsNew-IsDirty-IsForDeletion)
    /// </summary>
    /// <remarks>
    /// <list type="table">
    /// <listheader>
    ///     <term>Date</term>
    ///     <term>Who</term>
    ///     <term>BR/WO</term>
    ///     <description>Description</description>
    /// </listheader>
    /// <item>
    ///     <term>08-09-2019</term>
    ///     <term>Jonatan Marquez</term>
    ///     <term>RM-47</term>
    ///     <description>Initial implementation</description>
    /// </item>
    /// <item>
    ///     <term>02-04-2020</term>
    ///     <term>Luis Petitjean</term>
    ///     <term>RM-80</term>
    ///     <description>Added a <c>List<MediatR.INotification></c> and methods for managing Domain Events</description>
    /// </item>
    /// </list>
    /// </remarks>
    public abstract class EntityBase
    {
        #region Private Members

        private bool _isMarkedForDeletion;
        private int? _requestedHashCode;
        private decimal _id;

        #endregion

        #region Public Properties
        
        /// <summary>
        /// Virtual property for ID
        /// </summary>
        public virtual decimal Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// Indicates whether or not the data in the object is new
        /// </summary>
        public bool IsNew { get; private set; }

        /// <summary>
        /// Indicates whether or not the data in the object has been modified
        /// </summary>
        /// 
        public bool IsDirty { get; private set; }

        /// <summary>
        /// Indicates whether or not the object is marked for deletion
        /// The deletion will not occur until Save is called
        /// </summary>
        public bool IsMarkedForDeletion
        {
            get { return _isMarkedForDeletion; }
            set
            {
                if (this.IsNew)
                {
                    throw new InvalidOperationException("A domain object that is marked new cannot be marked for deletion.");
                }

                this._isMarkedForDeletion = value;
                this.MarkDirty();
            }
        }

        #endregion

        #region Protected Properties

        /// <summary>
        /// Returns true if the object has data
        /// </summary>
        protected bool HasData
        {
            get { return this != null; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Marks the object as new
        /// </summary>
        public void MarkNew()
        {
            if (this.IsMarkedForDeletion)
            {
                throw new InvalidOperationException("A business object that is marked for deletion cannot be marked new.");
            }

            this.IsNew = true;
            this.MarkDirty();
        }

        /// <summary>
        /// Marks the object's state as changed
        /// </summary>
        public void MarkDirty()
        {
            IsDirty = true;
        }

        /// <summary>
        /// Resets the object states
        /// </summary>
        public void ResetState()
        {
            this.IsNew = false;
            this.IsDirty = false;
            this.IsMarkedForDeletion = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsTransient()
        {
            return Id == default(Int32);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is EntityBase))
            {
                return false;
            }


            if (Object.ReferenceEquals(this, obj))
            {
                return true;
            }


            if (this.GetType() != obj.GetType())
            {
                return false;
            }

            EntityBase item = (EntityBase)obj;

            if (item.IsTransient() || this.IsTransient())
            {
                return false;

            }
            else
            {
                return item.Id == this.Id;
            }
        }

        public override int GetHashCode()
        {
            if (!IsTransient())
            {
                if (!_requestedHashCode.HasValue)
                {
                    _requestedHashCode = this.Id.GetHashCode() ^ 31; // XOR for random distribution (http://blogs.msdn.com/b/ericlippert/archive/2011/02/28/guidelines-and-rules-for-gethashcode.aspx)
                }

                return _requestedHashCode.Value;
            }
            else
            {
                return base.GetHashCode();
            }
        }

        public static bool operator ==(EntityBase left, EntityBase right)
        {
            if (Object.Equals(left, null))
                return (Object.Equals(right, null)) ? true : false;
            else
                return left.Equals(right);
        }

        public static bool operator !=(EntityBase left, EntityBase right)
        {
            return !(left == right);
        }

        #endregion
    }
}
