using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CreditOne.Microservices.BuildingBlocks.Literals
{
    /// <summary>
    /// Abstrat class for Enumeration
    /// </summary>
    /// <remarks>
    /// <list type="table">
    /// <listheader>
    ///		<term>Date</term>
    ///		<term>Who</term>
    ///		<term>BR/WO</term>
    ///		<description>Description</description>
    /// </listheader>
    /// <item>
    ///		<term>03-22-2019</term>
    ///		<term>Jonatan Marquez</term>
    ///		<term>RM-47</term>
    ///		<description>Initial implementation</description>
    /// </item>
    /// </list>
    /// </remarks>
    public abstract class Enumeration : IComparable
    {
        #region Constructors

        protected Enumeration()
        { }

        /// <summary>
        /// Conistructor 
        /// </summary>
        /// <param name="id">id of the enum</param>
        /// <param name="name">name of the enum value</param>
        protected Enumeration(string id, string name)
        {
            Id = id;
            Name = name;
        }

        /// <summary>
        /// Conistructor 
        /// </summary>
        /// <param name="id">id of the enum</param>
        /// <param name="name">name of the enum</param>
        protected Enumeration(int id, string name)
        {
            Id = id.ToString();
            Name = name;
        }

        #endregion

        #region Public Properties

        public string Name { get; private set; }

        public string Id { get; private set; }

        #endregion

        #region Public Methods 

        public override string ToString() => Name;

        public override bool Equals(object obj)
        {
            var otherValue = obj as Enumeration;

            if (otherValue == null)
                return false;

            var typeMatches = GetType().Equals(obj.GetType());
            var valueMatches = Id.Equals(otherValue.Id);

            return typeMatches && valueMatches;
        }

        public override int GetHashCode() => Id.GetHashCode();

        /// <summary>
        /// Gets all enum values
        /// </summary>
        /// <typeparam name="T">type of the enum</typeparam>
        /// <returns>Enumerable of enum</returns>
        public static IEnumerable<T> GetAll<T>() where T : Enumeration
        {
            var fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

            return fields.Select(f => f.GetValue(null)).Cast<T>();
        }

        /// <summary>
        /// This method returns the enum by display name
        /// </summary>
        /// <typeparam name="T">type of enum</typeparam>
        /// <param name="displayName">display name</param>
        public static T FromDisplayName<T>(string displayName) where T : Enumeration
        {
            var matchingItem = Parse<T, string>(displayName, "display name", item => item.Name == displayName);
            return matchingItem;
        }

        /// <summary>
        /// This method returns the enum by name
        /// </summary>
        /// <typeparam name="T">type of enum</typeparam>
        /// <param name="name">name of lookup type enum</param>
        /// <returns>Enum</returns>
        public static T FromName<T>(string name) where T : Enumeration
        {
            var type = GetAll<T>().SingleOrDefault(s => String.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

            if (type == null)
            {
                throw new Exception($"Possible values for {typeof(T).Name}: { String.Join(",", GetAll<T>().Select(l => l.Name)) }");
            }

            return type;
        }

        /// <summary>
        /// This method returns the enum by id
        /// </summary>
        /// <param name="id">id of lookup type enum</param>
        /// <returns>Enum</returns>
        public static T FromId<T>(string id) where T : Enumeration
        {
            var type = GetAll<T>().SingleOrDefault(s => String.Equals(s.Id, id, StringComparison.CurrentCultureIgnoreCase));

            if (type == null)
            {
                throw new Exception($"Possible values for {typeof(T).Name}: {String.Join(",", GetAll<T>().Select(l => l.Name))}");
            }

            return type;
        }

        /// <summary>
        /// Compares object id with enum id
        /// </summary>
        /// <param name="other">object to compare</param>
        public int CompareTo(object other) => Id.CompareTo(((Enumeration)other).Id);

        #endregion

        #region Private Methods 

        private static T Parse<T, K>(K value, string description, Func<T, bool> predicate) where T : Enumeration
        {
            var matchingItem = GetAll<T>().FirstOrDefault(predicate);

            if (matchingItem == null)
                throw new InvalidOperationException($"'{value}' is not a valid {description} in {typeof(T)}");

            return matchingItem;
        }

        #endregion        
    }
}
