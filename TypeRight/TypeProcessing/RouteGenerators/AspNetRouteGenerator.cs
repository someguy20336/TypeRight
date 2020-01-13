﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TypeRight.CodeModel;

namespace TypeRight.TypeProcessing.RouteGenerators
{
	/// <summary>
	/// Route generator for ASP net controllers
	/// </summary>
	internal class AspNetRouteGenerator : MvcRouteGenerator
	{
		public AspNetRouteGenerator(MvcControllerInfo controllerInfo) : base(controllerInfo)
		{
		}

		protected override string GetArea()
		{
			FileInfo fileInfo = new FileInfo(Controller.FilePath);
			DirectoryInfo controllerDir = fileInfo.Directory;

			// Check if this controller is in an "Area"
			bool foundAreas = false;
			DirectoryInfo dir = controllerDir;
			DirectoryInfo areaDir = null;  // if areas is found, this is the specific area directory (like "Admin", or "Shared", etc)
			while (dir != null)
			{
				if (dir.Name == "Areas")
				{
					foundAreas = true;
					break;
				}
				areaDir = dir;
				dir = dir.Parent;
			}

			return foundAreas ? areaDir.Name : "";
		}

		protected override string GetBaseRouteTemplate()
		{
			IAttributeData attr = Controller.NamedType.Attributes.FirstOrDefault(a =>
				a.AttributeType.FullName == MvcConstants.RouteAttributeFullName_AspNet
			);

			return attr != null
				? attr.ConstructorArguments[0] as string
				: "";
		}
	}
}
