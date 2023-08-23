using Elements;
using System.Collections.Generic;
using System;
using System.Linq;

namespace LineCase
{
	/// <summary>
	/// Override metadata for GuidesOverrideAddition
	/// </summary>
	public partial class GuidesOverrideAddition : IOverride
	{
        public static string Name = "Guides Addition";
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