/*
 * Created by SharpDevelop.
 * User: VMP
 * Date: 04.02.2013
 * Time: 17:56
 */
using System;
using XNAExamples;

namespace XNAExamples
{
#if WINDOWS || XBOX
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		static void Main(string[] args)
		{
			using (BouncingBox game = new BouncingBox())
			{
				game.Run();
			}
		}
	}
#endif
}
