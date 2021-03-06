﻿using System ;
using System.Collections.Generic ;
using System.IO ;
using System.Linq ;
using System.Text;

namespace SwitchVsVersion
{
    class Disk
    {
        public static IEnumerable<string> GetFiles( string path, string extension )
        {
			foreach(string filename in Directory.GetFiles( path, extension ))
			{
				yield return filename ;
			}

			string[ ] directories = Directory.GetDirectories( path ) ;
            foreach( var file in directories.SelectMany( eachDirectoryPath => GetFiles( eachDirectoryPath, extension ) ) )
            {
            	yield return file ;
            }
        }

    	public static void ModifyFile( string pathToFile, IEnumerable< Mapping > mappings )
    	{
			string allText = null;
    	    Encoding originalEncoding;
            using (var fileStream = File.OpenText(pathToFile))
            {
                originalEncoding = fileStream.CurrentEncoding;
                allText = fileStream.ReadToEnd();
            }

            if ( string.IsNullOrEmpty( allText ) )
    		{
    			return ;
    		}

			if(!anythingNeedsReplacing( allText, mappings ) )
			{
				Console.WriteLine(@"nothing to modify in " + pathToFile);
				return ;
			}

			Console.WriteLine(@"modifying " + pathToFile);

			allText = mappings.Aggregate(
				allText,
				(current, eachMapping) => current.Replace(eachMapping.OldText, eachMapping.NewText));

			File.WriteAllText(pathToFile, allText, originalEncoding);
		}

    	static bool anythingNeedsReplacing( string allText, IEnumerable< Mapping > mappings )
    	{
    		return mappings.Any(m => allText.Contains(m.OldText)) ;
    	}
    }
}