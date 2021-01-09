namespace TypeRight.Configuration
{
	/// <summary>
	/// Gets the type of model binding to use for the webservice calls
	/// </summary>
	public enum ModelBindingType
	{
		/// <summary>
		/// Uses an object of parameters for multi-parameter binding.  This doesn't appear to be
		/// well supported anymore, as any post parameters should be coming in as a single model
		/// </summary>
		MultiParam = 0,

		/// <summary>
		/// The Post web service calls take in a single parameter with "FromBody".  This is the preferred style using ASP.NET Core (it seems...)
		/// </summary>
		SingleParam = 1
	}
}
