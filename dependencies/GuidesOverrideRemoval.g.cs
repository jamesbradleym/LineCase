using Elements;
using System.Collections.Generic;
using System;
using System.Linq;

namespace LineCase
{
	/// <summary>
	/// Override metadata for GuidesOverrideRemoval
	/// </summary>
	public partial class GuidesOverrideRemoval : IOverride
	{
        public static string Name = "Guides Removal";
        public static string Dependency = null;
        public static string Context = "[*discriminator=Elements.Guide]";
		public static string Paradigm = "Edit";

        /// <summary>
        /// Get the override name for this override.
        /// </summary>
        public string GetName() {
			return Name;
		}

		public object GetIdentity() {

			return Identity;
		}

	}

}