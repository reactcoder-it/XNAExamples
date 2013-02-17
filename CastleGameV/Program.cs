/*
 * Created by SharpDevelop.
 * User: Vadim Pashaev <vmp@live.ru>
 * Date: 17.02.2013
 * Time: 20:59
 */
using System;
using System.Windows.Forms;

namespace CastleGameV
{
	/// <summary>
	/// Class with program entry point.
	/// </summary>
	class Program
	{
		/// <summary>
		/// Program entry point.
		/// </summary>
		[STAThread]
		static void Main(string[] args) {
			using (var program = new MiniGame()) {
				program.Run();
			}
		}
		
	}
}
